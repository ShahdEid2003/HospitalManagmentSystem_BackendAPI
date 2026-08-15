using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.BillRepositories;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.BillServices
{
    public class BillService:IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StripeSettings _stripeSettings;

        public BillService(
            IBillRepository billRepository,
            IHttpContextAccessor httpContextAccessor,
            IOptions<StripeSettings> stripeSettings)
        {
            _billRepository = billRepository;
            _httpContextAccessor = httpContextAccessor;
            _stripeSettings = stripeSettings.Value;
        }

        public async Task<BillResponse> GetBillById(int billId)
        {
            var bill = await _billRepository.GetOne(
                x => x.Id == billId,
                new string[]
                {
            $"{nameof(Bill.Patient)}.{nameof(Patient.User)}"
                });

            if (bill == null)
            {
                throw new Exception("Bill not found.");
            }

            return bill.Adapt<BillResponse>();
        }

        public async Task<List<BillResponse>> GetBills()
        {
            var bills = await _billRepository.GetAll(
                null,
                new string[]
                {
            $"{nameof(Bill.Patient)}.{nameof(Patient.User)}"
                });

            return bills
                .OrderByDescending(x => x.IssuedAt)
                .Adapt<List<BillResponse>>();
        }

        public async Task<List<BillResponse>> GetPatientBills(int patientId)
        {
            var bills = await _billRepository.GetAll(
                x => x.PatientId == patientId,
                new string[]
                {
            $"{nameof(Bill.Patient)}.{nameof(Patient.User)}"
                });

            return bills
                .OrderByDescending(x => x.IssuedAt)
                .Adapt<List<BillResponse>>();
        }

        public async Task<BillResponse> HandleSuccess( string sessionId)
        {
            var bill = await _billRepository.GetOne(
                x => x.StripeSessionId == sessionId,
                new string[]
                {
            $"{nameof(Bill.Patient)}.{nameof(Patient.User)}"
                });

            if (bill == null)
            {
                throw new Exception("Bill not found.");
            }

            bill.PaymentStatus = PaymentStatusEnum.Paid;

            await _billRepository.Update(bill);

            return bill.Adapt<BillResponse>();
        }

        public async Task<BillResponse> ProcessBill(BillRequest request)
        {
            var existingBill = await _billRepository.GetOne(
                x => x.AppointmentId == request.AppointmentId);

            if (existingBill != null)
            {
                throw new Exception(
                    "A bill already exists for this appointment.");
            }

            if (request.Amount <= 0)
            {
                throw new Exception(
                    "Amount must be greater than zero.");
            }

            var bill = request.Adapt<Bill>();

            bill.PaymentStatus =
                PaymentStatusEnum.Pending;

            bill.IssuedAt =
                DateTime.UtcNow;

            await _billRepository.Create(bill);


            // Cash
            if (request.PaymentMethod ==PaymentMethodEnum.Cash)
            {
                bill.PaymentStatus = PaymentStatusEnum.Paid;

                await _billRepository.Update(bill);

                return bill.Adapt<BillResponse>();
            }


            // Visa
            if (request.PaymentMethod ==PaymentMethodEnum.Visa)
            {
                Stripe.StripeConfiguration.ApiKey =_stripeSettings.SecretKey;

                var httpRequest = _httpContextAccessor.HttpContext!.Request;

                var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes =
                        new List<string>
                        {
                    "card"
                        },

                    Mode = "payment",

                    SuccessUrl = $"{baseUrl}/api/Bill/success?sessionId={{CHECKOUT_SESSION_ID}}",

                    CancelUrl =$"{baseUrl}/api/Bill/cancel",

                    LineItems =
                        new List<SessionLineItemOptions>
                        {
                            new SessionLineItemOptions
                            {
                                PriceData =
                                    new SessionLineItemPriceDataOptions
                                    {
                                        Currency = "usd",

                                        ProductData =
                                            new SessionLineItemPriceDataProductDataOptions
                                            {
                                                Name =
                                                    $"Hospital Bill #{bill.Id}"
                                            },

                                        UnitAmount =
                                            (long)(bill.Amount * 100)
                                    },

                                Quantity = 1
                            }
                        }
                };

                var service = new SessionService();

                var session = await service.CreateAsync(options);

                bill.StripeSessionId =session.Id;

                await _billRepository.Update(bill);

                var response = bill.Adapt<BillResponse>();

                response.StripeUrl =session.Url;

                return response;
            }

            throw new Exception("Invalid payment method.");
        }
    }
}
