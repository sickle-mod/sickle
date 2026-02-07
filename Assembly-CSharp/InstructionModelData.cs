using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000101 RID: 257
[CreateAssetMenu(fileName = "InstructionModelData", menuName = "Data/InstructionModelData")]
[Serializable]
public class InstructionModelData : ScriptableObject
{
	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600086A RID: 2154 RVA: 0x0002DAEF File Offset: 0x0002BCEF
	// (set) Token: 0x0600086B RID: 2155 RVA: 0x0002DAF7 File Offset: 0x0002BCF7
	public IDictionary<InstructionLanguage, string> EnumInstructionURLDictionary
	{
		get
		{
			return this.instructionURLDictionary;
		}
		set
		{
			this.instructionURLDictionary.CopyFrom(value);
		}
	}

	// Token: 0x04000714 RID: 1812
	public EnumInstructionModelDictionary instructionURLDictionary;
}
