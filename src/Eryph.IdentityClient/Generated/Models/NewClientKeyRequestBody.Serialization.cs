// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Eryph.IdentityClient.Models
{
    public partial class NewClientKeyRequestBody : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(SharedSecret))
            {
                if (SharedSecret != null)
                {
                    writer.WritePropertyName("sharedSecret"u8);
                    writer.WriteBooleanValue(SharedSecret.Value);
                }
                else
                {
                    writer.WriteNull("sharedSecret");
                }
            }
            writer.WriteEndObject();
        }
    }
}
