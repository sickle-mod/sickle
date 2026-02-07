using System;
using System.Collections.Generic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000087 RID: 135
public class ResourcesTween : MonoBehaviour
{
	// Token: 0x06000499 RID: 1177 RVA: 0x0002AE5A File Offset: 0x0002905A
	private void Awake()
	{
		ResourcesTween.Instance = this;
		this.objectsToMove = new List<GameObject>();
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000648A8 File Offset: 0x00062AA8
	private void Update()
	{
		if (this.gainResources)
		{
			for (int i = 0; i < this.objectsToMove.Count; i++)
			{
				this.objectsToMove[i].transform.position = this.matPlayerPresenter.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].gameObject.transform.position;
				this.objectsToMove[i].SetActive(true);
			}
			this.MoveOnXAxis();
			this.MoveOnYAxis();
			this.objectsToMove = new List<GameObject>();
			this.gainResources = false;
		}
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0006494C File Offset: 0x00062B4C
	public void SetupResourcesToMove(int amount, Sprite statSprit, Vector3 position)
	{
		this.actualDestinationPosition = position;
		for (int i = 0; i < amount; i++)
		{
			this.gainIcons[i].GetComponent<Image>().sprite = statSprit;
			this.objectsToMove.Add(this.gainIcons[i]);
		}
		this.gainResources = true;
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0006499C File Offset: 0x00062B9C
	private void MoveOnXAxis()
	{
		for (int i = 0; i < this.objectsToMove.Count; i++)
		{
			if (i == this.objectsToMove.Count - 1)
			{
				iTween.MoveTo(this.objectsToMove[i], iTween.Hash(new object[]
				{
					"position",
					this.actualDestinationPosition,
					"x",
					this.objectsToMove[i].transform.position.x + (this.actualDestinationPosition.x - this.objectsToMove[i].transform.position.x),
					"delay",
					0,
					"time",
					global::UnityEngine.Random.Range(this.minAnimationTime, this.maxAnimationTime),
					"oncomplete",
					"HideGainIcon",
					"oncompletetarget",
					base.gameObject,
					"easetype",
					"easeOutBack"
				}));
			}
			else
			{
				iTween.MoveTo(this.objectsToMove[i], iTween.Hash(new object[]
				{
					"position",
					this.actualDestinationPosition,
					"x",
					this.objectsToMove[i].transform.position.x + (this.actualDestinationPosition.x - this.objectsToMove[i].transform.position.x),
					"delay",
					0,
					"time",
					global::UnityEngine.Random.Range(this.minAnimationTime, this.maxAnimationTime),
					"easetype",
					"easeOutBack"
				}));
			}
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00064B94 File Offset: 0x00062D94
	private void MoveOnYAxis()
	{
		for (int i = 0; i < this.objectsToMove.Count; i++)
		{
			if (i == this.objectsToMove.Count - 1)
			{
				iTween.MoveTo(this.objectsToMove[i], iTween.Hash(new object[]
				{
					"position",
					this.actualDestinationPosition,
					"y",
					this.objectsToMove[i].transform.position.y + (this.actualDestinationPosition.y - this.objectsToMove[i].transform.position.y),
					"delay",
					0,
					"time",
					global::UnityEngine.Random.Range(this.minAnimationTime, this.maxAnimationTime),
					"oncomplete",
					"HideGainIcon",
					"oncompletetarget",
					base.gameObject,
					"easetype",
					"easeInCubic"
				}));
			}
			else
			{
				iTween.MoveTo(this.objectsToMove[i], iTween.Hash(new object[]
				{
					"position",
					this.actualDestinationPosition,
					"y",
					this.objectsToMove[i].transform.position.y + (this.actualDestinationPosition.y - this.objectsToMove[i].transform.position.y),
					"delay",
					0,
					"time",
					global::UnityEngine.Random.Range(this.minAnimationTime, this.maxAnimationTime),
					"easetype",
					"easeInCubic"
				}));
			}
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00064D8C File Offset: 0x00062F8C
	private void HideGainIcon()
	{
		GameObject[] array = this.gainIcons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x040003C5 RID: 965
	public static ResourcesTween Instance;

	// Token: 0x040003C6 RID: 966
	[SerializeField]
	private float maxAnimationTime = 0.8f;

	// Token: 0x040003C7 RID: 967
	[SerializeField]
	private float minAnimationTime = 0.6f;

	// Token: 0x040003C8 RID: 968
	[SerializeField]
	private MatPlayerPresenter matPlayerPresenter;

	// Token: 0x040003C9 RID: 969
	[SerializeField]
	private GameObject[] gainIcons;

	// Token: 0x040003CA RID: 970
	private bool gainResources;

	// Token: 0x040003CB RID: 971
	private Vector3 actualDestinationPosition;

	// Token: 0x040003CC RID: 972
	private List<GameObject> objectsToMove;
}
