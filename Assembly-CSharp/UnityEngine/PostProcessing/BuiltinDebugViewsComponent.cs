using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000718 RID: 1816
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x0004AFA5 File Offset: 0x000491A5
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x00141DBC File Offset: 0x0013FFBC
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			switch (mode)
			{
			case BuiltinDebugViewsModel.Mode.Depth:
				depthTextureMode |= DepthTextureMode.Depth;
				break;
			case BuiltinDebugViewsModel.Mode.Normals:
				depthTextureMode |= DepthTextureMode.DepthNormals;
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
				break;
			}
			return depthTextureMode;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x0004AFD1 File Offset: 0x000491D1
		public override CameraEvent GetCameraEvent()
		{
			if (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors)
			{
				return CameraEvent.BeforeImageEffectsOpaque;
			}
			return CameraEvent.BeforeImageEffects;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x0004AFEB File Offset: 0x000491EB
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x00141E04 File Offset: 0x00140004
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			ref BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			switch (settings.mode)
			{
			case BuiltinDebugViewsModel.Mode.Depth:
				this.DepthPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.Normals:
				this.DepthNormalsPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				this.MotionVectorsPass(cb);
				break;
			}
			this.context.Interrupt();
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00141E94 File Offset: 0x00140094
		private void DepthPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, material, 0);
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x00141EF0 File Offset: 0x001400F0
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, material, 1);
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x00141F24 File Offset: 0x00140124
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int num = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(num, this.context.width, this.context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, num, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
				cb.Blit(num, tempRT, material, 3);
				cb.ReleaseTemporaryRT(num);
				num = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num2 = 1f / (float)motionVectors.motionVectorsResolution;
				float num3 = num2 * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(num3, num2));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
			cb.Blit(num, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(num);
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00142118 File Offset: 0x00140318
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x0004AFF2 File Offset: 0x000491F2
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x040027F1 RID: 10225
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x040027F2 RID: 10226
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x02000719 RID: 1817
		private static class Uniforms
		{
			// Token: 0x040027F3 RID: 10227
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x040027F4 RID: 10228
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x040027F5 RID: 10229
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x040027F6 RID: 10230
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x040027F7 RID: 10231
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x040027F8 RID: 10232
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x040027F9 RID: 10233
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x0200071A RID: 1818
		private enum Pass
		{
			// Token: 0x040027FB RID: 10235
			Depth,
			// Token: 0x040027FC RID: 10236
			Normals,
			// Token: 0x040027FD RID: 10237
			MovecOpacity,
			// Token: 0x040027FE RID: 10238
			MovecImaging,
			// Token: 0x040027FF RID: 10239
			MovecArrows
		}

		// Token: 0x0200071B RID: 1819
		private class ArrowArray
		{
			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x06003688 RID: 13960 RVA: 0x0004B016 File Offset: 0x00049216
			// (set) Token: 0x06003689 RID: 13961 RVA: 0x0004B01E File Offset: 0x0004921E
			public Mesh mesh { get; private set; }

			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x0600368A RID: 13962 RVA: 0x0004B027 File Offset: 0x00049227
			// (set) Token: 0x0600368B RID: 13963 RVA: 0x0004B02F File Offset: 0x0004922F
			public int columnCount { get; private set; }

			// Token: 0x17000405 RID: 1029
			// (get) Token: 0x0600368C RID: 13964 RVA: 0x0004B038 File Offset: 0x00049238
			// (set) Token: 0x0600368D RID: 13965 RVA: 0x0004B040 File Offset: 0x00049240
			public int rowCount { get; private set; }

			// Token: 0x0600368E RID: 13966 RVA: 0x00142208 File Offset: 0x00140408
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 vector = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(vector);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, MeshTopology.Lines, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x0600368F RID: 13967 RVA: 0x0004B049 File Offset: 0x00049249
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
