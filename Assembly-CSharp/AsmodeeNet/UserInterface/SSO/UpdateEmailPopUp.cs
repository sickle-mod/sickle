using System;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x020007F9 RID: 2041
	public class UpdateEmailPopUp : MonoBehaviour
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06003A4D RID: 14925 RVA: 0x0004D8DB File Offset: 0x0004BADB
		// (set) Token: 0x06003A4E RID: 14926 RVA: 0x0004D8ED File Offset: 0x0004BAED
		public string Email
		{
			get
			{
				return this._ui.emailInputField.text;
			}
			set
			{
				this._ui.emailInputField.text = value;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x0004D900 File Offset: 0x0004BB00
		public bool AreRequirementsMet
		{
			get
			{
				return EmailFormatValidator.IsValidEmail(this.Email);
			}
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x0004D90D File Offset: 0x0004BB0D
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.emailInputField);
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x0004D91F File Offset: 0x0004BB1F
		public void Init(Action<bool> onComplete)
		{
			this._onComplete = onComplete;
			this.OnEmailValueChanged();
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x0004D92E File Offset: 0x0004BB2E
		public void OnEmailValueChanged()
		{
			this._ui.submitButton.interactable = this.AreRequirementsMet;
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x0004D946 File Offset: 0x0004BB46
		public void OnEmailEndEdit()
		{
			this.OnYesButtonClicked();
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x0015076C File Offset: 0x0014E96C
		public void OnYesButtonClicked()
		{
			if (!this.AreRequirementsMet)
			{
				return;
			}
			ActivityIndicatorButton activityIndicatorButton = this._ui.submitButton.GetComponent<ActivityIndicatorButton>();
			if (activityIndicatorButton != null)
			{
				activityIndicatorButton.Waiting = true;
			}
			this._endpoint = new UpdateEmailAndNewsletterEndpoint(this._ui.emailInputField.text, this._ui.subscribeToNewsletterButton.IsOn, null);
			this._endpoint.Execute(delegate(WebError error)
			{
				if (activityIndicatorButton != null)
				{
					activityIndicatorButton.Waiting = false;
				}
				if (this._onComplete != null)
				{
					this._onComplete(error == null);
				}
			});
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x0004D94E File Offset: 0x0004BB4E
		public void OnNoButtonClicked()
		{
			if (this._endpoint != null)
			{
				this._endpoint.Abort();
				this._endpoint = null;
			}
			if (this._onComplete != null)
			{
				this._onComplete(false);
			}
		}

		// Token: 0x04002BFB RID: 11259
		private const string _kModuleName = "UpdateEmailPopUp";

		// Token: 0x04002BFC RID: 11260
		[SerializeField]
		private UpdateEmailPopUp.UI _ui;

		// Token: 0x04002BFD RID: 11261
		private Action<bool> _onComplete;

		// Token: 0x04002BFE RID: 11262
		private UpdateEmailAndNewsletterEndpoint _endpoint;

		// Token: 0x020007FA RID: 2042
		[Serializable]
		public class UI
		{
			// Token: 0x04002BFF RID: 11263
			public TMP_InputField emailInputField;

			// Token: 0x04002C00 RID: 11264
			public ToggleButton subscribeToNewsletterButton;

			// Token: 0x04002C01 RID: 11265
			public Button submitButton;
		}
	}
}
