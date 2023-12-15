using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Marketplace.Models.LiqPaySDK;

public class Notify
{
	[JsonPropertyName("data")]
	[JsonProperty("data")]
	public string Data{ get; set; }

	[JsonPropertyName("signature")]
	[JsonProperty("signature")]
	public string Signature { get; set; }
}
