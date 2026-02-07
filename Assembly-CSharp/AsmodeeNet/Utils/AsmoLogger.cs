using System;
using System.Collections;
using MiniJSON;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000826 RID: 2086
	public static class AsmoLogger
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x0004E45B File Offset: 0x0004C65B
		// (set) Token: 0x06003B23 RID: 15139 RVA: 0x0004E462 File Offset: 0x0004C662
		public static AsmoLogger.Severity LogLevel { get; set; } = (AsmoLogger.IsDebugBuild ? AsmoLogger.Severity.Debug : AsmoLogger.Severity.Info);

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x0004E46A File Offset: 0x0004C66A
		// (set) Token: 0x06003B25 RID: 15141 RVA: 0x0004E471 File Offset: 0x0004C671
		public static bool IsDebugBuild { get; private set; } = global::UnityEngine.Debug.isDebugBuild;

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06003B26 RID: 15142 RVA: 0x0004E479 File Offset: 0x0004C679
		// (set) Token: 0x06003B27 RID: 15143 RVA: 0x0004E480 File Offset: 0x0004C680
		public static bool EnableTestMode { get; set; } = false;

		// Token: 0x06003B28 RID: 15144 RVA: 0x00152810 File Offset: 0x00150A10
		static AsmoLogger()
		{
			AsmoLogger.Info("AsmoLogger", "AsmoLogger initialized", new Hashtable { 
			{
				"IsDebugBuild",
				AsmoLogger.IsDebugBuild
			} });
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0004E488 File Offset: 0x0004C688
		public static void Trace(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Trace, module, message, extraInfo);
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x0004E493 File Offset: 0x0004C693
		public static void Trace(string module, AsmoLogger.LazyString lazyMessage, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Trace, module, lazyMessage, extraInfo);
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0004E49E File Offset: 0x0004C69E
		public static void Debug(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Debug, module, message, extraInfo);
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0004E4A9 File Offset: 0x0004C6A9
		public static void Debug(string module, AsmoLogger.LazyString lazyMessage, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Debug, module, lazyMessage, extraInfo);
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0004E4B4 File Offset: 0x0004C6B4
		public static void Info(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Info, module, message, extraInfo);
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0004E4BF File Offset: 0x0004C6BF
		public static void Info(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Info, module, message, extraInfo);
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x0004E4CA File Offset: 0x0004C6CA
		public static void Notice(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Notice, module, message, extraInfo);
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x0004E4D5 File Offset: 0x0004C6D5
		public static void Notice(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Notice, module, message, extraInfo);
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x0004E4E0 File Offset: 0x0004C6E0
		public static void Warning(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Warning, module, message, extraInfo);
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x0004E4EB File Offset: 0x0004C6EB
		public static void Warning(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Warning, module, message, extraInfo);
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x0004E4F6 File Offset: 0x0004C6F6
		public static void Error(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Error, module, message, extraInfo);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x0004E501 File Offset: 0x0004C701
		public static void Error(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Error, module, message, extraInfo);
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x0004E50C File Offset: 0x0004C70C
		public static void Fatal(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Fatal, module, message, extraInfo);
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x0004E517 File Offset: 0x0004C717
		public static void Fatal(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Fatal, module, message, extraInfo);
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0004E522 File Offset: 0x0004C722
		public static bool CanLog(AsmoLogger.Severity severity)
		{
			return severity >= AsmoLogger.LogLevel;
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0004E52F File Offset: 0x0004C72F
		public static void Log(AsmoLogger.Severity severity, string module, AsmoLogger.LazyString lazyMessage, Hashtable extraInfo = null)
		{
			if (!AsmoLogger.CanLog(severity))
			{
				return;
			}
			AsmoLogger._Log(severity, module, lazyMessage(), extraInfo);
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x0004E548 File Offset: 0x0004C748
		public static void Log(AsmoLogger.Severity severity, string module, string message, Hashtable extraInfo = null)
		{
			if (!AsmoLogger.CanLog(severity))
			{
				return;
			}
			AsmoLogger._Log(severity, module, message, extraInfo);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x00152868 File Offset: 0x00150A68
		private static void _Log(AsmoLogger.Severity severity, string module, string message, Hashtable extraInfo = null)
		{
			string text = string.Format("[{0}][{1}][{2}] {3}", new object[]
			{
				DateTime.UtcNow.ToString("o"),
				severity,
				module,
				message
			});
			if (extraInfo != null && extraInfo.Count > 0)
			{
				string text2 = Json.Serialize(extraInfo);
				text = text + " " + text2;
			}
			if (severity - AsmoLogger.Severity.Notice <= 1)
			{
				global::UnityEngine.Debug.LogWarning(text);
				return;
			}
			if (severity - AsmoLogger.Severity.Error > 1)
			{
				global::UnityEngine.Debug.Log(text);
				return;
			}
			if (AsmoLogger.EnableTestMode)
			{
				global::UnityEngine.Debug.LogWarning(text);
				return;
			}
			global::UnityEngine.Debug.LogError(text);
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x001528FC File Offset: 0x00150AFC
		public static void LogException(Exception ex, string moduleName, AsmoLogger.Severity severity = AsmoLogger.Severity.Error)
		{
			string text = ((ex.InnerException != null) ? ex.InnerException.GetType().ToString() : null);
			string text2 = ((ex.InnerException != null) ? ex.InnerException.Message : null);
			AsmoLogger.Log(severity, moduleName, "Inner Exception", new Hashtable
			{
				{
					"type",
					ex.GetType().ToString()
				},
				{ "message", ex.Message },
				{ "inner_type", text },
				{ "inner_message", text2 },
				{ "stack", ex.StackTrace }
			});
		}

		// Token: 0x04002CD7 RID: 11479
		public const bool IS_ON = true;

		// Token: 0x02000827 RID: 2087
		public enum Severity
		{
			// Token: 0x04002CDC RID: 11484
			Trace,
			// Token: 0x04002CDD RID: 11485
			Debug,
			// Token: 0x04002CDE RID: 11486
			Info,
			// Token: 0x04002CDF RID: 11487
			Notice,
			// Token: 0x04002CE0 RID: 11488
			Warning,
			// Token: 0x04002CE1 RID: 11489
			Error,
			// Token: 0x04002CE2 RID: 11490
			Fatal
		}

		// Token: 0x02000828 RID: 2088
		// (Invoke) Token: 0x06003B3D RID: 15165
		public delegate string LazyString();
	}
}
