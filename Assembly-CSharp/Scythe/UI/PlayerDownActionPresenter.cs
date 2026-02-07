using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000487 RID: 1159
	public class PlayerDownActionPresenter : PlayerActionPresenter
	{
		// Token: 0x060024D6 RID: 9430 RVA: 0x000DA96C File Offset: 0x000D8B6C
		public void Init(Player player, int actionCosts, int actionGains, int actionUpgradeLevel, int maxUpgradeLevel, bool recruitEnlisted)
		{
			this.player = player;
			this.Reset();
			for (int i = actionCosts - (maxUpgradeLevel - actionUpgradeLevel); i < actionCosts; i++)
			{
				if (this.upgradeableCostLayout != null)
				{
					this.actionCost[i].transform.SetParent(this.upgradeableCostLayout);
					this.actionCost[i].transform.SetAsLastSibling();
				}
				if (PlatformManager.IsStandalone)
				{
					this.actionCost[i].transform.GetChild(3).GetComponent<Image>().enabled = true;
				}
			}
			int num = actionCosts;
			if (PlatformManager.IsStandalone)
			{
				num += actionUpgradeLevel;
			}
			for (int j = num; j < this.actionCost.Length; j++)
			{
				this.actionCost[j].gameObject.SetActive(false);
			}
			if (this.actionUpgradableSign != null)
			{
				this.actionUpgradableSign.enabled = actionUpgradeLevel < maxUpgradeLevel;
			}
			this.UpdateSection(player, actionCosts, actionGains, recruitEnlisted);
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000DAA54 File Offset: 0x000D8C54
		public void UpdateSection(Player player, int actionCosts, int actionGains, bool recruitEnlisted)
		{
			this.player = player;
			for (int i = actionCosts; i < this.actionCost.Length; i++)
			{
				this.actionCost[i].sprite = this.CostInactiveBackground;
				if (PlatformManager.IsStandalone)
				{
					this.actionCost[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
					this.actionCost[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
					this.actionCost[i].transform.GetChild(3).GetComponent<Image>().enabled = false;
					this.actionCost[i].transform.GetChild(3).GetChild(0).GetComponent<Image>()
						.enabled = false;
					this.actionCost[i].transform.GetChild(3).GetChild(1).GetComponent<Image>()
						.enabled = false;
				}
				else
				{
					this.actionCost[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
					this.actionCost[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
				}
			}
			if (!PlatformManager.IsStandalone)
			{
				if (this.upgradeAvailableIcons.Length != 0 && this.upgradeAvailableIcons[0] != null)
				{
					this.upgradeAvailableIcons[0].enabled = actionCosts > this.actionCost.Length - this.upgradeableCostLayout.childCount;
				}
				DownAction action = this.GetAction();
				if (action != null)
				{
					base.UpdateActionCostOpacity(action);
					base.UpdateActionCostOverlay(this.GetAction());
				}
			}
			for (int j = 0; j < actionGains; j++)
			{
				this.actionGain[j].gameObject.SetActive(true);
			}
			if (recruitEnlisted)
			{
				this.recruit.color = Color.white;
				this.recruitBonus.material = null;
				this.recruitBackground.sprite = GameController.Instance.matPlayer.bonusUnlocked;
			}
			if (GameController.GameManager != null)
			{
				this.UpdateEnemyRecruits(player);
			}
			if (this.hintAction != null)
			{
				ActionContextWindow component = this.hintAction.GetComponent<ActionContextWindow>();
				this.hintAction.SetActive(false);
				if (component != null)
				{
					component.PayAmount = actionCosts;
					component.GainAmount = actionGains;
				}
			}
			if (this.maxReachedBadge != null)
			{
				this.maxReachedBadge.SetActive(false);
			}
			if (this.noSuitablePlaceBadge != null)
			{
				this.noSuitablePlaceBadge.SetActive(false);
			}
			if (player != null)
			{
				if (player.matPlayer.GetDownAction(this.actionType).IsMaxReached() && this.maxReachedBadge != null)
				{
					this.maxReachedBadge.SetActive(true);
					return;
				}
				if (player.matPlayer.GetDownAction(this.actionType).CanPlayerPayActions() && this.noSuitablePlaceBadge != null)
				{
					bool flag = this.actionType == DownActionType.Enlist || this.actionType == DownActionType.Factory || this.actionType == DownActionType.Upgrade || (this.actionType == DownActionType.Build && GameController.Instance.matPlayer.SuitablePlaceForBuildCheck()) || (this.actionType == DownActionType.Deploy && GameController.Instance.matPlayer.SuitablePlaceForMechCheck());
					this.noSuitablePlaceBadge.SetActive(!flag);
				}
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000DAD7C File Offset: 0x000D8F7C
		public void UpdateSectionSpecial(int actionCosts, int actionGains, int actionUpgradeLevel, int maxUpgradeLevel, bool recruitEnlisted)
		{
			for (int i = 0; i < this.actionGain.Length; i++)
			{
				this.actionGain[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < this.actionCost.Length; j++)
			{
				this.actionCost[j].gameObject.SetActive(false);
				this.actionCost[j].transform.GetChild(3).GetComponent<Image>().enabled = false;
			}
			for (int k = actionCosts - (maxUpgradeLevel - actionUpgradeLevel); k < actionCosts; k++)
			{
				this.actionCost[k].transform.GetChild(3).GetComponent<Image>().enabled = true;
				if (this.upgradeableCostLayout != null)
				{
					this.actionCost[k].transform.SetParent(this.upgradeableCostLayout);
					this.actionCost[k].transform.SetAsLastSibling();
				}
			}
			if (this.actionUpgradableSign != null)
			{
				this.actionUpgradableSign.enabled = actionUpgradeLevel < maxUpgradeLevel;
			}
			for (int l = 0; l < actionGains; l++)
			{
				this.actionGain[l].gameObject.SetActive(true);
			}
			for (int m = 0; m < actionCosts; m++)
			{
				this.actionCost[m].gameObject.SetActive(true);
			}
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000DAEBC File Offset: 0x000D90BC
		public void UpdateEnemyRecruits(Player player)
		{
			if (this.actionType != DownActionType.Factory)
			{
				List<Player> playerNeighbours = GameController.GameManager.GetPlayerNeighbours(player);
				if (playerNeighbours.Count > 0)
				{
					this.recruitNeighbourRight.enabled = true;
					this.recruitNeighbourRight.sprite = GameController.factionInfo[playerNeighbours[0].matFaction.faction].logo;
					if (playerNeighbours[0].matPlayer.GetDownAction(this.actionType).IsRecruitEnlisted)
					{
						this.recruitNeighbourRight.material = null;
					}
					else
					{
						this.recruitNeighbourRight.material = this.recruitNeighbourInactive;
					}
				}
				else
				{
					this.recruitNeighbourRight.enabled = false;
				}
				if (playerNeighbours.Count > 1)
				{
					this.recruitNeighbourLeft.enabled = true;
					this.recruitNeighbourLeft.sprite = GameController.factionInfo[playerNeighbours[1].matFaction.faction].logo;
					if (playerNeighbours[1].matPlayer.GetDownAction(this.actionType).IsRecruitEnlisted)
					{
						this.recruitNeighbourLeft.material = null;
						return;
					}
					this.recruitNeighbourLeft.material = this.recruitNeighbourInactive;
					return;
				}
				else
				{
					this.recruitNeighbourLeft.enabled = false;
				}
			}
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x000DAFF8 File Offset: 0x000D91F8
		protected override void Reset()
		{
			base.Reset();
			if (PlatformManager.IsMobile)
			{
				this.SelectAction(false);
			}
			this.ChangeModeToNormal();
			if (this.actionType != DownActionType.Factory)
			{
				this.recruit.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
				this.recruitBonus.material = this.InactiveMaterial;
				if (GameController.Instance != null)
				{
					this.recruitBackground.sprite = GameController.Instance.matPlayer.bonusDefault;
				}
				this.recruitButton.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.actionCost.Length; i++)
			{
				this.actionCost[i].gameObject.SetActive(true);
				this.actionCost[i].sprite = this.CostActiveBackground;
				this.actionCost[i].color = Color.white;
				this.actionCost[i].enabled = base.IsActionCostEnabled;
				if (PlatformManager.IsStandalone)
				{
					this.actionCost[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
					this.actionCost[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
					this.actionCost[i].transform.GetChild(1).GetComponent<Image>().color = Color.white;
					this.actionCost[i].transform.GetChild(3).GetComponent<Image>().enabled = false;
					this.actionCost[i].transform.GetChild(3).GetChild(0).GetComponent<Image>()
						.enabled = false;
					this.actionCost[i].transform.GetChild(3).GetChild(1).GetComponent<Image>()
						.enabled = false;
					this.actionCostButton[i].gameObject.SetActive(false);
					this.actionCostButton[i].interactable = true;
					this.actionCostButton[i].image.raycastTarget = true;
				}
				else
				{
					this.actionCost[i].transform.SetParent(this.defaultCostLayout);
					this.actionCost[i].transform.SetAsLastSibling();
					this.actionCost[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
					this.actionCost[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
					this.actionCost[i].transform.GetChild(1).GetComponent<Image>().color = Color.white;
				}
			}
			for (int j = 0; j < this.actionGain.Length; j++)
			{
				this.actionGain[j].gameObject.SetActive(false);
			}
			this.ResetGainTileColors();
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x000DB2B4 File Offset: 0x000D94B4
		public void ResetGainTileColors()
		{
			for (int i = 0; i < this.actionGain.Length; i++)
			{
				this.actionGain[i].color = Color.white;
				this.actionGain[i].transform.GetChild(0).GetComponent<Image>().color = ((i == 0) ? this.mainGain : Color.white);
			}
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000DB314 File Offset: 0x000D9514
		public void SelectAction(bool fade)
		{
			if (!PlatformManager.IsStandalone)
			{
				Color color = new Color(1f, 1f, 1f, 0.9f);
				for (int i = 0; i < this.inactiveActionOverlays.Length; i++)
				{
					if (this.inactiveActionOverlays.Length > i && this.inactiveActionOverlays[i] != null)
					{
						base.ChangeInteractiveActionOverlayState(i, fade);
						base.ChangeInteractiveActionColor(i, color);
					}
				}
				if (this.GetAction() != null)
				{
					base.UpdateActionCostOverlay(this.GetAction());
				}
			}
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000DB398 File Offset: 0x000D9598
		private void SelectAction(MatPlayerPresenter mat, int sectionId)
		{
			if (this.player.matPlayer.GetPlayerMatSection(sectionId).ActionDown.CanPlayerPayActions())
			{
				if (this.guidelinePay != null && PlatformManager.IsStandalone)
				{
					this.guidelinePay.SetActive(true);
				}
				if (OptionsManager.IsActionAssist() && mat.matSection[sectionId].GetComponent<Animator>() != null && PlatformManager.IsStandalone)
				{
					mat.matSection[sectionId].GetComponent<Animator>().Play("PlayerMatSectionTopIconsShow");
				}
				switch (this.actionType)
				{
				case DownActionType.Upgrade:
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardUpgradeConfrm, AudioSourceType.Buttons);
					HumanInputHandler.Instance.upgradePresenter.GetComponent<UpgradePresenter>().AssistHighlight(0);
					break;
				case DownActionType.Deploy:
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardDeplyConfrm, AudioSourceType.Buttons);
					HumanInputHandler.Instance.deployPresenter.GetComponent<DeployPresenter>().AssistHighlight(0);
					break;
				case DownActionType.Build:
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildConfrm, AudioSourceType.Buttons);
					HumanInputHandler.Instance.buildPresenter.GetComponent<BuildPresenter>().AssistHighlight(0);
					break;
				case DownActionType.Enlist:
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardEnlistConfrm, AudioSourceType.Buttons);
					HumanInputHandler.Instance.enlistPresenter.GetComponent<EnlistPresenter>().AssistHighlight(0);
					break;
				case DownActionType.Factory:
					WorldSFXManager.PlaySound(SoundEnum.FactoryActonChoose, AudioSourceType.Buttons);
					break;
				}
			}
			mat.MatPlayerSectionSelected(sectionId);
			mat.StartBottomAction();
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000DB4E0 File Offset: 0x000D96E0
		public void ClickAction()
		{
			GameController.Instance.matPlayer.maxResources.Hide();
			GameController.Instance.matPlayer.DisableAllDialogs();
			int sectionID = base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID;
			GameController.Instance.matPlayer.ShowSectionMatActionReloading(sectionID, false);
			foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in GameController.Instance.matPlayer.matSection)
			{
				Image[] array = matPlayerSectionPresenter.topActionPresenter.gainActionButtonBackground;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = (matPlayerSectionPresenter.topActionPresenter.isFactoryAction ? GameController.Instance.matPlayer.factoryActiveAction : GameController.Instance.matPlayer.defaultAction);
				}
				array = matPlayerSectionPresenter.downActionPresenter.gainActionButtonBackground;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = (matPlayerSectionPresenter.downActionPresenter.isFactoryAction ? GameController.Instance.matPlayer.factoryActiveAction : GameController.Instance.matPlayer.defaultAction);
				}
			}
			if (!this.actionEnabled[0])
			{
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.matPlayer.ShowSectionMatActionSelection(false);
				}
				if (GameController.GameManager.IsMultiplayer)
				{
					GameController.Instance.matPlayer.ShowSectionMatActionReloading(sectionID, false);
				}
				else
				{
					GameController.Instance.matPlayer.ShowSectionMatActionReloading(true);
				}
				this.ShowHelp(false);
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardInactiveScreenClick, AudioSourceType.Buttons);
				return;
			}
			this.gainActionButtonBackground[0].sprite = GameController.Instance.matPlayer.activeAction;
			GameController.Instance.MinimizePlayerMat();
			if (OptionsManager.IsConfirmActions())
			{
				GameController.Instance.matPlayer.ShowSectionMatActionReloading(false);
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.matPlayer.ShowSectionMatActionSelection(true);
					GameController.Instance.matPlayer.confirmActionSelection.onClick.RemoveAllListeners();
					GameController.Instance.matPlayer.confirmActionSelection.onClick.AddListener(delegate
					{
						this.StartAction();
					});
				}
				this.ShowHelp(true);
				this.ActionClickSound();
				return;
			}
			this.StartAction();
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000DB72C File Offset: 0x000D992C
		private void ActionClickSound()
		{
			switch (this.actionType)
			{
			case DownActionType.Upgrade:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardUpgradeClick, AudioSourceType.Buttons);
				return;
			case DownActionType.Deploy:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardDeplyClick, AudioSourceType.Buttons);
				return;
			case DownActionType.Build:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildClick, AudioSourceType.Buttons);
				return;
			case DownActionType.Enlist:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardEnlistClick, AudioSourceType.Buttons);
				return;
			case DownActionType.Factory:
				WorldSFXManager.PlaySound(SoundEnum.FactoryActonClick, AudioSourceType.Buttons);
				return;
			default:
				return;
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000DB788 File Offset: 0x000D9988
		public void StartAction()
		{
			int sectionId = base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID;
			MatPlayerPresenter mat = GameController.Instance.matPlayer;
			bool flag = this.player.matPlayer.GetPlayerMatSection(sectionId).ActionDown.CanPlayerPayActions();
			bool flag2 = this.player.matPlayer.GetPlayerMatSection(sectionId).ActionDown.CanPlayerGainFromActions();
			this.notEnoughResourcesDialog.gameObject.SetActive(false);
			if (!this.player.topActionFinished && !this.player.downActionFinished && OptionsManager.IsWarningsActive())
			{
				mat.topActionAvailable.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
				{
					if (OptionsManager.IsConfirmActions())
					{
						this.ShowHelp(true);
					}
					if ((this.actionType == DownActionType.Deploy && !mat.SuitablePlaceForMechCheck()) || (this.actionType == DownActionType.Build && !mat.SuitablePlaceForBuildCheck()))
					{
						this.SuitablePlaceForActionWarning(mat, sectionId);
						return;
					}
					this.SelectAction(mat, sectionId);
					if (!this.player.matPlayer.GetPlayerMatSection(this.GetComponentInParent<MatPlayerSectionPresenter>().sectionID).ActionDown.CanPlayerPayActions() && OptionsManager.IsConfirmActions())
					{
						GameController.Instance.NextTurn();
					}
				}, delegate
				{
					if (OptionsManager.IsConfirmActions())
					{
						GameController.Instance.matPlayer.ShowSectionMatActionSelection(true);
						return;
					}
					this.ResetButtonsBackgrounds();
				});
				return;
			}
			if (!this.actionEnabled[0])
			{
				this.ShowHelp(true);
				return;
			}
			if (!flag)
			{
				this.SelectAction(mat, sectionId);
				GameController.Instance.PopupWindowsBeforeNextTurn();
				return;
			}
			if ((this.actionType == DownActionType.Deploy && !mat.SuitablePlaceForMechCheck()) || (this.actionType == DownActionType.Build && !mat.SuitablePlaceForBuildCheck()))
			{
				this.SuitablePlaceForActionWarning(mat, sectionId);
				return;
			}
			if (flag2 && (this.actionType != DownActionType.Build || this.player.matPlayer.buildings.Count != 4))
			{
				this.SelectAction(mat, sectionId);
				return;
			}
			mat.maxedOutWarning.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
			{
				this.SelectAction(mat, sectionId);
			}, delegate
			{
				if (OptionsManager.IsConfirmActions())
				{
					GameController.Instance.matPlayer.ShowSectionMatActionSelection(true);
					return;
				}
				this.ResetButtonsBackgrounds();
			});
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000DB974 File Offset: 0x000D9B74
		public void LightUp(Color background, bool on)
		{
			base.GetComponent<Image>().color = background;
			for (int i = 0; i < this.actionCost.Length; i++)
			{
				if (this.actionCost[i].sprite != this.CostInactiveBackground)
				{
					this.actionCost[i].transform.GetChild(0).GetComponent<Image>().enabled = on;
				}
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000DB9D8 File Offset: 0x000D9BD8
		public void LightUpUpgradeMarkers(bool on)
		{
			int num = this.actionCost.Length - 1;
			if (PlatformManager.IsStandalone)
			{
				while (num > 0 && !this.actionCost[num].transform.GetChild(3).GetComponent<Image>().enabled)
				{
					num--;
				}
				Transform child = this.actionCost[num].transform.GetChild(3);
				if (child.GetComponent<Image>().enabled)
				{
					if (on)
					{
						this.actionCost[num].GetComponent<Image>().type = Image.Type.Simple;
						this.actionCost[num].GetComponent<Image>().sprite = this.goldBackgroundMaterial;
						this.actionCost[num].GetComponent<Image>().color = Color.white;
					}
					else
					{
						this.actionCost[num].GetComponent<Image>().sprite = this.CostActiveBackground;
						this.actionCost[num].GetComponent<Image>().type = Image.Type.Simple;
					}
					child.GetChild(0).GetComponent<Image>().enabled = on;
					child.GetChild(1).GetComponent<Image>().enabled = on;
					return;
				}
			}
			else
			{
				while (num > 0 && !this.actionCost[num].transform.GetChild(1).GetComponent<Image>().enabled)
				{
					num--;
				}
				this.actionCost[num].transform.GetChild(0).GetComponent<Image>().enabled = on;
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000DBB24 File Offset: 0x000D9D24
		private void ChangeLayoutToUpgrade()
		{
			for (int i = 0; i < this.actionCost.Length; i++)
			{
				this.actionCost[i].GetComponent<LayoutElement>().minWidth = this.costIconUpscaledSize.x;
				this.actionCost[i].GetComponent<LayoutElement>().minHeight = this.costIconUpscaledSize.y;
			}
			if (!this.isFactoryAction)
			{
				this.actionGain[0].transform.GetChild(0).GetComponent<Image>().enabled = false;
				this.actionGain[0].transform.GetChild(1).GetComponent<Image>().enabled = true;
			}
			if (PlatformManager.IsStandalone)
			{
				this.actionCostsLayout.spacing = this.actionCostsLayoutUpscaledSpacing;
				this.actionCostsLayout.padding.top = 0;
			}
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000DBBEC File Offset: 0x000D9DEC
		private void ChangeLayoutToDefault()
		{
			for (int i = 0; i < this.actionCost.Length; i++)
			{
				this.actionCost[i].GetComponent<LayoutElement>().minWidth = this.costIconDefaultSize.x;
				this.actionCost[i].GetComponent<LayoutElement>().minHeight = this.costIconDefaultSize.y;
			}
			if (!this.isFactoryAction)
			{
				this.actionGain[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
				this.actionGain[0].transform.GetChild(1).GetComponent<Image>().enabled = false;
			}
			if (PlatformManager.IsStandalone)
			{
				this.actionCostsLayout.spacing = this.actionCostsLayoutDefaultSpacing;
				this.actionCostsLayout.padding.top = 0;
			}
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x0003F74C File Offset: 0x0003D94C
		public override void ChangeModeToUpgrade(int index)
		{
			this.LightUpUpgradeMarkers(true);
			if (!PlatformManager.IsStandalone)
			{
				this.ChangeLayoutToUpgrade();
			}
			base.ChangeModeToUpgrade(index);
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x0003F769 File Offset: 0x0003D969
		public override void ChangeModeToNormal()
		{
			base.ChangeModeToNormal();
			if (!PlatformManager.IsStandalone)
			{
				this.ChangeLayoutToDefault();
			}
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000DBCB4 File Offset: 0x000D9EB4
		public override void OnPayResourceEnd()
		{
			this.LightUp(base.GetComponent<Image>().color, false);
			base.StartCoroutine(this.HideGuidelinePay());
			if (PlatformManager.IsStandalone)
			{
				for (int i = 0; i < this.actionCost.Length; i++)
				{
					if (this.actionCost[i].sprite != this.CostInactiveBackground)
					{
						this.actionCost[i].transform.GetChild(1).GetComponent<Image>().color = Color.gray;
					}
				}
			}
			base.OnPayResourceEnd();
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x0003F77E File Offset: 0x0003D97E
		private IEnumerator HideGuidelinePay()
		{
			yield return new WaitForSeconds(1f);
			if (this.guidelinePay != null)
			{
				this.guidelinePay.SetActive(false);
			}
			yield break;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000DBD3C File Offset: 0x000D9F3C
		public void AcquireGain(int index)
		{
			if (PlatformManager.IsStandalone && index < this.actionGain.Length)
			{
				this.actionGain[index].color = Color.gray;
				this.actionGain[index].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000DBD90 File Offset: 0x000D9F90
		public void FinishAction(MatPlayerSectionPresenter section)
		{
			GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
			this.OnPayResourceEnd();
			if (PlatformManager.IsStandalone)
			{
				for (int i = 0; i < this.actionGain.Length; i++)
				{
					this.actionGain[i].color = Color.gray;
					this.actionGain[i].transform.GetChild(0).GetComponent<Image>().color = ((i == 0) ? (this.mainGain / 2f) : Color.gray);
				}
			}
			if (OptionsManager.IsActionAssist() && section.GetComponent<Animator>() != null && PlatformManager.IsStandalone)
			{
				section.GetComponent<Animator>().Play("PlayerMatSectionTopIconsHide");
			}
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000DBE44 File Offset: 0x000DA044
		public void ShowHelp(bool interactable)
		{
			GameController.Instance.matFaction.ClearHintStories();
			if (this.hintAction != null)
			{
				ActionContextWindow context = this.hintAction.GetComponent<ActionContextWindow>();
				if (context != null)
				{
					bool flag = GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(this.player.lastMatSection, base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID);
					context.SetUsedLastTurn(flag);
					bool flag2 = this.player.matPlayer.GetPlayerMatSection(base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID).ActionDown.CanPlayerPayActions();
					if (!flag)
					{
						context.SetCanAfford(flag2);
						if (flag2)
						{
							bool flag3 = this.actionType == DownActionType.Enlist || this.actionType == DownActionType.Factory || this.actionType == DownActionType.Upgrade || (this.actionType == DownActionType.Build && GameController.Instance.matPlayer.SuitablePlaceForBuildCheck()) || (this.actionType == DownActionType.Deploy && GameController.Instance.matPlayer.SuitablePlaceForMechCheck());
							if (this.player.matPlayer.GetPlayerMatSection(base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID).ActionDown.IsMaxReached())
							{
								context.SetMaxReached(true);
							}
							else if (!flag3)
							{
								context.SetNoSuitablePlace(true);
							}
						}
					}
					if (interactable)
					{
						TextMeshProUGUI textMeshProUGUI;
						if (PlatformManager.IsMobile)
						{
							textMeshProUGUI = context.ConfirmButton.GetComponentInChildren<TextMeshProUGUI>();
							context.ConfirmButton.onClick.RemoveAllListeners();
							context.ConfirmButton.onClick.AddListener(delegate
							{
								this.StartAction();
							});
							context.ConfirmButton.onClick.AddListener(delegate
							{
								context.ClearCloseEvent();
							});
							context.OnClose += delegate
							{
								this.gainActionButtonBackground[0].sprite = (this.isFactoryAction ? GameController.Instance.matPlayer.factoryActiveAction : GameController.Instance.matPlayer.defaultAction);
							};
							context.ConfirmButton.gameObject.SetActive(true);
						}
						else
						{
							textMeshProUGUI = GameController.Instance.matPlayer.confirmActionSelection.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
						}
						textMeshProUGUI.text = ScriptLocalization.Get(flag2 ? "GameScene/Confirm" : "GameScene/EndTurn");
					}
					else if (PlatformManager.IsMobile)
					{
						context.ConfirmButton.gameObject.SetActive(false);
					}
				}
				this.hintAction.SetActive(true);
				return;
			}
			if (interactable)
			{
				this.StartAction();
			}
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000DC0C0 File Offset: 0x000DA2C0
		public Transform GetUprgradePlaceForAnimation()
		{
			for (int i = 0; i < this.actionCost.Length; i++)
			{
				if (this.actionCost[i].transform.GetComponent<Image>().sprite.name != "RedActionButton_out")
				{
					return this.actionCost[i].transform;
				}
			}
			return this.actionCost[0].transform;
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000DC124 File Offset: 0x000DA324
		private void SuitablePlaceForActionWarning(MatPlayerPresenter mat, int sectionId)
		{
			if (this.actionType == DownActionType.Deploy && !mat.SuitablePlaceForMechCheck())
			{
				mat.noSuitablePlace.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
				{
					this.SelectAction(mat, sectionId);
				}, null);
			}
			if (this.actionType == DownActionType.Build && !mat.SuitablePlaceForBuildCheck())
			{
				mat.noSuitablePlace.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
				{
					this.SelectAction(mat, sectionId);
				}, null);
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x0003F78D File Offset: 0x0003D98D
		public void HideGuidline()
		{
			if (this.guidelinePay != null)
			{
				this.guidelinePay.SetActive(false);
			}
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000DC1F0 File Offset: 0x000DA3F0
		private DownAction GetAction()
		{
			MatPlayerSectionPresenter componentInParent = base.GetComponentInParent<MatPlayerSectionPresenter>();
			if (!(componentInParent != null) || this.player == null)
			{
				return null;
			}
			int sectionID = componentInParent.sectionID;
			MatPlayerSection playerMatSection = this.player.matPlayer.GetPlayerMatSection(sectionID);
			if (playerMatSection != null)
			{
				return playerMatSection.ActionDown;
			}
			return null;
		}

		// Token: 0x040019CD RID: 6605
		public DownActionType actionType;

		// Token: 0x040019CE RID: 6606
		public Image[] actionGain;

		// Token: 0x040019CF RID: 6607
		public Image actionUpgradableSign;

		// Token: 0x040019D0 RID: 6608
		public GameObject hintAction;

		// Token: 0x040019D1 RID: 6609
		public GameObject maxReachedBadge;

		// Token: 0x040019D2 RID: 6610
		public GameObject noSuitablePlaceBadge;

		// Token: 0x040019D3 RID: 6611
		public Image recruit;

		// Token: 0x040019D4 RID: 6612
		public Image recruitBonus;

		// Token: 0x040019D5 RID: 6613
		public Image recruitBackground;

		// Token: 0x040019D6 RID: 6614
		public Image guideline;

		// Token: 0x040019D7 RID: 6615
		public GameObject enlistObject;

		// Token: 0x040019D8 RID: 6616
		public Button recruitButton;

		// Token: 0x040019D9 RID: 6617
		public Image recruitNeighbourLeft;

		// Token: 0x040019DA RID: 6618
		public Image recruitNeighbourRight;

		// Token: 0x040019DB RID: 6619
		public Sprite CostInactiveBackground;

		// Token: 0x040019DC RID: 6620
		public Sprite CostActiveBackground;

		// Token: 0x040019DD RID: 6621
		public Sprite GainInactiveBackground;

		// Token: 0x040019DE RID: 6622
		public Sprite GainActiveBackground;

		// Token: 0x040019DF RID: 6623
		public Material InactiveMaterial;

		// Token: 0x040019E0 RID: 6624
		public Material recruitNeighbourInactive;

		// Token: 0x040019E1 RID: 6625
		public GameObject guidelinePay;

		// Token: 0x040019E2 RID: 6626
		[FormerlySerializedAs("_actionCostsLayout")]
		public VerticalLayoutGroup actionCostsLayout;

		// Token: 0x040019E3 RID: 6627
		public Transform defaultCostLayout;

		// Token: 0x040019E4 RID: 6628
		public Transform upgradeableCostLayout;

		// Token: 0x040019E5 RID: 6629
		public float actionCostsLayoutDefaultSpacing;

		// Token: 0x040019E6 RID: 6630
		public float actionCostsLayoutUpscaledSpacing;

		// Token: 0x040019E7 RID: 6631
		public Vector2 costIconDefaultSize;

		// Token: 0x040019E8 RID: 6632
		public Vector2 costIconUpscaledSize;

		// Token: 0x040019E9 RID: 6633
		public Color mainGain;
	}
}
