using System;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200040A RID: 1034
	public class UpgradePresenter : ActionPresenter
	{
		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06001FA1 RID: 8097 RVA: 0x000C1A4C File Offset: 0x000BFC4C
		// (remove) Token: 0x06001FA2 RID: 8098 RVA: 0x000C1A80 File Offset: 0x000BFC80
		public static event UpgradePresenter.UpgradeEnd UpgradeEnded;

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000C1AB4 File Offset: 0x000BFCB4
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainUpgrade;
			this.topSectionID = -1;
			this.actionIndex = -1;
			GameController.Instance.matFaction.ClearHintStories();
			this.hintAction.SetActive(true);
			this.EnableInput();
			this.AssistHighlight(1);
			if (PlatformManager.IsStandalone)
			{
				this.guideline.enabled = true;
				if (OptionsManager.IsActionAssist())
				{
					GameController.Instance.MaximizePlayerMat();
					return;
				}
			}
			else
			{
				this.hintMobile.SetActive(true);
				GameController.Instance.matFaction.ClearHintStories();
				this.mobileInstruction1.color = Color.white;
				this.mobileInstruction2.color = Color.gray;
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Expanded, 0.25f);
			}
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x000C1B74 File Offset: 0x000BFD74
		public void AssistHighlight(int step)
		{
			for (int i = 0; i < this.assistIcon.Length; i++)
			{
				this.assistHighlight[i].SetActive(i == step);
				this.assistIcon[i].color = ((i == step) ? Color.white : Color.black);
			}
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000C1BC4 File Offset: 0x000BFDC4
		private void EnableInput()
		{
			for (int i = 0; i < 4; i++)
			{
				TopAction actionTop = GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionTop;
				DownAction actionDown = GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionDown;
				MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
				if (!PlatformManager.IsStandalone)
				{
					matPlayerSectionPresenter.ResetColors(actionTop);
					if (actionDown.Type == DownActionType.Upgrade)
					{
						matPlayerSectionPresenter.downActionPresenter.LightUp(matPlayerSectionPresenter.colorActive, false);
						matPlayerSectionPresenter.downActionPresenter.gainActionButtonBackground[0].sprite = GameController.Instance.matPlayer.defaultAction;
					}
				}
				matPlayerSectionPresenter.sectionGlass.enabled = false;
				if (PlatformManager.IsMobile)
				{
					matPlayerSectionPresenter.topActionPresenter.HideInactiveActionOverlays();
				}
				else
				{
					matPlayerSectionPresenter.SetSectionCooldown(false, false);
				}
				for (int j = 0; j < actionTop.GetNumberOfGainActions(); j++)
				{
					if (actionTop.GetGainAction(j).CanUpgrade())
					{
						matPlayerSectionPresenter.topActionPresenter.ChangeModeToUpgrade(j);
					}
					else if (!PlatformManager.IsStandalone)
					{
						matPlayerSectionPresenter.topActionPresenter.ShowInactiveActionOverlay(j);
					}
				}
				if (!PlatformManager.IsStandalone)
				{
					matPlayerSectionPresenter.downActionPresenter.ShowInactiveActionOverlay(0);
				}
			}
			base.EnableMapBlackout();
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0003C53C File Offset: 0x0003A73C
		public void UpgradeGetMove()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			this.AfterUpgradeClick(GainType.Move);
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0003C54D File Offset: 0x0003A74D
		public void UpgradeGetCoin()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			this.AfterUpgradeClick(GainType.Coin);
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0003C55E File Offset: 0x0003A75E
		public void UpgradeProduce()
		{
			this.AfterUpgradeClick(GainType.Produce);
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0003C56F File Offset: 0x0003A76F
		public void UpgradeGetPower()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			this.AfterUpgradeClick(GainType.Power);
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0003C580 File Offset: 0x0003A780
		public void UpgradeGetCards()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			this.AfterUpgradeClick(GainType.CombatCard);
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0003C591 File Offset: 0x0003A791
		public void UpgradeGetPopularity()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			this.AfterUpgradeClick(GainType.Popularity);
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000C1D00 File Offset: 0x000BFF00
		private void AfterUpgradeClick(GainType type)
		{
			this.AssistHighlight(2);
			if (PlatformManager.IsStandalone)
			{
				for (int i = 0; i < 4; i++)
				{
					TopAction actionTop = GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionTop;
					MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
					for (int j = 0; j < actionTop.GetNumberOfGainActions(); j++)
					{
						if (actionTop.GetGainAction(j).GetGainType() == type)
						{
							this.topSectionID = i;
							this.actionIndex = j;
							matPlayerSectionPresenter.topActionPresenter.LightUpUpgradeMarkers(true, j, true, false);
						}
						else
						{
							matPlayerSectionPresenter.topActionPresenter.LightUpUpgradeMarkers(false, j, false, false);
						}
					}
					if (GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionDown.GetPayAction(0).CanUpgrade())
					{
						matPlayerSectionPresenter.downActionPresenter.ChangeModeToUpgrade(0);
					}
				}
				return;
			}
			for (int k = 0; k < 4; k++)
			{
				TopAction actionTop2 = GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(k).ActionTop;
				MatPlayerSectionPresenter matPlayerSectionPresenter2 = GameController.Instance.matPlayer.matSection[k];
				for (int l = 0; l < actionTop2.GetNumberOfGainActions(); l++)
				{
					if (actionTop2.GetGainAction(l).GetGainType() == type)
					{
						this.topSectionID = k;
						this.actionIndex = l;
						matPlayerSectionPresenter2.topActionPresenter.LightUpUpgradeMarkers(actionTop2.GetGainAction(l).CanUpgrade(), l, true, false);
					}
					else
					{
						matPlayerSectionPresenter2.topActionPresenter.LightUpUpgradeMarkers(actionTop2.GetGainAction(l).CanUpgrade(), l, false, false);
					}
					if (!PlatformManager.IsStandalone)
					{
						matPlayerSectionPresenter2.topActionPresenter.ShowInactiveActionOverlay(l);
					}
				}
				if (GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(k).ActionDown.GetPayAction(0).CanUpgrade())
				{
					matPlayerSectionPresenter2.downActionPresenter.HideInactiveActionOverlays();
					matPlayerSectionPresenter2.downActionPresenter.ChangeModeToUpgrade(0);
				}
			}
			this.mobileInstruction1.color = Color.gray;
			this.mobileInstruction2.color = Color.white;
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000C1F1C File Offset: 0x000C011C
		public void OnChooseDownAction(int downSectionID)
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
			GainAction gainAction = GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.topSectionID).ActionTop.GetGainAction(this.actionIndex);
			PayAction payAction = GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(downSectionID).ActionDown.GetPayAction(0);
			this.action.SetPayAndGainActions(gainAction, payAction);
			this.OnActionEnded();
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0003AD11 File Offset: 0x00038F11
		public void OnEndUpgradeButtonClicked()
		{
			GameController.Instance.PopupWindowsBeforeNextTurn();
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x000C1F94 File Offset: 0x000C0194
		public override void OnActionEnded()
		{
			this.Clear();
			if (PlatformManager.IsStandalone)
			{
				GameController.Instance.MinimizePlayerMat();
			}
			if (this.hintMobile != null)
			{
				this.hintMobile.SetActive(false);
			}
			this.DisableInput();
			HumanInputHandler.Instance.OnInputEnded();
			base.gameObject.SetActive(false);
			if (UpgradePresenter.UpgradeEnded != null)
			{
				UpgradePresenter.UpgradeEnded();
			}
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000C2000 File Offset: 0x000C0200
		private void DisableInput()
		{
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
				matPlayerSectionPresenter.topActionPresenter.ChangeModeToNormal();
				matPlayerSectionPresenter.downActionPresenter.ChangeModeToNormal();
				matPlayerSectionPresenter.topActionPresenter.DisableAllButtons(false);
				matPlayerSectionPresenter.downActionPresenter.DisableAllButtons(false);
				matPlayerSectionPresenter.downActionPresenter.LightUpUpgradeMarkers(false);
				if (PlatformManager.IsMobile)
				{
					if (GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(matPlayerSectionPresenter.sectionID).ActionDown.CanPlayerPayActions() && this.action.IsEncounter)
					{
						matPlayerSectionPresenter.SetSectionCooldown(false, true);
					}
					else
					{
						matPlayerSectionPresenter.SetSectionCooldown(true, false);
					}
				}
				else
				{
					matPlayerSectionPresenter.GetComponent<Animator>().Play("PlayerMatSectionTopIconsHide");
				}
				for (int j = 0; j < GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionTop.GetNumberOfGainActions(); j++)
				{
					matPlayerSectionPresenter.topActionPresenter.LightUpUpgradeMarkers(false, j, false, false);
				}
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000C2108 File Offset: 0x000C0308
		public override void Clear()
		{
			GameObject[] array = this.assistHighlight;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			if (this.guideline != null)
			{
				this.guideline.enabled = false;
			}
			GameController.Instance.matFaction.ClearHintStories();
			base.DisableMapBlackout();
		}

		// Token: 0x0400163B RID: 5691
		public Image guideline;

		// Token: 0x0400163C RID: 5692
		public GameObject hintAction;

		// Token: 0x0400163D RID: 5693
		public GameObject hintMobile;

		// Token: 0x0400163E RID: 5694
		public GameObject[] assistHighlight;

		// Token: 0x0400163F RID: 5695
		public Image[] assistIcon;

		// Token: 0x04001640 RID: 5696
		public TextMeshProUGUI mobileInstruction1;

		// Token: 0x04001641 RID: 5697
		public TextMeshProUGUI mobileInstruction2;

		// Token: 0x04001642 RID: 5698
		private GainUpgrade action;

		// Token: 0x04001643 RID: 5699
		private int topSectionID = -1;

		// Token: 0x04001644 RID: 5700
		private int actionIndex = -1;

		// Token: 0x0200040B RID: 1035
		// (Invoke) Token: 0x06001FB4 RID: 8116
		public delegate void UpgradeEnd();
	}
}
