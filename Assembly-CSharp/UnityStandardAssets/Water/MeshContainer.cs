using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000696 RID: 1686
	public class MeshContainer
	{
		// Token: 0x0600347A RID: 13434 RVA: 0x00049233 File Offset: 0x00047433
		public MeshContainer(Mesh m)
		{
			this.mesh = m;
			this.vertices = m.vertices;
			this.normals = m.normals;
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x0004925A File Offset: 0x0004745A
		public void Update()
		{
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
		}

		// Token: 0x040024CC RID: 9420
		public Mesh mesh;

		// Token: 0x040024CD RID: 9421
		public Vector3[] vertices;

		// Token: 0x040024CE RID: 9422
		public Vector3[] normals;
	}
}
