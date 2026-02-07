using System;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004FA RID: 1274
	public class SafetyAreaVisibility : MonoBehaviour
	{
		// Token: 0x060028DE RID: 10462 RVA: 0x000428A2 File Offset: 0x00040AA2
		private void Awake()
		{
			this.DetermineVisibility();
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000428AA File Offset: 0x00040AAA
		private void DetermineVisibility()
		{
			base.gameObject.SetActive((PlatformManager.IsSafeAreaActive && this.visibility == SafetyAreaVisibility.Visibility.OnlySafetyArea) || (!PlatformManager.IsSafeAreaActive && this.visibility == SafetyAreaVisibility.Visibility.OnlyNoSafetyArea));
		}

		// Token: 0x04001D4D RID: 7501
		[SerializeField]
		private SafetyAreaVisibility.Visibility visibility;

		// Token: 0x020004FB RID: 1275
		private enum Visibility
		{
			// Token: 0x04001D4F RID: 7503
			OnlySafetyArea,
			// Token: 0x04001D50 RID: 7504
			OnlyNoSafetyArea
		}
	}
}
