using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class BillingServicesActionType : MonoBehaviour
{
	// Token: 0x02000069 RID: 105
	public enum BillingServicesAction
	{
		// Token: 0x0400033E RID: 830
		InitializeStore,
		// Token: 0x0400033F RID: 831
		CanMakePayments,
		// Token: 0x04000340 RID: 832
		IsProductPurchased,
		// Token: 0x04000341 RID: 833
		BuyProduct,
		// Token: 0x04000342 RID: 834
		GetTransactions,
		// Token: 0x04000343 RID: 835
		FinishTransactions,
		// Token: 0x04000344 RID: 836
		RestorePurchases,
		// Token: 0x04000345 RID: 837
		ResourcePage
	}
}
