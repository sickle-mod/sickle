using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008F3 RID: 2291
	[Serializable]
	public class ShowcaseTile
	{
		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06003E39 RID: 15929 RVA: 0x000500B9 File Offset: 0x0004E2B9
		// (set) Token: 0x06003E3A RID: 15930 RVA: 0x000500C1 File Offset: 0x0004E2C1
		public int Width { get; private set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06003E3B RID: 15931 RVA: 0x000500CA File Offset: 0x0004E2CA
		// (set) Token: 0x06003E3C RID: 15932 RVA: 0x000500D2 File Offset: 0x0004E2D2
		public int Height { get; private set; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x000500DB File Offset: 0x0004E2DB
		// (set) Token: 0x06003E3E RID: 15934 RVA: 0x000500E3 File Offset: 0x0004E2E3
		public string ImageUrl { get; private set; }

		// Token: 0x06003E3F RID: 15935 RVA: 0x000500EC File Offset: 0x0004E2EC
		public ShowcaseTile(ApiShowcaseProduct.Tile raw)
		{
			this.Width = raw.width;
			this.Height = raw.height;
			this.ImageUrl = raw.image_url;
		}
	}
}
