using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200076A RID: 1898
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06003765 RID: 14181 RVA: 0x0004B964 File Offset: 0x00049B64
		// (set) Token: 0x06003766 RID: 14182 RVA: 0x0004B96C File Offset: 0x00049B6C
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x0004B975 File Offset: 0x00049B75
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x0400296D RID: 10605
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x0200076B RID: 1899
		public enum SSRResolution
		{
			// Token: 0x0400296F RID: 10607
			High,
			// Token: 0x04002970 RID: 10608
			Low = 2
		}

		// Token: 0x0200076C RID: 1900
		public enum SSRReflectionBlendType
		{
			// Token: 0x04002972 RID: 10610
			PhysicallyBased,
			// Token: 0x04002973 RID: 10611
			Additive
		}

		// Token: 0x0200076D RID: 1901
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x04002974 RID: 10612
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04002975 RID: 10613
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04002976 RID: 10614
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04002977 RID: 10615
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x0200076E RID: 1902
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x04002978 RID: 10616
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x04002979 RID: 10617
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x0400297A RID: 10618
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x0400297B RID: 10619
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x0400297C RID: 10620
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x0400297D RID: 10621
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x0400297E RID: 10622
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x0400297F RID: 10623
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x0200076F RID: 1903
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x04002980 RID: 10624
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x02000770 RID: 1904
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000440 RID: 1088
			// (get) Token: 0x06003769 RID: 14185 RVA: 0x00146A94 File Offset: 0x00144C94
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x04002981 RID: 10625
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x04002982 RID: 10626
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x04002983 RID: 10627
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
