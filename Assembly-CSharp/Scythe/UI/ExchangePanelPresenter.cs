using System;
using System.Collections.Generic;
using DG.Tweening;
using HoneyFramework;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003E7 RID: 999
	public class ExchangePanelPresenter : MonoBehaviour
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x0003B43B File Offset: 0x0003963B
		// (set) Token: 0x06001DAE RID: 7598 RVA: 0x0003B475 File Offset: 0x00039675
		private ExchangePanel Context
		{
			get
			{
				if (this.context == null)
				{
					this.context = new ExchangePanel();
					if (this.exchangePanelAlternative != null)
					{
						this.exchangePanelAlternative.ShareContext(this.Context);
					}
				}
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06001DAF RID: 7599 RVA: 0x000B78C4 File Offset: 0x000B5AC4
		// (remove) Token: 0x06001DB0 RID: 7600 RVA: 0x000B78F8 File Offset: 0x000B5AF8
		public static event ExchangePanelPresenter.WorkerButtonClicked OnWorkerButtonClicked;

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06001DB1 RID: 7601 RVA: 0x000B792C File Offset: 0x000B5B2C
		// (remove) Token: 0x06001DB2 RID: 7602 RVA: 0x000B7960 File Offset: 0x000B5B60
		public static event ExchangePanelPresenter.WorkerLoaded OnWorkerLoaded;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06001DB3 RID: 7603 RVA: 0x000B7994 File Offset: 0x000B5B94
		// (remove) Token: 0x06001DB4 RID: 7604 RVA: 0x000B79C8 File Offset: 0x000B5BC8
		public static event ExchangePanelPresenter.ResourceLoaded OnResourceLoaded;

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x0003B47E File Offset: 0x0003967E
		public DragAndDropPanel DragAndDropBar
		{
			get
			{
				return GameController.Instance.dragAndDropPanel;
			}
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0003B48A File Offset: 0x0003968A
		private void Awake()
		{
			this.InitValues();
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0003B492 File Offset: 0x00039692
		private void OnEnable()
		{
			if (this.alternative)
			{
				CameraControler.CameraMovementBlocked = true;
			}
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0003B4A2 File Offset: 0x000396A2
		private void OnDisable()
		{
			if (this.alternative)
			{
				CameraControler.CameraMovementBlocked = false;
			}
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0003B4B2 File Offset: 0x000396B2
		public void ShareContext(ExchangePanel context)
		{
			this.Context = context;
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x000B79FC File Offset: 0x000B5BFC
		public void SetPanelParameters(Unit unit, bool battleMode = false, SelectMethod selectMethod = SelectMethod.Default)
		{
			this.restoreData = false;
			if (GameController.Instance.DragAndDrop)
			{
				this.restoreData = this.DragAndDropBar.OnUnitChanged(unit, selectMethod) || selectMethod == SelectMethod.LoadingWorker;
			}
			if (this.Context.containerUnit == unit)
			{
				this.restoreData = false;
				return;
			}
			if (!GameController.Instance.DragAndDrop)
			{
				this.PassAllToField(true);
				this.UnloadAllWorkersFromMech();
			}
			this.Context.battleMode = battleMode;
			this.UpdateUnitInfo(unit, false);
			this.UpdateHexWorkers(unit.position);
			if (this.restoreData)
			{
				this.RestoreData(this.Context.previousStateOfSliders);
			}
			this.InitImages();
			this.SetInteractable(true);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x000B7AB0 File Offset: 0x000B5CB0
		public void RecoverSharedContext()
		{
			this.restoreData = true;
			this.UpdateUnitInfo(this.Context.containerUnit, false);
			this.UpdateHexWorkers(this.Context.field);
			if (this.restoreData)
			{
				this.RestoreData(this.Context.previousStateOfSliders);
			}
			this.restoreData = false;
			this.UpdateActiveWorkersText();
			this.UpdateInactiveWorkersText();
			this.InitImages();
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x000B7B1C File Offset: 0x000B5D1C
		public void UpdateHexWorkers(GameHex hex)
		{
			if (this.Context != null)
			{
				if (this.Context.battleMode)
				{
					if (!GameController.GameManager.combatManager.AttackerIsWinner())
					{
						this.Context.HexWorkers = new List<Unit>(hex.GetEnemyWorkers().ToArray());
					}
					else
					{
						this.Context.HexWorkers = new List<Unit>(hex.GetOwnerWorkers().ToArray());
					}
				}
				else
				{
					this.Context.HexWorkers = new List<Unit>(hex.GetOwnerWorkers().ToArray());
				}
				foreach (Unit unit in this.Context.LoadedWorkers)
				{
					this.Context.HexWorkers.Remove(unit);
				}
				this.UpdateWorkersButtons();
				this.UpdateActiveWorkersText();
				this.UpdateInactiveWorkersText();
			}
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x000B7C10 File Offset: 0x000B5E10
		private void InitImages()
		{
			if (this.unitImage != null)
			{
				if (this.Context.containerUnit is Worker)
				{
					this.unitImage.sprite = this.unitSprites[0];
					return;
				}
				if (this.Context.containerUnit is Mech)
				{
					this.unitImage.sprite = this.unitSprites[1];
					return;
				}
				this.unitImage.sprite = this.unitSprites[2];
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x000B7C8C File Offset: 0x000B5E8C
		private void InitValues()
		{
			if (this.Context == null)
			{
				this.Context = new ExchangePanel();
				if (this.exchangePanelAlternative != null)
				{
					this.exchangePanelAlternative.ShareContext(this.Context);
				}
			}
			if (this.exchangePanels != null)
			{
				return;
			}
			this.exchangePanels = new Dictionary<ResourceType, ExchangeSlot>();
			if (this.canvasGroup == null)
			{
				this.canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
			}
			Transform transform = base.transform.Find("Interactive");
			if (transform == null)
			{
				transform = base.transform.Find("Background/Interactive");
			}
			if (transform != null)
			{
				if (this.Context.battleMode)
				{
					transform.gameObject.SetActive(false);
					return;
				}
				transform.gameObject.SetActive(true);
				this.InitExchangePanel(this.FoodExchange, ResourceType.food);
				this.InitExchangePanel(this.MetalExchange, ResourceType.metal);
				this.InitExchangePanel(this.OilExchange, ResourceType.oil);
				this.InitExchangePanel(this.WoodExchange, ResourceType.wood);
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000B7D8C File Offset: 0x000B5F8C
		private void InitExchangePanel(GameObject exchangePanel, ResourceType resourceType)
		{
			if (exchangePanel != null)
			{
				Transform transform = exchangePanel.transform.Find("TextField");
				Transform transform2 = exchangePanel.transform.Find("TextUnit");
				Transform transform3 = exchangePanel.transform.Find("Slider");
				ExchangeSlot exchangeSlot = new ExchangeSlot(new SliderWrapper((transform3 != null) ? transform3.GetComponent<Slider>() : null));
				exchangeSlot.buttons = exchangePanel.GetComponentsInChildren<Button>();
				if (transform != null)
				{
					exchangeSlot.textField = transform.GetComponent<Text>();
				}
				if (transform2 != null)
				{
					exchangeSlot.textUnit = transform2.GetComponent<Text>();
				}
				this.SetSlider(exchangeSlot, 0, 0);
				exchangeSlot.SetSliderValueChangedEvent(delegate
				{
					this.OnSliderChanged((int)resourceType);
				});
				this.exchangePanels.Add(resourceType, exchangeSlot);
			}
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x000B7E74 File Offset: 0x000B6074
		private void SetSlider(ExchangeSlot exchangePanel, int unitValue, int fieldValue)
		{
			exchangePanel.MaxValue = unitValue + fieldValue;
			exchangePanel.Value = unitValue;
			if (exchangePanel.textField != null)
			{
				exchangePanel.textField.text = fieldValue.ToString();
			}
			if (exchangePanel.textUnit != null)
			{
				exchangePanel.textUnit.text = unitValue.ToString();
			}
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0003B4BB File Offset: 0x000396BB
		public void UpdateUnitInfo(Unit unit, bool selectedOnResourceDrop = false)
		{
			this.Context.containerUnit = unit;
			this.UpdateResourceInfo();
			this.UpdateActiveWorkersText();
			this.UpdateInactiveWorkersText();
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000B7ED4 File Offset: 0x000B60D4
		public void UpdateResourceInfo()
		{
			if (this.Context.battleMode)
			{
				return;
			}
			this.Context.field = this.Context.containerUnit.position;
			this.UpdateValues(ResourceType.oil);
			this.UpdateValues(ResourceType.metal);
			this.UpdateValues(ResourceType.food);
			this.UpdateValues(ResourceType.wood);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0003B4DB File Offset: 0x000396DB
		public void SetInteractable(bool interactable)
		{
			this.canvasGroup.interactable = interactable;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000B7F28 File Offset: 0x000B6128
		public void RestoreData(Dictionary<ResourceType, int> previouslyLoadedResources)
		{
			if (previouslyLoadedResources[ResourceType.oil] != 0)
			{
				this.OnResourceChanged(0, true, false, previouslyLoadedResources[ResourceType.oil]);
			}
			if (previouslyLoadedResources[ResourceType.metal] != 0)
			{
				this.OnResourceChanged(1, true, false, previouslyLoadedResources[ResourceType.metal]);
			}
			if (previouslyLoadedResources[ResourceType.food] != 0)
			{
				this.OnResourceChanged(2, true, false, previouslyLoadedResources[ResourceType.food]);
			}
			if (previouslyLoadedResources[ResourceType.wood] != 0)
			{
				this.OnResourceChanged(3, true, false, previouslyLoadedResources[ResourceType.wood]);
			}
			this.restoreData = false;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0003B4E9 File Offset: 0x000396E9
		public ExchangePanel GetContext()
		{
			return this.Context;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0003B4F1 File Offset: 0x000396F1
		private void UpdateValues(ResourceType resource)
		{
			this.UpdateSliderValuesForResource(resource);
			this.UpdatePanelHexResourceText(resource);
			this.UpdatePanelUnitResourceText(resource);
			this.UpdateButtonsInteractableForResource(resource);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000B7FA0 File Offset: 0x000B61A0
		private void UpdateSliderValuesForResource(ResourceType resource)
		{
			this.exchangePanels[resource].MaxValue = this.Context.containerUnit.resources[resource] + this.Context.field.resources[resource];
			this.exchangePanels[resource].Value = this.Context.containerUnit.resources[resource];
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000B8014 File Offset: 0x000B6214
		private void UpdatePanelHexResourceText(ResourceType resource)
		{
			if (this.Context.containerUnit == null || this.Context.field == null)
			{
				return;
			}
			if (this.exchangePanels[resource].textField != null)
			{
				this.exchangePanels[resource].textField.text = (this.Context.containerUnit.resources[resource] + this.Context.field.resources[resource] - this.exchangePanels[resource].Value).ToString();
			}
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000B80B4 File Offset: 0x000B62B4
		private void UpdatePanelUnitResourceText(ResourceType resource)
		{
			if (this.exchangePanels[resource].textUnit != null)
			{
				this.exchangePanels[resource].textUnit.text = this.exchangePanels[resource].Value.ToString();
			}
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000B810C File Offset: 0x000B630C
		private void UpdateButtonsInteractableForResource(ResourceType resource)
		{
			bool flag = this.Context.containerUnit.resources[resource] + this.Context.field.resources[resource] > 0;
			this.exchangePanels[resource].SetSliderInteractable(flag);
			Button[] buttons = this.exchangePanels[resource].buttons;
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].interactable = flag;
			}
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000B8188 File Offset: 0x000B6388
		private void UpdateButtonsInteractableForActiveWorkers(bool activeWorkersInteractivity)
		{
			if (this.ActiveWorkersExchange == null)
			{
				return;
			}
			Button[] componentsInChildren = this.ActiveWorkersExchange.GetComponentsInChildren<Button>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].interactable = activeWorkersInteractivity;
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000B81C8 File Offset: 0x000B63C8
		private void UpdateButtonsInteractableForInactiveWorkers(bool inactiveWorkersInteractivity)
		{
			if (this.InactiveWorkersExchange == null)
			{
				return;
			}
			Button[] componentsInChildren = this.InactiveWorkersExchange.GetComponentsInChildren<Button>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].interactable = inactiveWorkersInteractivity;
			}
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000B8208 File Offset: 0x000B6408
		public void OnSliderChanged(int resourceInt)
		{
			if (Enum.IsDefined(typeof(ResourceType), (ResourceType)resourceInt))
			{
				this.UpdatePanelUnitResourceText((ResourceType)resourceInt);
				this.UpdatePanelHexResourceText((ResourceType)resourceInt);
				if (!GameController.Instance.DragAndDrop || (GameController.Instance.DragAndDrop && this.DragAndDropBar.PreviousUnitEqualsUnit(this.Context.containerUnit)))
				{
					if (!this.restoreData)
					{
						this.MoveResources((ResourceType)resourceInt, this.exchangePanels[(ResourceType)resourceInt].Value - this.Context.previousStateOfSliders[(ResourceType)resourceInt]);
					}
					if (!this.restoreData && GameController.Instance.DragAndDrop)
					{
						this.DragAndDropBar.SetResourceAmount((ResourceType)resourceInt, this.exchangePanels[(ResourceType)resourceInt].Value);
					}
				}
				if (!this.restoreData && ((GameController.Instance.DragAndDrop && this.DragAndDropBar.PreviousUnitEqualsUnit(this.Context.containerUnit)) || !GameController.Instance.DragAndDrop))
				{
					this.Context.previousStateOfSliders[(ResourceType)resourceInt] = this.exchangePanels[(ResourceType)resourceInt].Value;
				}
			}
			if (ExchangePanelPresenter.OnResourceLoaded != null)
			{
				ExchangePanelPresenter.OnResourceLoaded();
			}
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x000B8340 File Offset: 0x000B6540
		private void MoveResources(ResourceType resource, int countOfMovedResource)
		{
			if (this.loadedOnDrop)
			{
				return;
			}
			Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(this.Context.containerUnit.position);
			if (countOfMovedResource < 0)
			{
				gameHexPresenter.ExchangeShowResourceMove(resource, GameController.GetUnitPresenter(this.Context.containerUnit).gameObject.transform.position, true, Mathf.Abs(countOfMovedResource), false);
				return;
			}
			if (countOfMovedResource > 0)
			{
				gameHexPresenter.ExchangeShowResourceMove(resource, GameController.GetUnitPresenter(this.Context.containerUnit).gameObject.transform.position, false, Mathf.Abs(countOfMovedResource), false);
			}
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x000B83DC File Offset: 0x000B65DC
		public void PassOneResourceToUnitMaxUnload(int resource)
		{
			WorldSFXManager.PlaySound(SoundEnum.TransportLoad, AudioSourceType.Buttons);
			if (this.exchangePanels[(ResourceType)resource].Value == this.exchangePanels[(ResourceType)resource].MaxValue)
			{
				this.PassAllResourcesToField(resource);
				return;
			}
			this.OnResourceChanged(resource, true, false, 1);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0003B50F File Offset: 0x0003970F
		public void PassOneResourceToUnit(int resource)
		{
			WorldSFXManager.PlaySound(SoundEnum.TransportLoad, AudioSourceType.Buttons);
			this.OnResourceChanged(resource, true, false, 1);
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x0003B526 File Offset: 0x00039726
		public void PassOneResourceToUnitOnDrop(int resource)
		{
			WorldSFXManager.PlaySound(SoundEnum.TransportLoad, AudioSourceType.Buttons);
			this.loadedOnDrop = true;
			this.OnResourceChanged(resource, true, false, 1);
			this.loadedOnDrop = false;
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x0003B54B File Offset: 0x0003974B
		public void PassOneResourceToField(int resource)
		{
			WorldSFXManager.PlaySound(SoundEnum.TransportUnload, AudioSourceType.Buttons);
			this.OnResourceChanged(resource, false, false, 1);
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0003B562 File Offset: 0x00039762
		public void PassAllResourcesToUnit(int resource)
		{
			WorldSFXManager.PlaySound(SoundEnum.TransportLoad, AudioSourceType.Buttons);
			this.OnResourceChanged(resource, true, true, 1);
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0003B579 File Offset: 0x00039779
		public void PassAllResourcesToField(int resource)
		{
			WorldSFXManager.PlaySound(SoundEnum.TransportUnload, AudioSourceType.Buttons);
			this.OnResourceChanged(resource, false, true, 1);
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x0003B590 File Offset: 0x00039790
		public void PassAllResourcesToField(int resource, bool withoutSound = false)
		{
			if (!withoutSound)
			{
				WorldSFXManager.PlaySound(SoundEnum.TransportUnload, AudioSourceType.Buttons);
			}
			this.OnResourceChanged(resource, false, true, 1);
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000B842C File Offset: 0x000B662C
		public void PassAllToUnit()
		{
			this.FinishResourceMoveAnimation();
			for (int i = 0; i < 4; i++)
			{
				this.PassAllResourcesToUnit(i);
			}
			if (!PlatformManager.IsStandalone && this.Context.containerUnit is Mech)
			{
				this.LoadAllWorkersToMech();
			}
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000B8474 File Offset: 0x000B6674
		public void PassAllToField()
		{
			this.FinishResourceMoveAnimation();
			for (int i = 0; i < 4; i++)
			{
				this.PassAllResourcesToField(i);
			}
			if (!PlatformManager.IsStandalone && this.Context.containerUnit is Mech)
			{
				this.UnloadAllWorkersFromMech();
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x000B84BC File Offset: 0x000B66BC
		public void PassAllToField(bool withoutSound = false)
		{
			this.FinishResourceMoveAnimation();
			for (int i = 0; i < 4; i++)
			{
				this.PassAllResourcesToField(i, withoutSound);
			}
			if (!PlatformManager.IsStandalone && this.Context.containerUnit is Mech)
			{
				this.UnloadAllWorkersFromMech();
			}
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0003B5AA File Offset: 0x000397AA
		private void FinishResourceMoveAnimation()
		{
			if (this.Context.field != null)
			{
				GameController.Instance.gameBoardPresenter.GetGameHexPresenter(this.Context.field).ForceResourceModelToFinishMove();
			}
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000B8504 File Offset: 0x000B6704
		private void OnResourceChanged(int resource, bool toUnit, bool exchangeAll, int multiplier = 1)
		{
			if (this.Context.containerUnit == null || this.Context.battleMode)
			{
				return;
			}
			GameController.GetUnitPresenter(this.Context.containerUnit).ForceFinishMoveAnimation();
			if (Enum.IsDefined(typeof(ResourceType), resource))
			{
				int num = this.Context.containerUnit.resources[(ResourceType)resource] + this.Context.field.resources[(ResourceType)resource];
				if (exchangeAll)
				{
					if (toUnit)
					{
						this.exchangePanels[(ResourceType)resource].Value = num;
					}
					else
					{
						this.exchangePanels[(ResourceType)resource].Value = 0;
					}
				}
				else if (toUnit)
				{
					if (this.exchangePanels[(ResourceType)resource].Value != num)
					{
						this.exchangePanels[(ResourceType)resource].Value += multiplier;
					}
				}
				else if (this.exchangePanels[(ResourceType)resource].Value != 0)
				{
					this.exchangePanels[(ResourceType)resource].Value -= multiplier;
				}
				if (ExchangePanelPresenter.OnResourceLoaded != null)
				{
					ExchangePanelPresenter.OnResourceLoaded();
				}
			}
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0003B5D8 File Offset: 0x000397D8
		public void OnLoadWorkerClicked(int workerId)
		{
			DOTween.CompleteAll(true);
			this.LoadWorkerToMech(workerId);
			WorldSFXManager.PlaySound(SoundEnum.MechLoad, AudioSourceType.WorldSfx);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0003B5F0 File Offset: 0x000397F0
		public void OnUnloadWorkerClicked(int workerId)
		{
			DOTween.CompleteAll(true);
			this.UnloadWorkerFromMech(workerId);
			WorldSFXManager.PlaySound(SoundEnum.MechUnload, AudioSourceType.WorldSfx);
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000B862C File Offset: 0x000B682C
		public void LoadWorkerToMech(int workerId)
		{
			WorldSFXManager.PlaySound(SoundEnum.MechLoad, AudioSourceType.WorldSfx);
			if (!(this.Context.containerUnit is Mech))
			{
				return;
			}
			if (workerId < this.Context.HexWorkers.Count)
			{
				GameController.GetUnitPresenter(this.Context.containerUnit as Mech).LoadWorker(this.Context.HexWorkers[workerId], false);
				this.Context.HexWorkers.RemoveAt(workerId);
				this.UpdateWorkersButtons();
				if (ExchangePanelPresenter.OnWorkerLoaded != null)
				{
					ExchangePanelPresenter.OnWorkerLoaded(this.Context.LoadedWorkers[this.Context.LoadedWorkers.Count - 1]);
				}
			}
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000B86E0 File Offset: 0x000B68E0
		public void LoadFirstWorkerWithMove()
		{
			int num = -1;
			for (int i = 0; i < this.Context.HexWorkers.Count; i++)
			{
				if (this.Context.HexWorkers[i].NotMoved())
				{
					num = i;
				}
			}
			if (num > -1)
			{
				this.LoadWorkerToMech(num);
				this.UpdateActiveWorkersText();
			}
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000B8738 File Offset: 0x000B6938
		public void LoadFirstWorkerWithoutMove()
		{
			int num = -1;
			for (int i = 0; i < this.Context.HexWorkers.Count; i++)
			{
				if (!this.Context.HexWorkers[i].NotMoved())
				{
					num = i;
				}
			}
			if (num > -1)
			{
				this.LoadWorkerToMech(num);
				this.UpdateInactiveWorkersText();
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000B8790 File Offset: 0x000B6990
		private int CountWorkersWithMoveOnHex()
		{
			int num = 0;
			for (int i = 0; i < this.Context.HexWorkers.Count; i++)
			{
				if (this.Context.HexWorkers[i].NotMoved())
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000B87D8 File Offset: 0x000B69D8
		private int CountWorkersWithoutMoveOnHex()
		{
			int num = 0;
			for (int i = 0; i < this.Context.HexWorkers.Count; i++)
			{
				if (!this.Context.HexWorkers[i].NotMoved())
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000B8820 File Offset: 0x000B6A20
		private int CountWorkersWithMoveInMech()
		{
			int num = 0;
			for (int i = 0; i < this.Context.LoadedWorkers.Count; i++)
			{
				if (this.Context.LoadedWorkers[i].NotMoved())
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000B8868 File Offset: 0x000B6A68
		private int CountWorkersWithoutMoveInMech()
		{
			int num = 0;
			for (int i = 0; i < this.Context.LoadedWorkers.Count; i++)
			{
				if (!this.Context.LoadedWorkers[i].NotMoved())
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0003B608 File Offset: 0x00039808
		private int CountWorkersWithMove()
		{
			return this.CountWorkersWithMoveOnHex() + this.CountWorkersWithMoveInMech();
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0003B617 File Offset: 0x00039817
		private int CountWorkersWithoutMove()
		{
			return this.CountWorkersWithoutMoveOnHex() + this.CountWorkersWithoutMoveInMech();
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0003B626 File Offset: 0x00039826
		public void LoadFirstWorkerWithMoveMaxUnload()
		{
			if (this.CountWorkersWithMoveInMech() == this.CountWorkersWithMove())
			{
				this.UnloadAllActiveWorkersFromMech();
				return;
			}
			this.LoadFirstWorkerWithMove();
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0003B643 File Offset: 0x00039843
		public void LoadFirstWorkerWithoutMoveMaxUnload()
		{
			if (this.CountWorkersWithoutMoveInMech() == this.CountWorkersWithoutMove())
			{
				this.UnloadAllInActiveorkersFromMech();
				return;
			}
			this.LoadFirstWorkerWithoutMove();
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000B88B0 File Offset: 0x000B6AB0
		public void UnloadWorkerFromMech(int workerId)
		{
			WorldSFXManager.PlaySound(SoundEnum.MechUnload, AudioSourceType.WorldSfx);
			if (!(this.Context.containerUnit is Mech))
			{
				return;
			}
			if (workerId < this.Context.LoadedWorkers.Count)
			{
				if (ExchangePanelPresenter.OnWorkerButtonClicked != null)
				{
					ExchangePanelPresenter.OnWorkerButtonClicked(this.Context.LoadedWorkers[workerId]);
				}
				UnitPresenter unitPresenter = GameController.GetUnitPresenter(this.Context.containerUnit as Mech).UnloadWorker(workerId, false);
				this.Context.HexWorkers.Add(unitPresenter.UnitLogic);
				this.DragAndDropBar.SetWorkersAmount(unitPresenter.UnitLogic.NotMoved(), this.GetNumberOfLoadedWorkers(unitPresenter.UnitLogic.NotMoved()));
				this.UpdateWorkersButtons();
			}
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000B8970 File Offset: 0x000B6B70
		private int GetNumberOfLoadedWorkers(bool notMoved)
		{
			int num = 0;
			if (this.Context.LoadedWorkers != null)
			{
				for (int i = 0; i < this.Context.LoadedWorkers.Count; i++)
				{
					if (this.Context.LoadedWorkers[i].NotMoved() == notMoved)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x000B89C8 File Offset: 0x000B6BC8
		public void UpdateWorkersButtons()
		{
			if (!(this.Context.containerUnit is Mech))
			{
				this.mechSection.SetActive(false);
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.mechSection.SetActive(true);
			if (this.workersLoadedButtons != null)
			{
				foreach (object obj in this.workersLoadedButtons.transform)
				{
					Transform transform = (Transform)obj;
					if (num < this.Context.LoadedWorkers.Count)
					{
						if (this.Context.LoadedWorkers[num].MovesLeft != 0)
						{
							transform.transform.GetChild(0).gameObject.GetComponent<Button>().image.color = Color.yellow;
							num2++;
						}
						else
						{
							transform.transform.GetChild(0).gameObject.GetComponent<Button>().image.color = Color.gray;
							num3++;
						}
						transform.gameObject.SetActive(true);
					}
					else
					{
						transform.gameObject.SetActive(false);
					}
					num++;
				}
			}
			if (this.workersLoadedActiveCount != null)
			{
				this.workersLoadedActiveCount.text = num2.ToString();
			}
			if (this.workersLoadedInactiveCount != null)
			{
				this.workersLoadedInactiveCount.text = num3.ToString();
			}
			num = 0;
			num2 = 0;
			num3 = 0;
			if (this.workersUnloadedButtons != null)
			{
				foreach (object obj2 in this.workersUnloadedButtons.transform)
				{
					Transform transform2 = (Transform)obj2;
					if (num < this.Context.HexWorkers.Count)
					{
						if (this.Context.HexWorkers[num].MovesLeft != 0)
						{
							transform2.transform.GetChild(0).gameObject.GetComponent<Button>().image.color = Color.yellow;
							num2++;
						}
						else
						{
							transform2.transform.GetChild(0).gameObject.GetComponent<Button>().image.color = Color.gray;
							num3++;
						}
						transform2.gameObject.SetActive(true);
					}
					else
					{
						transform2.gameObject.SetActive(false);
					}
					num++;
				}
			}
			if (this.workersUnloadedActiveCount != null)
			{
				this.workersUnloadedActiveCount.text = num2.ToString();
			}
			if (this.workersUnloadedInactiveCount != null)
			{
				this.workersUnloadedInactiveCount.text = num3.ToString();
			}
			if (this.DragAndDropBar.PreviousUnitEqualsUnit(this.Context.containerUnit))
			{
				this.DragAndDropBar.SetWorkersAmount(true, this.GetNumberOfLoadedWorkers(true));
				this.DragAndDropBar.SetWorkersAmount(false, this.GetNumberOfLoadedWorkers(false));
			}
			this.UpdateActiveWorkersText();
			this.UpdateInactiveWorkersText();
			if (this.workersText != null)
			{
				this.workersText.text = string.Format("{0}/{1}", this.Context.LoadedWorkers.Count, this.Context.LoadedWorkers.Count + this.Context.HexWorkers.Count);
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x000B8D3C File Offset: 0x000B6F3C
		public void UpdateActiveWorkersText()
		{
			int num = 0;
			if (this.workersLoadedActiveCount != null)
			{
				for (int i = 0; i < this.Context.LoadedWorkers.Count; i++)
				{
					if (this.Context.LoadedWorkers[i].NotMoved())
					{
						num++;
					}
				}
				this.workersLoadedActiveCount.text = num.ToString();
			}
			bool flag = num > 0;
			if (this.workersUnloadedActiveCount != null)
			{
				num = 0;
				for (int j = 0; j < this.Context.HexWorkers.Count; j++)
				{
					if (this.Context.HexWorkers[j].NotMoved())
					{
						num++;
					}
				}
				this.workersUnloadedActiveCount.text = num.ToString();
			}
			if (!flag)
			{
				flag = num > 0;
			}
			this.UpdateButtonsInteractableForActiveWorkers(flag);
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000B8E10 File Offset: 0x000B7010
		public void UpdateInactiveWorkersText()
		{
			int num = 0;
			if (this.workersLoadedInactiveCount != null)
			{
				for (int i = 0; i < this.Context.LoadedWorkers.Count; i++)
				{
					if (!this.Context.LoadedWorkers[i].NotMoved())
					{
						num++;
					}
				}
				this.workersLoadedInactiveCount.text = num.ToString();
			}
			bool flag = num > 0;
			if (this.workersUnloadedInactiveCount != null)
			{
				num = 0;
				for (int j = 0; j < this.Context.HexWorkers.Count; j++)
				{
					if (!this.Context.HexWorkers[j].NotMoved())
					{
						num++;
					}
				}
				this.workersUnloadedInactiveCount.text = num.ToString();
			}
			if (!flag)
			{
				flag = num > 0;
			}
			this.UpdateButtonsInteractableForInactiveWorkers(flag);
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0003B660 File Offset: 0x00039860
		public void ResetLoadedWorkersInPanel()
		{
			if (this.Context.LoadedWorkers != null)
			{
				this.Context.LoadedWorkers.Clear();
			}
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000B8EE4 File Offset: 0x000B70E4
		public void UnloadAllWorkersFromMech()
		{
			if (this.Context == null || !(this.Context.containerUnit is Mech))
			{
				return;
			}
			while (this.Context.LoadedWorkers.Count != 0)
			{
				this.UnloadWorkerFromMech(0);
			}
			this.UpdateActiveWorkersText();
			this.UpdateInactiveWorkersText();
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000B8F34 File Offset: 0x000B7134
		public void UnloadAllActiveWorkersFromMech()
		{
			if (this.Context == null || !(this.Context.containerUnit is Mech))
			{
				return;
			}
			int num = this.CountWorkersWithMoveInMech();
			for (int i = 0; i < num; i++)
			{
				this.UnloadFirstWorkerWithMove();
			}
			this.UpdateActiveWorkersText();
			this.UpdateInactiveWorkersText();
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000B8F84 File Offset: 0x000B7184
		public void UnloadAllInActiveorkersFromMech()
		{
			if (this.Context == null || !(this.Context.containerUnit is Mech))
			{
				return;
			}
			int num = this.CountWorkersWithoutMoveInMech();
			for (int i = 0; i < num; i++)
			{
				this.UnloadFirstWorkerWithoutMove();
			}
			this.UpdateActiveWorkersText();
			this.UpdateInactiveWorkersText();
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0003B67F File Offset: 0x0003987F
		public void LoadAllWorkersToMech()
		{
			if (!(this.Context.containerUnit is Mech))
			{
				return;
			}
			while (this.Context.HexWorkers.Count != 0)
			{
				this.LoadWorkerToMech(0);
			}
			this.UpdateActiveWorkersText();
			this.UpdateInactiveWorkersText();
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000B8FD4 File Offset: 0x000B71D4
		public void UnloadFirstWorkerWithMove()
		{
			for (int i = 0; i < this.Context.LoadedWorkers.Count; i++)
			{
				if (this.Context.LoadedWorkers[i].NotMoved())
				{
					if (ExchangePanelPresenter.OnWorkerButtonClicked != null)
					{
						ExchangePanelPresenter.OnWorkerButtonClicked(this.Context.LoadedWorkers[i]);
					}
					UnitPresenter unitPresenter = GameController.GetUnitPresenter(this.Context.containerUnit as Mech).UnloadWorker(i, false);
					this.Context.HexWorkers.Add(unitPresenter.UnitLogic);
					this.DragAndDropBar.SetWorkersAmount(unitPresenter.UnitLogic.NotMoved(), this.GetNumberOfLoadedWorkers(unitPresenter.UnitLogic.NotMoved()));
					this.UpdateWorkersButtons();
					this.UpdateActiveWorkersText();
					return;
				}
			}
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000B90A8 File Offset: 0x000B72A8
		public void UnloadFirstWorkerWithoutMove()
		{
			for (int i = 0; i < this.Context.LoadedWorkers.Count; i++)
			{
				if (!this.Context.LoadedWorkers[i].NotMoved())
				{
					if (ExchangePanelPresenter.OnWorkerButtonClicked != null)
					{
						ExchangePanelPresenter.OnWorkerButtonClicked(this.Context.LoadedWorkers[i]);
					}
					UnitPresenter unitPresenter = GameController.GetUnitPresenter(this.Context.containerUnit as Mech).UnloadWorker(i, false);
					this.Context.HexWorkers.Add(unitPresenter.UnitLogic);
					this.DragAndDropBar.SetWorkersAmount(unitPresenter.UnitLogic.NotMoved(), this.GetNumberOfLoadedWorkers(unitPresenter.UnitLogic.NotMoved()));
					this.UpdateWorkersButtons();
					this.UpdateInactiveWorkersText();
					return;
				}
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000B917C File Offset: 0x000B737C
		public void LoadWorker(Unit worker)
		{
			if (this.Context.HexWorkers.Contains(worker))
			{
				GameController.GetUnitPresenter(this.Context.containerUnit).AddWorker(worker);
				this.Context.HexWorkers.Remove(worker);
				this.UpdateWorkersButtons();
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0003B6B9 File Offset: 0x000398B9
		public void ClearPreviousUnit(bool unload = true)
		{
			if (unload)
			{
				this.DragAndDropBar.UnloadEverything();
			}
			this.DragAndDropBar.Clear();
			this.ClearPreviousSliderValues();
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0003B6DA File Offset: 0x000398DA
		public void ClearPreviousSliderValues()
		{
			if (this.Context == null)
			{
				return;
			}
			this.Context.previousStateOfSliders = new Dictionary<ResourceType, int>
			{
				{
					ResourceType.oil,
					0
				},
				{
					ResourceType.metal,
					0
				},
				{
					ResourceType.food,
					0
				},
				{
					ResourceType.wood,
					0
				}
			};
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000B91CC File Offset: 0x000B73CC
		public void Clear()
		{
			if (this.Context != null && (this.Context.previousStateOfSliders[ResourceType.food] != 0 || this.Context.previousStateOfSliders[ResourceType.oil] != 0 || this.Context.previousStateOfSliders[ResourceType.metal] != 0 || this.Context.previousStateOfSliders[ResourceType.wood] != 0))
			{
				this.PassAllToField();
			}
			this.UnloadAllWorkersFromMech();
			this.ClearPreviousSliderValues();
			if (this.Context != null)
			{
				this.Context.containerUnit = null;
				this.Context.field = null;
			}
			if (this.exchangePanelAlternative != null)
			{
				this.exchangePanelAlternative.SetActive(false);
			}
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0003B715 File Offset: 0x00039915
		public void ClearOnUnitsWithdraw()
		{
			if (this.Context != null)
			{
				this.Context.containerUnit = null;
				this.Context.field = null;
			}
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0003B737 File Offset: 0x00039937
		public void OnUnitMoveFinished()
		{
			this.ClearPreviousSliderValues();
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000B927C File Offset: 0x000B747C
		public Dictionary<ResourceType, int> GetResources()
		{
			return new Dictionary<ResourceType, int>
			{
				{
					ResourceType.oil,
					this.exchangePanels[ResourceType.oil].Value - this.Context.containerUnit.resources[ResourceType.oil]
				},
				{
					ResourceType.metal,
					this.exchangePanels[ResourceType.metal].Value - this.Context.containerUnit.resources[ResourceType.metal]
				},
				{
					ResourceType.food,
					this.exchangePanels[ResourceType.food].Value - this.Context.containerUnit.resources[ResourceType.food]
				},
				{
					ResourceType.wood,
					this.exchangePanels[ResourceType.wood].Value - this.Context.containerUnit.resources[ResourceType.wood]
				}
			};
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000B934C File Offset: 0x000B754C
		public bool AllResourcesLoaded()
		{
			return this.exchangePanels[ResourceType.oil].Value == this.exchangePanels[ResourceType.oil].MaxValue && this.exchangePanels[ResourceType.metal].Value == this.exchangePanels[ResourceType.metal].MaxValue && this.exchangePanels[ResourceType.food].Value == this.exchangePanels[ResourceType.food].MaxValue && this.exchangePanels[ResourceType.wood].Value == this.exchangePanels[ResourceType.wood].MaxValue;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0003B73F File Offset: 0x0003993F
		public Dictionary<ResourceType, int> GetPreviusSliderValues()
		{
			return new Dictionary<ResourceType, int>(this.Context.previousStateOfSliders);
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000B93F4 File Offset: 0x000B75F4
		public int GetResourceCount()
		{
			Dictionary<ResourceType, int> resources = this.GetResources();
			return 0 + resources[ResourceType.food] + resources[ResourceType.metal] + resources[ResourceType.oil] + resources[ResourceType.wood];
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0003B751 File Offset: 0x00039951
		public void SwitchExchangePanel()
		{
			this.SetActive(false);
			this.exchangePanelAlternative.SetActive(true);
			this.exchangePanelAlternative.RecoverSharedContext();
		}

		// Token: 0x04001550 RID: 5456
		public GameObject FoodExchange;

		// Token: 0x04001551 RID: 5457
		public GameObject MetalExchange;

		// Token: 0x04001552 RID: 5458
		public GameObject OilExchange;

		// Token: 0x04001553 RID: 5459
		public GameObject WoodExchange;

		// Token: 0x04001554 RID: 5460
		public Dictionary<ResourceType, ExchangeSlot> exchangePanels;

		// Token: 0x04001555 RID: 5461
		private ExchangePanel context;

		// Token: 0x04001556 RID: 5462
		public GameObject mechSection;

		// Token: 0x04001557 RID: 5463
		public GameObject workersLoadedButtons;

		// Token: 0x04001558 RID: 5464
		public GameObject workersUnloadedButtons;

		// Token: 0x04001559 RID: 5465
		public GameObject ActiveWorkersExchange;

		// Token: 0x0400155A RID: 5466
		public GameObject InactiveWorkersExchange;

		// Token: 0x0400155B RID: 5467
		public Text workersLoadedActiveCount;

		// Token: 0x0400155C RID: 5468
		public Text workersLoadedInactiveCount;

		// Token: 0x0400155D RID: 5469
		public Text workersUnloadedActiveCount;

		// Token: 0x0400155E RID: 5470
		public Text workersUnloadedInactiveCount;

		// Token: 0x0400155F RID: 5471
		public Text workersText;

		// Token: 0x04001560 RID: 5472
		public Image unitImage;

		// Token: 0x04001561 RID: 5473
		public Sprite[] unitSprites;

		// Token: 0x04001562 RID: 5474
		public ExchangePanelPresenter exchangePanelAlternative;

		// Token: 0x04001566 RID: 5478
		private CanvasGroup canvasGroup;

		// Token: 0x04001567 RID: 5479
		private bool restoreData;

		// Token: 0x04001568 RID: 5480
		private bool loadedOnDrop;

		// Token: 0x04001569 RID: 5481
		public bool alternative;

		// Token: 0x020003E8 RID: 1000
		// (Invoke) Token: 0x06001E02 RID: 7682
		public delegate void WorkerButtonClicked(Unit worker);

		// Token: 0x020003E9 RID: 1001
		// (Invoke) Token: 0x06001E06 RID: 7686
		public delegate void WorkerLoaded(Unit worker);

		// Token: 0x020003EA RID: 1002
		// (Invoke) Token: 0x06001E0A RID: 7690
		public delegate void ResourceLoaded();
	}
}
