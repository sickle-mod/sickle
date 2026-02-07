using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class SaveBackgroundIndex : MonoBehaviour
{
	// Token: 0x06000326 RID: 806 RVA: 0x0002A18E File Offset: 0x0002838E
	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt(SaveBackgroundIndex.BackgroundPrefs, SaveBackgroundIndex.BackgroundIndex);
	}

	// Token: 0x04000327 RID: 807
	public static string BackgroundPrefs = "BackgroundIndex";

	// Token: 0x04000328 RID: 808
	public static int BackgroundIndex;
}
