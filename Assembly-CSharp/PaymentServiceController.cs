using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class PaymentServiceController : MonoBehaviour
{
	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060003C3 RID: 963 RVA: 0x0002A590 File Offset: 0x00028790
	// (set) Token: 0x060003C4 RID: 964 RVA: 0x0002A597 File Offset: 0x00028797
	public static PaymentServiceController Instance
	{
		get
		{
			return PaymentServiceController.instance;
		}
		private set
		{
			PaymentServiceController.instance = value;
		}
	}

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x060003C5 RID: 965 RVA: 0x0006287C File Offset: 0x00060A7C
	// (remove) Token: 0x060003C6 RID: 966 RVA: 0x000628B4 File Offset: 0x00060AB4
	public event Action<string> ProductUnlocked;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x060003C7 RID: 967 RVA: 0x000628EC File Offset: 0x00060AEC
	// (remove) Token: 0x060003C8 RID: 968 RVA: 0x00062924 File Offset: 0x00060B24
	public event Action<string> ProductPurchasedFailed;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x060003C9 RID: 969 RVA: 0x0006295C File Offset: 0x00060B5C
	// (remove) Token: 0x060003CA RID: 970 RVA: 0x00062994 File Offset: 0x00060B94
	public event Action ProductsRestored;

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x060003CB RID: 971 RVA: 0x000629CC File Offset: 0x00060BCC
	// (remove) Token: 0x060003CC RID: 972 RVA: 0x00062A04 File Offset: 0x00060C04
	public event Action<string> ProductsRestoreFailed;

	// Token: 0x060003CD RID: 973 RVA: 0x00062A3C File Offset: 0x00060C3C
	protected void Awake()
	{
		if (PaymentServiceController.Instance == null)
		{
			PaymentServiceController.Instance = this;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.paymentService = PaymentServiceFactory.GetPaymentServiceForCurrentPlatform();
			this.paymentService.SetPurchaseEvents(new Action<string>(this.OnProductUnlocked), new Action<string>(this.OnProductPurchaseError));
			this.paymentService.SetRestorePurchaseEvents(new Action(this.OnProductsRestored), new Action<string>(this.OnProductsRestoreFailed));
			return;
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0002A59F File Offset: 0x0002879F
	public void AuthenticatePaymentService(Action onSuccess, Action<string> OnFailure)
	{
		this.paymentService.Authenticate(onSuccess, OnFailure);
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0002A5AE File Offset: 0x000287AE
	public bool IsConnected()
	{
		return this.paymentService.IsConnected();
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0002A5BB File Offset: 0x000287BB
	public string DLCId(DLCs dlc)
	{
		return this.paymentService.DLCId(dlc);
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0002A5C9 File Offset: 0x000287C9
	public string ProductPrice(string productId)
	{
		return this.paymentService.ProductPrice(productId);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0002A5D7 File Offset: 0x000287D7
	public void BuyProduct(string productId)
	{
		this.paymentService.BuyProduct(productId);
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0002A5E5 File Offset: 0x000287E5
	public bool IsProductPurchased(string productId)
	{
		return this.paymentService.IsProductPurchased(productId);
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0002A5F3 File Offset: 0x000287F3
	public void RestorePurchases()
	{
		this.paymentService.RestorePurchases(true);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0002A601 File Offset: 0x00028801
	private void OnProductUnlocked(string productId)
	{
		if (this.ProductUnlocked != null)
		{
			this.ProductUnlocked(productId);
		}
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0002A617 File Offset: 0x00028817
	private void OnProductPurchaseError(string error)
	{
		if (this.ProductPurchasedFailed != null)
		{
			this.ProductPurchasedFailed(error);
		}
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0002A62D File Offset: 0x0002882D
	private void OnProductsRestored()
	{
		if (this.ProductsRestored != null)
		{
			this.ProductsRestored();
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0002A642 File Offset: 0x00028842
	private void OnProductsRestoreFailed(string error)
	{
		if (this.ProductsRestoreFailed != null)
		{
			this.ProductsRestoreFailed(error);
		}
	}

	// Token: 0x0400034E RID: 846
	private IPaymentService paymentService;

	// Token: 0x0400034F RID: 847
	private static PaymentServiceController instance;
}
