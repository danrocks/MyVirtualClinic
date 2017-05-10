using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Collections.Specialized;
//using System.Net.Http;
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Xamarin.Forms;

using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http.Filters;
using MyVirtualClinic;

[assembly: Xamarin.Forms.Dependency(typeof(MyVirtualClinic.WinPhone.ImageUploaderImplementation))]
namespace MyVirtualClinic.WinPhone
{

    class ImageUploaderImplementation : IImageUploader
    {
        private string _webServer;
        private string _user;
        private string _password;


        public void UploadImage(string b64, string webServer, string user, string password)
        {
            System.Diagnostics.Debug.WriteLine("Test harness for Image Uploading...");
            //HttpPost("Submit/Edit", b64);

            _webServer = webServer;
            _user = user;
            _password = password;

            RunAsync(b64).Wait();
        }


        async Task RunAsync(string b64)
        {
            await HttpPost("Submit/Edit", b64);
            return;
            //return  DoGet(new HttpClient()).Result.Content.ToString();        
        }

        private async Task<HttpResponseMessage> DoGet(HttpClient client)
        {
            System.Diagnostics.Debug.WriteLine("Do Get");
            HttpResponseMessage r = await client.GetAsync(new Uri("http://bbc.co.uk"));
            System.Diagnostics.Debug.WriteLine("Done Get");
            System.Diagnostics.Debug.WriteLine(".......");

            return r;
        }

        private async Task HttpPost(string uri, string b64)
        {
            using (HttpClient client = GetHttpClient(true).Result)
            {
                System.Diagnostics.Debug.WriteLine("Start image submission");
                HttpMultipartFormDataContent form = new HttpMultipartFormDataContent();
                // Create a test image            
                form.Add(new HttpStringContent("17ad0657-7330-4762-9b7d-3bdc0ac9f454"), "Upload");
                form.Add(new HttpStringContent("555"), "PersonId");
                form.Add(new HttpStringContent("12/12/2016"), "AuditWhen");
                form.Add(new HttpStringContent("8c59449b-3e3c-4267-8f2c-c490cd710e1a"), "ApplicationUserId");

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(b64)))
                //using (var ms = new InMemoryRandomAccessStream())
                {
                    //byte[] b = Encoding.UTF8.GetBytes(b64);
                    //await ms.WriteAsync(b);
                    var t = new HttpStreamContent(ms.AsRandomAccessStream());
                    //t.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "Img" };
                    //form.Add(t);
                    form.Add(t, "Img");

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

            string ss = "";
            foreach (var key in cookieCollection)
            {
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

        public void UploadImage(ICollection<DecoratedMediaFile> decoratedMediaFiles, string webServer, string user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
