using System;
using System.Collections;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200093A RID: 2362
	public class LinkUnlinkMultipleEndpoint : Endpoint
	{
		// Token: 0x06003F70 RID: 16240 RVA: 0x0015C470 File Offset: 0x0015A670
		public LinkUnlinkMultipleEndpoint(PartnerAccount[] add, PartnerAccount[] remove, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			if ((add == null || add.Length == 0) && (remove == null || remove.Length == 0))
			{
				throw new ArgumentException("'add' and 'remove' arrays cannot both be null or empty");
			}
			base.DebugModuleName += ".User.LinkUnlinkMultiple";
			base._HttpMethod = HTTPMethods.Put;
			this._usePutAsPatch = true;
			base._URL = "/main/v1/user/me/link";
			base._Parameters = new Hashtable();
			Hashtable[] array;
			if (add != null)
			{
				array = add.Select((PartnerAccount x) => new Hashtable
				{
					{ "partner", x.PartnerId },
					{ "partner_user", x.PartnerUser },
					{ "partner_extra[token]", x.Token },
					{ "partner_extra[app_id]", x.App_Id }
				}).ToArray<Hashtable>();
			}
			else
			{
				array = new Hashtable[0];
			}
			Hashtable[] array2 = array;
			Hashtable[] array3;
			if (remove != null)
			{
				array3 = remove.Select((PartnerAccount x) => new Hashtable
				{
					{ "partner", x.PartnerId },
					{ "partner_user", x.PartnerUser }
				}).ToArray<Hashtable>();
			}
			else
			{
				array3 = new Hashtable[0];
			}
			Hashtable[] array4 = array3;
			base._Parameters.Add("add", array2);
			base._Parameters.Add("remove", array4);
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x00050B13 File Offset: 0x0004ED13
		protected override ApiResponseError _ParseError()
		{
			return JsonUtility.FromJson<ApiResponseLinkUnlinkMultipleError>(base._HTTPResponse.DataAsText);
		}
	}
}
