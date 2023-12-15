using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Marketplace.Models.LiqPaySDK;

public class PayResponse
{
	[JsonPropertyName("payment_id")]
	[JsonProperty("payment_id")]
	public long PaymentId { get; set; }

	[JsonPropertyName("action")]
	[JsonProperty("action")]
	public string Action { get; set; }

	[JsonPropertyName("status")]
	[JsonProperty("status")]
	public string Status { get; set; }

	[JsonPropertyName("version")]
	[JsonProperty("version")]
	public int Version { get; set; }

	[JsonPropertyName("type")]
	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonPropertyName("paytype")]
	[JsonProperty("paytype")]
	public string PayType { get; set; }

	[JsonPropertyName("public_key")]
	[JsonProperty("public_key")]
	public string PublicKey { get; set; }

	[JsonPropertyName("acq_id")]
	[JsonProperty("acq_id")]
	public long AcqId { get; set; }

	[JsonPropertyName("order_id")]
	[JsonProperty("order_id")]
	public string OrderId { get; set; }

	[JsonPropertyName("liqpay_order_id")]
	[JsonProperty("liqpay_order_id")]
	public string LiqPayOrderId { get; set; }

	[JsonPropertyName("description")]
	[JsonProperty("description")]
	public string Description { get; set; }

	[JsonPropertyName("sender_card_mask2")]
	[JsonProperty("sender_card_mask2")]
	public string SenderCardMask { get; set; }

	[JsonPropertyName("sender_card_bank")]
	[JsonProperty("sender_card_bank")]
	public string SenderCardBank { get; set; }

	[JsonPropertyName("sender_card_type")]
	[JsonProperty("sender_card_type")]
	public string SenderCardType { get; set; }

	[JsonPropertyName("sender_card_country")]
	[JsonProperty("sender_card_country")]
	public string SenderCardCountry { get; set; }

	[JsonPropertyName("ip")]
	[JsonProperty("ip")]
	public string Ip { get; set; }

	[JsonPropertyName("amount")]
	[JsonProperty("amount")]
	public decimal Amount { get; set; }

	[JsonPropertyName("currency")]
	[JsonProperty("currency")]
	public string Currency { get; set; }

	[JsonPropertyName("transaction_id")]
	[JsonProperty("transaction_id")]
	public long TransactionId { get; set; }

	//TODO
	//{,"sender_commission":0.0,"receiver_commission":11.02,"agent_commission":0.0,"amount_debit":27527.73,"amount_credit":27527.73,"commission_debit":0.0,"commission_credit":412.92,"currency_debit":"UAH","currency_credit":"UAH","sender_bonus":0.0,"amount_bonus":0.0,"mpi_eci":"7","is_3ds":false,"language":"uk","create_date":1696178876800,"end_date":1696178876948}
}
