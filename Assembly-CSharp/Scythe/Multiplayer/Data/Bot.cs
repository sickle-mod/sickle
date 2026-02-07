using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000348 RID: 840
	public class Bot
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x000384E5 File Offset: 0x000366E5
		// (set) Token: 0x0600180D RID: 6157 RVA: 0x000384ED File Offset: 0x000366ED
		public int Difficulty { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x000384F6 File Offset: 0x000366F6
		// (set) Token: 0x0600180F RID: 6159 RVA: 0x000384FE File Offset: 0x000366FE
		public string Name { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x00038507 File Offset: 0x00036707
		// (set) Token: 0x06001811 RID: 6161 RVA: 0x0003850F File Offset: 0x0003670F
		public string RoomId { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x00038518 File Offset: 0x00036718
		// (set) Token: 0x06001813 RID: 6163 RVA: 0x00038520 File Offset: 0x00036720
		public int Slot { get; set; }

		// Token: 0x06001814 RID: 6164 RVA: 0x00027E56 File Offset: 0x00026056
		public Bot()
		{
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00038529 File Offset: 0x00036729
		public Bot(int slot)
		{
			this.RoomId = PlayerInfo.me.RoomId;
			this.Slot = slot;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x00038548 File Offset: 0x00036748
		public Bot(int slot, int difficulty)
			: this(slot)
		{
			this.Difficulty = difficulty;
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x00038558 File Offset: 0x00036758
		public Bot(int slot, int difficulty, string name)
			: this(slot, difficulty)
		{
			this.Name = name;
		}
	}
}
