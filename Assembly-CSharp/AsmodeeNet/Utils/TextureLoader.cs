using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000858 RID: 2136
	public class TextureLoader
	{
		// Token: 0x06003C41 RID: 15425 RVA: 0x0004F047 File Offset: 0x0004D247
		public static IEnumerator LoadTexture(string url, MaskableGraphic image, Action<bool, byte[]> afterLoading = null)
		{
			byte[] downloadedBytes = null;
			Texture2D texture2D;
			if (url.StartsWith("http"))
			{
				UnityWebRequest uwr = new UnityWebRequest(url);
				uwr.SendWebRequest();
				while (!uwr.isDone)
				{
					yield return null;
				}
				if (!string.IsNullOrEmpty(uwr.error))
				{
					Hashtable hashtable = new Hashtable
					{
						{ "url", url },
						{ "error", uwr.error }
					};
					AsmoLogger.Error("TextureLoader", "Download failed", hashtable);
					if (afterLoading != null)
					{
						afterLoading(false, null);
					}
					yield break;
				}
				downloadedBytes = uwr.downloadHandler.data;
				if (!TextureLoader.StartWithJPEGHeader(downloadedBytes) && !TextureLoader.StartWithPNGHeader(downloadedBytes))
				{
					AsmoLogger.Error("TextureLoader", "Only JPEG and PNG supported", null);
					if (afterLoading != null)
					{
						afterLoading(false, null);
					}
					yield break;
				}
				DownloadHandlerTexture downloadHandlerTexture = uwr.downloadHandler as DownloadHandlerTexture;
				if (downloadHandlerTexture == null)
				{
					AsmoLogger.Error("TextureLoader", "Failed to retrive texture download handler.", null);
					if (afterLoading != null)
					{
						afterLoading(false, null);
					}
					yield break;
				}
				texture2D = downloadHandlerTexture.texture;
				uwr = null;
			}
			else
			{
				downloadedBytes = File.ReadAllBytes(url);
				texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				texture2D.LoadImage(downloadedBytes);
				texture2D.anisoLevel = 16;
			}
			if (image != null && image.gameObject != null && texture2D != null)
			{
				if (image is Image)
				{
					((Image)image).sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
				}
				else if (image is RawImage)
				{
					((RawImage)image).texture = texture2D;
				}
				else
				{
					AsmoLogger.Error("TextureLoader", "Parameter 'image' is not of type Image or RawImage", null);
				}
				yield return null;
				image.enabled = true;
				image.gameObject.SetActive(true);
				yield return null;
				if (afterLoading != null)
				{
					afterLoading(true, downloadedBytes);
				}
			}
			else
			{
				Hashtable hashtable2 = new Hashtable
				{
					{ "url", url },
					{ "image", image },
					{ "texture", texture2D }
				};
				AsmoLogger.Error("TextureLoader", "Something was wrong with the image or the texture", hashtable2);
				afterLoading(false, downloadedBytes);
			}
			yield break;
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x00154C48 File Offset: 0x00152E48
		private static string GetResourcePath(string path)
		{
			path = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
			int num = path.IndexOf("Resources");
			path = path.Substring(num + "Resources/".Length).Replace("\\", "/");
			return path;
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x0004F064 File Offset: 0x0004D264
		public static bool StartWithJPEGHeader(byte[] data)
		{
			return data != null && data.Length > 4 && (data[0] == byte.MaxValue && data[1] == 216 && data[data.Length - 2] == byte.MaxValue) && data[data.Length - 1] == 217;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x0004F0A3 File Offset: 0x0004D2A3
		public static bool StartWithPNGHeader(byte[] data)
		{
			return data[0] == 137 && data[1] == 80 && data[2] == 78 && data[3] == 71 && data[4] == 13 && data[5] == 10 && data[6] == 26 && data[7] == 10;
		}

		// Token: 0x04002DB0 RID: 11696
		private const string _debugModuleName = "TextureLoader";
	}
}
