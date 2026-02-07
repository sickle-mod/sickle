using System;
using System.Linq;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200085B RID: 2139
	public static class UniqueId
	{
		// Token: 0x06003C4D RID: 15437 RVA: 0x00154FD4 File Offset: 0x001531D4
		public static string GetUniqueId()
		{
			string text;
			if (!KeyValueStore.HasKey("key"))
			{
				text = Guid.NewGuid().ToString("N") + "OAuthGate()#|@^*%§!?:;.,$~안녕하세요";
				text = UniqueId._Shuffle(text, global::UnityEngine.Random.Range(0, int.MaxValue));
				KeyValueStore.SetString("key", text);
				KeyValueStore.Save();
			}
			else
			{
				text = KeyValueStore.GetString("key", "");
			}
			return UniqueId._Shuffle(text, (from x in CoreApplication.GetUserAgent()
				select (int)x).Sum());
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00155074 File Offset: 0x00153274
		private static string _Shuffle(string s, int seed)
		{
			global::UnityEngine.Random.InitState(seed);
			byte[] array = UniqueId._GetBytes(s);
			for (int i = array.Length - 1; i >= 1; i--)
			{
				int num = global::UnityEngine.Random.Range(0, i);
				byte b = array[num];
				array[num] = array[i];
				array[i] = b;
			}
			return UniqueId._GetString(array);
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0007DF90 File Offset: 0x0007C190
		private static byte[] _GetBytes(string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0007DFC0 File Offset: 0x0007C1C0
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

		// Token: 0x04002DBA RID: 11706
		private const string _kKey = "key";
	}
}
