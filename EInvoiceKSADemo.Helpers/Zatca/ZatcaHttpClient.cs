using Application.Models.Zatca;
using Domain.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca
{
    public class ZatcaHttpClient
    {
        public static string LastErrorMessage { get; private set; }

        public static async Task<TResult> PostAsync<TResult, TInput>(string url, TInput model, IDictionary<string, string> headers, bool requireAuth = false, bool patchHttpMethod = false) where TResult : class
        {
            LastErrorMessage = null;
            HttpResponseMessage response = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SharedData.APIUrl);
                    client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en");

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    if (requireAuth)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Basic",
                            parameter: Convert.ToBase64String(Encoding.ASCII.GetBytes($"{SharedData.UserName}:{SharedData.Secret}")));
                    }
                    var req = new HttpRequestMessage(HttpMethod.Post, url);
                    req.Content = JsonContent.Create(model);
                    if (patchHttpMethod)
                    {
                        req.Method = new HttpMethod("Patch");
                        response = await client.SendAsync(req);
                    }
                    else
                    {
                        response = await client.PostAsJsonAsync(url, model);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError
                        || response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                        || (model is InputComplianceModel && response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        || (model is InputCSIDModel && response.StatusCode == System.Net.HttpStatusCode.BadRequest))
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        LastErrorMessage = $"Response: {(int)response.StatusCode}-{response.ReasonPhrase} , Message : {JsonSerializer.Serialize(errorResponse)}";
                        Helper.ZatcaHttpClientMessage = LastErrorMessage;
                        
                        return null;
                    }

                    var result = await response.Content.ReadFromJsonAsync<TResult>();

                    var reportingResult = result as InvoiceModelResult;
                    if (reportingResult != null)
                    {
                        if (reportingResult.ValidationResults.ErrorMessages.Count > 0 || reportingResult.ValidationResults.WarningMessages.Count > 0)
                        {
                            LastErrorMessage = JsonSerializer.Serialize(reportingResult);
                            Helper.ZatcaHttpClientMessage = LastErrorMessage;
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                LastErrorMessage = $"Response Code :  {(int)response?.StatusCode} : {response?.ReasonPhrase} , Exception Message : {ex.Message}{ex.InnerException?.Message}";
                Helper.ZatcaHttpClientMessage = LastErrorMessage;

                return null;
            }
        }
    }
}
