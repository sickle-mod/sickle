using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000697 RID: 1687
	[ExecuteInEditMode]
	[RequireComponent(typeof(WaterBase))]
	public class PlanarReflection : MonoBehaviour
	{
		// Token: 0x0600347C RID: 13436 RVA: 0x0004927E File Offset: 0x0004747E
		public void Start()
		{
			this.m_SharedMaterial = ((WaterBase)base.gameObject.GetComponent(typeof(WaterBase))).sharedMaterial;
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x00135234 File Offset: 0x00133434
		private Camera CreateReflectionCameraFor(Camera cam)
		{
			string text = base.gameObject.name + "Reflection" + cam.name;
			GameObject gameObject = GameObject.Find(text);
			if (!gameObject)
			{
				gameObject = new GameObject(text, new Type[] { typeof(Camera) });
			}
			if (!gameObject.GetComponent(typeof(Camera)))
			{
				gameObject.AddComponent(typeof(Camera));
			}
			Camera component = gameObject.GetComponent<Camera>();
			component.backgroundColor = this.clearColor;
			component.clearFlags = (this.reflectSkybox ? CameraClearFlags.Skybox : CameraClearFlags.Color);
			this.SetStandardCameraParameter(component, this.reflectionMask);
			if (!component.targetTexture)
			{
				component.targetTexture = this.CreateTextureFor(cam);
			}
			return component;
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000492A5 File Offset: 0x000474A5
		private void SetStandardCameraParameter(Camera cam, LayerMask mask)
		{
			cam.cullingMask = mask & ~(1 << LayerMask.NameToLayer("Water"));
			cam.backgroundColor = Color.black;
			cam.enabled = false;
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x000492D6 File Offset: 0x000474D6
		private RenderTexture CreateTextureFor(Camera cam)
		{
			return new RenderTexture(Mathf.FloorToInt((float)cam.pixelWidth * 0.5f), Mathf.FloorToInt((float)cam.pixelHeight * 0.5f), 24)
			{
				hideFlags = HideFlags.DontSave
			};
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x001352FC File Offset: 0x001334FC
		public void RenderHelpCameras(Camera currentCam)
		{
			if (this.m_HelperCameras == null)
			{
				this.m_HelperCameras = new Dictionary<Camera, bool>();
			}
			if (!this.m_HelperCameras.ContainsKey(currentCam))
			{
				this.m_HelperCameras.Add(currentCam, false);
			}
			if (this.m_HelperCameras[currentCam])
			{
				return;
			}
			if (!this.m_ReflectionCamera)
			{
				this.m_ReflectionCamera = this.CreateReflectionCameraFor(currentCam);
			}
			this.RenderReflectionFor(currentCam, this.m_ReflectionCamera);
			this.m_HelperCameras[currentCam] = true;
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x0004930B File Offset: 0x0004750B
		public void LateUpdate()
		{
			if (this.m_HelperCameras != null)
			{
				this.m_HelperCameras.Clear();
			}
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x00049320 File Offset: 0x00047520
		public void WaterTileBeingRendered(Transform tr, Camera currentCam)
		{
			this.RenderHelpCameras(currentCam);
			if (this.m_ReflectionCamera && this.m_SharedMaterial)
			{
				this.m_SharedMaterial.SetTexture(this.reflectionSampler, this.m_ReflectionCamera.targetTexture);
			}
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x0004935F File Offset: 0x0004755F
		public void OnEnable()
		{
			Shader.EnableKeyword("WATER_REFLECTIVE");
			Shader.DisableKeyword("WATER_SIMPLE");
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x00049375 File Offset: 0x00047575
		public void OnDisable()
		{
			Shader.EnableKeyword("WATER_SIMPLE");
			Shader.DisableKeyword("WATER_REFLECTIVE");
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x0013537C File Offset: 0x0013357C
		private void RenderReflectionFor(Camera cam, Camera reflectCamera)
		{
			if (!reflectCamera)
			{
				return;
			}
			if (this.m_SharedMaterial && !this.m_SharedMaterial.HasProperty(this.reflectionSampler))
			{
				return;
			}
			reflectCamera.cullingMask = this.reflectionMask & ~(1 << LayerMask.NameToLayer("Water"));
			this.SaneCameraSettings(reflectCamera);
			reflectCamera.backgroundColor = this.clearColor;
			reflectCamera.clearFlags = (this.reflectSkybox ? CameraClearFlags.Skybox : CameraClearFlags.Color);
			if (this.reflectSkybox && cam.gameObject.GetComponent(typeof(Skybox)))
			{
				Skybox skybox = (Skybox)reflectCamera.gameObject.GetComponent(typeof(Skybox));
				if (!skybox)
				{
					skybox = (Skybox)reflectCamera.gameObject.AddComponent(typeof(Skybox));
				}
				skybox.material = ((Skybox)cam.GetComponent(typeof(Skybox))).material;
			}
			GL.invertCulling = true;
			Transform transform = base.transform;
			Vector3 eulerAngles = cam.transform.eulerAngles;
			reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
			reflectCamera.transform.position = cam.transform.position;
			Vector3 position = transform.transform.position;
			position.y = transform.position.y;
			Vector3 up = transform.transform.up;
			float num = -Vector3.Dot(up, position) - this.clipPlaneOffset;
			Vector4 vector = new Vector4(up.x, up.y, up.z, num);
			Matrix4x4 matrix4x = Matrix4x4.zero;
			matrix4x = PlanarReflection.CalculateReflectionMatrix(matrix4x, vector);
			this.m_Oldpos = cam.transform.position;
			Vector3 vector2 = matrix4x.MultiplyPoint(this.m_Oldpos);
			reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
			Vector4 vector3 = this.CameraSpacePlane(reflectCamera, position, up, 1f);
			Matrix4x4 matrix4x2 = cam.projectionMatrix;
			matrix4x2 = PlanarReflection.CalculateObliqueMatrix(matrix4x2, vector3);
			reflectCamera.projectionMatrix = matrix4x2;
			reflectCamera.transform.position = vector2;
			Vector3 eulerAngles2 = cam.transform.eulerAngles;
			reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
			reflectCamera.Render();
			GL.invertCulling = false;
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x0004938B File Offset: 0x0004758B
		private void SaneCameraSettings(Camera helperCam)
		{
			helperCam.depthTextureMode = DepthTextureMode.None;
			helperCam.backgroundColor = Color.black;
			helperCam.clearFlags = CameraClearFlags.Color;
			helperCam.renderingPath = RenderingPath.Forward;
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x001355E4 File Offset: 0x001337E4
		private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
		{
			Vector4 vector = projection.inverse * new Vector4(PlanarReflection.Sgn(clipPlane.x), PlanarReflection.Sgn(clipPlane.y), 1f, 1f);
			Vector4 vector2 = clipPlane * (2f / Vector4.Dot(clipPlane, vector));
			projection[2] = vector2.x - projection[3];
			projection[6] = vector2.y - projection[7];
			projection[10] = vector2.z - projection[11];
			projection[14] = vector2.w - projection[15];
			return projection;
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00135698 File Offset: 0x00133898
		private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
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
			return reflectionMat;
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000493AD File Offset: 0x000475AD
		private static float Sgn(float a)
		{
			if (a > 0f)
			{
				return 1f;
			}
			if (a < 0f)
			{
				return -1f;
			}
			return 0f;
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x00135850 File Offset: 0x00133A50
		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 vector = pos + normal * this.clipPlaneOffset;
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
			Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
		}

		// Token: 0x040024CF RID: 9423
		public LayerMask reflectionMask;

		// Token: 0x040024D0 RID: 9424
		public bool reflectSkybox;

		// Token: 0x040024D1 RID: 9425
		public Color clearColor = Color.grey;

		// Token: 0x040024D2 RID: 9426
		public string reflectionSampler = "_ReflectionTex";

		// Token: 0x040024D3 RID: 9427
		public float clipPlaneOffset = 0.07f;

		// Token: 0x040024D4 RID: 9428
		private Vector3 m_Oldpos;

		// Token: 0x040024D5 RID: 9429
		private Camera m_ReflectionCamera;

		// Token: 0x040024D6 RID: 9430
		private Material m_SharedMaterial;

		// Token: 0x040024D7 RID: 9431
		private Dictionary<Camera, bool> m_HelperCameras;
	}
}
