using System;

// Token: 0x0200016B RID: 363
public static class TMPHelper
{
	// Token: 0x06000A74 RID: 2676 RVA: 0x0002F25C File Offset: 0x0002D45C
	public static string GetTMPSprite(string spriteName)
	{
		return string.Format("<sprite name=\"{0}\">", spriteName);
	}
}
