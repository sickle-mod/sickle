using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008CC RID: 2252
	[Serializable]
	public class ApiShowcaseProduct
	{
		// Token: 0x04002F27 RID: 12071
		public int id;

		// Token: 0x04002F28 RID: 12072
		public string name;

		// Token: 0x04002F29 RID: 12073
		public string description;

		// Token: 0x04002F2A RID: 12074
		public string status;

		// Token: 0x04002F2B RID: 12075
		public string icon_url;

		// Token: 0x04002F2C RID: 12076
		public string banner_url;

		// Token: 0x04002F2D RID: 12077
		public string shop_digital_url;

		// Token: 0x04002F2E RID: 12078
		public string shop_physical_url;

		// Token: 0x04002F2F RID: 12079
		public ApiShowcaseProduct.Tile tile;

		// Token: 0x04002F30 RID: 12080
		public ApiShowcaseProduct.Image[] images;

		// Token: 0x04002F31 RID: 12081
		public ApiShowcaseProduct.Video[] videos;

		// Token: 0x04002F32 RID: 12082
		public ApiShowcaseProduct.Award[] awards;

		// Token: 0x020008CD RID: 2253
		[Serializable]
		public class Tile
		{
			// Token: 0x04002F33 RID: 12083
			public int width;

			// Token: 0x04002F34 RID: 12084
			public int height;

			// Token: 0x04002F35 RID: 12085
			public string image_url;
		}

		// Token: 0x020008CE RID: 2254
		[Serializable]
		public class Award
		{
			// Token: 0x04002F36 RID: 12086
			public string name;

			// Token: 0x04002F37 RID: 12087
			public string image_url;
		}

		// Token: 0x020008CF RID: 2255
		[Serializable]
		public class Image
		{
			// Token: 0x04002F38 RID: 12088
			public string image_url;

			// Token: 0x04002F39 RID: 12089
			public string thumb_url;
		}

		// Token: 0x020008D0 RID: 2256
		[Serializable]
		public class Video
		{
			// Token: 0x04002F3A RID: 12090
			public string video_url;

			// Token: 0x04002F3B RID: 12091
			public string thumb_url;
		}
	}
}
