using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;

namespace HappyMeal_v3.Services
{
    public class Mail
    {
        private readonly string account;
        private readonly string password;
        private ImapClient imap;
        private SmtpClient smtp;
        public IMailFolder Inbox;

        public Mail()
        {
            account = ConfigurationService.GetValue("sendFrom") ?? string.Empty;
            password = ConfigurationService.GetValue("sendPassword") ?? string.Empty;
        }

        public string GetAttachment(int index)
        {
            if (index >= Inbox.Count)
                return string.Empty;

            var fromWebConfig = ConfigurationService.GetValue("searchFrom");
            var pathWebConfig = ConfigurationService.GetValue("pathTemp");
            if (string.IsNullOrEmpty(fromWebConfig))
                return string.Empty;

            var message = Inbox.GetMessage(index);
            if (message.From.Mailboxes.First().Address.Equals(fromWebConfig))
            {
                if (message.Attachments.Any())
                {
                    var attachment = message.Attachments.First();
                    var path = pathWebConfig + @"\" + attachment.ContentDisposition.FileName;

                    if (File.Exists(path))
                        File.Delete(path);

                    using (var stream = File.Create(path))
                    {
                        if (!(attachment is MessagePart))
                            ((MimePart)attachment).ContentObject.DecodeTo(stream);
                    }

                    return attachment.ContentDisposition.FileName;
                }
            }
            return string.Empty;
        }

        public void ImapConnect()
        {
            imap = new ImapClient();
            imap.ServerCertificateValidationCallback = (s, c, h, e) => true;
            imap.Connect("imap.gmail.com", 993, true);
            imap.AuthenticationMechanisms.Remove("XOAUTH2");
            imap.Authenticate(account, password);

            Inbox = imap.Inbox;
            Inbox.Open(FolderAccess.ReadOnly);
        }

        private void SmtpConnect()
        {
            smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, false);
            smtp.Authenticate(account, password);
        }

        public void SendMessage(string email, string firstName, string lastName, string path)
        {
            SmtpConnect();

            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Traiteur Trsb", account));
            msg.To.Add(new MailboxAddress(ConfigurationService.GetValue("sendName"), ConfigurationService.GetValue("sendTo")));
            msg.Cc.Add(new MailboxAddress($"{firstName} {lastName}", email));
            msg.Subject = $"Commande {firstName} {lastName}";

            var body = new TextPart("plain")
            {
                Text = $"Bonjour,\nCommande {firstName} {lastName} - TRSb.\nCordialement."
            };
            var version = ConfigurationService.GetValue("version");
            if (!string.IsNullOrEmpty(version)) body.Text += $"\n\nHappyMeal - v{version}";

            using (var file = File.OpenRead(path))
            {
                var attachment = new MimePart("image", "gif")
                {
                    ContentObject = new ContentObject(file),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(path)
                };

                var multipart = new Multipart("mixed") { body, attachment };

                msg.Body = multipart;
                smtp.Send(msg);
            }
            File.Delete(path);
            SmtpClose();
        }

        private void SmtpClose()
        {
            smtp.Disconnect(true);
        }

        public void ImapClose()
        {
            imap.Disconnect(true);
        }
    }
}