using System;
using GaryPortalAPI.Data;
using GaryPortalAPI.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace GaryPortalAPI.Services
{
    public interface IEmailService
    {
        IRestResponse SendVerificationEmail(User user, string hash);
        IRestResponse SendPassRestEmail(User user, string hash);
    }

    public class EmailService : IEmailService
    {
        private readonly ApiSettings _appSettings;
        public EmailService(IOptions<ApiSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public IRestResponse SendPassRestEmail(User user, string hash)
        {
            RestClient client = new RestClient("https://api.eu.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator("api", _appSettings.MailApi)
            };

            RestRequest request = new RestRequest();
            request.AddParameter("domain", "garyportal.tomk.online", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Gary Portal <no-reply@garyportal.tomk.online>");
            request.AddParameter("to", $"{user.UserFullName} <{user.UserAuthentication.UserEmail}>");
            request.AddParameter("subject", "Gary Portal Password Reset Request");
            request.AddParameter("template", "gp_pass_reset");
            request.AddParameter("v:username", user.UserName); request.AddParameter("v:link", $"{_appSettings.Issuer}/auth/reset?token={hash}");
            request.Method = Method.POST;
            return client.Execute(request);
        }

        public IRestResponse SendVerificationEmail(User user, string hash)
        {
            throw new NotImplementedException();
        }
    }
}
