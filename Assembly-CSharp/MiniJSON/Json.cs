using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniJSON
{
	// Token: 0x02000782 RID: 1922
	public static class Json
	{
		// Token: 0x060037BA RID: 14266 RVA: 0x0004BBF9 File Offset: 0x00049DF9
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x0004BC06 File Offset: 0x00049E06
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x02000783 RID: 1923
		private sealed class Parser : IDisposable
		{
			// Token: 0x060037BC RID: 14268 RVA: 0x0004BC0E File Offset: 0x00049E0E
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x060037BD RID: 14269 RVA: 0x0004BC2B File Offset: 0x00049E2B
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x060037BE RID: 14270 RVA: 0x00147E54 File Offset: 0x00146054
			public static object Parse(string jsonString)
			{
				object obj;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					obj = parser.ParseValue();
				}
				return obj;
			}

			// Token: 0x060037BF RID: 14271 RVA: 0x0004BC3F File Offset: 0x00049E3F
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x060037C0 RID: 14272 RVA: 0x00147E8C File Offset: 0x0014608C
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						break;
					}
					if (nextToken == Json.Parser.TOKEN.CURLY_CLOSE)
					{
						return dictionary;
					}
					if (nextToken != Json.Parser.TOKEN.COMMA)
					{
						string text = this.ParseString();
						if (text == null)
						{
							goto Block_4;
						}
						if (this.NextToken != Json.Parser.TOKEN.COLON)
						{
							goto Block_5;
						}
						this.json.Read();
						dictionary[text] = this.ParseValue();
					}
				}
				return null;
				Block_4:
				return null;
				Block_5:
				return null;
			}

			// Token: 0x060037C1 RID: 14273 RVA: 0x00147EF4 File Offset: 0x001460F4
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						return null;
					}
					if (nextToken != Json.Parser.TOKEN.SQUARED_CLOSE)
					{
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							object obj = this.ParseByToken(nextToken);
							list.Add(obj);
						}
					}
					else
					{
						flag = false;
					}
				}
				return list;
			}

			// Token: 0x060037C2 RID: 14274 RVA: 0x00147F44 File Offset: 0x00146144
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x060037C3 RID: 14275 RVA: 0x00147F60 File Offset: 0x00146160
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case Json.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x060037C4 RID: 14276 RVA: 0x00147FD0 File Offset: 0x001461D0
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char c = this.NextChar;
					if (c != '"')
					{
						if (c != '\\')
						{
							stringBuilder.Append(c);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							c = this.NextChar;
							if (c <= '\\')
							{
								if (c == '"' || c == '/' || c == '\\')
								{
									stringBuilder.Append(c);
								}
							}
							else if (c <= 'f')
							{
								if (c != 'b')
								{
									if (c == 'f')
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
							}
							else if (c != 'n')
							{
								switch (c)
								{
								case 'r':
									stringBuilder.Append('\r');
									break;
								case 't':
									stringBuilder.Append('\t');
									break;
								case 'u':
								{
									char[] array = new char[4];
									for (int i = 0; i < 4; i++)
									{
										array[i] = this.NextChar;
									}
									stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
									break;
								}
								}
							}
							else
							{
								stringBuilder.Append('\n');
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060037C5 RID: 14277 RVA: 0x00148120 File Offset: 0x00146320
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x060037C6 RID: 14278 RVA: 0x0004BC53 File Offset: 0x00049E53
			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x060037C7 RID: 14279 RVA: 0x0004BC7E File Offset: 0x00049E7E
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000453 RID: 1107
			// (get) Token: 0x060037C8 RID: 14280 RVA: 0x0004BC90 File Offset: 0x00049E90
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000454 RID: 1108
			// (get) Token: 0x060037C9 RID: 14281 RVA: 0x00148160 File Offset: 0x00146360
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!Json.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x17000455 RID: 1109
			// (get) Token: 0x060037CA RID: 14282 RVA: 0x001481A4 File Offset: 0x001463A4
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return Json.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					if (peekChar <= '[')
					{
						switch (peekChar)
						{
						case '"':
							return Json.Parser.TOKEN.STRING;
						case '#':
						case '$':
						case '%':
						case '&':
						case '\'':
						case '(':
						case ')':
						case '*':
						case '+':
						case '.':
						case '/':
							break;
						case ',':
							this.json.Read();
							return Json.Parser.TOKEN.COMMA;
						case '-':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							return Json.Parser.TOKEN.NUMBER;
						case ':':
							return Json.Parser.TOKEN.COLON;
						default:
							if (peekChar == '[')
							{
								return Json.Parser.TOKEN.SQUARED_OPEN;
							}
							break;
						}
					}
					else
					{
						if (peekChar == ']')
						{
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
						if (peekChar == '{')
						{
							return Json.Parser.TOKEN.CURLY_OPEN;
						}
						if (peekChar == '}')
						{
							this.json.Read();
							return Json.Parser.TOKEN.CURLY_CLOSE;
						}
					}
					string nextWord = this.NextWord;
					if (nextWord == "false")
					{
						return Json.Parser.TOKEN.FALSE;
					}
					if (nextWord == "true")
					{
						return Json.Parser.TOKEN.TRUE;
					}
					if (!(nextWord == "null"))
					{
						return Json.Parser.TOKEN.NONE;
					}
					return Json.Parser.TOKEN.NULL;
				}
			}

			// Token: 0x040029D1 RID: 10705
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x040029D2 RID: 10706
			private StringReader json;

			// Token: 0x02000784 RID: 1924
			private enum TOKEN
			{
				// Token: 0x040029D4 RID: 10708
				NONE,
				// Token: 0x040029D5 RID: 10709
				CURLY_OPEN,
				// Token: 0x040029D6 RID: 10710
				CURLY_CLOSE,
				// Token: 0x040029D7 RID: 10711
				SQUARED_OPEN,
				// Token: 0x040029D8 RID: 10712
				SQUARED_CLOSE,
				// Token: 0x040029D9 RID: 10713
				COLON,
				// Token: 0x040029DA RID: 10714
				COMMA,
				// Token: 0x040029DB RID: 10715
				STRING,
				// Token: 0x040029DC RID: 10716
				NUMBER,
				// Token: 0x040029DD RID: 10717
				TRUE,
				// Token: 0x040029DE RID: 10718
				FALSE,
				// Token: 0x040029DF RID: 10719
				NULL
			}
		}

		// Token: 0x02000785 RID: 1925
		private sealed class Serializer
		{
			// Token: 0x060037CB RID: 14283 RVA: 0x0004BCA2 File Offset: 0x00049EA2
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x060037CC RID: 14284 RVA: 0x0004BCB5 File Offset: 0x00049EB5
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x060037CD RID: 14285 RVA: 0x001482C8 File Offset: 0x001464C8
			private void SerializeValue(object value)
			{
				if (value == null)
				{
					this.builder.Append("null");
					return;
				}
				string text;
				if ((text = value as string) != null)
				{
					this.SerializeString(text);
					return;
				}
				if (value is bool)
				{
					this.builder.Append(((bool)value) ? "true" : "false");
					return;
				}
				IList list;
				if ((list = value as IList) != null)
				{
					this.SerializeArray(list);
					return;
				}
				IDictionary dictionary;
				if ((dictionary = value as IDictionary) != null)
				{
					this.SerializeObject(dictionary);
					return;
				}
				if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
					return;
				}
				this.SerializeOther(value);
			}

			// Token: 0x060037CE RID: 14286 RVA: 0x0014836C File Offset: 0x0014656C
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (object obj2 in obj.Keys)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x060037CF RID: 14287 RVA: 0x00148414 File Offset: 0x00146614
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object obj in anArray)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(obj);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x060037D0 RID: 14288 RVA: 0x00148494 File Offset: 0x00146694
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				int i = 0;
				while (i < array.Length)
				{
					char c = array[i];
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					case '\v':
						goto IL_00E0;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								goto IL_00E0;
							}
							this.builder.Append("\\\\");
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					}
					IL_0129:
					i++;
					continue;
					IL_00E0:
					int num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						this.builder.Append(c);
						goto IL_0129;
					}
					this.builder.Append("\\u");
					this.builder.Append(num.ToString("x4"));
					goto IL_0129;
				}
				this.builder.Append('"');
			}

			// Token: 0x060037D1 RID: 14289 RVA: 0x001485E8 File Offset: 0x001467E8
			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R"));
					return;
				}
				if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
					return;
				}
				if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R"));
					return;
				}
				this.SerializeString(value.ToString());
			}

			// Token: 0x040029E0 RID: 10720
			private StringBuilder builder;
		}
	}
}
