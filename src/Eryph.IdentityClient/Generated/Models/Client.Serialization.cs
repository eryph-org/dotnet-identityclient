// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.IdentityClient.Models
{
    public partial class Client
    {
        internal static Client DeserializeClient(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            string name = default;
            IReadOnlyList<string> allowedScopes = default;
            IReadOnlyList<string> roles = default;
            string tenantId = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"u8))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("allowed_scopes"u8))
                {
                    List<string> array = new List<string>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(item.GetString());
                    }
                    allowedScopes = array;
                    continue;
                }
                if (property.NameEquals("roles"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<string> array = new List<string>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(item.GetString());
                    }
                    roles = array;
                    continue;
                }
                if (property.NameEquals("tenant_id"u8))
                {
                    tenantId = property.Value.GetString();
                    continue;
                }
            }
            return new Client(id, name, allowedScopes, roles ?? new ChangeTrackingList<string>(), tenantId);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static Client FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeClient(document.RootElement);
        }
    }
}
