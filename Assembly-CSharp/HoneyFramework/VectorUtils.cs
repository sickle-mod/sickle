using System;
using UnityEngine;

namespace HoneyFramework
{
	// Token: 0x020001C1 RID: 449
	public class VectorUtils
	{
		// Token: 0x06000D3C RID: 3388 RVA: 0x00030BFE File Offset: 0x0002EDFE
		public static Vector2 Vector3To2D(Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00030C11 File Offset: 0x0002EE11
		public static Vector3 Vector2To3D(Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}
	}
}
