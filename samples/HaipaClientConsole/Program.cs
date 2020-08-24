using System;
using System.Threading.Tasks;
using Haipa.ClientRuntime.Authentication;
using Haipa.ClientRuntime.OData;
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

            var createdClient = await identityClient.Clients.CreateAsync(new Client{Name = "info@haipa.io"});
            Console.WriteLine(createdClient.Key);

            var results =
                await identityClient.Clients.ListAsync(new ODataQuery<Client>(x => x.Name == "info@haipa.io"));
            foreach (var result in results.Value)
            {
                Console.WriteLine($"{result.Id} - {result.Name}");
            }

            Console.ReadLine();
        }
    }
}
