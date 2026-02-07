using System;
using Scythe.BoardPresenter;
using Scythe.GameLogic;

// Token: 0x020000C6 RID: 198
public class BuildSpawnAnimation : EnemyActionAnimation
{
	// Token: 0x04000500 RID: 1280
	public Faction owner;

	// Token: 0x04000501 RID: 1281
	public GameHexPresenter hex;

	// Token: 0x04000502 RID: 1282
	public Building buildingToSpawn;
}
