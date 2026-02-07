using System;
using System.Collections.Generic;
using System.Xml;

namespace Scythe.GameLogic
{
	// Token: 0x020005F2 RID: 1522
	public class Mech : Unit
	{
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06003045 RID: 12357 RVA: 0x00046312 File Offset: 0x00044512
		// (set) Token: 0x06003046 RID: 12358 RVA: 0x0004631A File Offset: 0x0004451A
		public List<Worker> LoadedWorkers { get; private set; }

		// Token: 0x06003047 RID: 12359 RVA: 0x00126D78 File Offset: 0x00124F78
		public Mech(GameManager gameManager, Player owner, short maxMoveCount = 1)
			: base(gameManager, owner, maxMoveCount)
		{
			base.UnitType = UnitType.Mech;
			base.Id = owner.matFaction.mechs.Count;
			if (owner.matFaction.faction != Faction.Albion && owner.matFaction.faction != Faction.Togawa && owner.matFaction.SkillUnlocked[3])
			{
				base.UpgradeMaxMoveCount();
			}
			this.LoadedWorkers = new List<Worker>();
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x00046323 File Offset: 0x00044523
		public int GetLoadedWorkersCount()
		{
			return this.LoadedWorkers.Count;
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x00046330 File Offset: 0x00044530
		public bool LoadWorker(Worker worker)
		{
			if (worker.position == this.position)
			{
				this.LoadedWorkers.Add(worker);
				worker.OnMech = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x00046356 File Offset: 0x00044556
		public bool UnloadWorker(Worker worker)
		{
			if (this.LoadedWorkers.Contains(worker))
			{
				worker.OnMech = false;
				this.LoadedWorkers.Remove(worker);
				return true;
			}
			return false;
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x00126DE8 File Offset: 0x00124FE8
		public override void MoveTo(GameHex position)
		{
			base.MoveTo(position);
			foreach (Worker worker in this.LoadedWorkers)
			{
				worker.MoveTo(position);
			}
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x00126E40 File Offset: 0x00125040
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("WLoaded") != null)
			{
				foreach (Worker worker in base.Owner.matPlayer.workers)
				{
					if (worker.OnMech && !this.LoadedWorkers.Contains(worker))
					{
						this.LoadedWorkers.Add(worker);
					}
				}
			}
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x0004637D File Offset: 0x0004457D
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.LoadedWorkers.Count > 0)
			{
				writer.WriteAttributeString("WLoaded", "");
			}
		}
	}
}
