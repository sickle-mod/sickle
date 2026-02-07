using System;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000404 RID: 1028
	public class SimpleAmountPresenter : ActionPresenter
	{
		// Token: 0x06001F5E RID: 8030 RVA: 0x0003C2EC File Offset: 0x0003A4EC
		public override void ChangeLayoutForAction(BaseAction action)
		{
			base.NestedMode = false;
			this.EnableInput(action);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0003C2FC File Offset: 0x0003A4FC
		public override void ChangeLayoutForAction(BaseAction action, ActionPresenter mainPresenter)
		{
			base.ChangeLayoutForAction(action, mainPresenter);
			this.EnableInput(action);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000C0A08 File Offset: 0x000BEC08
		public void EnableInput(BaseAction action)
		{
			if (action is GainAction && ((GainAction)action).GetGainType() == GainType.CombatCard && GameController.Instance.IsAmmoPoolEmpty())
			{
				return;
			}
			GameController.Instance.EndTurnButtonDisable();
			GameController.Instance.playersFactions.HidePassCoinPresenter();
			base.EnableMapBlackout();
			if (action is GainAction)
			{
				this.action = action as GainAction;
				this.amount = action.Amount;
				int num;
				if ((num = (int)((GainAction)action).GetGainType()) == 6)
				{
					num = (int)(4 + ((GainResource)action).ResourceToGain);
				}
				for (int i = 0; i < this.gainButtons.Length; i++)
				{
					this.gainButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = this.gainSprites[num];
					if (i < (int)action.Amount)
					{
						this.gainButtons[i].gameObject.SetActive(true);
						this.gainButtons[i].interactable = true;
						this.gainButtons[i].transform.GetChild(0).GetComponent<Image>().color = this.active;
						this.buttonState[i] = true;
					}
					else
					{
						this.gainButtons[i].gameObject.SetActive(false);
						this.gainButtons[i].interactable = false;
						this.buttonState[i] = false;
					}
				}
			}
			this.actualFactionEmlbem.sprite = GameController.factionInfo[action.GetPlayer().matFaction.faction].logo;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000C0B84 File Offset: 0x000BED84
		public void SelectAmount(int id)
		{
			this.buttonState[id] = !this.buttonState[id];
			if (this.buttonState[id])
			{
				this.gainButtons[id].image.color = this.active;
				this.amount += 1;
			}
			else
			{
				this.gainButtons[id].image.color = this.inactive;
				this.amount -= 1;
			}
			WorldSFXManager.PlaySound(SoundEnum.ProduceSwitch, AudioSourceType.Buttons);
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000C0C08 File Offset: 0x000BEE08
		public override void OnActionEnded()
		{
			switch (this.action.GetGainType())
			{
			case GainType.Coin:
				(this.action as GainCoin).SetCoins(this.amount);
				break;
			case GainType.Popularity:
				(this.action as GainPopularity).SetPopularity(this.amount);
				break;
			case GainType.Power:
				(this.action as GainPower).SetPower(this.amount);
				break;
			case GainType.CombatCard:
				(this.action as GainCombatCard).SetCards(this.amount);
				break;
			case GainType.Resource:
			{
				(this.action as GainResource).SetDestinationAmount(this.action.GetPlayer().character.position, this.amount);
				GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(this.action.GetPlayer().character.position);
				ResourceType resourceToGain = (this.action as GainResource).ResourceToGain;
				gameHexPresenter.ProduceAnimation((int)this.amount, resourceToGain, false);
				break;
			}
			}
			base.DisableMapBlackout();
			base.gameObject.SetActive(false);
			if (!GameController.GameManager.IsMultiplayer || GameController.GameManager.IsMyTurn())
			{
				GameController.Instance.EndTurnButtonEnable();
			}
			if (base.NestedMode)
			{
				this.mainPresenter.OnNestedActionEnded();
			}
			else
			{
				HumanInputHandler.Instance.OnInputEnded();
			}
			WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
		}

		// Token: 0x04001615 RID: 5653
		public Button[] gainButtons;

		// Token: 0x04001616 RID: 5654
		public Sprite[] gainSprites;

		// Token: 0x04001617 RID: 5655
		public Image actualFactionEmlbem;

		// Token: 0x04001618 RID: 5656
		private short amount;

		// Token: 0x04001619 RID: 5657
		private Color inactive = new Color(0.5f, 0.5f, 0.5f, 0.4f);

		// Token: 0x0400161A RID: 5658
		private Color active = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400161B RID: 5659
		private GainAction action;

		// Token: 0x0400161C RID: 5660
		private bool[] buttonState = new bool[4];
	}
}
