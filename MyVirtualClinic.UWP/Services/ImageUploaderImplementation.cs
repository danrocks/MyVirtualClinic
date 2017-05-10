using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVirtualClinic;

using System.IO;
using System.Collections.Specialized;
//using System.Net.Http;
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http.Filters;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(MyVirtualClinic.UWP.ImageUploaderImplementation))]
namespace MyVirtualClinic.UWP { 

    class ImageUploaderImplementation : AbstractImageUploaderImplementation
    {

        protected override async Task HttpPost(string uri, ICollection<DecoratedMediaFile> decoratedMediaFiles)
        {
            using (HttpClient client = await GetHttpClient(true))
            {
                System.Diagnostics.Debug.WriteLine("Start image submission");
                HttpMultipartFormDataContent form = new HttpMultipartFormDataContent();
                // Create a test image            
                form.Add(new HttpStringContent(Guid.NewGuid().ToString()), "Upload"); //unique identifier for this clinic submission
                form.Add(new HttpStringContent("555"), "PersonId"); // todo should  be replaced by the fuller person construct
                form.Add(new HttpStringContent(DateTime.Now.ToString("dd/MM/yyyy")), "AuditWhen"); //time of submission
                form.Add(new HttpStringContent("8c59449b-3e3c-4267-8f2c-c490cd710e1a"), "ApplicationUserId"); //Id of halthcare professional using th system.
                form.Add(new HttpStringContent((string)Application.Current.Properties["personJson"]), "PersonModel");//identifer of the healthcare professional using the device

                List<string> b64s = new List<string>();
                foreach (var dmf in decoratedMediaFiles) {
                    b64s.Add(dmf.Base64String);
                }


                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes((JsonConvert.SerializeObject(b64s)))))
                {
                    var t = new HttpStreamContent(ms.AsRandomAccessStream());
                    form.Add(t, "ImageModels");

                    try
                    {
                        var result = await client.PostAsync(new Uri(_webServer + "Image/Create"), form);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Form submission problem:" + ex.Message);
                        throw;
                    }
                }
                System.Diagnostics.Debug.WriteLine("Image submited ");
                return;
            }
        }
    
        private async Task<HttpClient> GetHttpClient(bool login)
        {
            var filter1 = new HttpBaseProtocolFilter();
#if DEBUG
            if (_webServer.Contains("localhost")) { 
                filter1.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            }
#endif
            
            var client = new HttpClient(filter1);
    
    
            if (login)
            {
                try
                {
                    HttpResponseMessage respMessage = await ReplicateCallToLoginPage(client);
                    HttpResponseMessage loginResp = await SubmitLoginDetails(client, respMessage);
                    HandleResult(loginResp);
                    return client;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                    throw e;
                }
            }
            return client;
        }
    
        private async Task<HttpResponseMessage> SubmitLoginDetails(HttpClient client, HttpResponseMessage r)
        {
            string CookieRequestVerificationToken = GetCookieVal(r);
            string HiddenRequestVerificationToken = GetHiddenVal(r).Result;
    
            System.Diagnostics.Debug.WriteLine("Attempt to login httpclient");
            var userPass = new Dictionary<string, string> { { "Email", _user }, { "Password", _password }, { "__RequestVerificationToken", HiddenRequestVerificationToken }, { "returnUrl", _webServer } };
    
            HttpResponseMessage resp = await client.PostAsync(new Uri(_webServer + "Account/Login"),
                   new HttpFormUrlEncodedContent(userPass));
    
            return resp;
        }
    
        private string GetCookieVal(HttpResponseMessage respMessage)
        {
            //IEnumerable<string> ss = respMessage.Headers.GetValues("Set-Cookie");

            var myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookieCollection = myFilter.CookieManager.GetCookies(new Uri(_webServer));

            string ss="";
            foreach (var key in cookieCollection) {
                System.Diagnostics.Debug.WriteLine(key.Name + ": " + key.Value);
                ss = key.Value;
            }
            //todo assertion may fail because the asp auth cookie from a previous upload
            // is still hanging about...what to do in this cicumstance?
            System.Diagnostics.Debug.Assert(cookieCollection.Count() == 1, "Unexpected Cookies in Http header");
            return ss;

        }
    
        private async Task<string> GetHiddenVal(HttpResponseMessage respMessage)
        {
            string text = await respMessage.Content.ReadAsStringAsync();
    
            Regex regex = new Regex(@"(<\s*input\s*name\s*=\s*""__RequestVerificationToken""\s*type\s*=\s*""hidden""\s*value\s*=\s*"")([-\w]*)(""\s*/>)");
    
            if (regex.IsMatch(text))
            {
                System.Diagnostics.Debug.WriteLine("REGEX MATCH");
                foreach (Match match in regex.Matches(text))
                {
                    // todo error handlin if not found
                    System.Diagnostics.Debug.WriteLine(match.Groups[2].Value);
                    return match.Groups[2].Value;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("NO MATCH");
            }
    
            System.Diagnostics.Debug.WriteLine(text.Substring(1600, 300));
    
            return "";
        }
    
        private async Task<HttpResponseMessage> ReplicateCallToLoginPage(HttpClient client)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ReplicateCallToLoginPage");
                return await client.GetAsync(new Uri(_webServer + "Account/Login?returnUrl=Index"), HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ReplicateCallToLoginPage error: " + ex.Message);
                throw ex;
            }
        }
    
        private void HandleResult(HttpResponseMessage r)
        {
            System.Diagnostics.Debug.WriteLine("Response received");
            try
            {
                r.EnsureSuccessStatusCode();
                System.Diagnostics.Debug.WriteLine("Log in successful");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Log in failed: " + ex.Message);
            }
        }
    }
}
