using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200018A RID: 394
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.None;
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002FC50 File Offset: 0x0002DE50
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0002FC66 File Offset: 0x0002DE66
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002FC7C File Offset: 0x0002DE7C
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x170000E3 RID: 227
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray { value });
			}
		}

		// Token: 0x170000E4 RID: 228
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONObject { { aKey, value } });
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0007F404 File Offset: 0x0007D604
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray { aItem });
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0007F3E0 File Offset: 0x0007D5E0
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONObject { { aKey, aItem } });
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002FCB6 File Offset: 0x0002DEB6
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002FCC1 File Offset: 0x0002DEC1
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002FCB6 File Offset: 0x0002DEB6
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0007F428 File Offset: 0x0007D628
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x0007F44C File Offset: 0x0007D64C
		public override int AsInt
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber((double)value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0007F468 File Offset: 0x0007D668
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x0007F44C File Offset: 0x0007D64C
		public override float AsFloat
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0f;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber((double)value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0007F490 File Offset: 0x0007D690
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x0007F4BC File Offset: 0x0007D6BC
		public override double AsDouble
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0.0;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber(value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0007F4D8 File Offset: 0x0007D6D8
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0007F4F4 File Offset: 0x0007D6F4
		public override bool AsBool
		{
			get
			{
				JSONBool jsonbool = new JSONBool(false);
				this.Set(jsonbool);
				return false;
			}
			set
			{
				JSONBool jsonbool = new JSONBool(value);
				this.Set(jsonbool);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0007F510 File Offset: 0x0007D710
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0007F52C File Offset: 0x0007D72C
		public override JSONObject AsObject
		{
			get
			{
				JSONObject jsonobject = new JSONObject();
				this.Set(jsonobject);
				return jsonobject;
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002FC3A File Offset: 0x0002DE3A
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000974 RID: 2420
		private JSONNode m_Node;

		// Token: 0x04000975 RID: 2421
		private string m_Key;
	}
}
