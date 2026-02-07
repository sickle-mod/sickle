using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000188 RID: 392
	public class JSONBool : JSONNode
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0002FB88 File Offset: 0x0002DD88
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Boolean;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool IsBoolean
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0002FB8B File Offset: 0x0002DD8B
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x0007F39C File Offset: 0x0007D59C
		public override string Value
		{
			get
			{
				return this.m_Data.ToString();
			}
			set
			{
				bool flag;
				if (bool.TryParse(value, out flag))
				{
					this.m_Data = flag;
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0002FB98 File Offset: 0x0002DD98
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0002FBA0 File Offset: 0x0002DDA0
		public override bool AsBool
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

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002FBA9 File Offset: 0x0002DDA9
		public JSONBool(bool aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002FB48 File Offset: 0x0002DD48
		public JSONBool(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0002FBB8 File Offset: 0x0002DDB8
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(6);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002FBCD File Offset: 0x0002DDCD
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data ? "true" : "false");
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002FBEA File Offset: 0x0002DDEA
		public override bool Equals(object obj)
		{
			return obj != null && obj is bool && this.m_Data == (bool)obj;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002FC09 File Offset: 0x0002DE09
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000973 RID: 2419
		private bool m_Data;
	}
}
