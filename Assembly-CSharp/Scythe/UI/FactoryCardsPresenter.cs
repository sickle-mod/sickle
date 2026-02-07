using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000472 RID: 1138
	public class FactoryCardsPresenter : MonoBehaviour
	{
		// Token: 0x140000EB RID: 235
		// (add) Token: 0x060023FB RID: 9211 RVA: 0x000D52B4 File Offset: 0x000D34B4
		// (remove) Token: 0x060023FC RID: 9212 RVA: 0x000D52EC File Offset: 0x000D34EC
		public event FactoryCardsPresenter.FactoryCardChoose OnFactoryCardChoosen;

		// Token: 0x060023FD RID: 9213 RVA: 0x000D5324 File Offset: 0x000D3524
		public void SetCards()
		{
			if (!this.displayedCards)
			{
				this.displayedCards = true;
				GameController.Instance.waitInfoFactory.SetActive(false);
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.panelInfo.DisableEndTurnObjectives();
				}
				else
				{
					SingletonMono<TopMenuPanelsManager>.Instance.GetTopMenuObjectivesPresenter().DisableEndTurnObjectives();
				}
				MusicManager.Instance.PrepareFactoryMusic();
				this.factoryCards.Clear();
				List<FactoryCard> list = GameController.GameManager.GetFactoryCards();
				for (int i = 0; i < list.Count; i++)
				{
					this.GenerateCard(list[i], i, true);
				}
				this.cardChoosen = false;
				if (this.cardSelectionInfo != null)
				{
					this.cardSelectionInfo.text = ScriptLocalization.Get("GameScene/ChooseFactoryCard");
					this.cardSelectionInfo.enabled = true;
				}
				if (this.mapDarken != null)
				{
					this.mapDarken.enabled = true;
				}
				base.gameObject.SetActive(true);
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				}
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.factory_popup, Contexts.ingame);
			}
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000D5444 File Offset: 0x000D3644
		public void ShowEmptyCards(int amount)
		{
			if (this.emptyCardPrefab != null && !this.displayedCards)
			{
				this.displayedCards = true;
				this.factoryCards.Clear();
				for (int i = 0; i < amount; i++)
				{
					GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.emptyCardPrefab, this.cardSelectionContainer);
					gameObject.GetComponent<RectTransform>().sizeDelta = this.factoryCardPrefab.GetComponent<RectTransform>().sizeDelta;
					gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
					gameObject.transform.localEulerAngles = Vector3.zero;
					gameObject.gameObject.SetActive(true);
					this.factoryCards.Add(gameObject);
				}
				if (this.cardSelectionInfo != null)
				{
					this.cardSelectionInfo.text = ScriptLocalization.Get("FactionMat/" + GameController.GameManager.PlayerCurrent.matFaction.faction.ToString()) + " " + ScriptLocalization.Get("GameScene/IsChoosing");
					this.cardSelectionInfo.enabled = true;
				}
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000D5558 File Offset: 0x000D3758
		public void ShowCard(int cardIndex, int positionIndex)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			FactoryCard factoryCard = new FactoryCard(cardIndex, GameController.GameManager);
			this.GenerateCard(factoryCard, positionIndex, false);
			global::UnityEngine.Object.Destroy(this.factoryCards[positionIndex]);
			this.factoryCards.RemoveAt(positionIndex);
			base.StartCoroutine(this.ClearEnemySelection(4f));
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0003EF0C File Offset: 0x0003D10C
		private IEnumerator ClearEnemySelection(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (!GameController.GameManager.IsMyTurn())
			{
				this.Clear();
			}
			yield break;
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x000D55B8 File Offset: 0x000D37B8
		public MatPlayerSectionPresenter GenerateSectionPresenter(GameObject factoryCardObject, FactoryCard factoryCard)
		{
			MatPlayerSectionPresenter component = factoryCardObject.GetComponent<MatPlayerSectionPresenter>();
			PlayerTopActionPresenter topActionPresenter = component.topActionPresenter;
			TopAction actionTop = factoryCard.ActionTop;
			this.SetTopActionPaySprites(topActionPresenter, actionTop);
			this.SetTopActionGainSprites(topActionPresenter, actionTop);
			component.DisableActionsAndSection(0, true);
			component.SetSectionCooldown(false, false);
			component.UpdateSection(actionTop, factoryCard.ActionDown, 0, true);
			component.topActionPresenter.GetComponent<Image>().sprite = component.actionDefault;
			component.downActionPresenter.GetComponent<Image>().sprite = component.actionDefault;
			component.downActionPresenter.ResetGainTileColors();
			component.topActionPresenter.transform.GetChild(1).gameObject.SetActive(factoryCard.CardId == 14);
			return component;
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000D5668 File Offset: 0x000D3868
		private void GenerateCard(FactoryCard factoryCard, int index, bool canBeChoosen)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.factoryCardPrefab);
			gameObject.transform.SetParent(this.cardSelectionContainer);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.sizeDelta = this.factoryCardPrefab.GetComponent<RectTransform>().sizeDelta;
			component.localScale = Vector3.one;
			component.localPosition = new Vector3(component.position.x, component.position.y, 0f);
			component.rotation = default(Quaternion);
			gameObject.gameObject.SetActive(true);
			if (KeyboardShortcuts.Instance != null)
			{
				KeyboardShortcuts.Instance.FactionLineKeyInfo.Add(gameObject.GetComponent<FactoryMatPlayerSectionPresenter>().keyInfoFactoryCard);
			}
			FactoryMatPlayerSectionPresenter factoryMatPlayerSectionPresenter = this.GenerateSectionPresenter(gameObject, factoryCard) as FactoryMatPlayerSectionPresenter;
			factoryMatPlayerSectionPresenter.sectionID = index;
			if (canBeChoosen)
			{
				factoryMatPlayerSectionPresenter.ShowChoosableCardPreview(factoryCard);
			}
			else
			{
				factoryMatPlayerSectionPresenter.ShowCardPreview(factoryCard);
			}
			factoryMatPlayerSectionPresenter.sectionGlass.enabled = false;
			factoryMatPlayerSectionPresenter.transform.GetChild(0).GetChild(0).GetComponent<Image>()
				.enabled = true;
			Color color = new Color(0.5f, 0.66f, 0.7f, 1f);
			factoryMatPlayerSectionPresenter.topActionPresenter.GetComponent<Image>().color = color;
			factoryMatPlayerSectionPresenter.downActionPresenter.GetComponent<Image>().color = color;
			this.factoryCards.Add(gameObject);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000D57C0 File Offset: 0x000D39C0
		private void SetTopActionPaySprites(PlayerTopActionPresenter presenter, TopAction action)
		{
			for (int i = 0; i < presenter.actionCost.Length; i++)
			{
				presenter.actionCost[i].gameObject.SetActive(false);
			}
			if (action.GetPayAction(0).GetPayType() == PayType.Resource && (action.GetPayAction(0) as PayResource).DifferentResources)
			{
				this.TransformImage(this.GetPaySprite(PayType.Resource), presenter.actionCost[0], 2);
				this.TransformImage(this.GetPaySprite(PayType.Resource), presenter.actionCost[3], 2);
				presenter.actionCost[1].color = new Color(0f, 0f, 0f, 0f);
				presenter.actionCost[1].transform.GetChild(2).GetComponent<Image>().sprite = this.differentSprite;
				presenter.actionCost[1].transform.GetChild(2).GetComponent<Image>().color = Color.white;
				presenter.actionCost[1].transform.GetChild(2).GetComponent<Image>().rectTransform.localRotation = Quaternion.identity;
				presenter.actionCost[4].color = new Color(0f, 0f, 0f, 0f);
				presenter.actionCost[4].transform.GetChild(2).GetComponent<Image>().sprite = this.differentSprite;
				presenter.actionCost[4].transform.GetChild(2).GetComponent<Image>().color = Color.white;
				presenter.actionCost[4].transform.GetChild(2).GetComponent<Image>().rectTransform.localRotation = Quaternion.identity;
				this.TransformImage(this.GetPaySprite(PayType.Resource), presenter.actionCost[2], 2);
				this.TransformImage(this.GetPaySprite(PayType.Resource), presenter.actionCost[5], 2);
				for (int j = 0; j < 3; j++)
				{
					presenter.actionCost[j].gameObject.SetActive(true);
					presenter.actionCost[j + 3].gameObject.SetActive(true);
				}
				return;
			}
			for (int k = 0; k < action.GetNumberOfPayActions(); k++)
			{
				for (int l = 0; l < (int)action.GetPayAction(k).Amount; l++)
				{
					this.TransformImage(this.GetPaySprite(action.GetPayAction(k).GetPayType()), presenter.actionCost[k + l], 2);
					this.TransformImage(this.GetPaySprite(action.GetPayAction(k).GetPayType()), presenter.actionCost[k + l + 3], 2);
					presenter.actionCost[k + l].gameObject.SetActive(true);
					presenter.actionCost[k + l + 3].gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000D5A74 File Offset: 0x000D3C74
		public void ClickTopAction(int index = -1)
		{
			FactoryMatPlayerSectionPresenter component = this.factoryCardPrefab.GetComponent<FactoryMatPlayerSectionPresenter>();
			if ((index == -1 && component.topActionPresenter.actionEnabled[0]) || (index != -1 && component.topActionPresenter.actionEnabled[index]))
			{
				int sectionID = component.sectionID;
				MatPlayerPresenter componentInParent = this.factoryCardPrefab.GetComponentInParent<MatPlayerPresenter>();
				componentInParent.MatPlayerSectionSelected(sectionID);
				componentInParent.StartTopAction(index);
				return;
			}
			if (!component.ShowingChoosableCard && component.sectionGlass.sprite != this.disabledCard)
			{
				component.topActionPresenter.ShowHelp(index, true);
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000D5B00 File Offset: 0x000D3D00
		public void ClickBottomAction()
		{
			FactoryMatPlayerSectionPresenter component = this.factoryCardPrefab.GetComponent<FactoryMatPlayerSectionPresenter>();
			if (component.downActionPresenter.actionEnabled[0])
			{
				int sectionID = component.sectionID;
				MatPlayerPresenter componentInParent = this.factoryCardPrefab.GetComponentInParent<MatPlayerPresenter>();
				componentInParent.MatPlayerSectionSelected(sectionID);
				componentInParent.StartBottomAction();
				return;
			}
			if (!component.ShowingChoosableCard && component.sectionGlass.sprite != this.disabledCard)
			{
				component.downActionPresenter.ShowHelp(true);
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000D5B74 File Offset: 0x000D3D74
		private void SetTopActionGainSprites(PlayerTopActionPresenter presenter, TopAction action)
		{
			for (int i = 1; i < presenter.actionGain1.Length; i++)
			{
				presenter.actionGain1[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < presenter.actionGain2.Length; j++)
			{
				presenter.actionGain2[j].gameObject.SetActive(false);
			}
			if (action.DifferentGain)
			{
				presenter.gainActionButton[0].interactable = false;
				presenter.gainActionButton[1].interactable = false;
				presenter.gainActionButton[0].onClick.RemoveAllListeners();
				presenter.gainActionButton[1].onClick.RemoveAllListeners();
				presenter.gainActionButton[1].gameObject.SetActive(true);
				presenter.gainActionButton[0].gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-32f, 0f);
				presenter.actionGain2[0].gameObject.SetActive(true);
				this.TransformImage(this.GetGainSprite(action.GetGainAction(0).GetGainType()), presenter.actionGain1[0], 0);
				this.TransformImage(this.GetGainSprite(action.GetGainAction(1).GetGainType()), presenter.actionGain2[0], 0);
				return;
			}
			presenter.gainActionButton[1].gameObject.SetActive(false);
			presenter.gainActionButton[0].gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
			presenter.gainActionButton[0].onClick.RemoveAllListeners();
			for (int k = 0; k < action.GetNumberOfGainActions(); k++)
			{
				for (int l = 0; l < (int)action.GetGainAction(k).Amount; l++)
				{
					this.TransformImage(this.GetGainSprite(action.GetGainAction(k).GetGainType()), presenter.actionGain1[k + l], 0);
					presenter.actionGain1[k + l].gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000D5D54 File Offset: 0x000D3F54
		private Image GetPaySprite(PayType payType)
		{
			switch (payType)
			{
			case PayType.Coin:
				return this.spriteCoin;
			case PayType.Popularity:
				return this.spritePopularity;
			case PayType.Power:
				return this.spritePower;
			case PayType.CombatCard:
				return this.spriteCombatCard;
			case PayType.Resource:
				return this.spriteResource;
			default:
				return null;
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000D5DA4 File Offset: 0x000D3FA4
		private Image GetGainSprite(GainType gainType)
		{
			switch (gainType)
			{
			case GainType.Coin:
				return this.spriteCoin;
			case GainType.Popularity:
				return this.spritePopularity;
			case GainType.Power:
				return this.spritePower;
			case GainType.CombatCard:
				return this.spriteCombatCard;
			case GainType.Produce:
				return this.gainProduceSprite;
			case GainType.AnyResource:
				return this.spriteResource;
			case GainType.Upgrade:
				return this.gainUpgradeSprite;
			case GainType.Mech:
				return this.gainMechSprite;
			case GainType.Worker:
				return this.gainWorkerSprite;
			case GainType.Building:
				return this.gainBuildingSprite;
			case GainType.Recruit:
				return this.gainRecruitSprite;
			}
			return null;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000D5E3C File Offset: 0x000D403C
		public void ChooseCard(int index)
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.factory_card_selected);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Contexts.ingame);
			if (!GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.AddFactoryCard(index);
				GameController.Instance.AddFactoryCard();
				this.Clear();
			}
			else
			{
				if (this.cardChoosen)
				{
					return;
				}
				this.cardChoosen = true;
				GameController.GameManager.AddFactoryCard(index);
				GameController.Instance.AddFactoryCard();
				this.Clear();
				GameController.GameManager.FactoryCardChoose(index);
			}
			if (this.OnFactoryCardChoosen != null)
			{
				this.OnFactoryCardChoosen();
			}
			GameController.Instance.undoController.PushToStack();
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000D5EE0 File Offset: 0x000D40E0
		public void Clear()
		{
			if (this.displayedCards)
			{
				this.displayedCards = false;
				GameController.Instance.UpdateStats(true, true);
				List<GameObject> list = new List<GameObject>();
				foreach (object obj in this.cardSelectionContainer)
				{
					Transform transform = (Transform)obj;
					list.Add(transform.gameObject);
				}
				list.ForEach(delegate(GameObject card)
				{
					global::UnityEngine.Object.Destroy(card);
				});
				if ((GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent == GameController.GameManager.PlayerOwner) || !GameController.GameManager.IsMultiplayer)
				{
					GameController.Instance.EndTurnButtonEnable();
				}
				if (this.mapDarken != null)
				{
					this.mapDarken.enabled = false;
				}
				if (this.cardSelectionInfo != null)
				{
					this.cardSelectionInfo.enabled = false;
				}
				base.gameObject.SetActive(false);
				if (!PlatformManager.IsStandalone)
				{
					SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Visible, 0.25f);
				}
				AssetBundleManager.UnloadAssetBundle("graphic_factory", true);
				if (!GameController.GameManager.IsMultiplayer || GameController.GameManager.PlayerCurrent == GameController.GameManager.PlayerOwner)
				{
					MusicManager.Instance.EndFactoryMusic();
				}
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000D6050 File Offset: 0x000D4250
		private void TransformImage(Image source, Image target, int childIndex)
		{
			target.color = Color.white;
			target.transform.GetChild(childIndex).GetComponent<Image>().sprite = source.sprite;
			target.transform.GetChild(childIndex).GetComponent<Image>().preserveAspect = true;
			target.transform.GetChild(childIndex).GetComponent<Image>().color = source.color;
			if (target.transform.GetChild(childIndex).GetComponent<Image>().rectTransform != null)
			{
				target.transform.GetChild(childIndex).GetComponent<Image>().rectTransform.anchoredPosition = source.rectTransform.anchoredPosition;
				target.transform.GetChild(childIndex).GetComponent<Image>().rectTransform.sizeDelta = source.rectTransform.sizeDelta;
				target.transform.GetChild(childIndex).GetComponent<Image>().rectTransform.localRotation = source.rectTransform.localRotation;
			}
		}

		// Token: 0x04001919 RID: 6425
		private List<GameObject> factoryCards = new List<GameObject>();

		// Token: 0x0400191A RID: 6426
		public GameObject factoryCardPrefab;

		// Token: 0x0400191B RID: 6427
		public GameObject emptyCardPrefab;

		// Token: 0x0400191C RID: 6428
		public GameObject keyInfoFactoryCard;

		// Token: 0x0400191D RID: 6429
		public Image mapDarken;

		// Token: 0x0400191E RID: 6430
		public Image spriteCoin;

		// Token: 0x0400191F RID: 6431
		public Image spriteCombatCard;

		// Token: 0x04001920 RID: 6432
		public Image spritePopularity;

		// Token: 0x04001921 RID: 6433
		public Image spritePower;

		// Token: 0x04001922 RID: 6434
		public Image spriteResource;

		// Token: 0x04001923 RID: 6435
		public Sprite differentSprite;

		// Token: 0x04001924 RID: 6436
		public Image gainProduceSprite;

		// Token: 0x04001925 RID: 6437
		public Image gainRecruitSprite;

		// Token: 0x04001926 RID: 6438
		public Image gainMechSprite;

		// Token: 0x04001927 RID: 6439
		public Image gainBuildingSprite;

		// Token: 0x04001928 RID: 6440
		public Image gainUpgradeSprite;

		// Token: 0x04001929 RID: 6441
		public Image gainWorkerSprite;

		// Token: 0x0400192A RID: 6442
		public Sprite disabledCard;

		// Token: 0x0400192B RID: 6443
		public Transform cardSelectionContainer;

		// Token: 0x0400192C RID: 6444
		public Text cardSelectionInfo;

		// Token: 0x0400192E RID: 6446
		private bool cardChoosen;

		// Token: 0x0400192F RID: 6447
		private bool displayedCards;

		// Token: 0x02000473 RID: 1139
		// (Invoke) Token: 0x0600240E RID: 9230
		public delegate void FactoryCardChoose();
	}
}
