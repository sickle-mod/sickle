using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200048C RID: 1164
	public class PlayerTopActionPresenter : PlayerActionPresenter
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x0003F86B File Offset: 0x0003DA6B
		private GameObject[] maxReachedBadges
		{
			get
			{
				return new GameObject[] { this.maxReachedBadge1, this.maxReachedBadge2 };
			}
		}

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06002505 RID: 9477 RVA: 0x000DC36C File Offset: 0x000DA56C
		// (remove) Token: 0x06002506 RID: 9478 RVA: 0x000DC3A0 File Offset: 0x000DA5A0
		public static event PlayerTopActionPresenter.InstantActionEnd InstantActionEnded;

		// Token: 0x06002507 RID: 9479 RVA: 0x0003F885 File Offset: 0x0003DA85
		public void Init(Player player, int actionCosts, int actionGains1, int actionGains2, bool structureBuilded)
		{
			this.player = player;
			this.Reset();
			this.UpdateSection(player, actionCosts, actionGains1, actionGains2, structureBuilded);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000DC3D4 File Offset: 0x000DA5D4
		public void UpdateSection(Player player, int actionCosts, int actionGains1, int actionGains2, bool structureBuilt)
		{
			this.player = player;
			if (this.actionType != TopActionType.Factory)
			{
				this.UpdateWorkers(actionCosts);
				if (PlatformManager.IsStandalone)
				{
					if (this.actionGain1.Length != 0)
					{
						Transform child = this.actionGain1[this.actionGain1.Length - 1].transform.GetChild(this.actionGain1[this.actionGain1.Length - 1].transform.childCount - 1);
						child.GetComponent<Image>().enabled = this.actionGain1.Length > actionGains1;
						child.GetChild(0).GetComponent<Image>().enabled = false;
					}
					if (this.actionGain2.Length != 0)
					{
						Transform child2 = this.actionGain2[this.actionGain2.Length - 1].transform.GetChild(this.actionGain2[this.actionGain2.Length - 1].transform.childCount - 1);
						child2.GetComponent<Image>().enabled = this.actionGain2.Length > actionGains2;
						child2.GetChild(0).GetComponent<Image>().enabled = false;
					}
				}
				else
				{
					if (this.actionGain1.Length != 0 && this.upgradeAvailableIcons.Length != 0 && this.upgradeAvailableIcons[0] != null)
					{
						this.upgradeAvailableIcons[0].enabled = this.actionGain1.Length > actionGains1;
					}
					if (this.actionGain2.Length != 0 && this.upgradeAvailableIcons.Length > 1 && this.upgradeAvailableIcons[1] != null)
					{
						this.upgradeAvailableIcons[1].enabled = this.actionGain2.Length > actionGains2;
					}
					TopAction action = this.GetAction();
					if (action != null)
					{
						base.UpdateActionCostOpacity(action);
						base.UpdateActionCostOverlay(action);
					}
				}
				for (int i = 0; i < actionGains1; i++)
				{
					this.actionGain1[i].sprite = this.GainActiveBackground;
					this.actionGain1[i].transform.GetChild(0).GetComponent<Image>().material = null;
				}
				for (int j = 0; j < actionGains2; j++)
				{
					if (this.actionGain2.Length != 0)
					{
						this.actionGain2[j].sprite = this.GainActiveBackground;
						this.actionGain2[j].transform.GetChild(0).GetComponent<Image>().material = null;
					}
				}
				if (structureBuilt)
				{
					this.structure.color = Color.white;
					this.buildingSpecific.material = null;
					if (GameController.Instance != null)
					{
						this.buildingBackground.sprite = GameController.Instance.matPlayer.bonusUnlocked;
					}
				}
				else
				{
					this.structure.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
					this.buildingSpecific.material = this.GainInactiveMaterial;
					if (GameController.Instance != null)
					{
						this.buildingBackground.sprite = GameController.Instance.matPlayer.bonusDefault;
					}
				}
			}
			else
			{
				for (int k = 0; k < this.actionCostButton.Length; k++)
				{
					if (PlatformManager.IsStandalone)
					{
						this.actionCost[k].gameObject.GetComponent<Image>().enabled = true;
					}
					else
					{
						TopAction action2 = this.GetAction();
						if (action2 != null)
						{
							base.UpdateActionCostOpacity(action2);
							base.UpdateActionCostOverlay(this.GetAction());
						}
					}
					this.actionCostButton[k].gameObject.SetActive(false);
					this.actionCostButton[k].interactable = true;
				}
			}
			if (this.hintAction1 != null)
			{
				ActionContextWindow component = this.hintAction1.GetComponent<ActionContextWindow>();
				this.hintAction1.SetActive(false);
				if (component != null)
				{
					component.PayAmount = actionCosts;
					component.GainAmount = actionGains1;
				}
			}
			if (this.hintAction2 != null)
			{
				ActionContextWindow component2 = this.hintAction2.GetComponent<ActionContextWindow>();
				this.hintAction2.SetActive(false);
				if (component2 != null)
				{
					component2.PayAmount = actionCosts;
					component2.GainAmount = actionGains2;
				}
			}
			if (this.maxReachedBadge1 != null && this.maxReachedBadge2 != null)
			{
				if (this.actionType == TopActionType.Factory && player != null)
				{
					for (int l = 0; l < this.maxReachedBadges.Length; l++)
					{
						if (l >= this.maxReachedBadges.Length || l >= player.matPlayer.GetTopAction(this.actionType).GetNumberOfGainActions())
						{
							return;
						}
						this.maxReachedBadges[l].SetActive(player.matPlayer.GetTopAction(this.actionType).GetGainAction(l).IsMaxReached());
					}
					return;
				}
				this.maxReachedBadge1.SetActive(false);
				this.maxReachedBadge2.SetActive(false);
			}
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000DC848 File Offset: 0x000DAA48
		public void UpdateSectionSpecial(int actionCosts, int actionGains1, int actionGains2, bool structureBuilt)
		{
			if (this.actionGain1.Length != 0)
			{
				Transform child = this.actionGain1[this.actionGain1.Length - 1].transform.GetChild(this.actionGain1[this.actionGain1.Length - 1].transform.childCount - 1);
				child.GetComponent<Image>().enabled = this.actionGain1.Length > actionGains1;
				child.GetChild(0).GetComponent<Image>().enabled = false;
			}
			if (this.actionGain2.Length != 0)
			{
				Transform child2 = this.actionGain2[this.actionGain2.Length - 1].transform.GetChild(this.actionGain2[this.actionGain2.Length - 1].transform.childCount - 1);
				child2.GetComponent<Image>().enabled = this.actionGain2.Length > actionGains2;
				child2.GetChild(0).GetComponent<Image>().enabled = false;
			}
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000DC928 File Offset: 0x000DAB28
		public void UpdateWorkers(int actionCosts)
		{
			if (this.actionType == TopActionType.Produce)
			{
				for (int i = 0; i < this.actionCost.Length; i++)
				{
					this.actionCost[i].transform.GetChild(2).GetComponent<Image>().enabled = i >= actionCosts;
				}
				for (int j = 0; j < 3; j++)
				{
					int num = j * 2 + 1;
					if (num >= actionCosts)
					{
						this.actionCost[num].sprite = this.CostInactiveBackground;
						this.actionCost[num].transform.GetChild(1).GetComponent<Image>().material = this.GainInactiveMaterial;
					}
					else
					{
						this.actionCost[num].sprite = this.CostActiveBackground;
						this.actionCost[num].transform.GetChild(1).GetComponent<Image>().material = null;
					}
				}
				if (this.hintAction1 != null)
				{
					ActionContextWindow component = this.hintAction1.GetComponent<ActionContextWindow>();
					if (component != null)
					{
						component.SetProductionCost(actionCosts);
					}
				}
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000DCA24 File Offset: 0x000DAC24
		protected override void Reset()
		{
			base.Reset();
			this.SelectAction(false, -1);
			this.ChangeModeToNormal();
			if (this.actionType != TopActionType.Factory)
			{
				for (int i = 0; i < this.actionGain1.Length; i++)
				{
					this.actionGain1[i].sprite = this.GainInactiveBackground;
					this.actionGain1[i].color = Color.white;
					this.actionGain1[i].transform.GetChild(0).GetComponent<Image>().material = this.GainInactiveMaterial;
					this.actionGain1[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
				}
				for (int j = 0; j < this.actionGain2.Length; j++)
				{
					this.actionGain2[j].sprite = this.GainInactiveBackground;
					this.actionGain2[j].color = Color.white;
					this.actionGain2[j].transform.GetChild(0).GetComponent<Image>().material = this.GainInactiveMaterial;
					this.actionGain2[j].transform.GetChild(0).GetComponent<Image>().color = Color.white;
				}
				for (int k = 0; k < this.actionCost.Length; k++)
				{
					this.actionCost[k].color = Color.white;
					this.actionCost[k].transform.GetChild(1).GetComponent<Image>().color = Color.white;
				}
				if (this.buildingSpecific != null)
				{
					this.buildingSpecific.color = Color.white;
				}
				this.buildingButton.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x000DCBC4 File Offset: 0x000DADC4
		public void AcquireGain(int index)
		{
			if (PlatformManager.IsStandalone)
			{
				if (this.selectedId == 0 || this.selectedId == -1)
				{
					if (this.actionGain1.Length > index && index != -1)
					{
						this.actionGain1[index].color = Color.gray;
						this.actionGain1[index].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
					}
				}
				else if (this.selectedId == 1 && this.actionGain2.Length > index)
				{
					this.actionGain2[index].color = Color.gray;
					this.actionGain2[index].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
				}
				if (this.actionType == TopActionType.Factory && this.actionCost.Length > index)
				{
					this.actionCost[index].color = Color.gray;
					this.actionCost[index].transform.GetChild(2).GetComponent<Image>().color = Color.gray;
				}
			}
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x0003F8A1 File Offset: 0x0003DAA1
		public void UseBuilding()
		{
			if (this.buildingSpecific != null)
			{
				this.buildingSpecific.color = Color.gray;
			}
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000DCCC4 File Offset: 0x000DAEC4
		public void FinishAction(bool all = false)
		{
			if (PlatformManager.IsStandalone)
			{
				if (this.selectedId == 0 || this.selectedId == -1 || all)
				{
					for (int i = 0; i < this.actionGain1.Length; i++)
					{
						this.actionGain1[i].color = Color.gray;
						this.actionGain1[i].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
					}
					if (this.actionType == TopActionType.Factory)
					{
						for (int j = 0; j < 3; j++)
						{
							this.actionCost[j].color = Color.gray;
							this.actionCost[j].transform.GetChild(2).GetComponent<Image>().color = Color.gray;
						}
					}
				}
				if (this.selectedId == 1 || all)
				{
					for (int k = 0; k < this.actionGain2.Length; k++)
					{
						this.actionGain2[k].color = Color.gray;
						this.actionGain2[k].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
					}
					if (this.actionType == TopActionType.Factory)
					{
						for (int l = 3; l < 6; l++)
						{
							this.actionCost[l].color = Color.gray;
							this.actionCost[l].transform.GetChild(2).GetComponent<Image>().color = Color.gray;
						}
					}
				}
			}
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000DCE28 File Offset: 0x000DB028
		public void ClickAction(int index = -1)
		{
			int sectionID = base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID;
			GameController.Instance.matPlayer.DisableAllDialogs();
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
			if (!this.actionEnabled[(index == -1) ? 0 : index])
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
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardInactiveScreenClick, AudioSourceType.Buttons);
				this.ShowHelp(index, false);
				return;
			}
			if (index != -1)
			{
				this.gainActionButtonBackground[index].sprite = GameController.Instance.matPlayer.activeAction;
			}
			else
			{
				this.gainActionButtonBackground[0].sprite = GameController.Instance.matPlayer.activeAction;
			}
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
						this.StartAction(index);
					});
					GameController.Instance.matPlayer.confirmActionSelection.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ScriptLocalization.Get("GameScene/Confirm");
				}
				this.ShowHelp(index, true);
				this.ActionClickSound(index);
				return;
			}
			this.StartAction(index);
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000DD100 File Offset: 0x000DB300
		private void ActionClickSound(int index)
		{
			switch (this.actionType)
			{
			case TopActionType.Bolster:
				if (index == 0)
				{
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBolsterPowerClick, AudioSourceType.Buttons);
					return;
				}
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBolsterAmmoClick, AudioSourceType.Buttons);
				return;
			case TopActionType.Trade:
				if (index == 0)
				{
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardTradeClick, AudioSourceType.Buttons);
					return;
				}
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardGainPopularityClick, AudioSourceType.Buttons);
				return;
			case TopActionType.Produce:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardProduceClick, AudioSourceType.Buttons);
				return;
			case TopActionType.MoveGain:
				if (index == 0)
				{
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardMoveClick, AudioSourceType.Buttons);
					return;
				}
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardEarnCashClic, AudioSourceType.Buttons);
				return;
			case TopActionType.Factory:
				WorldSFXManager.PlaySound(SoundEnum.FactoryActonClick, AudioSourceType.Buttons);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000DD180 File Offset: 0x000DB380
		public void StartAction(int index = -1)
		{
			this.notEnoughResourcesDialog.gameObject.SetActive(false);
			if ((index == -1 && this.actionEnabled[0]) || (index != -1 && this.actionEnabled[index]))
			{
				int sectionId = base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID;
				MatPlayerPresenter matPlayer = GameController.Instance.matPlayer;
				bool flag = this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.CanPlayerPayActions();
				if (matPlayer.topActionAvailable != null)
				{
					matPlayer.topActionAvailable.Hide();
				}
				if (flag)
				{
					if (this.IsThereNoSuitablePlaceForAction(matPlayer, sectionId, index))
					{
						this.SuitablePlaceForActionWarning(matPlayer, sectionId, index);
					}
					else if (!this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.CanPlayerGainFromActions(index) && OptionsManager.IsWarningsActive())
					{
						if (this.actionType == TopActionType.Factory)
						{
							this.MaxedOutWarning(matPlayer, sectionId, index);
						}
						else
						{
							matPlayer.maxResources.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
							{
								this.StartAction(sectionId, index);
							}, null);
						}
					}
					else if (this.NoSuitablePlaceForProduce())
					{
						this.MaxedOutWarning(matPlayer, sectionId, index);
					}
					else
					{
						this.StartAction(sectionId, index);
					}
				}
				else
				{
					Debug.Log("HAY");
					this.notEnoughResourcesDialog.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
					{
						this.StartAction(sectionId, index);
					}, null);
				}
			}
			else
			{
				this.ShowHelp(index, true);
			}
			if (PlayerTopActionPresenter.InstantActionEnded != null)
			{
				PlayerTopActionPresenter.InstantActionEnded(this.actionType);
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000DD390 File Offset: 0x000DB590
		private bool IsThereNoSuitablePlaceForAction(MatPlayerPresenter mat, int sectionId, int index)
		{
			return (this.actionType == TopActionType.Produce && (!mat.SuitablePlaceForMechCheck() || !mat.SuitablePlaceForProduceCheck())) || (this.actionType == TopActionType.Trade && !mat.SuitablePlaceForTradeCheck() && index == 0) || (this.actionType == TopActionType.Factory && this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.CanPlayerGainBuildFromAction(index) && !mat.SuitablePlaceForBuildCheck()) || (this.actionType == TopActionType.Factory && this.player.matPlayer.GetPlayerMatSection(sectionId).ActionTop.CanPlayerGainWorkerFromAction(index) && !mat.SuitablePlaceForTradeWorkerCheck());
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000DD430 File Offset: 0x000DB630
		private void StartAction(int sectionId, int index)
		{
			GameController.Instance.matPlayer.ShowSectionMatActionSelection(false);
			GameController.Instance.matPlayer.MatPlayerSectionSelected(sectionId);
			GameController.Instance.matPlayer.StartTopAction(index);
			if (this.player.topActionFinished)
			{
				base.ResetButtonsBackgrounds();
			}
			if (this.actionType == TopActionType.MoveGain && index != 0)
			{
				GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
			}
			switch (this.actionType)
			{
			case TopActionType.Bolster:
				if (index == 0)
				{
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBolsterPowerConfrm, AudioSourceType.Buttons);
					return;
				}
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBolsterAmmoConfrm, AudioSourceType.Buttons);
				return;
			case TopActionType.Trade:
				if (index == 0)
				{
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardTradeConfrm, AudioSourceType.Buttons);
					return;
				}
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardGainPopularityConfrm, AudioSourceType.Buttons);
				return;
			case TopActionType.Produce:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardProduceConfirm, AudioSourceType.Buttons);
				return;
			case TopActionType.MoveGain:
				if (index == 0)
				{
					WorldSFXManager.PlaySound(SoundEnum.PlayersBoardMoveConfirm, AudioSourceType.Buttons);
					return;
				}
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardEarnCashConfrm, AudioSourceType.Buttons);
				return;
			case TopActionType.Factory:
				WorldSFXManager.PlaySound(SoundEnum.FactoryActonChoose, AudioSourceType.Buttons);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000DD510 File Offset: 0x000DB710
		public void SelectAction(bool fade, int index = -1)
		{
			this.selectedId = index;
			if (PlatformManager.IsStandalone)
			{
				for (int i = 0; i < this.gainActionButton.Length; i++)
				{
					if (this.actionType != TopActionType.Produce)
					{
						if (i != index || index == -1)
						{
							foreach (Image image in this.gainActionButton[i].GetComponentsInChildren<Image>())
							{
								Color color = image.color;
								if (color != new Color(0f, 0f, 0f, 0f) && image.type != Image.Type.Sliced)
								{
									if (fade)
									{
										color.a = 0.5f;
									}
									else
									{
										color.a = 1f;
									}
									image.color = color;
								}
							}
						}
						else if (this.actionType != TopActionType.Factory && this.actionCost.Length > i)
						{
							this.actionCost[i].color = Color.gray;
							this.actionCost[i].transform.GetChild(1).GetComponent<Image>().color = Color.gray;
						}
					}
				}
				return;
			}
			Color color2 = new Color(1f, 1f, 1f, 0.9f);
			for (int k = 0; k < this.inactiveActionOverlays.Length; k++)
			{
				if (this.inactiveActionOverlays.Length > k && this.inactiveActionOverlays[k] != null)
				{
					base.ChangeInteractiveActionOverlayState(k, fade);
					base.ChangeInteractiveActionColor(k, color2);
				}
			}
			SectionAction action = this.GetAction();
			if (action != null)
			{
				base.UpdateActionCostOverlay(action);
			}
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000DD694 File Offset: 0x000DB894
		public void LightUpUpgradeMarkers(bool on, int id, bool selected, bool init = false)
		{
			Color color = new Color(1f, 1f, 1f, 1f);
			if (selected)
			{
				color = new Color(0.5f, 1f, 0f, 1f);
			}
			if (id == 0 || id == -1)
			{
				if (PlatformManager.IsStandalone)
				{
					Transform child = this.actionGain1[this.actionGain1.Length - 1].transform.GetChild(this.actionGain1[this.actionGain1.Length - 1].transform.childCount - 1);
					if (child.GetComponent<Image>().enabled)
					{
						if (on)
						{
							this.actionGain1[this.actionGain1.Length - 1].GetComponent<Image>().type = Image.Type.Simple;
							this.actionGain1[this.actionGain1.Length - 1].GetComponent<Image>().sprite = this.goldBackgroundMaterial;
							this.actionGain1[this.actionGain1.Length - 1].GetComponent<Image>().color = Color.white;
						}
						else
						{
							this.actionGain1[this.actionGain1.Length - 1].GetComponent<Image>().sprite = this.GainInactiveBackground;
							this.actionGain1[this.actionGain1.Length - 1].GetComponent<Image>().type = Image.Type.Simple;
						}
						child.GetChild(0).GetComponent<Image>().enabled = on;
						child.GetChild(0).GetComponent<Image>().color = color;
					}
				}
				if (!PlatformManager.IsStandalone)
				{
					Sprite sprite = (selected ? this.UpgradeSelectedSprite : this.UpgradeAvailableSprite);
					sprite = (init ? this.UpgradeAvailableDefault : sprite);
					this.gainActionButtonBackground[0].enabled = on;
					this.gainActionButtonBackground[0].sprite = sprite;
				}
			}
			if ((id == 1 || id == -1) && this.actionGain2.Length != 0)
			{
				if (PlatformManager.IsStandalone)
				{
					Transform child2 = this.actionGain2[this.actionGain2.Length - 1].transform.GetChild(this.actionGain2[this.actionGain2.Length - 1].transform.childCount - 1);
					if (child2.GetComponent<Image>().enabled)
					{
						if (on)
						{
							this.actionGain2[this.actionGain2.Length - 1].GetComponent<Image>().type = Image.Type.Simple;
							this.actionGain2[this.actionGain2.Length - 1].GetComponent<Image>().sprite = this.goldBackgroundMaterial;
							this.actionGain2[this.actionGain2.Length - 1].GetComponent<Image>().color = Color.white;
						}
						else
						{
							this.actionGain2[this.actionGain2.Length - 1].GetComponent<Image>().sprite = this.GainInactiveBackground;
							this.actionGain2[this.actionGain2.Length - 1].GetComponent<Image>().type = Image.Type.Simple;
						}
						child2.GetChild(0).GetComponent<Image>().enabled = on;
						child2.GetChild(0).GetComponent<Image>().color = color;
					}
				}
				if (!PlatformManager.IsStandalone)
				{
					Sprite sprite2 = (selected ? this.UpgradeSelectedSprite : this.UpgradeAvailableSprite);
					sprite2 = (init ? this.UpgradeAvailableDefault : sprite2);
					this.gainActionButtonBackground[1].enabled = on;
					this.gainActionButtonBackground[1].sprite = sprite2;
				}
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x0003F8C1 File Offset: 0x0003DAC1
		public override void ChangeModeToUpgrade(int index)
		{
			if (!PlatformManager.IsStandalone)
			{
				this.LightUpUpgradeMarkers(true, index, false, true);
			}
			else
			{
				this.LightUpUpgradeMarkers(true, -1, false, false);
			}
			base.ChangeModeToUpgrade(index);
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000DD9B4 File Offset: 0x000DBBB4
		public void ShowHelp(int index, bool interactable)
		{
			GameController.Instance.matFaction.ClearHintStories();
			GameObject gameObject = ((index != 1) ? this.hintAction1 : this.hintAction2);
			if (gameObject != null)
			{
				if (this.actionType == TopActionType.Factory)
				{
					string text = PlayerActionPresenter.CreateFactoryCardHintContent(this.player.matPlayer.GetTopAction(TopActionType.Factory), index);
					if (PlatformManager.IsStandalone)
					{
						gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
					}
					else
					{
						gameObject.GetComponent<ActionContextWindow>().SetDescription(text);
					}
				}
				ActionContextWindow context = gameObject.GetComponent<ActionContextWindow>();
				if (context != null)
				{
					bool flag = GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(this.player.lastMatSection, base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID);
					context.SetUsedLastTurn(flag);
					if (!flag)
					{
						int sectionID = base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID;
						bool flag2 = this.player.matPlayer.GetPlayerMatSection(sectionID).ActionTop.CanPlayerPayActions();
						context.SetCanAfford(flag2);
						if (flag2)
						{
							bool flag3 = this.player.matPlayer.GetPlayerMatSection(sectionID).ActionTop.CanPlayerGainFromActions((index == -1) ? 0 : index);
							bool flag4 = this.actionType == TopActionType.Bolster || this.actionType == TopActionType.Factory || this.actionType == TopActionType.MoveGain || (this.actionType == TopActionType.Produce && flag3) || (this.actionType == TopActionType.Trade && flag3);
							if (!flag3)
							{
								context.SetMaxReached(!flag3);
							}
							else if (!flag4)
							{
								context.SetNoSuitablePlace(true);
							}
						}
					}
					if (PlatformManager.IsMobile && context.ConfirmButton != null)
					{
						if (interactable)
						{
							context.ConfirmButton.gameObject.SetActive(true);
							context.ConfirmButton.onClick.RemoveAllListeners();
							context.ConfirmButton.onClick.AddListener(delegate
							{
								this.StartAction(index);
							});
							context.ConfirmButton.onClick.AddListener(delegate
							{
								context.ClearCloseEvent();
							});
							if (index == -1)
							{
								context.OnClose += delegate
								{
									this.gainActionButtonBackground[0].sprite = (this.isFactoryAction ? GameController.Instance.matPlayer.factoryActiveAction : GameController.Instance.matPlayer.defaultAction);
								};
							}
							else
							{
								context.OnClose += delegate
								{
									this.gainActionButtonBackground[index].sprite = (this.isFactoryAction ? GameController.Instance.matPlayer.factoryActiveAction : GameController.Instance.matPlayer.defaultAction);
								};
							}
						}
						else
						{
							context.ConfirmButton.gameObject.SetActive(false);
						}
					}
				}
				gameObject.SetActive(true);
				return;
			}
			if (interactable)
			{
				this.StartAction(index);
			}
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x000DDC88 File Offset: 0x000DBE88
		public void ShowBuildingHelp()
		{
			switch (this.actionType)
			{
			case TopActionType.Bolster:
				GameController.Instance.matFaction.ShowBuildingHint(BuildingType.Monument, base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID, false);
				return;
			case TopActionType.Trade:
				GameController.Instance.matFaction.ShowBuildingHint(BuildingType.Armory, base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID, false);
				return;
			case TopActionType.Produce:
				GameController.Instance.matFaction.ShowBuildingHint(BuildingType.Mill, base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID, false);
				return;
			case TopActionType.MoveGain:
				GameController.Instance.matFaction.ShowBuildingHint(BuildingType.Mine, base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000DDD28 File Offset: 0x000DBF28
		private void MaxedOutWarning(MatPlayerPresenter mat, int sectionId, int index)
		{
			Debug.Log("hay");
			if (OptionsManager.IsWarningsActive())
			{
				mat.maxedOutWarning.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
				{
					mat.MatPlayerSectionSelected(sectionId);
					mat.StartTopAction(index);
				}, null);
				return;
			}
			mat.MatPlayerSectionSelected(sectionId);
			mat.StartTopAction(index);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000DDDC0 File Offset: 0x000DBFC0
		public bool NoSuitablePlaceForProduce()
		{
			int sectionID = base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID;
			MatPlayerPresenter matPlayer = GameController.Instance.matPlayer;
			return this.actionType == TopActionType.Produce && !this.player.matPlayer.GetPlayerMatSection(sectionID).ActionTop.CanPlayerGainWorkerFromActions() && !matPlayer.SuitablePlaceForProduceCheck() && OptionsManager.IsWarningsActive() && this.player.matPlayer.workers.Count == 8;
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000DDE34 File Offset: 0x000DC034
		public bool SuitablePlaceForProduceUI()
		{
			int sectionID = base.GetComponentInParent<MatPlayerSectionPresenter>().sectionID;
			MatPlayerPresenter matPlayer = GameController.Instance.matPlayer;
			return !this.player.matPlayer.GetPlayerMatSection(sectionID).ActionTop.CanPlayerGainWorkerFromActions() && !matPlayer.SuitablePlaceForProduceCheck() && this.player.matPlayer.workers.Count == 8;
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000DDE98 File Offset: 0x000DC098
		private void SuitablePlaceForActionWarning(MatPlayerPresenter mat, int sectionId, int index)
		{
			if (OptionsManager.IsWarningsActive())
			{
				mat.noSuitablePlace.Show(GameController.factionInfo[this.player.matFaction.faction].logo, delegate
				{
					mat.MatPlayerSectionSelected(sectionId);
					mat.StartTopAction(index);
					if (this.player.topActionFinished)
					{
						this.ResetButtonsBackgrounds();
					}
				}, null);
				return;
			}
			mat.MatPlayerSectionSelected(sectionId);
			mat.StartTopAction(index);
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x0003F8E7 File Offset: 0x0003DAE7
		public void ShowActionLabel(bool show)
		{
			this.actionLabel.SetActive(show);
			if (show)
			{
				this.actionLabel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
			}
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000DDF30 File Offset: 0x000DC130
		private TopAction GetAction()
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
				return playerMatSection.ActionTop;
			}
			return null;
		}

		// Token: 0x040019F4 RID: 6644
		public TopActionType actionType;

		// Token: 0x040019F5 RID: 6645
		public Image[] actionGain1;

		// Token: 0x040019F6 RID: 6646
		public Image[] actionGain2;

		// Token: 0x040019F7 RID: 6647
		public GameObject hintAction1;

		// Token: 0x040019F8 RID: 6648
		public GameObject hintAction2;

		// Token: 0x040019F9 RID: 6649
		public GameObject building;

		// Token: 0x040019FA RID: 6650
		public GameObject maxReachedBadge1;

		// Token: 0x040019FB RID: 6651
		public GameObject maxReachedBadge2;

		// Token: 0x040019FC RID: 6652
		public Image structure;

		// Token: 0x040019FD RID: 6653
		public Image buildingSpecific;

		// Token: 0x040019FE RID: 6654
		public Image buildingBackground;

		// Token: 0x040019FF RID: 6655
		public Image guideline;

		// Token: 0x04001A00 RID: 6656
		public Button buildingButton;

		// Token: 0x04001A01 RID: 6657
		[SerializeField]
		private GameObject actionLabel;

		// Token: 0x04001A02 RID: 6658
		[SerializeField]
		private Image differentGainImage;

		// Token: 0x04001A03 RID: 6659
		[SerializeField]
		private Sprite GainInactiveBackground;

		// Token: 0x04001A04 RID: 6660
		[SerializeField]
		private Sprite GainActiveBackground;

		// Token: 0x04001A05 RID: 6661
		[SerializeField]
		private Sprite CostActiveBackground;

		// Token: 0x04001A06 RID: 6662
		[SerializeField]
		private Sprite CostInactiveBackground;

		// Token: 0x04001A07 RID: 6663
		[SerializeField]
		private Sprite UpgradeAvailableSprite;

		// Token: 0x04001A08 RID: 6664
		[SerializeField]
		private Sprite UpgradeSelectedSprite;

		// Token: 0x04001A09 RID: 6665
		[SerializeField]
		private Sprite UpgradeAvailableDefault;

		// Token: 0x04001A0A RID: 6666
		[SerializeField]
		public Material GainInactiveMaterial;

		// Token: 0x04001A0C RID: 6668
		private int selectedId = -1;

		// Token: 0x0200048D RID: 1165
		// (Invoke) Token: 0x06002521 RID: 9505
		public delegate void InstantActionEnd(TopActionType actionTypeEnded);
	}
}
