using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000186 RID: 390
	public class JSONString : JSONNode
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0002FAB1 File Offset: 0x0002DCB1
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.String;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool IsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0002FAB4 File Offset: 0x0002DCB4
		// (set) Token: 0x06000B56 RID: 2902 RVA: 0x0002FABC File Offset: 0x0002DCBC
		public override string Value
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

		// Token: 0x06000B57 RID: 2903 RVA: 0x0002FAC5 File Offset: 0x0002DCC5
		public JSONString(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002FAD4 File Offset: 0x0002DCD4
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0002FAE9 File Offset: 0x0002DCE9
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0007F26C File Offset: 0x0007D46C
		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				return true;
			}
			string text = obj as string;
			if (text != null)
			{
				return this.m_Data == text;
			}
			JSONString jsonstring = obj as JSONString;
			return jsonstring != null && this.m_Data == jsonstring.m_Data;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002FB0B File Offset: 0x0002DD0B
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000971 RID: 2417
		private string m_Data;
	}
}
