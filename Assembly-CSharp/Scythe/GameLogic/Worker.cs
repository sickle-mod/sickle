using System;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005F5 RID: 1525
	public class Worker : Unit
	{
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x000464AA File Offset: 0x000446AA
		// (set) Token: 0x0600306C RID: 12396 RVA: 0x000464B2 File Offset: 0x000446B2
		public bool OnMech { get; set; }

		// Token: 0x0600306D RID: 12397 RVA: 0x000464BB File Offset: 0x000446BB
		public Worker(GameManager gameManager, Player owner, short maxMoveCount = 1, int id = -1)
			: base(gameManager, owner, maxMoveCount)
		{
			base.UnitType = UnitType.Worker;
			if (owner != null)
			{
				base.Id = owner.matPlayer.workers.Count;
			}
			else
			{
				base.Id = id;
			}
			this.OnMech = false;
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000464F7 File Offset: 0x000446F7
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("OnMech") != null)
			{
				this.OnMech = true;
			}
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x00046514 File Offset: 0x00044714
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.OnMech)
			{
				writer.WriteAttributeString("OnMech", "");
			}
		}
	}
}
