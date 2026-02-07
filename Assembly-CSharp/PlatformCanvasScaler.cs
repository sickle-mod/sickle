using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200014B RID: 331
public class PlatformCanvasScaler : MonoBehaviour
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x060009BD RID: 2493 RVA: 0x0007C454 File Offset: 0x0007A654
	// (remove) Token: 0x060009BE RID: 2494 RVA: 0x0007C48C File Offset: 0x0007A68C
	public event Action<float> OnScaleFactorChanged;

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x0002EA30 File Offset: 0x0002CC30
	// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0002EA38 File Offset: 0x0002CC38
	public float ScaleFactor
	{
		get
		{
			return this.scaleFactor;
		}
		private set
		{
			if (this.scaleFactor != value)
			{
				this.scaleFactor = value;
				UniversalInvocator.Event_Invocator<float>(this.OnScaleFactorChanged, new object[] { this.scaleFactor });
			}
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0002EA69 File Offset: 0x0002CC69
	public float HeightFactor
	{
		get
		{
			return (float)Screen.height / this.defaultReferenceResolution.y;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0002EA7D File Offset: 0x0002CC7D
	private void Awake()
	{
		this.canvasScaler = base.GetComponent<CanvasScaler>();
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0002EA8B File Offset: 0x0002CC8B
	private void Start()
	{
		base.StartCoroutine(this.CalculateCanvasScaler());
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0002EA9A File Offset: 0x0002CC9A
	private IEnumerator CalculateCanvasScaler()
	{
		while (!PlatformManager.IsSafeAreaInitialized)
		{
			yield return new WaitForEndOfFrame();
		}
		if (this.canvasScaler)
		{
			Vector2 vector = this.defaultReferenceResolution * PlatformManager.DeviceScaleFactor;
			vector.x /= PlatformManager.SafetyAreaPixels.width / PlatformManager.ScreenResolution.x;
			vector.y /= PlatformManager.SafetyAreaPixels.height / PlatformManager.ScreenResolution.y;
			this.canvasScaler.referenceResolution = vector;
		}
		this.ScaleFactor = PlatformManager.DeviceScaleFactor;
		yield break;
	}

	// Token: 0x040008CC RID: 2252
	private CanvasScaler canvasScaler;

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private Vector2 defaultReferenceResolution = new Vector2(568f, 320f);

	// Token: 0x040008CE RID: 2254
	private float scaleFactor;
}
