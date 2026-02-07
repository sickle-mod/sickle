using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class ShowroomUnitPresenter : MonoBehaviour
{
	// Token: 0x0600089F RID: 2207 RVA: 0x00079914 File Offset: 0x00077B14
	private void Start()
	{
		foreach (GameObject gameObject in this.units)
		{
			this.defaultUnitRotation = base.transform.localRotation;
			gameObject.SetActive(false);
		}
		this.UpdateSkinSetting();
		this.SetSkin(this.characterVariants, this.characterSkinMode);
		this.SetSkin(this.mechVariants, this.mechSkinMode);
		this.SetSkin(this.workerVariants, this.workerSkinMode);
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x000799B4 File Offset: 0x00077BB4
	public void UpdateSkinSetting()
	{
		string name = base.gameObject.name;
		uint num = global::<PrivateImplementationDetails>.ComputeStringHash(name);
		if (num <= 2706759793U)
		{
			if (num != 186139318U)
			{
				if (num != 357991065U)
				{
					if (num == 2706759793U)
					{
						if (name == "PolaniaUnitPresenter")
						{
							this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_CHARACTER);
							this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_MECH);
							this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_WORKER);
							return;
						}
					}
				}
				else if (name == "SaxonyUnitPresenter")
				{
					this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_CHARACTER);
					this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_MECH);
					this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_WORKER);
					return;
				}
			}
			else if (name == "TogawaUnitPresenter")
			{
				this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_CHARACTER);
				this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_MECH);
				this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_WORKER);
				return;
			}
		}
		else if (num <= 2893145233U)
		{
			if (num != 2793991166U)
			{
				if (num == 2893145233U)
				{
					if (name == "RusvietUnitPresenter")
					{
						this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_CHARACTER);
						this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_MECH);
						this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_WORKER);
						return;
					}
				}
			}
			else if (name == "NordicUnitPresenter")
			{
				this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_CHARACTER);
				this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_MECH);
				this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_WORKER);
				return;
			}
		}
		else if (num != 3630689966U)
		{
			if (num == 3701669634U)
			{
				if (name == "AlbionUnitPresenter")
				{
					this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_CHARACTER);
					this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_MECH);
					this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_WORKER);
					return;
				}
			}
		}
		else if (name == "CrimeaUnitPresenter")
		{
			this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_CHARACTER);
			this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_MECH);
			this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_WORKER);
			return;
		}
		Debug.Log("Could not find a proper unit presenter name. Check your game objects' names");
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00079C28 File Offset: 0x00077E28
	public void SetSkin(List<GameObject> variants, int value)
	{
		if (variants.Count > 1)
		{
			for (int i = 0; i < variants.Count; i++)
			{
				if (i == value)
				{
					variants[i].SetActive(true);
				}
				else
				{
					variants[i].SetActive(false);
				}
			}
		}
	}

	// Token: 0x0400075E RID: 1886
	[SerializeField]
	public List<GameObject> units;

	// Token: 0x0400075F RID: 1887
	public Quaternion defaultUnitRotation;

	// Token: 0x04000760 RID: 1888
	public int characterSkinMode;

	// Token: 0x04000761 RID: 1889
	public int mechSkinMode;

	// Token: 0x04000762 RID: 1890
	public int workerSkinMode;

	// Token: 0x04000763 RID: 1891
	public List<GameObject> characterVariants;

	// Token: 0x04000764 RID: 1892
	public List<GameObject> mechVariants;

	// Token: 0x04000765 RID: 1893
	public List<GameObject> workerVariants;
}
