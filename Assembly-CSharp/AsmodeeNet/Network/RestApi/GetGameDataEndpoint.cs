using System;
using System.Collections.Generic;
using System.Linq;
using BestHTTP;
using MiniJSON;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200090E RID: 2318
	public class GetGameDataEndpoint : Endpoint<Dictionary<string, string>>
	{
		// Token: 0x06003EF5 RID: 16117 RVA: 0x0015A7BC File Offset: 0x001589BC
		public GetGameDataEndpoint(string game, int userId, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/{0}/data/{1}", userId, game);
			base._HttpMethod = HTTPMethods.Get;
			this._game = game;
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x0015A820 File Offset: 0x00158A20
		public GetGameDataEndpoint(string game, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/data/{0}", game);
			base._HttpMethod = HTTPMethods.Get;
			this._game = game;
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x0015A880 File Offset: 0x00158A80
		protected override void ProcessResponse(Action<Dictionary<string, string>, WebError> onCompletion)
		{
			Dictionary<string, string> dictionary = (((Json.Deserialize(base._HTTPResponse.DataAsText) as Dictionary<string, object>)["data"] as Dictionary<string, object>)[this._game] as Dictionary<string, object>).ToDictionary((KeyValuePair<string, object> x) => x.Key, (KeyValuePair<string, object> x) => x.Value as string);
			if (onCompletion != null)
			{
				onCompletion(dictionary, null);
			}
		}

		// Token: 0x0400305C RID: 12380
		private string _game;
	}
}
