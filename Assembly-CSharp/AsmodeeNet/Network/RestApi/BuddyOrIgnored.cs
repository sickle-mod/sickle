using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008D5 RID: 2261
	[Serializable]
	public class BuddyOrIgnored
	{
		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06003D77 RID: 15735 RVA: 0x0004F892 File Offset: 0x0004DA92
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06003D78 RID: 15736 RVA: 0x0004F89A File Offset: 0x0004DA9A
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x0004F8A2 File Offset: 0x0004DAA2
		public BuddyOrIgnored()
		{
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x0004F8B1 File Offset: 0x0004DAB1
		public BuddyOrIgnored(int id, string name)
		{
			this._id = id;
			this._name = name;
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x00157F08 File Offset: 0x00156108
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			BuddyOrIgnored buddyOrIgnored = o as BuddyOrIgnored;
			return buddyOrIgnored != null && this.Id == buddyOrIgnored.Id && this.Name == buddyOrIgnored.Name;
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x0004F8CE File Offset: 0x0004DACE
		public override int GetHashCode()
		{
			return this.Id;
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x00157F48 File Offset: 0x00156148
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"this : Id : \"",
				this.Id.ToString(),
				"\",  Name : \"",
				this.Name,
				"\""
			});
		}

		// Token: 0x04002F49 RID: 12105
		[SerializeField]
		private int _id = -1;

		// Token: 0x04002F4A RID: 12106
		[SerializeField]
		private string _name;
	}
}
