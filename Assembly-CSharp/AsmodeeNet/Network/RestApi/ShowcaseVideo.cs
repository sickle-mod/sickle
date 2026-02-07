using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008F6 RID: 2294
	[Serializable]
	public class ShowcaseVideo
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x0005019C File Offset: 0x0004E39C
		// (set) Token: 0x06003E4B RID: 15947 RVA: 0x000501A4 File Offset: 0x0004E3A4
		public string VideoUrl { get; private set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x000501AD File Offset: 0x0004E3AD
		// (set) Token: 0x06003E4D RID: 15949 RVA: 0x000501B5 File Offset: 0x0004E3B5
		public string ThumbUrl { get; private set; }

		// Token: 0x06003E4E RID: 15950 RVA: 0x000501BE File Offset: 0x0004E3BE
		public ShowcaseVideo(ApiShowcaseProduct.Video raw)
		{
			this.VideoUrl = raw.video_url;
			this.ThumbUrl = raw.thumb_url;
		}
	}
}
