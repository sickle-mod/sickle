using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000102 RID: 258
[CreateAssetMenu(fileName = "ReplaceFileModelData", menuName = "Data/ReplaceFileModelData")]
[Serializable]
public class ReplaceFileModelData : ScriptableObject
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x0002DB0D File Offset: 0x0002BD0D
	// (set) Token: 0x0600086E RID: 2158 RVA: 0x0002DB15 File Offset: 0x0002BD15
	public IDictionary<PlatformDLL, ReplaceFileModel> StringReplaceFileModelDictionary
	{
		get
		{
			return this.replaceFileModelDictionary;
		}
		set
		{
			this.replaceFileModelDictionary.CopyFrom(value);
		}
	}

	// Token: 0x04000715 RID: 1813
	public EnumReplaceFileModelDictionary replaceFileModelDictionary;
}
