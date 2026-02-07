using System;
using I2.Loc;
using Multiplayer.AuthApi;
using Multiplayer.Support;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Scythe.UI
{
	// Token: 0x020004F1 RID: 1265
	public class SupportPanel : MonoBehaviour
	{
		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x0600289B RID: 10395 RVA: 0x000EB04C File Offset: 0x000E924C
		// (remove) Token: 0x0600289C RID: 10396 RVA: 0x000EB084 File Offset: 0x000E9284
		public event global::System.Action OnServerCallWaitPanel;

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x0600289D RID: 10397 RVA: 0x000EB0BC File Offset: 0x000E92BC
		// (remove) Token: 0x0600289E RID: 10398 RVA: 0x000EB0F4 File Offset: 0x000E92F4
		public event global::System.Action OnServerCallWaitPanelHide;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x0600289F RID: 10399 RVA: 0x000EB12C File Offset: 0x000E932C
		// (remove) Token: 0x060028A0 RID: 10400 RVA: 0x000EB164 File Offset: 0x000E9364
		public event global::System.Action OnSupportMailSent;

		// Token: 0x060028A1 RID: 10401 RVA: 0x000EB19C File Offset: 0x000E939C
		private void OnEnable()
		{
			this.sendButton.onClick.AddListener(new UnityAction(this.SendSupportMessage));
			this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
			this.emailInputField.Clear();
			this.messageInputField.Clear();
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x0004245F File Offset: 0x0004065F
		private void OnDisable()
		{
			this.sendButton.onClick.RemoveListener(new UnityAction(this.SendSupportMessage));
			this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x00042499 File Offset: 0x00040699
		private void ExitWindow()
		{
			SingletonMono<MainMenu>.Instance.OnSupportPanelExit();
			base.gameObject.SetActive(false);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000EB1F8 File Offset: 0x000E93F8
		private void SendSupportMessage()
		{
			if (this.messageInputField.text.Length < 5 || this.emailInputField.text.Length < 5)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/FillFields");
				return;
			}
			if (this.emailInputField.text.IndexOf('@') <= 0)
			{
				this.errorText.text = ScriptLocalization.Get("MainMenu/IncorrectEmail");
				return;
			}
			this.errorText.text = "";
			this.SendPlainMail(this.emailInputField.text, this.messageInputField.text);
			global::System.Action onServerCallWaitPanel = this.OnServerCallWaitPanel;
			if (onServerCallWaitPanel == null)
			{
				return;
			}
			onServerCallWaitPanel();
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000424B1 File Offset: 0x000406B1
		private void SendPlainMail(string authorEmail, string messageBody)
		{
			Singleton<SupportController>.Instance.SendSupportTicket(authorEmail, messageBody + this.GetInfo(), new Action<SendSupportTicketResponse>(this.SupportController_OnSendTicketSuccess), new Action<FailureResponse>(this.SupportController_OnSendTicketFailure), new Action<Exception>(this.SupportController_OnSendTicketError));
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000424EE File Offset: 0x000406EE
		private void SupportController_OnSendTicketSuccess(SendSupportTicketResponse completedEventArgs)
		{
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide != null)
			{
				onServerCallWaitPanelHide();
			}
			global::System.Action onSupportMailSent = this.OnSupportMailSent;
			if (onSupportMailSent == null)
			{
				return;
			}
			onSupportMailSent();
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x00042511 File Offset: 0x00040711
		private void SupportController_OnSendTicketFailure(FailureResponse response)
		{
			this.ShowInfo(ScriptLocalization.Get("ErrorMessages/GenericError"));
			Debug.LogError(response.Error);
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide == null)
			{
				return;
			}
			onServerCallWaitPanelHide();
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x00042543 File Offset: 0x00040743
		private void SupportController_OnSendTicketError(Exception e)
		{
			this.ShowInfo(ScriptLocalization.Get("ErrorMessages/GenericError"));
			Debug.LogError(e.ToString());
			global::System.Action onServerCallWaitPanelHide = this.OnServerCallWaitPanelHide;
			if (onServerCallWaitPanelHide == null)
			{
				return;
			}
			onServerCallWaitPanelHide();
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x00042570 File Offset: 0x00040770
		private void ShowInfo(string message)
		{
			this.errorText.text = message;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000EB2A8 File Offset: 0x000E94A8
		private string GetInfo()
		{
			this.deviceInfo = string.Format(" {0}\n{1}\n{2}\n{3}\n", new object[]
			{
				SystemInfo.deviceModel,
				SystemInfo.deviceName,
				SystemInfo.deviceType,
				SystemInfo.operatingSystem
			});
			this.infoToSend = string.Concat(new string[]
			{
				"Application Version :\n",
				Application.version,
				"\nDevice info :\n",
				this.deviceInfo,
				"\nPlayer name :\n",
				PlayerInfo.me.PlayerStats.Name,
				"\n"
			});
			return this.infoToSend;
		}

		// Token: 0x04001D1C RID: 7452
		[SerializeField]
		private TMP_InputField emailInputField;

		// Token: 0x04001D1D RID: 7453
		[SerializeField]
		private TMP_InputField messageInputField;

		// Token: 0x04001D1E RID: 7454
		[SerializeField]
		private Button sendButton;

		// Token: 0x04001D1F RID: 7455
		[SerializeField]
		private Button exitButton;

		// Token: 0x04001D20 RID: 7456
		[SerializeField]
		private TMP_Text errorText;

		// Token: 0x04001D21 RID: 7457
		private const string FillFields = "MainMenu/FillFields";

		// Token: 0x04001D22 RID: 7458
		private const string IncorrectEmail = "MainMenu/IncorrectEmail";

		// Token: 0x04001D23 RID: 7459
		private const string GenericError = "ErrorMessages/GenericError";

		// Token: 0x04001D24 RID: 7460
		private string deviceInfo;

		// Token: 0x04001D25 RID: 7461
		private string infoToSend;
	}
}
