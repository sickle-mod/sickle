using System;
using I2.Loc;
using Scythe.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003C1 RID: 961
	public class ConnectionProblemPresenter : MonoBehaviour
	{
		// Token: 0x06001C04 RID: 7172 RVA: 0x0003A50E File Offset: 0x0003870E
		private void OnDisable()
		{
			this.okButton.onClick.RemoveListener(new UnityAction(this.OnOkButtonClicked));
			this.okButton.onClick.RemoveListener(new UnityAction(this.OutOfTimeOkButton));
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000AFB14 File Offset: 0x000ADD14
		private void Update()
		{
			if (this.turnOnButton)
			{
				this.turnOnButton = false;
				this.timeLeftText.gameObject.SetActive(false);
				this.okButton.gameObject.SetActive(true);
			}
			this.timeLeftText.text = ConnectionProblem.TimeLeft.ToString();
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000AFB6C File Offset: 0x000ADD6C
		public void Activate()
		{
			this.timeLeftText.gameObject.SetActive(true);
			this.okButton.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
			this.errorText.text = ScriptLocalization.Get(ConnectionProblem.currentError);
			this.titleText.text = ScriptLocalization.Get(ConnectionProblem.currentErrorTitle);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x00029172 File Offset: 0x00027372
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x0003A548 File Offset: 0x00038748
		public void ChangeActiveState()
		{
			if (ConnectionProblem.ErrorShown)
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

		// Token: 0x06001C09 RID: 7177 RVA: 0x000AFBD4 File Offset: 0x000ADDD4
		public void Disconnected()
		{
			this.okButton.onClick.AddListener(new UnityAction(this.OnOkButtonClicked));
			this.okButton.onClick.RemoveListener(new UnityAction(this.OutOfTimeOkButton));
			this.turnOnButton = true;
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000AFC20 File Offset: 0x000ADE20
		public void OnRunOutOfTime()
		{
			if (GameController.GameManager.GetPlayersWithoutAICount() == 1)
			{
				this.okButton.onClick.RemoveListener(new UnityAction(this.OnOkButtonClicked));
				this.okButton.onClick.AddListener(new UnityAction(this.OutOfTimeOkButton));
			}
			else
			{
				this.okButton.onClick.AddListener(new UnityAction(this.OnOkButtonClicked));
				this.okButton.onClick.RemoveListener(new UnityAction(this.OutOfTimeOkButton));
			}
			this.turnOnButton = true;
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x0003A56B File Offset: 0x0003876B
		private void OutOfTimeOkButton()
		{
			base.gameObject.SetActive(false);
			GameController.Instance.RunOutOfTime();
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0003A583 File Offset: 0x00038783
		private void OnOkButtonClicked()
		{
			GameController.Instance.ExitGame();
		}

		// Token: 0x0400141B RID: 5147
		[SerializeField]
		private Button okButton;

		// Token: 0x0400141C RID: 5148
		[SerializeField]
		private Text errorText;

		// Token: 0x0400141D RID: 5149
		[SerializeField]
		private Text timeLeftText;

		// Token: 0x0400141E RID: 5150
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x0400141F RID: 5151
		private bool turnOnButton;
	}
}
