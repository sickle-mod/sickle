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
	// Token: 0x020003F7 RID: 1015
	public class PayResourcePresenter : ActionPresenter
	{
		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06001EBC RID: 7868 RVA: 0x000BCA70 File Offset: 0x000BAC70
		// (remove) Token: 0x06001EBD RID: 7869 RVA: 0x000BCAA4 File Offset: 0x000BACA4
		public static event PayResourcePresenter.PayResourceStart PayResourceStarted;

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06001EBE RID: 7870 RVA: 0x000BCAD8 File Offset: 0x000BACD8
		// (remove) Token: 0x06001EBF RID: 7871 RVA: 0x000BCB0C File Offset: 0x000BAD0C
		public static event PayResourcePresenter.PayResourceEnd PayResourceEnded;

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000BCB40 File Offset: 0x000BAD40
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as PayResource;
			this.hexesWithResource.Clear();
			this.selectedHex = null;
			this.actionPayed = false;
			this.choosenCombatCard = null;
			this.combatCardIndex = -1;
			GameController.resourcePayed = 0;
			for (int i = 0; i < 4; i++)
			{
				this.selectedHexes[i] = null;
			}
			this.EnableInput();
			if (PayResourcePresenter.PayResourceStarted != null)
			{
				PayResourcePresenter.PayResourceStarted();
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000BCBB8 File Offset: 0x000BADB8
		private void EnableInput()
		{
			GameController.HexGetFocused += this.OnHexSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.PayResource;
			if (!PlatformManager.IsStandalone)
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				GameController.Instance.matFaction.ClearHintStories();
			}
			if (this.action.DifferentResources || this.action.AnyResource)
			{
				if (PlatformManager.IsStandalone)
				{
					this.ResourceChoosingPanel.SetActive(true);
				}
				else
				{
					this.payHint.SetActive(true);
				}
				this.UpdateResourceLeftText();
				if (PlatformManager.IsStandalone)
				{
					this.ChooseFood();
				}
			}
			else
			{
				this.resourceChoosen = this.action.ResourceToPay;
				if (!PlatformManager.IsStandalone)
				{
					this.payHint.SetActive(true);
				}
			}
			this.EnablePayingWithCards();
			this.ChangeUnitsColliderState(false);
			this.EnableTemporaryResourcesMechanism();
			base.EnableMapBlackout();
			this.FindAllResources();
			GameController.Instance.hexPointerController.SetHexesWithResource(this.hexesWithResource.ToList<Scythe.BoardPresenter.GameHexPresenter>());
			if (OptionsManager.IsActionAssist())
			{
				if (this.hexesWithResource.Count == 1)
				{
					if (OptionsManager.IsCameraAnimationsActive())
					{
						ShowEnemyMoves.Instance.AnimateCamToHex(this.hexesWithResource.ToList<Scythe.BoardPresenter.GameHexPresenter>()[0].GetWorldPosition());
					}
				}
				else if (this.hexesWithResource.Count > 1)
				{
					List<Vector3> list = new List<Vector3>();
					HashSet<GameHex> hashSet = new HashSet<GameHex>();
					foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesWithResource)
					{
						list.Add(gameHexPresenter.GetWorldPosition());
						hashSet.Add(gameHexPresenter.GetGameHexLogic());
					}
					Vector3 vector = AnimateCamera.Instance.CalculateCenterOfHexes(list);
					float num = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet);
					if (OptionsManager.IsCameraAnimationsActive())
					{
						ShowEnemyMoves.Instance.AnimateCamToShowAllHexes(vector, num);
					}
				}
			}
			if (!PlatformManager.IsStandalone)
			{
				this.SetPayedTilesMobile();
			}
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000BCDB4 File Offset: 0x000BAFB4
		private void SetPayedTilesMobile()
		{
			if (this.action.AnyResource && !this.action.DifferentResources)
			{
				for (int i = 0; i < 4; i++)
				{
					this.mobilePayTiles[i].SetActive(i < (int)this.action.Amount);
					this.mobilePaidIndicators[i].SetActive(false);
					this.mobileResourceIcons[i].sprite = this.resourcePictures[4];
				}
				return;
			}
			if (this.action.DifferentResources)
			{
				this.mobilePayTiles[0].SetActive(true);
				this.mobilePaidIndicators[0].SetActive(false);
				this.mobileResourceIcons[0].sprite = this.resourcePictures[4];
				this.mobilePayTiles[1].SetActive(true);
				this.mobilePaidIndicators[1].SetActive(false);
				this.mobileResourceIcons[1].sprite = this.resourcePictures[5];
				this.mobilePayTiles[2].SetActive(true);
				this.mobilePaidIndicators[2].SetActive(false);
				this.mobileResourceIcons[2].sprite = this.resourcePictures[4];
				this.mobilePayTiles[3].SetActive(false);
				return;
			}
			for (int j = 0; j < 4; j++)
			{
				this.mobilePayTiles[j].SetActive(j < (int)this.action.Amount);
				this.mobilePaidIndicators[j].SetActive(false);
				this.mobileResourceIcons[j].sprite = this.resourcePictures[(int)this.resourceChoosen];
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000BCF28 File Offset: 0x000BB128
		private void UpdatePayedTilesMobile()
		{
			if (this.action.AnyResource && !this.action.DifferentResources)
			{
				for (int i = 0; i < 4; i++)
				{
					this.mobilePaidIndicators[i].SetActive(i < GameController.resourcePayed);
				}
				return;
			}
			if (this.action.DifferentResources)
			{
				this.mobilePaidIndicators[0].SetActive(GameController.resourcePayed >= 1);
				this.mobilePaidIndicators[2].SetActive(GameController.resourcePayed >= 2);
				return;
			}
			for (int j = 0; j < 4; j++)
			{
				this.mobilePaidIndicators[j].SetActive(j < GameController.resourcePayed);
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x00032537 File Offset: 0x00030737
		public bool HaveAction()
		{
			return base.gameObject.activeInHierarchy;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0003BEF0 File Offset: 0x0003A0F0
		public void EnableVisuals(bool enabled)
		{
			this.EnableMarkers(enabled);
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000BCFD0 File Offset: 0x000BB1D0
		private void EnableMarkers(bool enabled)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesWithResource)
			{
				gameHexPresenter.SetFocus(enabled, HexMarkers.MarkerType.PayResource, 0f, false);
				foreach (Scythe.BoardPresenter.GameHexPresenter.HexResource hexResource in gameHexPresenter.resources)
				{
					hexResource.resourceObject.GetComponent<ResourcePresenter>().SetFocus(enabled && hexResource.resourceType == this.action.ResourceToPay);
				}
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000BD06C File Offset: 0x000BB26C
		private void ChangeUnitsColliderState(bool enabled)
		{
			foreach (PlayerUnits playerUnits2 in GameController.Instance.playerUnits)
			{
				if (playerUnits2 != null)
				{
					playerUnits2.SetColliders(enabled);
				}
			}
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000BD0A8 File Offset: 0x000BB2A8
		private void EnableTemporaryResourcesMechanism()
		{
			GameController.GameManager.SetTemporaryResources(this.action.GetPlayer().Resources(false));
			GameController.GameManager.SetTemporaryCombatCardsCount(this.action.GetPlayer().GetCombatCardsCount());
			GameController.GameManager.EnableTemporaryResourcesMechanism(true);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000BD0F8 File Offset: 0x000BB2F8
		private void EnablePayingWithCards()
		{
			if (GameController.GameManager.PlayerCurrent.matFaction.factionPerk == AbilityPerk.Coercion && GameController.GameManager.PlayerCurrent.GetCombatCardsCount() > 0 && this.choosenCombatCard == null)
			{
				GameController.Instance.matFaction.combatCardsPresenter.PayResourceMode();
				GameController.Instance.matFaction.combatCardsPresenter.FocusCards(true);
				this.UpdateTexts();
				OptionsManager.OnLanguageChanged += this.UpdateTexts;
			}
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0003BEF9 File Offset: 0x0003A0F9
		private void DisableTemporaryResourcesMechanism()
		{
			GameController.GameManager.EnableTemporaryResourcesMechanism(false);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000BD178 File Offset: 0x000BB378
		private void FindAllResources()
		{
			Player player = this.action.GetPlayer();
			this.LookForResourceAndAddHexPresenter(player.character.position);
			foreach (Worker worker in player.matPlayer.workers)
			{
				this.LookForResourceAndAddHexPresenter(worker.position);
			}
			foreach (Mech mech in player.matFaction.mechs)
			{
				this.LookForResourceAndAddHexPresenter(mech.position);
			}
			foreach (Building building in player.matPlayer.buildings)
			{
				if (building.position.Owner == player)
				{
					this.LookForResourceAndAddHexPresenter(building.position);
				}
			}
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000BD2A0 File Offset: 0x000BB4A0
		private void LookForResourceAndAddHexPresenter(GameHex hex)
		{
			if ((hex.resources[this.resourceChoosen] > 0 || (hex.GetResourceCount() > 0 && !PlatformManager.IsStandalone)) && hex.hexType != HexType.capital)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = this.GetGameHexPresenter(hex);
				if (!this.hexesWithResource.Contains(gameHexPresenter) && this.CanHexBeUsed(gameHexPresenter, false))
				{
					this.hexesWithResource.Add(gameHexPresenter);
					this.SetResourceHighlight(gameHexPresenter, true);
					if (!PlatformManager.IsStandalone)
					{
						gameHexPresenter.SetFocus(this.CanHexBeUsed(gameHexPresenter, true), HexMarkers.MarkerType.PayResource, 0.3f, false);
					}
				}
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000BD32C File Offset: 0x000BB52C
		private void UpdateResourceLeftText()
		{
			if (PlatformManager.IsStandalone && this.ResourceChoosingPanel.activeInHierarchy)
			{
				this.resourceLeftText.text = ((int)this.action.Amount - GameController.resourcePayed).ToString();
			}
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000BD374 File Offset: 0x000BB574
		private void OnHexSelected(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (this.CanHexBeUsed(hex, true))
			{
				if (PlatformManager.IsStandalone)
				{
					this.OnResourceSelected(hex.GetResourcePresenter(this.resourceChoosen));
					switch (this.resourceChoosen)
					{
					case ResourceType.oil:
						WorldSFXManager.PlaySound(SoundEnum.UseOil, AudioSourceType.WorldSfx);
						break;
					case ResourceType.metal:
						WorldSFXManager.PlaySound(SoundEnum.UseIron, AudioSourceType.WorldSfx);
						break;
					case ResourceType.food:
						WorldSFXManager.PlaySound(SoundEnum.UseGrain, AudioSourceType.WorldSfx);
						break;
					case ResourceType.wood:
						WorldSFXManager.PlaySound(SoundEnum.UseWood, AudioSourceType.WorldSfx);
						break;
					}
					if (GameController.resourcePayed != (int)this.action.Amount)
					{
						hex.SetFocus(this.CanHexBeUsed(hex, true), HexMarkers.MarkerType.PayResource, 0.3f, false);
						this.ChangeResourceType();
						return;
					}
				}
				else
				{
					this.OnHexClickedMobile(hex);
				}
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000BD430 File Offset: 0x000BB630
		private bool CanHexBeUsed(Scythe.BoardPresenter.GameHexPresenter hex, bool fullCheck = true)
		{
			if (fullCheck && !this.hexesWithResource.Contains(hex))
			{
				return false;
			}
			if (this.action.DifferentResources)
			{
				return this.CheckDifferentResourcesCase(hex);
			}
			if (this.action.AnyResource)
			{
				return this.CheckAnyResourceCase(hex);
			}
			return this.CheckOneResourceCase(hex);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000BD484 File Offset: 0x000BB684
		private bool CheckDifferentResourcesCase(Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			return (selectedHex.GetGameHexLogic().resources[this.resourceChoosen] != 0 || !PlatformManager.IsStandalone) && (selectedHex.GetGameHexLogic().GetResourceCount() != 0 || PlatformManager.IsStandalone) && (this.selectedHexes[0] == null || ((!PlatformManager.IsStandalone || this.resourceChoosen != this.selectedResources[0]) && (PlatformManager.IsStandalone || selectedHex.GetGameHexLogic().GetResourceCount() != selectedHex.GetGameHexLogic().resources[this.resourceChoosen])));
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000BD518 File Offset: 0x000BB718
		private bool CheckAnyResourceCase(Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			if (!PlatformManager.IsStandalone)
			{
				return selectedHex.GetGameHexLogic().GetResourceCount() > 0;
			}
			int num = 1;
			for (int i = 0; i < this.selectedHexes.Count; i++)
			{
				if (this.selectedHexes[i] == selectedHex && this.selectedResources[i] == this.resourceChoosen)
				{
					num++;
				}
			}
			return num <= selectedHex.GetGameHexLogic().resources[this.resourceChoosen];
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000BD594 File Offset: 0x000BB794
		private bool CheckOneResourceCase(Scythe.BoardPresenter.GameHexPresenter selectedHex)
		{
			int num = 1;
			using (List<Scythe.BoardPresenter.GameHexPresenter>.Enumerator enumerator = this.selectedHexes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == selectedHex)
					{
						num++;
					}
				}
			}
			return num <= selectedHex.GetGameHexLogic().resources[this.action.ResourceToPay];
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000BD60C File Offset: 0x000BB80C
		private void OnResourceSelected(ResourcePresenter resource)
		{
			if (this.actionPayed)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if (this.selectedHexes[i] == null && this.combatCardIndex != i)
				{
					this.selectedHexes[i] = resource.hex;
					this.selectedResources[i] = resource.resourceType;
					break;
				}
			}
			resource.hex.UpdateTempResource(resource.resourceType, false, false, 0);
			GameController.GameManager.UpdateTemporaryResource(resource.resourceType, -1);
			if (GameController.GameManager.PlayerCurrent.currentMatSection != 4)
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].downActionPresenter.PayResource(GameController.resourcePayed);
			}
			else if (this.action.DifferentResources)
			{
				if (PlatformManager.IsStandalone)
				{
					this.FocusHexes(resource.resourceType, false);
				}
				if (GameController.resourcePayed == 1)
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.PayResource(GameController.resourcePayed + 1);
				}
				else
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.PayResource(GameController.resourcePayed);
				}
			}
			else
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.PayResource(GameController.resourcePayed);
			}
			GameController.resourcePayed++;
			if (PlatformManager.IsStandalone && this.ResourceChoosingPanel.activeInHierarchy)
			{
				this.resourceLeftText.text = ((int)this.action.Amount - GameController.resourcePayed).ToString();
			}
			GameController.Instance.OnUpdatePlayerStats();
			this.CheckActionPayed();
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000BD7F4 File Offset: 0x000BB9F4
		private void FocusHexes(ResourceType type, bool focus)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesWithResource)
			{
				foreach (Scythe.BoardPresenter.GameHexPresenter.HexResource hexResource in gameHexPresenter.resources)
				{
					hexResource.resourceObject.GetComponent<ResourcePresenter>().SetFocus(focus && hexResource.resourceType == type);
					if (PlatformManager.IsMobile)
					{
						hexResource.resourceOutlineMobile.enabled = focus && hexResource.resourceType == type;
						hexResource.resourceOutlineMobile.materials[0].SetColor("_TintColor", Color.red);
					}
					else
					{
						hexResource.resourceOutlineStandalone.enabled = focus && hexResource.resourceType == type;
						hexResource.resourceOutlineStandalone.color = 0;
					}
				}
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000BD8E8 File Offset: 0x000BBAE8
		public void OnCombatCardSelected(CombatCard card)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.selectedHexes[i] == null)
				{
					this.choosenCombatCard = card;
					GameController.Instance.matFaction.combatCardsPresenter.LockCard(card);
					GameController.Instance.matFaction.combatCardsPresenter.FocusCards(false);
					OptionsManager.OnLanguageChanged -= this.UpdateTexts;
					this.combatCardIndex = i;
					break;
				}
			}
			if (GameController.GameManager.PlayerCurrent.currentMatSection != 4)
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].downActionPresenter.PayResource(GameController.resourcePayed);
			}
			else if (this.action.DifferentResources)
			{
				if (GameController.resourcePayed == 1)
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.PayResource(GameController.resourcePayed + 1);
				}
				else
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.PayResource(GameController.resourcePayed);
				}
			}
			else
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.PayResource(GameController.resourcePayed);
			}
			GameController.GameManager.UpdateTemporaryCombatCardsCount(-1);
			GameController.Instance.OnUpdatePlayerStats();
			GameController.resourcePayed++;
			this.CheckActionPayed();
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0003BF06 File Offset: 0x0003A106
		public void OnResourceReturn(int index)
		{
			if (PlatformManager.IsStandalone)
			{
				this.ResourceReturned(index);
				return;
			}
			this.ResourceReturnedMobile(index);
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000BDA7C File Offset: 0x000BBC7C
		private void ResourceReturned(int index)
		{
			if (GameController.resourcePayed == 0)
			{
				return;
			}
			for (int i = 3; i >= 0; i--)
			{
				if (this.selectedHexes[i] != null)
				{
					Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = this.selectedHexes[i];
					this.FocusHexes(this.selectedResources[i], true);
					this.selectedHexes[i].UpdateTempResource(this.selectedResources[i], true, false, 0);
					GameController.GameManager.UpdateTemporaryResource(this.selectedResources[i], 1);
					this.selectedHexes[i] = null;
					this.ChangeResourceType();
					gameHexPresenter.SetFocus(this.CanHexBeUsed(gameHexPresenter, true), HexMarkers.MarkerType.PayResource, 0.3f, false);
					break;
				}
				if (this.combatCardIndex == i)
				{
					GameController.Instance.matFaction.combatCardsPresenter.UnlockAllCards();
					this.choosenCombatCard = null;
					this.combatCardIndex = -1;
					GameController.Instance.matFaction.combatCardsPresenter.FocusCards(true);
					GameController.GameManager.UpdateTemporaryCombatCardsCount(1);
					break;
				}
			}
			GameController.resourcePayed--;
			if (PlatformManager.IsStandalone && this.ResourceChoosingPanel.activeInHierarchy)
			{
				this.resourceLeftText.text = ((int)this.action.Amount - GameController.resourcePayed).ToString();
			}
			if (GameController.GameManager.PlayerCurrent.currentMatSection != 4)
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].downActionPresenter.GetResource(GameController.resourcePayed);
			}
			else if (this.action.DifferentResources)
			{
				if (GameController.resourcePayed == 1)
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.GetResource(GameController.resourcePayed + 1);
				}
				else
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.GetResource(GameController.resourcePayed);
				}
			}
			else
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.GetResource(GameController.resourcePayed);
			}
			if (GameController.GameManager.PlayerCurrent.matFaction.factionPerk == AbilityPerk.Coercion && this.choosenCombatCard == null)
			{
				GameController.Instance.matFaction.combatCardsPresenter.PayResourceMode();
				GameController.Instance.matFaction.combatCardsPresenter.FocusCards(true);
				this.UpdateTexts();
				OptionsManager.OnLanguageChanged += this.UpdateTexts;
			}
			GameController.Instance.OnUpdatePlayerStats();
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000BDD1C File Offset: 0x000BBF1C
		private void UpdateTexts()
		{
			if (PlatformManager.IsStandalone)
			{
				return;
			}
			string text = string.Empty;
			if (this.action.AnyResource || this.action.DifferentResources)
			{
				text = "AnyResource";
			}
			else
			{
				text = this.action.ResourceToPay.ToString();
				text = char.ToUpper(text[0]).ToString() + text.Substring(1);
			}
			GameController.Instance.matFaction.combatCardsPresenter.UpdateResourceForPayWithResourceText(string.Format(ScriptLocalization.Get("GameScene/UseCombatCard"), text));
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0003BF1E File Offset: 0x0003A11E
		public HashSet<Scythe.BoardPresenter.GameHexPresenter> GetAvaliableHexes()
		{
			return this.hexesWithResource;
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x000BDDB8 File Offset: 0x000BBFB8
		public override void OnActionEnded()
		{
			GameController.Instance.matFaction.combatCardsPresenter.PayResourceWithCombatCardeEnded();
			GameController.Instance.hexPointerController.Clear();
			this.ChangeUnitsColliderState(true);
			this.DisableTemporaryResourcesMechanism();
			base.DisableMapBlackout();
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesWithResource)
			{
				this.SetResourceHighlight(gameHexPresenter, false);
			}
			if (GameController.GameManager.PlayerCurrent.matFaction.factionPerk == AbilityPerk.Coercion)
			{
				GameController.Instance.matFaction.combatCardsPresenter.FocusCards(false);
				if (!PlatformManager.IsStandalone)
				{
					GameController.Instance.matFaction.combatCardsPresenter.HideCombatCards();
				}
			}
			GameController.HexGetFocused -= this.OnHexSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			if (PlatformManager.IsStandalone)
			{
				this.ResourceChoosingPanel.SetActive(false);
			}
			else
			{
				this.ResourceChoosingPanelMobile.SetActive(false);
				HumanInputHandler.Instance.endActionController.OnActionFinished();
			}
			HumanInputHandler.Instance.OnInputEnded();
			GameController.ClearFocus();
			GameController.Instance.cameraControler.HooverReset();
			if (this.payHint != null)
			{
				this.payHint.SetActive(false);
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000BDF18 File Offset: 0x000BC118
		private void CheckActionPayed()
		{
			if (GameController.resourcePayed == (int)this.action.Amount)
			{
				foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesWithResource)
				{
					this.SetResourceHighlight(gameHexPresenter, false);
				}
				this.actionPayed = true;
				if (GameController.GameManager.PlayerCurrent.matFaction.factionPerk == AbilityPerk.Coercion)
				{
					GameController.Instance.matFaction.combatCardsPresenter.FocusCards(false);
					GameController.Instance.matFaction.combatCardsPresenter.PayResourceWithCombatCardeEnded();
					OptionsManager.OnLanguageChanged += this.UpdateTexts;
				}
				this.EndPay();
				return;
			}
			GameController.Instance.hexPointerController.SetHexesWithResource(this.hexesWithResource.ToList<Scythe.BoardPresenter.GameHexPresenter>());
			if (!PlatformManager.IsStandalone)
			{
				this.UpdatePayedTilesMobile();
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000BE008 File Offset: 0x000BC208
		public void EndPay()
		{
			List<ResourceBundle> list = new List<ResourceBundle>();
			for (int i = 0; i < (int)this.action.Amount; i++)
			{
				ResourceBundle resourceBundle = default(ResourceBundle);
				resourceBundle.amount = 1;
				if (this.combatCardIndex == i)
				{
					resourceBundle.gameHex = GameController.GameManager.PlayerCurrent.character.position;
					resourceBundle.resourceType = ResourceType.combatCard;
				}
				else
				{
					resourceBundle.gameHex = this.selectedHexes[i].GetGameHexLogic();
					if (this.action.AnyResource)
					{
						resourceBundle.resourceType = this.selectedResources[i];
					}
					else
					{
						resourceBundle.resourceType = this.action.ResourceToPay;
					}
				}
				list.Add(resourceBundle);
			}
			if (!this.action.SetResources(list, this.choosenCombatCard))
			{
				Debug.LogError("PayDownAction error on action.SetResource");
			}
			if (GameController.GameManager.PlayerCurrent.currentMatSection != 4)
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].downActionPresenter.OnPayResourceEnd();
			}
			else
			{
				GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.OnPayResourceEnd();
			}
			if (PayResourcePresenter.PayResourceEnded != null)
			{
				PayResourcePresenter.PayResourceEnded();
			}
			this.OnActionEnded();
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0003BF26 File Offset: 0x0003A126
		private Scythe.BoardPresenter.GameHexPresenter GetGameHexPresenter(GameHex hex)
		{
			return GameController.Instance.gameBoardPresenter.GetGameHexPresenter(hex);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000BE168 File Offset: 0x000BC368
		private void SetResourceHighlight(Scythe.BoardPresenter.GameHexPresenter hex, bool isHighlight)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter.HexResource hexResource in hex.resources)
			{
				if ((!this.action.AnyResource && !this.action.DifferentResources && hexResource.resourceType == this.action.ResourceToPay) || ((this.action.DifferentResources || this.action.AnyResource) && this.resourceChoosen == hexResource.resourceType && PlatformManager.IsStandalone) || !isHighlight)
				{
					hexResource.resourceObject.GetComponent<ResourcePresenter>().SetFocus(isHighlight);
					hex.SetFocus(isHighlight, HexMarkers.MarkerType.PayResource, 0.3f, false);
				}
			}
			if (!isHighlight)
			{
				hex.SetFocus(isHighlight, HexMarkers.MarkerType.PayResource, 0.3f, false);
			}
			GameController.Instance.gameBoardPresenter.RevertResourcesLabels(hex);
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0003BF38 File Offset: 0x0003A138
		private void ResourceSelected(ResourceType resource)
		{
			if (PlatformManager.IsStandalone)
			{
				this.resourceChoosen = resource;
				this.ResetButtons();
				this.GetResourceButton(resource).image.color = this.choosenColor;
				this.GetResourcehighlight(resource).SetActive(true);
				this.ChangeResourceType();
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0003BF78 File Offset: 0x0003A178
		private Button GetResourceButton(ResourceType resourceType)
		{
			switch (resourceType)
			{
			case ResourceType.oil:
				return this.gainOilButton;
			case ResourceType.metal:
				return this.gainMetalButton;
			case ResourceType.food:
				return this.gainFoodButton;
			case ResourceType.wood:
				return this.gainWoodButton;
			default:
				return null;
			}
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0003BFAF File Offset: 0x0003A1AF
		private GameObject GetResourcehighlight(ResourceType resourceType)
		{
			switch (resourceType)
			{
			case ResourceType.oil:
				return this.highlightOil;
			case ResourceType.metal:
				return this.highlightMetal;
			case ResourceType.food:
				return this.highlightFood;
			case ResourceType.wood:
				return this.highlightWood;
			default:
				return null;
			}
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0003BFE6 File Offset: 0x0003A1E6
		public void ChooseFood()
		{
			if (PlatformManager.IsStandalone)
			{
				this.ResourceSelected(ResourceType.food);
				return;
			}
			this.ResourceSelectedMobile(ResourceType.food);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0003BFFE File Offset: 0x0003A1FE
		public void ChooseMetal()
		{
			if (PlatformManager.IsStandalone)
			{
				this.ResourceSelected(ResourceType.metal);
				return;
			}
			this.ResourceSelectedMobile(ResourceType.metal);
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0003C016 File Offset: 0x0003A216
		public void ChooseOil()
		{
			if (PlatformManager.IsStandalone)
			{
				this.ResourceSelected(ResourceType.oil);
				return;
			}
			this.ResourceSelectedMobile(ResourceType.oil);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0003C02E File Offset: 0x0003A22E
		public void ChooseWood()
		{
			if (PlatformManager.IsStandalone)
			{
				this.ResourceSelected(ResourceType.wood);
				return;
			}
			this.ResourceSelectedMobile(ResourceType.wood);
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000BE234 File Offset: 0x000BC434
		private void ResetButtons()
		{
			if (PlatformManager.IsStandalone)
			{
				this.gainFoodButton.image.color = this.normalColor;
				this.gainOilButton.image.color = this.normalColor;
				this.gainWoodButton.image.color = this.normalColor;
				this.gainMetalButton.image.color = this.normalColor;
				this.highlightFood.SetActive(false);
				this.highlightMetal.SetActive(false);
				this.highlightOil.SetActive(false);
				this.highlightWood.SetActive(false);
			}
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000BE2D4 File Offset: 0x000BC4D4
		private void ChangeResourceType()
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesWithResource)
			{
				this.SetResourceHighlight(gameHexPresenter, false);
			}
			GameController.Instance.hexPointerController.Clear();
			this.hexesWithResource.Clear();
			this.FindAllResources();
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000BE2D4 File Offset: 0x000BC4D4
		private void UpdateHexesMobile()
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.hexesWithResource)
			{
				this.SetResourceHighlight(gameHexPresenter, false);
			}
			GameController.Instance.hexPointerController.Clear();
			this.hexesWithResource.Clear();
			this.FindAllResources();
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000BE348 File Offset: 0x000BC548
		private void UpdateResourcePanelMobile(Scythe.BoardPresenter.GameHexPresenter hexPresenter)
		{
			if (!this.action.DifferentResources && !this.action.AnyResource)
			{
				return;
			}
			if (hexPresenter.GetGameHexLogic().GetResourceCount() == 0 || this.TemporaryResourceCount(hexPresenter) == 0)
			{
				this.ResourceChoosingPanelMobile.SetActive(false);
				this.selectedHex.SetFocus(false, HexMarkers.MarkerType.PayResource, 0f, false);
				return;
			}
			this.selectedHex.SetFocus(true, HexMarkers.MarkerType.PayResource, 0f, false);
			this.ResourceChoosingPanelMobile.SetActive(true);
			this.UpdateResourceButtonMobile(ResourceType.food);
			this.UpdateResourceButtonMobile(ResourceType.oil);
			this.UpdateResourceButtonMobile(ResourceType.wood);
			this.UpdateResourceButtonMobile(ResourceType.metal);
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0003C046 File Offset: 0x0003A246
		private int TemporaryResourceCount(Scythe.BoardPresenter.GameHexPresenter hexPresenter)
		{
			return 0 + hexPresenter.GetResourcePresenter(ResourceType.oil).GetTemporaryResourceValue() + hexPresenter.GetResourcePresenter(ResourceType.food).GetTemporaryResourceValue() + hexPresenter.GetResourcePresenter(ResourceType.wood).GetTemporaryResourceValue() + hexPresenter.GetResourcePresenter(ResourceType.metal).GetTemporaryResourceValue();
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000BE3E4 File Offset: 0x000BC5E4
		private void UpdateResourceButtonMobile(ResourceType resource)
		{
			Button resourceButton = this.GetResourceButton(resource);
			if (this.selectedHex.GetResourcePresenter(resource).GetTemporaryResourceValue() == 0)
			{
				resourceButton.gameObject.SetActive(false);
				return;
			}
			resourceButton.gameObject.SetActive(true);
			if (this.action.DifferentResources && this.selectedHexes[0] != null && resource == this.selectedResources[0])
			{
				resourceButton.interactable = false;
			}
			else
			{
				resourceButton.interactable = true;
			}
			resourceButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = this.selectedHex.GetResourcePresenter(resource).GetTemporaryResourceValue().ToString();
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000BE48C File Offset: 0x000BC68C
		private void ResourceSelectedMobile(ResourceType resource)
		{
			this.resourceChoosen = resource;
			this.OnResourceSelected(this.selectedHex.GetResourcePresenter(resource));
			switch (resource)
			{
			case ResourceType.oil:
				WorldSFXManager.PlaySound(SoundEnum.UseOil, AudioSourceType.WorldSfx);
				break;
			case ResourceType.metal:
				WorldSFXManager.PlaySound(SoundEnum.UseIron, AudioSourceType.WorldSfx);
				break;
			case ResourceType.food:
				WorldSFXManager.PlaySound(SoundEnum.UseGrain, AudioSourceType.WorldSfx);
				break;
			case ResourceType.wood:
				WorldSFXManager.PlaySound(SoundEnum.UseWood, AudioSourceType.WorldSfx);
				break;
			}
			if (GameController.resourcePayed != (int)this.action.Amount)
			{
				this.UpdateHexesMobile();
				this.UpdateResourcePanelMobile(this.selectedHex);
			}
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000BE520 File Offset: 0x000BC720
		private void OnHexClickedMobile(Scythe.BoardPresenter.GameHexPresenter hexPresenter)
		{
			if (this.hexesWithResource.Contains(this.selectedHex))
			{
				this.selectedHex.SetFocus(true, HexMarkers.MarkerType.PayResource, 0f, false);
			}
			this.selectedHex = hexPresenter;
			if (!this.action.DifferentResources && !this.action.AnyResource)
			{
				this.ResourceSelectedMobile(this.action.ResourceToPay);
				return;
			}
			GameHex gameHexLogic = hexPresenter.GetGameHexLogic();
			if (gameHexLogic.OneResourceAvaliable())
			{
				this.ResourceSelectedMobile(gameHexLogic.GetFirstAvaliableResource());
				return;
			}
			this.UpdateResourcePanelMobile(hexPresenter);
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000BE5AC File Offset: 0x000BC7AC
		private void ResourceReturnedMobile(int index)
		{
			if (this.selectedHexes[index] == null && this.combatCardIndex != index)
			{
				return;
			}
			if (this.selectedHexes[index] != null)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = this.selectedHexes[index];
				this.FocusHexes(this.selectedResources[index], true);
				this.selectedHexes[index].UpdateTempResource(this.selectedResources[index], true, false, 0);
				GameController.GameManager.UpdateTemporaryResource(this.selectedResources[index], 1);
				this.selectedHexes.RemoveAt(index);
				this.selectedHexes.Add(null);
				this.ChangeResourceType();
				gameHexPresenter.SetFocus(this.CanHexBeUsed(gameHexPresenter, true), HexMarkers.MarkerType.PayResource, 0.3f, false);
				this.UpdateResourcePanelMobile(gameHexPresenter);
				if (this.combatCardIndex > index)
				{
					this.combatCardIndex--;
				}
			}
			else if (this.combatCardIndex == index)
			{
				GameController.Instance.matFaction.combatCardsPresenter.UnlockAllCards();
				this.choosenCombatCard = null;
				this.combatCardIndex = -1;
				GameController.Instance.matFaction.combatCardsPresenter.FocusCards(true);
				GameController.GameManager.UpdateTemporaryCombatCardsCount(1);
				this.selectedHexes.RemoveAt(index);
				this.selectedHexes.Add(null);
			}
			GameController.resourcePayed--;
			if (GameController.GameManager.PlayerCurrent.currentMatSection == 4)
			{
				if (this.action.DifferentResources)
				{
					if (GameController.resourcePayed == 1)
					{
						GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.GetResource(GameController.resourcePayed + 1);
					}
					else
					{
						GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.GetResource(GameController.resourcePayed);
					}
				}
				else
				{
					GameController.Instance.matPlayer.matSection[GameController.GameManager.PlayerCurrent.currentMatSection].topActionPresenter.GetResource(GameController.resourcePayed);
				}
			}
			this.EnablePayingWithCards();
			GameController.Instance.OnUpdatePlayerStats();
			this.UpdatePayedTilesMobile();
		}

		// Token: 0x040015BB RID: 5563
		public GameObject payHint;

		// Token: 0x040015BC RID: 5564
		public GameObject ResourceChoosingPanel;

		// Token: 0x040015BD RID: 5565
		public GameObject ResourceChoosingPanelMobile;

		// Token: 0x040015BE RID: 5566
		public Button gainFoodButton;

		// Token: 0x040015BF RID: 5567
		public Button gainMetalButton;

		// Token: 0x040015C0 RID: 5568
		public Button gainOilButton;

		// Token: 0x040015C1 RID: 5569
		public Button gainWoodButton;

		// Token: 0x040015C2 RID: 5570
		public GameObject highlightFood;

		// Token: 0x040015C3 RID: 5571
		public GameObject highlightMetal;

		// Token: 0x040015C4 RID: 5572
		public GameObject highlightOil;

		// Token: 0x040015C5 RID: 5573
		public GameObject highlightWood;

		// Token: 0x040015C6 RID: 5574
		public GameObject[] mobilePayTiles;

		// Token: 0x040015C7 RID: 5575
		public Image[] mobileResourceIcons;

		// Token: 0x040015C8 RID: 5576
		public GameObject[] mobilePaidIndicators;

		// Token: 0x040015C9 RID: 5577
		public Sprite[] resourcePictures;

		// Token: 0x040015CA RID: 5578
		public Color normalColor;

		// Token: 0x040015CB RID: 5579
		public Color choosenColor;

		// Token: 0x040015CC RID: 5580
		public Text resourceLeftText;

		// Token: 0x040015CD RID: 5581
		public TextMeshProUGUI resourceLeftTextMobile;

		// Token: 0x040015CE RID: 5582
		private PayResource action;

		// Token: 0x040015CF RID: 5583
		private bool actionPayed;

		// Token: 0x040015D0 RID: 5584
		private HashSet<Scythe.BoardPresenter.GameHexPresenter> hexesWithResource = new HashSet<Scythe.BoardPresenter.GameHexPresenter>();

		// Token: 0x040015D1 RID: 5585
		private List<Scythe.BoardPresenter.GameHexPresenter> selectedHexes = new List<Scythe.BoardPresenter.GameHexPresenter>(4) { null, null, null, null };

		// Token: 0x040015D2 RID: 5586
		private Scythe.BoardPresenter.GameHexPresenter selectedHex;

		// Token: 0x040015D3 RID: 5587
		private ResourceType[] selectedResources = new ResourceType[4];

		// Token: 0x040015D4 RID: 5588
		private ResourceType resourceChoosen;

		// Token: 0x040015D5 RID: 5589
		private CombatCard choosenCombatCard;

		// Token: 0x040015D6 RID: 5590
		private int combatCardIndex = -1;

		// Token: 0x020003F8 RID: 1016
		// (Invoke) Token: 0x06001EF1 RID: 7921
		public delegate void PayResourceStart();

		// Token: 0x020003F9 RID: 1017
		// (Invoke) Token: 0x06001EF5 RID: 7925
		public delegate void PayResourceEnd();
	}
}
