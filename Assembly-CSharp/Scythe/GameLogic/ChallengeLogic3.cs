using System;

namespace Scythe.GameLogic
{
	// Token: 0x0200055A RID: 1370
	public class ChallengeLogic3
	{
		// Token: 0x06002BF2 RID: 11250 RVA: 0x000F4038 File Offset: 0x000F2238
		public void InitChallenge3(GameManager gameManager)
		{
			gameManager.InitMap();
			gameManager.promoCardsOn = true;
			gameManager.SetCardDecks(3);
			gameManager.encounterCards.ClearDeck();
			gameManager.availableFactions.Clear();
			gameManager.objectiveCards.ClearDeck();
			gameManager.availableFactions.Add(new MatFaction(gameManager, Faction.Polania, 15, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			gameManager.availableFactions.Add(new MatFaction(gameManager, Faction.Rusviet, 0, 0, AbilityPerk.Coercion, new AbilityPerk[]
			{
				(AbilityPerk)12,
				AbilityPerk.Wayfare,
				AbilityPerk.Scout,
				AbilityPerk.Speed
			}));
			gameManager.availableFactions.Add(new MatFaction(gameManager, Faction.Saxony, 0, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			gameManager.availableActionMats.Clear();
			gameManager.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Engineering));
			gameManager.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Industrial));
			gameManager.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Mechanical));
			Player player = new Player(gameManager, gameManager.availableFactions[0], gameManager.availableActionMats[0], 0);
			player.Name = "Player";
			player.GainStar(StarType.Combat);
			player.GainStar(StarType.Objective);
			player.GainStar(StarType.Workers);
			player.GainStar(StarType.Structures);
			player.GainStar(StarType.Upgrades);
			gameManager.players.Add(player);
			Player player2 = new Player(gameManager, gameManager.availableFactions[1], gameManager.availableActionMats[1], 2);
			player2.aiPlayer = new AiPlayerTutorial03(player2, gameManager);
			player2.Name = "AIRusviet";
			player2.GainStar(StarType.Combat);
			player2.GainStar(StarType.Combat);
			player2.GainStar(StarType.Combat);
			player2.GainStar(StarType.Combat);
			gameManager.players.Add(player2);
			Player player3 = new Player(gameManager, gameManager.availableFactions[2], gameManager.availableActionMats[2], 2);
			player3.aiPlayer = new AiPlayerTutorial03(player3, gameManager);
			player3.Name = "AISaxony";
			player3.GainStar(StarType.Combat);
			player3.GainStar(StarType.Objective);
			player3.GainStar(StarType.Workers);
			player3.GainStar(StarType.Structures);
			gameManager.players.Add(player3);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameManager.gameBoard.hexMap[i, j].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			gameManager.gameBoard.UpdateHexOwnerships();
			gameManager.SortPlayers();
		}
	}
}
