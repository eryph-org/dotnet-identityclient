using System;
using System.Threading.Tasks;
using Haipa.ClientRuntime.Authentication;
using Haipa.IdentityClient;
using Haipa.IdentityClient.Models;

namespace HaipaClientConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var credentials = await ApplicationTokenProvider.LogonWithHaipaClient(Array.Empty<string>());
            using var identityClient = new HaipaIdentityClient(credentials);

            await identityClient.Clients.CreateAsync(new HaipaClient{Name="test"});

            var results = await identityClient.Clients.ListAsync() as HaipaClientList;
            foreach (var result in results.Value)
            {
                Console.WriteLine($"{result.Id} - {result.Name}");
            }

            Console.ReadLine();
        }
    }
}
