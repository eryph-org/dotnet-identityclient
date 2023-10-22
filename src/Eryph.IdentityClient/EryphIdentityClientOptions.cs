using System.Runtime.CompilerServices;
using Eryph.IdentityModel.Clients;

namespace Eryph.IdentityClient
{
    public partial class EryphIdentityClientOptions
    {
        public ClientCredentials ClientCredentials { get; }

        public EryphIdentityClientOptions(ClientCredentials clientCredentials, ServiceVersion version = LatestVersion)
            : this(version)
        {
            ClientCredentials = clientCredentials;
        }
    }
}