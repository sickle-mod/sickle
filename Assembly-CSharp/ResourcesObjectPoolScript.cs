using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class ResourcesObjectPoolScript : MonoBehaviour
{
	// Token: 0x060009AD RID: 2477 RVA: 0x0002E902 File Offset: 0x0002CB02
	private void Awake()
	{
		if (ResourcesObjectPoolScript.Instance == null)
		{
			ResourcesObjectPoolScript.Instance = this;
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0007BC3C File Offset: 0x00079E3C
	private void Start()
	{
		this.pooledMetalObjects = new List<GameObject>();
		this.pooledWoodObjects = new List<GameObject>();
		this.pooledOilObjects = new List<GameObject>();
		this.pooledFoodObjects = new List<GameObject>();
		this.coinsMarkers = new List<GameObject>();
		this.popularityMarkers = new List<GameObject>();
		this.powerMarkers = new List<GameObject>();
		this.ammoMarkers = new List<GameObject>();
		this.recruitMarkers = new List<GameObject>();
		this.skillMarkers = new List<GameObject>();
		this.getResourceOnBaseParticles = new List<GameObject>();
		this.spendResourceOnBaseParticles = new List<GameObject>();
		this.starMarkers = new List<GameObject>();
		this.allModels = new List<GameObject>();
		for (int i = 0; i < this.resourcesPooledAmount; i++)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.pooledMetalObject);
			gameObject.SetActive(false);
			this.pooledMetalObjects.Add(gameObject);
			gameObject.transform.SetParent(base.transform);
		}
		for (int j = 0; j < this.resourcesPooledAmount; j++)
		{
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.pooledWoodObject);
			gameObject2.SetActive(false);
			this.pooledWoodObjects.Add(gameObject2);
			gameObject2.transform.SetParent(base.transform);
		}
		for (int k = 0; k < this.resourcesPooledAmount; k++)
		{
			GameObject gameObject3 = global::UnityEngine.Object.Instantiate<GameObject>(this.pooledOilObject);
			gameObject3.SetActive(false);
			this.pooledOilObjects.Add(gameObject3);
			gameObject3.transform.SetParent(base.transform);
		}
		for (int l = 0; l < this.resourcesPooledAmount; l++)
		{
			GameObject gameObject4 = global::UnityEngine.Object.Instantiate<GameObject>(this.pooledFoodObject);
			gameObject4.SetActive(false);
			this.pooledFoodObjects.Add(gameObject4);
			gameObject4.transform.SetParent(base.transform);
		}
		for (int m = 0; m < this.markersPooledAmount; m++)
		{
			GameObject gameObject5 = global::UnityEngine.Object.Instantiate<GameObject>(this.coinMarker);
			gameObject5.SetActive(false);
			this.coinsMarkers.Add(gameObject5);
			gameObject5.transform.SetParent(base.transform);
		}
		for (int n = 0; n < this.markersPooledAmount; n++)
		{
			GameObject gameObject6 = global::UnityEngine.Object.Instantiate<GameObject>(this.ammoMarker);
			gameObject6.SetActive(false);
			this.ammoMarkers.Add(gameObject6);
			gameObject6.transform.SetParent(base.transform);
		}
		for (int num = 0; num < this.markersPooledAmount; num++)
		{
			GameObject gameObject7 = global::UnityEngine.Object.Instantiate<GameObject>(this.popularityMarker);
			gameObject7.SetActive(false);
			this.popularityMarkers.Add(gameObject7);
			gameObject7.transform.SetParent(base.transform);
		}
		for (int num2 = 0; num2 < this.markersPooledAmount; num2++)
		{
			GameObject gameObject8 = global::UnityEngine.Object.Instantiate<GameObject>(this.powerMarker);
			gameObject8.SetActive(false);
			this.powerMarkers.Add(gameObject8);
			gameObject8.transform.SetParent(base.transform);
		}
		for (int num3 = 0; num3 < this.markersPooledAmount; num3++)
		{
			GameObject gameObject9 = global::UnityEngine.Object.Instantiate<GameObject>(this.recruitMarker);
			gameObject9.SetActive(false);
			this.recruitMarkers.Add(gameObject9);
			gameObject9.transform.SetParent(base.transform);
		}
		for (int num4 = 0; num4 < this.markersPooledAmount; num4++)
		{
			GameObject gameObject10 = global::UnityEngine.Object.Instantiate<GameObject>(this.skillMarker);
			gameObject10.SetActive(false);
			this.skillMarkers.Add(gameObject10);
			gameObject10.transform.SetParent(base.transform);
		}
		for (int num5 = 0; num5 < 2; num5++)
		{
			GameObject gameObject11 = global::UnityEngine.Object.Instantiate<GameObject>(this.getResourceOnBaseParticle);
			gameObject11.SetActive(false);
			this.getResourceOnBaseParticles.Add(gameObject11);
			gameObject11.transform.SetParent(base.transform);
		}
		for (int num6 = 0; num6 < 2; num6++)
		{
			GameObject gameObject12 = global::UnityEngine.Object.Instantiate<GameObject>(this.spendResourceOnBaseParticle);
			gameObject12.SetActive(false);
			this.spendResourceOnBaseParticles.Add(gameObject12);
			gameObject12.transform.SetParent(base.transform);
		}
		for (int num7 = 0; num7 < 2; num7++)
		{
			GameObject gameObject13 = global::UnityEngine.Object.Instantiate<GameObject>(this.starMarker);
			gameObject13.SetActive(false);
			this.starMarkers.Add(gameObject13);
			gameObject13.transform.SetParent(base.transform);
		}
		this.poolsArray = new List<GameObject>[13];
		this.poolsArray[0] = this.pooledMetalObjects;
		this.poolsArray[1] = this.pooledWoodObjects;
		this.poolsArray[2] = this.pooledOilObjects;
		this.poolsArray[3] = this.pooledFoodObjects;
		this.poolsArray[4] = this.coinsMarkers;
		this.poolsArray[5] = this.popularityMarkers;
		this.poolsArray[6] = this.powerMarkers;
		this.poolsArray[7] = this.ammoMarkers;
		this.poolsArray[8] = this.recruitMarkers;
		this.poolsArray[9] = this.skillMarkers;
		this.poolsArray[10] = this.getResourceOnBaseParticles;
		this.poolsArray[11] = this.spendResourceOnBaseParticles;
		this.poolsArray[12] = this.starMarkers;
		this.allModels.Add(this.pooledMetalObject);
		this.allModels.Add(this.pooledWoodObject);
		this.allModels.Add(this.pooledOilObject);
		this.allModels.Add(this.pooledFoodObject);
		this.allModels.Add(this.coinMarker);
		this.allModels.Add(this.ammoMarker);
		this.allModels.Add(this.popularityMarker);
		this.allModels.Add(this.powerMarker);
		this.allModels.Add(this.recruitMarker);
		this.allModels.Add(this.skillMarker);
		this.allModels.Add(this.getResourceOnBaseParticle);
		this.allModels.Add(this.spendResourceOnBaseParticle);
		this.allModels.Add(this.starMarker);
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0007C220 File Offset: 0x0007A420
	public void HideAllObjects()
	{
		for (int i = 0; i < this.poolsArray.Length; i++)
		{
			foreach (GameObject gameObject in this.poolsArray[i])
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0007C288 File Offset: 0x0007A488
	public GameObject GetPooledObject(int poolID)
	{
		for (int i = 0; i < this.poolsArray[poolID].Count; i++)
		{
			if (!this.poolsArray[poolID][i].activeInHierarchy)
			{
				this.poolsArray[poolID][i].transform.localScale = Vector3.one;
				return this.poolsArray[poolID][i];
			}
		}
		if (this.shouldExpand)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.allModels[poolID]);
			gameObject.SetActive(false);
			this.poolsArray[poolID].Add(gameObject);
			return gameObject;
		}
		return null;
	}

	// Token: 0x040008A2 RID: 2210
	[SerializeField]
	private int resourcesPooledAmount = 9;

	// Token: 0x040008A3 RID: 2211
	[SerializeField]
	private int markersPooledAmount = 5;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private GameObject pooledMetalObject;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	private GameObject pooledWoodObject;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private GameObject pooledOilObject;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	private GameObject pooledFoodObject;

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private GameObject coinMarker;

	// Token: 0x040008A9 RID: 2217
	[SerializeField]
	private GameObject ammoMarker;

	// Token: 0x040008AA RID: 2218
	[SerializeField]
	private GameObject popularityMarker;

	// Token: 0x040008AB RID: 2219
	[SerializeField]
	private GameObject powerMarker;

	// Token: 0x040008AC RID: 2220
	[SerializeField]
	private GameObject recruitMarker;

	// Token: 0x040008AD RID: 2221
	[SerializeField]
	private GameObject skillMarker;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private GameObject getResourceOnBaseParticle;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private GameObject spendResourceOnBaseParticle;

	// Token: 0x040008B0 RID: 2224
	[SerializeField]
	private GameObject starMarker;

	// Token: 0x040008B1 RID: 2225
	public List<GameObject> pooledMetalObjects;

	// Token: 0x040008B2 RID: 2226
	public List<GameObject> pooledWoodObjects;

	// Token: 0x040008B3 RID: 2227
	public List<GameObject> pooledOilObjects;

	// Token: 0x040008B4 RID: 2228
	public List<GameObject> pooledFoodObjects;

	// Token: 0x040008B5 RID: 2229
	public List<GameObject> coinsMarkers;

	// Token: 0x040008B6 RID: 2230
	public List<GameObject> popularityMarkers;

	// Token: 0x040008B7 RID: 2231
	public List<GameObject> powerMarkers;

	// Token: 0x040008B8 RID: 2232
	public List<GameObject> ammoMarkers;

	// Token: 0x040008B9 RID: 2233
	public List<GameObject> recruitMarkers;

	// Token: 0x040008BA RID: 2234
	public List<GameObject> skillMarkers;

	// Token: 0x040008BB RID: 2235
	public List<GameObject> getResourceOnBaseParticles;

	// Token: 0x040008BC RID: 2236
	public List<GameObject> spendResourceOnBaseParticles;

	// Token: 0x040008BD RID: 2237
	public List<GameObject> starMarkers;

	// Token: 0x040008BE RID: 2238
	private List<GameObject>[] poolsArray;

	// Token: 0x040008BF RID: 2239
	private List<GameObject> allModels;

	// Token: 0x040008C0 RID: 2240
	public static ResourcesObjectPoolScript Instance;

	// Token: 0x040008C1 RID: 2241
	public bool shouldExpand = true;
}
