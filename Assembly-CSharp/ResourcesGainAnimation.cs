using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000081 RID: 129
public class ResourcesGainAnimation : MonoBehaviour
{
	// Token: 0x06000479 RID: 1145 RVA: 0x000644BC File Offset: 0x000626BC
	public void SetupResourcesToAnimation(int amount, Sprite statSprit, Vector3 position)
	{
		if (amount > 0)
		{
			this.backgroundImage[0].transform.position = position;
			foreach (GameObject gameObject in this.gainIcons)
			{
				gameObject.transform.position = position;
				gameObject.GetComponent<Image>().sprite = statSprit;
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.IconGainAnimation(amount));
			}
		}
		if (amount < 0)
		{
			this.backgroundImage[1].transform.position = position;
			foreach (GameObject gameObject2 in this.loseIcons)
			{
				gameObject2.transform.position = position;
				gameObject2.GetComponent<Image>().sprite = statSprit;
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.IconLoseAnimation(Mathf.Abs(amount)));
			}
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00064590 File Offset: 0x00062790
	public void StopAnimation()
	{
		GameObject[] array = this.gainIcons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = this.loseIcons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = this.backgroundImage;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0002ADB4 File Offset: 0x00028FB4
	private IEnumerator IconGainAnimation(int amount)
	{
		int num;
		for (int i = 0; i < amount; i = num + 1)
		{
			if (i < this.gainIcons.Length)
			{
				this.gainIcons[i].SetActive(true);
				this.gainIcons[i].GetComponent<Animator>().Play("GainIconScalling");
				yield return new WaitForSeconds(0.5f);
				this.gainIcons[i].SetActive(false);
				base.StartCoroutine(this.BackgroundGainAnimation());
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0002ADCA File Offset: 0x00028FCA
	private IEnumerator IconLoseAnimation(int amount)
	{
		int num;
		for (int i = 0; i < amount; i = num + 1)
		{
			if (i < this.loseIcons.Length)
			{
				this.loseIcons[i].SetActive(true);
				this.loseIcons[i].GetComponent<Animator>().Play("LoseIconScalling");
				yield return new WaitForSeconds(0.5f);
				this.loseIcons[i].SetActive(false);
				base.StartCoroutine(this.BackgroundLoseAnimation());
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0002ADE0 File Offset: 0x00028FE0
	private IEnumerator BackgroundGainAnimation()
	{
		this.backgroundImage[0].SetActive(true);
		this.backgroundImage[0].GetComponent<Animator>().Play("BackgroundGain");
		yield return new WaitForSeconds(0.8f);
		this.backgroundImage[0].SetActive(false);
		yield break;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0002ADEF File Offset: 0x00028FEF
	private IEnumerator BackgroundLoseAnimation()
	{
		this.backgroundImage[1].SetActive(true);
		this.backgroundImage[1].GetComponent<Animator>().Play("BackgroundLose");
		yield return new WaitForSeconds(0.8f);
		this.backgroundImage[1].SetActive(false);
		yield break;
	}

	// Token: 0x040003A2 RID: 930
	[SerializeField]
	private GameObject[] gainIcons;

	// Token: 0x040003A3 RID: 931
	[SerializeField]
	private GameObject[] loseIcons;

	// Token: 0x040003A4 RID: 932
	[SerializeField]
	private GameObject[] backgroundImage;

	// Token: 0x040003A5 RID: 933
	[SerializeField]
	private Animator gainAnimator;

	// Token: 0x040003A6 RID: 934
	private int tempAmount;
}
