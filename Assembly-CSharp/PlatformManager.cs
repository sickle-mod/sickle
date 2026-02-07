using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public static class PlatformManager
{
	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600098E RID: 2446 RVA: 0x000283F8 File Offset: 0x000265F8
	public static bool IsStandalone
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static bool IsUnityEditor
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static bool IsMobile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static bool IsAndroid
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000992 RID: 2450 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static bool IsIOS
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x000283F8 File Offset: 0x000265F8
	public static bool IsSteam
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000994 RID: 2452 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static bool IsGOG
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x0002E898 File Offset: 0x0002CA98
	public static RuntimePlatform RuntimePlatform
	{
		get
		{
			return Application.platform;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000996 RID: 2454 RVA: 0x0002E89F File Offset: 0x0002CA9F
	public static string OperatingSystem
	{
		get
		{
			return SystemInfo.operatingSystem;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000997 RID: 2455 RVA: 0x0002E8A6 File Offset: 0x0002CAA6
	public static string DeviceModel
	{
		get
		{
			return SystemInfo.deviceModel;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000998 RID: 2456 RVA: 0x0002E8AD File Offset: 0x0002CAAD
	public static DeviceType DeviceType
	{
		get
		{
			return SystemInfo.deviceType;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000999 RID: 2457 RVA: 0x0002E8B4 File Offset: 0x0002CAB4
	public static string DeviceName
	{
		get
		{
			return SystemInfo.deviceName;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600099A RID: 2458 RVA: 0x0002E8BB File Offset: 0x0002CABB
	public static string DeviceUniqueIdentifier
	{
		get
		{
			return SystemInfo.deviceUniqueIdentifier;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x0002E8C2 File Offset: 0x0002CAC2
	public static Vector2 ScreenResolution
	{
		get
		{
			return new Vector2((float)Screen.width, (float)Screen.height);
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x0002E8D5 File Offset: 0x0002CAD5
	public static Rect SafetyAreaPixels
	{
		get
		{
			return Screen.safeArea;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600099D RID: 2461 RVA: 0x0007B950 File Offset: 0x00079B50
	public static bool IsSafeAreaInitialized
	{
		get
		{
			Rect safetyAreaPixels = PlatformManager.SafetyAreaPixels;
			return !float.IsNaN(safetyAreaPixels.x) && !float.IsNaN(safetyAreaPixels.y) && !float.IsNaN(safetyAreaPixels.width) && !float.IsNaN(safetyAreaPixels.height);
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600099E RID: 2462 RVA: 0x0007B9A0 File Offset: 0x00079BA0
	public static bool IsSafeAreaActive
	{
		get
		{
			return !Mathf.Approximately(PlatformManager.ScreenResolution.x, PlatformManager.SafetyAreaPixels.width) || !Mathf.Approximately(PlatformManager.ScreenResolution.y, PlatformManager.SafetyAreaPixels.height);
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x0007B9EC File Offset: 0x00079BEC
	public static float ScreenAspectRatio
	{
		get
		{
			Vector2 screenResolution = PlatformManager.ScreenResolution;
			return screenResolution.x / screenResolution.y;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0002E8DC File Offset: 0x0002CADC
	public static float ScreenDPI
	{
		get
		{
			return Screen.dpi;
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0007BA0C File Offset: 0x00079C0C
	public static float ScreenInches()
	{
		float num = PlatformManager.ScreenResolution.x * PlatformManager.ScreenResolution.x;
		float num2 = PlatformManager.ScreenResolution.y * PlatformManager.ScreenResolution.y;
		return Mathf.Sqrt(num + num2) / PlatformManager.ScreenDPI;
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static bool IsIPad
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static bool IsIPhone
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public static PlatformManager.DeviceSizeType DeviceSize
	{
		get
		{
			return PlatformManager.DeviceSizeType.UNKNOWN;
		}
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0007BA54 File Offset: 0x00079C54
	private static PlatformManager.DeviceSizeType DeviceSizeByDiagonalInches()
	{
		float num = PlatformManager.ScreenInches();
		if (num < 8f)
		{
			return PlatformManager.DeviceSizeType.XS;
		}
		if (num < 10f)
		{
			return PlatformManager.DeviceSizeType.S;
		}
		if (num < 10.9f)
		{
			return PlatformManager.DeviceSizeType.M;
		}
		if (num < 12f)
		{
			return PlatformManager.DeviceSizeType.L;
		}
		if (num < 13f)
		{
			return PlatformManager.DeviceSizeType.XL;
		}
		return PlatformManager.DeviceSizeType.UNKNOWN;
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0002E8E3 File Offset: 0x0002CAE3
	public static float DeviceScaleFactor
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x0007BA9C File Offset: 0x00079C9C
	public static Rect GetSafetyAreaUnits(Vector2 referenceResolution)
	{
		float num = referenceResolution.y / PlatformManager.ScreenResolution.y;
		float num2 = PlatformManager.SafetyAreaPixels.x * num;
		float num3 = PlatformManager.SafetyAreaPixels.y * num;
		float num4 = PlatformManager.ScreenResolution.x - 2f * num2;
		float num5 = PlatformManager.ScreenResolution.y - num3;
		return new Rect(num2, num3, num4, num5);
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x0007BB08 File Offset: 0x00079D08
	public static string GetText()
	{
		return string.Empty + "<b>Platform:</b> " + PlatformManager.RuntimePlatform.ToString() + "\n" + "<b>Screen resolution [px]:</b> " + PlatformManager.ScreenResolution.ToString() + "\n" + "<b>Safety area [px]:</b> " + PlatformManager.SafetyAreaPixels.ToString() + "\n";
	}

	// Token: 0x02000145 RID: 325
	public enum DeviceSizeType
	{
		// Token: 0x0400089C RID: 2204
		UNKNOWN,
		// Token: 0x0400089D RID: 2205
		XS,
		// Token: 0x0400089E RID: 2206
		S,
		// Token: 0x0400089F RID: 2207
		M,
		// Token: 0x040008A0 RID: 2208
		L,
		// Token: 0x040008A1 RID: 2209
		XL
	}
}
