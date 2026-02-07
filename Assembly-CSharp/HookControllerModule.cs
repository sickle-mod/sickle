using System;

// Token: 0x0200007B RID: 123
public class HookControllerModule
{
	// Token: 0x06000451 RID: 1105 RVA: 0x0002AC7D File Offset: 0x00028E7D
	public void SetHookController(HookController hookController)
	{
		this.hookController = hookController;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00027EF0 File Offset: 0x000260F0
	public virtual void Clear()
	{
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00027EF0 File Offset: 0x000260F0
	public virtual void OnConnectionBroken()
	{
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00027EF0 File Offset: 0x000260F0
	public virtual void OnMouseRelease()
	{
	}

	// Token: 0x04000399 RID: 921
	protected HookController hookController;
}
