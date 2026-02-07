using System;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D4 RID: 212
public class ContextAreaGainNotification : MonoBehaviour
{
	// Token: 0x06000640 RID: 1600 RVA: 0x0002BE9F File Offset: 0x0002A09F
	private void Update()
	{
		this.animationKickOff = false;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0006F570 File Offset: 0x0006D770
	public void ShowAnimation(int gainType, int gainAmount)
	{
		base.gameObject.SetActive(true);
		Animator component = base.GetComponent<Animator>();
		if (!this.animationKickOff && (component.GetCurrentAnimatorClipInfoCount(0) == 0 || component.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Gain"))
		{
			for (int i = 0; i < this.gainIcons.Length; i++)
			{
				if (i < gainAmount)
				{
					this.gainIcons[i].gameObject.SetActive(true);
					this.gainIcons[i].sprite = this.gainSprites[gainType];
				}
				else
				{
					this.gainIcons[i].gameObject.SetActive(false);
				}
			}
			component.Play("Gain");
			this.animationKickOff = true;
			return;
		}
		int num = 0;
		while (num < this.gainIcons.Length && this.gainIcons[num].IsActive())
		{
			num++;
		}
		for (int j = 0; j < gainAmount; j++)
		{
			if (j + num < this.gainIcons.Length)
			{
				this.gainIcons[j + num].gameObject.SetActive(true);
				this.gainIcons[j + num].sprite = this.gainSprites[gainType];
			}
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0006F698 File Offset: 0x0006D898
	public void ShowAnimation(GainType gainType, int gainAmount)
	{
		int num = 0;
		switch (gainType)
		{
		case GainType.Coin:
			num = 0;
			break;
		case GainType.Popularity:
			num = 1;
			break;
		case GainType.Power:
			num = 2;
			break;
		case GainType.CombatCard:
			num = 3;
			break;
		}
		this.ShowAnimation(num, gainAmount);
	}

	// Token: 0x0400055E RID: 1374
	public Sprite[] gainSprites;

	// Token: 0x0400055F RID: 1375
	public Image[] gainIcons;

	// Token: 0x04000560 RID: 1376
	private bool animationKickOff;
}
