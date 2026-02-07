using System;
using Scythe.Analytics;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004AE RID: 1198
	public class TopMenuStructureBonus : MonoBehaviour
	{
		// Token: 0x060025F6 RID: 9718 RVA: 0x000403F7 File Offset: 0x0003E5F7
		private void Awake()
		{
			OptionsManager.OnLanguageChanged += this.UpdateTexts;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0004040A File Offset: 0x0003E60A
		public void OnToggleClicked()
		{
			if (!GameController.GameManager.IsCampaign)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.tab_structure_toggle);
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x0004042B File Offset: 0x0003E62B
		private void OnEnable()
		{
			this.UpdateTexts();
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_structure_bonus, Contexts.ingame);
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000E1AB8 File Offset: 0x000DFCB8
		public void Init(StructureBonusCard structCard)
		{
			this.structureBonusTitle.text = GameController.GetStructureBonusName(structCard.CardId);
			this.structureBonusDescription.text = GameController.GetStructureBonusDescription(structCard.CardId);
			this.structureBonusValues.text = this.ConstructStructureBonusValues(structCard);
			this.StructureBonusIcon.sprite = this.StructureBonusSymbols[structCard.CardId - 1];
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000E1B20 File Offset: 0x000DFD20
		public void UpdateUnlockedBuildings(Player player)
		{
			for (int i = 0; i < this.structureIcons.Length; i++)
			{
				if (player.matPlayer.GetBuilding((BuildingType)i) != null)
				{
					this.structureIcons[i].sprite = this.activeBuildingsSprites[i];
					this.SetImageOpacity(this.structureIcons[i], 1f);
					this.SetImageOpacity(this.structureActiveIcon[i], 1f);
					this.SetImageOpacity(this.structureBackgrounds[i], 1f);
					this.SetImageOpacity(this.scrollbarImages[i], 1f);
					this.SetTextOpacity(this.structureNames[i], 1f);
					this.SetTextOpacity(this.structureDescriptions[i], 1f);
				}
				else
				{
					this.structureIcons[i].sprite = this.inactiveBuildingsSprites[i];
					this.SetImageOpacity(this.structureIcons[i], 0.3f);
					this.SetImageOpacity(this.structureActiveIcon[i], 0.3f);
					this.SetImageOpacity(this.structureBackgrounds[i], 0.9f);
					this.SetImageOpacity(this.scrollbarImages[i], 0.3f);
					this.SetTextOpacity(this.structureNames[i], 0.3f);
					this.SetTextOpacity(this.structureDescriptions[i], 0.3f);
				}
			}
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x00040440 File Offset: 0x0003E640
		private void SetImageOpacity(Image image, float opacity)
		{
			image.color = this.ChangeColorOpactity(image.color, opacity);
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x00040440 File Offset: 0x0003E640
		private void SetTextOpacity(TextMeshProUGUI text, float opacity)
		{
			text.color = this.ChangeColorOpactity(text.color, opacity);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000E1C6C File Offset: 0x000DFE6C
		private Color ChangeColorOpactity(Color color, float opacity)
		{
			Color color2 = color;
			color2.a = opacity;
			return color2;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000E1C84 File Offset: 0x000DFE84
		private void UpdateTexts()
		{
			StructureBonusCard structureBonus = GameController.GameManager.StructureBonus;
			this.structureBonusTitle.text = GameController.GetStructureBonusName(structureBonus.CardId);
			this.structureBonusDescription.text = GameController.GetStructureBonusDescription(structureBonus.CardId);
			this.structureBonusValues.text = this.ConstructStructureBonusValues(structureBonus);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x00040455 File Offset: 0x0003E655
		private bool CheckStructureBonusPair(StructureBonusCard structCard, int bonusIndex, int bonusListCount)
		{
			return bonusIndex + 1 <= bonusListCount && structCard.structureBonus[bonusIndex] == structCard.structureBonus[bonusIndex + 1];
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000E1CDC File Offset: 0x000DFEDC
		private string ConstructStructureBonusValues(StructureBonusCard structCard)
		{
			string text = string.Empty;
			int num = 1;
			int num2 = 1;
			int num3 = structCard.structureBonus.BonusListCount();
			for (int i = 1; i < num3 + 1; i++)
			{
				if (this.CheckStructureBonusPair(structCard, i, num3))
				{
					text += num.ToString();
					num++;
					text = text + " - " + num.ToString();
					text = text + ": " + this.GetCoinsBonusForTier(num2);
					text += "        ";
					num++;
					i++;
				}
				else
				{
					text += num.ToString();
					text = text + ": " + this.GetCoinsBonusForTier(num2);
					text += "        ";
					num++;
				}
				num2++;
			}
			return text;
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0004047E File Offset: 0x0003E67E
		private string GetCoinsBonusForTier(int tier)
		{
			switch (tier)
			{
			default:
				return "<sprite name=\"2Coins\">";
			case 2:
				return "<sprite name=\"4Coins\">";
			case 3:
				return "<sprite name=\"6Coins\">";
			case 4:
				return "<sprite name=\"9Coins\">";
			}
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000404AF File Offset: 0x0003E6AF
		private void OnDestroy()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateTexts;
		}

		// Token: 0x04001AE6 RID: 6886
		public TextMeshProUGUI structureBonusTitle;

		// Token: 0x04001AE7 RID: 6887
		public TMP_Text structureBonusDescription;

		// Token: 0x04001AE8 RID: 6888
		public TextMeshProUGUI structureBonusValues;

		// Token: 0x04001AE9 RID: 6889
		public Image StructureBonusIcon;

		// Token: 0x04001AEA RID: 6890
		public Sprite[] StructureBonusSymbols;

		// Token: 0x04001AEB RID: 6891
		[SerializeField]
		private Image[] structureIcons;

		// Token: 0x04001AEC RID: 6892
		[SerializeField]
		private Image[] structureActiveIcon;

		// Token: 0x04001AED RID: 6893
		[SerializeField]
		private Sprite[] activeBuildingsSprites;

		// Token: 0x04001AEE RID: 6894
		[SerializeField]
		private Sprite[] inactiveBuildingsSprites;

		// Token: 0x04001AEF RID: 6895
		[SerializeField]
		private TextMeshProUGUI[] structureNames;

		// Token: 0x04001AF0 RID: 6896
		[SerializeField]
		private TextMeshProUGUI[] structureDescriptions;

		// Token: 0x04001AF1 RID: 6897
		[SerializeField]
		private Image[] structureBackgrounds;

		// Token: 0x04001AF2 RID: 6898
		[SerializeField]
		private Image[] scrollbarImages;

		// Token: 0x04001AF3 RID: 6899
		private const float ACTIVE_OPACITY = 1f;

		// Token: 0x04001AF4 RID: 6900
		private const float INACTIVE_OPACITY = 0.3f;

		// Token: 0x04001AF5 RID: 6901
		private const float BACKGROUND_INACTIVE_OPACITY = 0.9f;
	}
}
