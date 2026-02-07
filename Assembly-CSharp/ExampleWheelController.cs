using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06000015 RID: 21 RVA: 0x00027E5E File Offset: 0x0002605E
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00052990 File Offset: 0x00050B90
	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		float num = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(num, -0.25f, 0.25f));
		}
	}

	// Token: 0x04000006 RID: 6
	public float acceleration;

	// Token: 0x04000007 RID: 7
	public Renderer motionVectorRenderer;

	// Token: 0x04000008 RID: 8
	private Rigidbody m_Rigidbody;

	// Token: 0x02000007 RID: 7
	private static class Uniforms
	{
		// Token: 0x04000009 RID: 9
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
