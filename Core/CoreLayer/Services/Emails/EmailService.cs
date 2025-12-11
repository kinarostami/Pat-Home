using System.Net.Mail;
using DomainLayer.Models;
using DomainLayer.Models.Newsletters;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Users;
using Microsoft.Extensions.Configuration;

namespace CoreLayer.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly IAppContext _app;
        private readonly IConfiguration _configuration;
        public EmailService(IAppContext app, IConfiguration configuration)
        {
            _app = app;
            _configuration = configuration;
        }


        public void SendForgotPassword(User user)
        {

            var fullName = user.Name + " " + user.Family;
            var body = $"<h2 style='text-align:center'> سلام {fullName} </h2><h4> برای تغییر کلمه عبور روی لینک زیر کلیک کنید </h4><a href='{_app.SiteBaseUrl}/Auth/ResetPassword/{user.Email}/{user.ActiveCode}' style='font-size:14px' >تغییر کلمه عبور</a></div>";
            Send(user.Email, "فراموشی کلمه عبور", BuildView(body));
        }

        public void SendActiveCode(User user)
        {
            var body = $"<h2 style='text-align: center;'>فعال سازی حساب کاربری</h2><p>به کارپولخان خوش آمدید. کد فعال سازی شما : <b>{user.SecondActiveCode}</b></p><p>برای فعال سازی روی <a href='{_app.SiteBaseUrl}/Activation-Account/{user.ActiveCode}/{user.PhoneNumber}'>این لینک</a> کلیک کنید</p>";
            Send(user.Email, "فعال سازی حساب کاربری", BuildView(body));
        }

        public void SendContactUsMessage(ContactUs contactUs)
        {
            var body = $"<h2 style='text-align: center;'> سلام {contactUs.FullName} </h2><h3>پاسخ پیام شما با عنوان  :  <b>{contactUs.Subject}</b> </h3><hr style='border-color: #f7060621'><h3 style='margin-top:5px;display:inline-block'>پاسخ :</h3><p style='display:inline'>{contactUs.Answer}</p></div>";
            Send(contactUs.Email, $"پاسخ درخواست پشتیانی", BuildView(body));

        }

        public void SendEmailForAnswerTicket(Ticket ticket)
        {
            var fullName = ticket.User.Name + " " + ticket.User.Family;
            var body = $"<h2 style='text-align: center;'> سلام {fullName} </h2><h3>پاسخ جدیدی در تیکت #{ticket.Id} ثبت شده است.</h3><h3>برای مشاهده روی <a href='{_app.SiteBaseUrl}/UserPanel/Tickets/Show/{ticket.Id}'>این لینک</a> کلیک کنید</h3></div>";
            Send(ticket.User.Email, $"تیکت #{ticket.Id} - {ticket.TicketTitle}", BuildView(body));
        }

        public void SendNewsletterMessage(Newsletter newsletter, List<NewsletterMember> members)
        {
            if (members.Any())
            {
                foreach (var member in members)
                {
                    var body = newsletter.Body +
                               $"<br><p style='margin:0'>اگر تمایل به دریافت ایمیل اطلاعیه های جدید ندارید روی <a href='{_app.SiteBaseUrl}/Newsletter/RemoveMember/{member.MemberCode}/{member.Email}'>این لینک </a>  کلیک کنید</p>";
                    Send(member.Email, newsletter.Subject, BuildView(body));
                }
            }
        }

        public void SendTrackingCode(Order order)
        {
            var fullName = $"{order.Address.Name} {order.Address.Family}";
            var body = $"<h2 style='text-align: center;'> سلام {fullName} </h2><h3>محصولات فاکتور شماره #{order.Id} برای شما ارسال شد. کد رهگیری پست : </h3><h3>{order.TrackingCode}</h3></div>";
            Send(order.User.Email, $"ارسال محصولات فاکتور #{order.Id}", BuildView(body));
        }


        #region Utils
        private void Send(string to, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_app.SiteSettings.EmailSmtpServer);
                mail.From = new MailAddress(_app.SiteSettings.Email, _app.SiteSettings.PersianSitName);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                //System.Net.Mail.Attachment attachment;
                // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
                // mail.Attachments.Add(attachment);

                SmtpServer.Port = _app.SiteSettings.EmailSmtpPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_app.SiteSettings.Email, _app.SiteSettings.EmailPassword);
                SmtpServer.EnableSsl = bool.Parse(_configuration.GetSection("Email")["EnableSsl"]);
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

        }
        private string BuildView(string body)
        {
            var styleBody = @"*{-webkit-font-smoothing:antialiased;}body{Margin:0;padding:0;min-width:100%;-webkit-font-smoothing:antialiased;mso-line-height-rule:exactly;}table{border-spacing:0;color:#333333;}img{border:0;}.wrapper{width:100%;table-layout:fixed;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;}.webkit{max-width:600px;}.outer{Margin:0auto;width:100%;max-width:600px;}
                            .full-width-imageimg{width:100%;max-width:600px;height:auto;}.inner{padding:10px;}p{Margin:0;padding-bottom:10px;}.h1{font-size:21px;font-weight:bold;Margin-top:15px;Margin-bottom:5px;font-family:Arial,sans-serif;-webkit-font-smoothing:antialiased;}
                            .h2{font-size:18px;font-weight:bold;Margin-top:10px;Margin-bottom:5px;-webkit-font-smoothing:antialiased;}.one-column.contents{text-align:left;-webkit-font-smoothing:antialiased;}.one-columnp{font-size:14px;Margin-bottom:10px;-webkit-font-smoothing:antialiased;}.two-column{text-align:center;font-size:0;}.two-column.column{width:100%;max-width:300px;display:inline-block;vertical-align:top;}.contents{width:100%;}.two-column.contents{font-size:14px;text-align:left;}.two-columnimg{width:100%;max-width:280px;height:auto;}
                            .two-column.text{padding-top:10px;}.three-column{text-align:center;font-size:0;padding-top:10px;padding-bottom:10px;}.three-column.column{width:100%;max-width:200px;display:inline-block;vertical-align:top;}.three-column.contents{font-size:14px;text-align:center;}.three-columnimg{width:100%;max-width:180px;height:auto;}.three-column.text{padding-top:10px;}.img-align-verticalimg{display:inline-block;vertical-align:middle;}";

            var tempBody = @$"<center  class='wrapper' style='width:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;'><table class='outer' align='center' cellpadding='0'cellspacing='0' border='0'style='border-spacing:0;Margin:0auto;width:100%;max-width:800px;'><tr><td style='padding-top:0;padding-bottom:0;padding-right:0;padding-left:0;'><table border='0'width='100%'cellpadding='0'cellspacing='0'><tr><td><table style='width:100%;'cellpadding='0'cellspacing='0'border='0'><tbody><tr><td  align='center'><center><table  border='0' align='center' width='100%'cellpadding='0'cellspacing='0' style='Margin:0auto;'><tbody><tr><td class='one-column' style='padding-top:0;padding-bottom:0;padding-right:0;padding-left:0;'>
                            <table border='0'cellpadding='0'cellspacing='0'width='100%' style=' border-spacing:0'><tr><td>&nbsp;</td></tr><tr><td align='center'>&nbsp;</td align=></tr><tr><td height='6'bgcolor='#1f3ca6' class='contents' style='width:100%; border-top-left-radius:10px; border-top-right-radius:10px'></td height=></tr></table></td></tr></tbody></table></center></td></tr></tbody></table></td></tr></table><table class='one-column' border='0'cellpadding='0'cellspacing='0'width='100%' style=' border-spacing:0'bgcolor='#f7f7f7'><tr>
                            <td  style='padding-left:40px;padding-right:40px;padding-top:0px;padding-bottom:40px'><div style='direction:rtl;font-family:Tahoma;font-size:12px'>{body}</div style=></tr></table><table width='100%' border='0'cellspacing='0'cellpadding='0'><tr><td><tablewidth='100%'cellpadding='0'cellspacing='0' border='0'bgcolor='#1f3ca6'><tr><td align='center'bgcolor='#1f3ca6' class='one-column' style='padding-top:4px;padding-bottom:4px;padding-right:10px;padding-left:10px;'><font style='font-size:17px;text-decoration:none;color:#ffffff;font-family:Verdana,Geneva,sans-serif;text-align:center'>
                            <h4 style='margin:0';><a href='{_app.SiteBaseUrl}' target='_blank' style='color:#ffffff;text-decoration:none;'>{_app.SiteSettings.PersianSitName}</a></h4></font></td></tr></table></td></tr><tr><td><table width='100%'cellpadding='0'cellspacing='0' border='0'><tr><td height='6' bgcolor='#1f3ca6' class='contents' style='width:100%; border-bottom-left-radius:10px; border-bottom-right-radius:10px'></td height=></tr><tr><td>&nbsp;</td></tr></table width=></td></tr></table></td></tr></table></center>";

            var emailBody = $"<style type='text/css'>{styleBody}</style>{tempBody}";
            return emailBody;
        }

        #endregion
    }
}