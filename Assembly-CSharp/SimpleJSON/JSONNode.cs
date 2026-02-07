using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200017C RID: 380
	public abstract class JSONNode
	{
		// Token: 0x170000A8 RID: 168
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170000A9 RID: 169
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0002F5FA File Offset: 0x0002D7FA
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual bool IsNumber
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual bool IsNull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public virtual bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002F601 File Offset: 0x0002D801
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002B03D File Offset: 0x0002923D
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0002F60F File Offset: 0x0002D80F
		public virtual IEnumerable<JSONNode> Children
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0002F618 File Offset: 0x0002D818
		public IEnumerable<JSONNode> DeepChildren
		{
			get
			{
				foreach (JSONNode jsonnode in this.Children)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChildren)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0007E00C File Offset: 0x0007C20C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, 0, JSONTextMode.Compact);
			return stringBuilder.ToString();
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0007E030 File Offset: 0x0007C230
		public virtual string ToString(int aIndent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, aIndent, JSONTextMode.Indent);
			return stringBuilder.ToString();
		}

		// Token: 0x06000AD3 RID: 2771
		internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000AD4 RID: 2772
		public abstract JSONNodeType Tag { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0007E054 File Offset: 0x0007C254
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0002F628 File Offset: 0x0002D828
		public virtual double AsDouble
		{
			get
			{
				double num = 0.0;
				if (double.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0002F637 File Offset: 0x0002D837
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x0002F640 File Offset: 0x0002D840
		public virtual int AsInt
		{
			get
			{
				return (int)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0002F64A File Offset: 0x0002D84A
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x0002F640 File Offset: 0x0002D840
		public virtual float AsFloat
		{
			get
			{
				return (float)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0007E088 File Offset: 0x0007C288
		// (set) Token: 0x06000ADC RID: 2780 RVA: 0x0002F653 File Offset: 0x0002D853
		public virtual bool AsBool
		{
			get
			{
				bool flag = false;
				if (bool.TryParse(this.Value, out flag))
				{
					return flag;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0002F66A File Offset: 0x0002D86A
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0002F672 File Offset: 0x0002D872
		public virtual JSONObject AsObject
		{
			get
			{
				return this as JSONObject;
			}
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002F67A File Offset: 0x0002D87A
		public static implicit operator JSONNode(string s)
		{
			return new JSONString(s);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002F682 File Offset: 0x0002D882
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002F695 File Offset: 0x0002D895
		public static implicit operator JSONNode(double n)
		{
			return new JSONNumber(n);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002F69D File Offset: 0x0002D89D
		public static implicit operator double(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsDouble;
			}
			return 0.0;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002F6B8 File Offset: 0x0002D8B8
		public static implicit operator JSONNode(float n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002F6C1 File Offset: 0x0002D8C1
		public static implicit operator float(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsFloat;
			}
			return 0f;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002F6B8 File Offset: 0x0002D8B8
		public static implicit operator JSONNode(int n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002F6D8 File Offset: 0x0002D8D8
		public static implicit operator int(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsInt;
			}
			return 0;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002F6EB File Offset: 0x0002D8EB
		public static implicit operator JSONNode(bool b)
		{
			return new JSONBool(b);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002F6F3 File Offset: 0x0002D8F3
		public static implicit operator bool(JSONNode d)
		{
			return !(d == null) && d.AsBool;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0007E0B8 File Offset: 0x0007C2B8
		public static bool operator ==(JSONNode a, object b)
		{
			if (a == b)
			{
				return true;
			}
			bool flag = a is JSONNull || a == null || a is JSONLazyCreator;
			bool flag2 = b is JSONNull || b == null || b is JSONLazyCreator;
			return (flag && flag2) || a.Equals(b);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002F706 File Offset: 0x0002D906
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0002F712 File Offset: 0x0002D912
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0002F718 File Offset: 0x0002D918
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0007E108 File Offset: 0x0007C308
		internal static string Escape(string aText)
		{
			JSONNode.m_EscapeBuilder.Length = 0;
			if (JSONNode.m_EscapeBuilder.Capacity < aText.Length + aText.Length / 10)
			{
				JSONNode.m_EscapeBuilder.Capacity = aText.Length + aText.Length / 10;
			}
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					JSONNode.m_EscapeBuilder.Append("\\b");
					break;
				case '\t':
					JSONNode.m_EscapeBuilder.Append("\\t");
					break;
				case '\n':
					JSONNode.m_EscapeBuilder.Append("\\n");
					break;
				case '\v':
					goto IL_00FA;
				case '\f':
					JSONNode.m_EscapeBuilder.Append("\\f");
					break;
				case '\r':
					JSONNode.m_EscapeBuilder.Append("\\r");
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_00FA;
						}
						JSONNode.m_EscapeBuilder.Append("\\\\");
					}
					else
					{
						JSONNode.m_EscapeBuilder.Append("\\\"");
					}
					break;
				}
				IL_0106:
				i++;
				continue;
				IL_00FA:
				JSONNode.m_EscapeBuilder.Append(c);
				goto IL_0106;
			}
			string text = JSONNode.m_EscapeBuilder.ToString();
			JSONNode.m_EscapeBuilder.Length = 0;
			return text;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0007E240 File Offset: 0x0007C440
		private static void ParseElement(JSONNode ctx, string token, string tokenName, bool quoted)
		{
			if (quoted)
			{
				ctx.Add(tokenName, token);
				return;
			}
			string text = token.ToLower();
			if (text == "false" || text == "true")
			{
				ctx.Add(tokenName, text == "true");
				return;
			}
			if (text == "null")
			{
				ctx.Add(tokenName, null);
				return;
			}
			double num;
			if (double.TryParse(token, out num))
			{
				ctx.Add(tokenName, num);
				return;
			}
			ctx.Add(tokenName, token);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0007E2D4 File Offset: 0x0007C4D4
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			bool flag = false;
			bool flag2 = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_033E;
						case '\v':
						case '\f':
							goto IL_0330;
						default:
							if (c != ' ')
							{
								goto IL_0330;
							}
							break;
						}
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_0330;
						}
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
						}
						else
						{
							if (stringBuilder.Length > 0 || flag2)
							{
								JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
							}
							text = "";
							stringBuilder.Length = 0;
							flag2 = false;
						}
					}
					else
					{
						flag = !flag;
						flag2 = flag2 || flag;
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									stringBuilder.Append(aJSON[i]);
									goto IL_033E;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									jsonnode.Add(text, stack.Peek());
								}
								text = "";
								stringBuilder.Length = 0;
								jsonnode = stack.Peek();
								goto IL_033E;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											stringBuilder.Append('\b');
											goto IL_033E;
										}
										if (c2 == 'f')
										{
											stringBuilder.Append('\f');
											goto IL_033E;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											stringBuilder.Append('\n');
											goto IL_033E;
										}
										switch (c2)
										{
										case 'r':
											stringBuilder.Append('\r');
											goto IL_033E;
										case 't':
											stringBuilder.Append('\t');
											goto IL_033E;
										case 'u':
										{
											string text2 = aJSON.Substring(i + 1, 4);
											stringBuilder.Append((char)int.Parse(text2, NumberStyles.AllowHexSpecifier));
											i += 4;
											goto IL_033E;
										}
										}
									}
									stringBuilder.Append(c2);
									goto IL_033E;
								}
								goto IL_033E;
							case ']':
								break;
							default:
								goto IL_0330;
							}
						}
						else
						{
							if (flag)
							{
								stringBuilder.Append(aJSON[i]);
								goto IL_033E;
							}
							text = stringBuilder.ToString();
							stringBuilder.Length = 0;
							flag2 = false;
							goto IL_033E;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_0330;
						}
					}
					else
					{
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
							goto IL_033E;
						}
						stack.Push(new JSONObject());
						if (jsonnode != null)
						{
							jsonnode.Add(text, stack.Peek());
						}
						text = "";
						stringBuilder.Length = 0;
						jsonnode = stack.Peek();
						goto IL_033E;
					}
					if (flag)
					{
						stringBuilder.Append(aJSON[i]);
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (stringBuilder.Length > 0 || flag2)
						{
							JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
							flag2 = false;
						}
						text = "";
						stringBuilder.Length = 0;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_033E:
				i++;
				continue;
				IL_0330:
				stringBuilder.Append(aJSON[i]);
				goto IL_033E;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0007E640 File Offset: 0x0007C840
		public void SaveToStream(Stream aData)
		{
			BinaryWriter binaryWriter = new BinaryWriter(aData);
			this.Serialize(binaryWriter);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002F720 File Offset: 0x0002D920
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002F720 File Offset: 0x0002D920
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002F720 File Offset: 0x0002D920
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0007E65C File Offset: 0x0007C85C
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0007E6AC File Offset: 0x0007C8AC
		public string SaveToBase64()
		{
			string text;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				text = Convert.ToBase64String(memoryStream.ToArray());
			}
			return text;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0007E6F8 File Offset: 0x0007C8F8
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONNodeType jsonnodeType = (JSONNodeType)aReader.ReadByte();
			switch (jsonnodeType)
			{
			case JSONNodeType.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONNodeType.Object:
			{
				int num2 = aReader.ReadInt32();
				JSONObject jsonobject = new JSONObject();
				for (int j = 0; j < num2; j++)
				{
					string text = aReader.ReadString();
					JSONNode jsonnode = JSONNode.Deserialize(aReader);
					jsonobject.Add(text, jsonnode);
				}
				return jsonobject;
			}
			case JSONNodeType.String:
				return new JSONString(aReader.ReadString());
			case JSONNodeType.Number:
				return new JSONNumber(aReader.ReadDouble());
			case JSONNodeType.NullValue:
				return new JSONNull();
			case JSONNodeType.Boolean:
				return new JSONBool(aReader.ReadBoolean());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonnodeType.ToString());
			}
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002F720 File Offset: 0x0002D920
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002F720 File Offset: 0x0002D920
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002F720 File Offset: 0x0002D920
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0007E7DC File Offset: 0x0007C9DC
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode jsonnode;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				jsonnode = JSONNode.Deserialize(binaryReader);
			}
			return jsonnode;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0007E814 File Offset: 0x0007CA14
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode jsonnode;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				jsonnode = JSONNode.LoadFromStream(fileStream);
			}
			return jsonnode;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002F72C File Offset: 0x0002D92C
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}

		// Token: 0x04000950 RID: 2384
		internal static StringBuilder m_EscapeBuilder = new StringBuilder();
	}
}
