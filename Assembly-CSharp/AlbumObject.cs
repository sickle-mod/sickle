using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E5 RID: 229
[RequireComponent(typeof(Image))]
public class AlbumObject : MonoBehaviour
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x0002C4E9 File Offset: 0x0002A6E9
	public Button.ButtonClickedEvent GetButtonClickedEvent()
	{
		return this.button.onClick;
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0002C4F6 File Offset: 0x0002A6F6
	public void SetFactoryCardBuilder(FactoryCardBuilder builder)
	{
		this.builder = builder;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x0007163C File Offset: 0x0006F83C
	public void UpdateObject(TabType cardType, Sprite artSprite, Sprite maskSprite, Vector2 size, int id, bool unlocked)
	{
		this.artImage.sprite = artSprite;
		this.artImage.preserveAspect = true;
		this.transparencyMaskImage.sprite = maskSprite;
		this.unlocked = unlocked;
		this.artImage.transform.GetComponent<RectTransform>().sizeDelta = size;
		this.artImage.transform.GetComponent<RectTransform>().localScale = Vector2.one * ((cardType == TabType.Mats) ? 0.994f : 1f);
		this.transparencyMaskImage.transform.GetComponent<RectTransform>().sizeDelta = size;
		this.UpdateDescription(id, cardType);
		if (unlocked)
		{
			this.lockedObject.SetActive(false);
			this.artImage.material = null;
			this.artImage.color = Color.white;
			return;
		}
		this.lockedObject.SetActive(true);
		this.artImage.material = this.sepia;
		this.artImage.color = this.additionalLockColor;
		this.factoryCardDescription.SetActive(false);
		this.objectiveDescription.SetActive(false);
		this.encounterDescription.SetActive(false);
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00071760 File Offset: 0x0006F960
	private void UpdateDescription(int id, TabType cardType)
	{
		switch (cardType)
		{
		case TabType.EncounterCards:
			this.UpdateEncounterDescription(id);
			return;
		case TabType.FactoryCards:
			this.UpdateFactoryCardDecription(id);
			return;
		case TabType.ObjectiveCards:
			this.UpdateObjectiveDescription(id);
			return;
		case TabType.Mats:
			this.UpdateMatDescription(id);
			return;
		default:
			this.factoryCardDescription.SetActive(false);
			this.objectiveDescription.SetActive(false);
			this.encounterDescription.SetActive(false);
			return;
		}
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x000717CC File Offset: 0x0006F9CC
	private void UpdateEncounterDescription(int id)
	{
		if (!this.unlocked)
		{
			this.GetUnlockCaseTextMeshPro(TabType.EncounterCards).text = AchievementManager.GetAchievementDescriptionForEncounterCard(id + 1);
		}
		this.factoryCardDescription.SetActive(false);
		this.objectiveDescription.SetActive(false);
		this.encounterDescription.SetActive(true);
		int num = id + 1;
		string text = string.Format("<sprite name=EncounterBlackIcon> {0} <font=\"MinionPro-ext\"><size=3>{1}</size></font>\n", GameController.GetEncounterDescription(num, 1), GameController.GetEncounterActionDescription(num, 1));
		text += string.Format("<sprite name=EncounterBlackIcon> {0} <font=\"MinionPro-ext\"><size=3>{1}</size></font>\n", GameController.GetEncounterDescription(num, 2), GameController.GetEncounterActionDescription(num, 2));
		text += string.Format("<sprite name=EncounterBlackIcon> {0} <font=\"MinionPro-ext\"><size=3>{1}</size></font>", GameController.GetEncounterDescription(num, 3), GameController.GetEncounterActionDescription(num, 3));
		this.encounterDescription.GetComponent<TextMeshProUGUI>().text = text;
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00071888 File Offset: 0x0006FA88
	private void UpdateFactoryCardDecription(int id)
	{
		if (!this.unlocked)
		{
			this.GetUnlockCaseTextMeshPro(TabType.FactoryCards).text = AchievementManager.GetAchievementDescriptionForFactoryCard(id + 1);
		}
		this.objectiveDescription.SetActive(false);
		this.encounterDescription.SetActive(false);
		this.factoryCardDescription.SetActive(true);
		if (this.card14Info != null)
		{
			if (id == 13)
			{
				this.card14Info.gameObject.SetActive(true);
			}
			else
			{
				this.card14Info.gameObject.SetActive(false);
			}
		}
		int num = id + 1;
		this.factoryCardNumber.text = num.ToString();
		MatPlayerSectionPresenter component = this.factoryCardDescription.transform.GetChild(0).GetComponent<MatPlayerSectionPresenter>();
		FactoryCard factoryCard = new FactoryCard(num, GameController.GameManager);
		if (this.builder != null)
		{
			this.builder.BuildFactoryCard(component, factoryCard);
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00071960 File Offset: 0x0006FB60
	private void UpdateObjectiveDescription(int id)
	{
		if (!this.unlocked)
		{
			this.GetUnlockCaseTextMeshPro(TabType.ObjectiveCards).text = AchievementManager.GetAchievementDescriptionForObjectiveCard(id + 1);
		}
		this.factoryCardDescription.SetActive(false);
		this.encounterDescription.SetActive(false);
		this.objectiveDescription.SetActive(true);
		int num = id + 1;
		this.objectiveDescription.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameController.GetObjectiveTitle(num);
		this.objectiveDescription.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GameController.GetObjectiveDescription(num);
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x000719F4 File Offset: 0x0006FBF4
	private void UpdateMatDescription(int id)
	{
		if (!this.unlocked)
		{
			this.GetUnlockCaseTextMeshPro(TabType.Mats).text = ScriptLocalization.Get("MainMenu/AvailableWithExpansion");
		}
		this.factoryCardDescription.SetActive(false);
		this.encounterDescription.SetActive(false);
		this.objectiveDescription.SetActive(false);
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00071A44 File Offset: 0x0006FC44
	private TextMeshProUGUI GetUnlockCaseTextMeshPro(TabType type)
	{
		this.lockedObject.transform.GetChild(0).gameObject.SetActive(false);
		this.lockedObject.transform.GetChild(1).gameObject.SetActive(false);
		this.lockedObject.transform.GetChild(2).gameObject.SetActive(false);
		this.lockedObject.transform.GetChild(3).gameObject.SetActive(false);
		switch (type)
		{
		case TabType.EncounterCards:
			this.lockedObject.transform.GetChild(0).gameObject.SetActive(true);
			return this.lockedObject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
		case TabType.FactoryCards:
			this.lockedObject.transform.GetChild(1).gameObject.SetActive(true);
			return this.lockedObject.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
		case TabType.ObjectiveCards:
			this.lockedObject.transform.GetChild(2).gameObject.SetActive(true);
			return this.lockedObject.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
		case TabType.Mats:
			this.lockedObject.transform.GetChild(3).gameObject.SetActive(true);
			return this.lockedObject.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
		default:
			this.lockedObject.transform.GetChild(0).gameObject.SetActive(false);
			return this.lockedObject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
		}
	}

	// Token: 0x040005FA RID: 1530
	[SerializeField]
	private GameObject lockedObject;

	// Token: 0x040005FB RID: 1531
	[SerializeField]
	private GameObject encounterDescription;

	// Token: 0x040005FC RID: 1532
	[SerializeField]
	private GameObject objectiveDescription;

	// Token: 0x040005FD RID: 1533
	[SerializeField]
	private GameObject factoryCardDescription;

	// Token: 0x040005FE RID: 1534
	[SerializeField]
	private Transform card14Info;

	// Token: 0x040005FF RID: 1535
	[SerializeField]
	private TextMeshProUGUI factoryCardNumber;

	// Token: 0x04000600 RID: 1536
	[SerializeField]
	private Image artImage;

	// Token: 0x04000601 RID: 1537
	[SerializeField]
	private Image transparencyMaskImage;

	// Token: 0x04000602 RID: 1538
	[SerializeField]
	private Material sepia;

	// Token: 0x04000603 RID: 1539
	[SerializeField]
	private Material blur;

	// Token: 0x04000604 RID: 1540
	[SerializeField]
	private Button button;

	// Token: 0x04000605 RID: 1541
	private Color additionalLockColor = new Color32(82, 82, 82, byte.MaxValue);

	// Token: 0x04000606 RID: 1542
	[SerializeField]
	private FactoryCardBuilder builder;

	// Token: 0x04000607 RID: 1543
	private bool unlocked;
}
