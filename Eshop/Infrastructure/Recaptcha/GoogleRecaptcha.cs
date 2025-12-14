using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Eshop.Infrastructure.Recaptcha
{
    public class GoogleRecaptcha : IGoogleRecaptcha
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        public GoogleRecaptcha(IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _configuration = configuration;
            _accessor = accessor;
        }

        public async Task<bool> IsSatisfy()
        {
            var response = _accessor?.HttpContext?.Request.Form["g-recaptcha-response"];
            var secretKey = _configuration.GetSection("Recaptcha")["SecretKey"];
            HttpClient http = new HttpClient();
            var result = await http.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}",null);
            if (result.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<RecaptchaResponse>(await result.Content.ReadAsStringAsync());

                if (responseModel == null)
                    return false;

                return responseModel.Success;
            }

            return false;
        }
    }

    public class RecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTimeOffset ChallengeTs { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }
    }
}