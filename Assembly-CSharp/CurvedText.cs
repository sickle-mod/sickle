using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class CurvedText : MonoBehaviour
{
	// Token: 0x06000053 RID: 83 RVA: 0x00028296 File Offset: 0x00026496
	private void Awake()
	{
		this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000282A9 File Offset: 0x000264A9
	private void Start()
	{
		base.StartCoroutine(this.WarpText());
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000282B8 File Offset: 0x000264B8
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		return new AnimationCurve
		{
			keys = curve.keys
		};
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000282CB File Offset: 0x000264CB
	private IEnumerator WarpText()
	{
		this.VertexCurve.preWrapMode = WrapMode.Once;
		this.VertexCurve.postWrapMode = WrapMode.Once;
		this.m_TextComponent.havePropertiesChanged = true;
		float old_CurveScale = this.CurveScale;
		AnimationCurve old_curve = this.CopyAnimationCurve(this.VertexCurve);
		for (;;)
		{
			if (!this.m_TextComponent.havePropertiesChanged && old_CurveScale == this.CurveScale && old_curve.keys[1].value == this.VertexCurve.keys[1].value)
			{
				yield return null;
			}
			else
			{
				old_CurveScale = this.CurveScale;
				old_curve = this.CopyAnimationCurve(this.VertexCurve);
				this.m_TextComponent.ForceMeshUpdate(false, false);
				TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
				int characterCount = textInfo.characterCount;
				if (characterCount != 0)
				{
					float x = this.m_TextComponent.textInfo.meshInfo[0].mesh.bounds.min.x;
					float x2 = this.m_TextComponent.textInfo.meshInfo[0].mesh.bounds.max.x;
					for (int i = 0; i < characterCount; i++)
					{
						if (textInfo.characterInfo[i].isVisible)
						{
							int vertexIndex = textInfo.characterInfo[i].vertexIndex;
							int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
							Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
							Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
							vertices[vertexIndex] += -vector;
							vertices[vertexIndex + 1] += -vector;
							vertices[vertexIndex + 2] += -vector;
							vertices[vertexIndex + 3] += -vector;
							float num = (vector.x - x) / (x2 - x);
							float num2 = num + 0.0001f;
							float num3 = this.VertexCurve.Evaluate(num) * this.CurveScale;
							float num4 = this.VertexCurve.Evaluate(num2) * this.CurveScale;
							Vector3 vector2 = new Vector3(1f, 0f, 0f);
							Vector3 vector3 = new Vector3(num2 * (x2 - x) + x, num4) - new Vector3(vector.x, num3);
							float num5 = Mathf.Acos(Vector3.Dot(vector2, vector3.normalized)) * 57.29578f;
							float num6 = ((Vector3.Cross(vector2, vector3).z > 0f) ? num5 : (360f - num5));
							Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, num3, 0f), Quaternion.Euler(0f, 0f, num6), Vector3.one);
							vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
							vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
							vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
							vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
							vertices[vertexIndex] += vector;
							vertices[vertexIndex + 1] += vector;
							vertices[vertexIndex + 2] += vector;
							vertices[vertexIndex + 3] += vector;
						}
					}
					this.m_TextComponent.UpdateVertexData();
					yield return new WaitForSeconds(0.025f);
				}
			}
		}
		yield break;
	}

	// Token: 0x04000045 RID: 69
	private TMP_Text m_TextComponent;

	// Token: 0x04000046 RID: 70
	public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 2f),
		new Keyframe(0.5f, 0f),
		new Keyframe(0.75f, 2f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04000047 RID: 71
	public float AngleMultiplier = 1f;

	// Token: 0x04000048 RID: 72
	public float SpeedMultiplier = 1f;

	// Token: 0x04000049 RID: 73
	public float CurveScale = 1f;
}
