using System;
using System.Collections;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000883 RID: 2179
	public class RemoveAwardsEndpoint : Endpoint
	{
		// Token: 0x06003D01 RID: 15617 RVA: 0x001574AC File Offset: 0x001556AC
		public RemoveAwardsEndpoint(Award[] achievements, OAuthGate oauthGate = null)
			: base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Award";
			if (achievements == null)
			{
				throw new ArgumentException("The \"achievements\" parameter (Achievements[] achievements) must not be null");
			}
			if (achievements.Length == 0)
			{
				throw new ArgumentException("The \"achievements\" parameter (Achievements[] achievements) must at least contain one item");
			}
			Hashtable[] array = new Hashtable[achievements.Length];
			int num = 0;
			foreach (Award award in achievements)
			{
				if (award.Id != -1)
				{
					array[num] = new Hashtable { { "achievement", award.Id } };
				}
				else
				{
					if (!(award.Tag != string.Empty))
					{
						throw new ArgumentException("An instance of Achievement has neither an Id nor a Tag, and thus can not be used.");
					}
					array[num] = new Hashtable { { "achievement", award.Tag } };
				}
				if (award.TableId != -1)
				{
					array[num].Add("table_id", award.TableId);
				}
				if (award.InfoId != -1)
				{
					array[num].Add("info_id", award.InfoId);
				}
				num++;
			}
			base._Parameters = new Hashtable();
			base._Parameters.Add("achievements", array);
			base._URL = "/main/v1/user/me/awards";
			base._HttpMethod = HTTPMethods.Delete;
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x0004F740 File Offset: 0x0004D940
		protected override ApiResponseError _ParseError()
		{
			return JsonUtility.FromJson<ApiResponseRemoveAwardError>(base._HTTPResponse.DataAsText);
		}
	}
}
