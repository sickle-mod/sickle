using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200011F RID: 287
public static class Censor
{
	// Token: 0x06000906 RID: 2310 RVA: 0x0002E25E File Offset: 0x0002C45E
	public static IEnumerator Init()
	{
		ResourceRequest resourceRequest = Resources.LoadAsync("CensorBlacklist");
		while (!resourceRequest.isDone)
		{
			yield return null;
		}
		Censor.blackList = new List<string>((resourceRequest.asset as TextAsset).text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
		Censor.blackList.Sort((string a, string b) => b.Length.CompareTo(a.Length));
		for (int i = 0; i < Censor.blackList.Count; i++)
		{
			string text = string.Empty;
			for (int j = 0; j < Censor.blackList[i].Length; j++)
			{
				text = text + Censor.blackList[i][j].ToString() + "\\s*";
			}
			Censor.blackList[i] = "\\b" + text.Replace("+", "\\+") + "\\b";
		}
		yield break;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0007AA80 File Offset: 0x00078C80
	public static string Process(string message)
	{
		foreach (string text in Censor.blackList)
		{
			message = Regex.Replace(message, text, "***", RegexOptions.IgnoreCase);
		}
		message = message.Replace("卐", "*");
		message = message.Replace("卍", "*");
		message = message.Replace("☭", "*");
		return message;
	}

	// Token: 0x0400083A RID: 2106
	private static List<string> blackList = new List<string>();
}
