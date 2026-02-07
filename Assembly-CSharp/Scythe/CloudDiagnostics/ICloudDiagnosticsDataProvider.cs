using System;

namespace Scythe.CloudDiagnostics
{
	// Token: 0x02000389 RID: 905
	public interface ICloudDiagnosticsDataProvider
	{
		// Token: 0x06001A28 RID: 6696
		void UpdateDiagnosticData();

		// Token: 0x06001A29 RID: 6697
		void OnRegister();

		// Token: 0x06001A2A RID: 6698
		void OnUnregister();
	}
}
