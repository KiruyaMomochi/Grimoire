using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Grimoire.Core.Services
{
    
    public class LineBotOptions
    {
        public const string LineBot = "LineBot";
         
        public string ChannelAccessToken { get; set; }
        public string Secret { get; set; }

        public Uri WebHook { get; set; }
    }
    
    public class LineSignatureService
    {
        public LineSignatureService(IOptions<LineBotOptions> config)
        {
            var secret = Encoding.UTF8.GetBytes(config.Value.Secret);
            _decrypt = new HMACSHA256(secret);
        }

        private readonly HMACSHA256 _decrypt;

        private async Task<byte[]> ValidateSignatureAsync(Stream stream)
        {
            return await _decrypt.ComputeHashAsync(stream);
        }

        private ReadOnlySpan<byte> ValidateSignature(Stream stream)
        {
            return _decrypt.ComputeHash(stream);
        }

        public bool ValidateSignature(Stream stream, ReadOnlySpan<byte> remoteSignature)
        {
            var result = ValidateSignature(stream);
            return remoteSignature.SequenceEqual(result);
        }

        public async Task<bool> ValidateSignatureAsync(Stream stream, byte[] remoteSignature)
        {
            var result = await ValidateSignatureAsync(stream);
            return Grimoire.Utils.Memory.UnsafeCompare(result, remoteSignature);
        }
    }
}
