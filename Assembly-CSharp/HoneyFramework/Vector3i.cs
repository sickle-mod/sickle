using System;
using UnityEngine;

namespace HoneyFramework
{
	// Token: 0x020001C0 RID: 448
	[Serializable]
	public struct Vector3i
	{
		// Token: 0x06000D2B RID: 3371 RVA: 0x00030A35 File Offset: 0x0002EC35
		public Vector3i(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00030A4C File Offset: 0x0002EC4C
		public Vector3i(int x, int y)
		{
			this.x = x;
			this.y = y;
			this.z = 0;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00085190 File Offset: 0x00083390
		public static int DistanceSquared(Vector3i a, Vector3i b)
		{
			int num = b.x - a.x;
			int num2 = b.y - a.y;
			int num3 = b.z - a.z;
			return num * num + num2 * num2 + num3 * num3;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00030A63 File Offset: 0x0002EC63
		public int DistanceSquared(Vector3i v)
		{
			return Vector3i.DistanceSquared(this, v);
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00030A71 File Offset: 0x0002EC71
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2) ^ (this.z.GetHashCode() >> 2);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000851D0 File Offset: 0x000833D0
		public override bool Equals(object other)
		{
			if (!(other is Vector3i))
			{
				return false;
			}
			Vector3i vector3i = (Vector3i)other;
			return this.x == vector3i.x && this.y == vector3i.y && this.z == vector3i.z;
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0008521C File Offset: 0x0008341C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Vector3i(",
				this.x.ToString(),
				" ",
				this.y.ToString(),
				" ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00030A9A File Offset: 0x0002EC9A
		public static Vector3i Min(Vector3i a, Vector3i b)
		{
			return new Vector3i(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00030AD4 File Offset: 0x0002ECD4
		public static Vector3i Max(Vector3i a, Vector3i b)
		{
			return new Vector3i(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00030B0E File Offset: 0x0002ED0E
		public static bool operator ==(Vector3i a, Vector3i b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00030B3C File Offset: 0x0002ED3C
		public static bool operator !=(Vector3i a, Vector3i b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00030B6D File Offset: 0x0002ED6D
		public static Vector3i operator -(Vector3i a, Vector3i b)
		{
			return new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00030B9B File Offset: 0x0002ED9B
		public static Vector3i operator +(Vector3i a, Vector3i b)
		{
			return new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00030BC9 File Offset: 0x0002EDC9
		public static implicit operator Vector3(Vector3i v)
		{
			return new Vector3((float)v.x, (float)v.y, (float)v.z);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00030BE5 File Offset: 0x0002EDE5
		public static Vector3i Convert(Vector3 floatVector)
		{
			return Vector3i.Convert(floatVector.x, floatVector.y, floatVector.z);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00085280 File Offset: 0x00083480
		public static Vector3i Convert(float x, float y, float z)
		{
			int num = (int)((x > 0f) ? (x + 0.5f) : (x - 0.5f));
			int num2 = (int)((y > 0f) ? (y + 0.5f) : (y - 0.5f));
			int num3 = (int)((z > 0f) ? (z + 0.5f) : (z - 0.5f));
			return new Vector3i(num, num2, num3);
		}

		// Token: 0x04000A9B RID: 2715
		public int x;

		// Token: 0x04000A9C RID: 2716
		public int y;

		// Token: 0x04000A9D RID: 2717
		public int z;

		// Token: 0x04000A9E RID: 2718
		public static readonly Vector3i zero = new Vector3i(0, 0, 0);

		// Token: 0x04000A9F RID: 2719
		public static readonly Vector3i one = new Vector3i(1, 1, 1);

		// Token: 0x04000AA0 RID: 2720
		public static readonly Vector3i forward = new Vector3i(0, 0, 1);

		// Token: 0x04000AA1 RID: 2721
		public static readonly Vector3i back = new Vector3i(0, 0, -1);

		// Token: 0x04000AA2 RID: 2722
		public static readonly Vector3i up = new Vector3i(0, 1, 0);

		// Token: 0x04000AA3 RID: 2723
		public static readonly Vector3i down = new Vector3i(0, -1, 0);

		// Token: 0x04000AA4 RID: 2724
		public static readonly Vector3i left = new Vector3i(-1, 0, 0);

		// Token: 0x04000AA5 RID: 2725
		public static readonly Vector3i right = new Vector3i(1, 0, 0);

		// Token: 0x04000AA6 RID: 2726
		public static readonly Vector3i[] directions = new Vector3i[]
		{
			Vector3i.left,
			Vector3i.right,
			Vector3i.back,
			Vector3i.forward,
			Vector3i.down,
			Vector3i.up
		};
	}
}
