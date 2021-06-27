using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Grimoire.Explore
{
    public class Test : PackageBase
    {
        private readonly ILogger<Test> _logger;

        public Test(ILogger<Test> logger)
        {
            _logger = logger;
        }
        
        [Command("ping", "Ping")]
        public async Task<string> Ping(string rua, int num)
        {
            // Do nothing
            var message = $"Ping. Parameter: {rua} {num}. \" Args: {GrimoireContext.Args}";
            _logger.LogInformation(message);
            return message;
        }

        [Command("Nya")]
        public void Nya()
        {
            Console.WriteLine("Nya");
            // Do nothing
        }
    }
}