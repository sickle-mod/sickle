using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001A RID: 26
[AddComponentMenu("UI/Effects/Gradient")]
public class GradientText : BaseMeshEffect
{
	// Token: 0x06000069 RID: 105 RVA: 0x00053774 File Offset: 0x00051974
	public override void ModifyMesh(VertexHelper helper)
	{
		if (!this.IsActive() || helper.currentVertCount == 0)
		{
			return;
		}
		List<UIVertex> list = new List<UIVertex>();
		helper.GetUIVertexStream(list);
		float num = list[0].position.y;
		float num2 = list[0].position.y;
		for (int i = 1; i < list.Count; i++)
		{
			float y = list[i].position.y;
			if (y > num2)
			{
				num2 = y;
			}
			else if (y < num)
			{
				num = y;
			}
		}
		float num3 = num2 - num;
		UIVertex uivertex = default(UIVertex);
		for (int j = 0; j < helper.currentVertCount; j++)
		{
			helper.PopulateUIVertex(ref uivertex, j);
			uivertex.color = Color32.Lerp(this.bottomColor, this.topColor, (uivertex.position.y - num) / num3);
			helper.SetUIVertex(uivertex, j);
		}
	}

	// Token: 0x04000065 RID: 101
	public Color32 topColor = Color.white;

	// Token: 0x04000066 RID: 102
	public Color32 bottomColor = Color.black;
}
