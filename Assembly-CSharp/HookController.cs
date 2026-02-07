using System;
using Scythe.BoardPresenter;
using Scythe.UI;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class HookController : MonoBehaviour
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x0600043B RID: 1083 RVA: 0x0002AAF0 File Offset: 0x00028CF0
	private DragAndDropHook Hook
	{
		get
		{
			return GameController.Instance.hook;
		}
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x0002AAFC File Offset: 0x00028CFC
	private void Start()
	{
		this.unitsModule = new HooksUnitsModule(this);
		this.resourceModule = new HooksResourcesModule(this);
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00063E74 File Offset: 0x00062074
	private void Update()
	{
		if (this.work && this.IsSomethingDragged() && Input.GetMouseButtonUp(0))
		{
			this.DetectMouseRelease();
		}
		if (Input.GetKeyDown(KeyCode.Escape) && this.IsSomethingDragged())
		{
			if (this.IsUnitDragged())
			{
				this.unitsModule.OnConnectionBroken();
			}
			this.Break();
		}
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x0002AB16 File Offset: 0x00028D16
	public void StartWorkWithUser(object hookUser)
	{
		this.hookUser = hookUser;
		this.work = true;
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0002AB26 File Offset: 0x00028D26
	public object GetHookUser()
	{
		return this.hookUser;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0002AB2E File Offset: 0x00028D2E
	public void PauseWork(bool pause)
	{
		if (this.IsSomethingDragged())
		{
			this.work = !pause;
		}
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0002AB42 File Offset: 0x00028D42
	public void StartWorkWithUnits(IHooksUnitControllerUser unitUser)
	{
		this.unitsModule.StartWorkWithUser(unitUser);
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0002AB50 File Offset: 0x00028D50
	public void StartWorkWithResources(IHooksResourceControllerUser resourceUser)
	{
		this.resourceModule.StartWorkWithUser(resourceUser);
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0002AB5E File Offset: 0x00028D5E
	public void FinishWork()
	{
		this.DetectMouseRelease();
		this.unitsModule.Clear();
		this.resourceModule.Clear();
		this.work = false;
		this.hookUser = null;
		this.SetResourceFocusDetectorEnabled(false);
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0002AB91 File Offset: 0x00028D91
	public void Attach(GameObject objectToMove, IDraggableObject objectLogic)
	{
		this.Hook.Attach(objectToMove, objectLogic);
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0002ABA0 File Offset: 0x00028DA0
	public void Detach(Vector3 position, bool loadingToUnit = false)
	{
		this.Hook.Detach(position, loadingToUnit);
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0002ABAF File Offset: 0x00028DAF
	public void Break()
	{
		this.Hook.Break();
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0002ABBC File Offset: 0x00028DBC
	public void SetMovingActive(bool active)
	{
		this.Hook.SetMovingActive(active);
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0002ABCA File Offset: 0x00028DCA
	public bool IsUnitDragged()
	{
		return this.Hook.GetDraggedObjectLogic() is UnitPresenter;
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0002ABDF File Offset: 0x00028DDF
	public bool IsResourceDragged()
	{
		return this.Hook.GetDraggedObjectLogic() is ResourcePresenter;
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0002ABF4 File Offset: 0x00028DF4
	public bool IsSomethingDragged()
	{
		return this.Hook.GetDraggedObject() != null;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x0002AC07 File Offset: 0x00028E07
	public bool CursorSnapsToObject()
	{
		return this.IsSomethingDragged() && this.Hook.CursorSnapsToObject();
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0002AC1E File Offset: 0x00028E1E
	private void DetectMouseRelease()
	{
		if (this.IsUnitDragged())
		{
			this.unitsModule.OnMouseRelease();
			return;
		}
		if (this.IsResourceDragged())
		{
			this.resourceModule.OnMouseRelease();
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0002AC47 File Offset: 0x00028E47
	public void SetResourceFocusDetectorEnabled(bool enabled)
	{
		if (enabled)
		{
			this.resourceModule.AddResourceListeners();
			return;
		}
		this.resourceModule.RemoveResourceListeners();
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0002AC63 File Offset: 0x00028E63
	public DragAndDropPanel GetDragAndDropPanel()
	{
		return this.Hook.GetDragAndDropPanel();
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0002AC70 File Offset: 0x00028E70
	public bool CursorOnDragAndDropPanel()
	{
		return this.Hook.IsCursorOnDragAndDropPanel();
	}

	// Token: 0x04000395 RID: 917
	private bool work;

	// Token: 0x04000396 RID: 918
	private object hookUser;

	// Token: 0x04000397 RID: 919
	private HooksUnitsModule unitsModule;

	// Token: 0x04000398 RID: 920
	private HooksResourcesModule resourceModule;
}
