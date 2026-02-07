using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class SimpleDustScript : MonoBehaviour
{
	// Token: 0x06000033 RID: 51 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void Step(string leg)
	{
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void HullGround()
	{
	}

	// Token: 0x04000031 RID: 49
	[SerializeField]
	private ParticleSystem legFR;

	// Token: 0x04000032 RID: 50
	[SerializeField]
	private ParticleSystem legFL;

	// Token: 0x04000033 RID: 51
	[SerializeField]
	private ParticleSystem legBR;

	// Token: 0x04000034 RID: 52
	[SerializeField]
	private ParticleSystem legBL;

	// Token: 0x04000035 RID: 53
	[SerializeField]
	private ParticleSystem hullDust;
}
