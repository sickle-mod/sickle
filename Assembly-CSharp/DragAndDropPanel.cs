using System;
using System.Collections.Generic;
using DG.Tweening;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000076 RID: 118
public class DragAndDropPanel : MonoBehaviour
{
	// Token: 0x06000407 RID: 1031 RVA: 0x000634E8 File Offset: 0x000616E8
	private void Start()
	{
		for (int i = 0; i < this.buttonsList.childCount - 2; i++)
		{
			int id = i;
			this.buttonsList.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
			{
				this.OnButtonClicked(id);
			});
		}
		this.GetLoadAllButton().GetButton().onClick.AddListener(delegate
		{
			this.LoadEverything();
		});
		this.GetUnloadAllButton().GetButton().onClick.AddListener(delegate
		{
			this.UnloadEverything();
		});
		this.EnableRotationTowardsCamera(true);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0002A8BE File Offset: 0x00028ABE
	private void Update()
	{
		this.UpdatePosition();
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00063594 File Offset: 0x00061794
	public void SetActive(bool active, bool force = false)
	{
		if (force)
		{
			base.transform.DOComplete(true);
		}
		if (active == base.gameObject.activeSelf)
		{
			return;
		}
		if (active && this.state != DragAndDropPanel.State.Maxmimizing)
		{
			this.ShowPanelAnimation();
			return;
		}
		if (!active && this.state != DragAndDropPanel.State.Minimizing)
		{
			this.HidePanelAnimation();
		}
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0002A8C6 File Offset: 0x00028AC6
	public void AttachExchangePanel(ExchangePanelPresenter exchangePanelPresenter)
	{
		this.exchangePanel = exchangePanelPresenter;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x000635E8 File Offset: 0x000617E8
	public void OnDragAndDropStateChanged(bool enabled)
	{
		if (this.exchangePanel.GetContext().containerUnit != null)
		{
			if (enabled)
			{
				this.OnDragAndDropEnabled(GameController.GetUnitPresenter(this.exchangePanel.GetContext().containerUnit), this.exchangePanel.GetPreviusSliderValues(), this.exchangePanel.GetContext().LoadedWorkers);
				return;
			}
			this.SetActive(false, false);
			this.Clear();
		}
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0002A8CF File Offset: 0x00028ACF
	public void OnDragAndDropEnabled(UnitPresenter unitPresenter, Dictionary<ResourceType, int> loadedResources, List<Unit> loadedUnits)
	{
		this.SetResources(loadedResources);
		this.SetLoadedUnits(loadedUnits);
		this.ChangeUnit(unitPresenter);
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00063650 File Offset: 0x00061850
	private void SetResources(Dictionary<ResourceType, int> loadedResources)
	{
		foreach (ResourceType resourceType in loadedResources.Keys)
		{
			this.SetResourceAmount(resourceType, loadedResources[resourceType]);
		}
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x000636AC File Offset: 0x000618AC
	private void SetLoadedUnits(List<Unit> loadedUnits)
	{
		int num = 0;
		int num2 = 0;
		using (List<Unit>.Enumerator enumerator = loadedUnits.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.NotMoved())
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		this.SetWorkersAmount(true, num);
		this.SetWorkersAmount(false, num2);
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0002A8E6 File Offset: 0x00028AE6
	public void ChangeUnit(UnitPresenter unitPresenter)
	{
		this.targetUnit = unitPresenter;
		this.targetUnitLogic = unitPresenter.UnitLogic;
		this.FinishAnimationsOnButtons();
		this.PrepareButtonsOnUnitChange(unitPresenter.UnitLogic.UnitType);
		if (this.AnythingLoaded())
		{
			this.SetActive(true, false);
		}
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00063718 File Offset: 0x00061918
	private void PrepareButtonsOnUnitChange(UnitType unitType)
	{
		if (unitType == UnitType.Mech)
		{
			if (!this.GetButton(4).gameObject.activeSelf)
			{
				this.GetUnloadAllButton().MoveByDistance(26f);
				this.GetLoadAllButton().MoveByDistance(26f);
			}
			this.GetButton(4).SetActive(true);
			this.GetButton(5).SetActive(true);
			return;
		}
		if (this.GetButton(4).gameObject.activeSelf)
		{
			this.GetUnloadAllButton().MoveByDistance(-26f);
			this.GetLoadAllButton().MoveByDistance(-26f);
		}
		this.GetButton(4).SetActive(false);
		this.GetButton(4).ForceToFinishAnimation();
		this.GetButton(5).SetActive(false);
		this.GetButton(5).ForceToFinishAnimation();
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x000637DC File Offset: 0x000619DC
	public bool OnUnitChanged(Unit newUnit, SelectMethod selectMethod = SelectMethod.Default)
	{
		bool flag = false;
		if (newUnit == this.targetUnitLogic)
		{
			flag = true;
		}
		else if (this.targetUnitLogic == null || selectMethod != SelectMethod.Default || !this.AnythingLoaded())
		{
			if (this.targetUnitLogic != null && this.targetUnitLogic != newUnit && selectMethod != SelectMethod.LoadingWorker)
			{
				this.UnloadEverything();
			}
			if (selectMethod != SelectMethod.LoadingWorker)
			{
				this.Clear();
			}
			else
			{
				flag = true;
			}
			if (selectMethod != SelectMethod.Default)
			{
				base.transform.DOComplete(true);
			}
			this.ChangeUnit(GameController.GetUnitPresenter(newUnit));
		}
		return flag;
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00063854 File Offset: 0x00061A54
	public void OnButtonClicked(int buttonId)
	{
		if (0 <= buttonId && buttonId <= 3)
		{
			this.exchangePanel.PassOneResourceToField(buttonId);
			return;
		}
		if (4 <= buttonId && buttonId <= 5)
		{
			if (buttonId == 4)
			{
				this.exchangePanel.UnloadFirstWorkerWithMove();
			}
			else if (buttonId == 5)
			{
				this.exchangePanel.UnloadFirstWorkerWithoutMove();
			}
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.WorkerUnload);
		}
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x000638B0 File Offset: 0x00061AB0
	public void UnloadEverything()
	{
		if (this.exchangePanel.GetContext().containerUnit == this.targetUnitLogic)
		{
			this.exchangePanel.PassAllToField(true);
			this.exchangePanel.UnloadAllWorkersFromMech();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.TransportUnload);
			return;
		}
		if (this.targetUnitLogic != null)
		{
			GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(this.targetUnitLogic.position);
			List<ResourceType> list = new List<ResourceType>(this.previouslyLoadedResources.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				ResourceType resourceType = list[i];
				if (this.previouslyLoadedResources[resourceType] != 0)
				{
					int num = this.targetUnitLogic.position.resources[resourceType];
					gameHexPresenter.ExchangeShowResourceMove(resourceType, GameController.GetUnitPresenter(this.targetUnitLogic).gameObject.transform.position, true, this.previouslyLoadedResources[resourceType], false);
					this.SetResourceAmount(resourceType, 0);
				}
			}
			GameController.GetUnitPresenter(this.targetUnitLogic).UnloadAllWorkers(false);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.TransportUnload);
		}
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x000639C4 File Offset: 0x00061BC4
	public void LoadEverything()
	{
		if (this.exchangePanel.GetContext().containerUnit == this.targetUnitLogic)
		{
			this.exchangePanel.PassAllToUnit();
			this.exchangePanel.LoadAllWorkersToMech();
			return;
		}
		if (this.targetUnitLogic != null)
		{
			Debug.Log("Should not be called");
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00063A14 File Offset: 0x00061C14
	public void UpdatePosition()
	{
		if (this.state != DragAndDropPanel.State.Minimizing && this.targetUnit != null)
		{
			this.attachedToPosition = this.targetUnit.transform.position;
		}
		if (this.targetUnit != null)
		{
			base.transform.position = this.attachedToPosition;
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0002A922 File Offset: 0x00028B22
	public void EnableRotationTowardsCamera(bool enabled)
	{
		if (this.rotateTowardsCamera == null)
		{
			this.rotateTowardsCamera = base.GetComponent<RotateTowardsCamera>();
		}
		this.rotateTowardsCamera.enabled = enabled;
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00063A70 File Offset: 0x00061C70
	private DragAndDropPanelButton GetButton(int id)
	{
		Transform child = this.buttonsList.GetChild(id);
		if (child == null)
		{
			Debug.LogError("Context button (id: " + id.ToString() + ") is empty!");
			return null;
		}
		DragAndDropPanelButton component = child.GetComponent<DragAndDropPanelButton>();
		if (component == null)
		{
			Debug.LogError("Context button (id: " + id.ToString() + ") logic not found!");
		}
		return component;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0002A94A File Offset: 0x00028B4A
	private DragAndDropPanelButton GetLoadAllButton()
	{
		return this.GetButton(6);
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0002A953 File Offset: 0x00028B53
	private DragAndDropPanelButton GetUnloadAllButton()
	{
		return this.GetButton(7);
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0002A95C File Offset: 0x00028B5C
	public UnitPresenter GetUnit()
	{
		return this.targetUnit;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00063ADC File Offset: 0x00061CDC
	private void FinishAnimationsOnButtons()
	{
		for (int i = 0; i < this.buttonsList.childCount; i++)
		{
			this.GetButton(i).ForceToFinishAnimation();
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00063B0C File Offset: 0x00061D0C
	public void SetResourceAmount(ResourceType resource, int amount)
	{
		DragAndDropPanelButton button = this.GetButton((int)resource);
		button.SetNumberOfObjects(amount);
		button.SetInteractable(amount != 0);
		this.previouslyLoadedResources[resource] = amount;
		bool flag = this.AnythingLoaded();
		this.SetActive(flag, false);
		if (base.gameObject.activeSelf != flag)
		{
			this.SetActive(flag, false);
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00063B64 File Offset: 0x00061D64
	public void SetWorkersAmount(bool workerWithMove, int amount)
	{
		DragAndDropPanelButton button = this.GetButton(4 + (workerWithMove ? 0 : 1));
		button.SetNumberOfObjects(amount);
		button.SetInteractable(amount != 0);
		bool flag = this.AnythingLoaded();
		if (base.gameObject.activeSelf != flag)
		{
			this.SetActive(flag, false);
		}
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0002A964 File Offset: 0x00028B64
	public bool PreviousUnitEqualsUnit(Unit unit)
	{
		return this.targetUnitLogic == unit;
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00063BB0 File Offset: 0x00061DB0
	public bool AnythingLoaded()
	{
		for (int i = 0; i < this.buttonsList.childCount - 2; i++)
		{
			if (!this.GetButton(i).GetText().Equals("0"))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00063BF0 File Offset: 0x00061DF0
	private void ShowPanelAnimation()
	{
		base.transform.DOComplete(true);
		base.gameObject.SetActive(true);
		this.state = DragAndDropPanel.State.Maxmimizing;
		base.transform.localScale = this.panelMinimizedScale;
		base.transform.DOScale(this.panelMaximizedScale, 0.5f).SetEase(Ease.OutBounce).OnComplete(delegate
		{
			this.OnShowPanelComplete();
		});
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0002A96F File Offset: 0x00028B6F
	private void OnShowPanelComplete()
	{
		this.state = DragAndDropPanel.State.Visible;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00063C60 File Offset: 0x00061E60
	private void HidePanelAnimation()
	{
		base.transform.DOComplete(true);
		this.state = DragAndDropPanel.State.Minimizing;
		base.transform.DOScale(this.panelMinimizedScale, 0.5f).SetEase(Ease.OutExpo).OnComplete(delegate
		{
			this.OnHidePanelComplete();
		});
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0002A978 File Offset: 0x00028B78
	private void OnHidePanelComplete()
	{
		base.gameObject.SetActive(false);
		this.state = DragAndDropPanel.State.Hidden;
		if (this.targetUnit != null)
		{
			this.attachedToPosition = this.targetUnit.transform.position;
		}
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00063CB0 File Offset: 0x00061EB0
	public void Clear()
	{
		if (this.targetUnit == null)
		{
			return;
		}
		this.SetResourceAmount(ResourceType.oil, 0);
		this.SetResourceAmount(ResourceType.metal, 0);
		this.SetResourceAmount(ResourceType.wood, 0);
		this.SetResourceAmount(ResourceType.food, 0);
		this.SetWorkersAmount(true, 0);
		this.SetWorkersAmount(false, 0);
		this.previouslyLoadedResources = new Dictionary<ResourceType, int>
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
		this.targetUnit = null;
		this.targetUnitLogic = null;
		this.SetActive(false, true);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0002A9B1 File Offset: 0x00028BB1
	public bool IsPanelVisible()
	{
		return this.state == DragAndDropPanel.State.Visible;
	}

	// Token: 0x04000382 RID: 898
	[SerializeField]
	private Transform buttonsList;

	// Token: 0x04000383 RID: 899
	[SerializeField]
	private ExchangePanelPresenter exchangePanel;

	// Token: 0x04000384 RID: 900
	[SerializeField]
	private RotateTowardsCamera rotateTowardsCamera;

	// Token: 0x04000385 RID: 901
	private UnitPresenter targetUnit;

	// Token: 0x04000386 RID: 902
	private Vector3 attachedToPosition;

	// Token: 0x04000387 RID: 903
	private Unit targetUnitLogic;

	// Token: 0x04000388 RID: 904
	private Dictionary<ResourceType, int> previouslyLoadedResources = new Dictionary<ResourceType, int>
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

	// Token: 0x04000389 RID: 905
	private Vector3 panelMaximizedScale = new Vector3(1f, 1f, 1f);

	// Token: 0x0400038A RID: 906
	private Vector3 panelMinimizedScale = new Vector3(0.01f, 0.01f, 1f);

	// Token: 0x0400038B RID: 907
	private DragAndDropPanel.State state = DragAndDropPanel.State.Hidden;

	// Token: 0x02000077 RID: 119
	private enum State
	{
		// Token: 0x0400038D RID: 909
		Visible,
		// Token: 0x0400038E RID: 910
		Hidden,
		// Token: 0x0400038F RID: 911
		Maxmimizing,
		// Token: 0x04000390 RID: 912
		Minimizing
	}
}
