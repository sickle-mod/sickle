using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200034D RID: 845
	public class Message : Data
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x00038672 File Offset: 0x00036872
		// (set) Token: 0x06001839 RID: 6201 RVA: 0x0003867A File Offset: 0x0003687A
		public string Text { get; set; }

		// Token: 0x0600183A RID: 6202 RVA: 0x00037A7B File Offset: 0x00035C7B
		public Message()
		{
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00038683 File Offset: 0x00036883
		public Message(string text)
		{
			base.RoomId = PlayerInfo.me.RoomId;
			this.Text = text;
		}
	}
}
