using System;

namespace AuthApi
{
	// Token: 0x020001C7 RID: 455
	public abstract class EndpointCollection
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00030E10 File Offset: 0x0002F010
		// (set) Token: 0x06000D60 RID: 3424 RVA: 0x00030E18 File Offset: 0x0002F018
		protected string _baseUrl { get; set; }

		// Token: 0x06000D61 RID: 3425 RVA: 0x00030E21 File Offset: 0x0002F021
		public EndpointCollection(string baseUrl)
		{
			this._baseUrl = baseUrl;
		}
	}
}
