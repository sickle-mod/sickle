using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C5 RID: 1733
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Global Fog")]
	internal class GlobalFog : PostEffectsBase
	{
		// Token: 0x06003513 RID: 13587 RVA: 0x00049A84 File Offset: 0x00047C84
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.fogMaterial = base.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x0013C0EC File Offset: 0x0013A2EC
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || (!this.distanceFog && !this.heightFog))
			{
				Graphics.Blit(source, destination);
				return;
			}
			Camera component = base.GetComponent<Camera>();
			Transform transform = component.transform;
			Vector3[] array = new Vector3[4];
			component.CalculateFrustumCorners(new Rect(0f, 0f, 1f, 1f), component.farClipPlane, component.stereoActiveEye, array);
			Vector3 vector = transform.TransformVector(array[0]);
			Vector3 vector2 = transform.TransformVector(array[1]);
			Vector3 vector3 = transform.TransformVector(array[2]);
			Vector3 vector4 = transform.TransformVector(array[3]);
			Matrix4x4 identity = Matrix4x4.identity;
			identity.SetRow(0, vector);
			identity.SetRow(1, vector4);
			identity.SetRow(2, vector2);
			identity.SetRow(3, vector3);
			Vector3 position = transform.position;
			float num = position.y - this.height;
			float num2 = ((num <= 0f) ? 1f : 0f);
			float num3 = (this.excludeFarPixels ? 1f : 2f);
			this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
			this.fogMaterial.SetVector("_CameraWS", position);
			this.fogMaterial.SetVector("_HeightParams", new Vector4(this.height, num, num2, this.heightDensity * 0.5f));
			this.fogMaterial.SetVector("_DistanceParams", new Vector4(-Mathf.Max(this.startDistance, 0f), num3, 0f, 0f));
			FogMode fogMode = RenderSettings.fogMode;
			float fogDensity = RenderSettings.fogDensity;
			float fogStartDistance = RenderSettings.fogStartDistance;
			float fogEndDistance = RenderSettings.fogEndDistance;
			bool flag = fogMode == FogMode.Linear;
			float num4 = (flag ? (fogEndDistance - fogStartDistance) : 0f);
			float num5 = ((Mathf.Abs(num4) > 0.0001f) ? (1f / num4) : 0f);
			Vector4 vector5;
			vector5.x = fogDensity * 1.2011224f;
			vector5.y = fogDensity * 1.442695f;
			vector5.z = (flag ? (-num5) : 0f);
			vector5.w = (flag ? (fogEndDistance * num5) : 0f);
			this.fogMaterial.SetVector("_SceneFogParams", vector5);
			this.fogMaterial.SetVector("_SceneFogMode", new Vector4((float)fogMode, (float)(this.useRadialDistance ? 1 : 0), 0f, 0f));
			int num6;
			if (this.distanceFog && this.heightFog)
			{
				num6 = 0;
			}
			else if (this.distanceFog)
			{
				num6 = 1;
			}
			else
			{
				num6 = 2;
			}
			Graphics.Blit(source, destination, this.fogMaterial, num6);
		}

		// Token: 0x04002660 RID: 9824
		[Tooltip("Apply distance-based fog?")]
		public bool distanceFog = true;

		// Token: 0x04002661 RID: 9825
		[Tooltip("Exclude far plane pixels from distance-based fog? (Skybox or clear color)")]
		public bool excludeFarPixels = true;

		// Token: 0x04002662 RID: 9826
		[Tooltip("Distance fog is based on radial distance from camera when checked")]
		public bool useRadialDistance;

		// Token: 0x04002663 RID: 9827
		[Tooltip("Apply height-based fog?")]
		public bool heightFog = true;

		// Token: 0x04002664 RID: 9828
		[Tooltip("Fog top Y coordinate")]
		public float height = 1f;

		// Token: 0x04002665 RID: 9829
		[Range(0.001f, 10f)]
		public float heightDensity = 2f;

		// Token: 0x04002666 RID: 9830
		[Tooltip("Push fog away from the camera by this amount")]
		public float startDistance;

		// Token: 0x04002667 RID: 9831
		public Shader fogShader;

		// Token: 0x04002668 RID: 9832
		private Material fogMaterial;
	}
}
