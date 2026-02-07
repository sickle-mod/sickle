using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008F1 RID: 2289
	[Serializable]
	public class ShowcaseProduct
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x0004FFBC File Offset: 0x0004E1BC
		// (set) Token: 0x06003E1B RID: 15899 RVA: 0x0004FFC4 File Offset: 0x0004E1C4
		public int Id { get; private set; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06003E1C RID: 15900 RVA: 0x0004FFCD File Offset: 0x0004E1CD
		// (set) Token: 0x06003E1D RID: 15901 RVA: 0x0004FFD5 File Offset: 0x0004E1D5
		public string Name { get; private set; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06003E1E RID: 15902 RVA: 0x0004FFDE File Offset: 0x0004E1DE
		// (set) Token: 0x06003E1F RID: 15903 RVA: 0x0004FFE6 File Offset: 0x0004E1E6
		public string Description { get; private set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06003E20 RID: 15904 RVA: 0x0004FFEF File Offset: 0x0004E1EF
		// (set) Token: 0x06003E21 RID: 15905 RVA: 0x0004FFF7 File Offset: 0x0004E1F7
		public ProductStatus Status { get; private set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06003E22 RID: 15906 RVA: 0x00050000 File Offset: 0x0004E200
		// (set) Token: 0x06003E23 RID: 15907 RVA: 0x00050008 File Offset: 0x0004E208
		public string IconUrl { get; private set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06003E24 RID: 15908 RVA: 0x00050011 File Offset: 0x0004E211
		// (set) Token: 0x06003E25 RID: 15909 RVA: 0x00050019 File Offset: 0x0004E219
		public string BannerUrl { get; private set; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06003E26 RID: 15910 RVA: 0x00050022 File Offset: 0x0004E222
		// (set) Token: 0x06003E27 RID: 15911 RVA: 0x0005002A File Offset: 0x0004E22A
		public string ShopDigitalUrl { get; private set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06003E28 RID: 15912 RVA: 0x00050033 File Offset: 0x0004E233
		// (set) Token: 0x06003E29 RID: 15913 RVA: 0x0005003B File Offset: 0x0004E23B
		public string ShopPhysicalUrl { get; private set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06003E2A RID: 15914 RVA: 0x00050044 File Offset: 0x0004E244
		// (set) Token: 0x06003E2B RID: 15915 RVA: 0x0005004C File Offset: 0x0004E24C
		public ShowcaseTile Tile { get; private set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06003E2C RID: 15916 RVA: 0x00050055 File Offset: 0x0004E255
		// (set) Token: 0x06003E2D RID: 15917 RVA: 0x0005005D File Offset: 0x0004E25D
		public ShowcaseImage[] Images { get; private set; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06003E2E RID: 15918 RVA: 0x00050066 File Offset: 0x0004E266
		// (set) Token: 0x06003E2F RID: 15919 RVA: 0x0005006E File Offset: 0x0004E26E
		public ShowcaseVideo[] Videos { get; private set; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06003E30 RID: 15920 RVA: 0x00050077 File Offset: 0x0004E277
		// (set) Token: 0x06003E31 RID: 15921 RVA: 0x0005007F File Offset: 0x0004E27F
		public ShowcaseAward[] Awards { get; private set; }

		// Token: 0x06003E32 RID: 15922 RVA: 0x00158E44 File Offset: 0x00157044
		public ShowcaseProduct(ApiShowcaseProduct raw)
		{
			this.jsonProduct = raw;
			this.Id = raw.id;
			this.Name = raw.name;
			this.Description = raw.description;
			if (raw.status != null)
			{
				this.Status = (ProductStatus)Enum.Parse(typeof(ProductStatus), raw.status);
			}
			this.IconUrl = raw.icon_url;
			this.BannerUrl = raw.banner_url;
			this.ShopDigitalUrl = raw.shop_digital_url;
			this.ShopPhysicalUrl = raw.shop_physical_url;
			this.Tile = new ShowcaseTile(raw.tile);
			this.Images = raw.images.Select((ApiShowcaseProduct.Image x) => new ShowcaseImage(x)).ToArray<ShowcaseImage>();
			this.Videos = raw.videos.Select((ApiShowcaseProduct.Video x) => new ShowcaseVideo(x)).ToArray<ShowcaseVideo>();
			this.Awards = raw.awards.Select((ApiShowcaseProduct.Award x) => new ShowcaseAward(x)).ToArray<ShowcaseAward>();
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00050088 File Offset: 0x0004E288
		public string ToJson()
		{
			return JsonUtility.ToJson(this.jsonProduct);
		}

		// Token: 0x04002FCE RID: 12238
		private ApiShowcaseProduct jsonProduct;
	}
}
