using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
[RequireComponent(typeof(MeshCollider))]
public class CylinderCollider : MonoBehaviour
{
	// Token: 0x06000473 RID: 1139 RVA: 0x0002AD72 File Offset: 0x00028F72
	private void Awake()
	{
		this.cylinderCollider = base.GetComponent<MeshCollider>();
		this.GenerateCylinderCollider();
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0002AD86 File Offset: 0x00028F86
	public void Recalculate()
	{
		this.GenerateCylinderCollider();
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0006430C File Offset: 0x0006250C
	private void GenerateCylinderCollider()
	{
		Mesh mesh = new Mesh();
		Vector3[] basesVerticies = this.GetBasesVerticies(this.verticiesForBase, this.cylinderRadius, this.cylinderWidth);
		Vector2[] array = new Vector2[basesVerticies.Length];
		int[] array2 = this.CreateCylinderMesh(basesVerticies, this.verticiesForBase);
		mesh.vertices = basesVerticies;
		mesh.uv = array;
		mesh.triangles = array2;
		this.cylinderCollider.sharedMesh = mesh;
		this.cylinderCollider.convex = true;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0006437C File Offset: 0x0006257C
	private Vector3[] GetBasesVerticies(int verticiesForBase, float radius, float width)
	{
		Vector3[] array = new Vector3[verticiesForBase * 2];
		float num = width / 2f;
		Vector3 vector = default(Vector3);
		for (int i = 0; i < verticiesForBase; i++)
		{
			float num2 = 60f * (float)i + 30f;
			float num3 = 0.017453292f * num2;
			vector.z = radius * Mathf.Cos(num3);
			vector.y = radius * Mathf.Sin(num3);
			vector.x = num;
			array[i] = vector;
			vector.x = -num;
			array[i + verticiesForBase] = vector;
		}
		return array;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0006440C File Offset: 0x0006260C
	private int[] CreateCylinderMesh(Vector3[] verticies, int verticiesForBase)
	{
		int[] array = new int[verticiesForBase * 6];
		int num = 0;
		for (int i = 0; i < verticiesForBase; i++)
		{
			if (i == 0)
			{
				array[num * 3] = i;
				array[num * 3 + 1] = verticiesForBase - 1;
				array[num * 3 + 2] = 2 * verticiesForBase - 1;
				num++;
				array[num * 3] = i;
				array[num * 3 + 1] = 2 * verticiesForBase - 1;
				array[num * 3 + 2] = verticiesForBase;
				num++;
			}
			else
			{
				array[num * 3] = i;
				array[num * 3 + 1] = i - 1;
				array[num * 3 + 2] = verticiesForBase + i - 1;
				num++;
				array[num * 3] = i;
				array[num * 3 + 1] = verticiesForBase + i - 1;
				array[num * 3 + 2] = verticiesForBase + i;
				num++;
			}
		}
		return array;
	}

	// Token: 0x0400039E RID: 926
	public MeshCollider cylinderCollider;

	// Token: 0x0400039F RID: 927
	public int verticiesForBase = 12;

	// Token: 0x040003A0 RID: 928
	public float cylinderRadius = 0.34f;

	// Token: 0x040003A1 RID: 929
	public float cylinderWidth = 0.2f;
}
