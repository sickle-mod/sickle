using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000484 RID: 1156
	public class MatPlayerSectionPresenter : MonoBehaviour
	{
		// Token: 0x0600249D RID: 9373 RVA: 0x000D91F4 File Offset: 0x000D73F4
		public void Init(TopAction topAction, DownAction downAction, Player player, bool isPreview)
		{
			this.player = player;
			int num = player.matPlayer.workers.Count - 2;
			this.UpdateSection(topAction, downAction, num, false);
			if (!isPreview)
			{
				this.CheckSection();
				return;
			}
			if (GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(player.lastMatSection, this.sectionID))
			{
				this.DisableActionsAndSection(-1, true);
				this.SetSectionCooldown(true, false);
				return;
			}
			this.DisableActionsAndSection(0, true);
			this.SetSectionCooldown(false, false);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x0003F525 File Offset: 0x0003D725
		public void ResetActionButtonsBackgrounds()
		{
			this.topActionPresenter.ResetButtonsBackgrounds();
			this.downActionPresenter.ResetButtonsBackgrounds();
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000D9274 File Offset: 0x000D7474
		public void UpdateSection(TopAction topAction, DownAction downAction, int numberOfWorkers, bool update = false)
		{
			int num = (int)((topAction.GetNumberOfPayActions() == 1) ? topAction.GetPayAction(0).Amount : 0);
			int amount = (int)topAction.GetGainAction(0).Amount;
			int num2 = (int)((topAction.GetNumberOfGainActions() == 2) ? topAction.GetGainAction(1).Amount : 0);
			Player player = topAction.GetPlayer();
			if (topAction.Type == TopActionType.Produce)
			{
				num = numberOfWorkers;
				num2 = 0;
			}
			int num3 = 0;
			if (downAction.GetNumberOfGainActions() == 2)
			{
				num3 = (int)downAction.GetGainAction(1).Amount;
			}
			bool flag = false;
			if (player != null && player.matPlayer.buildings.Contains(topAction.Structure))
			{
				flag = true;
			}
			int num4 = 0;
			int num5 = 1;
			int num6 = 0;
			int num7 = 0;
			if (downAction.Type != DownActionType.Factory)
			{
				num4 = (int)downAction.GetPayAction(0).Amount;
				num5 = (int)downAction.GetGainAction(0).Amount;
				num6 = (int)downAction.GetPayAction(0).GetUpgradeLevel();
				num7 = (int)downAction.GetPayAction(0).GetMaxUpgradeLevel();
			}
			if (!update)
			{
				this.topActionPresenter.Init(player, num, amount, num2, flag);
				this.downActionPresenter.Init(player, num4, num5 + num3, num6, num7, downAction.RecruitEnlisted());
				return;
			}
			this.topActionPresenter.UpdateSection(player, num, amount, num2, flag);
			this.downActionPresenter.UpdateSection(player, num4, num5 + num3, downAction.RecruitEnlisted());
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000D93BC File Offset: 0x000D75BC
		public void ResetColors(TopAction topAction)
		{
			int amount = (int)topAction.GetGainAction(0).Amount;
			int num = (int)((topAction.GetNumberOfGainActions() == 2) ? topAction.GetGainAction(1).Amount : 0);
			for (int i = 0; i < amount; i++)
			{
				this.topActionPresenter.actionGain1[i].color = Color.white;
				this.topActionPresenter.actionGain1[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
			}
			for (int j = 0; j < num; j++)
			{
				this.topActionPresenter.actionGain2[j].color = Color.white;
				this.topActionPresenter.actionGain2[j].transform.GetChild(0).GetComponent<Image>().color = Color.white;
			}
			for (int k = 0; k < this.topActionPresenter.actionCost.Length; k++)
			{
				this.topActionPresenter.actionCost[k].transform.GetChild(1).GetComponent<Image>().color = Color.white;
			}
			for (int l = 0; l < this.downActionPresenter.actionCost.Length; l++)
			{
				this.downActionPresenter.actionCost[l].transform.GetChild(1).GetComponent<Image>().color = Color.white;
			}
			this.downActionPresenter.gainActionButtonBackground[0].color = Color.white;
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000D9524 File Offset: 0x000D7724
		public void UpdateSection(TopAction topAction, DownAction downAction)
		{
			int num = (int)((topAction.GetNumberOfPayActions() == 1) ? topAction.GetPayAction(0).Amount : 0);
			int amount = (int)topAction.GetGainAction(0).Amount;
			int num2 = (int)((topAction.GetNumberOfGainActions() == 2) ? topAction.GetGainAction(1).Amount : 0);
			if (topAction.Type == TopActionType.Produce)
			{
				num = 0;
				num2 = 0;
			}
			int num3 = 0;
			if (downAction.GetNumberOfGainActions() == 2)
			{
				num3 = (int)downAction.GetGainAction(1).Amount;
			}
			int num4 = 0;
			int num5 = 1;
			int num6 = 0;
			int num7 = 0;
			if (downAction.GetNumberOfPayActions() > 0)
			{
				num4 = (int)downAction.GetPayAction(0).Amount;
				num6 = (int)downAction.GetPayAction(0).GetUpgradeLevel();
				num7 = (int)downAction.GetPayAction(0).GetMaxUpgradeLevel();
			}
			if (downAction.GetNumberOfGainActions() > 0)
			{
				num5 = (int)downAction.GetGainAction(0).Amount;
			}
			this.topActionPresenter.UpdateSectionSpecial(num, amount, num2, false);
			this.downActionPresenter.UpdateSectionSpecial(num4, num5 + num3, num6, num7, false);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000D9610 File Offset: 0x000D7810
		public void UpdateSectionForEnemyAnimation(TopAction topAction, DownAction downAction, int numberOfWorkers, bool decreaseGainOnTopAction, bool increaseCostOnDownAction, GainType gainType)
		{
			int num = (int)((topAction.GetNumberOfPayActions() == 1) ? topAction.GetPayAction(0).Amount : 0);
			int num2 = (int)topAction.GetGainAction(0).Amount;
			int num3 = (int)((topAction.GetNumberOfGainActions() == 2) ? topAction.GetGainAction(1).Amount : 0);
			if (decreaseGainOnTopAction)
			{
				if (topAction.GetGainAction(0).GetGainType() == gainType)
				{
					num2--;
				}
				else if (topAction.GetGainAction(1).GetGainType() == gainType)
				{
					num3--;
				}
			}
			if (topAction.Type == TopActionType.Produce)
			{
				num = numberOfWorkers;
				num3 = 0;
			}
			int num4 = 0;
			if (downAction.GetNumberOfGainActions() == 2)
			{
				num4 = (int)downAction.GetGainAction(1).Amount;
			}
			bool flag = false;
			if (topAction.GetPlayer().matPlayer.buildings.Contains(topAction.Structure))
			{
				flag = true;
			}
			int num5 = 0;
			int num6 = 1;
			int num7 = 0;
			int num8 = 0;
			if (downAction.Type != DownActionType.Factory)
			{
				num5 = (int)downAction.GetPayAction(0).Amount;
				num6 = (int)downAction.GetGainAction(0).Amount;
				num7 = (int)downAction.GetPayAction(0).GetUpgradeLevel();
				num8 = (int)downAction.GetPayAction(0).GetMaxUpgradeLevel();
				if (increaseCostOnDownAction)
				{
					num5++;
				}
			}
			this.topActionPresenter.Init(topAction.GetPlayer(), num, num2, num3, flag);
			this.downActionPresenter.Init(topAction.GetPlayer(), num5, num6 + num4, num7, num8, downAction.RecruitEnlisted());
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000D9760 File Offset: 0x000D7960
		public void SwapTopAction(MatPlayerSectionPresenter playerSection)
		{
			PlayerTopActionPresenter playerTopActionPresenter = this.topActionPresenter;
			this.topActionPresenter = playerSection.topActionPresenter;
			playerSection.topActionPresenter = playerTopActionPresenter;
			playerSection.SetTopActionPosition();
			this.SetTopActionPosition();
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000D9794 File Offset: 0x000D7994
		public void SetTopActionPosition()
		{
			this.topActionPresenter.gameObject.transform.SetParent(base.transform);
			this.topActionPresenter.gameObject.transform.SetSiblingIndex(1);
			this.topActionPresenter.transform.localPosition = new Vector3(0f, this.topActionPresenter.transform.localPosition.y);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000D9804 File Offset: 0x000D7A04
		public void SetTubeLamp(int state)
		{
			if (state == 1)
			{
				this.background.sprite = this.backgroundActive;
			}
			else
			{
				this.background.sprite = this.backgroundInactive;
			}
			if (state < 0)
			{
				this.background.color = new Color(0.2f, 0.2f, 0.2f);
				return;
			}
			this.background.color = Color.white;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000D9870 File Offset: 0x000D7A70
		public void CheckSection()
		{
			this.SetTubeLamp(0);
			if (GameController.GameManager.PlayerCurrent.matFaction.DidPlayerUsedMatLastTurn(this.player.lastMatSection, this.sectionID))
			{
				this.SetTubeLamp(-1);
				this.topActionPresenter.DisableAllButtons(true);
				this.downActionPresenter.DisableAllButtons(true);
				this.SetSectionCooldown(true, false);
				this.sectionGlass.enabled = false;
				return;
			}
			this.SetSectionCooldown(false, false);
			this.CheckActions();
			this.topActionPresenter.EnableAllButtons();
			this.downActionPresenter.EnableAllButtons();
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000D9904 File Offset: 0x000D7B04
		private void SetTopActionColor(Sprite actionBackground, Color frameCol)
		{
			this.topActionPresenter.GetComponent<Image>().sprite = actionBackground;
			Image[] gainActionFrame = this.topActionPresenter.gainActionFrame;
			for (int i = 0; i < gainActionFrame.Length; i++)
			{
				gainActionFrame[i].color = frameCol;
			}
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000D9948 File Offset: 0x000D7B48
		private void SetDownActionColor(Sprite actionBackground, Color frameCol)
		{
			this.downActionPresenter.GetComponent<Image>().sprite = actionBackground;
			Image[] gainActionFrame = this.downActionPresenter.gainActionFrame;
			for (int i = 0; i < gainActionFrame.Length; i++)
			{
				gainActionFrame[i].color = frameCol;
			}
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000D998C File Offset: 0x000D7B8C
		public void CheckActions()
		{
			bool flag = true;
			if (this.UpdateTopActionColors())
			{
				flag = false;
			}
			if (this.UpdateDownActionColors())
			{
				flag = false;
			}
			if (flag)
			{
				this.NothingToDo();
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000D99B8 File Offset: 0x000D7BB8
		public bool UpdateTopActionColors()
		{
			if (!this.player.matPlayer.GetPlayerMatSection(this.sectionID).ActionTop.CanPlayerPayActions())
			{
				this.SetTopActionColor(this.actionTooExpensive, this.frameColorTooExpensive);
				return false;
			}
			for (int i = 0; i < this.player.matPlayer.GetPlayerMatSection(this.sectionID).ActionTop.GetNumberOfGainActions(); i++)
			{
				if (i < this.topActionPresenter.gainActionFrame.Length)
				{
					if (!this.player.matPlayer.GetPlayerMatSection(this.sectionID).ActionTop.CanPlayerGainFromActions(i) || (this.player.matPlayer.GetPlayerMatSection(this.sectionID).ActionTop.Type == TopActionType.Produce && this.topActionPresenter.SuitablePlaceForProduceUI()))
					{
						this.topActionPresenter.gainActionFrame[i].color = this.frameColorNoGain;
					}
					else
					{
						this.topActionPresenter.gainActionFrame[i].color = this.frameColorDefault;
					}
				}
			}
			this.topActionPresenter.GetComponent<Image>().sprite = this.actionDefault;
			return true;
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000D9ADC File Offset: 0x000D7CDC
		public bool UpdateDownActionColors()
		{
			if (!this.player.matPlayer.GetPlayerMatSection(this.sectionID).ActionDown.CanPlayerPayActions())
			{
				this.SetDownActionColor(this.actionTooExpensive, this.frameColorTooExpensive);
				return false;
			}
			if (!this.player.matPlayer.GetPlayerMatSection(this.sectionID).ActionDown.CanPlayerGainFromActions())
			{
				this.SetDownActionColor(this.actionDefault, this.frameColorNoGain);
				this.downActionPresenter.HideInactiveActionOverlays();
				return true;
			}
			this.SetDownActionColor(this.actionDefault, this.frameColorDefault);
			this.downActionPresenter.HideInactiveActionOverlays();
			return true;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0003F53D File Offset: 0x0003D73D
		public void CheckDownAction()
		{
			if (!this.UpdateDownActionColors())
			{
				this.NothingToDo();
			}
			this.downActionPresenter.EnableAllButtons();
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x0003F558 File Offset: 0x0003D758
		public void DisableActionsAndSection(int tubeState = 0, bool setTube = true)
		{
			if (setTube)
			{
				this.SetTubeLamp(tubeState);
			}
			this.topActionPresenter.DisableAllButtons(GameController.GameManager.IsMultiplayer);
			this.downActionPresenter.DisableAllButtons(GameController.GameManager.IsMultiplayer);
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000D9B80 File Offset: 0x000D7D80
		public void AfterLoadUpdate()
		{
			this.OnMatSectionSelected();
			if (this.player.topActionFinished)
			{
				this.topActionPresenter.DisableAllButtons(false);
				if (PlatformManager.IsMobile)
				{
					this.topActionPresenter.SelectAction(true, -1);
				}
			}
			if (this.player.downActionFinished)
			{
				this.topActionPresenter.DisableAllButtons(false);
				this.downActionPresenter.DisableAllButtons(false);
				if (PlatformManager.IsMobile)
				{
					this.downActionPresenter.SelectAction(true);
				}
			}
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x0003F58E File Offset: 0x0003D78E
		public void OnMatSectionSelected()
		{
			this.sectionGlass.enabled = false;
			this.SetTubeLamp(1);
			this.topActionPresenter.EnableAllButtons();
			this.downActionPresenter.EnableAllButtons();
			this.CheckActions();
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000D9BF8 File Offset: 0x000D7DF8
		public void OnMatSectionHighlighted()
		{
			foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in GameController.Instance.matPlayer.matSection)
			{
				if (matPlayerSectionPresenter != this)
				{
					matPlayerSectionPresenter.CheckSection();
				}
				else
				{
					this.SetTubeLamp(1);
				}
			}
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000D9C68 File Offset: 0x000D7E68
		public void NothingToDo()
		{
			if (PlatformManager.IsStandalone)
			{
				if (this.player.matFaction.factionPerk != AbilityPerk.Relenteless || this.sectionID == 4)
				{
					foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in GameController.Instance.matPlayer.matSection)
					{
						matPlayerSectionPresenter.SetTubeLamp(0);
					}
				}
				this.SetTubeLamp(-1);
			}
			GameController.Instance.endTurnButton.image.sprite = GameController.Instance.endTurnButtonImageActiveGlow;
			GameController.Instance.endTurnButton.GetComponent<Animator>().Play("EndTurnPulse", -1, 0f);
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0003F5BF File Offset: 0x0003D7BF
		public void ShowAssit()
		{
			if (OptionsManager.IsActionAssist() && base.GetComponent<Animator>() != null)
			{
				base.GetComponent<Animator>().Play("PlayerMatSectionTopIconsShow");
			}
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0003F5E6 File Offset: 0x0003D7E6
		public void HideAssist(bool immidiate = false)
		{
			if (OptionsManager.IsActionAssist() && base.GetComponent<Animator>() != null)
			{
				base.GetComponent<Animator>().Play("PlayerMatSectionTopIconsHide");
			}
			this.downActionPresenter.HideGuidline();
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000D9D2C File Offset: 0x000D7F2C
		public void SetSectionCooldown(bool isCooldown, bool skipTopActionCooldownSetup = false)
		{
			if (PlatformManager.IsStandalone)
			{
				if (isCooldown)
				{
					this.topActionPresenter.SelectAction(false, -1);
					this.downActionPresenter.SelectAction(false);
				}
				if (this.lastSectionImage != null)
				{
					this.lastSectionImage.enabled = isCooldown;
					return;
				}
			}
			else
			{
				if (!skipTopActionCooldownSetup)
				{
					this.topActionPresenter.SelectAction(isCooldown, -1);
				}
				this.downActionPresenter.SelectAction(isCooldown);
			}
		}

		// Token: 0x040019A6 RID: 6566
		public PlayerTopActionPresenter topActionPresenter;

		// Token: 0x040019A7 RID: 6567
		public PlayerDownActionPresenter downActionPresenter;

		// Token: 0x040019A8 RID: 6568
		public int sectionID = -1;

		// Token: 0x040019A9 RID: 6569
		public Image sectionGlass;

		// Token: 0x040019AA RID: 6570
		[SerializeField]
		private Image lastSectionImage;

		// Token: 0x040019AB RID: 6571
		public Image background;

		// Token: 0x040019AC RID: 6572
		public Sprite backgroundInactive;

		// Token: 0x040019AD RID: 6573
		public Sprite backgroundActive;

		// Token: 0x040019AE RID: 6574
		public Sprite actionDefault;

		// Token: 0x040019AF RID: 6575
		public Sprite actionTooExpensive;

		// Token: 0x040019B0 RID: 6576
		public Color colorActive;

		// Token: 0x040019B1 RID: 6577
		public Color frameColorDefault = new Color(0.5f, 0.9f, 0f);

		// Token: 0x040019B2 RID: 6578
		public Color frameColorNoGain = new Color(1f, 0.8f, 0.25f);

		// Token: 0x040019B3 RID: 6579
		public Color frameColorTooExpensive = new Color(1f, 0.1f, 0f);

		// Token: 0x040019B4 RID: 6580
		public GameObject usedLastTurnMessage;

		// Token: 0x040019B5 RID: 6581
		private Player player;
	}
}
