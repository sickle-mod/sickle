using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class UnitLighting : MonoBehaviour
{
	// Token: 0x06000071 RID: 113 RVA: 0x000283DC File Offset: 0x000265DC
	public void SetBulbs(bool on)
	{
		this.bulbContainer.SetActive(on);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x000283EA File Offset: 0x000265EA
	public void SetHighlight(bool on)
	{
		this.highlightsContainer.SetActive(on);
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00053A1C File Offset: 0x00051C1C
	public void SetColor(Color col)
	{
		for (int i = 0; i < this.colorLight.Length; i++)
		{
			this.colorLight[i].color = col;
		}
		for (int j = 0; j < this.colorMesh.Length; j++)
		{
			this.colorMesh[j].material.color = col;
			this.colorMesh[j].material.SetColor("_EmissionColor", col);
		}
	}

	// Token: 0x0400006D RID: 109
	public GameObject highlightsContainer;

	// Token: 0x0400006E RID: 110
	public GameObject bulbContainer;

	// Token: 0x0400006F RID: 111
	public Light[] colorLight;

	// Token: 0x04000070 RID: 112
	public MeshRenderer[] colorMesh;
}
