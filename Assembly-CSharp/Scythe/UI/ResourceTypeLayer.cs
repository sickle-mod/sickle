using System;
using System.Collections.Generic;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200041D RID: 1053
	public class ResourceTypeLayer : MonoBehaviour
	{
		// Token: 0x06002033 RID: 8243 RVA: 0x0003CA49 File Offset: 0x0003AC49
		public void Init()
		{
			this.fields.Clear();
			this.fieldMappings.Clear();
			this.hexIcons.Clear();
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000C3FE0 File Offset: 0x000C21E0
		public void AddFlatHex(FlatGameHexPresenter hex)
		{
			if (!this.fields.Contains(hex))
			{
				GameHex gameHexLogic = hex.GetGameHexLogic();
				if (gameHexLogic != null)
				{
					HexType hexType = gameHexLogic.hexType;
					if (hexType == HexType.farm || hexType == HexType.forest || hexType == HexType.mountain || hexType == HexType.tundra || hexType == HexType.village || hexType == HexType.lake || hexType == HexType.capital || hexType == HexType.factory)
					{
						this.resourcesIconContainer = this.resourceMarkPrefab.GetComponent<ResourcesIconContainer>();
						if (hex.hasEncounter)
						{
							GameObject gameObject = new GameObject("Encounter");
							gameObject.AddComponent<SpriteRenderer>();
							gameObject.transform.SetParent(hex.hexObject.transform);
							gameObject.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
							gameObject.transform.localPosition = new Vector3(-0.05f, 1.43f, 0f);
							gameObject.transform.localEulerAngles = new Vector3(180f, 90f, 0f);
							gameObject.GetComponent<SpriteRenderer>().sprite = this.resourcesIconContainer.takenEcounter;
							gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
							hex.fadedEncounterMarker = gameObject;
						}
						if (hex.hexTemplate != this.fieldTemplate)
						{
							this.hexIcons.Add(hex.hexTemplate);
						}
					}
				}
			}
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x0003CA6C File Offset: 0x0003AC6C
		public void OnToggle(bool state)
		{
			if (PlatformManager.IsStandalone)
			{
				base.gameObject.SetActive(state);
			}
			this.SetHexTypeMarkersActive(state);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000C4140 File Offset: 0x000C2340
		public void SetHexTypeColorsActive(bool active)
		{
			this.waterMap.enabled = active;
			foreach (GameObject gameObject in this.hexIcons)
			{
				gameObject.transform.GetChild(1).gameObject.SetActive(active);
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000C41B0 File Offset: 0x000C23B0
		private void SetHexTypeMarkersActive(bool active)
		{
			foreach (GameObject gameObject in this.hexIcons)
			{
				gameObject.transform.GetChild(0).gameObject.SetActive(active);
			}
		}

		// Token: 0x04001695 RID: 5781
		public GameObject resourceMarkPrefab;

		// Token: 0x04001696 RID: 5782
		public Toggle showToggle;

		// Token: 0x04001697 RID: 5783
		public SpriteRenderer waterMap;

		// Token: 0x04001698 RID: 5784
		public GameObject fieldTemplate;

		// Token: 0x04001699 RID: 5785
		private Dictionary<Transform, GameHexPresenter> fieldMappings = new Dictionary<Transform, GameHexPresenter>();

		// Token: 0x0400169A RID: 5786
		private HashSet<GameHexPresenter> fields = new HashSet<GameHexPresenter>();

		// Token: 0x0400169B RID: 5787
		private ResourcesIconContainer resourcesIconContainer;

		// Token: 0x0400169C RID: 5788
		private GameHexPresenter tempGameHexPresenter;

		// Token: 0x0400169D RID: 5789
		private List<GameObject> hexIcons = new List<GameObject>();
	}
}
