using System;
using System.Linq;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000178 RID: 376
	public static class UniqueId
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x0007DE98 File Offset: 0x0007C098
		public static string GetUniqueId()
		{
			string text;
			if (!KeyValueStore.HasKey("key"))
			{
				text = string.Format("{0:N}{1}", Guid.NewGuid(), "()#|@^*%§!?:;.,$~안녕하세요");
				text = UniqueId._Shuffle(text, global::UnityEngine.Random.Range(0, int.MaxValue));
				KeyValueStore.SetString("key", text);
				KeyValueStore.Save();
			}
			else
			{
				text = KeyValueStore.GetString("key", "");
			}
			return UniqueId._Shuffle(text, (from x in UniqueId.GetUserAgent()
				select (int)x).Sum());
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0007DF34 File Offset: 0x0007C134
		private static string _Shuffle(string s, int seed)
		{
			global::UnityEngine.Random.InitState(seed);
			byte[] array = UniqueId._GetBytes(s);
			for (int i = array.Length - 1; i >= 1; i--)
			{
				int num = global::UnityEngine.Random.Range(0, i);
				ref byte ptr = ref array[num];
				ref byte ptr2 = ref array[i];
				byte b = array[i];
				byte b2 = array[num];
				ptr = b;
				ptr2 = b2;
			}
			return UniqueId._GetString(array);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0007DF90 File Offset: 0x0007C190
		private static byte[] _GetBytes(string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0007DFC0 File Offset: 0x0007C1C0
		private static string _GetString(byte[] bytes)
		{
			char[] array = new char[bytes.Length / 2];
			Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '\0')
				{
					char[] array2 = array;
					int num = i;
					array2[num] += '\u0001';
				}
			}
			return new string(array);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002F5B4 File Offset: 0x0002D7B4
		private static string GetUserAgent()
		{
			return string.Format("{0}/{1} {2}/ Unity/{3}", new object[]
			{
				Application.productName,
				Application.version,
				Application.platform,
				Application.unityVersion
			});
		}

		// Token: 0x04000941 RID: 2369
		private const string _kKey = "key";

		// Token: 0x04000942 RID: 2370
		private const string kForbiddenChars = "()#|@^*%§!?:;.,$~안녕하세요";
	}
}
