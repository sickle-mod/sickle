using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000187 RID: 391
	public class JSONNumber : JSONNode
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0002FB18 File Offset: 0x0002DD18
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Number;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool IsNumber
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0002FB1B File Offset: 0x0002DD1B
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0007F2C0 File Offset: 0x0007D4C0
		public override string Value
		{
			get
			{
				return this.m_Data.ToString();
			}
			set
			{
				double num;
				if (double.TryParse(value, out num))
				{
					this.m_Data = num;
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0002FB28 File Offset: 0x0002DD28
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x0002FB30 File Offset: 0x0002DD30
		public override double AsDouble
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0002FB39 File Offset: 0x0002DD39
		public JSONNumber(double aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0002FB48 File Offset: 0x0002DD48
		public JSONNumber(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002FB57 File Offset: 0x0002DD57
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(4);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0002FB6C File Offset: 0x0002DD6C
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0007F2E0 File Offset: 0x0007D4E0
		private static bool IsNumeric(object value)
		{
			return value is int || value is uint || value is float || value is double || value is decimal || value is long || value is ulong || value is short || value is ushort || value is sbyte || value is byte;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0007F348 File Offset: 0x0007D548
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (base.Equals(obj))
			{
				return true;
			}
			JSONNumber jsonnumber = obj as JSONNumber;
			if (jsonnumber != null)
			{
				return this.m_Data == jsonnumber.m_Data;
			}
			return JSONNumber.IsNumeric(obj) && Convert.ToDouble(obj) == this.m_Data;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0002FB7B File Offset: 0x0002DD7B
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000972 RID: 2418
		private double m_Data;
	}
}
