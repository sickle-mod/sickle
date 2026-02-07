using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class EditorGrid : MonoBehaviour
{
	// Token: 0x0600092B RID: 2347 RVA: 0x0007AD00 File Offset: 0x00078F00
	public void SetupOnGrid()
	{
		this.objectsIterator = 0;
		for (int i = 0; i < this.ySize; i++)
		{
			for (int j = 0; j < this.xSize; j++)
			{
				this.objectsToSetupOnGrid[this.objectsIterator].transform.position = new Vector3((float)j * this.xOffset, 0f, (float)i * this.yOffset);
				this.objectsIterator++;
			}
		}
	}

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private GameObject[] objectsToSetupOnGrid;

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private float xOffset = 1f;

	// Token: 0x04000866 RID: 2150
	[SerializeField]
	private float yOffset = 1f;

	// Token: 0x04000867 RID: 2151
	[SerializeField]
	private int xSize = 4;

	// Token: 0x04000868 RID: 2152
	[SerializeField]
	private int ySize = 4;

	// Token: 0x04000869 RID: 2153
	private int objectsIterator;
}
