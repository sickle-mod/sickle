using System;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000406 RID: 1030
	public class TradePresenter : ActionPresenter
	{
		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06001F75 RID: 8053 RVA: 0x000C0FF4 File Offset: 0x000BF1F4
		// (remove) Token: 0x06001F76 RID: 8054 RVA: 0x000C1028 File Offset: 0x000BF228
		public static event TradePresenter.TradeEnd TradeEnded;

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x06001F77 RID: 8055 RVA: 0x000C105C File Offset: 0x000BF25C
		// (remove) Token: 0x06001F78 RID: 8056 RVA: 0x000C1090 File Offset: 0x000BF290
		public static event TradePresenter.GainResource ResourceGained;

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06001F79 RID: 8057 RVA: 0x000C10C4 File Offset: 0x000BF2C4
		// (remove) Token: 0x06001F7A RID: 8058 RVA: 0x000C10F8 File Offset: 0x000BF2F8
		public static event TradePresenter.ResourceSelect OnResourceSelected;

		// Token: 0x06001F7B RID: 8059 RVA: 0x0003C46F File Offset: 0x0003A66F
		private void Awake()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.originalTradeSelectionSizeDelta = this.tradeWindowRect.sizeDelta;
				this.originalTradeWindowAnchoredPosition = this.tradeWindowRect.anchoredPosition;
			}
			this.CreateTradeButtonsPanels();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0003C4A0 File Offset: 0x0003A6A0
		private void Update()
		{
			if ((Input.anyKeyDown || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)) && this.tradePanelAnimation != null)
			{
				this.tradePanelAnimation.Complete(true);
			}
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x000C112C File Offset: 0x000BF32C
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainAnyResource;
			this.resourceChoosen = ResourceType.combatCard;
			this.tradeButtonsMain.SetActive(true);
			this.availableHexes.Clear();
			if (this.resourceLeftText != null)
			{
				this.resourceLeftText.text = this.action.ResourcesLeft.ToString();
			}
			if (!PlatformManager.IsStandalone)
			{
				this.InitUsedTradeIconsMobile();
				this.tradeMobile.SetActive(false);
				GameController.HexSelectionMode = GameController.SelectionMode.PayResource;
			}
			if (this.action.GainAvaliable())
			{
				this.EnableInput();
				return;
			}
			this.OnActionEnded();
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x000C11C8 File Offset: 0x000BF3C8
		private void InitUsedTradeIconsMobile()
		{
			for (int i = 0; i < this.tradeIconsUsed.Length; i++)
			{
				this.tradeIconsUsed[i].transform.parent.gameObject.SetActive(i < (int)this.action.Amount);
				this.tradeIconsUsed[i].SetActive(false);
			}
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000C1220 File Offset: 0x000BF420
		private void CreateTradeButtonsPanels()
		{
			for (int i = 0; i < 8; i++)
			{
				global::UnityEngine.Object.Instantiate<GameObject>(this.tradeButtonsPanel, this.tradeButtonsMain.transform).SetActive(false);
			}
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000C1258 File Offset: 0x000BF458
		private void EnableInput()
		{
			this.FindAllHexes();
			this.AssingTradeButtonsPanel(false);
			this.EnableTemporaryResourcesMechanism();
			base.EnableMapBlackout();
			if (!PlatformManager.IsStandalone)
			{
				(GameController.Instance.gameBoardPresenter as FlatWorld).SetCollidersEnabledOnUnits(false, null, null);
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				GameController.Instance.matFaction.ClearHintStories();
				this.ResetHexColor();
			}
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0003C4D7 File Offset: 0x0003A6D7
		public bool HaveAction()
		{
			return this.action != null;
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0003C4E2 File Offset: 0x0003A6E2
		public void EnableVisuals(bool enabled)
		{
			this.EnableMarkers(enabled);
			this.tradeButtonsMain.SetActive(enabled);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000C12C4 File Offset: 0x000BF4C4
		private void EnableMarkers(bool enabled)
		{
			for (int i = 0; i < this.tradeButtonsMain.transform.childCount; i++)
			{
				TradeButtonsPanel component = this.tradeButtonsMain.transform.GetChild(i).GetComponent<TradeButtonsPanel>();
				if (component.gameObject.activeSelf)
				{
					component.GetAttachedGameHex().SetFocus(enabled, HexMarkers.MarkerType.Move, 0f, false);
				}
			}
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x000C1324 File Offset: 0x000BF524
		private void EnableTemporaryResourcesMechanism()
		{
			GameController.GameManager.SetTemporaryResources(this.action.GetPlayer().Resources(false));
			GameController.GameManager.SetTemporaryCombatCardsCount(this.action.GetPlayer().GetCombatCardsCount());
			GameController.GameManager.EnableTemporaryResourcesMechanism(true);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0003BEF9 File Offset: 0x0003A0F9
		private void DisableTemporaryResourcesMechanism()
		{
			GameController.GameManager.EnableTemporaryResourcesMechanism(false);
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x000C1374 File Offset: 0x000BF574
		private void ResetHexColor()
		{
			for (int i = 0; i < this.availableHexes.Count; i++)
			{
				this.tradeButtonsMain.transform.GetChild(i).GetComponent<TradeButtonsPanel>().hexBackground.color = Color.white;
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x000C13BC File Offset: 0x000BF5BC
		private void AssingTradeButtonsPanel(bool selectFieldMode)
		{
			if (this.tradeButtonsMain.transform.childCount < 8)
			{
				this.CreateTradeButtonsPanels();
			}
			for (int i = 0; i < this.tradeButtonsMain.transform.childCount; i++)
			{
				this.tradeButtonsMain.transform.GetChild(i).gameObject.SetActive(false);
			}
			for (int j = 0; j < this.availableHexes.Count; j++)
			{
				this.tradeButtonsMain.transform.GetChild(j).GetComponent<TradeButtonsPanel>().AttachTradePanel(this, this.availableHexes[j], selectFieldMode);
				this.tradeButtonsMain.transform.GetChild(j).gameObject.SetActive(true);
				this.availableHexes[j].SetFocus(true, HexMarkers.MarkerType.Move, 1f, false);
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000C1490 File Offset: 0x000BF690
		private void HideUnnecessaryTradeButtonsPanels(Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			for (int i = 0; i < this.tradeButtonsMain.transform.childCount; i++)
			{
				TradeButtonsPanel component = this.tradeButtonsMain.transform.GetChild(i).GetComponent<TradeButtonsPanel>();
				if (component.gameObject.activeInHierarchy && component.GetAttachedGameHex() != selectedHex)
				{
					component.GetAttachedGameHex().SetFocus(false, HexMarkers.MarkerType.Move, 1f, false);
					component.Hide();
				}
			}
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000C1500 File Offset: 0x000BF700
		private void AnimateTradeSelectionPanel()
		{
			Vector2 vector = new Vector2(this.tradeWindowRect.parent.GetComponent<RectTransform>().rect.width / 2f, this.tradeWindowRect.parent.GetComponent<RectTransform>().rect.height / 2f);
			this.SetButtonsInteractable(false);
			this.tradeWindowRect.anchoredPosition = vector;
			this.tradeWindowRect.localScale = Vector3.zero;
			Canvas.ForceUpdateCanvases();
			this.tradePanelAnimation = DOTween.Sequence();
			this.tradePanelAnimation.Append(this.tradeWindowRect.DOScale(1f, this.popupAnimationSpeed).SetEase(Ease.InOutCubic));
			this.tradePanelAnimation.Join(this.tradeWindowRect.DOAnchorPos(this.originalTradeWindowAnchoredPosition, this.popupAnimationSpeed, false).SetEase(Ease.InOutCubic));
			this.tradePanelAnimation.OnComplete(delegate
			{
				this.OnAnimationComplete();
			});
			this.tradePanelAnimation.Play<Sequence>();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0003C4F7 File Offset: 0x0003A6F7
		private void OnAnimationComplete()
		{
			this.SetButtonsInteractable(true);
			this.tradePanelAnimation = null;
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000C1608 File Offset: 0x000BF808
		private void SetButtonsInteractable(bool interactable)
		{
			for (int i = 0; i < this.ResourcesButtonsMobile.transform.childCount; i++)
			{
				this.ResourcesButtonsMobile.transform.GetChild(i).GetComponent<Button>().interactable = interactable;
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000C164C File Offset: 0x000BF84C
		public void GameHexSelected(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (!this.availableHexes.Contains(hex))
			{
				return;
			}
			this.selectedHex = hex;
			if (!PlatformManager.IsStandalone && !this.tradeMobile.activeSelf)
			{
				this.tradeMobile.SetActive(true);
				if (OptionsManager.IsConfirmActions())
				{
					this.AnimateTradeSelectionPanel();
				}
			}
			this.ResetHexColor();
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0003C507 File Offset: 0x0003A707
		public void ResourceSelectedMobile(int resource)
		{
			this.ResourceSelected((ResourceType)resource, this.selectedHex);
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000C16A4 File Offset: 0x000BF8A4
		public void ResourceSelected(ResourceType resource, Scythe.BoardPresenter.GameHexPresenter hex)
		{
			this.resourceChoosen = resource;
			if (TradePresenter.OnResourceSelected != null)
			{
				TradePresenter.OnResourceSelected(resource, hex);
			}
			if (this.resourceChoosen != ResourceType.combatCard && this.availableHexes.Contains(hex) && this.action.AddResourceToField(this.resourceChoosen, hex.GetGameHexLogic(), 1))
			{
				if (TradePresenter.ResourceGained != null)
				{
					TradePresenter.ResourceGained();
				}
				if (this.resourceLeftText != null)
				{
					this.resourceLeftText.text = this.action.ResourcesLeft.ToString();
				}
				if (!PlatformManager.IsStandalone)
				{
					int num = (int)this.action.Amount - (this.action.ResourcesLeft + 1);
					this.tradeIconsUsed[num].SetActive(true);
				}
				hex.UpdateTempResource(this.resourceChoosen, true, false, 0);
				GameController.GameManager.UpdateTemporaryResource(this.resourceChoosen, 1);
				GameController.Instance.OnUpdatePlayerStats();
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.AcquireGain(this.action.ResourcesLeft);
				if (this.action.ResourcesLeft <= 0)
				{
					this.OnActionEnded();
				}
			}
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x000C17E8 File Offset: 0x000BF9E8
		private void FindAllHexes()
		{
			if (this.action.IsEncounter)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(GameController.GameManager.PlayerCurrent.character.position);
				this.availableHexes.Add(gameHexPresenter);
				return;
			}
			foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter2 = GameController.Instance.GetGameHexPresenter(worker.position.posX, worker.position.posY);
				if (!this.availableHexes.Contains(gameHexPresenter2) && gameHexPresenter2.hexType != HexType.capital)
				{
					this.availableHexes.Add(gameHexPresenter2);
				}
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x000C18C0 File Offset: 0x000BFAC0
		public void WarningBeforeActionEnd()
		{
			if (this.action.ResourcesLeft > 0 && OptionsManager.IsWarningsActive())
			{
				if (PlatformManager.IsStandalone)
				{
					this.endAction.GetComponentInChildren<Text>().text = ScriptLocalization.Get("PlayerMat/EndTradeText");
				}
				else
				{
					this.endAction.GetFirstDescriptionLine().GetComponent<TextMeshProUGUI>().text = ScriptLocalization.Get("PlayerMat/EndTradeText");
				}
				WorldSFXManager.PlaySound(SoundEnum.Alert, AudioSourceType.Buttons);
				this.endAction.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
				{
					this.OnActionEnded();
				}, null);
				return;
			}
			this.OnActionEnded();
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x000C1974 File Offset: 0x000BFB74
		public override void OnActionEnded()
		{
			this.HideUnnecessaryTradeButtonsPanels(null);
			this.DisableTemporaryResourcesMechanism();
			base.DisableMapBlackout();
			foreach (object obj in this.tradeButtonsMain.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
			this.tradeButtonsMain.SetActive(false);
			if (!PlatformManager.IsStandalone)
			{
				GameController.ClearFocus();
				GameController.HexSelectionMode = GameController.SelectionMode.Normal;
				(GameController.Instance.gameBoardPresenter as FlatWorld).SetCollidersEnabledOnUnits(true, null, null);
			}
			HumanInputHandler.Instance.OnInputEnded();
			base.gameObject.SetActive(false);
			this.action = null;
			if (TradePresenter.TradeEnded != null)
			{
				TradePresenter.TradeEnded();
			}
		}

		// Token: 0x04001627 RID: 5671
		private GainAnyResource action;

		// Token: 0x04001628 RID: 5672
		private ResourceType resourceChoosen;

		// Token: 0x04001629 RID: 5673
		private List<Scythe.BoardPresenter.GameHexPresenter> availableHexes = new List<Scythe.BoardPresenter.GameHexPresenter>();

		// Token: 0x0400162A RID: 5674
		public YesNoDialog endAction;

		// Token: 0x0400162B RID: 5675
		public Text resourceLeftText;

		// Token: 0x0400162C RID: 5676
		public Image mapDarken;

		// Token: 0x0400162D RID: 5677
		public GameObject tradeButtonsMain;

		// Token: 0x0400162E RID: 5678
		public GameObject tradeButtonsPanel;

		// Token: 0x0400162F RID: 5679
		public GameObject tradeMobile;

		// Token: 0x04001630 RID: 5680
		public GameObject ResourcesButtonsMobile;

		// Token: 0x04001631 RID: 5681
		public GameObject[] tradeIconsUsed;

		// Token: 0x04001635 RID: 5685
		private Scythe.BoardPresenter.GameHexPresenter selectedHex;

		// Token: 0x04001636 RID: 5686
		[SerializeField]
		private RectTransform tradeWindowRect;

		// Token: 0x04001637 RID: 5687
		[SerializeField]
		private float popupAnimationSpeed = 0.5f;

		// Token: 0x04001638 RID: 5688
		private Vector2 originalTradeSelectionSizeDelta;

		// Token: 0x04001639 RID: 5689
		private Vector2 originalTradeWindowAnchoredPosition;

		// Token: 0x0400163A RID: 5690
		private Sequence tradePanelAnimation;

		// Token: 0x02000407 RID: 1031
		// (Invoke) Token: 0x06001F96 RID: 8086
		public delegate void TradeEnd();

		// Token: 0x02000408 RID: 1032
		// (Invoke) Token: 0x06001F9A RID: 8090
		public delegate void GainResource();

		// Token: 0x02000409 RID: 1033
		// (Invoke) Token: 0x06001F9E RID: 8094
		public delegate void ResourceSelect(ResourceType resource, Scythe.BoardPresenter.GameHexPresenter hex);
	}
}
