using System;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004A8 RID: 1192
	public class TopMenuStarsEntry : MonoBehaviour
	{
		// Token: 0x060025D7 RID: 9687 RVA: 0x00040231 File Offset: 0x0003E431
		private void OnEnable()
		{
			if (this.player != null)
			{
				this.SetStars();
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00040241 File Offset: 0x0003E441
		public void SetEntryPlayerOwner(Player playerOwner)
		{
			this.player = playerOwner;
			this.FactionLogo.sprite = GameController.factionInfo[this.player.matFaction.faction].logo;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000E0DF4 File Offset: 0x000DEFF4
		public void SetStars()
		{
			Player playerByFaction = GameController.GameManager.GetPlayerByFaction(this.player.matFaction.faction);
			if (playerByFaction.GetNumberOfStars(StarType.Combat) > 0)
			{
				this.BattlesStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].GetProperStarsImage(playerByFaction.GetNumberOfStars(StarType.Combat));
				this.BattlesStar.color = Color.white;
			}
			else
			{
				this.BattlesStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.BattlesStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Mechs) > 0)
			{
				this.MechsStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].starMark;
				this.MechsStar.color = Color.white;
			}
			else
			{
				this.MechsStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.MechsStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Objective) > 0)
			{
				this.ObjectivesStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].GetProperStarsImage(playerByFaction.GetNumberOfStars(StarType.Objective));
				this.ObjectivesStar.color = Color.white;
			}
			else
			{
				this.ObjectivesStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.ObjectivesStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Popularity) > 0)
			{
				this.PopularityStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].starMark;
				this.PopularityStar.color = Color.white;
			}
			else
			{
				this.PopularityStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.PopularityStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Power) > 0)
			{
				this.PowerStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].starMark;
				this.PowerStar.color = Color.white;
			}
			else
			{
				this.PowerStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.PowerStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Recruits) > 0)
			{
				this.RecruitsStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].starMark;
				this.RecruitsStar.color = Color.white;
			}
			else
			{
				this.RecruitsStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.RecruitsStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Structures) > 0)
			{
				this.BuildingsStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].starMark;
				this.BuildingsStar.color = Color.white;
			}
			else
			{
				this.BuildingsStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.BuildingsStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Upgrades) > 0)
			{
				this.UpgradeStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].starMark;
				this.UpgradeStar.color = Color.white;
			}
			else
			{
				this.UpgradeStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
				this.UpgradeStar.color = this.emptyColor;
			}
			if (playerByFaction.GetNumberOfStars(StarType.Workers) > 0)
			{
				this.WorkersStar.sprite = ColorFactionMarks.colorFactionMarks[this.player.matFaction.faction].starMark;
				this.WorkersStar.color = Color.white;
				return;
			}
			this.WorkersStar.sprite = SingletonMono<TopMenuPanelsManager>.Instance.GetEmptyStar();
			this.WorkersStar.color = this.emptyColor;
		}

		// Token: 0x04001AB0 RID: 6832
		public Image FactionLogo;

		// Token: 0x04001AB1 RID: 6833
		public Image UpgradeStar;

		// Token: 0x04001AB2 RID: 6834
		public Image MechsStar;

		// Token: 0x04001AB3 RID: 6835
		public Image BuildingsStar;

		// Token: 0x04001AB4 RID: 6836
		public Image RecruitsStar;

		// Token: 0x04001AB5 RID: 6837
		public Image WorkersStar;

		// Token: 0x04001AB6 RID: 6838
		public Image ObjectivesStar;

		// Token: 0x04001AB7 RID: 6839
		public Image BattlesStar;

		// Token: 0x04001AB8 RID: 6840
		public Image PopularityStar;

		// Token: 0x04001AB9 RID: 6841
		public Image PowerStar;

		// Token: 0x04001ABA RID: 6842
		private Player player;

		// Token: 0x04001ABB RID: 6843
		private Color emptyColor = new Color(1f, 1f, 1f, 0f);
	}
}
