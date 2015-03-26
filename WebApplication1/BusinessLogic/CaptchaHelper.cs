using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;

namespace WebApplication1.BusinessLogic
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

    public class CaptchaHelper
    {
        public int CheckRecaptcha()
        {
            const string CAPTCHA_VALID = "Valid";
            var response = HttpContext.Current.Request["g-recaptcha-response"];
            const string secret = "6LczHgQTAAAAADTTQLxiA7NtG_O1xLSZOGhG-ybu";

            var client = new WebClient();
            var reply = client.DownloadString(

            string.Format(
               "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
            string captchaMsg = "";
            int result = 0; 

            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0)
                {
                    captchaMsg = CAPTCHA_VALID;
                    return result; 
                }

                result = -1; 
                var error = captchaResponse.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        captchaMsg = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        captchaMsg = "The secret parameter is invalid or malformed.";
                        break;
                    case ("missing-input-response"):
                        captchaMsg = "The response parameter is missing.";
                        break;
                    case ("invalid-input-response"):
                        captchaMsg = "The response parameter is invalid or malformed.";
                        break;
                    default:
                        captchaMsg = "Error occured. Please try again";
                        break;
                }
            }
            else
            {
                captchaMsg = "Valid";
            }

            // return captchaMsg;
            return result; 
        }
    }
}