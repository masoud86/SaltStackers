using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SaltStackers.Application.Custom
{
    public class HmacDelegatingHandler : DelegatingHandler
    {
        private readonly string _publicKey;
        private readonly string _privateKay;

        public HmacDelegatingHandler(string publicKey, string privateKey)
        {
            _publicKey = string.IsNullOrEmpty(publicKey) ? "" : publicKey;
            _privateKay = string.IsNullOrEmpty(privateKey) ? "" : privateKey;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_publicKey) || string.IsNullOrEmpty(_privateKay))
            {
                return await base.SendAsync(request, cancellationToken);
            }
            var requestContentBase64String = string.Empty;
            var requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
            var requestHttpMethod = request.Method.Method;
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = DateTime.UtcNow - epochStart;
            var requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
            var nonce = Guid.NewGuid().ToString("N");
            if (request.Content != null)
            {
                var content = await request.Content.ReadAsByteArrayAsync();
                var md5 = MD5.Create();
                var requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }
            var signatureRawData = $"{_publicKey}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}{requestContentBase64String}";
            var secretKeyByteArray = Convert.FromBase64String(_privateKay);
            var signature = Encoding.UTF8.GetBytes(signatureRawData);
            using (var hmac = new HMACSHA256(secretKeyByteArray))
            {
                var signatureBytes = hmac.ComputeHash(signature);
                var requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                request.Headers.Authorization = new AuthenticationHeaderValue("hmacauth", $"{_publicKey}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}");
                
                //TODO: Remove after handle in CONSOL
                request.Headers.Add("hmacauth", $"{_publicKey}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
