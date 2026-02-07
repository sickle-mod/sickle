using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x02000015 RID: 21
public class Blur2RT : BlurOptimized
{
	// Token: 0x06000050 RID: 80 RVA: 0x0002825A File Offset: 0x0002645A
	private new void Start()
	{
		base.Start();
		this.blurShader = Shader.Find("Hidden/FastBlur");
		if (this.blurShader == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000530F0 File Offset: 0x000512F0
	public new void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.renderTexture != null)
		{
			base.OnRenderImage(source, this.renderTexture);
		}
		if (this.renderTextureOnly)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (this.renderTexture == null)
		{
			base.OnRenderImage(source, destination);
			return;
		}
		Graphics.Blit(this.renderTexture, destination);
	}

	// Token: 0x04000043 RID: 67
	public RenderTexture renderTexture;

	// Token: 0x04000044 RID: 68
	public bool renderTextureOnly = true;
}
