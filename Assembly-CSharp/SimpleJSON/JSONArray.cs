using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200017F RID: 383
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x000283F8 File Offset: 0x000265F8
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Array;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C1 RID: 193
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				this.m_List.Add(value);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x0002F86A File Offset: 0x0002DA6A
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002F84C File Offset: 0x0002DA4C
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = new JSONNull();
			}
			this.m_List.Add(aItem);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002F877 File Offset: 0x0002DA77
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode jsonnode = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return jsonnode;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002F8A5 File Offset: 0x0002DAA5
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0002F8B5 File Offset: 0x0002DAB5
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002F8C5 File Offset: 0x0002DAC5
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode jsonnode in this.m_List)
			{
				yield return jsonnode;
			}
			List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0007EA30 File Offset: 0x0007CC30
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0007EA80 File Offset: 0x0007CC80
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('[');
			int count = this.m_List.Count;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
				{
					aSB.Append(',');
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				this.m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append(']');
		}

		// Token: 0x0400095A RID: 2394
		private List<JSONNode> m_List = new List<JSONNode>();

		// Token: 0x0400095B RID: 2395
		public bool inline;
	}
}
