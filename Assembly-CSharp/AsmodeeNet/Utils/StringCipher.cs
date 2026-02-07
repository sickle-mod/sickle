using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000847 RID: 2119
	public static class StringCipher
	{
		// Token: 0x06003BF4 RID: 15348 RVA: 0x0015411C File Offset: 0x0015231C
		public static string Encrypt(string plainText, string passPhrase)
		{
			string text;
			try
			{
				byte[] array = StringCipher.Generate256BitsOfRandomEntropy();
				byte[] array2 = StringCipher.Generate256BitsOfRandomEntropy();
				byte[] bytes = Encoding.UTF8.GetBytes(plainText);
				byte[] bytes2 = new Rfc2898DeriveBytes(passPhrase, array, 1000).GetBytes(32);
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.BlockSize = 256;
					rijndaelManaged.Mode = CipherMode.CBC;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(bytes2, array2))
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
							{
								cryptoStream.Write(bytes, 0, bytes.Length);
								cryptoStream.FlushFinalBlock();
								byte[] array3 = array.Concat(array2).ToArray<byte>().Concat(memoryStream.ToArray())
									.ToArray<byte>();
								memoryStream.Close();
								cryptoStream.Close();
								text = Convert.ToBase64String(array3);
							}
						}
					}
				}
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0007DCDC File Offset: 0x0007BEDC
		public static string Decrypt(string cipherText, string passPhrase)
		{
			string text;
			try
			{
				byte[] array = Convert.FromBase64String(cipherText);
				byte[] array2 = array.Take(32).ToArray<byte>();
				byte[] array3 = array.Skip(32).Take(32).ToArray<byte>();
				byte[] array4 = array.Skip(64).Take(array.Length - 64).ToArray<byte>();
				byte[] bytes = new Rfc2898DeriveBytes(passPhrase, array2, 1000).GetBytes(32);
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.BlockSize = 256;
					rijndaelManaged.Mode = CipherMode.CBC;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(bytes, array3))
					{
						using (MemoryStream memoryStream = new MemoryStream(array4))
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
							{
								byte[] array5 = new byte[array4.Length];
								int num = cryptoStream.Read(array5, 0, array5.Length);
								memoryStream.Close();
								cryptoStream.Close();
								text = Encoding.UTF8.GetString(array5, 0, num);
							}
						}
					}
				}
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0007DE74 File Offset: 0x0007C074
		private static byte[] Generate256BitsOfRandomEntropy()
		{
			byte[] array = new byte[32];
			new RNGCryptoServiceProvider().GetBytes(array);
			return array;
		}

		// Token: 0x04002D93 RID: 11667
		private const int Keysize = 256;

		// Token: 0x04002D94 RID: 11668
		private const int DerivationIterations = 1000;
	}
}
