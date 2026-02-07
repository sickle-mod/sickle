using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class ExceptionLogger : MonoBehaviour
{
	// Token: 0x060002A0 RID: 672 RVA: 0x0002999B File Offset: 0x00027B9B
	private void OnEnable()
	{
		Application.logMessageReceived += this.HandleLog;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x000299AE File Offset: 0x00027BAE
	private void OnDisable()
	{
		this.Print();
		Application.logMessageReceived -= this.HandleLog;
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x000299C7 File Offset: 0x00027BC7
	private void Awake()
	{
		this.logMemory = new Dictionary<string, int>();
		if (ExceptionLogger.exceptionLogger == null)
		{
			ExceptionLogger.exceptionLogger = base.gameObject;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0005D350 File Offset: 0x0005B550
	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (type != LogType.Error && type != LogType.Exception)
		{
			return;
		}
		ErrorLogData errorLogData = new ErrorLogData("1.65a", type.ToString(), logString, stackTrace);
		if (!this.CheckLogOccurance(stackTrace))
		{
			this.logsRecieved++;
			RequestController.SaveErronLogOnTheServer(errorLogData);
		}
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0005D39C File Offset: 0x0005B59C
	private bool CheckLogOccurance(string stackTrace)
	{
		Match match = new Regex("\\n.*\\n").Match(stackTrace);
		if (!match.Success)
		{
			return true;
		}
		string text = match.Value.Substring(0, match.Value.Length - 1);
		if (this.logMemory.ContainsKey(text))
		{
			Dictionary<string, int> dictionary = this.logMemory;
			string text2 = text;
			dictionary[text2]++;
			return true;
		}
		this.AddErrorToMemory(text);
		return false;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0005D418 File Offset: 0x0005B618
	private void AddErrorToMemory(string errorSource)
	{
		if (this.logMemory.Count >= this.maxMemorySize)
		{
			string text = "";
			int num = int.MaxValue;
			foreach (KeyValuePair<string, int> keyValuePair in this.logMemory)
			{
				if (keyValuePair.Value < num)
				{
					num = keyValuePair.Value;
					text = keyValuePair.Key;
				}
			}
			this.logMemory.Remove(text);
		}
		this.logMemory.Add(errorSource, 1);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0005D4B8 File Offset: 0x0005B6B8
	private void Print()
	{
		if (this.logMemory.Count == 0)
		{
			return;
		}
		string text = string.Concat(new string[]
		{
			"Logs in memory: ",
			this.logMemory.Count.ToString(),
			"/",
			this.maxMemorySize.ToString(),
			"\n"
		});
		text = text + "Count of all logs recieved: " + this.logsRecieved.ToString() + ".\n";
		foreach (KeyValuePair<string, int> keyValuePair in this.logMemory)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair.Key,
				" => called (at least) ",
				keyValuePair.Value.ToString(),
				" time(s).\n"
			});
		}
		Debug.LogWarning(text);
	}

	// Token: 0x040001FA RID: 506
	public int logsRecieved;

	// Token: 0x040001FB RID: 507
	private Dictionary<string, int> logMemory;

	// Token: 0x040001FC RID: 508
	private int maxMemorySize = 10;

	// Token: 0x040001FD RID: 509
	private static GameObject exceptionLogger;
}
