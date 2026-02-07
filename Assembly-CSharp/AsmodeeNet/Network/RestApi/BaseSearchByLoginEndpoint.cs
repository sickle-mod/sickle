using System;
using System.Linq;
using BestHTTP;
using I2.Loc;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000926 RID: 2342
	public abstract class BaseSearchByLoginEndpoint : EndpointWithPaginatedResponse<UserSearchResult>
	{
		// Token: 0x06003F3D RID: 16189 RVA: 0x0015B804 File Offset: 0x00159A04
		public BaseSearchByLoginEndpoint(OAuthGate oauthGate = null)
			: base(oauthGate, false)
		{
			BaseSearchByLoginEndpoint.kNoTokenException = ScriptLocalization.Get("MainMenu/NoTokenException");
			BaseSearchByLoginEndpoint.kEmptyLoginArrayException = ScriptLocalization.Get("MainMenu/EmptyLoginArrayException");
			BaseSearchByLoginEndpoint.kNullOrEmptyLoginException = ScriptLocalization.Get("MainMenu/NullOrEmptyLoginException");
			BaseSearchByLoginEndpoint.kLoginToShortException = ScriptLocalization.Get("MainMenu/LoginToShortException");
			BaseSearchByLoginEndpoint.kForbiddenCharacterException = ScriptLocalization.Get("MainMenu/ForbiddenCharacterException");
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x0015B864 File Offset: 0x00159A64
		protected void CtorCore(Extras extras, int offset, int limit)
		{
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
			if (offset > 0)
			{
				base._URL = base._URL + "&offset=" + offset.ToString();
			}
			if (limit > 0)
			{
				base._URL = base._URL + "&limit=" + limit.ToString();
			}
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0015B91C File Offset: 0x00159B1C
		protected override void ProcessResponse(Action<PaginatedResult<UserSearchResult>, WebError> onCompletion)
		{
			ApiSearchUserResponse apiSearchUserResponse = JsonUtility.FromJson<ApiSearchUserResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<UserSearchResult> paginatedResult = new PaginatedResult<UserSearchResult>(apiSearchUserResponse.data.total, apiSearchUserResponse.data.users.Select((ApiSearchUserResponse.Data.User x) => new UserSearchResult(x)).ToArray<UserSearchResult>(), base._LinkSetter(apiSearchUserResponse.data._links.next), base._LinkSetter(apiSearchUserResponse.data._links.prev), base._LinkSetter(apiSearchUserResponse.data._links.first), base._LinkSetter(apiSearchUserResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(paginatedResult, null);
			}
		}

		// Token: 0x04003079 RID: 12409
		public const string kForbiddenChars = "()#|@^*%§!?:;.,$~";

		// Token: 0x0400307A RID: 12410
		public static string kNoTokenException = "A public token is needed to call this endpoint.";

		// Token: 0x0400307B RID: 12411
		public static string kEmptyLoginArrayException = "Logins array cannot be null and must at least contain one item.";

		// Token: 0x0400307C RID: 12412
		public static string kNullOrEmptyLoginException = "A login cannot be null or empty.";

		// Token: 0x0400307D RID: 12413
		public static string kLoginToShortException = "A login must be {0} characters long minimum.";

		// Token: 0x0400307E RID: 12414
		public static string kForbiddenCharacterException = "A login cannot contain item from the following set : ";
	}
}
