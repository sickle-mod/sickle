using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008F5 RID: 2293
	[Serializable]
	public class ShowcaseImage
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x0005015A File Offset: 0x0004E35A
		// (set) Token: 0x06003E46 RID: 15942 RVA: 0x00050162 File Offset: 0x0004E362
		public string ImageUrl { get; private set; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0005016B File Offset: 0x0004E36B
		// (set) Token: 0x06003E48 RID: 15944 RVA: 0x00050173 File Offset: 0x0004E373
		public string ThumbUrl { get; private set; }

		// Token: 0x06003E49 RID: 15945 RVA: 0x0005017C File Offset: 0x0004E37C
		public ShowcaseImage(ApiShowcaseProduct.Image raw)
		{
			this.ImageUrl = raw.image_url;
			this.ThumbUrl = raw.thumb_url;
		}
	}
}
