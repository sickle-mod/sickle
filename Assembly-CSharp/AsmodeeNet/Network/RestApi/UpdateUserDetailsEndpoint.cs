using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000935 RID: 2357
	public class UpdateUserDetailsEndpoint : Endpoint
	{
		// Token: 0x06003F6B RID: 16235 RVA: 0x0015C1A4 File Offset: 0x0015A3A4
		public UpdateUserDetailsEndpoint(User userDetails, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			if (userDetails == null)
			{
				throw new ArgumentException("'userDetails' parameter cannot be null.");
			}
			base.DebugModuleName += ".User";
			base._URL = "/main/v1/user/me";
			base._HttpMethod = HTTPMethods.Put;
			base._Parameters = new Hashtable();
			if (userDetails.LoginName != null)
			{
				base._Parameters.Add("login_name", userDetails.LoginName);
			}
			if (userDetails.Name != null)
			{
				base._Parameters.Add("name", userDetails.Name);
			}
			if (userDetails.Gender != null)
			{
				base._Parameters.Add("gender", userDetails.Gender.Value.ToString().ToUpper());
			}
			if (userDetails.Birthday != null)
			{
				base._Parameters.Add("birthday", userDetails.Birthday.Value.ToString("yyyy-MM-dd"));
			}
			if (userDetails.Zipcode != null)
			{
				base._Parameters.Add("zipcode", userDetails.Zipcode);
			}
			if (userDetails.Country != null)
			{
				base._Parameters.Add("country", userDetails.Country);
			}
			if (userDetails.Language != null)
			{
				base._Parameters.Add("language", userDetails.Language.Value.ToString().ToLower());
			}
			if (userDetails.TimeZone != null)
			{
				base._Parameters.Add("time_zone", userDetails.TimeZone);
			}
			if (userDetails.Avatar != null)
			{
				base._Parameters.Add("avatar", userDetails.Avatar);
			}
			if (userDetails.Coppa != null)
			{
				base._Parameters.Add("coppa", userDetails.Coppa);
			}
			if (userDetails.Newsletter != null)
			{
				base._Parameters.Add("newsletter", userDetails.Newsletter);
			}
		}
	}
}
