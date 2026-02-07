using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public abstract class GameRoomSlotState : MonoBehaviour
{
	// Token: 0x0200002D RID: 45
	public enum GameRoomSlotStateType
	{
		// Token: 0x040000F0 RID: 240
		EMPTY,
		// Token: 0x040000F1 RID: 241
		PLAYER,
		// Token: 0x040000F2 RID: 242
		BOT,
		// Token: 0x040000F3 RID: 243
		INVITATION,
		// Token: 0x040000F4 RID: 244
		SELECTING
	}
}
