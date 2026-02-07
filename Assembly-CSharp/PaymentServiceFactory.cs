using System;

// Token: 0x0200006F RID: 111
public static class PaymentServiceFactory
{
	// Token: 0x060003DA RID: 986 RVA: 0x0002A658 File Offset: 0x00028858
	public static IPaymentService GetPaymentServiceForCurrentPlatform()
	{
		return new DummyPaymentService();
	}
}
