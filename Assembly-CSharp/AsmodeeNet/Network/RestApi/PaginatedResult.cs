using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008E3 RID: 2275
	public class PaginatedResult<T> where T : class
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06003DC3 RID: 15811 RVA: 0x0004FB89 File Offset: 0x0004DD89
		// (set) Token: 0x06003DC4 RID: 15812 RVA: 0x0004FB91 File Offset: 0x0004DD91
		public Action<Action<PaginatedResult<T>, WebError>> Next { get; private set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06003DC5 RID: 15813 RVA: 0x0004FB9A File Offset: 0x0004DD9A
		// (set) Token: 0x06003DC6 RID: 15814 RVA: 0x0004FBA2 File Offset: 0x0004DDA2
		public Action<Action<PaginatedResult<T>, WebError>> Prev { get; private set; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x0004FBAB File Offset: 0x0004DDAB
		// (set) Token: 0x06003DC8 RID: 15816 RVA: 0x0004FBB3 File Offset: 0x0004DDB3
		public Action<Action<PaginatedResult<T>, WebError>> First { get; private set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06003DC9 RID: 15817 RVA: 0x0004FBBC File Offset: 0x0004DDBC
		// (set) Token: 0x06003DCA RID: 15818 RVA: 0x0004FBC4 File Offset: 0x0004DDC4
		public Action<Action<PaginatedResult<T>, WebError>> Last { get; private set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06003DCB RID: 15819 RVA: 0x0004FBCD File Offset: 0x0004DDCD
		// (set) Token: 0x06003DCC RID: 15820 RVA: 0x0004FBD5 File Offset: 0x0004DDD5
		public int TotalElement { get; private set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06003DCD RID: 15821 RVA: 0x0004FBDE File Offset: 0x0004DDDE
		// (set) Token: 0x06003DCE RID: 15822 RVA: 0x0004FBE6 File Offset: 0x0004DDE6
		public T[] Elements { get; private set; }

		// Token: 0x06003DCF RID: 15823 RVA: 0x0004FBEF File Offset: 0x0004DDEF
		public PaginatedResult(int totalElement, T[] elements, Action<Action<PaginatedResult<T>, WebError>> next, Action<Action<PaginatedResult<T>, WebError>> prev, Action<Action<PaginatedResult<T>, WebError>> first, Action<Action<PaginatedResult<T>, WebError>> last)
		{
			this.TotalElement = totalElement;
			this.Elements = elements;
			this.Next = next;
			this.Prev = prev;
			this.First = first;
			this.Last = last;
		}
	}
}
