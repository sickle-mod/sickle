using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000711 RID: 1809
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x0600366D RID: 13933 RVA: 0x0004AE9E File Offset: 0x0004909E
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x040027D2 RID: 10194
		public readonly string method;
	}
}
