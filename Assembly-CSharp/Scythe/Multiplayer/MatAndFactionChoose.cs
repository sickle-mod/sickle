using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer
{
	// Token: 0x0200024B RID: 587
	public class MatAndFactionChoose
	{
		// Token: 0x060011A5 RID: 4517 RVA: 0x0003363E File Offset: 0x0003183E
		private bool DLCFaction(Faction faction)
		{
			return faction == Faction.Albion || faction == Faction.Togawa;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0003364A File Offset: 0x0003184A
		private bool DLCPlayerMat(PlayerMatType matType)
		{
			return matType == PlayerMatType.Innovative || matType == PlayerMatType.Militant;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00094B2C File Offset: 0x00092D2C
		public MatAndFactionChoose(bool dlc)
		{
			this.random = new Random();
			this.currentSlotChoosing = -1;
			this.freeFactions = MatAndFactionSelection.SetAvailableFactions(dlc);
			this.freeFactions.Add(7);
			this.freePlayerMats = MatAndFactionSelection.SetAvailablePlayerMats(dlc);
			this.freePlayerMats.Add(7);
			this.currentFaction = this.freeFactions.Count - 1;
			this.currentPlayerMat = this.freePlayerMats.Count - 1;
			this.dlc = dlc;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00033656 File Offset: 0x00031856
		public void MoveToNextPlayer()
		{
			this.currentSlotChoosing++;
			this.currentFaction = this.freeFactions.Count - 1;
			this.currentPlayerMat = this.freePlayerMats.Count - 1;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00094BB8 File Offset: 0x00092DB8
		public void MoveToNextFaction()
		{
			this.currentFaction = this.Modulo(this.currentFaction + 1, this.freeFactions.Count);
			while (!this.CombinationAvailable())
			{
				this.currentFaction = this.Modulo(this.currentFaction + 1, this.freeFactions.Count);
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00094C10 File Offset: 0x00092E10
		public void MoveToPreviousFaction()
		{
			this.currentFaction = this.Modulo(this.currentFaction - 1, this.freeFactions.Count);
			while (!this.CombinationAvailable())
			{
				this.currentFaction = this.Modulo(this.currentFaction - 1, this.freeFactions.Count);
			}
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00094C68 File Offset: 0x00092E68
		public void MoveToNextMat()
		{
			this.currentPlayerMat = this.Modulo(this.currentPlayerMat + 1, this.freePlayerMats.Count);
			while (!this.CombinationAvailable())
			{
				this.currentPlayerMat = this.Modulo(this.currentPlayerMat + 1, this.freePlayerMats.Count);
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00094CC0 File Offset: 0x00092EC0
		public void MoveToPreviousMat()
		{
			this.currentPlayerMat = this.Modulo(this.currentPlayerMat - 1, this.freePlayerMats.Count);
			while (!this.CombinationAvailable())
			{
				this.currentPlayerMat = this.Modulo(this.currentPlayerMat - 1, this.freePlayerMats.Count);
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0003368C File Offset: 0x0003188C
		public int GetCurrentSlot()
		{
			return this.currentSlotChoosing;
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00033694 File Offset: 0x00031894
		public int GetCurrentFaction()
		{
			return this.freeFactions[this.currentFaction];
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000336A7 File Offset: 0x000318A7
		public int GetCurrentPlayerMat()
		{
			return this.freePlayerMats[this.currentPlayerMat];
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000336BA File Offset: 0x000318BA
		public void RemoveData(int faction, int mat)
		{
			this.freeFactions.Remove(faction);
			this.freePlayerMats.Remove(mat);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00094D18 File Offset: 0x00092F18
		public void SetRandomFaction()
		{
			this.currentFaction = this.random.Next(0, this.freeFactions.Count - 1);
			while (!this.CombinationAvailable())
			{
				this.currentFaction = this.random.Next(0, this.freeFactions.Count - 1);
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00094D70 File Offset: 0x00092F70
		public void SetRandomMat()
		{
			this.currentPlayerMat = this.random.Next(0, this.freePlayerMats.Count - 1);
			while (!this.CombinationAvailable())
			{
				this.currentPlayerMat = this.random.Next(0, this.freePlayerMats.Count - 1);
			}
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00094DC8 File Offset: 0x00092FC8
		public void AddLeaversMats(int faction, int playerMat)
		{
			if (this.currentFaction == this.freeFactions.Count - 1)
			{
				this.currentFaction++;
			}
			if (this.currentPlayerMat == this.freePlayerMats.Count - 1)
			{
				this.currentPlayerMat++;
			}
			this.freeFactions.Insert(this.freeFactions.Count - 1, faction);
			this.freePlayerMats.Insert(this.freePlayerMats.Count - 1, playerMat);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000336D6 File Offset: 0x000318D6
		private int Modulo(int x, int m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00094E50 File Offset: 0x00093050
		private bool CombinationAvailable()
		{
			return (this.freeFactions[this.currentFaction] == 7 || GameServiceController.Instance.InvadersFromAfarUnlocked() || !this.DLCFaction((Faction)this.freeFactions[this.currentFaction])) && (this.freePlayerMats[this.currentPlayerMat] == 7 || GameServiceController.Instance.InvadersFromAfarUnlocked() || !this.DLCPlayerMat((PlayerMatType)this.freePlayerMats[this.currentPlayerMat])) && !this.CurrentCombinationBlocked() && this.RemainingPlayersCanChoose();
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00094EE4 File Offset: 0x000930E4
		private bool CurrentCombinationBlocked()
		{
			return this.freeFactions[this.currentFaction] != 7 && this.freePlayerMats[this.currentPlayerMat] != 7 && MatAndFactionSelection.FactionIsOverpoweredWithPlayerMat((Faction)this.freeFactions[this.currentFaction], (PlayerMatType)this.freePlayerMats[this.currentPlayerMat]);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00094F44 File Offset: 0x00093144
		private bool RemainingPlayersCanChoose()
		{
			for (int i = 0; i < PlayerInfo.me.CurrentLobbyRoom.PlayersList.Count; i++)
			{
				if (!this.MatsChoosen(PlayerInfo.me.CurrentLobbyRoom.PlayersList[i]) && !this.CheckIfCanChoose(PlayerInfo.me.CurrentLobbyRoom.PlayersList[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x000336DF File Offset: 0x000318DF
		private bool MatsChoosen(PlayerInfo player)
		{
			return player.Slot <= this.currentSlotChoosing;
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00094FB0 File Offset: 0x000931B0
		private bool CheckIfCanChoose(PlayerInfo player)
		{
			List<int> list = new List<int>(this.freeFactions);
			list.RemoveAt(this.currentFaction);
			list.Remove(7);
			List<int> list2 = new List<int>(this.freePlayerMats);
			list2.RemoveAt(this.currentPlayerMat);
			list2.Remove(7);
			if (!player.DLC)
			{
				list.Remove(1);
				list.Remove(4);
				list2.Remove(6);
				list2.Remove(5);
			}
			return (list.Count > 1 && list2.Count > 1) || !MatAndFactionSelection.FactionIsOverpoweredWithPlayerMat((Faction)list[0], (PlayerMatType)list2[0]);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00095054 File Offset: 0x00093254
		public override string ToString()
		{
			string text = "freeFactions.Count = " + this.freeFactions.Count.ToString() + " {";
			for (int i = 0; i < this.freeFactions.Count; i++)
			{
				text = text + " " + this.freeFactions[i].ToString();
			}
			text += " }";
			text = text + "\nfreePlayerMats.Count = " + this.freePlayerMats.Count.ToString() + " {";
			for (int j = 0; j < this.freePlayerMats.Count; j++)
			{
				text = text + " " + this.freePlayerMats[j].ToString();
			}
			text += " }";
			text = text + "\ncurrentSlotChoosing = " + this.currentSlotChoosing.ToString();
			text = text + "\ncurrentFaction = " + this.currentFaction.ToString();
			text = text + "\ncurrentPlayerMat = " + this.currentPlayerMat.ToString();
			return base.ToString() + "\n" + text;
		}

		// Token: 0x04000DAB RID: 3499
		private List<int> freeFactions;

		// Token: 0x04000DAC RID: 3500
		private List<int> freePlayerMats;

		// Token: 0x04000DAD RID: 3501
		private Random random;

		// Token: 0x04000DAE RID: 3502
		private int currentSlotChoosing = -1;

		// Token: 0x04000DAF RID: 3503
		private int currentFaction;

		// Token: 0x04000DB0 RID: 3504
		private int currentPlayerMat;

		// Token: 0x04000DB1 RID: 3505
		private bool dlc;
	}
}
