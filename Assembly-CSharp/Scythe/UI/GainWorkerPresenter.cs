using System;
using System.Collections.Generic;
using HoneyFramework;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003EC RID: 1004
	public class GainWorkerPresenter : ActionPresenter
	{
		// Token: 0x06001E0F RID: 7695 RVA: 0x0003B784 File Offset: 0x00039984
		private void OnEnable()
		{
			this.mouseHeldDownOnEnable = Input.GetMouseButton(0);
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0003B792 File Offset: 0x00039992
		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				this.mouseHeldDownOnEnable = false;
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x0002920A File Offset: 0x0002740A
		private void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000B942C File Offset: 0x000B762C
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainWorker;
			this.availableHexes.Clear();
			GameController.HexSelectionMode = GameController.SelectionMode.GainWorker;
			if (this.darken != null)
			{
				this.darken.enabled = true;
			}
			this.optionPresenter.SetActive(true);
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000B947C File Offset: 0x000B767C
		public void OptionSelected(bool option)
		{
			if (this.mouseHeldDownOnEnable)
			{
				return;
			}
			if (this.darken != null)
			{
				this.darken.enabled = false;
			}
			this.optionPresenter.SetActive(false);
			if (option)
			{
				this.EnableInput();
				return;
			}
			this.DontTakeWorker();
			this.OnActionEnded();
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0003B7A3 File Offset: 0x000399A3
		private void EnableInput()
		{
			GameController.HexGetFocused += this.OnHexSelected;
			this.FindAllHexes();
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x0002BA43 File Offset: 0x00029C43
		private Scythe.BoardPresenter.GameHexPresenter GetGameHexPresenter(int x, int y)
		{
			return GameController.Instance.gameBoardPresenter.GetGameHexPresenter(x, y);
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000B94D0 File Offset: 0x000B76D0
		private void FindAllHexes()
		{
			GameController.ClearFocus();
			if (this.action.IsEncounter)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(GameController.GameManager.PlayerCurrent.character.position);
				gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.FieldSelected, 0f, false);
				this.availableHexes.Add(gameHexPresenter);
				this.OnHexSelected(gameHexPresenter);
				return;
			}
			if (GameController.GameManager.PlayerCurrent.currentMatSection == 4)
			{
				using (List<Mech>.Enumerator enumerator = GameController.GameManager.PlayerCurrent.matFaction.mechs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mech mech = enumerator.Current;
						Scythe.BoardPresenter.GameHexPresenter gameHexPresenter2 = GameController.Instance.GetGameHexPresenter(mech.position.posX, mech.position.posY);
						if (!this.availableHexes.Contains(gameHexPresenter2) && gameHexPresenter2.hexType != HexType.capital)
						{
							gameHexPresenter2.SetFocus(true, HexMarkers.MarkerType.FieldSelected, 0f, false);
							this.availableHexes.Add(gameHexPresenter2);
						}
					}
					return;
				}
			}
			foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter3 = GameController.Instance.GetGameHexPresenter(worker.position.posX, worker.position.posY);
				if (!this.availableHexes.Contains(gameHexPresenter3) && gameHexPresenter3.hexType != HexType.capital)
				{
					gameHexPresenter3.SetFocus(true, HexMarkers.MarkerType.FieldSelected, 0f, false);
					this.availableHexes.Add(gameHexPresenter3);
				}
			}
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0003B7BC File Offset: 0x000399BC
		public void OnHexSelected(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (this.availableHexes.Contains(hex) && this.action.SetLocationAndWorkersAmount(hex.GetGameHexLogic(), 1))
			{
				this.OnActionEnded();
			}
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0003B7E6 File Offset: 0x000399E6
		public bool HaveAction()
		{
			return this.action != null;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0003B7F1 File Offset: 0x000399F1
		public void EnableVisuals(bool enabled)
		{
			this.EnableMarkers(enabled);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000B9694 File Offset: 0x000B7894
		private void EnableMarkers(bool enabled)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.availableHexes)
			{
				gameHexPresenter.SetFocus(enabled, HexMarkers.MarkerType.DeployTrade, 0f, false);
			}
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0003B7FA File Offset: 0x000399FA
		public override void OnActionEnded()
		{
			this.DisableInput();
			HumanInputHandler.Instance.OnInputEnded();
			GameController.Instance.gameBoardPresenter.UpdateBoard(true, true);
			this.action = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000B96EC File Offset: 0x000B78EC
		private void DontTakeWorker()
		{
			GameHex gameHex;
			if (this.action.IsEncounter)
			{
				gameHex = GameController.GameManager.PlayerCurrent.character.position;
			}
			else if (GameController.GameManager.PlayerCurrent.currentMatSection == 4)
			{
				gameHex = GameController.GameManager.PlayerCurrent.matFaction.mechs[0].position;
			}
			else
			{
				gameHex = GameController.GameManager.PlayerCurrent.matPlayer.workers[0].position;
			}
			this.action.SetLocationAndWorkersAmount(gameHex, 0);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000B9784 File Offset: 0x000B7984
		private void DisableInput()
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.availableHexes)
			{
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.FieldSelected, 0f, false);
			}
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			GameController.HexGetFocused -= this.OnHexSelected;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000B97F4 File Offset: 0x000B79F4
		public override void Clear()
		{
			if (this.darken != null)
			{
				this.darken.enabled = false;
			}
			this.optionPresenter.SetActive(false);
			this.action = null;
			this.availableHexes.Clear();
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			this.SetActive(false);
		}

		// Token: 0x0400156C RID: 5484
		public GameObject optionPresenter;

		// Token: 0x0400156D RID: 5485
		public Image darken;

		// Token: 0x0400156E RID: 5486
		private GainWorker action;

		// Token: 0x0400156F RID: 5487
		private HashSet<Scythe.BoardPresenter.GameHexPresenter> availableHexes = new HashSet<Scythe.BoardPresenter.GameHexPresenter>();

		// Token: 0x04001570 RID: 5488
		private bool mouseHeldDownOnEnable;
	}
}
