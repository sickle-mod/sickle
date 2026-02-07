using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CE RID: 1742
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	internal class PostEffectsHelper : MonoBehaviour
	{
		// Token: 0x06003542 RID: 13634 RVA: 0x00049D6B File Offset: 0x00047F6B
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Debug.Log("OnRenderImage in Helper called ...");
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x0013D6F4 File Offset: 0x0013B8F4
		private static void DrawLowLevelPlaneAlignedWithCamera(float dist, RenderTexture source, RenderTexture dest, Material material, Camera cameraForProjectionMatrix)
		{
			RenderTexture.active = dest;
			material.SetTexture("_MainTex", source);
			bool flag = true;
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.LoadProjectionMatrix(cameraForProjectionMatrix.projectionMatrix);
			float num = cameraForProjectionMatrix.fieldOfView * 0.5f * 0.017453292f;
			float num2 = Mathf.Cos(num) / Mathf.Sin(num);
			float aspect = cameraForProjectionMatrix.aspect;
			float num3 = aspect / -num2;
			float num4 = aspect / num2;
			float num5 = 1f / -num2;
			float num6 = 1f / num2;
			float num7 = 1f;
			num3 *= dist * num7;
			num4 *= dist * num7;
			num5 *= dist * num7;
			num6 *= dist * num7;
			float num8 = -dist;
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				GL.Begin(7);
				float num9;
				float num10;
				if (flag)
				{
					num9 = 1f;
					num10 = 0f;
				}
				else
				{
					num9 = 0f;
					num10 = 1f;
				}
				GL.TexCoord2(0f, num9);
				GL.Vertex3(num3, num5, num8);
				GL.TexCoord2(1f, num9);
				GL.Vertex3(num4, num5, num8);
				GL.TexCoord2(1f, num10);
				GL.Vertex3(num4, num6, num8);
				GL.TexCoord2(0f, num10);
				GL.Vertex3(num3, num6, num8);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x0013D84C File Offset: 0x0013BA4C
		private static void DrawBorder(RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				float num;
				float num2;
				if (flag)
				{
					num = 1f;
					num2 = 0f;
				}
				else
				{
					num = 0f;
					num2 = 1f;
				}
				float num3 = 0f;
				float num4 = 0f + 1f / ((float)dest.width * 1f);
				float num5 = 0f;
				float num6 = 1f;
				GL.Begin(7);
				GL.TexCoord2(0f, num);
				GL.Vertex3(num3, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num3, num6, 0.1f);
				float num7 = 1f - 1f / ((float)dest.width * 1f);
				num4 = 1f;
				num5 = 0f;
				num6 = 1f;
				GL.TexCoord2(0f, num);
				GL.Vertex3(num7, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num7, num6, 0.1f);
				float num8 = 0f;
				num4 = 1f;
				num5 = 0f;
				num6 = 0f + 1f / ((float)dest.height * 1f);
				GL.TexCoord2(0f, num);
				GL.Vertex3(num8, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num8, num6, 0.1f);
				float num9 = 0f;
				num4 = 1f;
				num5 = 1f - 1f / ((float)dest.height * 1f);
				num6 = 1f;
				GL.TexCoord2(0f, num);
				GL.Vertex3(num9, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num9, num6, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x0013DAE8 File Offset: 0x0013BCE8
		private static void DrawLowLevelQuad(float x1, float x2, float y1, float y2, RenderTexture source, RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			material.SetTexture("_MainTex", source);
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				GL.Begin(7);
				float num;
				float num2;
				if (flag)
				{
					num = 1f;
					num2 = 0f;
				}
				else
				{
					num = 0f;
					num2 = 1f;
				}
				GL.TexCoord2(0f, num);
				GL.Vertex3(x1, y1, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(x2, y1, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(x2, y2, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(x1, y2, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}
	}
}
