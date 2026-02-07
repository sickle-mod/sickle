using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x0200066A RID: 1642
	[Serializable]
	public class FOVKick
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x00048B7C File Offset: 0x00046D7C
		public void Setup(Camera camera)
		{
			this.CheckStatus(camera);
			this.Camera = camera;
			this.originalFov = camera.fieldOfView;
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x00048B98 File Offset: 0x00046D98
		private void CheckStatus(Camera camera)
		{
			if (camera == null)
			{
				throw new Exception("FOVKick camera is null, please supply the camera to the constructor");
			}
			if (this.IncreaseCurve == null)
			{
				throw new Exception("FOVKick Increase curve is null, please define the curve for the field of view kicks");
			}
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x00048BC1 File Offset: 0x00046DC1
		public void ChangeCamera(Camera camera)
		{
			this.Camera = camera;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x00048BCA File Offset: 0x00046DCA
		public IEnumerator FOVKickUp()
		{
			float t = Mathf.Abs((this.Camera.fieldOfView - this.originalFov) / this.FOVIncrease);
			while (t < this.TimeToIncrease)
			{
				this.Camera.fieldOfView = this.originalFov + this.IncreaseCurve.Evaluate(t / this.TimeToIncrease) * this.FOVIncrease;
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x00048BD9 File Offset: 0x00046DD9
		public IEnumerator FOVKickDown()
		{
			float t = Mathf.Abs((this.Camera.fieldOfView - this.originalFov) / this.FOVIncrease);
			while (t > 0f)
			{
				this.Camera.fieldOfView = this.originalFov + this.IncreaseCurve.Evaluate(t / this.TimeToDecrease) * this.FOVIncrease;
				t -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			this.Camera.fieldOfView = this.originalFov;
			yield break;
		}

		// Token: 0x0400241D RID: 9245
		public Camera Camera;

		// Token: 0x0400241E RID: 9246
		[HideInInspector]
		public float originalFov;

		// Token: 0x0400241F RID: 9247
		public float FOVIncrease = 3f;

		// Token: 0x04002420 RID: 9248
		public float TimeToIncrease = 1f;

		// Token: 0x04002421 RID: 9249
		public float TimeToDecrease = 1f;

		// Token: 0x04002422 RID: 9250
		public AnimationCurve IncreaseCurve;
	}
}
