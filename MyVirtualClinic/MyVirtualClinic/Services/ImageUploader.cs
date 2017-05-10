using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using ModernHttpClient;
//using System.Windows.Web.Http;


namespace MyVirtualClinic.Services
{
    /// <summary>
    /// This is tool for uploading pictures taken on a smart phone to the MyVirtualClinic database.
    /// The images can then be viewd on the web via the MyVirtualClinic website.
    /// </summary>
    class ImageUploader
    {

        private string _webServer;
        private string _user;
        private string _password;

        public ImageUploader(string b64, string webServer, string user, string password)
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
            return ;
            //return  DoGet(new HttpClient()).Result.Content.ToString();        
        }

        private  async Task<HttpResponseMessage> DoGet(HttpClient client)
        {
            string text = "html not set";
            System.Diagnostics.Debug.WriteLine("Do Get");
            HttpResponseMessage r =  client.GetAsync("http://bbc.co.uk").Result;
            System.Diagnostics.Debug.WriteLine("Done Get");
            System.Diagnostics.Debug.WriteLine(".......");
            
            return r;
        }

        private async Task  HttpPost(string uri, string b64)
        {
            using (HttpClient client = GetHttpClient(true).Result)
            {
                System.Diagnostics.Debug.WriteLine("Start image submission");
                MultipartFormDataContent form = new MultipartFormDataContent();
                // Create a test image            
                form.Add(new StringContent("17ad0657-7330-4762-9b7d-3bdc0ac9f454"), "Upload");
                form.Add(new StringContent("555"), "PersonId");
                form.Add(new StringContent("12/12/2016"), "AuditWhen");
                form.Add(new StringContent("8c59449b-3e3c-4267-8f2c-c490cd710e1a"), "ApplicationUserId");
                
                 using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(b64)))
                {
                    var t = new StreamContent(ms, 4096);
                    t.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "Img" };
                    form.Add(t);
           
                try
                {
                    var result = await client.PostAsync("Image/Create", form);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Form submission problem:" + ex.Message);
                    throw;
                }
                }
                System.Diagnostics.Debug.WriteLine("Image submited " );
                return;
            }            
        }

        private async  Task<HttpClient> GetHttpClient(bool login)
        {
            var client = new HttpClient();
        
            client.BaseAddress = new Uri(_webServer);
            client.Timeout = new TimeSpan(300000000);

            if (login)
            {
                try
                {
                    HttpResponseMessage respMessage = await ReplicateCallToLoginPage(client);
                    HttpResponseMessage loginResp = await SubmitLoginDetails(client, respMessage);
                    HandleResult(loginResp);
                    return client;
                }
                catch (Exception e )
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                    throw e;
                }
            }
            return client;
        }

        private async Task<HttpResponseMessage> SubmitLoginDetails(HttpClient client, HttpResponseMessage r) {
            string CookieRequestVerificationToken = GetCookieVal(r);
            string HiddenRequestVerificationToken = GetHiddenVal(r).Result;

            System.Diagnostics.Debug.WriteLine("Attempt to login httpclient");
            var userPass = new Dictionary<string, string> { { "Email", _user }, { "Password", _password }, { "__RequestVerificationToken", HiddenRequestVerificationToken }, { "returnUrl", webServer } };

            HttpResponseMessage resp = await client.PostAsync("Account/Login",
                   new FormUrlEncodedContent(userPass));
                   
            return resp;            
        }

        private  string GetCookieVal(HttpResponseMessage respMessage)
        {
            IEnumerable<string> ss = respMessage.Headers.GetValues("Set-Cookie");
            System.Diagnostics.Debug.WriteLine("Cookies...");
            System.Diagnostics.Debug.Assert(ss.Count() == 1, "Unexpected Cookies in Http header");

            string cookies = ss.First();
            Regex regex = new Regex(@"(__RequestVerificationToken=)([-\w]*)(;)");

            if (regex.IsMatch(cookies))
            {
                System.Diagnostics.Debug.WriteLine("REGEX MATCH");
                foreach (Match match in regex.Matches(cookies))
                {
                    // todo error handlin if not foundp3rfection
                    System.Diagnostics.Debug.WriteLine(match.Groups[2].Value);
                    return match.Groups[2].Value;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("NO MATCH");
            }

            return "";
        }

        private  async Task<string> GetHiddenVal(HttpResponseMessage respMessage)
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
                return await client.GetAsync(_webServer + "Account/Login?returnUrl=Index", HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ReplicateCallToLoginPage error: " + ex.Message);
                throw ex;
            }            
        }

        private  void HandleResult(HttpResponseMessage r)
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

