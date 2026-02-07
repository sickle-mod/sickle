using System;
using System.Linq;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;
using UnityEngine.Networking;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000922 RID: 2338
	public class SearchByEmailEndpoint : EndpointWithPaginatedResponse<UserSearchResult>
	{
		// Token: 0x06003F32 RID: 16178 RVA: 0x0015B47C File Offset: 0x0015967C
		public SearchByEmailEndpoint(string email, Extras extras = Extras.None, OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Search";
			if (string.IsNullOrEmpty(email) || !EmailFormatValidator.IsValidEmail(email))
			{
				throw new ArgumentException("'email' parameter cannot be null or empty, and it must have a valid email format");
			}
			base._URL = "/main/v1/users?email=" + UnityWebRequest.EscapeURL(email);
			string text = "&extras=avatar,";
			if (extras != Extras.None && extras != Extras.Partners)
			{
				if ((extras & Extras.Boardgames) != Extras.None)
				{
					text += "boardgames,";
				}
				if ((extras & Extras.Onlinegames) != Extras.None)
				{
					text += "onlinegames,";
				}
				if ((extras & Extras.Features) != Extras.None)
				{
					text += "features,";
				}
			}
			text = text.Substring(0, text.Length - 1);
			base._URL += text;
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0015B544 File Offset: 0x00159744
		protected override void ProcessResponse(Action<PaginatedResult<UserSearchResult>, WebError> onCompletion)
		{
			ApiSearchUserResponse apiSearchUserResponse = JsonUtility.FromJson<ApiSearchUserResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<UserSearchResult> paginatedResult = new PaginatedResult<UserSearchResult>(apiSearchUserResponse.data.total, apiSearchUserResponse.data.users.Select((ApiSearchUserResponse.Data.User x) => new UserSearchResult(x)).ToArray<UserSearchResult>(), base._LinkSetter(apiSearchUserResponse.data._links.next), base._LinkSetter(apiSearchUserResponse.data._links.prev), base._LinkSetter(apiSearchUserResponse.data._links.first), base._LinkSetter(apiSearchUserResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(paginatedResult, null);
			}
		}
	}
}
