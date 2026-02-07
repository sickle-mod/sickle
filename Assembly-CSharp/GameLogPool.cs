using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class GameLogPool : MonoBehaviour
{
	// Token: 0x060002A9 RID: 681 RVA: 0x00029A26 File Offset: 0x00027C26
	private void Awake()
	{
		if (GameLogPool.Instance == null)
		{
			GameLogPool.Instance = this;
		}
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0005D5B8 File Offset: 0x0005B7B8
	private void Start()
	{
		this.logIconsObjects = new List<GameObject>();
		for (int i = 0; i < this.pooledAmount; i++)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.pooledMetalObject);
			gameObject.SetActive(false);
			this.logIconsObjects.Add(gameObject);
			gameObject.transform.SetParent(base.transform);
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0005D614 File Offset: 0x0005B814
	public GameObject GetPooledObject(int poolID)
	{
		for (int i = 0; i < this.logIconsObjects.Count; i++)
		{
			if (!this.logIconsObjects[i].activeInHierarchy)
			{
				return this.logIconsObjects[i];
			}
		}
		return null;
	}

	// Token: 0x04000201 RID: 513
	[SerializeField]
	private GameObject pooledMetalObject;

	// Token: 0x04000202 RID: 514
	[SerializeField]
	private int pooledAmount = 10;

	// Token: 0x04000203 RID: 515
	public List<GameObject> logIconsObjects;

	// Token: 0x04000204 RID: 516
	public static GameLogPool Instance;
}
