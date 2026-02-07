using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000182 RID: 386
	public class JSONObject : JSONNode, IEnumerable
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0002F95C File Offset: 0x0002DB5C
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Object;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool IsObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CB RID: 203
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
					return;
				}
				this.m_Dict.Add(aKey, value);
			}
		}

		// Token: 0x170000CC RID: 204
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (value == null)
				{
					value = new JSONNull();
				}
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0002F9BE File Offset: 0x0002DBBE
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0007EDB0 File Offset: 0x0007CFB0
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = new JSONNull();
			}
			if (string.IsNullOrEmpty(aKey))
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
				return;
			}
			if (this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict[aKey] = aItem;
				return;
			}
			this.m_Dict.Add(aKey, aItem);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0002F9CB File Offset: 0x0002DBCB
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode jsonnode = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return jsonnode;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0007EE20 File Offset: 0x0007D020
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0007EE68 File Offset: 0x0007D068
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode jsonnode;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.Where((KeyValuePair<string, JSONNode> k) => k.Value == aNode).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				jsonnode = aNode;
			}
			catch
			{
				jsonnode = null;
			}
			return jsonnode;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0002F9F6 File Offset: 0x0002DBF6
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
				{
					yield return keyValuePair.Value;
				}
				Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002FA06 File Offset: 0x0002DC06
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				yield return keyValuePair;
			}
			Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0007EED4 File Offset: 0x0007D0D4
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(2);
			aWriter.Write(this.m_Dict.Count);
			foreach (string text in this.m_Dict.Keys)
			{
				aWriter.Write(text);
				this.m_Dict[text].Serialize(aWriter);
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0007EF58 File Offset: 0x0007D158
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('{');
			bool flag = true;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (!flag)
				{
					aSB.Append(',');
				}
				flag = false;
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				aSB.Append('"').Append(JSONNode.Escape(keyValuePair.Key)).Append('"');
				if (aMode == JSONTextMode.Compact)
				{
					aSB.Append(':');
				}
				else
				{
					aSB.Append(" : ");
				}
				keyValuePair.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append('}');
		}

		// Token: 0x04000965 RID: 2405
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		// Token: 0x04000966 RID: 2406
		public bool inline;
	}
}
