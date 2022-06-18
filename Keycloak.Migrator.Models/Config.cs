using Newtonsoft.Json;
using System;
using System.Linq;

namespace Keycloak.Migrator.Models
{
    /// <summary>
    /// Configurations for claim mapping configuration a protocol.
    /// </summary>
    public class Config
    {
        [JsonProperty("aggregate.attrs")]
        public string? AggregateAttrs { get; set; }

        [JsonProperty("multivalued")]
        public string? MultiValued { get; set; }

        [JsonProperty("userinfo.token.claim")]
        public string? UserInfoTokenClaim { get; set; }

        [JsonProperty("user.attribute")]
        public string? UserAttribute { get; set; }

        [JsonProperty("id.token.claim")]
        public string? IdTokenClaim { get; set; }

        [JsonProperty("access.token.claim")]
        public string? AccessTokenClaim { get; set; }

        [JsonProperty("claim.name")]
        public string? ClaimName { get; set; }

        [JsonProperty("jsonType.label")]
        public string? JsonTypeLabel { get; set; }

        [JsonProperty("usermodel.clientRoleMapping.clientId")]
        public string? UserModelClientRoleMappingClientId { get; set; }

        [JsonProperty("full.path")]
        public string? FullPath { get; set; }
    }
}
