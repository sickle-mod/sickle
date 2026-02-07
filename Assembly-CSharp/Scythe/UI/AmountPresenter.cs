using System;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003D2 RID: 978
	public class AmountPresenter : ActionPresenter
	{
		// Token: 0x06001CAE RID: 7342 RVA: 0x000B291C File Offset: 0x000B0B1C
		private void Awake()
		{
			this.lessButton.onClick.AddListener(new UnityAction(this.OnLessButtonClicked));
			this.moreButton.onClick.AddListener(new UnityAction(this.OnMoreButtonClicked));
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0003AB4B File Offset: 0x00038D4B
		public override void ChangeLayoutForAction(BaseAction action)
		{
			base.NestedMode = false;
			this.Initialize(action);
			GameController.Instance.matFaction.ClearHintStories();
			this.Show();
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x0003AB70 File Offset: 0x00038D70
		public override void ChangeLayoutForAction(BaseAction action, ActionPresenter mainPresenter)
		{
			base.ChangeLayoutForAction(action, mainPresenter);
			this.Initialize(action);
			GameController.Instance.matFaction.ClearHintStories();
			this.Show();
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000B2980 File Offset: 0x000B0B80
		public override void OnActionEnded()
		{
			switch (this.gainAction.GetGainType())
			{
			case GainType.Coin:
				((GainCoin)this.gainAction).SetCoins((short)this.currentAmount);
				break;
			case GainType.Popularity:
				((GainPopularity)this.gainAction).SetPopularity((short)this.currentAmount);
				break;
			case GainType.Power:
				((GainPower)this.gainAction).SetPower((short)this.currentAmount);
				break;
			case GainType.CombatCard:
				((GainCombatCard)this.gainAction).SetCards((short)this.currentAmount);
				break;
			case GainType.Resource:
			{
				GainResource gainResource = (GainResource)this.gainAction;
				gainResource.SetDestinationAmount(gainResource.GetPlayer().character.position, (short)this.currentAmount);
				break;
			}
			}
			this.Hide();
			this.NotifyActionEnded();
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0002D68D File Offset: 0x0002B88D
		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00029172 File Offset: 0x00027372
		private void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0003AB96 File Offset: 0x00038D96
		private void Initialize(BaseAction action)
		{
			this.gainAction = (GainAction)action;
			this.maxAmount = (int)this.gainAction.Amount;
			this.currentAmount = (int)this.gainAction.Amount;
			this.Refresh();
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0003ABCC File Offset: 0x00038DCC
		private void ModifyAmount(int mod)
		{
			this.currentAmount = Mathf.Clamp(this.currentAmount + mod, 0, this.maxAmount);
			this.Refresh();
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0003ABEE File Offset: 0x00038DEE
		private void NotifyActionEnded()
		{
			if (base.NestedMode)
			{
				this.mainPresenter.OnNestedActionEnded();
				return;
			}
			HumanInputHandler.Instance.OnInputEnded();
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x0003AC0E File Offset: 0x00038E0E
		private void Refresh()
		{
			this.RefreshText();
			this.RefreshIcon();
			this.RefreshButtons();
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x0003AC22 File Offset: 0x00038E22
		private void RefreshText()
		{
			this.valueText.text = this.currentAmount.ToString();
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000B2A5C File Offset: 0x000B0C5C
		private void RefreshIcon()
		{
			this.gainAction.GetGainType();
			if (!(this.gainAction is GainResource))
			{
				this.iconImage.sprite = this.resourceIcons[(int)this.gainAction.GetGainType()];
				return;
			}
			GainResource gainResource = (GainResource)this.gainAction;
			this.iconImage.sprite = this.resourceIcons[(int)(4 + gainResource.ResourceToGain)];
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0003AC3A File Offset: 0x00038E3A
		private void RefreshButtons()
		{
			this.lessButton.interactable = this.currentAmount > 0;
			this.moreButton.interactable = this.currentAmount < this.maxAmount;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x0003AC69 File Offset: 0x00038E69
		private void OnLessButtonClicked()
		{
			this.ModifyAmount(-1);
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x0003AC72 File Offset: 0x00038E72
		private void OnMoreButtonClicked()
		{
			this.ModifyAmount(1);
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x0003AB21 File Offset: 0x00038D21
		private void OnConfirmButtonClicked()
		{
			this.OnActionEnded();
		}

		// Token: 0x040014B8 RID: 5304
		[SerializeField]
		private Button lessButton;

		// Token: 0x040014B9 RID: 5305
		[SerializeField]
		private Button moreButton;

		// Token: 0x040014BA RID: 5306
		[SerializeField]
		private Button confirmButton;

		// Token: 0x040014BB RID: 5307
		[SerializeField]
		private TextMeshProUGUI valueText;

		// Token: 0x040014BC RID: 5308
		[SerializeField]
		private Image iconImage;

		// Token: 0x040014BD RID: 5309
		[SerializeField]
		private Sprite[] resourceIcons;

		// Token: 0x040014BE RID: 5310
		private int currentAmount;

		// Token: 0x040014BF RID: 5311
		private int maxAmount;

		// Token: 0x040014C0 RID: 5312
		private GainAction gainAction;
	}
}
