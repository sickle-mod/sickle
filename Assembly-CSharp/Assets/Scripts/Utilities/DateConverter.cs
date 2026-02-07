using System;
using I2.Loc;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
	// Token: 0x020001A9 RID: 425
	public class DateConverter
	{
		// Token: 0x06000C70 RID: 3184 RVA: 0x00080D74 File Offset: 0x0007EF74
		public static string SecondsToMMHHmmssFormat(int total)
		{
			int num = Mathf.FloorToInt((float)(total / 86400));
			total -= num * 60 * 60 * 24;
			int num2 = Mathf.FloorToInt((float)(total / 3600));
			total -= num2 * 60 * 60;
			int num3 = Mathf.FloorToInt((float)(total / 60));
			total -= num3 * 60;
			string text = string.Empty;
			if (num > 0)
			{
				text = string.Format("{0} {1}", num + 1, ScriptLocalization.Get("Lobby/Days"));
			}
			else if (num2 > 0)
			{
				text = string.Format("{0} {1}", num2 + 1, ScriptLocalization.Get("Lobby/Hours"));
			}
			else if (num3 > 0)
			{
				text = string.Format("{0} {1}", num3 + 1, ScriptLocalization.Get("Lobby/MinutesAbbreviation"));
			}
			else
			{
				text = string.Format("<1 {0}", ScriptLocalization.Get("Lobby/MinutesAbbreviation"));
			}
			return text;
		}
	}
}
