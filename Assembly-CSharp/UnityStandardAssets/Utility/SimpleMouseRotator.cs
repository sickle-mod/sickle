using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000676 RID: 1654
	public class SimpleMouseRotator : MonoBehaviour
	{
		// Token: 0x06003403 RID: 13315 RVA: 0x00048D58 File Offset: 0x00046F58
		private void Start()
		{
			this.m_OriginalRotation = base.transform.localRotation;
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x00133918 File Offset: 0x00131B18
		private void Update()
		{
			base.transform.localRotation = this.m_OriginalRotation;
			if (this.relative)
			{
				float num = CrossPlatformInputManager.GetAxis("Mouse X");
				float num2 = CrossPlatformInputManager.GetAxis("Mouse Y");
				if (this.m_TargetAngles.y > 180f)
				{
					this.m_TargetAngles.y = this.m_TargetAngles.y - 360f;
					this.m_FollowAngles.y = this.m_FollowAngles.y - 360f;
				}
				if (this.m_TargetAngles.x > 180f)
				{
					this.m_TargetAngles.x = this.m_TargetAngles.x - 360f;
					this.m_FollowAngles.x = this.m_FollowAngles.x - 360f;
				}
				if (this.m_TargetAngles.y < -180f)
				{
					this.m_TargetAngles.y = this.m_TargetAngles.y + 360f;
					this.m_FollowAngles.y = this.m_FollowAngles.y + 360f;
				}
				if (this.m_TargetAngles.x < -180f)
				{
					this.m_TargetAngles.x = this.m_TargetAngles.x + 360f;
					this.m_FollowAngles.x = this.m_FollowAngles.x + 360f;
				}
				this.m_TargetAngles.y = this.m_TargetAngles.y + num * this.rotationSpeed;
				this.m_TargetAngles.x = this.m_TargetAngles.x + num2 * this.rotationSpeed;
				this.m_TargetAngles.y = Mathf.Clamp(this.m_TargetAngles.y, -this.rotationRange.y * 0.5f, this.rotationRange.y * 0.5f);
				this.m_TargetAngles.x = Mathf.Clamp(this.m_TargetAngles.x, -this.rotationRange.x * 0.5f, this.rotationRange.x * 0.5f);
			}
			else
			{
				float num = Input.mousePosition.x;
				float num2 = Input.mousePosition.y;
				this.m_TargetAngles.y = Mathf.Lerp(-this.rotationRange.y * 0.5f, this.rotationRange.y * 0.5f, num / (float)Screen.width);
				this.m_TargetAngles.x = Mathf.Lerp(-this.rotationRange.x * 0.5f, this.rotationRange.x * 0.5f, num2 / (float)Screen.height);
			}
			this.m_FollowAngles = Vector3.SmoothDamp(this.m_FollowAngles, this.m_TargetAngles, ref this.m_FollowVelocity, this.dampingTime);
			base.transform.localRotation = this.m_OriginalRotation * Quaternion.Euler(-this.m_FollowAngles.x, this.m_FollowAngles.y, 0f);
		}

		// Token: 0x04002450 RID: 9296
		public Vector2 rotationRange = new Vector3(70f, 70f);

		// Token: 0x04002451 RID: 9297
		public float rotationSpeed = 10f;

		// Token: 0x04002452 RID: 9298
		public float dampingTime = 0.2f;

		// Token: 0x04002453 RID: 9299
		public bool autoZeroVerticalOnMobile = true;

		// Token: 0x04002454 RID: 9300
		public bool autoZeroHorizontalOnMobile;

		// Token: 0x04002455 RID: 9301
		public bool relative = true;

		// Token: 0x04002456 RID: 9302
		private Vector3 m_TargetAngles;

		// Token: 0x04002457 RID: 9303
		private Vector3 m_FollowAngles;

		// Token: 0x04002458 RID: 9304
		private Vector3 m_FollowVelocity;

		// Token: 0x04002459 RID: 9305
		private Quaternion m_OriginalRotation;
	}
}
