using System;
using I2.Loc;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020003BD RID: 957
	public class TooltipInfo : MonoBehaviour, ITooltipInfo
	{
		// Token: 0x06001BEC RID: 7148 RVA: 0x000AF80C File Offset: 0x000ADA0C
		string ITooltipInfo.InfoBasic()
		{
			if (this.customPath)
			{
				return ScriptLocalization.Get(this.InfoBasic).TrimEnd(new char[] { ' ', ':' });
			}
			return ScriptLocalization.Get("Tooltips/" + this.InfoBasic).TrimEnd(new char[] { ' ', ':' });
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0003A362 File Offset: 0x00038562
		string ITooltipInfo.InfoAdv()
		{
			if (this.customPath)
			{
				return ScriptLocalization.Get(this.InfoExtended);
			}
			return ScriptLocalization.Get("Tooltips/" + this.InfoExtended);
		}

		// Token: 0x04001409 RID: 5129
		public string InfoBasic;

		// Token: 0x0400140A RID: 5130
		public string InfoExtended;

		// Token: 0x0400140B RID: 5131
		public bool block;

		// Token: 0x0400140C RID: 5132
		public bool customPath;
	}
}
