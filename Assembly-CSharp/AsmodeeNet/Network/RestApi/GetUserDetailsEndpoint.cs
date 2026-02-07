using System;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000933 RID: 2355
	public class GetUserDetailsEndpoint : Endpoint<User>
	{
		// Token: 0x06003F64 RID: 16228 RVA: 0x00050A70 File Offset: 0x0004EC70
		public GetUserDetailsEndpoint(int userId, Extras extras = Extras.None, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			base._URL = string.Format("/main/v1/user/{0}", userId);
			this.CtorCore(extras);
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x00050A97 File Offset: 0x0004EC97
		public GetUserDetailsEndpoint(Extras extras = Extras.None, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base._URL = "/main/v1/user/me";
			this.CtorCore(extras);
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x0015C0B4 File Offset: 0x0015A2B4
		private void CtorCore(Extras extras)
		{
			base.DebugModuleName += ".User";
			if (extras != Extras.None)
			{
				string text = "?extras=";
				if ((extras & Extras.Partners) != Extras.None)
				{
					text += "partners,";
				}
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
				text = text.Substring(0, text.Length - 1);
				base._URL += text;
			}
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x0015C150 File Offset: 0x0015A350
		protected override void ProcessResponse(Action<User, WebError> onCompletion)
		{
			Either<User, Builder<User>.BuilderErrors[]> either = new User.Builder(JsonUtility.FromJson<ApiGetUserDetailsResponse>(base._HTTPResponse.DataAsText)).Build(false);
			if (onCompletion != null)
			{
				if (either.Error == null)
				{
					onCompletion(either.Value, null);
					return;
				}
				onCompletion(null, new UserError(either.Error));
			}
		}
	}
}
