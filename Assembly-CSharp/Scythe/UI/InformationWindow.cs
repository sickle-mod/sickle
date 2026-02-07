using System;
using TMPro;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x0200038F RID: 911
	public class InformationWindow : MonoBehaviour
	{
		// Token: 0x06001B3B RID: 6971 RVA: 0x000ABFEC File Offset: 0x000AA1EC
		public static string GetSpriteString(TextIcons spriteType)
		{
			string text = "<sprite=";
			int num = (int)spriteType;
			return text + num.ToString() + ">";
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x00039BE4 File Offset: 0x00037DE4
		public void ShowPanel(string message)
		{
			base.gameObject.SetActive(true);
			this.text.SetText(message, true);
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00029172 File Offset: 0x00027372
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00039BFF File Offset: 0x00037DFF
		protected virtual void Clear()
		{
			this.text.SetText("", true);
		}

		// Token: 0x04001330 RID: 4912
		public TextMeshProUGUI text;
	}
}
