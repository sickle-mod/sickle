using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200007C RID: 124
public class HooksResourcesModule : HookControllerModule
{
	// Token: 0x06000456 RID: 1110 RVA: 0x0002AC86 File Offset: 0x00028E86
	public HooksResourcesModule(HookController hookController)
	{
		base.SetHookController(hookController);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0002AC95 File Offset: 0x00028E95
	public void StartWorkWithUser(IHooksResourceControllerUser hookUser)
	{
		this.hookUser = hookUser;
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0002AC9E File Offset: 0x00028E9E
	public void AddResourceListeners()
	{
		ResourcePresenter.ResourceDown += this.ResourcePickedUp;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0002ACB1 File Offset: 0x00028EB1
	public void RemoveResourceListeners()
	{
		ResourcePresenter.ResourceDown -= this.ResourcePickedUp;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00063ECC File Offset: 0x000620CC
	private void ResourcePickedUp(ResourcePresenter presenter)
	{
		if (GameController.Instance.DragAndDrop && this.CanBePickedUp(presenter))
		{
			this.draggedResource = presenter;
			this.resourceObject = this.GetResourceObject(presenter);
			Outline[] componentsInChildren = this.resourceObject.GetComponentsInChildren<Outline>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			this.resourceObject.SetActive(true);
			this.resourceObject.transform.position = presenter.gameObject.transform.position;
			presenter.SetTemporaryResource(this.resourceObject);
			this.hookController.Attach(this.resourceObject.gameObject, presenter);
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00063F7C File Offset: 0x0006217C
	private bool CanBePickedUp(ResourcePresenter presenter)
	{
		if (presenter.hex.GetGameHexLogic().Owner != GameController.GameManager.PlayerCurrent || presenter.hex.GetGameHexLogic().Enemy != null)
		{
			return false;
		}
		List<Unit> ownerUnits = presenter.hex.GetGameHexLogic().GetOwnerUnits();
		for (int i = 0; i < ownerUnits.Count; i++)
		{
			if (ownerUnits[i].MovesLeft != 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00063FEC File Offset: 0x000621EC
	public GameObject GetResourceObject(ResourcePresenter presenter)
	{
		switch (presenter.resourceType)
		{
		case ResourceType.oil:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(2);
		case ResourceType.metal:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(0);
		case ResourceType.food:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(3);
		case ResourceType.wood:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(1);
		default:
			return null;
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0002ACC4 File Offset: 0x00028EC4
	public override void OnMouseRelease()
	{
		this.DetectMouseRelease(this.draggedResource);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0006404C File Offset: 0x0006224C
	private void DetectMouseRelease(ResourcePresenter resourcePresenter)
	{
		if (GameController.Instance.DragAndDrop && this.resourceObject != null && this.resourceObject.activeInHierarchy)
		{
			UnitPresenter unitPresenter = null;
			RaycastHit raycastHit;
			if (this.hookController.CursorOnDragAndDropPanel())
			{
				unitPresenter = this.hookController.GetDragAndDropPanel().GetUnit();
			}
			else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, LayerMask.GetMask(new string[] { "Units" })) && GameController.Instance.cameraControler.MouseHitTestUI())
			{
				unitPresenter = raycastHit.collider.GetComponent<UnitPresenter>();
			}
			if (unitPresenter != null)
			{
				this.DroppedOnUnit(resourcePresenter, unitPresenter);
			}
			else
			{
				this.hookController.Break();
			}
			this.draggedResource = null;
			this.resourceObject = null;
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0002ACD2 File Offset: 0x00028ED2
	private void DroppedOnUnit(ResourcePresenter resourcePresenter, UnitPresenter unitPresenter)
	{
		if (this.IsUnitNotOnCorrectHex(resourcePresenter, unitPresenter))
		{
			this.hookController.Break();
			return;
		}
		if (this.hookUser.UnitUnderTheCursorIsCorrect(unitPresenter))
		{
			this.hookUser.CursorHitUnit(resourcePresenter, unitPresenter);
			return;
		}
		this.hookController.Break();
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0002AD11 File Offset: 0x00028F11
	private bool IsUnitNotOnCorrectHex(ResourcePresenter resourcePresenter, UnitPresenter unitPresenter)
	{
		return unitPresenter == null || resourcePresenter == null || unitPresenter.UnitLogic.position != resourcePresenter.hex.GetGameHexLogic();
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0002AD42 File Offset: 0x00028F42
	public override void Clear()
	{
		this.hookUser = null;
	}

	// Token: 0x0400039A RID: 922
	private GameObject resourceObject;

	// Token: 0x0400039B RID: 923
	private ResourcePresenter draggedResource;

	// Token: 0x0400039C RID: 924
	private IHooksResourceControllerUser hookUser;
}
