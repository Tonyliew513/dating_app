using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModel.SerilogModels
{
    public class AppSettings_Service
    {

        public Logging? Logging { get; set; }
        public FTPServer? FTPServer { get; set; }
        public WebServer? WebServer { get; set; }
        public string? AllowedHosts { get; set; }
        public string? Token { get; set; }
        public GoogleReCaptcha? GoogleReCaptcha { get; set; }
        public string? DefaultRole { get; set; }
        public EmailSettings? EmailSettings { get; set; }
        public PermitPayment? PermitPayment { get; set; }
        public string? EnvironmentName { get; set; }
        public JwtConfig? JwtConfig { get; set; }
        public KestrelWebServer? KestrelWebServer { get; set; }
    }

    public class LogLevel
    {
        public string? Default { get; set; }
        public string? System { get; set; }
        public string? Microsoft { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public LogLevel? LogLevel { get; set; }

    }

    public class FTPServer
    {
        public string? UploadPhotoPath { get; set; }
    }

    public class WebServer
    {
        public string? HostAddress { get; set; }
        public string? ApiServer { get; set; }

        public string? OdataApiServer { get; set; }
        public string? SubDomain { get; set; }
    }

    public class GoogleReCaptcha
    {
        public string? ClientKey { get; set; }
        public string? SecretKey { get; set; }
    }

    public class EmailSettings
    {
        public string? Mail { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
    }
    public class PermitPayment
    {
        public ProceedPageInfo? ProceedPageInfo { get; set; }
        public PayNowPageInfo? PayNowPageInfo { get; set; }
        public ConfirmPaymentPageInfo? ConfirmPaymentPageInfo { get; set; }
    }
    public class ProceedPageInfo
    {
        public string? Description { get; set; }
        public string? Quantity { get; set; }
        public string? Amount { get; set; }
    }
    public class PayNowPageInfo
    {
        public string? Description { get; set; }
        public string? Quantity { get; set; }
        public string? Amount { get; set; }
    }
    public class ConfirmPaymentPageInfo
    {
        public string? Description { get; set; }
        public string? Quantity { get; set; }
        public string? Amount { get; set; }
    }

    public class JwtConfig
    {
        public string? SecretKey { get; set; }
        public string? TokenLifetime { get; set; }
        public string? TokenExtension { get; set; }
        public string? TokenRenewalTime { get; set; }
    }
    public class KestrelWebServer
    {
        public Endpoints? Endpoints { get; set; }
    }

    public class Endpoints
    {
        public Http? Http { get; set; }
        public Https? Https { get; set; }
    }

    public class Http
    {
        public string? IPAddress { get; set; }
        public string? Port { get; set; }
    }

    public class Https
    {
        public string? IPAddress { get; set; }
        public string? Port { get; set; }
    }
}
