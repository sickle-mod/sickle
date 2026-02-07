using System;

// Token: 0x0200013D RID: 317
public interface IGooglePlayObbDownloader
{
	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600095E RID: 2398
	// (set) Token: 0x0600095F RID: 2399
	string PublicKey { get; set; }

	// Token: 0x06000960 RID: 2400
	string GetExpansionFilePath();

	// Token: 0x06000961 RID: 2401
	string GetMainOBBPath();

	// Token: 0x06000962 RID: 2402
	string GetPatchOBBPath();

	// Token: 0x06000963 RID: 2403
	void FetchOBB();
}
