using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000737 RID: 1847
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06003708 RID: 14088 RVA: 0x00145C98 File Offset: 0x00143E98
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x00145D08 File Offset: 0x00143F08
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x00145D8C File Offset: 0x00143F8C
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			GUI.DrawTexture(new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height), settings.lut);
		}

		// Token: 0x02000738 RID: 1848
		private static class Uniforms
		{
			// Token: 0x040028BF RID: 10431
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x040028C0 RID: 10432
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
