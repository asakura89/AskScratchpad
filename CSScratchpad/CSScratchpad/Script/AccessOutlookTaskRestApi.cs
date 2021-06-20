using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Scratch;

namespace CSScratchpad.Script {
    class AccessOutlookTaskRestApi : Common, IRunnable {
        /*

        NOTE:
            https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_user
            https://docs.microsoft.com/en-us/previous-versions/office/office-365-api/api/version-2.0/use-outlook-rest-api
            https://docs.microsoft.com/en-us/previous-versions/office/office-365-api/api/version-2.0/task-rest-operations
            https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/resources/outlooktask
            https://docs.microsoft.com/en-us/previous-versions/office/office-365-api/how-to/http-response-status-codes
    
            https://stackoverflow.com/questions/41119193/refresh-auth-token-for-ms-graph-with-c-sharp
            https://docs.microsoft.com/en-us/azure/architecture/multitenant-identity/token-cache
    
            https://developer.microsoft.com/en-us/graph/docs/concepts/query_parameters
            https://developer.microsoft.com/en-us/graph/graph-explorer?request=me/contacts?$count=true&method=GET&version=v1.0
    
        */

        public void Run() {
            String[] scopes = new[] { "tasks.read", "tasks.read.shared" };
            AuthenticationResult authResult = null;

            try {
                /*
                authResult = ClientApp.Users.Any() ?
                    await ClientApp.AcquireTokenSilentAsync(scopes, ClientApp.Users.FirstOrDefault()) :
                    await ClientApp.AcquireTokenAsync(scopes);
                    */
                if (authResult != null) {
                    /*
                    DisplayBasicTokenInfo(authResult);

                    authResult.Dump();
                    Client.Dump();

                    System.Net.Http.HttpResponseMessage res;

                    res = GetHttpContentWithToken(System.Net.Http.HttpMethod.Get, "https://graph.microsoft.com/v1.0/me", authResult).Result.Dump();
                    res.Content.ReadAsStringAsync().Result.Dump();

                    res = GetHttpContentWithToken(System.Net.Http.HttpMethod.Get, "https://outlook.office.com/api/v2.0/me/tasks", authResult).Result.Dump();
                    res.Content.ReadAsStringAsync().Result.Dump();

                    res = GetHttpContentWithToken(System.Net.Http.HttpMethod.Get, "https://graph.microsoft.com/beta/me/outlook/tasks", authResult).Result.Dump();
                    res.Content.ReadAsStringAsync().Result.Dump();

                    res = GetHttpContentWithToken(System.Net.Http.HttpMethod.Get, "https://graph.microsoft.com/beta/users/ditaadisubrata%40outlook.com/outlook/tasks", authResult).Result.Dump();
                    res.Content.ReadAsStringAsync().Result.Dump();

                    res = GetHttpContentWithToken(System.Net.Http.HttpMethod.Get, "https://graph.microsoft.com/beta/me/outlook/taskFolders", authResult).Result.Dump();
                    res.Content.ReadAsStringAsync().Result.Dump();

                    res = GetHttpContentWithToken(System.Net.Http.HttpMethod.Get, "https://graph.microsoft.com/beta/me/outlook/taskGroups", authResult).Result.Dump();
                    res.Content.ReadAsStringAsync().Result.Dump();
                    */

                    /*
                    String resultjson = GetHttpContentWithToken(HttpMethod.Get, "https://graph.microsoft.com/beta/me/outlook/taskFolders?$count=true", authResult)
                        .Result
                        .Content
                        .ReadAsStringAsync()
                        .Result;

                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(resultjson);
                    dynamic pageInfo = new ExpandoObject();
                    pageInfo.Current = 1;
                    pageInfo.Size = result.value.Count;
                    pageInfo.DataCount = Convert.ToInt32((result as IDictionary<String, Object>)["@odata.count"] ?? 0);
                    pageInfo.PageCount = Convert.ToInt32(pageInfo.DataCount / pageInfo.Size) + (pageInfo.DataCount % pageInfo.Size > 0 ? pageInfo.DataCount % pageInfo.Size : 0);
                    */

                    /*
                    (result as ExpandoObject).Dump();
                    (pageInfo as ExpandoObject).Dump();
                    */

                    /*
                    IList<TestNvy.NameValueItem> folders = (result.value as List<dynamic>)
                        .AsNameValueList(dyn => dyn.name, dyn => dyn.id)
                        .ToList();

                    for (; pageInfo.Current <= pageInfo.PageCount; pageInfo.Current++) {
                        dynamic res = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(
                            GetHttpContentWithToken(HttpMethod.Get, $"https://graph.microsoft.com/beta/me/outlook/taskFolders?$count=true&$skip={pageInfo.Current * pageInfo.Size}", authResult)
                                .Result
                                .Content
                                .ReadAsStringAsync()
                                .Result);

                        folders = folders.Concat((res.value as List<dynamic>).AsNameValueList(dyn => dyn.name, dyn => dyn.id)).ToList();
                        System.Threading.Thread.Sleep(1000);
                    }

                    folders.Dump();

                    var tasks = new Dictionary<String, dynamic>();
                    foreach (TestNvy.NameValueItem folder in folders) {
                        dynamic res = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(
                            GetHttpContentWithToken(HttpMethod.Get, $"https://graph.microsoft.com/beta/me/outlook/taskFolders/{folder.Value}/tasks?$count=true", authResult)
                                .Result
                                .Content
                                .ReadAsStringAsync()
                                .Result)
                            .Dump();
                        System.Threading.Thread.Sleep(1000);
                    }
                    */
                }
            }
            catch (MsalUiRequiredException msaluiex) {
                /*Dbg($"MsalUiRequiredException: {msaluiex.Message}");

                try {
                    authResult = await ClientApp.AcquireTokenAsync(new[] { "tasks.read", "tasks.read.shared" });
                }
                catch (MsalException msalex) {
                    Dbg($"Error Acquiring Token:{Environment.NewLine}{msalex}");
                }*/
            }
            catch (Exception ex) {
                /*Dbg($"Error Acquiring Token Silently:{Environment.NewLine}{ex}");
                return;*/
            }
        }

