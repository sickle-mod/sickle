using System;
using TMPro;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004F5 RID: 1269
	public class DebugConsoleLog : MonoBehaviour
	{
		// Token: 0x060028C8 RID: 10440 RVA: 0x000EB6C8 File Offset: 0x000E98C8
		public void SetValues(string condition, string stackTrace, LogType type)
		{
			this.conditionText.text = "<color=\"green\">></color> " + condition;
			this.stackTraceText.text = stackTrace;
			switch (type)
			{
			case LogType.Error:
			case LogType.Exception:
				this.conditionText.color = Color.red;
				return;
			case LogType.Assert:
			case LogType.Warning:
				this.conditionText.color = Color.yellow;
				return;
			case LogType.Log:
				this.conditionText.color = Color.white;
				return;
			default:
				this.conditionText.color = Color.white;
				return;
			}
		}

		// Token: 0x04001D3E RID: 7486
		[SerializeField]
		private TextMeshProUGUI conditionText;

		// Token: 0x04001D3F RID: 7487
		[SerializeField]
		private TextMeshProUGUI stackTraceText;
	}
}
