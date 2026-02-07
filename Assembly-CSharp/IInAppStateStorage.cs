using System;

// Token: 0x0200006B RID: 107
internal interface IInAppStateStorage
{
	// Token: 0x06000395 RID: 917
	void StoreUnlockedProduct(string productId);

	// Token: 0x06000396 RID: 918
	bool IsProductUnlocked(string productId);

	// Token: 0x06000397 RID: 919
	void ClearCachedProduct(string productId);
}
