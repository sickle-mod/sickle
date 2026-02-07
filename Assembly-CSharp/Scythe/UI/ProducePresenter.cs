using System;
using System.Collections.Generic;
using System.Linq;
using HoneyFramework;
using I2.Loc;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003FE RID: 1022
	public class ProducePresenter : ActionPresenter
	{
		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06001F0D RID: 7949 RVA: 0x000BEC58 File Offset: 0x000BCE58
		// (remove) Token: 0x06001F0E RID: 7950 RVA: 0x000BEC8C File Offset: 0x000BCE8C
		public static event ProducePresenter.ProductionStart ProductionStarted;

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06001F0F RID: 7951 RVA: 0x000BECC0 File Offset: 0x000BCEC0
		// (remove) Token: 0x06001F10 RID: 7952 RVA: 0x000BECF4 File Offset: 0x000BCEF4
		public static event ProducePresenter.ProductionEnd ProductionEnded;

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06001F11 RID: 7953 RVA: 0x000BED28 File Offset: 0x000BCF28
		// (remove) Token: 0x06001F12 RID: 7954 RVA: 0x000BED5C File Offset: 0x000BCF5C
		public static event ProducePresenter.HexProductionClick OnHexProductionClicked;

		// Token: 0x06001F13 RID: 7955 RVA: 0x0003C137 File Offset: 0x0003A337
		private void Awake()
		{
			this.InitProductionGridPool();
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x000BED90 File Offset: 0x000BCF90
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainProduce;
			GameController.ClearFocus();
			if (!PlatformManager.IsStandalone)
			{
				for (int i = 0; i < this.mobileInfoTiles.Length; i++)
				{
					if (i != this.mobileInfoTiles.Length - 1)
					{
						this.mobileInfoTiles[i].SetActive(i < (int)this.action.Amount);
					}
					else
					{
						Building building = action.GetPlayer().matPlayer.GetBuilding(BuildingType.Mill);
						this.mobileInfoTiles[i].SetActive(building != null);
					}
					this.mobileInfoCheckmarks[i].SetActive(false);
				}
				GameController.HexSelectionMode = GameController.SelectionMode.PayResource;
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				GameController.Instance.matFaction.ClearHintStories();
				this.produceInfo.SetActive(true);
				(GameController.Instance.gameBoardPresenter as FlatWorld).SetCollidersEnabledOnUnits(false, null, null);
			}
			if (this.action.GainAvaliable())
			{
				this.AssignProductionGridsToHexes();
				this.UpdateHexPointers();
				base.EnableMapBlackout();
				if (PlatformManager.IsStandalone)
				{
					this.EndProduceButton.SetActive(true);
				}
				if (ProducePresenter.ProductionStarted != null)
				{
					ProducePresenter.ProductionStarted();
					return;
				}
			}
			else
			{
				this.OnActionEnded();
			}
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x000BEEB8 File Offset: 0x000BD0B8
		public void ResetHexColors()
		{
			foreach (ProductionGridPresenter productionGridPresenter in this.productionGrids.Values)
			{
				productionGridPresenter.hexBackground.color = Color.white;
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0003C13F File Offset: 0x0003A33F
		private void UpdateHexPointers()
		{
			GameController.Instance.hexPointerController.SetHexesWithResource(this.productionGrids.Keys.ToList<Scythe.BoardPresenter.GameHexPresenter>());
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000BEF18 File Offset: 0x000BD118
		private void InitProductionGridPool()
		{
			this.productionGridsPool = new List<GameObject>();
			for (int i = 0; i < 9; i++)
			{
				this.productionGridsPool.Add(global::UnityEngine.Object.Instantiate<GameObject>(this.ProductionImagesGrid));
				this.productionGridsPool[i].SetActive(false);
				this.productionGridsPool[i].transform.SetParent(this.GridClipboard.transform);
				this.productionGridsPool[i].GetComponent<ProductionGridPresenter>().FillImageGrid();
				this.productionGridsPool[i].GetComponent<ProductionGridPresenter>().producePresenter = this;
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000BEFB4 File Offset: 0x000BD1B4
		private void DisableGrids()
		{
			foreach (GameObject gameObject in this.productionGridsPool)
			{
				gameObject.GetComponent<ProductionGridPresenter>().TurnOffGrid();
				gameObject.SetActive(false);
			}
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000BF010 File Offset: 0x000BD210
		private void DisableGridsExceptMill()
		{
			foreach (GameObject gameObject in this.productionGridsPool)
			{
				if (!gameObject.GetComponent<ProductionGridPresenter>().hasMill)
				{
					gameObject.GetComponent<ProductionGridPresenter>().TurnOffGrid();
					gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000BF07C File Offset: 0x000BD27C
		private void ClearGrids()
		{
			foreach (GameObject gameObject in this.productionGridsPool)
			{
				gameObject.GetComponent<ProductionGridPresenter>().Clear();
			}
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000BF0D4 File Offset: 0x000BD2D4
		private void AssignProductionGridsToHexes()
		{
			GameController.Instance.endTurnButton.interactable = false;
			this.productionGrids = new Dictionary<Scythe.BoardPresenter.GameHexPresenter, ProductionGridPresenter>();
			foreach (Worker worker in this.action.GetPlayer().matPlayer.workers)
			{
				this.AssignHexToUnusedGrid(GameController.Instance.GetGameHexPresenter(worker.position));
			}
			if (this.action.MillProduce && !this.FactoryCardProduce())
			{
				Building building = this.action.GetPlayer().matPlayer.GetBuilding(BuildingType.Mill);
				if (building != null && building.position.Owner == building.player)
				{
					this.AssignHexToUnusedGrid(GameController.Instance.GetGameHexPresenter(building.position));
				}
			}
			if (GameController.Instance.AdjustingPresenters)
			{
				foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.productionGrids.Keys.ToArray<Scythe.BoardPresenter.GameHexPresenter>())
				{
					if (this.action.ResourceFields.ContainsKey(gameHexPresenter.GetGameHexLogic()))
					{
						this.clickedHexes++;
						if (!this.IsHexUnusedMillField(gameHexPresenter))
						{
							this.clickedNonMillHexes++;
						}
						this.RemoveUsedHex(gameHexPresenter);
					}
				}
			}
			this.UpdateHexesLeftInfo();
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0003C160 File Offset: 0x0003A360
		public bool FactoryCardProduce()
		{
			return this.action.GetPlayer().currentMatSection == 4;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000BF23C File Offset: 0x000BD43C
		private void AssignHexToUnusedGrid(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (!this.productionGrids.ContainsKey(hex))
			{
				if (hex.hexType == HexType.capital || hex.hexType == HexType.factory || hex.hexType == HexType.lake)
				{
					return;
				}
				ProductionGridPresenter component = this.productionGridsPool.Find((GameObject grid) => !grid.activeInHierarchy).GetComponent<ProductionGridPresenter>();
				component.AttachToHex(hex);
				this.productionGrids.Add(hex, component);
			}
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0003C175 File Offset: 0x0003A375
		public bool HaveAction()
		{
			return this.action != null;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000BF2BC File Offset: 0x000BD4BC
		public void EnableVisuals(bool enabled)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.productionGrids.Keys)
			{
				if (this.productionGrids[gameHexPresenter].gameObject.activeSelf)
				{
					this.productionGrids[gameHexPresenter].EnableMarker(enabled);
				}
			}
			this.GridClipboard.SetActive(enabled);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000BF344 File Offset: 0x000BD544
		public void OnHexSelectedMobile(Scythe.BoardPresenter.GameHexPresenter hex, int amountToProduce)
		{
			if (this.action.FieldsLeftToProduce == 0 && !this.IsHexUnusedMillField(hex))
			{
				return;
			}
			if (this.productionGrids.ContainsKey(hex))
			{
				this.produceAmountMobile = (this.produceAmountMaxMobile = amountToProduce);
				this.produceHexMobile = hex;
				HexType hexType = hex.hexType;
				switch (hexType)
				{
				case HexType.mountain:
					this.produceResourceIconMobile.sprite = this.ToProduce[1];
					break;
				case HexType.forest:
					this.produceResourceIconMobile.sprite = this.ToProduce[3];
					break;
				case (HexType)3:
					break;
				case HexType.farm:
					this.produceResourceIconMobile.sprite = this.ToProduce[2];
					break;
				default:
					if (hexType != HexType.tundra)
					{
						if (hexType == HexType.village)
						{
							this.produceResourceIconMobile.sprite = this.ToProduce[4];
						}
					}
					else
					{
						this.produceResourceIconMobile.sprite = this.ToProduce[0];
					}
					break;
				}
				this.produceButtonMore.interactable = false;
				this.produceButtonLess.interactable = true;
				this.produceCounterMobile.text = amountToProduce.ToString();
				if (!GameController.GameManager.IsCampaign)
				{
					this.produceWindowMobile.SetActive(true);
					return;
				}
				this.OnHexApprovedMobile();
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000BF46C File Offset: 0x000BD66C
		public void OnAmountIncreaseMobile()
		{
			if (this.produceAmountMobile < this.produceAmountMaxMobile)
			{
				this.produceAmountMobile++;
			}
			this.produceCounterMobile.text = this.produceAmountMobile.ToString();
			this.produceButtonLess.interactable = this.produceAmountMobile > 0;
			this.produceButtonMore.interactable = this.produceAmountMobile < this.produceAmountMaxMobile;
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000BF4D8 File Offset: 0x000BD6D8
		public void OnAmountDecreaseMobile()
		{
			if (this.produceAmountMobile > 0)
			{
				this.produceAmountMobile--;
			}
			this.produceCounterMobile.text = this.produceAmountMobile.ToString();
			this.produceButtonLess.interactable = this.produceAmountMobile > 0;
			this.produceButtonMore.interactable = this.produceAmountMobile < this.produceAmountMaxMobile;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000BF540 File Offset: 0x000BD740
		public void OnHexApprovedMobile()
		{
			this.produceWindowMobile.SetActive(false);
			if (!PlatformManager.IsStandalone)
			{
				Building building = this.produceHexMobile.GetGameHexLogic().Building;
				if (building != null && building.buildingType == BuildingType.Mill && building.player == this.action.GetPlayer())
				{
					this.mobileInfoCheckmarks[this.mobileInfoCheckmarks.Length - 1].SetActive(true);
				}
				else
				{
					int num = (int)this.action.Amount - this.action.FieldsLeftToProduce;
					this.mobileInfoCheckmarks[num].SetActive(true);
				}
			}
			this.OnHexApproved(this.produceHexMobile, this.produceAmountMobile);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0003C180 File Offset: 0x0003A380
		public void OnXClickedInProduceWindowMobile()
		{
			this.produceWindowMobile.SetActive(false);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000BF5E4 File Offset: 0x000BD7E4
		public void OnHexApproved(Scythe.BoardPresenter.GameHexPresenter hex, int amountToProduce)
		{
			if (this.action.FieldsLeftToProduce == 0 && !this.IsHexUnusedMillField(hex) && !this.FactoryCardProduce())
			{
				return;
			}
			if (this.productionGrids.ContainsKey(hex))
			{
				this.clickedHexes++;
				if (this.IsHexUnusedMillField(hex) && !this.FactoryCardProduce())
				{
					this.action.MillProduce = true;
				}
				else
				{
					this.clickedNonMillHexes++;
				}
				this.action.ExecuteOnce(hex.GetGameHexLogic(), amountToProduce);
				if (hex.hexType == HexType.forest && amountToProduce > 0)
				{
					hex.UpdateTempResource(ResourceType.wood, true, true, amountToProduce);
					hex.ProduceAnimation(amountToProduce, ResourceType.wood, false);
				}
				else if (hex.hexType == HexType.tundra && amountToProduce > 0)
				{
					hex.UpdateTempResource(ResourceType.oil, true, true, amountToProduce);
					hex.ProduceAnimation(amountToProduce, ResourceType.oil, false);
				}
				else if (hex.hexType == HexType.farm && amountToProduce > 0)
				{
					hex.UpdateTempResource(ResourceType.food, true, true, amountToProduce);
					hex.ProduceAnimation(amountToProduce, ResourceType.food, false);
				}
				else if (hex.hexType == HexType.mountain && amountToProduce > 0)
				{
					hex.UpdateTempResource(ResourceType.metal, true, true, amountToProduce);
					hex.ProduceAnimation(amountToProduce, ResourceType.metal, false);
				}
				if (ProducePresenter.OnHexProductionClicked != null)
				{
					ProducePresenter.OnHexProductionClicked(hex, amountToProduce);
				}
				this.RemoveUsedHex(hex);
				if (hex.hexType == HexType.village)
				{
					this.UpdateGridsForVillages();
				}
				GameController.Instance.gameBoardPresenter.UpdateBoard(true, true);
				GameController.Instance.OnUpdatePlayerStats();
				this.UpdateHexesLeftInfo();
				if (this.action.FieldsLeftToProduce == 0 && this.IsMillNotUsed() && this.action.MillProduce && !this.FactoryCardProduce())
				{
					this.DisableGridsExceptMill();
				}
				if (this.action == null || (this.IsLastProductionOnVillage() && this.action.GetPlayer().matPlayer.workers.Count == 8) || (this.action.FieldsLeftToProduce == 0 && !this.IsMillNotUsed()) || (this.productionGrids.Count == 0 || (this.action.FieldsLeftToProduce == 0 && this.IsMillNotUsed() && !this.CanMillProduceWorker() && this.IsMillOnVillage())) || (this.action.FieldsLeftToProduce == 0 && this.FactoryCardProduce()))
				{
					this.DisableGrids();
					this.OnActionEnded();
				}
			}
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000BF808 File Offset: 0x000BDA08
		private bool IsHexUnusedMillField(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			Building building = hex.GetGameHexLogic().Building;
			return this.productionGrids.ContainsKey(hex) && building != null && building.buildingType == BuildingType.Mill && building.player == this.action.GetPlayer();
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000BF854 File Offset: 0x000BDA54
		private void UpdateGridsForVillages()
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.productionGrids.Keys.ToList<Scythe.BoardPresenter.GameHexPresenter>())
			{
				if (gameHexPresenter.GetGameHexLogic().hexType == HexType.village)
				{
					if (this.action.GetPlayer().matPlayer.workers.Count == 8)
					{
						this.RemoveUsedHex(gameHexPresenter);
					}
					else
					{
						this.productionGrids[gameHexPresenter].UpdateProductionGrid();
					}
				}
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000BF8F0 File Offset: 0x000BDAF0
		private void RemoveUsedHex(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (this.productionGrids.ContainsKey(hex))
			{
				this.productionGrids[hex].Clear();
				this.productionGrids[hex].gameObject.SetActive(false);
				this.productionGrids.Remove(hex);
				hex.SetFocus(false, HexMarkers.MarkerType.Move, 1f, false);
				Building building = this.action.GetPlayer().matPlayer.GetBuilding(BuildingType.Mill);
				if (hex.GetGameHexLogic().Building != null && building != null && building.position == hex.GetGameHexLogic())
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.UseBuilding();
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.ProduceMill();
				}
				else
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.AcquireGain(this.clickedNonMillHexes - 1);
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.ProduceRegular(this.clickedNonMillHexes - 1);
				}
				this.UpdateHexPointers();
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000BFA54 File Offset: 0x000BDC54
		public bool IsMillNotUsed()
		{
			if (this.action == null)
			{
				return false;
			}
			Building building = this.action.GetPlayer().matPlayer.GetBuilding(BuildingType.Mill);
			if (building == null)
			{
				return false;
			}
			foreach (KeyValuePair<Scythe.BoardPresenter.GameHexPresenter, ProductionGridPresenter> keyValuePair in this.productionGrids)
			{
				if (keyValuePair.Key.GetGameHexLogic().Building == building)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000BFAE4 File Offset: 0x000BDCE4
		private bool IsLastProductionOnVillage()
		{
			return this.productionGrids.Count == 1 && this.productionGrids.FirstOrDefault<KeyValuePair<Scythe.BoardPresenter.GameHexPresenter, ProductionGridPresenter>>().Key.hexType == HexType.village;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0003C18E File Offset: 0x0003A38E
		public bool CanMillProduceWorker()
		{
			return this.IsMillOnVillage() && this.action.GetPlayer().matPlayer.workers.Count != 8;
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x000BFB20 File Offset: 0x000BDD20
		public bool IsMillOnVillage()
		{
			Building building = this.action.GetPlayer().matPlayer.GetBuilding(BuildingType.Mill);
			return building != null && building.position.hexType == HexType.village;
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000BFB5C File Offset: 0x000BDD5C
		private void UpdateHexesLeftInfo()
		{
			string text = ScriptLocalization.Get("PlayerMat/ProductionInfo");
			int num;
			if (this.action != null && this.productionGrids.Count >= this.action.FieldsLeftToProduce)
			{
				num = this.action.FieldsLeftToProduce;
			}
			else
			{
				num = this.productionGrids.Count;
			}
			if (this.IsMillNotUsed())
			{
				if (this.productionGrids.Count == num)
				{
					num--;
				}
				text = text + num.ToString() + ScriptLocalization.Get("PlayerMat/MillAddition");
			}
			else
			{
				text += num.ToString();
			}
			if (this.HexesLeftInfo != null)
			{
				this.HexesLeftInfo.text = text;
			}
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0003C1BA File Offset: 0x0003A3BA
		public static void ProductionButtonClick(Scythe.BoardPresenter.GameHexPresenter hex, bool isInteractable = true)
		{
			global::UnityEngine.Object.FindObjectOfType<ProducePresenter>().productionGrids[hex].produceButton.interactable = isInteractable;
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x000BFC0C File Offset: 0x000BDE0C
		private bool GridHexesRemaining()
		{
			using (Dictionary<Scythe.BoardPresenter.GameHexPresenter, ProductionGridPresenter>.ValueCollection.Enumerator enumerator = this.productionGrids.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.AmountToProduce > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000BFC6C File Offset: 0x000BDE6C
		public void WarningBeforeActionEnd()
		{
			if (this.produceWindowMobile != null)
			{
				this.produceWindowMobile.gameObject.SetActive(false);
			}
			if ((this.action.FieldsLeftToProduce > 0 || this.IsMillNotUsed()) && OptionsManager.IsWarningsActive() && GameController.Instance.matPlayer.SuitablePlaceForProduceCheck() && this.GridHexesRemaining())
			{
				if (PlatformManager.IsStandalone)
				{
					this.endAction.GetComponentInChildren<Text>().text = ScriptLocalization.Get("PlayerMat/EndProductionText");
				}
				else
				{
					this.endAction.GetFirstDescriptionLine().GetComponent<TextMeshProUGUI>().text = ScriptLocalization.Get("PlayerMat/EndProductionText");
				}
				WorldSFXManager.PlaySound(SoundEnum.Alert, AudioSourceType.Buttons);
				this.endAction.Show(GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo, delegate
				{
					this.OnActionEnded();
				}, null);
				return;
			}
			this.OnActionEnded();
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x000BFD68 File Offset: 0x000BDF68
		public override void OnActionEnded()
		{
			if (this.action != null)
			{
				this.action.SelectAction();
			}
			GameController.ClearFocus();
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			GameController.Instance.EndTurnButtonEnable();
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.productionGrids.Keys)
			{
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.Move, 1f, false);
			}
			GameController.Instance.hexPointerController.Clear();
			this.DisableGrids();
			this.ClearGrids();
			base.DisableMapBlackout();
			this.productionGrids.Clear();
			if (PlatformManager.IsStandalone)
			{
				this.EndProduceButton.SetActive(false);
			}
			this.mapDarken.enabled = false;
			if (!PlatformManager.IsStandalone)
			{
				(GameController.Instance.gameBoardPresenter as FlatWorld).SetCollidersEnabledOnUnits(true, null, null);
				this.produceInfo.SetActive(false);
			}
			HumanInputHandler.Instance.OnInputEnded();
			GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.OnProduceEnded();
			this.action = null;
			if (ProducePresenter.ProductionEnded != null)
			{
				ProducePresenter.ProductionEnded();
			}
			this.clickedHexes = 0;
			this.clickedNonMillHexes = 0;
			base.gameObject.SetActive(false);
		}

		// Token: 0x040015E8 RID: 5608
		public GameObject ProductionImagesGrid;

		// Token: 0x040015E9 RID: 5609
		public GameObject GridClipboard;

		// Token: 0x040015EA RID: 5610
		public Sprite[] ToProduce;

		// Token: 0x040015EB RID: 5611
		public Text HexesLeftInfo;

		// Token: 0x040015EC RID: 5612
		public GameObject EndProduceButton;

		// Token: 0x040015ED RID: 5613
		public Image mapDarken;

		// Token: 0x040015F1 RID: 5617
		public YesNoDialog endAction;

		// Token: 0x040015F2 RID: 5618
		public GameObject produceInfo;

		// Token: 0x040015F3 RID: 5619
		public GameObject[] mobileInfoTiles;

		// Token: 0x040015F4 RID: 5620
		public GameObject[] mobileInfoCheckmarks;

		// Token: 0x040015F5 RID: 5621
		private GainProduce action;

		// Token: 0x040015F6 RID: 5622
		public int clickedHexes;

		// Token: 0x040015F7 RID: 5623
		private Dictionary<Scythe.BoardPresenter.GameHexPresenter, ProductionGridPresenter> productionGrids;

		// Token: 0x040015F8 RID: 5624
		private List<GameObject> productionGridsPool;

		// Token: 0x040015F9 RID: 5625
		private int clickedNonMillHexes;

		// Token: 0x040015FA RID: 5626
		public GameObject produceWindowMobile;

		// Token: 0x040015FB RID: 5627
		public Image produceResourceIconMobile;

		// Token: 0x040015FC RID: 5628
		public TextMeshProUGUI produceCounterMobile;

		// Token: 0x040015FD RID: 5629
		public Button produceButtonLess;

		// Token: 0x040015FE RID: 5630
		public Button produceButtonMore;

		// Token: 0x040015FF RID: 5631
		private int produceAmountMobile;

		// Token: 0x04001600 RID: 5632
		private int produceAmountMaxMobile;

		// Token: 0x04001601 RID: 5633
		private Scythe.BoardPresenter.GameHexPresenter produceHexMobile;

		// Token: 0x020003FF RID: 1023
		// (Invoke) Token: 0x06001F35 RID: 7989
		public delegate void ProductionStart();

		// Token: 0x02000400 RID: 1024
		// (Invoke) Token: 0x06001F39 RID: 7993
		public delegate void ProductionEnd();

		// Token: 0x02000401 RID: 1025
		// (Invoke) Token: 0x06001F3D RID: 7997
		public delegate void HexProductionClick(Scythe.BoardPresenter.GameHexPresenter hex, int amount);
	}
}
