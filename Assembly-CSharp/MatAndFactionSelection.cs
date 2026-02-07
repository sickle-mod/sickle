using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.GameLogic;

// Token: 0x02000025 RID: 37
public class MatAndFactionSelection
{
	// Token: 0x060000BB RID: 187 RVA: 0x00028666 File Offset: 0x00026866
	public static bool FactionIsOverpoweredWithPlayerMat(Faction faction, PlayerMatType playerMat)
	{
		return (faction == Faction.Rusviet && playerMat == PlayerMatType.Industrial) || (faction == Faction.Crimea && playerMat == PlayerMatType.Patriotic);
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0002867B File Offset: 0x0002687B
	public static bool IsDLCFaction(Faction faction)
	{
		return faction == Faction.Albion || faction == Faction.Togawa;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00028687 File Offset: 0x00026887
	public static bool IsDLCPlayerMatType(PlayerMatType playerMatType)
	{
		return playerMatType == PlayerMatType.Innovative || playerMatType == PlayerMatType.Militant;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x000555C4 File Offset: 0x000537C4
	public static List<Player> CreatePlayerList(GameManager gameManager, List<MatAndFactionSelection.PlayerEntry> playerList, bool invadersFromAfar, bool balanced = true)
	{
		List<int> list = MatAndFactionSelection.SetAvailableFactions(invadersFromAfar);
		List<int> list2 = MatAndFactionSelection.SetAvailablePlayerMats(invadersFromAfar);
		List<int> list3 = Enumerable.Repeat<int>(7, invadersFromAfar ? 7 : 5).ToList<int>();
		List<int> list4 = Enumerable.Repeat<int>(7, invadersFromAfar ? 7 : 5).ToList<int>();
		MatAndFactionSelection.SetChosenMats(list3, list4, playerList);
		MatAndFactionSelection.RemoveChosenMatsFromPool(list, list3);
		MatAndFactionSelection.RemoveChosenMatsFromPool(list2, list4);
		if (invadersFromAfar)
		{
			MatAndFactionSelection.SetDLCFactions(playerList, list3, 7, gameManager.random);
			MatAndFactionSelection.SetDLCPlayerMats(playerList, list4, 7, gameManager.random);
			MatAndFactionSelection.RemoveDLCMats(list, list2);
		}
		if (balanced)
		{
			MatAndFactionSelection.BalancePlayers(list3, list4, gameManager.random);
			MatAndFactionSelection.RemoveUnbalancedMats(list, list2);
		}
		MatAndFactionSelection.SetMatsRandomly(list, list3, 7, gameManager.random);
		MatAndFactionSelection.SetMatsRandomly(list2, list4, 7, gameManager.random);
		return MatAndFactionSelection.CreatePlayers(gameManager, playerList, list3, list4);
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00055680 File Offset: 0x00053880
	private static void SetChosenMats(List<int> chosenFactions, List<int> chosenPlayerMats, List<MatAndFactionSelection.PlayerEntry> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			chosenFactions[i] = players[i].Faction;
			chosenPlayerMats[i] = players[i].PlayerMat;
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x000556C4 File Offset: 0x000538C4
	private static void RemoveChosenMatsFromPool(List<int> availableMats, List<int> choosenMats)
	{
		foreach (int num in choosenMats)
		{
			availableMats.Remove(num);
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00055714 File Offset: 0x00053914
	private static void SetDLCFactions(List<MatAndFactionSelection.PlayerEntry> playerList, List<int> factions, int randomFactionIdentifier, Random random)
	{
		List<int> list = (from i in Enumerable.Range(0, factions.Count)
			where factions[i] == randomFactionIdentifier
			select i).ToList<int>();
		List<int> list2 = new List<int>();
		for (int j = 0; j < randomFactionIdentifier; j++)
		{
			if (list.Contains(j) && (playerList.Count <= j || playerList[j].HasDLC))
			{
				list2.Add(j);
			}
		}
		if (factions.FindIndex((int f) => f == 1) == -1)
		{
			int num = list2[random.Next(0, list2.Count)];
			list2.Remove(num);
			factions[num] = 1;
		}
		if (factions.FindIndex((int f) => f == 4) == -1)
		{
			int num2 = list2[random.Next(0, list2.Count)];
			list2.Remove(num2);
			factions[num2] = 4;
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00055850 File Offset: 0x00053A50
	private static void SetDLCPlayerMats(List<MatAndFactionSelection.PlayerEntry> playerList, List<int> playerMats, int randomMatIdentifier, Random random)
	{
		List<int> list = (from i in Enumerable.Range(0, playerMats.Count)
			where playerMats[i] == randomMatIdentifier
			select i).ToList<int>();
		List<int> list2 = new List<int>();
		for (int j = 0; j < randomMatIdentifier; j++)
		{
			if (list.Contains(j) && (playerList.Count <= j || playerList[j].HasDLC))
			{
				list2.Add(j);
			}
		}
		if (playerMats.FindIndex((int f) => f == 6) == -1)
		{
			int num = list2[random.Next(0, list2.Count)];
			list2.Remove(num);
			playerMats[num] = 6;
		}
		if (playerMats.FindIndex((int f) => f == 5) == -1)
		{
			int num2 = list2[random.Next(0, list2.Count)];
			list2.Remove(num2);
			playerMats[num2] = 5;
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00028693 File Offset: 0x00026893
	private static void RemoveDLCMats(List<int> factions, List<int> playerMats)
	{
		factions.Remove(1);
		factions.Remove(4);
		playerMats.Remove(6);
		playerMats.Remove(5);
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x000286B5 File Offset: 0x000268B5
	private static void BalancePlayers(List<int> factions, List<int> playerMats, Random random)
	{
		MatAndFactionSelection.SplitFactionAndPlayerMat(factions, playerMats, Faction.Rusviet, PlayerMatType.Industrial, random);
		MatAndFactionSelection.SplitFactionAndPlayerMat(factions, playerMats, Faction.Crimea, PlayerMatType.Patriotic, random);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0005598C File Offset: 0x00053B8C
	private static void SplitFactionAndPlayerMat(List<int> factions, List<int> playerMats, Faction faction, PlayerMatType type, Random random)
	{
		int factionIndex = factions.FindIndex((int f) => f == (int)faction);
		int matIndex = playerMats.FindIndex((int mat) => mat == (int)type);
		if (factionIndex == -1 && matIndex == -1)
		{
			List<int> list = (from index in Enumerable.Range(0, factions.Count)
				where factions[index] == 7
				select index).ToList<int>();
			List<int> list2 = (from index in Enumerable.Range(0, playerMats.Count)
				where playerMats[index] == 7
				select index).ToList<int>();
			if (list.Count == 1 && list2.Count == 1)
			{
				factions[list[0]] = (int)faction;
				playerMats[list2[0]] = (int)type;
				return;
			}
			int num = list[random.Next(0, list.Count)];
			list2.Remove(num);
			int num2 = list2[random.Next(0, list2.Count)];
			factions[num] = (int)faction;
			playerMats[num2] = (int)type;
			return;
		}
		else
		{
			if (factionIndex == -1)
			{
				List<int> list3 = (from index in Enumerable.Range(0, factions.Count)
					where factions[index] == 7
					select index).ToList<int>();
				list3.RemoveAll((int slot) => slot == matIndex);
				int num3 = list3[random.Next(0, list3.Count)];
				factions[num3] = (int)faction;
				return;
			}
			if (matIndex == -1)
			{
				List<int> list4 = (from index in Enumerable.Range(0, playerMats.Count)
					where playerMats[index] == 7
					select index).ToList<int>();
				list4.RemoveAll((int slot) => slot == factionIndex);
				int num4 = list4[random.Next(0, list4.Count)];
				playerMats[num4] = (int)type;
			}
			return;
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000286CB File Offset: 0x000268CB
	private static void RemoveUnbalancedMats(List<int> factions, List<int> playerMats)
	{
		factions.Remove(3);
		factions.Remove(5);
		playerMats.Remove(0);
		playerMats.Remove(2);
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00055BDC File Offset: 0x00053DDC
	private static void SetMatsRandomly(List<int> availableMats, List<int> choosenMats, int randomMatIdentifier, Random random)
	{
		for (int i = 0; i < choosenMats.Count; i++)
		{
			if (choosenMats[i] == randomMatIdentifier)
			{
				int num = availableMats[random.Next(0, availableMats.Count)];
				choosenMats[i] = num;
				availableMats.Remove(num);
			}
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00055C28 File Offset: 0x00053E28
	private static List<Player> CreatePlayers(GameManager gameManager, List<MatAndFactionSelection.PlayerEntry> playerList, List<int> factions, List<int> playerMats)
	{
		List<Player> list = new List<Player>(playerList.Count);
		for (int i = 0; i < playerList.Count; i++)
		{
			Player player = new Player(gameManager, new MatFaction(gameManager, (Faction)factions[i]), new MatPlayer(gameManager, (PlayerMatType)playerMats[i]), playerList[i].Type);
			player.Name = playerList[i].Name;
			player.aiPlayer = new AiPlayer(player, gameManager);
			list.Add(player);
		}
		return list;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00055CA8 File Offset: 0x00053EA8
	public static List<int> SetAvailableFactions(bool includingDLC)
	{
		List<int> list = new List<int>();
		list.Add(5);
		list.Add(2);
		list.Add(0);
		list.Add(3);
		list.Add(6);
		if (includingDLC)
		{
			list.Add(1);
			list.Add(4);
		}
		return list;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00055CF0 File Offset: 0x00053EF0
	public static List<int> SetAvailablePlayerMats(bool includingDLC)
	{
		List<int> list = new List<int>();
		list.Add(4);
		list.Add(0);
		list.Add(3);
		list.Add(1);
		list.Add(2);
		if (includingDLC)
		{
			list.Add(5);
			list.Add(6);
		}
		return list;
	}

	// Token: 0x040000BF RID: 191
	public const int MAXIMUM_NUMBER_OF_PLAYERS = 5;

	// Token: 0x040000C0 RID: 192
	public const int MAXIMUM_NUMBER_OF_PLAYERS_WITH_DLC = 7;

	// Token: 0x040000C1 RID: 193
	public const int NUMBER_OF_FACTIONS = 7;

	// Token: 0x040000C2 RID: 194
	public const int NUMBER_OF_PLAYER_MATS = 7;

	// Token: 0x040000C3 RID: 195
	public const int NUMBER_OF_DLC_MATS = 2;

	// Token: 0x040000C4 RID: 196
	public const int NUMBER_OF_STANDARD_MATS = 5;

	// Token: 0x02000026 RID: 38
	public struct PlayerEntry
	{
		// Token: 0x060000CC RID: 204 RVA: 0x000286ED File Offset: 0x000268ED
		public PlayerEntry(string name, int type, bool dlc, int faction, int playerMat, Guid playerId = default(Guid))
		{
			this.Name = name;
			this.Type = type;
			this.HasDLC = dlc;
			this.Faction = faction;
			this.PlayerMat = playerMat;
			this.PlayerId = playerId;
		}

		// Token: 0x040000C5 RID: 197
		public string Name;

		// Token: 0x040000C6 RID: 198
		public int Type;

		// Token: 0x040000C7 RID: 199
		public bool HasDLC;

		// Token: 0x040000C8 RID: 200
		public int Faction;

		// Token: 0x040000C9 RID: 201
		public int PlayerMat;

		// Token: 0x040000CA RID: 202
		public Guid PlayerId;
	}
}
