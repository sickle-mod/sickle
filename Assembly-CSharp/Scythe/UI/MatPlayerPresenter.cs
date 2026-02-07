using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200047E RID: 1150
	public class MatPlayerPresenter : MonoBehaviour, IActionProxy
	{
		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06002474 RID: 9332 RVA: 0x000D80B4 File Offset: 0x000D62B4
		// (remove) Token: 0x06002475 RID: 9333 RVA: 0x000D80E8 File Offset: 0x000D62E8
		public static event MatPlayerPresenter.EndTurnAvailable OnEndTurnAvailable;

		// Token: 0x06002476 RID: 9334 RVA: 0x000D811C File Offset: 0x000D631C
		public void LateUpdate()
		{
			if (PlatformManager.IsMobile)
			{
				if (this.isMatPreview && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
				{
					this.matchHeightCanvas.enabled = true;
					this.isMatPreview = false;
					SingletonMono<BottomBar>.Instance.RestorePreviousState();
				}
				if (this.isMatPreview && Application.isEditor && Input.GetMouseButtonDown(0))
				{
					this.matchHeightCanvas.enabled = true;
					this.isMatPreview = false;
					SingletonMono<BottomBar>.Instance.RestorePreviousState();
				}
			}
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000D81A4 File Offset: 0x000D63A4
		public void UpdateMat(Player player, bool update = false)
		{
			this.player = player;
			if (!update)
			{
				if (this.matSection.Count == 5)
				{
					this.matSection.RemoveAt(4);
				}
				if (player.matPlayer.GetPlayerMatSection(4) == null)
				{
					this.factoryCardImage.SetActive(true);
					this.factoryCardSlot.SetActive(false);
				}
				else
				{
					this.factoryCardImage.SetActive(false);
					this.factoryCardSlot.SetActive(true);
					this.matSection.Add(GameController.Instance.factoryCardPresenter.GenerateSectionPresenter(this.factoryCardSlot, player.matPlayer.GetPlayerMatSection(4) as FactoryCard));
				}
			}
			for (int i = 0; i < this.matSection.Count; i++)
			{
				MatPlayerSection s = player.matPlayer.GetPlayerMatSection(i);
				if (update)
				{
					this.matSection[i].UpdateSection(s.ActionTop, s.ActionDown, player.matPlayer.workers.Count - 2, update);
				}
				else
				{
					int sectionID = this.matSection.SingleOrDefault((MatPlayerSectionPresenter section) => section.topActionPresenter.actionType == s.ActionTop.Type).sectionID;
					if (sectionID != i && sectionID < this.matSection.Count)
					{
						this.matSection[i].SwapTopAction(this.matSection[sectionID]);
					}
					this.matSection[i].Init(s.ActionTop, s.ActionDown, player, this.isPreview);
				}
			}
			this.factionLogo.sprite = GameController.factionInfo[player.matFaction.faction].logo;
			if (player.downActionFinished)
			{
				this.ShowSectionMatActionSelection(false);
			}
			if (GameController.GameManager.GameFinished)
			{
				for (int j = 0; j < this.matSection.Count; j++)
				{
					this.matSection[j].DisableActionsAndSection(0, true);
				}
			}
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000D83A0 File Offset: 0x000D65A0
		public void UpdateMat(MatPlayer matPlayer)
		{
			for (int i = 0; i < this.matSection.Count; i++)
			{
				MatPlayerSection s = matPlayer.GetPlayerMatSection(i);
				if (PlatformManager.IsStandalone)
				{
					this.matSection[i].UpdateSection(s.ActionTop, s.ActionDown);
				}
				int sectionID = this.matSection.SingleOrDefault((MatPlayerSectionPresenter section) => section.topActionPresenter.actionType == s.ActionTop.Type).sectionID;
				if (sectionID != i && sectionID < this.matSection.Count)
				{
					this.matSection[i].SwapTopAction(this.matSection[sectionID]);
				}
				if (!PlatformManager.IsStandalone)
				{
					this.matSection[i].UpdateSection(s.ActionTop, s.ActionDown, 0, false);
				}
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000D8488 File Offset: 0x000D6688
		public void UpdateRecruits(Player player)
		{
			for (int i = 0; i < this.matSection.Count; i++)
			{
				this.matSection[i].downActionPresenter.UpdateEnemyRecruits(player);
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000D84C4 File Offset: 0x000D66C4
		public void SectionActionSelected(SectionAction action, int actionId = -1)
		{
			if (GameController.GameManager.LastEncounterCard == null)
			{
				GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
			}
			this.matSection[this.player.currentMatSection].DisableActionsAndSection(1, true);
			if (!GameController.Instance.AdjustingPresenters)
			{
				GameController.GameManager.actionManager.SetSectionAction(action, this, actionId);
			}
			this.ShowSectionMatActionSelection(false);
			if (PlatformManager.IsMobile && action.GetGainAction(0) is GainMove)
			{
				GameController.Instance.undoController.TriggerUndoInteractivityChange(false);
			}
			if (this.player.topActionFinished && !this.player.ActionInProgress)
			{
				GameController.GameManager.CheckObjectiveCards();
				return;
			}
			if (PlatformManager.IsStandalone)
			{
				GameController.Instance.panelInfo.FocusAllObjectiveCards(false);
				return;
			}
			SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().FocusAllObjectiveCards(false);
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000D85A4 File Offset: 0x000D67A4
		public void SectionActionFinished()
		{
			if (this.player == null)
			{
				return;
			}
			MatPlayerSectionPresenter matPlayerSectionPresenter = this.matSection[this.player.currentMatSection];
			if (PlatformManager.IsStandalone)
			{
				GameController.Instance.MinimizePlayerMat();
			}
			if (this.player.topActionFinished && !this.player.downActionFinished)
			{
				matPlayerSectionPresenter.topActionPresenter.GetComponent<Image>().sprite = matPlayerSectionPresenter.actionDefault;
				matPlayerSectionPresenter.topActionPresenter.FinishAction(false);
				Image[] array = matPlayerSectionPresenter.topActionPresenter.gainActionButtonBackground;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = (matPlayerSectionPresenter.topActionPresenter.isFactoryAction ? this.factoryActiveAction : this.defaultAction);
				}
				matPlayerSectionPresenter.CheckDownAction();
			}
			else
			{
				matPlayerSectionPresenter.downActionPresenter.GetComponent<Image>().sprite = matPlayerSectionPresenter.actionDefault;
				matPlayerSectionPresenter.downActionPresenter.FinishAction(matPlayerSectionPresenter);
				Image[] array = matPlayerSectionPresenter.downActionPresenter.gainActionButtonBackground;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = (matPlayerSectionPresenter.downActionPresenter.isFactoryAction ? this.factoryActiveAction : this.defaultAction);
				}
				matPlayerSectionPresenter.NothingToDo();
			}
			MatPlayerSection playerMatSection = this.player.matPlayer.GetPlayerMatSection(this.player.currentMatSection);
			GameController.Instance.undoController.TriggerUndoInteractivityChange(true);
			if ((this.player.topActionFinished && !this.player.downActionFinished && playerMatSection.ActionTop.Type == TopActionType.MoveGain) || (this.player.downActionFinished && playerMatSection.ActionDown.Type == DownActionType.Factory))
			{
				GameController.Instance.UpdateStats(true, false);
			}
			else
			{
				if (PlatformManager.IsMobile && !this.player.downActionFinished)
				{
					matPlayerSectionPresenter.downActionPresenter.SelectAction(false);
				}
				GameController.Instance.UpdateStats(true, true);
			}
			GameController.Instance.matFaction.ClearHintStories();
			if ((this.player.topActionFinished && !this.player.downActionFinished && playerMatSection.ActionTop.Type == TopActionType.MoveGain) || (this.player.downActionFinished && playerMatSection.ActionDown.Type == DownActionType.Factory))
			{
				GameController.Instance.gameBoardPresenter.UpdateBoard(false, false);
			}
			else
			{
				GameController.Instance.gameBoardPresenter.UpdateBoard(true, true);
			}
			this.ResetActionSectionButtonsBackgrounds();
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000D87F0 File Offset: 0x000D69F0
		public void AfterLoadUpdate()
		{
			if (this.player.currentMatSection != -1)
			{
				for (int i = 0; i < this.matSection.Count; i++)
				{
					if (i != this.player.currentMatSection)
					{
						this.matSection[i].DisableActionsAndSection(0, true);
						this.matSection[i].SetSectionCooldown(true, false);
					}
					else
					{
						this.matSection[i].AfterLoadUpdate();
					}
				}
			}
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000D8868 File Offset: 0x000D6A68
		public void StartTopAction(int index = -1)
		{
			MatPlayerSectionPresenter matPlayerSectionPresenter = this.matSection[this.player.currentMatSection];
			TopAction actionTop = this.player.matPlayer.GetPlayerMatSection(this.player.currentMatSection).ActionTop;
			if (actionTop.CanPlayerPayActions())
			{
				if (!actionTop.DifferentGain)
				{
					index = -1;
				}
				this.SectionActionSelected(actionTop, index);
				this.SelectTopAction(matPlayerSectionPresenter, index);
				return;
			}
			matPlayerSectionPresenter.OnMatSectionSelected();
			this.SectionActionFinished();
			matPlayerSectionPresenter.topActionPresenter.DisableAllButtons(false);
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000D88EC File Offset: 0x000D6AEC
		public void SelectTopAction(MatPlayerSectionPresenter section, int index = -1)
		{
			section.topActionPresenter.SelectAction(true, index);
			GameObject gameObject = ((index != 1) ? section.topActionPresenter.hintAction1 : section.topActionPresenter.hintAction2);
			if (gameObject != null)
			{
				ActionContextWindow component = gameObject.GetComponent<ActionContextWindow>();
				section.topActionPresenter.gainActionButtonBackground[(index != 1) ? 0 : 1].sprite = this.activeAction;
				if (PlatformManager.IsMobile && component != null)
				{
					component.OnClose += delegate
					{
						section.topActionPresenter.gainActionButtonBackground[(index != 1) ? 0 : 1].sprite = this.defaultAction;
					};
				}
				if (component != null && component.Instruction != null)
				{
					component.Instruction.SetActive(true);
				}
				if (component != null && component.Description != null)
				{
					component.Description.SetActive(false);
				}
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000D89FC File Offset: 0x000D6BFC
		public void StartBottomAction()
		{
			MatPlayerSectionPresenter matPlayerSectionPresenter = this.matSection[this.player.currentMatSection];
			DownAction actionDown = this.player.matPlayer.GetPlayerMatSection(this.player.currentMatSection).ActionDown;
			if (actionDown.CanPlayerPayActions())
			{
				this.SectionActionSelected(actionDown, -1);
				this.SelectBottomAction(matPlayerSectionPresenter);
				return;
			}
			this.SectionActionFinished();
			matPlayerSectionPresenter.topActionPresenter.DisableAllButtons(false);
			matPlayerSectionPresenter.downActionPresenter.DisableAllButtons(false);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000D8A78 File Offset: 0x000D6C78
		public void SelectBottomAction(MatPlayerSectionPresenter section)
		{
			if (PlatformManager.IsMobile)
			{
				section.topActionPresenter.SelectAction(true, -1);
				section.downActionPresenter.SelectAction(true);
			}
			section.topActionPresenter.GetComponent<Image>().sprite = section.actionDefault;
			section.topActionPresenter.DisableAllButtons(false);
			section.topActionPresenter.FinishAction(true);
			section.downActionPresenter.LightUp(section.colorActive, true);
			section.downActionPresenter.gainActionButtonBackground[0].sprite = this.activeAction;
			ActionContextWindow component = section.downActionPresenter.hintAction.GetComponent<ActionContextWindow>();
			if (component != null && component.Instruction != null)
			{
				component.Instruction.SetActive(true);
			}
			if (component != null && component.Description != null)
			{
				component.Description.SetActive(true);
			}
			if (PlatformManager.IsMobile && component != null)
			{
				component.OnClose += delegate
				{
					section.downActionPresenter.gainActionButtonBackground[0].sprite = this.defaultAction;
				};
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000D8BBC File Offset: 0x000D6DBC
		public void MatPlayerSectionSelected(int sectionID)
		{
			if (!GameController.Instance.AdjustingPresenters)
			{
				GameController.GameManager.ChooseSection(sectionID);
			}
			for (int i = 0; i < this.matSection.Count; i++)
			{
				if (i != sectionID)
				{
					this.matSection[i].SetSectionCooldown(true, false);
					this.matSection[i].DisableActionsAndSection(0, false);
					this.matSection[i].sectionGlass.enabled = false;
				}
			}
			this.matSection[sectionID].OnMatSectionSelected();
			GameController.Instance.EndTurnButtonEnable();
			if (MatPlayerPresenter.OnEndTurnAvailable != null)
			{
				MatPlayerPresenter.OnEndTurnAvailable();
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000D8C64 File Offset: 0x000D6E64
		public bool SuitablePlaceForMechCheck()
		{
			bool flag = true;
			int num = 0;
			int num2 = 0;
			foreach (Worker worker in this.player.matPlayer.workers)
			{
				if (worker.position.hexType == HexType.capital)
				{
					num++;
				}
				else if (worker.position.hexType == HexType.lake)
				{
					num2++;
				}
			}
			if (num + num2 == this.player.matPlayer.workers.Count)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000D8D08 File Offset: 0x000D6F08
		public bool SuitablePlaceForTradeCheck()
		{
			foreach (Worker worker in this.player.matPlayer.workers)
			{
				if (worker.position.hexType != HexType.capital && worker.position.hexType != HexType.lake)
				{
					return true;
				}
				if (worker.position.hexType == HexType.lake)
				{
					using (List<Mech>.Enumerator enumerator2 = this.player.matFaction.mechs.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.position == worker.position)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000D8DF0 File Offset: 0x000D6FF0
		public bool SuitablePlaceForTradeWorkerCheck()
		{
			bool flag = true;
			int num = 0;
			if (this.player.matFaction.mechs.Count == 0)
			{
				return false;
			}
			using (List<Mech>.Enumerator enumerator = this.player.matFaction.mechs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == this.player.GetCapital())
					{
						num++;
					}
				}
			}
			if (num == this.player.matFaction.mechs.Count)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000D8E94 File Offset: 0x000D7094
		public bool SuitablePlaceForBuildCheck()
		{
			foreach (Worker worker in this.player.matPlayer.workers)
			{
				if (worker.position.Building == null && worker.position.hexType != HexType.lake && worker.position.hexType != HexType.capital)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000D8F1C File Offset: 0x000D711C
		public bool SuitablePlaceForProduceCheck()
		{
			foreach (Worker worker in this.player.matPlayer.workers)
			{
				if (worker.position.hexType != HexType.factory && worker.position.hexType != HexType.lake && worker.position.hexType != HexType.capital)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000D8FA8 File Offset: 0x000D71A8
		public void ResetActionSectionButtonsBackgrounds()
		{
			for (int i = 0; i < this.matSection.Count; i++)
			{
				this.matSection[i].ResetActionButtonsBackgrounds();
			}
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000D8FDC File Offset: 0x000D71DC
		public void DisableHints()
		{
			if (PlatformManager.IsMobile)
			{
				using (List<MatPlayerSectionPresenter>.Enumerator enumerator = this.matSection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MatPlayerSectionPresenter matPlayerSectionPresenter = enumerator.Current;
						if (matPlayerSectionPresenter.topActionPresenter.hintAction1 != null)
						{
							matPlayerSectionPresenter.topActionPresenter.hintAction1.SetActive(false);
						}
						if (matPlayerSectionPresenter.topActionPresenter.hintAction2 != null)
						{
							matPlayerSectionPresenter.topActionPresenter.hintAction2.SetActive(false);
						}
						if (matPlayerSectionPresenter.downActionPresenter.hintAction != null)
						{
							matPlayerSectionPresenter.downActionPresenter.hintAction.SetActive(false);
						}
					}
					return;
				}
			}
			foreach (object obj in this.context.gameObject.transform.Find("Hints"))
			{
				((Transform)obj).gameObject.SetActive(false);
			}
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x0003F3E7 File Offset: 0x0003D5E7
		public void DisableAllDialogs()
		{
			this.topActionAvailable.Hide();
			this.maxResources.Hide();
			this.noSuitablePlace.Hide();
			this.maxedOutWarning.Hide();
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000D9100 File Offset: 0x000D7300
		public void ShowActionLabels(bool show)
		{
			for (int i = 0; i < 4; i++)
			{
				this.matSection[i].topActionPresenter.ShowActionLabel(show);
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000D9130 File Offset: 0x000D7330
		public void ShowSectionMatActionReloading(bool show)
		{
			if (PlatformManager.IsStandalone)
			{
				this.contextMatActionReloading.SetActive(show);
				return;
			}
			if (this.player.lastMatSection > -1)
			{
				if (this.player.lastMatSection >= this.matSection.Count)
				{
					return;
				}
				this.matSection[this.player.lastMatSection].usedLastTurnMessage.SetActive(show);
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000D919C File Offset: 0x000D739C
		public void ShowSectionMatActionReloading(int sectionId, bool show)
		{
			if (this.matSection[sectionId].usedLastTurnMessage != null)
			{
				this.matSection[sectionId].usedLastTurnMessage.SetActive(show);
				return;
			}
			if (this.contextMatActionReloading != null)
			{
				this.contextMatActionReloading.SetActive(show);
			}
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0003F415 File Offset: 0x0003D615
		public void ShowSectionMatActionSelection(bool show)
		{
			if (this.contextMatActionSelection != null)
			{
				this.contextMatActionSelection.SetActive(show);
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0003F431 File Offset: 0x0003D631
		public void CloseHint()
		{
			if (PlatformManager.IsStandalone)
			{
				return;
			}
			this.ShowSectionMatActionSelection(false);
			this.ShowSectionMatActionReloading(false);
			this.DisableHints();
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0003F44F File Offset: 0x0003D64F
		public void MobileMatPreview()
		{
			this.matchHeightCanvas.enabled = false;
			this.isMatPreview = true;
			this.buttonPreviewHide.SetActive(true);
			SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Peeked, 0.25f);
		}

		// Token: 0x04001988 RID: 6536
		public List<MatPlayerSectionPresenter> matSection = new List<MatPlayerSectionPresenter>();

		// Token: 0x04001989 RID: 6537
		public GameObject factoryCardImage;

		// Token: 0x0400198A RID: 6538
		public GameObject factoryCardSlot;

		// Token: 0x0400198B RID: 6539
		public bool isPreview;

		// Token: 0x0400198C RID: 6540
		public Image factionLogo;

		// Token: 0x0400198D RID: 6541
		public YesNoDialog topActionAvailable;

		// Token: 0x0400198E RID: 6542
		public YesNoDialog maxResources;

		// Token: 0x0400198F RID: 6543
		public YesNoDialog noSuitablePlace;

		// Token: 0x04001990 RID: 6544
		public YesNoDialog maxedOutWarning;

		// Token: 0x04001991 RID: 6545
		public Sprite activeAction;

		// Token: 0x04001992 RID: 6546
		public Sprite defaultAction;

		// Token: 0x04001993 RID: 6547
		public Sprite factoryActiveAction;

		// Token: 0x04001994 RID: 6548
		public Sprite bonusDefault;

		// Token: 0x04001995 RID: 6549
		public Sprite bonusUnlocked;

		// Token: 0x04001996 RID: 6550
		public Image context;

		// Token: 0x04001997 RID: 6551
		public GameObject contextMatActionSelection;

		// Token: 0x04001998 RID: 6552
		public Button confirmActionSelection;

		// Token: 0x04001999 RID: 6553
		public GameObject contextMatActionReloading;

		// Token: 0x0400199A RID: 6554
		public GameObject buttonPreviewHide;

		// Token: 0x0400199B RID: 6555
		public Canvas matchHeightCanvas;

		// Token: 0x0400199D RID: 6557
		private bool isMatPreview;

		// Token: 0x0400199E RID: 6558
		private Player player;

		// Token: 0x0200047F RID: 1151
		// (Invoke) Token: 0x06002492 RID: 9362
		public delegate void EndTurnAvailable();
	}
}
