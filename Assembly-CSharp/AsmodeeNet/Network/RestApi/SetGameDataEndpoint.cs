using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BestHTTP;
using MiniJSON;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000911 RID: 2321
	public class SetGameDataEndpoint : Endpoint<Dictionary<string, string>>
	{
		// Token: 0x06003EFE RID: 16126 RVA: 0x0015A9B0 File Offset: 0x00158BB0
		public SetGameDataEndpoint(string game, Dictionary<string, string> keyAndValue, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (keyAndValue == null || keyAndValue.Count == 0)
			{
				throw new ArgumentException("'keyAndValue' argument cannot be null or empty");
			}
			if (keyAndValue.Any((KeyValuePair<string, string> x) => x.Key.Length > 32 || x.Value.Length > 2048))
			{
				throw new ArgumentException("'keyAndValue' argument cannot contain a key whose length is longer than 32 bytes or a value whose length is longer than 2048 bytes");
			}
			base._URL = string.Format("/main/v1/user/me/data/{0}", game);
			base._HttpMethod = HTTPMethods.Post;
			base._Parameters = new Hashtable(keyAndValue);
			this._game = game;
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x0015AA64 File Offset: 0x00158C64
		protected override void ProcessResponse(Action<Dictionary<string, string>, WebError> onCompletion)
		{
			Dictionary<string, string> dictionary = (((Json.Deserialize(base._HTTPResponse.DataAsText) as Dictionary<string, object>)["data"] as Dictionary<string, object>)[this._game] as Dictionary<string, object>).ToDictionary((KeyValuePair<string, object> x) => x.Key, (KeyValuePair<string, object> x) => x.Value as string);
			if (onCompletion != null)
			{
				onCompletion(dictionary, null);
			}
		}

		// Token: 0x04003063 RID: 12387
		public const int kMaxKeyLength = 32;

		// Token: 0x04003064 RID: 12388
		public const int kMaxValueLength = 2048;

		// Token: 0x04003065 RID: 12389
		private string _game;
	}
}
