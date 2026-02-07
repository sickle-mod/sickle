using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x0200069D RID: 1693
	[ExecuteInEditMode]
	public class WaterTile : MonoBehaviour
	{
		// Token: 0x0600349D RID: 13469 RVA: 0x00049483 File Offset: 0x00047683
		public void Start()
		{
			this.AcquireComponents();
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x001365AC File Offset: 0x001347AC
		private void AcquireComponents()
		{
			if (!this.reflection)
			{
				if (base.transform.parent)
				{
					this.reflection = base.transform.parent.GetComponent<PlanarReflection>();
				}
				else
				{
					this.reflection = base.transform.GetComponent<PlanarReflection>();
				}
			}
			if (!this.waterBase)
			{
				if (base.transform.parent)
				{
					this.waterBase = base.transform.parent.GetComponent<WaterBase>();
					return;
				}
				this.waterBase = base.transform.GetComponent<WaterBase>();
			}
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x00136648 File Offset: 0x00134848
		public void OnWillRenderObject()
		{
			if (this.reflection)
			{
				this.reflection.WaterTileBeingRendered(base.transform, Camera.current);
			}
			if (this.waterBase)
			{
				this.waterBase.WaterTileBeingRendered(base.transform, Camera.current);
			}
		}

		// Token: 0x040024F3 RID: 9459
		public PlanarReflection reflection;

		// Token: 0x040024F4 RID: 9460
		public WaterBase waterBase;
	}
}
