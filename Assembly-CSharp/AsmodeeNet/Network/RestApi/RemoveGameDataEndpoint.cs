using System;
using System.Text;
using BestHTTP;
using MiniJSON;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000910 RID: 2320
	public class RemoveGameDataEndpoint : Endpoint
	{
		// Token: 0x06003EFC RID: 16124 RVA: 0x0015A910 File Offset: 0x00158B10
		public RemoveGameDataEndpoint(string game, string[] keys, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (keys == null || keys.Length == 0)
			{
				throw new ArgumentException("'keys' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/data/{0}", game);
			base._HttpMethod = HTTPMethods.Delete;
			this._keys = keys;
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0015A980 File Offset: 0x00158B80
		protected override void _SetRequestParameters()
		{
			string text = Json.Serialize(this._keys);
			base._HTTPRequest.RawData = Encoding.UTF8.GetBytes(text);
		}

		// Token: 0x04003060 RID: 12384
		public const int kMaxKeyLength = 32;

		// Token: 0x04003061 RID: 12385
		public const int kMaxValueLength = 2048;

		// Token: 0x04003062 RID: 12386
		private string[] _keys;
	}
}
