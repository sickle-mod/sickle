using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000699 RID: 1689
	[ExecuteInEditMode]
	public class Water : MonoBehaviour
	{
		// Token: 0x0600348F RID: 13455 RVA: 0x0013593C File Offset: 0x00133B3C
		public void OnWillRenderObject()
		{
			if (!base.enabled || !base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || !base.GetComponent<Renderer>().enabled)
			{
				return;
			}
			Camera current = Camera.current;
			if (!current)
			{
				return;
			}
			if (Water.s_InsideWater)
			{
				return;
			}
			Water.s_InsideWater = true;
			this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
			Water.WaterMode waterMode = this.GetWaterMode();
			Camera camera;
			Camera camera2;
			this.CreateWaterObjects(current, out camera, out camera2);
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			int pixelLightCount = QualitySettings.pixelLightCount;
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = 0;
			}
			this.UpdateCameraModes(current, camera);
			this.UpdateCameraModes(current, camera2);
			if (waterMode >= Water.WaterMode.Reflective)
			{
				float num = -Vector3.Dot(up, position) - this.clipPlaneOffset;
				Vector4 vector = new Vector4(up.x, up.y, up.z, num);
				Matrix4x4 zero = Matrix4x4.zero;
				Water.CalculateReflectionMatrix(ref zero, vector);
				Vector3 position2 = current.transform.position;
				Vector3 vector2 = zero.MultiplyPoint(position2);
				camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
				Vector4 vector3 = this.CameraSpacePlane(camera, position, up, 1f);
				camera.projectionMatrix = current.CalculateObliqueMatrix(vector3);
				camera.cullingMatrix = current.projectionMatrix * current.worldToCameraMatrix;
				camera.cullingMask = -17 & this.reflectLayers.value;
				camera.targetTexture = this.m_ReflectionTexture;
				bool invertCulling = GL.invertCulling;
				GL.invertCulling = !invertCulling;
				camera.transform.position = vector2;
				Vector3 eulerAngles = current.transform.eulerAngles;
				camera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
				camera.Render();
				camera.transform.position = position2;
				GL.invertCulling = invertCulling;
				base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
			}
			if (waterMode >= Water.WaterMode.Refractive)
			{
				camera2.worldToCameraMatrix = current.worldToCameraMatrix;
				Vector4 vector4 = this.CameraSpacePlane(camera2, position, up, -1f);
				camera2.projectionMatrix = current.CalculateObliqueMatrix(vector4);
				camera2.cullingMatrix = current.projectionMatrix * current.worldToCameraMatrix;
				camera2.cullingMask = -17 & this.refractLayers.value;
				camera2.targetTexture = this.m_RefractionTexture;
				camera2.transform.position = current.transform.position;
				camera2.transform.rotation = current.transform.rotation;
				camera2.Render();
				base.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", this.m_RefractionTexture);
			}
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = pixelLightCount;
			}
			switch (waterMode)
			{
			case Water.WaterMode.Simple:
				Shader.EnableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
				break;
			case Water.WaterMode.Reflective:
				Shader.DisableKeyword("WATER_SIMPLE");
				Shader.EnableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
				break;
			case Water.WaterMode.Refractive:
				Shader.DisableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.EnableKeyword("WATER_REFRACTIVE");
				break;
			}
			Water.s_InsideWater = false;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x00135C84 File Offset: 0x00133E84
		private void OnDisable()
		{
			if (this.m_ReflectionTexture)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
				this.m_ReflectionTexture = null;
			}
			if (this.m_RefractionTexture)
			{
				global::UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
				this.m_RefractionTexture = null;
			}
			foreach (KeyValuePair<Camera, Camera> keyValuePair in this.m_ReflectionCameras)
			{
				global::UnityEngine.Object.DestroyImmediate(keyValuePair.Value.gameObject);
			}
			this.m_ReflectionCameras.Clear();
			foreach (KeyValuePair<Camera, Camera> keyValuePair2 in this.m_RefractionCameras)
			{
				global::UnityEngine.Object.DestroyImmediate(keyValuePair2.Value.gameObject);
			}
			this.m_RefractionCameras.Clear();
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x00135D84 File Offset: 0x00133F84
		private void Update()
		{
			if (!base.GetComponent<Renderer>())
			{
				return;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return;
			}
			Vector4 vector = sharedMaterial.GetVector("_WaveSpeed");
			float @float = sharedMaterial.GetFloat("_WaveScale");
			Vector4 vector2 = new Vector4(@float, @float, @float * 0.6f, @float * 0.53f);
			Vector4 vector3 = new Vector4(@float * Mathf.Sin(Time.time * vector.x) * 0.015f, @float * Mathf.Cos(Time.time * vector.y) * 0.015f, @float * Mathf.Sin(Time.time * vector.z) * 0.5f, @float * Mathf.Cos(Time.time * vector.w) * 0.5f);
			sharedMaterial.SetVector("_WaveOffset", vector3);
			sharedMaterial.SetVector("_WaveScale4", vector2);
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x00135E68 File Offset: 0x00134068
		private void UpdateCameraModes(Camera src, Camera dest)
		{
			if (dest == null)
			{
				return;
			}
			dest.clearFlags = src.clearFlags;
			dest.backgroundColor = src.backgroundColor;
			if (src.clearFlags == CameraClearFlags.Skybox)
			{
				Skybox component = src.GetComponent<Skybox>();
				Skybox component2 = dest.GetComponent<Skybox>();
				if (!component || !component.material)
				{
					component2.enabled = false;
				}
				else
				{
					component2.enabled = true;
					component2.material = component.material;
				}
			}
			dest.farClipPlane = src.farClipPlane;
			dest.nearClipPlane = src.nearClipPlane;
			dest.orthographic = src.orthographic;
			dest.fieldOfView = src.fieldOfView;
			dest.aspect = src.aspect;
			dest.orthographicSize = src.orthographicSize;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x00135F28 File Offset: 0x00134128
		private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
		{
			Water.WaterMode waterMode = this.GetWaterMode();
			reflectionCamera = null;
			refractionCamera = null;
			if (waterMode >= Water.WaterMode.Reflective)
			{
				if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.textureSize)
				{
					if (this.m_ReflectionTexture)
					{
						global::UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
					}
					this.m_ReflectionTexture = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_ReflectionTexture.name = "__WaterReflection" + base.GetInstanceID().ToString();
					this.m_ReflectionTexture.isPowerOfTwo = true;
					this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
					this.m_OldReflectionTextureSize = this.textureSize;
				}
				this.m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
				if (!reflectionCamera)
				{
					GameObject gameObject = new GameObject("Water Refl Camera id" + base.GetInstanceID().ToString() + " for " + currentCamera.GetInstanceID().ToString(), new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					});
					reflectionCamera = gameObject.GetComponent<Camera>();
					reflectionCamera.enabled = false;
					reflectionCamera.transform.position = base.transform.position;
					reflectionCamera.transform.rotation = base.transform.rotation;
					reflectionCamera.gameObject.AddComponent<FlareLayer>();
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					this.m_ReflectionCameras[currentCamera] = reflectionCamera;
				}
			}
			if (waterMode >= Water.WaterMode.Refractive)
			{
				if (!this.m_RefractionTexture || this.m_OldRefractionTextureSize != this.textureSize)
				{
					if (this.m_RefractionTexture)
					{
						global::UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
					}
					this.m_RefractionTexture = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_RefractionTexture.name = "__WaterRefraction" + base.GetInstanceID().ToString();
					this.m_RefractionTexture.isPowerOfTwo = true;
					this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
					this.m_OldRefractionTextureSize = this.textureSize;
				}
				this.m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
				if (!refractionCamera)
				{
					GameObject gameObject2 = new GameObject("Water Refr Camera id" + base.GetInstanceID().ToString() + " for " + currentCamera.GetInstanceID().ToString(), new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					});
					refractionCamera = gameObject2.GetComponent<Camera>();
					refractionCamera.enabled = false;
					refractionCamera.transform.position = base.transform.position;
					refractionCamera.transform.rotation = base.transform.rotation;
					refractionCamera.gameObject.AddComponent<FlareLayer>();
					gameObject2.hideFlags = HideFlags.HideAndDontSave;
					this.m_RefractionCameras[currentCamera] = refractionCamera;
				}
			}
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x0004941B File Offset: 0x0004761B
		private Water.WaterMode GetWaterMode()
		{
			if (this.m_HardwareWaterSupport < this.waterMode)
			{
				return this.m_HardwareWaterSupport;
			}
			return this.waterMode;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x00136210 File Offset: 0x00134410
		private Water.WaterMode FindHardwareWaterSupport()
		{
			if (!base.GetComponent<Renderer>())
			{
				return Water.WaterMode.Simple;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return Water.WaterMode.Simple;
			}
			string tag = sharedMaterial.GetTag("WATERMODE", false);
			if (tag == "Refractive")
			{
				return Water.WaterMode.Refractive;
			}
			if (tag == "Reflective")
			{
				return Water.WaterMode.Reflective;
			}
			return Water.WaterMode.Simple;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x00136270 File Offset: 0x00134470
		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 vector = pos + normal * this.clipPlaneOffset;
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
			Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x001362D8 File Offset: 0x001344D8
		private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
		{
			reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
			reflectionMat.m01 = -2f * plane[0] * plane[1];
			reflectionMat.m02 = -2f * plane[0] * plane[2];
			reflectionMat.m03 = -2f * plane[3] * plane[0];
			reflectionMat.m10 = -2f * plane[1] * plane[0];
			reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
			reflectionMat.m12 = -2f * plane[1] * plane[2];
			reflectionMat.m13 = -2f * plane[3] * plane[1];
			reflectionMat.m20 = -2f * plane[2] * plane[0];
			reflectionMat.m21 = -2f * plane[2] * plane[1];
			reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
			reflectionMat.m23 = -2f * plane[3] * plane[2];
			reflectionMat.m30 = 0f;
			reflectionMat.m31 = 0f;
			reflectionMat.m32 = 0f;
			reflectionMat.m33 = 1f;
		}

		// Token: 0x040024DA RID: 9434
		public Water.WaterMode waterMode = Water.WaterMode.Refractive;

		// Token: 0x040024DB RID: 9435
		public bool disablePixelLights = true;

		// Token: 0x040024DC RID: 9436
		public int textureSize = 256;

		// Token: 0x040024DD RID: 9437
		public float clipPlaneOffset = 0.07f;

		// Token: 0x040024DE RID: 9438
		public LayerMask reflectLayers = -1;

		// Token: 0x040024DF RID: 9439
		public LayerMask refractLayers = -1;

		// Token: 0x040024E0 RID: 9440
		private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x040024E1 RID: 9441
		private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x040024E2 RID: 9442
		private RenderTexture m_ReflectionTexture;

		// Token: 0x040024E3 RID: 9443
		private RenderTexture m_RefractionTexture;

		// Token: 0x040024E4 RID: 9444
		private Water.WaterMode m_HardwareWaterSupport = Water.WaterMode.Refractive;

		// Token: 0x040024E5 RID: 9445
		private int m_OldReflectionTextureSize;

		// Token: 0x040024E6 RID: 9446
		private int m_OldRefractionTextureSize;

		// Token: 0x040024E7 RID: 9447
		private static bool s_InsideWater;

		// Token: 0x0200069A RID: 1690
		public enum WaterMode
		{
			// Token: 0x040024E9 RID: 9449
			Simple,
			// Token: 0x040024EA RID: 9450
			Reflective,
			// Token: 0x040024EB RID: 9451
			Refractive
		}
	}
}
