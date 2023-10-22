// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

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
            Optional<string> count = default;
            Optional<string> nextLink = default;
            Optional<IReadOnlyList<Client>> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("count"u8))
                {
                    count = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("nextLink"u8))
                {
                    nextLink = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("value"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<Client> array = new List<Client>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(Client.DeserializeClient(item));
                    }
                    value = array;
                    continue;
                }
            }
            return new ClientList(count.Value, nextLink.Value, Optional.ToList(value));
        }
    }
}
