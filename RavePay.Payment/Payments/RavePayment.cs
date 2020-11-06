using RavePay.Payment.Helpers;
using RavePay.Payment.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RavePay.Payment.Payments
{
    public class RavePayment
    {
        private readonly string _secretKey;

        public RavePayment(string secretKey)
        {
            this._secretKey = secretKey;
        }

        /// <summary>
        /// Initiate the payment by calling Flutterwave internal API with the collected payment details
        /// </summary>
        /// <param name="tx_ref">Your transaction reference. This MUST be unique for every transaction</param>
        /// <param name="amount">Amount to charge the customer.</param>
        /// <param name="redirect_url">URL to redirect to when a transaction is completed</param>
        /// <param name="customerEmail">Customer's email address</param>
        /// <param name="customerPhone">Customer's phone number</param>
        /// <param name="customerName">Customer's name</param>
        /// <param name="siteTile">Title to diplay on the flutterwave payment modal</param>
        /// <param name="siteDescription">Description to diplay on the flutterwave payment modal</param>
        /// <param name="siteLogoUrl">Logo to diplay on the flutterwave payment modal</param>
        /// <param name="payment_options">This specifies the payment options to be displayed e.g - card, mobilemoney, ussd and so on.</param>
        /// <param name="currency">Currency to charge in. Defaults to NGN</param>
        /// <returns>Payment link</returns>
        public async Task<TransactionInitResponseDTO> InitializeTransaction(string tx_ref, string amount,
                                        string redirect_url, string customerEmail, string customerPhone,
                                        string customerName, string payment_options = "card", string currency = "NGN")
        {

            //Build the input model from the paramters           

            var model = new TransactionInitInputDTO()
            {
                amount = amount,
                currency = currency,
                tx_ref = tx_ref,
                customer = new Customer()
                {
                    email = customerEmail,
                    name = customerName,
                    phonenumber = customerPhone
                },
                redirect_url = redirect_url,
                payment_options = payment_options,
                customizations = new Customizations()
                {
                    title = RaveConstant.SITE_TITLE,
                    description = RaveConstant.SITE_DESCRIPTION,
                    logo = RaveConstant.COY_LOGO_URL
                }
            };

            //create httpclient here

            var _client = HttpFactory.InitHttpClient(RaveConstant.BASE_URL)
                      .AddAuthorizationHeader(RaveConstant.AUTHORIZATION_TYPE, _secretKey)
                      .AddMediaType(RaveConstant.REQUEST_MEDIA_TYPE)
                      .AddHeader("cache-control", "no-cache");

            //Build the request body as json
            var jsonObj = JsonSerializer.Serialize(model);

            var content = new StringContent(jsonObj, Encoding.UTF8, RaveConstant.REQUEST_MEDIA_TYPE);

            content.Headers.ContentType = new MediaTypeHeaderValue(RaveConstant.REQUEST_MEDIA_TYPE);


            //send the request
            var response = await _client.PostAsync("payments", content);

            var json = await response.Content.ReadAsStringAsync();

            //Deserialize and send the response
            return JsonSerializer.Deserialize<TransactionInitResponseDTO>(json);

        }



        /// <summary>
        /// Use to verify payment transaction
        /// </summary>
        /// <param name="transaction_id">The Id of the transaction to verify</param>
        /// <returns></returns>
        public async Task<TransactionVerifyResponseDTO> VerifyTransaction(int transaction_id)
        {
            var _client = HttpFactory.InitHttpClient(RaveConstant.BASE_URL)
                     .AddAuthorizationHeader(RaveConstant.AUTHORIZATION_TYPE, _secretKey)
                     .AddMediaType(RaveConstant.REQUEST_MEDIA_TYPE)
                     .AddHeader("cache-control", "no-cache");

            //Send the request
            var response = await _client.GetAsync($"transactions/{transaction_id}/verify");

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TransactionVerifyResponseDTO>(json);
        }
    
    
    }
}
