using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200077B RID: 1915
	public class PostProcessingContext
	{
		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x0004BA9E File Offset: 0x00049C9E
		// (set) Token: 0x06003797 RID: 14231 RVA: 0x0004BAA6 File Offset: 0x00049CA6
		public bool interrupted { get; private set; }

		// Token: 0x06003798 RID: 14232 RVA: 0x0004BAAF File Offset: 0x00049CAF
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x0004BAB8 File Offset: 0x00049CB8
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600379A RID: 14234 RVA: 0x0004BADE File Offset: 0x00049CDE
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600379B RID: 14235 RVA: 0x0004BAEE File Offset: 0x00049CEE
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x0600379C RID: 14236 RVA: 0x0004BAFB File Offset: 0x00049CFB
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x0600379D RID: 14237 RVA: 0x0004BB08 File Offset: 0x00049D08
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600379E RID: 14238 RVA: 0x0004BB15 File Offset: 0x00049D15
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x040029B3 RID: 10675
		public PostProcessingProfile profile;

		// Token: 0x040029B4 RID: 10676
		public Camera camera;

		// Token: 0x040029B5 RID: 10677
		public MaterialFactory materialFactory;

		// Token: 0x040029B6 RID: 10678
		public RenderTextureFactory renderTextureFactory;
	}
}
