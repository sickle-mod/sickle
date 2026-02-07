using System;
using TMPro;
using UnityEngine;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x020007F4 RID: 2036
	public class SSOOKPanel : MonoBehaviour
	{
		// Token: 0x06003A3C RID: 14908 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void Awake()
		{
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x001506D8 File Offset: 0x0014E8D8
		public void Show(string title, string message, Action onOkAction, SSOOKPanel.MessageType messageType)
		{
			this._ui.title.text = title;
			this._ui.message.text = message;
			if (this._ui.standardIllustration != null)
			{
				this._ui.standardIllustration.SetActive(messageType == SSOOKPanel.MessageType.Standard);
			}
			if (this._ui.errorIllustration != null)
			{
				this._ui.errorIllustration.SetActive(messageType == SSOOKPanel.MessageType.Error);
			}
			this._onOkAction = onOkAction;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x00029172 File Offset: 0x00027372
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x0004D813 File Offset: 0x0004BA13
		public void OnOkButtonClicked()
		{
			if (this._onOkAction != null)
			{
				this._onOkAction();
			}
			this._onOkAction = null;
		}

		// Token: 0x04002BEF RID: 11247
		[SerializeField]
		private SSOOKPanel.UI _ui;

		// Token: 0x04002BF0 RID: 11248
		private Action _onOkAction;

		// Token: 0x020007F5 RID: 2037
		[Serializable]
		public class UI
		{
			// Token: 0x04002BF1 RID: 11249
			public TextMeshProUGUI title;

			// Token: 0x04002BF2 RID: 11250
			public TextMeshProUGUI message;

			// Token: 0x04002BF3 RID: 11251
			public GameObject standardIllustration;

			// Token: 0x04002BF4 RID: 11252
			public GameObject errorIllustration;
		}

		// Token: 0x020007F6 RID: 2038
		public enum MessageType
		{
			// Token: 0x04002BF6 RID: 11254
			Standard,
			// Token: 0x04002BF7 RID: 11255
			Error
		}
	}
}
