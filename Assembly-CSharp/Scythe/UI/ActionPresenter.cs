using System;
using Scythe.GameLogic.Actions;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020003D1 RID: 977
	public abstract class ActionPresenter : MonoBehaviour
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x0003AB00 File Offset: 0x00038D00
		// (set) Token: 0x06001CA4 RID: 7332 RVA: 0x0003AB08 File Offset: 0x00038D08
		[HideInInspector]
		public bool NestedMode { get; set; }

		// Token: 0x06001CA5 RID: 7333
		public abstract void ChangeLayoutForAction(BaseAction action);

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0003AB11 File Offset: 0x00038D11
		public virtual void ChangeLayoutForAction(BaseAction action, ActionPresenter mainPresenter)
		{
			this.mainPresenter = mainPresenter;
			this.NestedMode = true;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0003AB21 File Offset: 0x00038D21
		public virtual void OnEndActionConfirmClicked()
		{
			this.OnActionEnded();
		}

		// Token: 0x06001CA8 RID: 7336
		public abstract void OnActionEnded();

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void OnNestedActionEnded()
		{
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Clear()
		{
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x0003AB29 File Offset: 0x00038D29
		protected void EnableMapBlackout()
		{
			GameController.Instance.gameBoardPresenter.EnableMapBlackout();
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0003AB3A File Offset: 0x00038D3A
		protected void DisableMapBlackout()
		{
			GameController.Instance.gameBoardPresenter.DisableMapBlackout();
		}

		// Token: 0x040014B7 RID: 5303
		protected ActionPresenter mainPresenter;
	}
}
