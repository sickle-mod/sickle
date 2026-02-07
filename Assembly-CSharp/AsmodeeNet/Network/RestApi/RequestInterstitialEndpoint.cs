using System;
using System.Linq;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000931 RID: 2353
	public class RequestInterstitialEndpoint : Endpoint<ShowcaseProduct[]>
	{
		// Token: 0x06003F5D RID: 16221 RVA: 0x0015BFBC File Offset: 0x0015A1BC
		public RequestInterstitialEndpoint(Channel channel, string lang, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Interstitial";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}", RequestInterstitialEndpoint._endpoint, channel, lang);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x0015C020 File Offset: 0x0015A220
		protected override void ProcessResponse(Action<ShowcaseProduct[], WebError> onCompletion)
		{
			ShowcaseProduct[] array = JsonUtility.FromJson<ApiShowcaseInterstitialOrGamesResponse>(base._HTTPResponse.DataAsText).data.products.Select((ApiShowcaseProduct x) => new ShowcaseProduct(x)).ToArray<ShowcaseProduct>();
			if (onCompletion != null)
			{
				onCompletion(array, null);
			}
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x0015C07C File Offset: 0x0015A27C
		public static int GetEndpointVersion()
		{
			Match match = Regex.Match(RequestInterstitialEndpoint._endpoint, "v[0-9]+");
			if (!match.Success)
			{
				return -1;
			}
			return int.Parse(match.Value.Substring(1));
		}

		// Token: 0x04003092 RID: 12434
		private static readonly string _endpoint = "/main/v3/showcase/interstitial";
	}
}
