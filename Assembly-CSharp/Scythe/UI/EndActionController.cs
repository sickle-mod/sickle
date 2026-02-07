using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003E0 RID: 992
	public class EndActionController : MonoBehaviour
	{
		// Token: 0x06001D6D RID: 7533 RVA: 0x0003B1A2 File Offset: 0x000393A2
		private void Start()
		{
			HumanInputHandler.Instance.SetEndActionController(this);
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000B6948 File Offset: 0x000B4B48
		private void OnEnable()
		{
			ActionManager.InputForPayAction += this.EnableInputForPayAction;
			ActionManager.InputForGainAction += this.EnableInputForGainAction;
			ActionManager.BreakActionInput += this.OnBreakSectionAction;
			ActionManager.DisableActionInput += this.OnActionFinished;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000B699C File Offset: 0x000B4B9C
		private void OnDisable()
		{
			ActionManager.InputForPayAction -= this.EnableInputForPayAction;
			ActionManager.InputForGainAction -= this.EnableInputForGainAction;
			ActionManager.BreakActionInput -= this.OnBreakSectionAction;
			ActionManager.DisableActionInput -= this.OnActionFinished;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x0002920A File Offset: 0x0002740A
		protected void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x0003B1AF File Offset: 0x000393AF
		private void OnBreakSectionAction()
		{
			this.SetButtonActive(false);
			if (this.endActionDialog != null)
			{
				this.endActionDialog.Hide();
			}
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000B69F0 File Offset: 0x000B4BF0
		private void EnableInputForGainAction(GainAction action)
		{
			if (action.GetPlayer() == GameController.GameManager.PlayerMaster && action.GetPlayer().IsHuman && action.GetGainType() != GainType.Coin && action.GetGainType() != GainType.Popularity && action.GetGainType() != GainType.Power && action.GetGainType() != GainType.CombatCard)
			{
				this.InputDetected();
			}
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x0003B1D1 File Offset: 0x000393D1
		private void EnableInputForPayAction(PayAction action)
		{
			if (action.GetPlayer() == GameController.GameManager.PlayerMaster && action.GetPlayer().IsHuman)
			{
				this.InputDetected();
			}
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x0003B1F8 File Offset: 0x000393F8
		public void OnActionFinished()
		{
			this.SetButtonActive(false);
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x000B6A48 File Offset: 0x000B4C48
		private void InputDetected()
		{
			this.SetButtonActive(true);
			this.SetDialogFirstLineOfDescription(ScriptLocalization.Get("GameScene/WarningBottomInProgress"));
			if (GameController.GameManager.PlayerCurrent.bottomActionInProgress)
			{
				this.SetButtonText(ScriptLocalization.Get("GameScene/EndTurn"));
				this.SetTextOnDialogYesButton(ScriptLocalization.Get("GameScene/EndTurn"));
				this.SetDialogSecondLineOfDescription(ScriptLocalization.Get("GameScene/UnusedActionQuestion"));
				return;
			}
			this.SetButtonText(ScriptLocalization.Get("GameScene/EndAction"));
			this.SetTextOnDialogYesButton(ScriptLocalization.Get("GameScene/EndAction"));
			this.SetDialogSecondLineOfDescription(ScriptLocalization.Get("GameScene/EndActionQuestion"));
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x0003B201 File Offset: 0x00039401
		public void SetButtonActive(bool active)
		{
			this.endActionButton.gameObject.SetActive(active);
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000B6AE0 File Offset: 0x000B4CE0
		public void ShowEndActionDialog()
		{
			if (this.endActionDialog != null && OptionsManager.IsWarningsActive())
			{
				this.endActionDialog.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, new YesNoDialog.OnClick(this.OnEndActionConfirmed), null);
				return;
			}
			this.OnEndActionConfirmed();
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000B6B44 File Offset: 0x000B4D44
		private void OnEndActionConfirmed()
		{
			HumanInputHandler.Instance.GetSelectedPresenter().OnEndActionConfirmClicked();
			this.SetButtonActive(false);
			if (GameController.GameManager.PlayerCurrent.bottomActionInProgress || GameController.GameManager.PlayerCurrent.downActionFinished)
			{
				GameController.Instance.NextTurn();
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0003B214 File Offset: 0x00039414
		public void SetButtonText(string text)
		{
			this.endActionButtonText.text = text;
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0003B222 File Offset: 0x00039422
		public void SetDialogTitle(string text)
		{
			if (this.endActionDialog != null)
			{
				this.endActionDialog.GetTitle().GetComponent<TextMeshProUGUI>().text = text;
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0003B248 File Offset: 0x00039448
		public void SetDialogFirstLineOfDescription(string text)
		{
			if (this.endActionDialog != null)
			{
				this.endActionDialog.GetFirstDescriptionLine().GetComponent<TextMeshProUGUI>().text = text;
			}
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0003B26E File Offset: 0x0003946E
		public void SetDialogSecondLineOfDescription(string text)
		{
			if (this.endActionDialog != null)
			{
				this.endActionDialog.GetSecondDescriptionLine().GetComponent<TextMeshProUGUI>().text = text;
			}
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000B6B94 File Offset: 0x000B4D94
		public void SetTextOnDialogYesButton(string text)
		{
			if (PlatformManager.IsStandalone)
			{
				if (this.endActionDialog != null)
				{
					this.endActionDialog.GetYesButton().GetComponentInChildren<Text>().text = text;
					return;
				}
			}
			else if (this.endActionDialog != null)
			{
				this.endActionDialog.GetYesButton().GetComponentInChildren<TextMeshProUGUI>().text = text;
			}
		}

		// Token: 0x0400151D RID: 5405
		[SerializeField]
		private Button endActionButton;

		// Token: 0x0400151E RID: 5406
		[SerializeField]
		private TextMeshProUGUI endActionButtonText;

		// Token: 0x0400151F RID: 5407
		[SerializeField]
		private YesNoDialog endActionDialog;
	}
}
