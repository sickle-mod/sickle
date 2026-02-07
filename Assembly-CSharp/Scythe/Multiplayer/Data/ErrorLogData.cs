using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200030E RID: 782
	public class ErrorLogData
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x0003782C File Offset: 0x00035A2C
		// (set) Token: 0x060016A7 RID: 5799 RVA: 0x00037834 File Offset: 0x00035A34
		public string GameVersion { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060016A8 RID: 5800 RVA: 0x0003783D File Offset: 0x00035A3D
		// (set) Token: 0x060016A9 RID: 5801 RVA: 0x00037845 File Offset: 0x00035A45
		public string LogType { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060016AA RID: 5802 RVA: 0x0003784E File Offset: 0x00035A4E
		// (set) Token: 0x060016AB RID: 5803 RVA: 0x00037856 File Offset: 0x00035A56
		public string LogString { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x0003785F File Offset: 0x00035A5F
		// (set) Token: 0x060016AD RID: 5805 RVA: 0x00037867 File Offset: 0x00035A67
		public string StackTrace { get; set; }

		// Token: 0x060016AE RID: 5806 RVA: 0x00027E56 File Offset: 0x00026056
		public ErrorLogData()
		{
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x00037870 File Offset: 0x00035A70
		public ErrorLogData(string GameVersion, string LogType, string LogString, string StackTrace)
		{
			this.GameVersion = GameVersion;
			this.LogType = LogType;
			this.LogString = LogString;
			this.StackTrace = StackTrace;
		}
	}
}
