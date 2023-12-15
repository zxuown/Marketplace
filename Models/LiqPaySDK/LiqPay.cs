using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;

namespace Marketplace.Models.LiqPaySDK;

public class LiqPay
{
	private string _privateKey;
	private string _publicKey;
	public string PrivateKey()
	{
        string file = @"D:\PrivateKey.txt";
		string str = "";
        if (File.Exists(file))
        {
             str = File.ReadAllText(file);
        }
		return str;
    }
    public string PublicKey()
    {
        string file = @"D:\PublicKey.txt";
        string str = "";
        if (File.Exists(file))
        {
            str = File.ReadAllText(file);
        }
        return str;
    }
    public LiqPay()
	{
		_publicKey = "sandbox_i70188001890";
		_privateKey = "sandbox_KV5BFtVHNcYivfSbUuTwE3RyLcsED22CMnqVeBcp";
	}



	private string Sign(string data)
	{
		return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(data)));
	}


	public Params PayParams(decimal amount, string description, string orderId)
	{
		return new Params
		{
			Version = 3,
			PublicKey = _publicKey,
			Action = "pay",
			Amount = amount,
			Currency = "USD",
			Description = description,
			OrderId = orderId,
			Language = "uk",
		};
	}

	public string GetData(Params param)
	{
		var json = JsonConvert.SerializeObject(param);
		var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
		return data;
	}

	public bool ValidateData(string data, string signature)
	{
		return Sign(_privateKey + data + _privateKey) == signature;
	}
	
	public string GetSignature(Params param)
	{
		var signature = Sign(_privateKey + GetData(param) + _privateKey);
		return signature;
	}
}
