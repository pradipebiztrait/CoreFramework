using EBiz.CoreFramework.DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Microsoft.Extensions.Options;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
	[ScopedService]
    public class GoogleService : IGoogleService
    {
        private readonly SiteSettings _siteSettings;
        public GoogleService(IOptions<SiteSettings> siteSettings)
        {
            _siteSettings = siteSettings.Value;
        }

        public GoogleUserResponseModel GetUserProfile(string accesstoken)
        {
            string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accesstoken + "";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            var info = JsonConvert.DeserializeObject<GoogleUserResponseModel>(responseFromServer);

            return info;
        }

        public string ReceiveTokenGmail(string code, string GoogleWebAppClientID, string GoogleWebAppClientSecret, string RedirectUrl)
        {
            string postString = "code=" + code + "&client_id=" + _siteSettings.GoogleAuthClientId + @"&client_secret=" + _siteSettings.GoogleAuthClientSecret + "&redirect_uri=" + RedirectUrl;

            string url = "https://accounts.google.com/o/oauth2/token";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.ToString());
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            UTF8Encoding utfenc = new UTF8Encoding();
            byte[] bytes = utfenc.GetBytes(postString);
            Stream os = null;
            try
            {
                request.ContentLength = bytes.Length;
                os = request.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
            }
            catch
            { }
            string result = "";

            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader responseStreamReader = new StreamReader(responseStream);
            result = responseStreamReader.ReadToEnd();

            return result;
        }

        public async Task<string> GetAccessToken()
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = _siteSettings.GoogleAuthClientId,
                ClientSecret = _siteSettings.GoogleAuthClientSecret
            }, new[] { "email", "profile", "https://mail.google.com/" },"user",CancellationToken.None);

            var jwtPayload = GoogleJsonWebSignature.ValidateAsync(credential.Token.IdToken).Result;
            var username = jwtPayload.Email;

            return credential.Token.AccessToken;
        }
    }
}
