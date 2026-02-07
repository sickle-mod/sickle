using System;
using System.Collections.Generic;
using System.Text;
using I2.Loc;
using Scythe.Multiplayer.AuthApi.Models;
using UnityEngine;

namespace Multiplayer.AuthApi
{
	// Token: 0x020001A4 RID: 420
	[Serializable]
	public class FailureResponse
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x000303E3 File Offset: 0x0002E5E3
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x000303EB File Offset: 0x0002E5EB
		[SerializeField]
		public Result Result { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x000303F4 File Offset: 0x0002E5F4
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x000303FC File Offset: 0x0002E5FC
		[SerializeField]
		public Error Error { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00030405 File Offset: 0x0002E605
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x0003040D File Offset: 0x0002E60D
		[SerializeField]
		public List<string> ValidationErrors { get; set; }

		// Token: 0x06000C5A RID: 3162 RVA: 0x00080CFC File Offset: 0x0007EEFC
		public string GetErrorsString()
		{
			StringBuilder sb = new StringBuilder();
			string text = ErrorHandler.ErrorMessage(this.Error);
			if (!string.IsNullOrEmpty(text))
			{
				sb.Append(ScriptLocalization.Get(text) + "\n");
			}
			List<string> validationErrors = this.ValidationErrors;
			if (validationErrors != null)
			{
				validationErrors.ForEach(delegate(string e)
				{
					sb.Append(ScriptLocalization.Get("ErrorMessages/" + e) + "\n");
				});
			}
			return sb.ToString();
		}
	}
}
