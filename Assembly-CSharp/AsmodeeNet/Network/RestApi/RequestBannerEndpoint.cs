using System;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200092D RID: 2349
	public class RequestBannerEndpoint : Endpoint<ShowcaseProduct>
	{
		// Token: 0x06003F51 RID: 16209 RVA: 0x0015BD74 File Offset: 0x00159F74
		public RequestBannerEndpoint(Channel channel, string lang, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Banner";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}", RequestBannerEndpoint._endpoint, channel, lang);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x0015BDD8 File Offset: 0x00159FD8
		protected override void ProcessResponse(Action<ShowcaseProduct, WebError> onCompletion)
		{
			ShowcaseProduct showcaseProduct = new ShowcaseProduct(JsonUtility.FromJson<ApiShowcaseBannerResponse>(base._HTTPResponse.DataAsText).data.product);
			if (onCompletion != null)
			{
				onCompletion(showcaseProduct, null);
			}
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x0015BE10 File Offset: 0x0015A010
		public static int GetEndpointVersion()
		{
			Match match = Regex.Match(RequestBannerEndpoint._endpoint, "v[0-9]+");
			if (!match.Success)
			{
				return -1;
			}
			return int.Parse(match.Value.Substring(1));
		}

		// Token: 0x04003089 RID: 12425
		private static readonly string _endpoint = "/main/v3/showcase/banner";
	}
}
