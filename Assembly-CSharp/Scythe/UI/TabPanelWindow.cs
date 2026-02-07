using System;
using System.Collections.Generic;
using HoneyFramework;
using I2.Loc;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004BF RID: 1215
	public class TabPanelWindow : MonoBehaviour
	{
		// Token: 0x060026A0 RID: 9888 RVA: 0x00040ACB File Offset: 0x0003ECCB
		private void Awake()
		{
			OptionsManager.OnLanguageChanged += this.UpdateTexts;
			TokensController.OnTokenPlaced += this.UpdateTokens;
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x000E4F38 File Offset: 0x000E3138
		private void Start()
		{
			if (!GameController.Game.GameManager.IsMultiplayer && GameController.Game.GameManager.GetPlayersWithoutAICount() > 1)
			{
				this.HideObjectiveCardPreviewTitles();
			}
			Faction faction = Faction.Nordic;
			if (GameController.GameManager.IsMultiplayer)
			{
				faction = GameController.GameManager.PlayerOwner.matFaction.faction;
			}
			else if (GameController.GameManager.IsHotSeat)
			{
				List<Player> players = GameController.GameManager.GetPlayers();
				Player player = GameController.GameManager.PlayerCurrent;
				while (!player.IsHuman)
				{
					player = players[(GameController.GameManager.GetPlayerLocalId(player) + 1) % players.Count];
				}
				faction = player.matFaction.faction;
			}
			switch (faction)
			{
			case Faction.Polania:
				this.leftPanelMechBG.sprite = this.mechFactionBG[0];
				return;
			case Faction.Albion:
				this.leftPanelMechBG.sprite = this.mechFactionBG[5];
				return;
			case Faction.Nordic:
				this.leftPanelMechBG.sprite = this.mechFactionBG[1];
				return;
			case Faction.Rusviet:
				this.leftPanelMechBG.sprite = this.mechFactionBG[2];
				return;
			case Faction.Togawa:
				this.leftPanelMechBG.sprite = this.mechFactionBG[6];
				return;
			case Faction.Crimea:
				this.leftPanelMechBG.sprite = this.mechFactionBG[3];
				return;
			case Faction.Saxony:
				this.leftPanelMechBG.sprite = this.mechFactionBG[4];
				return;
			default:
				return;
			}
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x000E5094 File Offset: 0x000E3294
		public void MechPanelUpgrade()
		{
			switch (GameController.GameManager.PlayerCurrent.matFaction.faction)
			{
			case Faction.Polania:
				this.leftPanelMechBG.sprite = this.mechFactionBG[0];
				return;
			case Faction.Albion:
				this.leftPanelMechBG.sprite = this.mechFactionBG[5];
				return;
			case Faction.Nordic:
				this.leftPanelMechBG.sprite = this.mechFactionBG[1];
				return;
			case Faction.Rusviet:
				this.leftPanelMechBG.sprite = this.mechFactionBG[2];
				return;
			case Faction.Togawa:
				this.leftPanelMechBG.sprite = this.mechFactionBG[6];
				return;
			case Faction.Crimea:
				this.leftPanelMechBG.sprite = this.mechFactionBG[3];
				return;
			case Faction.Saxony:
				this.leftPanelMechBG.sprite = this.mechFactionBG[4];
				return;
			default:
				return;
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x00040AEF File Offset: 0x0003ECEF
		private void OnDestroy()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
			TokensController.OnTokenPlaced -= this.UpdateTokens;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000E5164 File Offset: 0x000E3364
		public void Init(StructureBonusCard structCard)
		{
			this.structureBonusTitle.text = GameController.GetStructureBonusName(structCard.CardId);
			this.structureBonusDescription.text = GameController.GetStructureBonusDescription(structCard.CardId);
			this.structureBonusValues.text = structCard.structureBonus.ToString();
			this.StructureBonusIcon.sprite = this.StructureBonusSymbols[structCard.CardId - 1];
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000E51D0 File Offset: 0x000E33D0
		public void UpdatePlayerInfo(GameController.FactionInfo factionInfo, FactionBasicInfo factionBasicInfo, List<ObjectiveCard> objectiveCards, List<CombatCard> combatCards, bool preview = false)
		{
			Image[] array = this.logos;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sprite = factionInfo.logo;
			}
			this.factionAbilityTitle.text = factionBasicInfo.abilityName.ToUpper();
			if (factionInfo.faction == Faction.Polania && GameController.GameManager.players.Count > 5)
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityDescriptionA").Replace("|", Environment.NewLine);
			}
			else
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/" + factionInfo.faction.ToString() + "FactionAbilityDescription").Replace("|", Environment.NewLine);
			}
			this.factionBackground.sprite = this.backgrounds[(int)factionInfo.faction];
			this.SetObjectiveCards(objectiveCards);
			if (this.combatCardsPresenter != null)
			{
				GameController.Instance.matFaction.combatCardsPresenter.SetCards(combatCards, null);
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000E52DC File Offset: 0x000E34DC
		public void UpdateObjectiveButtonLabels(List<ObjectiveCard> objectiveCards, int objectiveIndex)
		{
			TextMeshProUGUI component = this.objectiveCardButton[objectiveIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			if (objectiveIndex >= objectiveCards.Count || objectiveCards[objectiveIndex].CanDoActionAfterCompletingObjective())
			{
				component.text = ScriptLocalization.Get("GameScene/Complete");
				return;
			}
			if (GameController.GameManager.PlayerCurrent.currentMatSection == -1 && objectiveCards[objectiveIndex].status == ObjectiveCard.ObjectiveStatus.Open && objectiveCards[objectiveIndex].CheckCondition())
			{
				component.text = ScriptLocalization.Get("GameScene/BlockedObjectiveInfo");
				return;
			}
			component.text = ScriptLocalization.Get("GameScene/Complete") + " & " + ScriptLocalization.Get("GameScene/EndTurn");
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000E5390 File Offset: 0x000E3590
		public void SetObjectiveCards(List<ObjectiveCard> objectiveCards)
		{
			this.objectiveCards = objectiveCards;
			this.UpdateObjectivesTexts(objectiveCards);
			for (int i = 0; i < objectiveCards.Count; i++)
			{
				Graphic component = this.objectiveCardButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
				this.objectiveCardButton[i].interactable = false;
				component.color = this.CompleteLabelInactive;
				if (GameController.GameManager.IsAIHotSeat || GameController.GameManager.IsMultiplayer)
				{
					this.objectivePreviewTitle[i].transform.gameObject.SetActive(objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Open);
				}
				this.objectiveStatusCompleted[i].SetActive(objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Completed);
				this.objectiveStatusDisabled[i].SetActive(objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Disabled);
				Color color = this.objectiveCardTitle[i].color;
				if (objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Open)
				{
					color.a = 1f;
				}
				else
				{
					color.a = 0.4f;
				}
				this.objectiveCardTitle[i].color = color;
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000E54B0 File Offset: 0x000E36B0
		private void UpdateTexts()
		{
			StructureBonusCard structureBonus = GameController.GameManager.StructureBonus;
			this.structureBonusTitle.text = GameController.GetStructureBonusName(structureBonus.CardId);
			this.structureBonusDescription.text = GameController.GetStructureBonusDescription(structureBonus.CardId);
			this.structureBonusValues.text = structureBonus.structureBonus.ToString();
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			GameController.FactionInfo factionInfo = GameController.factionInfo[player.matFaction.faction];
			FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[player.matFaction.faction];
			this.factionAbilityTitle.text = factionBasicInfo.abilityName.ToUpper();
			if (factionInfo.faction == Faction.Polania && GameController.GameManager.players.Count > 5)
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityDescriptionA").Replace("|", Environment.NewLine);
			}
			else
			{
				this.factionAbilityDescription.text = ScriptLocalization.Get("FactionMat/" + factionInfo.faction.ToString() + "FactionAbilityDescription").Replace("|", Environment.NewLine);
			}
			this.factionBackground.sprite = this.backgrounds[(int)factionInfo.faction];
			this.UpdateObjectivesTexts(player.objectiveCards);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000E5638 File Offset: 0x000E3838
		private void UpdateObjectivesTexts(List<ObjectiveCard> objectiveCards)
		{
			for (int i = 0; i < objectiveCards.Count; i++)
			{
				this.objectivePreviewTitle[i].SetText(GameController.GetObjectiveTitle(objectiveCards[i].CardId));
				this.objectiveCardTitle[i].text = GameController.GetObjectiveTitle(objectiveCards[i].CardId);
				this.objectiveCardText[i].text = GameController.GetObjectiveDescription(objectiveCards[i].CardId);
				string text = objectiveCards[i].CardId.ToString().PadLeft(2, '0');
				if (this.objectiveCardImage[i].sprite == null || !this.objectiveCardImage[i].sprite.name.EndsWith(text))
				{
					AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_objectives");
					this.objectiveCardImage[i].sprite = assetBundle.LoadAllAssets<Sprite>()[objectiveCards[i].CardId - 1];
				}
				this.UpdateObjectiveButtonLabels(objectiveCards, i);
			}
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000E5738 File Offset: 0x000E3938
		public bool IsAnyObjectiveReady()
		{
			return GameController.GameManager.PlayerCurrent.objectiveCards.Count != 0 && ((GameController.GameManager.PlayerCurrent.objectiveCards[0].CheckCondition() && GameController.GameManager.PlayerCurrent.objectiveCards[0].status == ObjectiveCard.ObjectiveStatus.Open) || (GameController.GameManager.PlayerCurrent.objectiveCards[1].CheckCondition() && GameController.GameManager.PlayerCurrent.objectiveCards[1].status == ObjectiveCard.ObjectiveStatus.Open));
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000E57D4 File Offset: 0x000E39D4
		public void FocusObjectiveCard(int index, bool focus)
		{
			if (GameController.GameManager.GameFinished)
			{
				return;
			}
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			this.UpdateObjectiveButtonLabels(player.objectiveCards, index);
			if (focus)
			{
				this.objectivePreviewTitle[index].SwitchStateToHighlighted();
			}
			else
			{
				this.objectivePreviewTitle[index].SwitchStateToIdle();
			}
			this.objectivePreviewTitle[index].IsActiveToClear = focus;
			this.objectiveCardButton[index].interactable = focus;
			this.objectiveCardButton[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = (focus ? this.CompleteLabelActive : this.CompleteLabelInactive);
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000E58A8 File Offset: 0x000E3AA8
		public void FocusAllObjectiveCards(bool focus)
		{
			for (int i = 0; i < this.objectiveCardPanel.Length; i++)
			{
				this.FocusObjectiveCard(i, focus);
			}
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x000E58D0 File Offset: 0x000E3AD0
		public void DisableEndTurnObjectives()
		{
			for (int i = 0; i < this.objectiveCardPanel.Length; i++)
			{
				if (this.objectiveCards != null && this.objectiveCards.Count > 0 && !this.objectiveCards[i].CanDoActionAfterCompletingObjective())
				{
					this.FocusObjectiveCard(i, false);
				}
			}
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000E5924 File Offset: 0x000E3B24
		public void OnObjectivePanelToggled()
		{
			if (this.objectiveCardPanel[0].activeInHierarchy)
			{
				ObjectivePreview[] array = this.objectivePreviewTitle;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SwitchStateToIdle();
				}
			}
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x00040B13 File Offset: 0x0003ED13
		public void OnObjectivePreviewClicked()
		{
			WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
			this.objectiveToggle.isOn = !this.objectiveToggle.isOn;
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000E5960 File Offset: 0x000E3B60
		public void HideObjectiveCardPreviewTitles()
		{
			ObjectivePreview[] array = this.objectivePreviewTitle;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.parent.gameObject.SetActive(false);
			}
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000E599C File Offset: 0x000E3B9C
		public void ShowObjectiveCardPreviewTitles()
		{
			ObjectivePreview[] array = this.objectivePreviewTitle;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.parent.gameObject.SetActive(true);
			}
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000E59D8 File Offset: 0x000E3BD8
		public void DisableObjectiveCardsTab()
		{
			if (GameController.Instance.tabToggles.Count > 0)
			{
				GameController.Instance.tabToggles[3].interactable = false;
				GameController.Instance.tabToggles[3].image.sprite = this.objectiveCardsCover;
			}
			AssetBundleManager.UnloadAssetBundle("graphic_objectives", false);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000E5A3C File Offset: 0x000E3C3C
		public void OnShowFactionPanel()
		{
			Player player = this.GetPlayer();
			if (player.matFaction.faction == Faction.Albion || player.matFaction.faction == Faction.Togawa)
			{
				this.tokensContainer.SetActive(true);
				this.UpdateTokens();
				return;
			}
			this.tokensContainer.SetActive(false);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000E5A8C File Offset: 0x000E3C8C
		public void UpdateTokens()
		{
			Player player = this.GetPlayer();
			for (int i = 0; i < this.tokensIcons.Length; i++)
			{
				Color color = this.tokensIcons[i].color;
				Faction faction = player.matFaction.faction;
				if (faction != Faction.Albion)
				{
					if (faction == Faction.Togawa)
					{
						this.tokensIcons[i].sprite = this.togawaSprites[i];
						TrapToken trapToken = (TrapToken)player.matFaction.FactionTokens.GetToken(i);
						switch (trapToken.Penalty)
						{
						case PayType.Coin:
							this.tokensIcons[i].sprite = this.togawaSprites[0];
							break;
						case PayType.Popularity:
							this.tokensIcons[i].sprite = this.togawaSprites[1];
							break;
						case PayType.Power:
							this.tokensIcons[i].sprite = this.togawaSprites[2];
							break;
						case PayType.CombatCard:
							this.tokensIcons[i].sprite = this.togawaSprites[3];
							break;
						case PayType.Resource:
							this.tokensIcons[i].sprite = null;
							break;
						default:
							DebugLog.LogError("Unexpected trap token penalty");
							break;
						}
						if (GameController.GameManager.SpectatorMode)
						{
							color.a = 1f;
						}
						else if (trapToken.IsTokenPlaced())
						{
							color.a = 0.5f;
						}
						else
						{
							color.a = 1f;
						}
					}
				}
				else
				{
					this.tokensIcons[i].sprite = this.albionSprite;
					if (GameController.GameManager.SpectatorMode)
					{
						color.a = 1f;
					}
					else if (i < player.matFaction.FactionTokens.GetUnplacedTokensCount())
					{
						color.a = 1f;
					}
					else
					{
						color.a = 0.5f;
					}
				}
				this.tokensIcons[i].color = color;
			}
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0008A35C File Offset: 0x0008855C
		private Player GetPlayer()
		{
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			return player;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000E5C5C File Offset: 0x000E3E5C
		public void OnTokensHoover(bool isHovering)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.GetGameHexPresentersWithTokens())
			{
				if (isHovering)
				{
					gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.MoveToEncounter, 0f, false);
				}
				else
				{
					gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.MoveToEncounter, 0f, false);
				}
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000E5CCC File Offset: 0x000E3ECC
		private List<Scythe.BoardPresenter.GameHexPresenter> GetGameHexPresentersWithTokens()
		{
			Player player = this.GetPlayer();
			List<Scythe.BoardPresenter.GameHexPresenter> list = new List<Scythe.BoardPresenter.GameHexPresenter>();
			for (int i = 0; i < player.matFaction.FactionTokens.GetTokensCount(); i++)
			{
				FactionAbilityToken token = player.matFaction.FactionTokens.GetToken(i);
				if (token.IsTokenPlaced())
				{
					list.Add(GameController.Instance.gameBoardPresenter.GetGameHexPresenter(token.Position));
				}
			}
			return list;
		}

		// Token: 0x04001B99 RID: 7065
		public Toggle2 objectiveToggle;

		// Token: 0x04001B9A RID: 7066
		public Text structureBonusTitle;

		// Token: 0x04001B9B RID: 7067
		public Text structureBonusDescription;

		// Token: 0x04001B9C RID: 7068
		public Text structureBonusValues;

		// Token: 0x04001B9D RID: 7069
		public Image StructureBonusIcon;

		// Token: 0x04001B9E RID: 7070
		public Sprite[] StructureBonusSymbols;

		// Token: 0x04001B9F RID: 7071
		public Text factionAbilityTitle;

		// Token: 0x04001BA0 RID: 7072
		public Text factionAbilityDescription;

		// Token: 0x04001BA1 RID: 7073
		public Image factionBackground;

		// Token: 0x04001BA2 RID: 7074
		public Image[] logos;

		// Token: 0x04001BA3 RID: 7075
		public Sprite[] backgrounds;

		// Token: 0x04001BA4 RID: 7076
		public CombatCardsPanelPresenter combatCardsPresenter;

		// Token: 0x04001BA5 RID: 7077
		public Text[] objectiveCardTitle = new Text[2];

		// Token: 0x04001BA6 RID: 7078
		public Text[] objectiveCardText = new Text[2];

		// Token: 0x04001BA7 RID: 7079
		public Button[] objectiveCardButton = new Button[2];

		// Token: 0x04001BA8 RID: 7080
		public GameObject[] objectiveStatusCompleted = new GameObject[2];

		// Token: 0x04001BA9 RID: 7081
		public GameObject[] objectiveStatusDisabled = new GameObject[2];

		// Token: 0x04001BAA RID: 7082
		public GameObject[] objectiveCardPanel = new GameObject[2];

		// Token: 0x04001BAB RID: 7083
		public Image[] objectiveCardImage = new Image[2];

		// Token: 0x04001BAC RID: 7084
		public ObjectivePreview[] objectivePreviewTitle = new ObjectivePreview[2];

		// Token: 0x04001BAD RID: 7085
		public Color CompleteLabelActive;

		// Token: 0x04001BAE RID: 7086
		public Color CompleteLabelInactive;

		// Token: 0x04001BAF RID: 7087
		public Sprite[] mechFactionBG = new Sprite[7];

		// Token: 0x04001BB0 RID: 7088
		public Image leftPanelMechBG;

		// Token: 0x04001BB1 RID: 7089
		public Sprite objectiveCardsCover;

		// Token: 0x04001BB2 RID: 7090
		[Header("DLC")]
		public Image[] tokensIcons;

		// Token: 0x04001BB3 RID: 7091
		public GameObject tokensContainer;

		// Token: 0x04001BB4 RID: 7092
		public Sprite albionSprite;

		// Token: 0x04001BB5 RID: 7093
		public Sprite[] togawaSprites;

		// Token: 0x04001BB6 RID: 7094
		private List<ObjectiveCard> objectiveCards;
	}
}
