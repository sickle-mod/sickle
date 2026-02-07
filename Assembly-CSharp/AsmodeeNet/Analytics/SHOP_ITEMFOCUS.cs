using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000997 RID: 2455
	public struct SHOP_ITEMFOCUS
	{
		// Token: 0x04003232 RID: 12850
		public string shop_session_id;

		// Token: 0x04003233 RID: 12851
		public string entry_point;

		// Token: 0x04003234 RID: 12852
		public string item_id;

		// Token: 0x04003235 RID: 12853
		public float item_price;

		// Token: 0x04003236 RID: 12854
		public string item_currency;

		// Token: 0x04003237 RID: 12855
		public int item_quantity;

		// Token: 0x04003238 RID: 12856
		public bool item_view;

		// Token: 0x04003239 RID: 12857
		public bool item_purchase;

		// Token: 0x0400323A RID: 12858
		public bool item_purchase_confirmed_user;

		// Token: 0x0400323B RID: 12859
		public bool item_purchase_confirmed_first_party;

		// Token: 0x0400323C RID: 12860
		public string transaction_backend_id;

		// Token: 0x0400323D RID: 12861
		public string transaction_first_party_id;

		// Token: 0x0400323E RID: 12862
		public float transaction_price;

		// Token: 0x0400323F RID: 12863
		public string transaction_currency;

		// Token: 0x04003240 RID: 12864
		public bool purchases_outside_shop;

		// Token: 0x04003241 RID: 12865
		public bool? is_default_item;
	}
}
