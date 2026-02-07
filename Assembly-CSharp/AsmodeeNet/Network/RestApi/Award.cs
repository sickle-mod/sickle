using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008D3 RID: 2259
	[Serializable]
	public class Award
	{
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06003D67 RID: 15719 RVA: 0x0004F839 File Offset: 0x0004DA39
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06003D68 RID: 15720 RVA: 0x0004F841 File Offset: 0x0004DA41
		public string Tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06003D69 RID: 15721 RVA: 0x0004F849 File Offset: 0x0004DA49
		public int TableId
		{
			get
			{
				return this._tableId;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06003D6A RID: 15722 RVA: 0x0004F851 File Offset: 0x0004DA51
		public int InfoId
		{
			get
			{
				return this._infoId;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x0004F859 File Offset: 0x0004DA59
		public DateTime? AwardedUTC
		{
			get
			{
				return this._awardedUTC;
			}
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x00157BD4 File Offset: 0x00155DD4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Achievement : \n\tid : ",
				this.Id.ToString(),
				"\n\ttag : ",
				(this.Tag == null) ? "" : this.Tag,
				"\n\ttableId : ",
				this.TableId.ToString(),
				"\n\tinfoId : ",
				this.InfoId.ToString(),
				"\n\tAwardedUTC : ",
				(this.AwardedUTC == null) ? "?" : this.AwardedUTC.Value.ToString()
			});
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0004F861 File Offset: 0x0004DA61
		public Award()
		{
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x00157C94 File Offset: 0x00155E94
		public Award(int id, string tag, int tableId = -1, int infoId = -1, DateTime? awardedUTC = null)
		{
			if (tag != null)
			{
				if (tag.All((char x) => char.IsDigit(x)))
				{
					throw new ArgumentException("The \"tag\" parameter must not be only composed by digit characters");
				}
			}
			this._id = id;
			this._tag = tag;
			this._tableId = tableId;
			this._infoId = infoId;
			this._awardedUTC = awardedUTC;
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x00157D18 File Offset: 0x00155F18
		public Award(int id, int tableId = -1, int infoId = -1, DateTime? awardedUTC = null)
		{
			this._id = id;
			this._tag = null;
			this._tableId = tableId;
			this._infoId = infoId;
			this._awardedUTC = awardedUTC;
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x00157D64 File Offset: 0x00155F64
		public Award(string tag, int tableId = -1, int infoId = -1, DateTime? awardedUTC = null)
		{
			if (tag != null)
			{
				if (tag.All((char x) => char.IsDigit(x)))
				{
					throw new ArgumentException("The \"tag\" parameter must not be only composed by digit characters");
				}
			}
			this._id = -1;
			this._tag = tag;
			this._tableId = tableId;
			this._infoId = infoId;
			this._awardedUTC = awardedUTC;
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00157DE8 File Offset: 0x00155FE8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Award award = obj as Award;
			return award != null && (this.Tag != null || this.Id != -1) && (award.Tag != null || award.Id != -1) && (this.Tag == award.Tag && this.Id == award.Id && this.TableId == award.TableId && this.InfoId == award.InfoId) && this.AwardedUTC == award.AwardedUTC;
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x00157EA8 File Offset: 0x001560A8
		public override int GetHashCode()
		{
			return ((this.Tag == null) ? 0 : this.Tag.GetHashCode()) ^ ((this.AwardedUTC == null) ? 0 : this.AwardedUTC.GetHashCode()) ^ this.Id ^ this.InfoId ^ this.TableId;
		}

		// Token: 0x04002F41 RID: 12097
		[SerializeField]
		private int _id = -1;

		// Token: 0x04002F42 RID: 12098
		[SerializeField]
		private string _tag;

		// Token: 0x04002F43 RID: 12099
		[SerializeField]
		private int _tableId = -1;

		// Token: 0x04002F44 RID: 12100
		[SerializeField]
		private int _infoId = -1;

		// Token: 0x04002F45 RID: 12101
		private DateTime? _awardedUTC;
	}
}
