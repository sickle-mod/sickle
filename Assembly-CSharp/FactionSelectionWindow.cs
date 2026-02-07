using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000EB RID: 235
public class FactionSelectionWindow : MonoBehaviour
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
	private int NUMBER_OF_FACTIONS
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x060006E6 RID: 1766 RVA: 0x00072068 File Offset: 0x00070268
	// (remove) Token: 0x060006E7 RID: 1767 RVA: 0x000720A0 File Offset: 0x000702A0
	public event Action OnClose;

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x060006E8 RID: 1768 RVA: 0x000720D8 File Offset: 0x000702D8
	// (remove) Token: 0x060006E9 RID: 1769 RVA: 0x00072110 File Offset: 0x00070310
	public event Action<int, int> OnPlayerFactionSelection;

	// Token: 0x060006EA RID: 1770 RVA: 0x00072148 File Offset: 0x00070348
	private void Awake()
	{
		Button button = this.previousFactionButton;
		if (button != null)
		{
			button.onClick.AddListener(new UnityAction(this.PreviousFactionButton_OnClick));
		}
		Button button2 = this.nextFactionButton;
		if (button2 != null)
		{
			button2.onClick.AddListener(new UnityAction(this.NextFactionButton_OnClick));
		}
		Button button3 = this.acceptFactionButton;
		if (button3 == null)
		{
			return;
		}
		button3.onClick.AddListener(new UnityAction(this.AcceptFactionButton_OnClick));
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x000721BC File Offset: 0x000703BC
	private void OnDestroy()
	{
		Button button = this.previousFactionButton;
		if (button != null)
		{
			button.onClick.RemoveListener(new UnityAction(this.PreviousFactionButton_OnClick));
		}
		Button button2 = this.nextFactionButton;
		if (button2 != null)
		{
			button2.onClick.RemoveListener(new UnityAction(this.NextFactionButton_OnClick));
		}
		Button button3 = this.acceptFactionButton;
		if (button3 == null)
		{
			return;
		}
		button3.onClick.RemoveListener(new UnityAction(this.AcceptFactionButton_OnClick));
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0002C6EC File Offset: 0x0002A8EC
	private void OnEnable()
	{
		base.StartCoroutine(this.UpdateScroll());
		this.DescriptionShrink();
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0002C701 File Offset: 0x0002A901
	public void EnableIFASelection(bool active)
	{
		if (PlatformManager.IsMobile && this.InvadersFromAfarUnlocked())
		{
			active = true;
		}
		this.IFASelectionUnlocked = active;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0002C71C File Offset: 0x0002A91C
	public void MoreThat5Players(bool state)
	{
		this.moreThan5Players = state;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00072230 File Offset: 0x00070430
	public int ChangeFactionSelection(int playerId, int currentFaction, bool next)
	{
		this.playerId = playerId;
		this.previouslySelectedFaction = currentFaction;
		this.currentFaction = currentFaction;
		this.UpdateFaction(next);
		return this.currentFaction;
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00072264 File Offset: 0x00070464
	public void EnableDetailedFactionSelection(int playerId, int currentFaction)
	{
		this.playerId = playerId;
		this.previouslySelectedFaction = currentFaction;
		this.currentFaction = currentFaction;
		this.UpdateFactionVisualRepresentation();
		base.gameObject.SetActive(true);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.faction_selecton_window, Contexts.outgame);
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x000722A8 File Offset: 0x000704A8
	private void UpdateFaction(bool next)
	{
		if (this.currentFaction != this.NUMBER_OF_FACTIONS)
		{
			this.UpdateBitInValue(ref this.selectedFactions, this.currentFaction, false);
		}
		do
		{
			if (this.currentFaction == this.NUMBER_OF_FACTIONS && next)
			{
				this.currentFaction = 0;
			}
			else if (this.currentFaction == 0 && !next)
			{
				this.currentFaction = this.NUMBER_OF_FACTIONS;
			}
			else
			{
				this.currentFaction += (next ? 1 : (-1));
				if ((this.currentFaction == 1 || this.currentFaction == 4) && (!this.InvadersFromAfarUnlocked() || !this.IFASelectionUnlocked))
				{
					this.currentFaction += (next ? 1 : (-1));
				}
			}
			if (this.currentFaction == this.NUMBER_OF_FACTIONS)
			{
				return;
			}
		}
		while (this.ValueBitSet(this.selectedFactions, this.currentFaction));
		this.UpdateBitInValue(ref this.selectedFactions, this.currentFaction, true);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0007238C File Offset: 0x0007058C
	private void UpdateFactionVisualRepresentation()
	{
		AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_backgrounds");
		this.factionLogo.sprite = this.factionLogos[this.currentFaction];
		if (this.currentFaction < 7)
		{
			this.background.sprite = assetBundle.LoadAsset<Sprite>(this.factionBackgroundNames[this.currentFaction]);
		}
		else
		{
			int num = (this.IFASelectionUnlocked ? 8 : 7);
			this.background.sprite = assetBundle.LoadAsset<Sprite>(this.factionBackgroundNames[num]);
		}
		if (this.currentFaction != this.NUMBER_OF_FACTIONS)
		{
			if (PlatformManager.IsStandalone)
			{
				this.factionDetails.SetActive(true);
			}
			else
			{
				this.randomOptionCover.SetActive(false);
				this.randomOptionCoverIFA.SetActive(false);
			}
			TMP_Text tmp_Text = this.factionName;
			string text = "FactionMat/";
			Faction faction = (Faction)this.currentFaction;
			tmp_Text.text = ScriptLocalization.Get(text + faction.ToString() + "Fullname");
			this.factionName.color = this.factionColors[this.currentFaction];
			if (this.factionCharacterName != null)
			{
				TMP_Text tmp_Text2 = this.factionCharacterName;
				string text2 = "FactionMat/";
				faction = (Faction)this.currentFaction;
				tmp_Text2.text = ScriptLocalization.Get(text2 + faction.ToString() + "Character");
			}
			if (PlatformManager.IsStandalone)
			{
				TMP_Text tmp_Text3 = this.factionDescription;
				string text3 = "FactionMat/";
				faction = (Faction)this.currentFaction;
				tmp_Text3.text = ScriptLocalization.Get(text3 + faction.ToString() + "FactionDescription");
			}
			else
			{
				string text4 = "FactionMat/";
				faction = (Faction)this.currentFaction;
				this.shortDescription = ScriptLocalization.Get(text4 + faction.ToString() + "FactionDescription").Substring(0, this.GetProperDescriptionLength());
				this.factionDescription.text = this.shortDescription + "...";
			}
			TMP_Text tmp_Text4 = this.factionAbilityName;
			string text5 = "FactionMat/";
			faction = (Faction)this.currentFaction;
			tmp_Text4.text = ScriptLocalization.Get(text5 + faction.ToString() + "FactionAbilityName");
			TMP_Text tmp_Text5 = this.factionAbilityDescription;
			string text7;
			if (this.currentFaction != 0 || !this.moreThan5Players)
			{
				string text6 = "FactionMat/";
				faction = (Faction)this.currentFaction;
				text7 = ScriptLocalization.Get(text6 + faction.ToString() + "FactionAbilityDescription").Replace("|", " ");
			}
			else
			{
				string text8 = "FactionMat/";
				faction = (Faction)this.currentFaction;
				text7 = ScriptLocalization.Get(text8 + faction.ToString() + "FactionAbilityDescriptionA").Replace("|", " ");
			}
			tmp_Text5.text = text7;
			this.powerBonus.text = MatFaction.GetStartingPower((Faction)this.currentFaction).ToString();
			this.ammoBonus.text = MatFaction.GetStartingAmmo((Faction)this.currentFaction).ToString();
			for (int i = 0; i < 4; i++)
			{
				TMP_Text tmp_Text6 = this.mechAbilityNames[i];
				string text9 = "FactionMat/";
				faction = (Faction)this.currentFaction;
				tmp_Text6.text = ScriptLocalization.Get(text9 + faction.ToString() + "MechAbilityTitle" + (i + 1).ToString());
				TMP_Text tmp_Text7 = this.mechAbilityDescriptions[i];
				string text11;
				if (i != 1 || this.currentFaction != 5 || !this.moreThan5Players)
				{
					string text10 = "FactionMat/";
					faction = (Faction)this.currentFaction;
					text11 = ScriptLocalization.Get(text10 + faction.ToString() + "MechAbilityDescription" + (i + 1).ToString()).Replace("|", " ");
				}
				else
				{
					string[] array = new string[5];
					array[0] = "FactionMat/";
					int num2 = 1;
					faction = (Faction)this.currentFaction;
					array[num2] = faction.ToString();
					array[2] = "MechAbilityDescription";
					array[3] = (i + 1).ToString();
					array[4] = "A";
					text11 = ScriptLocalization.Get(string.Concat(array)).Replace("|", " ");
				}
				tmp_Text7.text = text11;
				this.mechAbilityIcons[i].sprite = ((i == 1 && this.currentFaction == 5 && this.moreThan5Players) ? this.factionMechAbilities[this.factionMechAbilities.Count - 1] : this.factionMechAbilities[this.currentFaction * 4 + i]);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.scrolledDescription.GetComponent<RectTransform>());
			this.scrolledDescription.verticalNormalizedPosition = 1f;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.scrolledDescription.GetComponent<RectTransform>());
			return;
		}
		this.factionName.text = ScriptLocalization.Get("GameScene/Random") + "\n<size=50%>" + ScriptLocalization.Get("MainMenu/SelectRandomFaction");
		this.factionName.color = this.factionColors[this.currentFaction];
		if (this.factionCharacterName != null)
		{
			this.factionCharacterName.text = string.Empty;
		}
		this.factionDescription.text = "";
		if (PlatformManager.IsStandalone)
		{
			this.factionDetails.SetActive(false);
			return;
		}
		if (GameServiceController.Instance.InvadersFromAfarUnlocked())
		{
			this.randomOptionCoverIFA.SetActive(true);
			return;
		}
		this.randomOptionCover.SetActive(true);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002C725 File Offset: 0x0002A925
	private IEnumerator UpdateScroll()
	{
		yield return new WaitForEndOfFrame();
		this.scrolledDescription.verticalNormalizedPosition = 1f;
		yield break;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0002C734 File Offset: 0x0002A934
	public void UpdateSelectedFactions(int faction, bool state)
	{
		if (faction == this.NUMBER_OF_FACTIONS)
		{
			return;
		}
		this.UpdateBitInValue(ref this.selectedFactions, faction, state);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0002C74E File Offset: 0x0002A94E
	private void UpdateBitInValue(ref int value, int position, bool set)
	{
		if (set)
		{
			value |= 1 << position;
			return;
		}
		value &= ~(1 << position);
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002C76B File Offset: 0x0002A96B
	private bool ValueBitSet(int value, int position)
	{
		return (1 & (value >> position)) == 1;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x000728B4 File Offset: 0x00070AB4
	public bool IsLastAvaliableFaction(int faction)
	{
		int num = (1 << this.NUMBER_OF_FACTIONS) - 1;
		num &= ~(1 << faction);
		if (!GameServiceController.Instance.InvadersFromAfarUnlocked() || !this.IFASelectionUnlocked)
		{
			num &= -3;
			num &= -17;
		}
		return this.selectedFactions == num;
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0002C778 File Offset: 0x0002A978
	public bool IsFactionAvaliable(int faction)
	{
		return faction == this.NUMBER_OF_FACTIONS || !this.ValueBitSet(this.selectedFactions, faction);
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002C795 File Offset: 0x0002A995
	private bool InvadersFromAfarUnlocked()
	{
		return GameServiceController.Instance.InvadersFromAfarUnlocked();
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0002C7A1 File Offset: 0x0002A9A1
	public int GetPlayerId()
	{
		return this.playerId;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0002C7A9 File Offset: 0x0002A9A9
	public int GetCurrentFaction()
	{
		return this.currentFaction;
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x0002C7B1 File Offset: 0x0002A9B1
	public int GetSelectedFactions()
	{
		return this.selectedFactions;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002C7B9 File Offset: 0x0002A9B9
	public void SaveSelectedFactions()
	{
		PlayerPrefs.SetInt("HotseatSelectedFactions", this.selectedFactions);
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002C7CB File Offset: 0x0002A9CB
	public void LoadSelectedFactions()
	{
		this.selectedFactions = PlayerPrefs.GetInt("HotseatSelectedFactions", this.selectedFactions);
		Debug.Log(Convert.ToString(this.selectedFactions, 2));
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002C7F4 File Offset: 0x0002A9F4
	public void OnXButtonClicked()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_x_button);
		this.CloseWindow(true);
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00072900 File Offset: 0x00070B00
	public void CloseWindow(bool revertChanges = true)
	{
		if (this.currentFaction != this.previouslySelectedFaction && revertChanges)
		{
			this.UpdateSelectedFactions(this.currentFaction, !revertChanges);
			this.UpdateSelectedFactions(this.previouslySelectedFaction, revertChanges);
			this.currentFaction = this.previouslySelectedFaction;
			this.UpdateFactionVisualRepresentation();
		}
		base.gameObject.SetActive(false);
		AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.pass_and_play_setup, Contexts.outgame);
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0007296C File Offset: 0x00070B6C
	public void DescriptionExpand()
	{
		TMP_Text tmp_Text = this.factionDescription;
		string text = "FactionMat/";
		Faction faction = (Faction)this.currentFaction;
		tmp_Text.text = ScriptLocalization.Get(text + faction.ToString() + "FactionDescription");
		this.scrolledDescription.vertical = true;
		if (this.readMore != null)
		{
			this.readMore.GetComponentInChildren<TextMeshProUGUI>().text = ScriptLocalization.Get("MainMenu/ReadLess");
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x000729E0 File Offset: 0x00070BE0
	public void DescriptionShrink()
	{
		if (PlatformManager.IsStandalone)
		{
			return;
		}
		if (this.currentFaction != this.NUMBER_OF_FACTIONS)
		{
			string text = "FactionMat/";
			Faction faction = (Faction)this.currentFaction;
			this.shortDescription = ScriptLocalization.Get(text + faction.ToString() + "FactionDescription").Substring(0, this.GetProperDescriptionLength());
			this.factionDescription.text = this.shortDescription + "...";
		}
		this.scrolledDescription.vertical = false;
		this.descriptionScrollContent.anchoredPosition = new Vector2(0f, 0f);
		if (this.readMore != null)
		{
			this.readMore.GetComponentInChildren<TextMeshProUGUI>().text = ScriptLocalization.Get("MainMenu/ReadMore");
		}
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00072AA8 File Offset: 0x00070CA8
	private int GetProperDescriptionLength()
	{
		string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
		uint num = global::<PrivateImplementationDetails>.ComputeStringHash(currentLanguageCode);
		if (num <= 1195724803U)
		{
			if (num <= 1111292255U)
			{
				if (num != 637978675U)
				{
					if (num != 1092248970U)
					{
						if (num == 1111292255U)
						{
							if (currentLanguageCode == "ko")
							{
								return 65;
							}
						}
					}
					else if (currentLanguageCode == "en")
					{
						return 118;
					}
				}
				else if (currentLanguageCode == "zh-CN")
				{
					return 65;
				}
			}
			else if (num <= 1176137065U)
			{
				if (num != 1162757945U)
				{
					if (num == 1176137065U)
					{
						if (currentLanguageCode == "es")
						{
							return 118;
						}
					}
				}
				else if (currentLanguageCode == "pl")
				{
					return 118;
				}
			}
			else if (num != 1194886160U)
			{
				if (num == 1195724803U)
				{
					if (currentLanguageCode == "tr")
					{
						return 118;
					}
				}
			}
			else if (currentLanguageCode == "it")
			{
				return 118;
			}
		}
		else if (num <= 1545391778U)
		{
			if (num != 1213488160U)
			{
				if (num != 1461901041U)
				{
					if (num == 1545391778U)
					{
						if (currentLanguageCode == "de")
						{
							return 118;
						}
					}
				}
				else if (currentLanguageCode == "fr")
				{
					return 118;
				}
			}
			else if (currentLanguageCode == "ru")
			{
				return 118;
			}
		}
		else if (num <= 1630957159U)
		{
			if (num != 1562713850U)
			{
				if (num == 1630957159U)
				{
					if (currentLanguageCode == "nl")
					{
						return 118;
					}
				}
			}
			else if (currentLanguageCode == "ar")
			{
				return 65;
			}
		}
		else if (num != 1816099348U)
		{
			if (num == 3973517379U)
			{
				if (currentLanguageCode == "zh-TW")
				{
					return 65;
				}
			}
		}
		else if (currentLanguageCode == "ja")
		{
			return 65;
		}
		return 118;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0002C80F File Offset: 0x0002AA0F
	public void DescriptionShrinkExpand()
	{
		if (this.scrolledDescription.vertical)
		{
			this.DescriptionShrink();
			return;
		}
		this.DescriptionExpand();
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0002C82B File Offset: 0x0002AA2B
	public void ResetFactions()
	{
		this.selectedFactions = 0;
		this.previouslySelectedFaction = 0;
		this.currentFaction = 7;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0002C842 File Offset: 0x0002AA42
	public void PreviousFactionButton_OnClick()
	{
		this.ChangeFaction(false);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002C84B File Offset: 0x0002AA4B
	public void NextFactionButton_OnClick()
	{
		this.ChangeFaction(true);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0002C854 File Offset: 0x0002AA54
	public void AcceptFactionButton_OnClick()
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
		AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_accept_button);
		Action<int, int> onPlayerFactionSelection = this.OnPlayerFactionSelection;
		if (onPlayerFactionSelection != null)
		{
			onPlayerFactionSelection(this.playerId, this.currentFaction);
		}
		this.CloseWindow(false);
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0002C88C File Offset: 0x0002AA8C
	private void ChangeFaction(bool isNextFaction)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		this.UpdateFaction(isNextFaction);
		this.UpdateFactionVisualRepresentation();
		this.DescriptionShrink();
	}

	// Token: 0x04000629 RID: 1577
	public Image background;

	// Token: 0x0400062A RID: 1578
	public Image factionLogo;

	// Token: 0x0400062B RID: 1579
	public Sprite[] factionLogos;

	// Token: 0x0400062C RID: 1580
	public Color[] factionColors;

	// Token: 0x0400062D RID: 1581
	public string[] factionBackgroundNames;

	// Token: 0x0400062E RID: 1582
	public List<Sprite> factionMechAbilities;

	// Token: 0x0400062F RID: 1583
	public GameObject randomDetails;

	// Token: 0x04000630 RID: 1584
	public GameObject factionDetails;

	// Token: 0x04000631 RID: 1585
	public TextMeshProUGUI factionName;

	// Token: 0x04000632 RID: 1586
	public TextMeshProUGUI factionCharacterName;

	// Token: 0x04000633 RID: 1587
	public TextMeshProUGUI factionDescription;

	// Token: 0x04000634 RID: 1588
	public ScrollRect scrolledDescription;

	// Token: 0x04000635 RID: 1589
	public Scrollbar descriptionScrollbar;

	// Token: 0x04000636 RID: 1590
	public TextMeshProUGUI factionAbilityName;

	// Token: 0x04000637 RID: 1591
	public TextMeshProUGUI factionAbilityDescription;

	// Token: 0x04000638 RID: 1592
	public TextMeshProUGUI powerBonus;

	// Token: 0x04000639 RID: 1593
	public TextMeshProUGUI ammoBonus;

	// Token: 0x0400063A RID: 1594
	public TextMeshProUGUI[] mechAbilityNames;

	// Token: 0x0400063B RID: 1595
	public TextMeshProUGUI[] mechAbilityDescriptions;

	// Token: 0x0400063C RID: 1596
	public Image[] mechAbilityIcons;

	// Token: 0x0400063D RID: 1597
	[SerializeField]
	private Button previousFactionButton;

	// Token: 0x0400063E RID: 1598
	[SerializeField]
	private Button nextFactionButton;

	// Token: 0x0400063F RID: 1599
	[SerializeField]
	private Button acceptFactionButton;

	// Token: 0x04000640 RID: 1600
	[Header("Mobile only")]
	[SerializeField]
	private Button readMore;

	// Token: 0x04000641 RID: 1601
	[SerializeField]
	private GameObject randomOptionCover;

	// Token: 0x04000642 RID: 1602
	[SerializeField]
	private GameObject randomOptionCoverIFA;

	// Token: 0x04000643 RID: 1603
	[SerializeField]
	private RectTransform descriptionScrollContent;

	// Token: 0x04000644 RID: 1604
	private int currentFaction = 7;

	// Token: 0x04000645 RID: 1605
	private int selectedFactions;

	// Token: 0x04000646 RID: 1606
	private int previouslySelectedFaction;

	// Token: 0x04000647 RID: 1607
	private int playerId;

	// Token: 0x04000648 RID: 1608
	private bool IFASelectionUnlocked;

	// Token: 0x04000649 RID: 1609
	private bool moreThan5Players;

	// Token: 0x0400064A RID: 1610
	private string shortDescription = "";

	// Token: 0x0400064B RID: 1611
	private const int asianLanguagesLength = 65;

	// Token: 0x0400064C RID: 1612
	private const int otherLanguagesLength = 118;
}
