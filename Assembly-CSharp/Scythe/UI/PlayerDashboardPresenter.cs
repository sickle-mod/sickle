using System;
using DG.Tweening;
using I2.Loc;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004B4 RID: 1204
	public class PlayerDashboardPresenter : MonoBehaviour
	{
		// Token: 0x06002633 RID: 9779 RVA: 0x000E30C8 File Offset: 0x000E12C8
		public void Show(Player player)
		{
			if (this.hideSequence != null)
			{
				this.hideSequence.Kill(false);
				this.hideSequence = null;
			}
			this.canvasGroup.DOFade(1f, this.fadeTime);
			base.gameObject.SetActive(true);
			this.UpdateDashboard(player);
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x00040751 File Offset: 0x0003E951
		public void Hide()
		{
			this.hideSequence = DOTween.Sequence();
			this.hideSequence.Append(this.canvasGroup.DOFade(0f, this.fadeTime)).AppendCallback(delegate
			{
				base.gameObject.SetActive(false);
			});
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x00040791 File Offset: 0x0003E991
		private void UpdateDashboard(Player player)
		{
			this.UpdateGeneralInformation(player);
			this.UpdateFactionAbility(player);
			this.UpdateMechs(player);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000E311C File Offset: 0x000E131C
		private void UpdateGeneralInformation(Player player)
		{
			this.logoSprite.sprite = GameController.factionInfo[player.matFaction.faction].logo;
			this.factionName.text = ScriptLocalization.Get("FactionMat/" + player.matFaction.faction.ToString());
			this.playerName.text = player.Name;
			int numberOfStars = player.GetNumberOfStars();
			for (int i = 0; i < this.starsParent.transform.childCount; i++)
			{
				this.starsParent.transform.GetChild(i).GetComponent<Image>().color = ((i < numberOfStars) ? this.activeStarColor : this.inactiveStarColor);
			}
			this.matType.text = ScriptLocalization.Get("PlayerMat/" + player.matPlayer.matType.ToString());
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000E3214 File Offset: 0x000E1414
		private void UpdateFactionAbility(Player player)
		{
			FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[player.matFaction.faction];
			this.factionAbilityTitle.text = factionBasicInfo.abilityName.ToUpper();
			bool flag = player.matFaction.faction == Faction.Polania && GameController.GameManager.players.Count > 5;
			this.factionAbilityDescription.text = (flag ? ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityDescriptionA").Replace("|", Environment.NewLine) : ScriptLocalization.Get("FactionMat/" + player.matFaction.faction.ToString() + "FactionAbilityDescription").Replace("|", Environment.NewLine));
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000E32D8 File Offset: 0x000E14D8
		private void UpdateMechs(Player player)
		{
			for (int i = 0; i < 4; i++)
			{
				FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[player.matFaction.faction];
				GameController.FactionInfo factionInfo = GameController.factionInfo[player.matFaction.faction];
				bool flag = factionInfo.faction == Faction.Crimea && GameController.GameManager.players.Count > 5 && i == 1;
				this.mechAbilityIcons[i].sprite = (flag ? GameController.Instance.matFaction.CrimeaWayfareA : factionInfo.mechAbilityIcons[i]);
				this.mechAbilityTitles[i].text = factionBasicInfo.mechAbilityTitles[i];
				this.mechAbilityDescriptions[i].text = (flag ? ScriptLocalization.Get("FactionMat/CrimeaMechAbilityDescription2A").Replace("|", Environment.NewLine) : factionBasicInfo.mechAbilityDescriptions[i].Replace("|", Environment.NewLine));
				if (!player.matFaction.SkillUnlocked[i])
				{
					this.mechAbilityIcons[i].material = this.grayUI;
					this.mechAbilityTitles[i].color = (this.mechAbilityDescriptions[i].color = this.mechTitleColorInactive);
					this.mechButton[i].image.sprite = this.mechTileInactive;
				}
				else
				{
					this.mechAbilityIcons[i].material = null;
					this.mechAbilityTitles[i].color = this.mechTitleColorActive;
					this.mechAbilityDescriptions[i].color = this.mechDescriptionColorActive;
					this.mechButton[i].image.sprite = this.mechTileActive;
				}
				this.mechButton[i].enabled = true;
				this.mechButton[i].interactable = false;
			}
		}

		// Token: 0x04001B26 RID: 6950
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04001B27 RID: 6951
		[SerializeField]
		private float fadeTime;

		// Token: 0x04001B28 RID: 6952
		[SerializeField]
		private Material grayUI;

		// Token: 0x04001B29 RID: 6953
		[Header("General")]
		[SerializeField]
		private Image logoSprite;

		// Token: 0x04001B2A RID: 6954
		[SerializeField]
		private TextMeshProUGUI factionName;

		// Token: 0x04001B2B RID: 6955
		[SerializeField]
		private TextMeshProUGUI playerName;

		// Token: 0x04001B2C RID: 6956
		[SerializeField]
		private TextMeshProUGUI matType;

		// Token: 0x04001B2D RID: 6957
		[SerializeField]
		private Transform starsParent;

		// Token: 0x04001B2E RID: 6958
		[SerializeField]
		private Color activeStarColor;

		// Token: 0x04001B2F RID: 6959
		[SerializeField]
		private Color inactiveStarColor;

		// Token: 0x04001B30 RID: 6960
		[Header("Faction ability")]
		[SerializeField]
		private Text factionAbilityTitle;

		// Token: 0x04001B31 RID: 6961
		[SerializeField]
		private Text factionAbilityDescription;

		// Token: 0x04001B32 RID: 6962
		[Header("Mechs")]
		[SerializeField]
		private Image[] mechAbilityIcons;

		// Token: 0x04001B33 RID: 6963
		[SerializeField]
		private TextMeshProUGUI[] mechAbilityTitles;

		// Token: 0x04001B34 RID: 6964
		[SerializeField]
		private Text[] mechAbilityDescriptions;

		// Token: 0x04001B35 RID: 6965
		[SerializeField]
		private Image[] mechGlow;

		// Token: 0x04001B36 RID: 6966
		[SerializeField]
		private Button[] mechButton;

		// Token: 0x04001B37 RID: 6967
		[SerializeField]
		private Sprite mechTileActive;

		// Token: 0x04001B38 RID: 6968
		[SerializeField]
		private Sprite mechTileInactive;

		// Token: 0x04001B39 RID: 6969
		[SerializeField]
		private Color mechTitleColorActive = new Color(1f, 0.875f, 0f, 0.875f);

		// Token: 0x04001B3A RID: 6970
		[SerializeField]
		private Color mechTitleColorInactive = new Color(0.5f, 0.5f, 0.5f, 0.75f);

		// Token: 0x04001B3B RID: 6971
		[SerializeField]
		private Color mechDescriptionColorActive = new Color(1f, 1f, 1f, 0.875f);

		// Token: 0x04001B3C RID: 6972
		private Sequence hideSequence;
	}
}
