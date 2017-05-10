using System;
using System.Collections.Generic;
using System.Text;
using MyVirtualClinic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(MyVirtualClinic.iOS.ImageUploaderImplementation))]
namespace MyVirtualClinic.iOS
{

    class ImageUploaderImplementation : AbstractImageUploaderImplementation
    {


        private async Task<HttpResponseMessage> DoGet(HttpClient client)
        {
            System.Diagnostics.Debug.WriteLine("Do Get");
            HttpResponseMessage r = client.GetAsync("http://bbc.co.uk").Result;
            System.Diagnostics.Debug.WriteLine("Done Get");
            System.Diagnostics.Debug.WriteLine(".......");

            return r;
        }

        protected override async Task HttpPost(string uri, ICollection<DecoratedMediaFile> decoratedMediaFiles)
        {
            using (HttpClient client = GetHttpClient(true).Result)
            {
                System.Diagnostics.Debug.WriteLine("Start image submission");
                MultipartFormDataContent form = new MultipartFormDataContent();

                form.Add(new StringContent(Guid.NewGuid().ToString()), "Upload"); //unique identifier for this clinic submission
                form.Add(new StringContent("555"), "PersonId"); // todo should  be replaced by the fuller person construct
                form.Add(new StringContent(DateTime.Now.ToString("dd/MM/yyyy")), "AuditWhen"); //time of submission
                form.Add(new StringContent("8c59449b-3e3c-4267-8f2c-c490cd710e1a"), "ApplicationUserId"); //Id of halthcare professional using th system.
                form.Add(new StringContent((string)Xamarin.Forms.Application.Current.Properties["personJson"]), "PersonModel");//identifer of the healthcare professional using the device

                List<string> b64s = new List<string>();
                foreach (var dmf in decoratedMediaFiles)
                {
                    b64s.Add(dmf.Base64String);
                }


                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes((JsonConvert.SerializeObject(b64s)))))
                {
                    var t = new StreamContent(ms, 4096);
                    t.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "ImageModels" };
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
                System.Diagnostics.Debug.WriteLine("Image submited ");
                return;
            }
        }
    
            private async Task<HttpClient> GetHttpClient(bool login)
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

                HttpResponseMessage resp = await client.PostAsync("Account/Login",
                       new FormUrlEncodedContent(userPass));

                return resp;
            }

            private string GetCookieVal(HttpResponseMessage respMessage)
            {
                IEnumerable<string> ss = respMessage.Headers.GetValues("Set-Cookie");
                System.Diagnostics.Debug.WriteLine("Cookies...");
               // System.Diagnostics.Debug.Assert(ss.Count() == 1, "Unexpected Cookies in Http header");

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
                    return await client.GetAsync("Account/Login?returnUrl=Index", HttpCompletionOption.ResponseContentRead);
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




