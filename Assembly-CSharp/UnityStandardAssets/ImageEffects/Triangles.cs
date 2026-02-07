using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006DF RID: 1759
	internal class Triangles
	{
		// Token: 0x06003569 RID: 13673 RVA: 0x0013F24C File Offset: 0x0013D44C
		private static bool HasMeshes()
		{
			if (Triangles.meshes == null)
			{
				return false;
			}
			for (int i = 0; i < Triangles.meshes.Length; i++)
			{
				if (null == Triangles.meshes[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x0013F288 File Offset: 0x0013D488
		private static void Cleanup()
		{
			if (Triangles.meshes == null)
			{
				return;
			}
			for (int i = 0; i < Triangles.meshes.Length; i++)
			{
				if (null != Triangles.meshes[i])
				{
					global::UnityEngine.Object.DestroyImmediate(Triangles.meshes[i]);
					Triangles.meshes[i] = null;
				}
			}
			Triangles.meshes = null;
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x0013F2D8 File Offset: 0x0013D4D8
		private static Mesh[] GetMeshes(int totalWidth, int totalHeight)
		{
			if (Triangles.HasMeshes() && Triangles.currentTris == totalWidth * totalHeight)
			{
				return Triangles.meshes;
			}
			int num = 21666;
			int num2 = totalWidth * totalHeight;
			Triangles.currentTris = num2;
			Triangles.meshes = new Mesh[Mathf.CeilToInt(1f * (float)num2 / (1f * (float)num))];
			int num3 = 0;
			for (int i = 0; i < num2; i += num)
			{
				int num4 = Mathf.FloorToInt((float)Mathf.Clamp(num2 - i, 0, num));
				Triangles.meshes[num3] = Triangles.GetMesh(num4, i, totalWidth, totalHeight);
				num3++;
			}
			return Triangles.meshes;
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x0013F368 File Offset: 0x0013D568
		private static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			Vector3[] array = new Vector3[triCount * 3];
			Vector2[] array2 = new Vector2[triCount * 3];
			Vector2[] array3 = new Vector2[triCount * 3];
			int[] array4 = new int[triCount * 3];
			for (int i = 0; i < triCount; i++)
			{
				int num = i * 3;
				int num2 = triOffset + i;
				float num3 = Mathf.Floor((float)(num2 % totalWidth)) / (float)totalWidth;
				float num4 = Mathf.Floor((float)(num2 / totalWidth)) / (float)totalHeight;
				Vector3 vector = new Vector3(num3 * 2f - 1f, num4 * 2f - 1f, 1f);
				array[num] = vector;
				array[num + 1] = vector;
				array[num + 2] = vector;
				array2[num] = new Vector2(0f, 0f);
				array2[num + 1] = new Vector2(1f, 0f);
				array2[num + 2] = new Vector2(0f, 1f);
				array3[num] = new Vector2(num3, num4);
				array3[num + 1] = new Vector2(num3, num4);
				array3[num + 2] = new Vector2(num3, num4);
				array4[num] = num;
				array4[num + 1] = num + 1;
				array4[num + 2] = num + 2;
			}
			mesh.vertices = array;
			mesh.triangles = array4;
			mesh.uv = array2;
			mesh.uv2 = array3;
			return mesh;
		}

		// Token: 0x04002706 RID: 9990
		private static Mesh[] meshes;

		// Token: 0x04002707 RID: 9991
		private static int currentTris;
	}
}
