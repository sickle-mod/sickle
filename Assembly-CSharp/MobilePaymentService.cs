using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

// Token: 0x0200006D RID: 109
public class MobilePaymentService : IPaymentService
{
	// Token: 0x14000020 RID: 32
	// (add) Token: 0x060003A1 RID: 929 RVA: 0x00062224 File Offset: 0x00060424
	// (remove) Token: 0x060003A2 RID: 930 RVA: 0x0006225C File Offset: 0x0006045C
	private event Action AuthenticationSucced;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060003A3 RID: 931 RVA: 0x00062294 File Offset: 0x00060494
	// (remove) Token: 0x060003A4 RID: 932 RVA: 0x000622CC File Offset: 0x000604CC
	private event Action<string> AuthenticationFailed;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x060003A5 RID: 933 RVA: 0x00062304 File Offset: 0x00060504
	// (remove) Token: 0x060003A6 RID: 934 RVA: 0x0006233C File Offset: 0x0006053C
	private event Action<string> ProductUnlocked;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x060003A7 RID: 935 RVA: 0x00062374 File Offset: 0x00060574
	// (remove) Token: 0x060003A8 RID: 936 RVA: 0x000623AC File Offset: 0x000605AC
	private event Action PurchasesRestored;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x060003A9 RID: 937 RVA: 0x000623E4 File Offset: 0x000605E4
	// (remove) Token: 0x060003AA RID: 938 RVA: 0x0006241C File Offset: 0x0006061C
	private event Action<string> PurchasesRestoreFailed;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x060003AB RID: 939 RVA: 0x00062454 File Offset: 0x00060654
	// (remove) Token: 0x060003AC RID: 940 RVA: 0x0006248C File Offset: 0x0006068C
	private event Action<string> PurchaseFailed;

