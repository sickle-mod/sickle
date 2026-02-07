using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000418 RID: 1048
	public class PlayerUnits : MonoBehaviour
	{
		// Token: 0x06001FFB RID: 8187 RVA: 0x0003C83E File Offset: 0x0003AA3E
		private void Start()
		{
			this.SetFactionModelSkin();
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000C2FD0 File Offset: 0x000C11D0
		public void Init()
		{
			this.LinkUnits();
			this.characterObject.gameObject.SetActive(false);
			for (int i = 0; i < 4; i++)
			{
				this.mechObjects[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < 8; j++)
			{
				this.workerObjects[j].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000C3034 File Offset: 0x000C1234
		public void CharacterFactionColor()
		{
			if (this.workerObjects != null && this.workerObjects.Length != 0)
			{
				MeshRenderer componentInChildren = this.workerObjects[0].GetComponentInChildren<MeshRenderer>();
				if (componentInChildren != null)
				{
					Color color = componentInChildren.material.color;
					SkinnedMeshRenderer[] componentsInChildren = this.characterObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						Material[] array = componentsInChildren[i].materials;
						for (int j = 0; j < array.Length; j++)
						{
							array[j].color = color;
						}
					}
					foreach (MeshRenderer meshRenderer in this.characterObject.GetComponentsInChildren<MeshRenderer>(true))
					{
						if (meshRenderer.gameObject != this.characterObject.coasterObject && meshRenderer.transform.parent.gameObject != this.characterObject.coasterObject)
						{
							Material[] array = meshRenderer.materials;
							for (int j = 0; j < array.Length; j++)
							{
								array[j].color = color;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000C3148 File Offset: 0x000C1348
		public void LinkUnits()
		{
			this.unitMap = new Dictionary<Unit, UnitPresenter>();
			Player playerByFaction = GameController.GameManager.GetPlayerByFaction(this.faction);
			if (playerByFaction != null)
			{
				this.characterObject.SetLogicUnit(playerByFaction.character);
				playerByFaction.character.IndexOnHex = 0;
				this.unitMap.Add(playerByFaction.character, this.characterObject);
				for (int i = 0; i < 4; i++)
				{
					if (i < playerByFaction.matFaction.mechs.Count)
					{
						this.mechObjects[i].SetLogicUnit(playerByFaction.matFaction.mechs[i]);
						playerByFaction.matFaction.mechs[i].IndexOnHex = i;
						this.unitMap.Add(playerByFaction.matFaction.mechs[i], this.mechObjects[i]);
					}
				}
				for (int j = 0; j < 8; j++)
				{
					if (j < playerByFaction.matPlayer.workers.Count)
					{
						this.workerObjects[j].SetLogicUnit(playerByFaction.matPlayer.workers[j]);
						playerByFaction.matPlayer.workers[j].IndexOnHex = j;
						this.unitMap.Add(playerByFaction.matPlayer.workers[j], this.workerObjects[j]);
					}
				}
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0003C846 File Offset: 0x0003AA46
		public UnitPresenter GetUnitPresenter(Unit unit)
		{
			if (this.unitMap.ContainsKey(unit))
			{
				return this.unitMap[unit];
			}
			this.LinkUnits();
			if (this.unitMap.ContainsKey(unit))
			{
				return this.unitMap[unit];
			}
			return null;
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x000C329C File Offset: 0x000C149C
		public void SetColliders(bool enabled)
		{
			this.characterObject.SetColliderEnabled(enabled);
			for (int i = 0; i < 4; i++)
			{
				this.mechObjects[i].SetColliderEnabled(enabled);
			}
			for (int j = 0; j < 8; j++)
			{
				this.workerObjects[j].SetColliderEnabled(enabled);
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000C32EC File Offset: 0x000C14EC
		public static void ChangeUnitsColliderState(bool enabled)
		{
			foreach (PlayerUnits playerUnits2 in GameController.Instance.playerUnits)
			{
				if (playerUnits2 != null)
				{
					playerUnits2.SetColliders(enabled);
				}
			}
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x000C3328 File Offset: 0x000C1528
		private void SetAllModelSkins()
		{
			this.characterObject.SetSkin(this.characterSkinMode);
			UnitPresenter[] array = this.mechObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetSkin(this.mechSkinMode);
			}
			array = this.workerObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetSkin(this.workerSkinMode);
			}
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x000C338C File Offset: 0x000C158C
		public void SetFactionModelSkin()
		{
			string name = base.gameObject.name;
			uint num = global::<PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1313277548U)
			{
				if (num != 391982364U)
				{
					if (num != 1283434229U)
					{
						if (num == 1313277548U)
						{
							if (name == "Nordic")
							{
								this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_CHARACTER);
								this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_MECH);
								this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.NORDIC_MODEL_TYPE_WORKER);
								goto IL_028A;
							}
						}
					}
					else if (name == "Saxony")
					{
						this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_CHARACTER);
						this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_MECH);
						this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.SAXONY_MODEL_TYPE_WORKER);
						goto IL_028A;
					}
				}
				else if (name == "Crimea")
				{
					this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_CHARACTER);
					this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_MECH);
					this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.CRIMEA_MODEL_TYPE_WORKER);
					goto IL_028A;
				}
			}
			else if (num <= 2871816328U)
			{
				if (num != 1743574061U)
				{
					if (num == 2871816328U)
					{
						if (name == "Albion")
						{
							this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_CHARACTER);
							this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_MECH);
							this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.ALBION_MODEL_TYPE_WORKER);
							goto IL_028A;
						}
					}
				}
				else if (name == "Rusviet")
				{
					this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_CHARACTER);
					this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_MECH);
					this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.RUSVIET_MODEL_TYPE_WORKER);
					goto IL_028A;
				}
			}
			else if (num != 3621391629U)
			{
				if (num == 4012704324U)
				{
					if (name == "Togawa")
					{
						this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_CHARACTER);
						this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_MECH);
						this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.TOGAWA_MODEL_TYPE_WORKER);
						goto IL_028A;
					}
				}
			}
			else if (name == "Polania")
			{
				this.characterSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_CHARACTER);
				this.mechSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_MECH);
				this.workerSkinMode = PlayerPrefs.GetInt(ShowroomPlatformPresenter.POLANIA_MODEL_TYPE_WORKER);
				goto IL_028A;
			}
			Debug.Log("No such faction detected. Could not set proper model variant. Please check code in " + base.gameObject.name);
			IL_028A:
			this.SetAllModelSkins();
		}

		// Token: 0x04001676 RID: 5750
		public Faction faction;

		// Token: 0x04001677 RID: 5751
		public UnitPresenter[] mechObjects;

		// Token: 0x04001678 RID: 5752
		public UnitPresenter[] workerObjects;

		// Token: 0x04001679 RID: 5753
		public UnitPresenter characterObject;

		// Token: 0x0400167A RID: 5754
		private Dictionary<Unit, UnitPresenter> unitMap;

		// Token: 0x0400167B RID: 5755
		public int characterSkinMode;

		// Token: 0x0400167C RID: 5756
		public int mechSkinMode;

		// Token: 0x0400167D RID: 5757
		public int workerSkinMode;
	}
}
