using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B1 RID: 1713
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Camera Motion Blur")]
	public class CameraMotionBlur : PostEffectsBase
	{
		// Token: 0x060034C3 RID: 13507 RVA: 0x00138384 File Offset: 0x00136584
		private void CalculateViewProjection()
		{
			Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
			Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
			this.currentViewProjMat = gpuprojectionMatrix * worldToCameraMatrix;
			if (this._camera.stereoEnabled)
			{
				for (int i = 0; i < 2; i++)
				{
					Matrix4x4 stereoViewMatrix = this._camera.GetStereoViewMatrix((i == 0) ? Camera.StereoscopicEye.Left : Camera.StereoscopicEye.Right);
					Matrix4x4 matrix4x = this._camera.GetStereoProjectionMatrix((i == 0) ? Camera.StereoscopicEye.Left : Camera.StereoscopicEye.Right);
					matrix4x = GL.GetGPUProjectionMatrix(matrix4x, true);
					this.currentStereoViewProjMat[i] = matrix4x * stereoViewMatrix;
				}
			}
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x0013841C File Offset: 0x0013661C
		private new void Start()
		{
			this.CheckResources();
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			this.currentStereoViewProjMat = new Matrix4x4[2];
			this.prevStereoViewProjMat = new Matrix4x4[2];
			this.CalculateViewProjection();
			this.Remember();
			this.wasActive = false;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x0004969E File Offset: 0x0004789E
		private void OnEnable()
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this._camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00138488 File Offset: 0x00136688
		private void OnDisable()
		{
			if (null != this.motionBlurMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.motionBlurMaterial);
				this.motionBlurMaterial = null;
			}
			if (null != this.dx11MotionBlurMaterial)
			{
				global::UnityEngine.Object.DestroyImmediate(this.dx11MotionBlurMaterial);
				this.dx11MotionBlurMaterial = null;
			}
			if (null != this.tmpCam)
			{
				global::UnityEngine.Object.DestroyImmediate(this.tmpCam);
				this.tmpCam = null;
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x001384F8 File Offset: 0x001366F8
		public override bool CheckResources()
		{
			base.CheckSupport(true, true);
			this.motionBlurMaterial = base.CheckShaderAndCreateMaterial(this.shader, this.motionBlurMaterial);
			if (this.supportDX11 && this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11)
			{
				this.dx11MotionBlurMaterial = base.CheckShaderAndCreateMaterial(this.dx11MotionBlurShader, this.dx11MotionBlurMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x00138564 File Offset: 0x00136764
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				this.StartFrame();
			}
			RenderTextureFormat renderTextureFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.RGHalf : RenderTextureFormat.ARGBHalf);
			RenderTexture temporary = RenderTexture.GetTemporary(CameraMotionBlur.divRoundUp(source.width, this.velocityDownsample), CameraMotionBlur.divRoundUp(source.height, this.velocityDownsample), 0, renderTextureFormat);
			this.maxVelocity = Mathf.Max(2f, this.maxVelocity);
			float num = this.maxVelocity;
			bool flag = this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && this.dx11MotionBlurMaterial == null;
			int num2;
			int num3;
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag || this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.maxVelocity = Mathf.Min(this.maxVelocity, CameraMotionBlur.MAX_RADIUS);
				num2 = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				num3 = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				num = (float)(temporary.width / num2);
			}
			else
			{
				num2 = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				num3 = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				num = (float)(temporary.width / num2);
			}
			RenderTexture temporary2 = RenderTexture.GetTemporary(num2, num3, 0, renderTextureFormat);
			RenderTexture temporary3 = RenderTexture.GetTemporary(num2, num3, 0, renderTextureFormat);
			temporary.filterMode = FilterMode.Point;
			temporary2.filterMode = FilterMode.Point;
			temporary3.filterMode = FilterMode.Point;
			if (this.noiseTexture)
			{
				this.noiseTexture.filterMode = FilterMode.Point;
			}
			source.wrapMode = TextureWrapMode.Clamp;
			temporary.wrapMode = TextureWrapMode.Clamp;
			temporary3.wrapMode = TextureWrapMode.Clamp;
			temporary2.wrapMode = TextureWrapMode.Clamp;
			this.CalculateViewProjection();
			if (base.gameObject.activeInHierarchy && !this.wasActive)
			{
				this.Remember();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			Matrix4x4 matrix4x = Matrix4x4.Inverse(this.currentViewProjMat);
			this.motionBlurMaterial.SetMatrix("_InvViewProj", matrix4x);
			this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
			this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			if (this._camera.stereoEnabled)
			{
				Matrix4x4[] array = new Matrix4x4[]
				{
					Matrix4x4.Inverse(this.currentStereoViewProjMat[0]),
					Matrix4x4.Inverse(this.currentStereoViewProjMat[1])
				};
				Matrix4x4 matrix4x2 = this.prevStereoViewProjMat[0] * array[0];
				this.motionBlurMaterial.SetMatrix("_StereoToPrevViewProjCombined0", matrix4x2);
				this.motionBlurMaterial.SetMatrix("_StereoToPrevViewProjCombined1", this.prevStereoViewProjMat[1] * array[1]);
			}
			this.motionBlurMaterial.SetFloat("_MaxVelocity", num);
			this.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num);
			this.motionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
			this.motionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
			this.motionBlurMaterial.SetFloat("_Jitter", this.jitter);
			this.motionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
			this.motionBlurMaterial.SetTexture("_VelTex", temporary);
			this.motionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
			this.motionBlurMaterial.SetTexture("_TileTexDebug", temporary2);
			if (this.preview)
			{
				Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
				Matrix4x4 identity = Matrix4x4.identity;
				identity.SetTRS(this.previewScale * 0.3333f, Quaternion.identity, Vector3.one);
				Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
				this.prevViewProjMat = gpuprojectionMatrix * identity * worldToCameraMatrix;
				this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
				this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Vector4 zero = Vector4.zero;
				float num4 = Vector3.Dot(base.transform.up, Vector3.up);
				Vector3 vector = this.prevFramePos - base.transform.position;
				float magnitude = vector.magnitude;
				float num5 = Vector3.Angle(base.transform.up, this.prevFrameUp) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.x = this.rotationScale * num5;
				num5 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.y = this.rotationScale * num4 * num5;
				num5 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.z = this.rotationScale * (1f - num4) * num5;
				if (magnitude > Mathf.Epsilon && this.movementScale > Mathf.Epsilon)
				{
					zero.w = this.movementScale * Vector3.Dot(base.transform.forward, vector) * ((float)source.width * 0.5f);
					zero.x += this.movementScale * Vector3.Dot(base.transform.up, vector) * ((float)source.width * 0.5f);
					zero.y += this.movementScale * Vector3.Dot(base.transform.right, vector) * ((float)source.width * 0.5f);
				}
				if (this.preview)
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", new Vector4(this.previewScale.y, this.previewScale.x, 0f, this.previewScale.z) * 0.5f * this._camera.fieldOfView);
				}
				else
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
				}
			}
			else
			{
				Graphics.Blit(source, temporary, this.motionBlurMaterial, 0);
				Camera camera = null;
				if (this.excludeLayers.value != 0)
				{
					camera = this.GetTmpCam();
				}
				if (camera && this.excludeLayers.value != 0 && this.replacementClear && this.replacementClear.isSupported)
				{
					camera.targetTexture = temporary;
					camera.cullingMask = this.excludeLayers;
					camera.RenderWithShader(this.replacementClear, "");
				}
			}
			if (!this.preview && Time.frameCount != this.prevFrameCount)
			{
				this.prevFrameCount = Time.frameCount;
				this.Remember();
			}
			source.filterMode = FilterMode.Bilinear;
			if (this.showVelocity)
			{
				this.motionBlurMaterial.SetFloat("_DisplayVelocityScale", this.showVelocityScale);
				Graphics.Blit(temporary, destination, this.motionBlurMaterial, 1);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && !flag)
			{
				this.dx11MotionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
				this.dx11MotionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
				this.dx11MotionBlurMaterial.SetFloat("_Jitter", this.jitter);
				this.dx11MotionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.dx11MotionBlurMaterial.SetTexture("_VelTex", temporary);
				this.dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
				this.dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				this.dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num);
				Graphics.Blit(temporary, temporary2, this.dx11MotionBlurMaterial, 0);
				Graphics.Blit(temporary2, temporary3, this.dx11MotionBlurMaterial, 1);
				Graphics.Blit(source, destination, this.dx11MotionBlurMaterial, 2);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 4);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 6);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 7);
			}
			else
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 5);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x00138E74 File Offset: 0x00137074
		private void Remember()
		{
			this.prevViewProjMat = this.currentViewProjMat;
			this.prevFrameForward = base.transform.forward;
			this.prevFrameUp = base.transform.up;
			this.prevFramePos = base.transform.position;
			this.prevStereoViewProjMat[0] = this.currentStereoViewProjMat[0];
			this.prevStereoViewProjMat[1] = this.currentStereoViewProjMat[1];
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x00138EF0 File Offset: 0x001370F0
		private Camera GetTmpCam()
		{
			if (this.tmpCam == null)
			{
				string text = "_" + this._camera.name + "_MotionBlurTmpCam";
				GameObject gameObject = GameObject.Find(text);
				if (null == gameObject)
				{
					this.tmpCam = new GameObject(text, new Type[] { typeof(Camera) });
				}
				else
				{
					this.tmpCam = gameObject;
				}
			}
			this.tmpCam.hideFlags = HideFlags.DontSave;
			this.tmpCam.transform.position = this._camera.transform.position;
			this.tmpCam.transform.rotation = this._camera.transform.rotation;
			this.tmpCam.transform.localScale = this._camera.transform.localScale;
			this.tmpCam.GetComponent<Camera>().CopyFrom(this._camera);
			this.tmpCam.GetComponent<Camera>().enabled = false;
			this.tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
			this.tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
			return this.tmpCam.GetComponent<Camera>();
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000496CD File Offset: 0x000478CD
		private void StartFrame()
		{
			this.prevFramePos = Vector3.Slerp(this.prevFramePos, base.transform.position, 0.75f);
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000496F0 File Offset: 0x000478F0
		private static int divRoundUp(int x, int d)
		{
			return (x + d - 1) / d;
		}

		// Token: 0x0400258E RID: 9614
		private static float MAX_RADIUS = 10f;

		// Token: 0x0400258F RID: 9615
		public CameraMotionBlur.MotionBlurFilter filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction;

		// Token: 0x04002590 RID: 9616
		public bool preview;

		// Token: 0x04002591 RID: 9617
		public Vector3 previewScale = Vector3.one;

		// Token: 0x04002592 RID: 9618
		public float movementScale;

		// Token: 0x04002593 RID: 9619
		public float rotationScale = 1f;

		// Token: 0x04002594 RID: 9620
		public float maxVelocity = 8f;

		// Token: 0x04002595 RID: 9621
		public float minVelocity = 0.1f;

		// Token: 0x04002596 RID: 9622
		public float velocityScale = 0.375f;

		// Token: 0x04002597 RID: 9623
		public float softZDistance = 0.005f;

		// Token: 0x04002598 RID: 9624
		public int velocityDownsample = 1;

		// Token: 0x04002599 RID: 9625
		public LayerMask excludeLayers = 0;

		// Token: 0x0400259A RID: 9626
		private GameObject tmpCam;

		// Token: 0x0400259B RID: 9627
		public Shader shader;

		// Token: 0x0400259C RID: 9628
		public Shader dx11MotionBlurShader;

		// Token: 0x0400259D RID: 9629
		public Shader replacementClear;

		// Token: 0x0400259E RID: 9630
		private Material motionBlurMaterial;

		// Token: 0x0400259F RID: 9631
		private Material dx11MotionBlurMaterial;

		// Token: 0x040025A0 RID: 9632
		public Texture2D noiseTexture;

		// Token: 0x040025A1 RID: 9633
		public float jitter = 0.05f;

		// Token: 0x040025A2 RID: 9634
		public bool showVelocity;

		// Token: 0x040025A3 RID: 9635
		public float showVelocityScale = 1f;

		// Token: 0x040025A4 RID: 9636
		private Matrix4x4 currentViewProjMat;

		// Token: 0x040025A5 RID: 9637
		private Matrix4x4[] currentStereoViewProjMat;

		// Token: 0x040025A6 RID: 9638
		private Matrix4x4 prevViewProjMat;

		// Token: 0x040025A7 RID: 9639
		private Matrix4x4[] prevStereoViewProjMat;

		// Token: 0x040025A8 RID: 9640
		private int prevFrameCount;

		// Token: 0x040025A9 RID: 9641
		private bool wasActive;

		// Token: 0x040025AA RID: 9642
		private Vector3 prevFrameForward = Vector3.forward;

		// Token: 0x040025AB RID: 9643
		private Vector3 prevFrameUp = Vector3.up;

		// Token: 0x040025AC RID: 9644
		private Vector3 prevFramePos = Vector3.zero;

		// Token: 0x040025AD RID: 9645
		private Camera _camera;

		// Token: 0x020006B2 RID: 1714
		public enum MotionBlurFilter
		{
			// Token: 0x040025AF RID: 9647
			CameraMotion,
			// Token: 0x040025B0 RID: 9648
			LocalBlur,
			// Token: 0x040025B1 RID: 9649
			Reconstruction,
			// Token: 0x040025B2 RID: 9650
			ReconstructionDX11,
			// Token: 0x040025B3 RID: 9651
			ReconstructionDisc
		}
	}
}
