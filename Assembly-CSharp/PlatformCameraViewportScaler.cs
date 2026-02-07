using System;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class PlatformCameraViewportScaler : MonoBehaviour
{
	// Token: 0x060009BA RID: 2490 RVA: 0x0002EA11 File Offset: 0x0002CC11
	private void Awake()
	{
		if (this.isEnable)
		{
			this.CalculateCameraViewportScaler();
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0007C3D8 File Offset: 0x0007A5D8
	private void CalculateCameraViewportScaler()
	{
		Camera component = base.GetComponent<Camera>();
		if (component)
		{
			Vector2 screenResolution = PlatformManager.ScreenResolution;
			Rect safetyAreaPixels = PlatformManager.SafetyAreaPixels;
			float num = safetyAreaPixels.x / screenResolution.x;
			float num2 = safetyAreaPixels.y / screenResolution.y;
			float num3 = safetyAreaPixels.width / screenResolution.x;
			float num4 = safetyAreaPixels.height / screenResolution.y;
			component.rect = new Rect(num, num2, num3, num4);
		}
	}

	// Token: 0x040008CA RID: 2250
	[SerializeField]
	private bool isEnable = true;
}