	// Token: 0x060003AD RID: 941 RVA: 0x000624C4 File Offset: 0x000606C4
	public MobilePaymentService()
	{
		this.inAppStateStorage = new PlayerPrefsInAppStateStorage();
		BillingServices.OnRestorePurchasesComplete += this.OnRestoreTransactionsCompleted;
		BillingServices.OnInitializeStoreComplete += this.ProcessPaymentServerAuthentication;
		BillingServices.OnTransactionStateChange += this.OnTransactionStateChange;
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0002A4A4 File Offset: 0x000286A4
	public void Authenticate(Action onSuccess, Action<string> onFailure)
	{
		this.AuthenticationSucced = onSuccess;
		this.AuthenticationFailed = onFailure;
		BillingServices.InitializeStore();
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0002A4B9 File Offset: 0x000286B9
	public void SetPurchaseEvents(Action<string> onSuccess, Action<string> onFailure)
	{
		this.ProductUnlocked = onSuccess;
		this.PurchaseFailed = onFailure;
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0002A4C9 File Offset: 0x000286C9
	public bool IsConnected()
	{
		return BillingServices.IsAvailable();
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0002A4D0 File Offset: 0x000286D0
	public void BuyProduct(string productId)
	{
		BillingServices.BuyProduct(productId, BuyProductOptions.Default);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0002A4DD File Offset: 0x000286DD
	public string DLCId(DLCs dlc)
	{
		if (dlc == DLCs.InvadersFromAfar)
		{
			return "com.asmodeedigital.scythe.invadersfromafar";
		}
		throw new ArgumentOutOfRangeException("Not supported dlc: " + dlc.ToString());
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00062518 File Offset: 0x00060718
	public string ProductPrice(string productId)
	{
		IBillingProduct product = this.GetProduct(productId);
		if (product == null)
		{
			return string.Empty;
		}
		return product.Price.LocalizedText;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0002A504 File Offset: 0x00028704
	public bool IsProductPurchased(string productId)
	{
		return this.inAppStateStorage != null && this.inAppStateStorage.IsProductUnlocked(productId);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0002A51C File Offset: 0x0002871C
	public void RestorePurchases(bool forceRefresh = false)
	{
		BillingServices.RestorePurchases(forceRefresh, null);
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0002A525 File Offset: 0x00028725
	public void SetRestorePurchaseEvents(Action onSuccess, Action<string> onFailure)
	{
		this.PurchasesRestored = onSuccess;
		this.PurchasesRestoreFailed = onFailure;
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00062544 File Offset: 0x00060744
	private void ProcessPaymentServerAuthentication(BillingServicesInitializeStoreResult result, Error error)
	{
		if (error == null)
		{
			this.AuthenticationLogProducts(result.Products);
			if (PlatformManager.IsAndroid)
			{
				this.RestorePurchases(false);
			}
			this.AuthenticationSucced();
		}
		else
		{
			this.AuthenticationFailed(error.Description);
			Debug.Log("Store initialization failed with error. Error: " + ((error != null) ? error.ToString() : null));
		}
		this.AuthenticationLogInvalidProducts(result.InvalidProductIds);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x000625B4 File Offset: 0x000607B4
	private void OnTransactionStateChange(BillingServicesTransactionStateChangeResult result)
	{
		foreach (IBillingTransaction billingTransaction in result.Transactions)
		{
			BillingTransactionState transactionState = billingTransaction.TransactionState;
			if (transactionState != BillingTransactionState.Purchased)
			{
				if (transactionState == BillingTransactionState.Failed)
				{
					Debug.Log(string.Format("Buy product with id:{0} failed with error. Error: {1}", billingTransaction.Product.Id, billingTransaction.Error));
					this.OnProductPurchasedError(billingTransaction.Error.Description);
				}
			}
			else
			{
				Debug.Log(string.Format("Buy product with id:{0} finished successfully with verification state {1}.", billingTransaction.Product.Id, billingTransaction.ReceiptVerificationState));
				this.OnProductUnlocked(billingTransaction.Product.Id);
			}
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0002A535 File Offset: 0x00028735
	private IBillingProduct GetProduct(string productId)
	{
		return BillingServices.GetProductWithId(productId, false);
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0006265C File Offset: 0x0006085C
	private void OnRestoreTransactionsCompleted(BillingServicesRestorePurchasesResult result, Error error)
	{
		if (error == null)
		{
			this.RestoredLogProducts(result.Transactions);
			this.CacheRestoredProducts(result.Transactions);
			if (this.PurchasesRestored != null)
			{
				this.PurchasesRestored();
				return;
			}
		}
		else if (this.PurchasesRestoreFailed != null)
		{
			this.PurchasesRestoreFailed(error.Description);
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0002A53E File Offset: 0x0002873E
	private void OnProductUnlocked(string productId)
	{
		this.inAppStateStorage.StoreUnlockedProduct(productId);
		if (this.ProductUnlocked != null)
		{
			this.ProductUnlocked(productId);
		}
		Debug.Log("[MobilePayhmentService] OnProductUnlocked()" + productId);
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0002A570 File Offset: 0x00028770
	private void OnProductPurchasedError(string error)
	{
		if (this.PurchaseFailed != null)
		{
			this.PurchaseFailed(error);
		}
		Debug.Log("[MobilePayhmentService] OnProductPurchasedError()");
	}

	// Token: 0x060003BD RID: 957 RVA: 0x000626B4 File Offset: 0x000608B4
	private void RestoredLogProducts(IBillingTransaction[] transactions)
	{
		Debug.Log("Request to restore purchases finished successfully.");
		Debug.Log("Total restored products: " + transactions.Length.ToString());
		for (int i = 0; i < transactions.Length; i++)
		{
			IBillingTransaction billingTransaction = transactions[i];
			Debug.Log(string.Format("[{0}]: {1}", i, billingTransaction.Product.Id));
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00062718 File Offset: 0x00060918
	private void CacheRestoredProducts(IBillingTransaction[] transactions)
	{
		this.ClearCachedProducts();
		foreach (IBillingTransaction billingTransaction in transactions)
		{
			this.inAppStateStorage.StoreUnlockedProduct(billingTransaction.Product.Id);
		}
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00062754 File Offset: 0x00060954
	private void AuthenticationLogProducts(IBillingProduct[] products)
	{
		Debug.Log("Store initialized successfully.");
		Debug.Log("Total products fetched: " + products.Length.ToString());
		Debug.Log("Below are the available products:");
		for (int i = 0; i < products.Length; i++)
		{
			IBillingProduct billingProduct = products[i];
			Debug.Log(string.Format("[{0}]: {1}", i, billingProduct));
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x000627B8 File Offset: 0x000609B8
	private void CacheProducts(IBillingProduct[] products)
	{
		foreach (IBillingProduct billingProduct in products)
		{
			this.inAppStateStorage.StoreUnlockedProduct(billingProduct.Id);
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x000627E8 File Offset: 0x000609E8
	private void AuthenticationLogInvalidProducts(string[] products)
	{
		Debug.Log("Total invalid products: " + products.Length.ToString());
		if (products.Length != 0)
		{
			Debug.Log("Here are the invalid product ids:");
			for (int i = 0; i < products.Length; i++)
			{
				Debug.Log(string.Format("[{0}]: {1}", i, products[i]));
			}
		}
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00062844 File Offset: 0x00060A44
	private void ClearCachedProducts()
	{
		foreach (IBillingProduct billingProduct in BillingServices.Products)
		{
			this.inAppStateStorage.ClearCachedProduct(billingProduct.Id);
		}
	}

	// Token: 0x0400034B RID: 843
	private IInAppStateStorage inAppStateStorage;

	// Token: 0x0400034D RID: 845
	private const string idIFADLC = "com.asmodeedigital.scythe.invadersfromafar";
}
