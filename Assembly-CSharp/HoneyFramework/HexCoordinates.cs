using System;
using Scythe.BoardPresenter;
using UnityEngine;

namespace HoneyFramework
{
	// Token: 0x020001BC RID: 444
	public class HexCoordinates
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x00084C28 File Offset: 0x00082E28
		public static Vector3 WorldToHex(Vector3 pos)
		{
			float num = 0.6666667f;
			float num2 = 0.33333334f;
			float num3 = num2 * Mathf.Sqrt(3f);
			float num4 = num * pos.x / GameHexPresenter.hexRadius;
			float num5 = (num3 * pos.z - num2 * pos.x) / GameHexPresenter.hexRadius;
			float num6 = -num4 - num5;
			return new Vector3(num4, num5, num6);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x000308CE File Offset: 0x0002EACE
		public static Vector2 HexToWorld(Vector3i pos)
		{
			return GameHexPresenter.GetDirX() * (float)pos.x + GameHexPresenter.GetDirY() * (float)pos.y + GameHexPresenter.GetDirZ() * (float)pos.z;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0003090D File Offset: 0x0002EB0D
		public static Vector2 HexToWorld(Vector3 pos)
		{
			return GameHexPresenter.GetDirX() * pos.x + GameHexPresenter.GetDirY() * pos.y + GameHexPresenter.GetDirZ() * pos.z;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00084C84 File Offset: 0x00082E84
		public static Vector3 HexToWorld3D(Vector3i pos)
		{
			Vector2 vector = HexCoordinates.HexToWorld(pos);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00084CB0 File Offset: 0x00082EB0
		public static Vector3 HexToWorld3D(Vector3 pos)
		{
			Vector2 vector = HexCoordinates.HexToWorld(pos);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00030949 File Offset: 0x0002EB49
		public static Vector3i GetHexCoordAt(Vector3 worldPos)
		{
			return HexCoordinates.CustomRoundingForHexes(HexCoordinates.WorldToHex(worldPos));
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00030956 File Offset: 0x0002EB56
		public static Vector3i GetHexCoordAt(Vector2 worldPos)
		{
			return HexCoordinates.GetHexCoordAt(VectorUtils.Vector2To3D(worldPos));
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00084CDC File Offset: 0x00082EDC
		public static Vector3i CustomRoundingForHexes(Vector3 position)
		{
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			int num3 = Mathf.RoundToInt(position.z);
			float num4 = Mathf.Abs((float)num - position.x);
			float num5 = Mathf.Abs((float)num2 - position.y);
			float num6 = Mathf.Abs((float)num3 - position.z);
			if (num6 > num5 && num6 > num4)
			{
				num3 = -num - num2;
			}
			else if (num5 > num4)
			{
				num2 = -num - num3;
			}
			else
			{
				num = -num2 - num3;
			}
			return new Vector3i(num, num2, num3);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00084D68 File Offset: 0x00082F68
		public static int HexDistance(Vector3i hexA, Vector3i hexB)
		{
			return Mathf.Max(new int[]
			{
				Mathf.Abs(hexB.x - hexA.x),
				Mathf.Abs(hexB.y - hexA.y),
				Mathf.Abs(hexB.z - hexA.z)
			});
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00084DC0 File Offset: 0x00082FC0
		public static Vector3i BoardToHexesPosition(int posX, int posY)
		{
			int num = 4 - posY;
			int num2 = posY - posX - Mathf.CeilToInt((float)posY / 2f) + 2 - ((posY % 2 == 1) ? 0 : 1);
			int num3 = -(num + num2);
			return new Vector3i(num, num2, num3);
		}
	}
}
