// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.IdentityClient.Models
{
    internal partial class ClientList
    {
        internal static ClientList DeserializeClientList(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            IReadOnlyList<Client> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("value"u8))
                {
                    List<Client> array = new List<Client>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(Client.DeserializeClient(item));
                    }
                    value = array;
                    continue;
                }
            }
            return new ClientList(value);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static ClientList FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeClientList(document.RootElement);
        }
    }
}
