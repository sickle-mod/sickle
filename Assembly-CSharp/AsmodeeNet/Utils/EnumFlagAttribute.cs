using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000848 RID: 2120
	public class EnumFlagAttribute : PropertyAttribute
	{
		// Token: 0x06003BF7 RID: 15351 RVA: 0x0004AEAD File Offset: 0x000490AD
		public EnumFlagAttribute()
		{
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x0004ED39 File Offset: 0x0004CF39
		public EnumFlagAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04002D95 RID: 11669
		public string name;
	}
}
