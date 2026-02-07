using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006F1 RID: 1777
	public class ButtonHandler : MonoBehaviour
	{
		// Token: 0x06003599 RID: 13721 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void OnEnable()
		{
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0004A243 File Offset: 0x00048443
		public void SetDownState()
		{
			CrossPlatformInputManager.SetButtonDown(this.Name);
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x0004A250 File Offset: 0x00048450
		public void SetUpState()
		{
			CrossPlatformInputManager.SetButtonUp(this.Name);
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x0004A25D File Offset: 0x0004845D
		public void SetAxisPositiveState()
		{
			CrossPlatformInputManager.SetAxisPositive(this.Name);
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x0004A26A File Offset: 0x0004846A
		public void SetAxisNeutralState()
		{
			CrossPlatformInputManager.SetAxisZero(this.Name);
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x0004A277 File Offset: 0x00048477
		public void SetAxisNegativeState()
		{
			CrossPlatformInputManager.SetAxisNegative(this.Name);
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void Update()
		{
		}

		// Token: 0x04002754 RID: 10068
		public string Name;
	}
}
