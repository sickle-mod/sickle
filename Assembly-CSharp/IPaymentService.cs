using System;

// Token: 0x0200006C RID: 108
public interface IPaymentService
{
	// Token: 0x06000398 RID: 920
	void Authenticate(Action onSuccess, Action<string> onFailure);

	// Token: 0x06000399 RID: 921
	void SetPurchaseEvents(Action<string> onSuccess, Action<string> onFailure);

	// Token: 0x0600039A RID: 922
	bool IsConnected();

	// Token: 0x0600039B RID: 923
	string DLCId(DLCs dlc);

	// Token: 0x0600039C RID: 924
	string ProductPrice(string productId);

	// Token: 0x0600039D RID: 925
	void BuyProduct(string productId);

	// Token: 0x0600039E RID: 926
	bool IsProductPurchased(string productId);

	// Token: 0x0600039F RID: 927
	void RestorePurchases(bool forceRefresh = false);

	// Token: 0x060003A0 RID: 928
	void SetRestorePurchaseEvents(Action onSuccess, Action<string> onFailure);
}
