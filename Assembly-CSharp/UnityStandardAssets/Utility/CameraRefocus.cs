using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000664 RID: 1636
	public class CameraRefocus
	{
		// Token: 0x060033B7 RID: 13239 RVA: 0x00048A8D File Offset: 0x00046C8D
		public CameraRefocus(Camera camera, Transform parent, Vector3 origCameraPos)
		{
			this.m_OrigCameraPos = origCameraPos;
			this.Camera = camera;
			this.Parent = parent;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x00048AAA File Offset: 0x00046CAA
		public void ChangeCamera(Camera camera)
		{
			this.Camera = camera;
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x00048AB3 File Offset: 0x00046CB3
		public void ChangeParent(Transform parent)
		{
			this.Parent = parent;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x00132CBC File Offset: 0x00130EBC
		public void GetFocusPoint()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.Parent.transform.position + this.m_OrigCameraPos, this.Parent.transform.forward, out raycastHit, 100f))
			{
				this.Lookatpoint = raycastHit.point;
				this.m_Refocus = true;
				return;
			}
			this.m_Refocus = false;
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x00048ABC File Offset: 0x00046CBC
		public void SetFocusPoint()
		{
			if (this.m_Refocus)
			{
				this.Camera.transform.LookAt(this.Lookatpoint);
			}
		}

		// Token: 0x040023F4 RID: 9204
		public Camera Camera;

		// Token: 0x040023F5 RID: 9205
		public Vector3 Lookatpoint;

		// Token: 0x040023F6 RID: 9206
		public Transform Parent;

		// Token: 0x040023F7 RID: 9207
		private Vector3 m_OrigCameraPos;

		// Token: 0x040023F8 RID: 9208
		private bool m_Refocus;
	}
}