        static readonly String ClientAppId = "b597aa3c-62c3-4b86-84f2-5b1c1f910f65";
        //static PublicClientApplication Client = new PublicClientApplication(ClientId);
        static IPublicClientApplication ClientApp = PublicClientApplicationBuilder.Create(ClientAppId).Build();
        const String GraphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        async Task<HttpResponseMessage> GetHttpContentWithToken(HttpMethod method, String url, AuthenticationResult auth/*string token*/) {
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            try {
                var request = new HttpRequestMessage(method, url);
                //Add the token in Authorization header
                //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("Authorization", auth.CreateAuthorizationHeader());
                response = await httpClient.SendAsync(request);
                /*var content = await response.Content.ReadAsStringAsync();
                return content;*/

                return response;
            }
            catch (Exception ex) {
                //return ex.ToString();
                Dbg($"Error get content: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Sign out the current user
        /// </summary>
        void SignOut() {
            IEnumerable<IAccount> users = ClientApp.GetAccountsAsync().Result;
            if (users.Any()) {
                try {
                    ClientApp.RemoveAsync(users.FirstOrDefault()).Wait();
                    Dbg("User has signed-out");
                }
                catch (MsalException ex) {
                    Dbg($"Error signing-out user: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Display basic information contained in the token
        /// </summary>
        void DisplayBasicTokenInfo(AuthenticationResult authResult) {
            if (authResult != null) {
                /*Dbg($"Name: {authResult.User.Name}" + Environment.NewLine);
                Dbg($"Username: {authResult.User.DisplayableId}" + Environment.NewLine);
                Dbg($"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine);
                Dbg($"Access Token: {authResult.AccessToken}" + Environment.NewLine);
                Dbg($"Scopes: {String.Join(", ", authResult.Scopes)}" + Environment.NewLine);
                Dbg($"Authorization Header: {authResult.CreateAuthorizationHeader()}" + Environment.NewLine);*/
            }
        }
    }
}
