using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DC RID: 220
public class EnemyInfoPreview : MonoBehaviour
{
	// Token: 0x06000673 RID: 1651 RVA: 0x0002C1D0 File Offset: 0x0002A3D0
	private void Awake()
	{
		EnemyInfoPreview.Instance = this;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
	public bool GetPreviewVisible()
	{
		return this.previewVisible;
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0002C1E0 File Offset: 0x0002A3E0
	public void Visibility(bool show, Player player)
	{
		if (player != null)
		{
			if (!show)
			{
				if (!this.factionOrderEnlarge.isMouseOnLogos())
				{
					this.TurnOffEnemyPlayerInfo();
					return;
				}
			}
			else
			{
				this.TurnOnEnemyPlayerInfo(player);
			}
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0006FDC8 File Offset: 0x0006DFC8
	public void TurnOnEnemyPlayerInfo(Player player)
	{
		this.previewVisible = true;
		this.playerOwner = player;
		GameController.FactionInfo factionInfo = GameController.factionInfo[player.matFaction.faction];
		this.enemyInfoPreviewUIParent.SetActive(true);
		this.factionOrderEnlarge.SetupFactionLogosReference();
		this.factionOrderEnlarge.gameObject.SetActive(true);
		this.factionOrderEnlarge.MovePanelActivation(true);
		this.enemyFactionName.text = ScriptLocalization.Get("FactionMat/" + player.matFaction.faction.ToString());
		this.factionLogo.sprite = factionInfo.logo;
		this.SetupFactionBackground(player.matFaction.faction);
		this.SetupMechAbilities(factionInfo, player);
		this.UpdateMechButtons();
		this.SetupFactionAbilityInfo(factionInfo, GameController.GameManager.factionBasicInfo[player.matFaction.faction], player);
		this.playerStatsPresenter.UpdatePlayerStats(player, GameController.factionInfo[player.matFaction.faction].logo);
		this.objectivesPreviewButton.SetActive(false);
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0006FEE4 File Offset: 0x0006E0E4
	public void TurnOffEnemyPlayerInfo()
	{
		this.factionOrderEnlarge.gameObject.SetActive(false);
		this.factionOrderEnlarge.MovePanelActivation(false);
		this.enemyInfoPreviewUIParent.SetActive(false);
		this.objectivesPreviewButton.SetActive(true);
		this.matPreview.gameObject.SetActive(false);
		this.previewVisible = false;
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0006FF40 File Offset: 0x0006E140
	public void SetupFactionAbilityInfo(GameController.FactionInfo factionInfo, FactionBasicInfo factionBasicInfo, Player player)
	{
		if (factionInfo.faction == Faction.Polania && GameController.GameManager.players.Count > 5)
		{
			this.factionAbility.text = ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityDescriptionA").Replace("|", Environment.NewLine);
			return;
		}
		this.factionAbility.text = factionBasicInfo.abilityDescription.Replace("|", Environment.NewLine);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0006FFAC File Offset: 0x0006E1AC
	public void SetupMechAbilities(GameController.FactionInfo factionInfo, Player player)
	{
		for (int i = 0; i < 4; i++)
		{
			FactionBasicInfo factionBasicInfo = GameController.GameManager.factionBasicInfo[player.matFaction.faction];
			bool flag = factionInfo.faction == Faction.Crimea && GameController.GameManager.players.Count > 5 && i == 1;
			this.mechAbilityIcons[i].sprite = (flag ? this.CrimeaWayfareA : factionInfo.mechAbilityIcons[i]);
			this.mechAbilityTitles[i].text = factionBasicInfo.mechAbilityTitles[i];
			this.mechAbilityDescriptions[i].text = (flag ? ScriptLocalization.Get("FactionMat/CrimeaMechAbilityDescription2A").Replace("|", Environment.NewLine) : factionBasicInfo.mechAbilityDescriptions[i].Replace("|", Environment.NewLine));
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0002C203 File Offset: 0x0002A403
	public bool IsEnemyInfoVisible()
	{
		return this.enemyInfoPreviewUIParent.activeInHierarchy;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00070080 File Offset: 0x0006E280
	public void UpdateMechButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			if (!this.playerOwner.matFaction.SkillUnlocked[i])
			{
				this.mechAbilityIcons[i].material = this.sepiaUI;
				this.mechAbilityTitles[i].color = (this.mechAbilityDescriptions[i].color = this.colInactive);
				this.mechBackground[i].color = this.backgroundInActive;
			}
			else
			{
				this.mechAbilityIcons[i].material = null;
				this.mechAbilityTitles[i].color = (this.mechAbilityDescriptions[i].color = this.colActive);
				this.mechBackground[i].color = this.backgroundActive;
			}
		}
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00070144 File Offset: 0x0006E344
	private void SetupFactionBackground(Faction factionToSetup)
	{
		switch (factionToSetup)
		{
		case Faction.Polania:
			this.factionBackground.sprite = this.factionBackgrounds[0];
			return;
		case Faction.Albion:
			this.factionBackground.sprite = this.factionBackgrounds[5];
			return;
		case Faction.Nordic:
			this.factionBackground.sprite = this.factionBackgrounds[1];
			return;
		case Faction.Rusviet:
			this.factionBackground.sprite = this.factionBackgrounds[2];
			return;
		case Faction.Togawa:
			this.factionBackground.sprite = this.factionBackgrounds[6];
			return;
		case Faction.Crimea:
			this.factionBackground.sprite = this.factionBackgrounds[3];
			return;
		case Faction.Saxony:
			this.factionBackground.sprite = this.factionBackgrounds[4];
			return;
		default:
			return;
		}
	}

	// Token: 0x04000582 RID: 1410
	public static EnemyInfoPreview Instance;

	// Token: 0x04000583 RID: 1411
	[SerializeField]
	private GameObject enemyInfoPreviewUIParent;

	// Token: 0x04000584 RID: 1412
	[SerializeField]
	private GameObject objectivesPreviewButton;

	// Token: 0x04000585 RID: 1413
	[SerializeField]
	private TextMeshProUGUI enemyFactionName;

	// Token: 0x04000586 RID: 1414
	[SerializeField]
	private TextMeshProUGUI factionAbility;

	// Token: 0x04000587 RID: 1415
	[SerializeField]
	private TextMeshProUGUI[] mechAbilityTitles;

	// Token: 0x04000588 RID: 1416
	[SerializeField]
	private TextMeshProUGUI[] mechAbilityDescriptions;

	// Token: 0x04000589 RID: 1417
	[SerializeField]
	private Image[] mechAbilityIcons;

	// Token: 0x0400058A RID: 1418
	[SerializeField]
	private Sprite CrimeaWayfareA;

	// Token: 0x0400058B RID: 1419
	[SerializeField]
	private Image factionLogo;

	// Token: 0x0400058C RID: 1420
	[SerializeField]
	private Image[] mechBackground;

	// Token: 0x0400058D RID: 1421
	[SerializeField]
	private Material sepiaUI;

	// Token: 0x0400058E RID: 1422
	[SerializeField]
	private Sprite[] factionBackgrounds = new Sprite[7];

	// Token: 0x0400058F RID: 1423
	[SerializeField]
	private Image factionBackground;

	// Token: 0x04000590 RID: 1424
	[SerializeField]
	private FactionOrderEnlarge factionOrderEnlarge;

	// Token: 0x04000591 RID: 1425
	[SerializeField]
	private PlayerStatsPresenter playerStatsPresenter;

	// Token: 0x04000592 RID: 1426
	[SerializeField]
	private MatPreview matPreview;

	// Token: 0x04000593 RID: 1427
	private Color colInactive = new Color(1f, 1f, 1f, 0.2f);

	// Token: 0x04000594 RID: 1428
	private Color colActive = new Color(1f, 1f, 1f, 0.75f);

	// Token: 0x04000595 RID: 1429
	private Color backgroundActive = new Color(0f, 0f, 0f, 1f);

	// Token: 0x04000596 RID: 1430
	private Color backgroundInActive = new Color(0f, 0f, 0f, 0.6f);

	// Token: 0x04000597 RID: 1431
	private Player playerOwner;

	// Token: 0x04000598 RID: 1432
	[SerializeField]
	private bool previewVisible;
}
