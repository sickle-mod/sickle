using System;
using I2.Loc;
using Scythe.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003BE RID: 958
	public class ReconnectPresenter : MonoBehaviour
	{
		// Token: 0x06001BEF RID: 7151 RVA: 0x0003A38D File Offset: 0x0003858D
		public void Activate()
		{
			base.gameObject.SetActive(true);
			this.content.text = ScriptLocalization.Get("GameScene/ReconnectContent");
			this.okButton.gameObject.SetActive(false);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x00029172 File Offset: 0x00027372
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0003A3C1 File Offset: 0x000385C1
		public void ChangeActiveState()
		{
			if (ReconnectManager.IsActive)
			{
				if (!base.gameObject.activeInHierarchy)
				{
					this.Activate();
					return;
				}
			}
			else
			{
				this.Deactivate();
			}
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0003A3E4 File Offset: 0x000385E4
		public void ShowError()
		{
			this.content.text = ScriptLocalization.Get("GameScene/ReconnectError");
			this.okButton.gameObject.SetActive(true);
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0003A40C File Offset: 0x0003860C
		public void OnOkButtonClicked()
		{
			GameController.Instance.ExitGame();
			this.Deactivate();
		}

		// Token: 0x0400140D RID: 5133
		public const string RECONNECT_CONTENT_TERM = "GameScene/ReconnectContent";

		// Token: 0x0400140E RID: 5134
		public const string RECONNECT_ERROR_TERM = "GameScene/ReconnectError";

		// Token: 0x0400140F RID: 5135
		[SerializeField]
		private Text content;

		// Token: 0x04001410 RID: 5136
		[SerializeField]
		private Button okButton;
	}
}
