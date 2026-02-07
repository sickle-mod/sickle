using System;
using System.Collections;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200093D RID: 2365
	public class UserSignUpEndpoint : Endpoint<ApiSignUpResponse>
	{
		// Token: 0x06003F78 RID: 16248 RVA: 0x0015C618 File Offset: 0x0015A818
		public UserSignUpEndpoint(string loginName, string password, string email, bool subscribeNewsletter, OAuthGate oauthGate = null)
			: base(false, oauthGate)
		{
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentException("'email' must not be null or empty");
			}
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentException("'password' must not be null or empty");
			}
			base.DebugModuleName += ".User.SignUp";
			base._HttpMethod = HTTPMethods.Post;
			base._URL = "/main/v2/user";
			base._Parameters = new Hashtable
			{
				{ "password", password },
				{ "email", email },
				{ "newsletter", subscribeNewsletter }
			};
			if (!string.IsNullOrEmpty(loginName))
			{
				base._Parameters.Add("login_name", loginName);
			}
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0015C6CC File Offset: 0x0015A8CC
		protected override void ProcessResponse(Action<ApiSignUpResponse, WebError> onCompletion)
		{
			ApiSignUpResponse apiSignUpResponse = JsonUtility.FromJson<ApiSignUpResponse>(base._HTTPResponse.DataAsText);
			if (onCompletion != null)
			{
				onCompletion(apiSignUpResponse, null);
			}
		}

		// Token: 0x04003099 RID: 12441
		public const int kLoginMinimalLength = 4;

		// Token: 0x0400309A RID: 12442
		public const int kPasswordMinimalLength = 1;
	}
}
