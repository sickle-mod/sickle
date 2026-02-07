using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x02000559 RID: 1369
	public class ChallengeLogic2
	{
		// Token: 0x06002BF0 RID: 11248 RVA: 0x000F3C2C File Offset: 0x000F1E2C
		public void InitChallenge2(GameManager gameManager)
		{
			gameManager.InitMap();
			gameManager.promoCardsOn = true;
			gameManager.SetCardDecks(3);
			gameManager.encounterCards.ClearDeck();
			gameManager.availableFactions.Clear();
			gameManager.objectiveCards.ClearDeck();
			gameManager.availableFactions.Add(new MatFaction(gameManager, Faction.Polania, 6, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			gameManager.availableFactions.Add(new MatFaction(gameManager, Faction.Crimea, 5, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			gameManager.availableFactions.Add(new MatFaction(gameManager, Faction.Saxony, 3, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			gameManager.availableActionMats.Clear();
			gameManager.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Industrial));
			gameManager.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Engineering));
			gameManager.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Mechanical));
			Player player = new Player(gameManager, gameManager.availableFactions[0], gameManager.availableActionMats[0], 0);
			player.Name = "Player";
			gameManager.players.Add(player);
			Player player2 = new Player(gameManager, gameManager.availableFactions[1], gameManager.availableActionMats[1], 2);
			player2.aiPlayer = new AiPlayerTutorial03(player2, gameManager);
			player2.Name = "AICrimea";
			gameManager.players.Add(player2);
			Player player3 = new Player(gameManager, gameManager.availableFactions[2], gameManager.availableActionMats[2], 2);
			player3.aiPlayer = new AiPlayerTutorial03(player3, gameManager);
			player3.Name = "AISaxony";
			gameManager.players.Add(player3);
			player.character.position = gameManager.gameBoard.hexMap[3, 3];
			player.matPlayer.workers[0].position = gameManager.gameBoard.hexMap[0, 4];
			player.matPlayer.workers[1].position = gameManager.gameBoard.hexMap[0, 4];
			Dictionary<ResourceType, int> resources = gameManager.gameBoard.hexMap[3, 3].resources;
			resources[ResourceType.food] = resources[ResourceType.food] + 2;
			player3.matPlayer.workers[0].position = gameManager.gameBoard.hexMap[1, 7];
			player3.matPlayer.workers[1].position = gameManager.gameBoard.hexMap[1, 7];
			Mech mech = new Mech(gameManager, player, 1);
			player.matFaction.mechs.Add(mech);
			mech.position = gameManager.gameBoard.hexMap[0, 4];
			Mech mech2 = new Mech(gameManager, player3, 1);
			player3.matFaction.mechs.Add(mech2);
			mech2.position = gameManager.gameBoard.hexMap[1, 4];
			Mech mech3 = new Mech(gameManager, player2, 1);
			player2.matFaction.mechs.Add(mech3);
			mech3.position = gameManager.gameBoard.hexMap[3, 6];
			Mech mech4 = new Mech(gameManager, player, 1);
			player.matFaction.mechs.Add(mech4);
			mech4.position = gameManager.gameBoard.hexMap[2, 6];
			player2.character.position = gameManager.gameBoard.hexMap[3, 6];
			player.combatCards.Add(new CombatCard(2));
			player2.combatCards.Add(new CombatCard(2));
			player2.combatCards.Add(new CombatCard(2));
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameManager.gameBoard.hexMap[i, j].hasEncounter = false;
				}
			}
			gameManager.gameBoard.UpdateHexOwnerships();
			gameManager.SortPlayers();
		}
	}
}
