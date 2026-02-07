using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class PlayerPrefsInAppStateStorage : IInAppStateStorage
{
	// Token: 0x060003DB RID: 987 RVA: 0x0002A65F File Offset: 0x0002885F
	public bool IsProductUnlocked(string productId)
	{
		return PlayerPrefs.HasKey("InApp_Unlocked_" + productId);
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0002A671 File Offset: 0x00028871
	public void ClearCachedProduct(string productId)
	{
		if (PlayerPrefs.HasKey("InApp_Unlocked_" + productId))
		{
			PlayerPrefs.DeleteKey("InApp_Unlocked_" + productId);
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0002A695 File Offset: 0x00028895
	public void StoreUnlockedProduct(string productId)
	{
		PlayerPrefs.SetString("InApp_Unlocked_" + productId, string.Empty);
	}

	// Token: 0x04000354 RID: 852
	private const string PLAYER_PREFS_KEY = "InApp_Unlocked_";
}
