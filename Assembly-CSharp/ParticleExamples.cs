using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
[Serializable]
public class ParticleExamples
{
	// Token: 0x04000023 RID: 35
	public string title;

	// Token: 0x04000024 RID: 36
	[TextArea]
	public string description;

	// Token: 0x04000025 RID: 37
	public bool isWeaponEffect;

	// Token: 0x04000026 RID: 38
	public GameObject particleSystemGO;

	// Token: 0x04000027 RID: 39
	public Vector3 particlePosition;

	// Token: 0x04000028 RID: 40
	public Vector3 particleRotation;
}
