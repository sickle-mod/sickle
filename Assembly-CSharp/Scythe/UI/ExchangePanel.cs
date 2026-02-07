using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.UI
{
	// Token: 0x020003E6 RID: 998
	public class ExchangePanel
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x0003B380 File Offset: 0x00039580
		// (set) Token: 0x06001DA7 RID: 7591 RVA: 0x0003B388 File Offset: 0x00039588
		public Unit containerUnit { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0003B391 File Offset: 0x00039591
		// (set) Token: 0x06001DA9 RID: 7593 RVA: 0x0003B399 File Offset: 0x00039599
		public GameHex field { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x0003B3A2 File Offset: 0x000395A2
		// (set) Token: 0x06001DAB RID: 7595 RVA: 0x0003B3D5 File Offset: 0x000395D5
		public List<Unit> LoadedWorkers
		{
			get
			{
				if (this.containerUnit != null && GameController.GetUnitPresenter(this.containerUnit) != null)
				{
					return GameController.GetUnitPresenter(this.containerUnit).GetWorkersList();
				}
				return new List<Unit>();
			}
			set
			{
				if (this.containerUnit != null && this.containerUnit is Mech)
				{
					GameController.GetUnitPresenter(this.containerUnit).AddWorkers(value);
				}
			}
		}

		// Token: 0x0400154D RID: 5453
		public bool battleMode;

		// Token: 0x0400154E RID: 5454
		public Dictionary<ResourceType, int> previousStateOfSliders = new Dictionary<ResourceType, int>
		{
			{
				ResourceType.oil,
				0
			},
			{
				ResourceType.metal,
				0
			},
			{
				ResourceType.food,
				0
			},
			{
				ResourceType.wood,
				0
			}
		};

		// Token: 0x0400154F RID: 5455
		public List<Unit> HexWorkers = new List<Unit>();
	}
}
