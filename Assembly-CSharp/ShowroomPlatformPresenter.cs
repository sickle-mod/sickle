using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000108 RID: 264
public class ShowroomPlatformPresenter : MonoBehaviour
{
	// Token: 0x0600088F RID: 2191 RVA: 0x00078C7C File Offset: 0x00076E7C
	public void Awake()
	{
		this.crimeaModelTypeCharacter = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_MECH, 0);
		this.saxonyModelTypeCharacter = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_MECH, 0);
		this.polaniaModelTypeCharacter = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_MECH, 0);
		this.albionModelTypeCharacter = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_MECH, 0);
		this.nordicModelTypeCharacter = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_MECH, 0);
		this.rusvietModelTypeCharacter = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_MECH, 0);
		this.togawaModelTypeCharacter = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_MECH, 0);
		this.crimeaModelTypeMech = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_MECH, 0);
		this.saxonyModelTypeMech = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_MECH, 0);
		this.polaniaModelTypeMech = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_MECH, 0);
		this.albionModelTypeMech = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_MECH, 0);
		this.nordicModelTypeMech = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_MECH, 0);
		this.rusvietModelTypeMech = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_MECH, 0);
		this.togawaModelTypeMech = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_MECH, 0);
		this.crimeaModelTypeWorker = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_MECH, 0);
		this.saxonyModelTypeWorker = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_MECH, 0);
		this.polaniaModelTypeWorker = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_MECH, 0);
		this.albionModelTypeWorker = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_MECH, 0);
		this.nordicModelTypeWorker = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_MECH, 0);
		this.rusvietModelTypeWorker = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_MECH, 0);
		this.togawaModelTypeWorker = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_MECH, 0);
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0002DCE1 File Offset: 0x0002BEE1
	private void Start()
	{
		this._sensitivity = 0.4f;
		this._rotation = Vector3.zero;
		this.unitType = 0;
		this.factionButtons[0].GetComponent<Button>().onClick.Invoke();
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00078DF0 File Offset: 0x00076FF0
	private void Update()
	{
		if (PlatformManager.IsStandalone && this.IsMouseAboveUI())
		{
			return;
		}
		if (Input.GetMouseButtonDown(this.GetMouseButtonId()))
		{
			this._mouseReference = Input.mousePosition;
		}
		if (Input.GetMouseButton(this.GetMouseButtonId()))
		{
			this._mouseOffset = -(Input.mousePosition - this._mouseReference);
			this._rotation.y = this._mouseOffset.x * this._sensitivity;
			this.activePresenter.transform.Rotate(this._rotation);
			this._mouseReference = Input.mousePosition;
		}
		if (this.isMechCameraView)
		{
			this.worldCamera.enabled = false;
			this.mechCamera.enabled = true;
			return;
		}
		this.mechCamera.enabled = false;
		this.worldCamera.enabled = true;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0002DD1B File Offset: 0x0002BF1B
	private bool IsMouseAboveUI()
	{
		return EventSystem.current.IsPointerOverGameObject();
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0002DD27 File Offset: 0x0002BF27
	private int GetMouseButtonId()
	{
		if (Input.touchCount <= 0)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00078EC4 File Offset: 0x000770C4
	public void SetActiveUnitPresenter(int index)
	{
		if (this.activePresenter != null)
		{
			foreach (GameObject gameObject in this.activePresenter.GetComponent<ShowroomUnitPresenter>().units)
			{
				gameObject.SetActive(false);
			}
		}
		if (index != this.unitPresenters.IndexOf(this.activePresenter))
		{
			this.activePresenter = this.unitPresenters[index];
			if (this.unitType < this.activePresenter.GetComponent<ShowroomUnitPresenter>().units.Count)
			{
				this.activePresenter.GetComponent<ShowroomUnitPresenter>().units[this.unitType].SetActive(true);
			}
			else if (this.unitType == 1000)
			{
				int num = 1;
				this.activePresenter.GetComponent<ShowroomUnitPresenter>().units[num].SetActive(true);
				this.unitTypeToggles[num].GetComponent<Toggle>().isOn = true;
			}
			else
			{
				this.DisableAllUnits();
			}
			this.activePresenter.GetComponent<ShowroomUnitPresenter>().transform.localRotation = this.activePresenter.GetComponent<ShowroomUnitPresenter>().defaultUnitRotation;
			foreach (GameObject gameObject2 in this.unitPresenters)
			{
				if (gameObject2 != null)
				{
					ShowroomUnitPresenter component = gameObject2.GetComponent<ShowroomUnitPresenter>();
					if (component != null)
					{
						component.UpdateSkinSetting();
					}
				}
			}
			this.CheckSkinSettings();
		}
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00079068 File Offset: 0x00077268
	public void EnableChosenUnit(int index)
	{
		this.unitType = index;
		ShowroomUnitPresenter component = this.activePresenter.GetComponent<ShowroomUnitPresenter>();
		if (this.activePresenter != null)
		{
			for (int i = 0; i < component.units.Count; i++)
			{
				if (i != index)
				{
					component.units[i].SetActive(false);
				}
				else
				{
					component.units[i].SetActive(true);
				}
				component.transform.localRotation = component.defaultUnitRotation;
			}
		}
		foreach (GameObject gameObject in this.unitPresenters)
		{
			if (gameObject != null)
			{
				gameObject.GetComponent<ShowroomUnitPresenter>().UpdateSkinSetting();
			}
		}
		this.CheckSkinSettings();
		WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00079148 File Offset: 0x00077348
	public void DisableAllUnits()
	{
		foreach (GameObject gameObject in this.activePresenter.GetComponent<ShowroomUnitPresenter>().units)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x000791A4 File Offset: 0x000773A4
	public void SetSkinOnUnits(int _modelType)
	{
		if (this.allModelsType != _modelType && this.allModelTypesToggle.GetComponent<Toggle>().isOn)
		{
			this.allModelTypesToggle.GetComponent<Toggle>().isOn = false;
		}
		ShowroomUnitPresenter component = this.activePresenter.GetComponent<ShowroomUnitPresenter>();
		switch (this.unitType)
		{
		case 0:
			component.characterSkinMode = _modelType;
			component.SetSkin(component.characterVariants, _modelType);
			break;
		case 1:
			component.mechSkinMode = _modelType;
			component.SetSkin(component.mechVariants, _modelType);
			break;
		case 2:
			component.workerSkinMode = _modelType;
			component.SetSkin(component.workerVariants, _modelType);
			break;
		default:
			Debug.Log("The chosen unit Type is not recognised");
			break;
		}
		this.SaveSkinSettings(_modelType, true);
		WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00079260 File Offset: 0x00077460
	public void SetSkinOnAllUnits()
	{
		if (this.plainSkinButton.GetComponent<Button>().interactable)
		{
			this.allModelsType = 1;
		}
		else if (this.paintedSkinButton.GetComponent<Button>().interactable)
		{
			this.allModelsType = 0;
		}
		foreach (GameObject gameObject in this.unitPresenters)
		{
			if (gameObject != null)
			{
				ShowroomUnitPresenter component = gameObject.GetComponent<ShowroomUnitPresenter>();
				component.UpdateSkinSetting();
				component.SetSkin(component.characterVariants, this.allModelsType);
				component.SetSkin(component.mechVariants, this.allModelsType);
				component.SetSkin(component.workerVariants, this.allModelsType);
			}
		}
		this.SaveSkinSettings(this.allModelsType, false);
		this.CheckSkinSettings();
		WorldSFXManager.PlaySound(SoundEnum.GuiUpperBeltClick, AudioSourceType.Buttons);
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00079348 File Offset: 0x00077548
	public void CheckSkinSettings()
	{
		ShowroomUnitPresenter component = this.activePresenter.GetComponent<ShowroomUnitPresenter>();
		switch (this.unitType)
		{
		case 0:
		{
			int num = component.characterSkinMode;
			if (num == 0)
			{
				this.plainSkinButton.GetComponent<Button>().interactable = false;
				this.paintedSkinButton.GetComponent<Button>().interactable = true;
				return;
			}
			if (num != 1)
			{
				return;
			}
			this.plainSkinButton.GetComponent<Button>().interactable = true;
			this.paintedSkinButton.GetComponent<Button>().interactable = false;
			return;
		}
		case 1:
		{
			int num = component.mechSkinMode;
			if (num == 0)
			{
				this.plainSkinButton.GetComponent<Button>().interactable = false;
				this.paintedSkinButton.GetComponent<Button>().interactable = true;
				return;
			}
			if (num != 1)
			{
				return;
			}
			this.plainSkinButton.GetComponent<Button>().interactable = true;
			this.paintedSkinButton.GetComponent<Button>().interactable = false;
			return;
		}
		case 2:
		{
			int num = component.workerSkinMode;
			if (num == 0)
			{
				this.plainSkinButton.GetComponent<Button>().interactable = false;
				this.paintedSkinButton.GetComponent<Button>().interactable = true;
				return;
			}
			if (num != 1)
			{
				return;
			}
			this.plainSkinButton.GetComponent<Button>().interactable = true;
			this.paintedSkinButton.GetComponent<Button>().interactable = false;
			return;
		}
		default:
			Debug.Log("The chosen unit Type is not recognised");
			return;
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00079488 File Offset: 0x00077688
	public void UpdateAllFactionButtons(int factionNumber)
	{
		for (int i = 0; i < this.factionButtons.Count; i++)
		{
			if (i != factionNumber)
			{
				this.factionButtons[i].GetComponent<ShowroomFactionButtonController>().TurnOnAndOff(false);
			}
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x000794C8 File Offset: 0x000776C8
	public void SaveSkinSettings(int _skinMode, bool setSingle = true)
	{
		if (setSingle)
		{
			if (this.activePresenter != null)
			{
				this.SetUnitsSkin(this.activePresenter.name, _skinMode, this.unitType);
				return;
			}
		}
		else
		{
			foreach (GameObject gameObject in this.unitPresenters)
			{
				if (gameObject != null)
				{
					for (int i = 0; i < this.unitTypeToggles.Count; i++)
					{
						this.SetUnitsSkin(gameObject.name, _skinMode, i);
					}
				}
			}
		}
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0007956C File Offset: 0x0007776C
	public void SetUnitsSkin(string factionName, int _skinIndex, int _unitType)
	{
		uint num = global::<PrivateImplementationDetails>.ComputeStringHash(factionName);
		if (num <= 2706759793U)
		{
			if (num != 186139318U)
			{
				if (num != 357991065U)
				{
					if (num == 2706759793U)
					{
						if (factionName == "PolaniaUnitPresenter")
						{
							switch (_unitType)
							{
							case 0:
								PlayerPrefs.SetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_CHARACTER, _skinIndex);
								break;
							case 1:
								PlayerPrefs.SetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_MECH, _skinIndex);
								break;
							case 2:
								PlayerPrefs.SetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_WORKER, _skinIndex);
								break;
							}
							PlayerPrefs.Save();
							return;
						}
					}
				}
				else if (factionName == "SaxonyUnitPresenter")
				{
					switch (_unitType)
					{
					case 0:
						PlayerPrefs.SetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_CHARACTER, _skinIndex);
						break;
					case 1:
						PlayerPrefs.SetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_MECH, _skinIndex);
						break;
					case 2:
						PlayerPrefs.SetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_WORKER, _skinIndex);
						break;
					}
					PlayerPrefs.Save();
					return;
				}
			}
			else if (factionName == "TogawaUnitPresenter")
			{
				switch (_unitType)
				{
				case 0:
					PlayerPrefs.SetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_CHARACTER, _skinIndex);
					break;
				case 1:
					PlayerPrefs.SetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_MECH, _skinIndex);
					break;
				case 2:
					PlayerPrefs.SetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_WORKER, _skinIndex);
					break;
				}
				PlayerPrefs.Save();
				return;
			}
		}
		else if (num <= 2893145233U)
		{
			if (num != 2793991166U)
			{
				if (num == 2893145233U)
				{
					if (factionName == "RusvietUnitPresenter")
					{
						switch (_unitType)
						{
						case 0:
							PlayerPrefs.SetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_CHARACTER, _skinIndex);
							break;
						case 1:
							PlayerPrefs.SetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_MECH, _skinIndex);
							break;
						case 2:
							PlayerPrefs.SetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_WORKER, _skinIndex);
							break;
						}
						PlayerPrefs.Save();
						return;
					}
				}
			}
			else if (factionName == "NordicUnitPresenter")
			{
				switch (_unitType)
				{
				case 0:
					PlayerPrefs.SetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_CHARACTER, _skinIndex);
					break;
				case 1:
					PlayerPrefs.SetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_MECH, _skinIndex);
					break;
				case 2:
					PlayerPrefs.SetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_WORKER, _skinIndex);
					break;
				}
				PlayerPrefs.Save();
				return;
			}
		}
		else if (num != 3630689966U)
		{
			if (num == 3701669634U)
			{
				if (factionName == "AlbionUnitPresenter")
				{
					switch (_unitType)
					{
					case 0:
						PlayerPrefs.SetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_CHARACTER, _skinIndex);
						break;
					case 1:
						PlayerPrefs.SetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_MECH, _skinIndex);
						break;
					case 2:
						PlayerPrefs.SetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_WORKER, _skinIndex);
						break;
					}
					PlayerPrefs.Save();
					return;
				}
			}
		}
		else if (factionName == "CrimeaUnitPresenter")
		{
			switch (_unitType)
			{
			case 0:
				PlayerPrefs.SetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_CHARACTER, _skinIndex);
				break;
			case 1:
				PlayerPrefs.SetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_MECH, _skinIndex);
				break;
			case 2:
				PlayerPrefs.SetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_WORKER, _skinIndex);
				break;
			}
			PlayerPrefs.Save();
			return;
		}
		Debug.Log("Could not find a proper unit presenter name. Check your game objects' names");
	}

	// Token: 0x04000721 RID: 1825
	private int crimeaModelTypeCharacter;

	// Token: 0x04000722 RID: 1826
	private int saxonyModelTypeCharacter;

	// Token: 0x04000723 RID: 1827
	private int polaniaModelTypeCharacter;

	// Token: 0x04000724 RID: 1828
	private int albionModelTypeCharacter;

	// Token: 0x04000725 RID: 1829
	private int nordicModelTypeCharacter;

	// Token: 0x04000726 RID: 1830
	private int rusvietModelTypeCharacter;

	// Token: 0x04000727 RID: 1831
	private int togawaModelTypeCharacter;

	// Token: 0x04000728 RID: 1832
	private int crimeaModelTypeMech;

	// Token: 0x04000729 RID: 1833
	private int saxonyModelTypeMech;

	// Token: 0x0400072A RID: 1834
	private int polaniaModelTypeMech;

	// Token: 0x0400072B RID: 1835
	private int albionModelTypeMech;

	// Token: 0x0400072C RID: 1836
	private int nordicModelTypeMech;

	// Token: 0x0400072D RID: 1837
	private int rusvietModelTypeMech;

	// Token: 0x0400072E RID: 1838
	private int togawaModelTypeMech;

	// Token: 0x0400072F RID: 1839
	private int crimeaModelTypeWorker;

	// Token: 0x04000730 RID: 1840
	private int saxonyModelTypeWorker;

	// Token: 0x04000731 RID: 1841
	private int polaniaModelTypeWorker;

	// Token: 0x04000732 RID: 1842
	private int albionModelTypeWorker;

	// Token: 0x04000733 RID: 1843
	private int nordicModelTypeWorker;

	// Token: 0x04000734 RID: 1844
	private int rusvietModelTypeWorker;

	// Token: 0x04000735 RID: 1845
	private int togawaModelTypeWorker;

	// Token: 0x04000736 RID: 1846
	public Camera worldCamera;

	// Token: 0x04000737 RID: 1847
	public Camera mechCamera;

	// Token: 0x04000738 RID: 1848
	public bool isMechCameraView;

	// Token: 0x04000739 RID: 1849
	public int unitType;

	// Token: 0x0400073A RID: 1850
	public int allModelsType;

	// Token: 0x0400073B RID: 1851
	public GameObject activePresenter;

	// Token: 0x0400073C RID: 1852
	public List<GameObject> unitPresenters;

	// Token: 0x0400073D RID: 1853
	public float _sensitivity;

	// Token: 0x0400073E RID: 1854
	private Vector3 _mouseReference;

	// Token: 0x0400073F RID: 1855
	private Vector3 _mouseOffset;

	// Token: 0x04000740 RID: 1856
	private Vector3 _rotation;

	// Token: 0x04000741 RID: 1857
	public bool _isRotating;

	// Token: 0x04000742 RID: 1858
	public GameObject plainSkinButton;

	// Token: 0x04000743 RID: 1859
	public GameObject paintedSkinButton;

	// Token: 0x04000744 RID: 1860
	public GameObject allModelTypesToggle;

	// Token: 0x04000745 RID: 1861
	public List<GameObject> factionButtons;

	// Token: 0x04000746 RID: 1862
	public List<GameObject> unitTypeToggles;

	// Token: 0x04000747 RID: 1863
	public float defaultUnitRotation;

	// Token: 0x04000748 RID: 1864
	public OptionsManager optionsManager;

	// Token: 0x04000749 RID: 1865
	public static string CRIMEA_MODEL_TYPE_CHARACTER = "CrimeaModelTypeCharacter";

	// Token: 0x0400074A RID: 1866
	public static string SAXONY_MODEL_TYPE_CHARACTER = "SaxonyModelTypeCharacter";

	// Token: 0x0400074B RID: 1867
	public static string POLANIA_MODEL_TYPE_CHARACTER = "PolaniaModelTypeCharacter";

	// Token: 0x0400074C RID: 1868
	public static string ALBION_MODEL_TYPE_CHARACTER = "AlbionModelTypeCharacter";

	// Token: 0x0400074D RID: 1869
	public static string NORDIC_MODEL_TYPE_CHARACTER = "NordicModelTypeCharacter";

	// Token: 0x0400074E RID: 1870
	public static string RUSVIET_MODEL_TYPE_CHARACTER = "RusvietModelTypeCharacter";

	// Token: 0x0400074F RID: 1871
	public static string TOGAWA_MODEL_TYPE_CHARACTER = "TogawaModelTypeCharacter";

	// Token: 0x04000750 RID: 1872
	public static string CRIMEA_MODEL_TYPE_MECH = "CrimeaModelType";

	// Token: 0x04000751 RID: 1873
	public static string SAXONY_MODEL_TYPE_MECH = "SaxonyModelType";

	// Token: 0x04000752 RID: 1874
	public static string POLANIA_MODEL_TYPE_MECH = "PolaniaModelType";

	// Token: 0x04000753 RID: 1875
	public static string ALBION_MODEL_TYPE_MECH = "AlbionModelType";

	// Token: 0x04000754 RID: 1876
	public static string NORDIC_MODEL_TYPE_MECH = "NordicModelType";

	// Token: 0x04000755 RID: 1877
	public static string RUSVIET_MODEL_TYPE_MECH = "RusvietModelType";

	// Token: 0x04000756 RID: 1878
	public static string TOGAWA_MODEL_TYPE_MECH = "TogawaModelType";

	// Token: 0x04000757 RID: 1879
	public static string CRIMEA_MODEL_TYPE_WORKER = "CrimeaModelTypeWorker";

	// Token: 0x04000758 RID: 1880
	public static string SAXONY_MODEL_TYPE_WORKER = "SaxonyModelTypeWorker";

	// Token: 0x04000759 RID: 1881
	public static string POLANIA_MODEL_TYPE_WORKER = "PolaniaModelTypeWorker";

	// Token: 0x0400075A RID: 1882
	public static string ALBION_MODEL_TYPE_WORKER = "AlbionModelTypeWorker";

	// Token: 0x0400075B RID: 1883
	public static string NORDIC_MODEL_TYPE_WORKER = "NordicModelTypeWorker";

	// Token: 0x0400075C RID: 1884
	public static string RUSVIET_MODEL_TYPE_WORKER = "RusvietModelTypeWorker";

	// Token: 0x0400075D RID: 1885
	public static string TOGAWA_MODEL_TYPE_WORKER = "TogawaModelTypeWorker";
}
