using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Grimoire.Web.Services
{
    public class LineSignatureService
    {
        public LineSignatureService(IOptions<LineBotOptions> config)
        {
            var secret = Convert.FromBase64String(config.Value.Secret);
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
            Console.WriteLine(BitConverter.ToString(result.ToArray()));
            Console.WriteLine(BitConverter.ToString(remoteSignature.ToArray()));
            return remoteSignature.SequenceEqual(result);
        }

        public async Task<bool> ValidateSignatureAsync(Stream stream, byte[] remoteSignature)
        {
            var result = await ValidateSignatureAsync(stream);
            Console.WriteLine(BitConverter.ToString(result));
            Console.WriteLine(BitConverter.ToString(remoteSignature));
            return remoteSignature.SequenceEqual(result);
        }
    }
}