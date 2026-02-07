using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000189 RID: 393
	public class JSONNull : JSONNode
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0002FC16 File Offset: 0x0002DE16
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.NullValue;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool IsNull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0002FC19 File Offset: 0x0002DE19
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x00027EF0 File Offset: 0x000260F0
		public override string Value
		{
			get
			{
				return "null";
			}
			set
			{
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0002A1D9 File Offset: 0x000283D9
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x00027EF0 File Offset: 0x000260F0
		public override bool AsBool
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0002FC20 File Offset: 0x0002DE20
		public override bool Equals(object obj)
		{
			return this == obj || obj is JSONNull;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002FC31 File Offset: 0x0002DE31
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(5);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002FC3A File Offset: 0x0002DE3A
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}
	}
}
