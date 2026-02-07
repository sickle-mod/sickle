using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class CountTimeInGame : MonoBehaviour
{
	// Token: 0x06000833 RID: 2099 RVA: 0x0002D818 File Offset: 0x0002BA18
	private void Awake()
	{
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		CountTimeInGame.seconds = PlayerPrefs.GetInt(CountTimeInGame.PREFS_SECONDS, 0);
		CountTimeInGame.minutes = PlayerPrefs.GetInt(CountTimeInGame.PREFS_MINUTES, 0);
		CountTimeInGame.playtime = PlayerPrefs.GetInt(CountTimeInGame.PREFS_PLAYTIME, 0);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0002D855 File Offset: 0x0002BA55
	private void Start()
	{
		base.StartCoroutine(this.PlayerTimer());
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0002D864 File Offset: 0x0002BA64
	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt(CountTimeInGame.PREFS_SECONDS, CountTimeInGame.seconds);
		PlayerPrefs.SetInt(CountTimeInGame.PREFS_MINUTES, CountTimeInGame.minutes);
		PlayerPrefs.SetInt(CountTimeInGame.PREFS_PLAYTIME, CountTimeInGame.playtime);
		PlayerPrefs.Save();
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0002D898 File Offset: 0x0002BA98
	private IEnumerator PlayerTimer()
	{
		for (;;)
		{
			yield return new WaitForSeconds(1f);
			CountTimeInGame.playtime++;
			CountTimeInGame.seconds = CountTimeInGame.playtime % 60;
			CountTimeInGame.minutes = CountTimeInGame.playtime / 60 % 60;
		}
		yield break;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0002D8A0 File Offset: 0x0002BAA0
	public static void ResetGameTime()
	{
		PlayerPrefs.SetInt(CountTimeInGame.PREFS_SECONDS, 0);
		PlayerPrefs.SetInt(CountTimeInGame.PREFS_MINUTES, 0);
		PlayerPrefs.SetInt(CountTimeInGame.PREFS_PLAYTIME, 0);
	}

	// Token: 0x040006F7 RID: 1783
	public static int seconds = 0;

	// Token: 0x040006F8 RID: 1784
	public static int minutes = 0;

	// Token: 0x040006F9 RID: 1785
	public static int playtime = 0;

	// Token: 0x040006FA RID: 1786
	public static string PREFS_SECONDS = "Seconds";

	// Token: 0x040006FB RID: 1787
	public static string PREFS_MINUTES = "Minutes";

	// Token: 0x040006FC RID: 1788
	public static string PREFS_PLAYTIME = "Playtime";
}
