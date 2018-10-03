﻿using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using WasteProducts.Logic.Common.Services.Donations;
using WasteProducts.Logic.Constants.Donations;

namespace WasteProducts.Logic.Services.Donations
{
    /// <inheritdoc />
    class PayPalVerificationService : IVerificationService
    {
        private readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;

        /// <summary>
        /// Verifies that the request comes from PayPal.
        /// </summary>
        /// <param name="payPalRequestString">PayPal request string.</param>
        public bool IsVerified(string payPalRequestString)
        {
            const string VERIFIED = "VERIFIED";

            HttpWebRequest verificationRequest = PrepareVerificationRequest(payPalRequestString);

            // Send the request to PayPal and get the response
            string verificationResponse = null;
            using (var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream()))
                verificationResponse = streamIn.ReadToEnd();
            return verificationResponse == VERIFIED;
        }

        /// <summary>
        /// Prepares a verification request.
        /// </summary>
        /// <param name="payPalRequestString">PayPal request string.</param>
        private HttpWebRequest PrepareVerificationRequest(string payPalRequestString)
        {            
            const string VERIFICATION_PREFIX = "cmd=_notify-validate&";
            const string POST = "POST";
            const string CONTENT_TYPE = "application/x-www-form-urlencoded";

            HttpWebRequest verificationRequest =
                (HttpWebRequest)WebRequest.Create(_appSettings[AppSettings.PAYPAL_URL]);

            // Set values for the verification request
            verificationRequest.Method = POST;
            verificationRequest.ContentType = CONTENT_TYPE;

            // Add cmd=_notify-validate to the payload
            string verificationString = VERIFICATION_PREFIX + payPalRequestString;
            verificationRequest.ContentLength = verificationString.Length;

            // Attach payload to the verification request
            using (var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.ASCII))
                streamOut.Write(verificationString);

            return verificationRequest;
        }
    }
}