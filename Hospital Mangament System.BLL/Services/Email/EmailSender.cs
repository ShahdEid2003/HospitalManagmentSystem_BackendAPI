using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)// smtp.gmail.com = عنوان خادم البريد الخاص بجوجل
            {

                EnableSsl = true, // تفعيل التشفير 
                UseDefaultCredentials = false,//استخدم البيانات التي سأعطيك إياها في Credentials
                Credentials = new NetworkCredential("shahdeid012@gmail.com", "wqfs dkmd xnnf omvi")
            };

            return client.SendMailAsync(
                new MailMessage(from: "shahdeid012@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml = true }
                );
        }
        public async  Task SendDoctorApprovalEmail(string adminEmail, string doctorName, string doctorEmail, string approveUrl)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    "shahdeid012@gmail.com",
                    "wqfs dkmd xnnf omvi")
            };

                string body = $@"
                    <h2>Doctor Registration Request</h2>

                    <p><b>{doctorName}</b> has registered.</p>

                    <p>Email : {doctorEmail}</p>

                    <a href='{approveUrl}'>Approve Doctor</a>";

                await client.SendMailAsync(
                    new MailMessage(
                        "shahdeid012@gmail.com",
                        adminEmail,
                        "Doctor Approval",
                        body)
                    {
                        IsBodyHtml = true
                    });
        }
    }
}
