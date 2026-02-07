using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class FadeInDistance : MonoBehaviour
{
	// Token: 0x06000067 RID: 103 RVA: 0x000536F4 File Offset: 0x000518F4
	private void Update()
	{
		float num = (base.transform.position - this.target.position).magnitude + this.distanceModifier;
		num *= this.distanceScaleMultiplier;
		if (num > 1f)
		{
			num = 1f;
		}
		if (num < 0f)
		{
			num = 0f;
		}
		Color color = this.sprite.color;
		color.a = num;
		this.sprite.color = color;
	}

	// Token: 0x04000061 RID: 97
	public Transform target;

	// Token: 0x04000062 RID: 98
	public float distanceScaleMultiplier = 1f;

	// Token: 0x04000063 RID: 99
	public float distanceModifier;

	// Token: 0x04000064 RID: 100
	public SpriteRenderer sprite;
}
