using System;

// Token: 0x0200006A RID: 106
public class DummyPaymentService : IPaymentService
{
	// Token: 0x0600038B RID: 907 RVA: 0x0002A1AB File Offset: 0x000283AB
	public void Authenticate(Action onSuccess, Action<string> onFailure)
	{
		UniversalInvocator.Event_Invocator(onSuccess);
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void BuyProduct(string productId)
	{
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0002A1DC File Offset: 0x000283DC
	public string DLCId(DLCs dlc)
	{
		return string.Empty;
	}

	// Token: 0x0600038E RID: 910 RVA: 0x000283F8 File Offset: 0x000265F8
	public bool IsConnected()
	{
		return true;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x000283F8 File Offset: 0x000265F8
	public bool IsProductPurchased(string productId)
	{
		return true;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0002A1DC File Offset: 0x000283DC
	public string ProductPrice(string productId)
	{
		return string.Empty;
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void RestorePurchases(bool forceRefresh = false)
	{
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void SetPurchaseEvents(Action<string> onSuccess, Action<string> onFailure)
	{
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void SetRestorePurchaseEvents(Action onSuccess, Action<string> onFailure)
	{
	}
}
