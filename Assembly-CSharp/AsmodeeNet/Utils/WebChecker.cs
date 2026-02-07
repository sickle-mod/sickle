using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200085D RID: 2141
	public static class WebChecker
	{
		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06003C54 RID: 15444 RVA: 0x00041D70 File Offset: 0x0003FF70
		public static bool IsNetworkReachable
		{
			get
			{
				return Application.internetReachability > NetworkReachability.NotReachable;
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0004F11B File Offset: 0x0004D31B
		public static IEnumerator WebRequest(IEnumerator connectionSuccess, IEnumerator connectionError, string targetURL = "https://www.google.com")
		{
			if (!WebChecker.IsNetworkReachable)
			{
				yield return connectionError;
			}
			else
			{
				UnityWebRequest uwr = new UnityWebRequest(targetURL);
				yield return uwr.SendWebRequest();
				if (uwr.error != null)
				{
					yield return connectionError;
				}
				else
				{
					yield return connectionSuccess;
				}
				uwr = null;
			}
			yield break;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x0004F138 File Offset: 0x0004D338
		public static IEnumerator WebRequest(Action connectionSuccess, Action connectionError, string targetURL = "https://www.google.com")
		{
			if (!WebChecker.IsNetworkReachable)
			{
				if (connectionError != null)
				{
					connectionError();
				}
			}
			else
			{
				UnityWebRequest uwr = new UnityWebRequest(targetURL);
				yield return uwr.SendWebRequest();
				if (uwr.error != null)
				{
					if (connectionError != null)
					{
						connectionError();
					}
				}
				else if (connectionSuccess != null)
				{
					connectionSuccess();
				}
				uwr = null;
			}
			yield break;
		}

		// Token: 0x04002DBD RID: 11709
		private const string _defaultTargetURL = "https://www.google.com";
	}
}
