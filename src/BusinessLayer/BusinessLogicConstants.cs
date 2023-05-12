namespace BusinessLayer
{

    public static class BusinessLogicConstants
    {
        public const string SmtpGmail = "smtp.gmail.com";
        public const int SmtpGmailPort = 587;
        public const string SmtpGmailLogin = "segmentationfaultteam@gmail.com";
        public static readonly string SmtpGmailPassword = Environment.GetEnvironmentVariable("stmp_password") ?? String.Empty;
        public const bool SslEnable = true;
        public const string MailAuthor = "Segmentation Fault Team";
    }
}
