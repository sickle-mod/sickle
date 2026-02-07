using System;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class HooksUnitsModule : HookControllerModule
{
	// Token: 0x06000464 RID: 1124 RVA: 0x0002AC86 File Offset: 0x00028E86
	public HooksUnitsModule(HookController hookController)
	{
		base.SetHookController(hookController);
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0002AD4B File Offset: 0x00028F4B
	public void StartWorkWithUser(IHooksUnitControllerUser dragAndDropMoveHookUser)
	{
		this.hookUser = dragAndDropMoveHookUser;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0002AD54 File Offset: 0x00028F54
	public override void OnConnectionBroken()
	{
		this.hookUser.OnConnectionBroken();
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0002AD61 File Offset: 0x00028F61
	public override void OnMouseRelease()
	{
		this.DetectMouseRelease();
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00064128 File Offset: 0x00062328
	private void DetectMouseRelease()
	{
		if (this.hookUser.GetSelectedUnit() == null)
		{
			return;
		}
		if (this.hookController.CursorOnDragAndDropPanel())
		{
			this.CursorOnUnitReleased(this.hookController.GetDragAndDropPanel().GetUnit());
			return;
		}
		if (!GameController.Instance.cameraControler.MouseHitTestUI())
		{
			this.hookUser.CursorNoHit();
			return;
		}
		if (this.hookUser.CursorRaycastBlocked())
		{
			this.hookUser.CursorNoHit();
			return;
		}
		RaycastHit raycastHit;
		bool flag = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, LayerMask.GetMask(new string[] { "Units" }) | LayerMask.GetMask(new string[] { "Hex2d" }));
		if (flag && raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Units"))
		{
			this.CursorOnUnitReleased(raycastHit.collider.gameObject.GetComponent<UnitPresenter>());
			return;
		}
		if (flag && raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Hex2d"))
		{
			this.hookUser.CursorHitHex();
			return;
		}
		this.hookUser.CursorNoHit();
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00064254 File Offset: 0x00062454
	private void CursorOnUnitReleased(UnitPresenter unitUnderTheCursor)
	{
		if (this.hookController.IsResourceDragged())
		{
			return;
		}
		ExchangePanelPresenter exchangePanel = this.hookUser.GetExchangePanel();
		Unit selectedUnit = this.hookUser.GetSelectedUnit();
		if (!this.hookUser.UnitUnderTheCursorIsCorrect(unitUnderTheCursor))
		{
			if (!exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(this.hookUser.GetSelectedUnit()))
			{
				exchangePanel.ClearPreviousUnit(true);
			}
			this.hookUser.CursorNoHit();
			return;
		}
		if (selectedUnit.UnitType == UnitType.Worker)
		{
			if (!exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(unitUnderTheCursor.UnitLogic) && !exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(selectedUnit))
			{
				exchangePanel.ClearPreviousUnit(true);
			}
			this.hookUser.CursorHitUnit(unitUnderTheCursor);
			return;
		}
		this.hookUser.CursorNoHit();
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0002AD69 File Offset: 0x00028F69
	public override void Clear()
	{
		this.hookUser = null;
	}

	// Token: 0x0400039D RID: 925
	private IHooksUnitControllerUser hookUser;
}
