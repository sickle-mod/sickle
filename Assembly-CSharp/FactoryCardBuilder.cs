using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000ED RID: 237
public class FactoryCardBuilder : MonoBehaviour
{
	// Token: 0x06000711 RID: 1809 RVA: 0x0002C8DE File Offset: 0x0002AADE
	public void BuildFactoryCard(MatPlayerSectionPresenter card, FactoryCard factoryCard)
	{
		this.SetTopActionPaySprites(card.topActionPresenter, factoryCard.ActionTop);
		this.SetTopActionGainSprites(card.topActionPresenter, factoryCard.ActionTop);
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00072D24 File Offset: 0x00070F24
	private void SetTopActionPaySprites(PlayerTopActionPresenter presenter, TopAction action)
	{
		for (int i = 0; i < presenter.actionCost.Length; i++)
		{
			presenter.actionCost[i].gameObject.SetActive(false);
		}
		if (action != null && action.GetPayAction(0).GetPayType() == PayType.Resource && (action.GetPayAction(0) as PayResource).DifferentResources)
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
		if (action != null)
		{
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
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00072FE4 File Offset: 0x000711E4
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
		if (action != null)
		{
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
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x000731C8 File Offset: 0x000713C8
	private Sprite GetPaySprite(PayType payType)
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

	// Token: 0x06000715 RID: 1813 RVA: 0x00073218 File Offset: 0x00071418
	private Sprite GetGainSprite(GainType gainType)
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

	// Token: 0x06000716 RID: 1814 RVA: 0x0002C904 File Offset: 0x0002AB04
	private void TransformImage(Sprite source, Image target, int childIndex)
	{
		target.color = Color.white;
		target.transform.GetChild(childIndex).GetComponent<Image>().sprite = source;
		target.transform.GetChild(childIndex).GetComponent<Image>().preserveAspect = true;
	}

	// Token: 0x04000652 RID: 1618
	public Sprite spriteCoin;

	// Token: 0x04000653 RID: 1619
	public Sprite spriteCombatCard;

	// Token: 0x04000654 RID: 1620
	public Sprite spritePopularity;

	// Token: 0x04000655 RID: 1621
	public Sprite spritePower;

	// Token: 0x04000656 RID: 1622
	public Sprite spriteResource;

	// Token: 0x04000657 RID: 1623
	public Sprite differentSprite;

	// Token: 0x04000658 RID: 1624
	public Sprite gainProduceSprite;

	// Token: 0x04000659 RID: 1625
	public Sprite gainRecruitSprite;

	// Token: 0x0400065A RID: 1626
	public Sprite gainMechSprite;

	// Token: 0x0400065B RID: 1627
	public Sprite gainBuildingSprite;

	// Token: 0x0400065C RID: 1628
	public Sprite gainUpgradeSprite;

	// Token: 0x0400065D RID: 1629
	public Sprite gainWorkerSprite;
}
