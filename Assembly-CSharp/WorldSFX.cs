using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000115 RID: 277
[CreateAssetMenu(fileName = "NewWorldSFX", menuName = "SFXData")]
public class WorldSFX : ScriptableObject
{
	// Token: 0x04000815 RID: 2069
	public List<SFXReference> sfxSounds = new List<SFXReference>();
}
