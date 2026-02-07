using System;
using System.Linq;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200092F RID: 2351
	public class RequestGamesEndpoint : Endpoint<ShowcaseProduct[]>
	{
		// Token: 0x06003F55 RID: 16213 RVA: 0x0015BE48 File Offset: 0x0015A048
		public RequestGamesEndpoint(Channel channel, string lang, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Games";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}", RequestGamesEndpoint._endpoint, channel, lang);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x0015BEAC File Offset: 0x0015A0AC
		public RequestGamesEndpoint(Channel channel, string lang, GameProductTag tag, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Games";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}?tag={3}", new object[]
			{
				RequestGamesEndpoint._endpoint,
				channel,
				lang,
				tag
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x0015BF28 File Offset: 0x0015A128
		protected override void ProcessResponse(Action<ShowcaseProduct[], WebError> onCompletion)
		{
			ShowcaseProduct[] array = JsonUtility.FromJson<ApiShowcaseInterstitialOrGamesResponse>(base._HTTPResponse.DataAsText).data.products.Select((ApiShowcaseProduct x) => new ShowcaseProduct(x)).ToArray<ShowcaseProduct>();
			if (onCompletion != null)
			{
				onCompletion(array, null);
			}
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x0015BF84 File Offset: 0x0015A184
		public static int GetEndpointVersion()
		{
			Match match = Regex.Match(RequestGamesEndpoint._endpoint, "v[0-9]+");
			if (!match.Success)
			{
				return -1;
			}
			return int.Parse(match.Value.Substring(1));
		}

		// Token: 0x0400308F RID: 12431
		private static readonly string _endpoint = "/main/v3/showcase/games";
	}
}
