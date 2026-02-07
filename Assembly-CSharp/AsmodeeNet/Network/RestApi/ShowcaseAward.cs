using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008F4 RID: 2292
	[Serializable]
	public class ShowcaseAward
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06003E40 RID: 15936 RVA: 0x00050118 File Offset: 0x0004E318
		// (set) Token: 0x06003E41 RID: 15937 RVA: 0x00050120 File Offset: 0x0004E320
		public string Name { get; private set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x00050129 File Offset: 0x0004E329
		// (set) Token: 0x06003E43 RID: 15939 RVA: 0x00050131 File Offset: 0x0004E331
		public string ImageUrl { get; private set; }

		// Token: 0x06003E44 RID: 15940 RVA: 0x0005013A File Offset: 0x0004E33A
		public ShowcaseAward(ApiShowcaseProduct.Award raw)
		{
			this.Name = raw.name;
			this.ImageUrl = raw.image_url;
		}
	}
}
