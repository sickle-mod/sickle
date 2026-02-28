using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using I2.Loc;
using Scythe.Analytics;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x02000520 RID: 1312
	[XmlRoot("GameManager")]
	[Serializable]
	public class GameManager : IXmlSerializable
	{
		// Token: 0x060029DE RID: 10718 RVA: 0x000EDC44 File Offset: 0x000EBE44
		public void InitTutorial02Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(1);
			this.encounterCards.ClearDeck();
			this.availableFactions.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Polania, 4, 3, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial02Stars));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[2, 5];
			player.matPlayer.workers.Clear();
			Worker[] array = new Worker[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Worker(gameManager, player, 1, -1);
				player.matPlayer.workers.Add(array[i]);
				array[i].position = gameManager.gameBoard.hexMap[1, 4];
			}
			Mech mech = new Mech(gameManager, player, 1);
			player.matFaction.mechs.Add(mech);
			mech.position = gameManager.gameBoard.hexMap[1, 4];
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					gameManager.gameBoard.hexMap[j, k].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
			Dictionary<ResourceType, int> dictionary = gameManager.gameBoard.hexMap[2, 5].resources;
			dictionary[ResourceType.food] = dictionary[ResourceType.food] + 2;
			dictionary = gameManager.gameBoard.hexMap[2, 3].resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + 2;
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x000EDE5C File Offset: 0x000EC05C
		public void InitTutorial03Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			this.availableFactions.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Polania, 1, 3, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Nordic, 10, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial03Player));
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial03Enemy));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			Player player2 = new Player(gameManager, this.availableFactions[1], this.availableActionMats[1], 2);
			player2.Name = ScriptLocalization.Get("Common/Bot");
			player2.aiPlayer = new AiPlayerTutorial03(player2, gameManager);
			this.players.Add(player2);
			player.character.position = gameManager.gameBoard.hexMap[3, 3];
			player.matPlayer.workers.Clear();
			Worker worker = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker);
			worker.position = gameManager.gameBoard.hexMap[1, 3];
			Worker worker2 = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker2);
			worker2.position = gameManager.gameBoard.hexMap[0, 4];
			Mech mech = new Mech(gameManager, player, 1);
			player.matFaction.mechs.Add(mech);
			mech.position = gameManager.gameBoard.hexMap[3, 3];
			player.combatCards = new List<CombatCard>
			{
				new CombatCard(2),
				new CombatCard(2),
				new CombatCard(2),
				new CombatCard(3)
			};
			player2.character.position = gameManager.gameBoard.hexMap[5, 1];
			player2.matPlayer.workers.Clear();
			Worker[] array = new Worker[2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Worker(gameManager, player2, 1, -1);
				player2.matPlayer.workers.Add(array[i]);
				array[i].position = gameManager.gameBoard.hexMap[4, 2];
			}
			Mech mech2 = new Mech(gameManager, player2, 1);
			player2.matFaction.mechs.Add(mech2);
			mech2.position = gameManager.gameBoard.hexMap[2, 3];
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					gameManager.gameBoard.hexMap[j, k].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x000EE1C0 File Offset: 0x000EC3C0
		public void InitTutorial04Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.encounterCards.AddCard(new EncounterCard(13, gameManager));
			this.objectiveCards.ClearDeck();
			this.availableFactions.Clear();
			this.factoryCards.Clear();
			this.factoryCards.Add(new FactoryCard(5, gameManager));
			this.factoryCards.Add(new FactoryCard(6, gameManager));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Nordic, 1, 1, AbilityPerk.Swim, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial04Player));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[4, 4];
			player.matPlayer.workers.Clear();
			Worker worker = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker);
			worker.position = gameManager.gameBoard.hexMap[4, 1];
			Worker worker2 = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker2);
			worker2.position = gameManager.gameBoard.hexMap[4, 1];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameManager.gameBoard.hexMap[i, j].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			gameManager.gameBoard.hexMap[2, 6].hasEncounter = true;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x000EE3C8 File Offset: 0x000EC5C8
		public void InitTutorial05Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			this.availableFactions.Clear();
			this.factoryCards.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Nordic, 5, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial05Player));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[4, 0];
			player.matPlayer.workers.Clear();
			Worker worker = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker);
			worker.position = gameManager.gameBoard.hexMap[5, 1];
			Worker[] array = new Worker[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = new Worker(gameManager, player, 1, -1);
				player.matPlayer.workers.Add(array[i]);
				array[i].position = gameManager.gameBoard.hexMap[4, 2];
			}
			Worker[] array2 = new Worker[3];
			for (int j = 0; j < 3; j++)
			{
				array2[j] = new Worker(gameManager, player, 1, -1);
				player.matPlayer.workers.Add(array2[j]);
				array2[j].position = gameManager.gameBoard.hexMap[4, 1];
			}
			for (int k = 0; k < 8; k++)
			{
				for (int l = 0; l < 8; l++)
				{
					gameManager.gameBoard.hexMap[k, l].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			gameManager.gameBoard.hexMap[2, 6].hasEncounter = true;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000EE614 File Offset: 0x000EC814
		public void InitTutorial06Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			this.factoryCards.Clear();
			this.availableFactions.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Saxony, 1, 4, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Nordic, 1, 4, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial06Player));
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Agricultural));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[0, 7];
			player.matPlayer.workers.Clear();
			Worker worker = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker);
			worker.position = gameManager.gameBoard.hexMap[1, 7];
			Worker worker2 = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker2);
			worker2.position = gameManager.gameBoard.hexMap[0, 6];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameManager.gameBoard.hexMap[i, j].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
			Dictionary<ResourceType, int> dictionary = gameManager.gameBoard.hexMap[1, 7].resources;
			dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + 3;
			dictionary = gameManager.gameBoard.hexMap[0, 6].resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + 2;
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000EE85C File Offset: 0x000ECA5C
		public void InitTutorial07Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			this.availableFactions.Clear();
			this.factoryCards.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Saxony, 3, 4, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial07Player));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[1, 6];
			player.matPlayer.workers.Clear();
			Worker[] array = new Worker[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = new Worker(gameManager, player, 1, -1);
				player.matPlayer.workers.Add(array[i]);
				array[i].position = gameManager.gameBoard.hexMap[0, 6];
			}
			Mech mech = new Mech(gameManager, player, 1);
			player.matFaction.mechs.Add(mech);
			mech.position = gameManager.gameBoard.hexMap[0, 6];
			player.matFaction.SkillUnlocked[2] = true;
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					gameManager.gameBoard.hexMap[j, k].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			gameManager.gameBoard.hexMap[1, 6].encounterUsed = true;
			Dictionary<ResourceType, int> dictionary = gameManager.gameBoard.hexMap[0, 6].resources;
			dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + 1;
			dictionary = gameManager.gameBoard.hexMap[0, 6].resources;
			dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + 2;
			dictionary = gameManager.gameBoard.hexMap[0, 6].resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + 1;
			dictionary = gameManager.gameBoard.hexMap[1, 6].resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + 2;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000EEB04 File Offset: 0x000ECD04
		public void InitTutorial08Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			this.factoryCards.Clear();
			this.availableFactions.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Saxony, 2, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Crimea, 2, 0, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial08Player));
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial08Player));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[1, 7];
			player.matPlayer.workers.Clear();
			Worker worker = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker);
			worker.position = gameManager.gameBoard.hexMap[0, 6];
			Worker worker2 = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker2);
			worker2.position = gameManager.gameBoard.hexMap[1, 6];
			Worker worker3 = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker3);
			worker3.position = gameManager.gameBoard.hexMap[1, 7];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameManager.gameBoard.hexMap[i, j].hasEncounter = false;
				}
			}
			GameHex gameHex = gameManager.gameBoard.hexMap[1, 7];
			gameHex.Owner = player;
			GameHex gameHex2 = gameManager.gameBoard.hexMap[0, 6];
			gameHex2.Owner = player;
			Building structure = player.matPlayer.GetPlayerMatSection(0).ActionTop.Structure;
			GainBuilding gainBuilding = new GainBuilding(gameManager);
			gainBuilding.SetPlayer(player);
			gainBuilding.SetStructureAndLocation(structure, gameHex);
			gainBuilding.Execute();
			Building structure2 = player.matPlayer.GetPlayerMatSection(2).ActionTop.Structure;
			GainBuilding gainBuilding2 = new GainBuilding(gameManager);
			gainBuilding2.SetPlayer(player);
			gainBuilding2.SetStructureAndLocation(structure2, gameHex2);
			gainBuilding2.Execute();
			worker3.position = gameManager.gameBoard.hexMap[0, 6];
			Dictionary<ResourceType, int> dictionary = gameManager.gameBoard.hexMap[0, 6].resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + 2;
			dictionary = gameManager.gameBoard.hexMap[3, 6].resources;
			dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + 4;
			dictionary = gameManager.gameBoard.hexMap[3, 7].resources;
			dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + 2;
			dictionary = gameManager.gameBoard.hexMap[3, 6].resources;
			dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + 1;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x000EEE84 File Offset: 0x000ED084
		public void InitTutorial09Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			this.availableFactions.Clear();
			this.factoryCards.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Polania, 2, 3, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Nordic, 4, 2, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Saxony, 1, 6, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial09Player));
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial09AINordic));
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial09AISaxony));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			Player player2 = new Player(gameManager, this.availableFactions[1], this.availableActionMats[1], 2);
			player2.aiPlayer = new AiPlayerTutorial03(player2, gameManager);
			player2.Name = ScriptLocalization.Get("Common/Bot");
			this.players.Add(player2);
			Player player3 = new Player(gameManager, this.availableFactions[2], this.availableActionMats[2], 2);
			player3.aiPlayer = new AiPlayerTutorial03(player3, gameManager);
			player3.Name = ScriptLocalization.Get("Common/Bot");
			this.players.Add(player3);
			player.character.position = gameManager.gameBoard.hexMap[1, 3];
			player.matPlayer.workers.Clear();
			Worker[] array = new Worker[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = new Worker(gameManager, player, 1, -1);
				player.matPlayer.workers.Add(array[i]);
				array[i].position = gameManager.gameBoard.hexMap[0, 4];
			}
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					gameManager.gameBoard.hexMap[j, k].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			Dictionary<ResourceType, int> resources = gameManager.gameBoard.hexMap[0, 4].resources;
			resources[ResourceType.food] = resources[ResourceType.food] + 3;
			GainRecruit gainRecruit = new GainRecruit(gameManager);
			gainRecruit.SetPlayer(player2);
			gainRecruit.SetSectionAndBonus(DownActionType.Enlist, this.availableFactions[1].GetOneTimeBonus(GainType.Coin));
			gainRecruit.Execute();
			GainRecruit gainRecruit2 = new GainRecruit(gameManager);
			gainRecruit2.SetPlayer(player3);
			gainRecruit2.SetSectionAndBonus(DownActionType.Enlist, this.availableFactions[2].GetOneTimeBonus(GainType.Coin));
			gainRecruit2.Execute();
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x000EF1B8 File Offset: 0x000ED3B8
		public void InitTutorial10Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			ObjectiveCard objectiveCard = new ObjectiveCard(gameManager, 4);
			ObjectiveCard objectiveCard2 = new ObjectiveCard(gameManager, 11);
			this.objectiveCards.AddCard(objectiveCard);
			this.objectiveCards.AddCard(objectiveCard2);
			this.availableFactions.Clear();
			this.factoryCards.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Nordic, 8, 1, AbilityPerk.Swim, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Seaworthy,
				AbilityPerk.Artillery,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial10Player));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[5, 5];
			player.matPlayer.workers.Clear();
			Worker worker = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker);
			worker.position = gameManager.gameBoard.hexMap[5, 1];
			Worker worker2 = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker2);
			worker2.position = gameManager.gameBoard.hexMap[4, 1];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameManager.gameBoard.hexMap[i, j].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			gameManager.gameBoard.hexMap[2, 6].hasEncounter = true;
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000EF3BC File Offset: 0x000ED5BC
		public void InitTutorial11Asm(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(2);
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			ObjectiveCard objectiveCard = new ObjectiveCard(gameManager, 9);
			ObjectiveCard objectiveCard2 = new ObjectiveCard(gameManager, 18);
			this.objectiveCards.AddCard(objectiveCard);
			this.objectiveCards.AddCard(objectiveCard2);
			gameManager.SetStructureBonusCard(new OccupationTunnelCard());
			this.availableFactions.Clear();
			this.factoryCards.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Nordic));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Polania));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial11Player));
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial11Enemy));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			Player player2 = new Player(gameManager, this.availableFactions[1], this.availableActionMats[1], 2);
			player2.aiPlayer = new AiPlayerTutorial03(player2, gameManager);
			player2.Name = ScriptLocalization.Get("Common/Bot");
			this.players.Add(player2);
			player2.character.position = gameManager.gameBoard.hexMap[1, 4];
			player.character.position = gameManager.gameBoard.hexMap[4, 0];
			player.matPlayer.workers.Clear();
			Worker worker = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker);
			worker.position = gameManager.gameBoard.hexMap[4, 1];
			Worker worker2 = new Worker(gameManager, player, 1, -1);
			player.matPlayer.workers.Add(worker2);
			worker2.position = gameManager.gameBoard.hexMap[4, 4];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					gameManager.gameBoard.hexMap[i, j].hasEncounter = false;
				}
			}
			gameManager.gameBoard.factory.hexType = HexType.factory;
			GameHex gameHex = gameManager.gameBoard.hexMap[4, 1];
			gameHex.Owner = player;
			GameHex gameHex2 = gameManager.gameBoard.hexMap[3, 2];
			gameHex2.Owner = player;
			GameHex gameHex3 = gameManager.gameBoard.hexMap[5, 3];
			gameHex3.Owner = player;
			Building structure = player.matPlayer.GetPlayerMatSection(2).ActionTop.Structure;
			GainBuilding gainBuilding = new GainBuilding(gameManager);
			gainBuilding.SetPlayer(player);
			gainBuilding.SetStructureAndLocation(structure, gameHex);
			gainBuilding.Execute();
			worker2.position = gameManager.gameBoard.hexMap[3, 2];
			Building structure2 = player.matPlayer.GetPlayerMatSection(3).ActionTop.Structure;
			GainBuilding gainBuilding2 = new GainBuilding(gameManager);
			gainBuilding2.SetPlayer(player);
			gainBuilding2.SetStructureAndLocation(structure2, gameHex2);
			gainBuilding2.Execute();
			worker2.position = gameManager.gameBoard.hexMap[5, 3];
			Building structure3 = player.matPlayer.GetPlayerMatSection(0).ActionTop.Structure;
			GainBuilding gainBuilding3 = new GainBuilding(gameManager);
			gainBuilding3.SetPlayer(player);
			gainBuilding3.SetStructureAndLocation(structure3, gameHex3);
			gainBuilding3.Execute();
			worker2.position = gameManager.gameBoard.hexMap[4, 4];
			Dictionary<ResourceType, int> resources = gameManager.gameBoard.hexMap[4, 1].resources;
			resources[ResourceType.wood] = resources[ResourceType.wood] + 5;
			player.Popularity = 0;
			player2.Popularity = 16;
			player.Coins = 7;
			player2.Coins = 10;
			player.Power = 2;
			player2.Power = 0;
			player.Power = 0;
			player2.Power = 1;
			player.BattleVictory();
			player.BattleVictory();
			player2.BattleVictory();
			player2.BattleVictory();
			GainRecruit gainRecruit = new GainRecruit(gameManager);
			gainRecruit.SetPlayer(player);
			gainRecruit.SetSectionAndBonus(DownActionType.Enlist, this.availableFactions[0].GetOneTimeBonus(GainType.Coin));
			gainRecruit.Execute();
			GainRecruit gainRecruit2 = new GainRecruit(gameManager);
			gainRecruit2.SetPlayer(player);
			gainRecruit2.SetSectionAndBonus(DownActionType.Deploy, this.availableFactions[0].GetOneTimeBonus(GainType.CombatCard));
			gainRecruit2.Execute();
			GainRecruit gainRecruit3 = new GainRecruit(gameManager);
			gainRecruit3.SetPlayer(player);
			gainRecruit3.SetSectionAndBonus(DownActionType.Build, this.availableFactions[0].GetOneTimeBonus(GainType.Power));
			gainRecruit3.Execute();
			GainRecruit gainRecruit4 = new GainRecruit(gameManager);
			gainRecruit4.SetPlayer(player);
			gainRecruit4.SetSectionAndBonus(DownActionType.Upgrade, this.availableFactions[0].GetOneTimeBonus(GainType.Popularity));
			gainRecruit4.Execute();
			GainRecruit gainRecruit5 = new GainRecruit(gameManager);
			gainRecruit5.SetPlayer(player2);
			gainRecruit5.SetSectionAndBonus(DownActionType.Enlist, this.availableFactions[1].GetOneTimeBonus(GainType.Coin));
			gainRecruit5.Execute();
			GainRecruit gainRecruit6 = new GainRecruit(gameManager);
			gainRecruit6.SetPlayer(player2);
			gainRecruit6.SetSectionAndBonus(DownActionType.Deploy, this.availableFactions[1].GetOneTimeBonus(GainType.CombatCard));
			gainRecruit6.Execute();
			GainRecruit gainRecruit7 = new GainRecruit(gameManager);
			gainRecruit7.SetPlayer(player2);
			gainRecruit7.SetSectionAndBonus(DownActionType.Build, this.availableFactions[1].GetOneTimeBonus(GainType.Power));
			gainRecruit7.Execute();
			GainRecruit gainRecruit8 = new GainRecruit(gameManager);
			gainRecruit8.SetPlayer(player2);
			gainRecruit8.SetSectionAndBonus(DownActionType.Upgrade, this.availableFactions[1].GetOneTimeBonus(GainType.Popularity));
			gainRecruit8.Execute();
			player.CompleteObjective(0);
			player.GainStar(StarType.Combat);
			player.GainStar(StarType.Combat);
			player.GainStar(StarType.Objective);
			player.GainStar(StarType.Power);
			player2.GainStar(StarType.Combat);
			player2.GainStar(StarType.Combat);
			player2.GainStar(StarType.Power);
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000EF980 File Offset: 0x000EDB80
		public void InitTutorialAsmB01(GameManager gameManager)
		{
			this.InitMap();
			this.promoCardsOn = true;
			this.SetCardDecks(1);
			this.encounterCards.ClearDeck();
			this.availableFactions.Clear();
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Polania, 14, 3, AbilityPerk.Dominate, new AbilityPerk[]
			{
				(AbilityPerk)3,
				AbilityPerk.Underpass,
				AbilityPerk.Disarm,
				AbilityPerk.Speed
			}));
			this.availableFactions.Add(new MatFaction(gameManager, Faction.Rusviet, 0, 0, AbilityPerk.Coercion, new AbilityPerk[]
			{
				(AbilityPerk)12,
				AbilityPerk.Wayfare,
				AbilityPerk.Scout,
				AbilityPerk.Speed
			}));
			this.availableActionMats.Clear();
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial01StarsB));
			this.availableActionMats.Add(new MatPlayer(gameManager, PlayerMatType.Tutorial01StarsB));
			Player player = new Player(gameManager, this.availableFactions[0], this.availableActionMats[0], 0);
			player.Name = ScriptLocalization.Get("Common/Player");
			this.players.Add(player);
			player.character.position = gameManager.gameBoard.hexMap[1, 3];
			player.matPlayer.workers.Clear();
			Worker[] array = new Worker[5];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Worker(gameManager, player, 1, -1);
				player.matPlayer.workers.Add(array[i]);
				array[i].position = gameManager.gameBoard.hexMap[1, 4];
			}
			Mech[] array2 = new Mech[3];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new Mech(gameManager, player, 1);
				player.matFaction.mechs.Add(array2[j]);
			}
			array2[0].position = gameManager.gameBoard.hexMap[2, 3];
			array2[1].position = gameManager.gameBoard.hexMap[0, 4];
			array2[2].position = gameManager.gameBoard.hexMap[1, 4];
			this.SortPlayers();
			Dictionary<ResourceType, int> dictionary = gameManager.gameBoard.hexMap[1, 4].resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + 3;
			dictionary = gameManager.gameBoard.hexMap[6, 4].resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + 2;
			dictionary = gameManager.gameBoard.hexMap[5, 4].resources;
			dictionary[ResourceType.food] = dictionary[ResourceType.food] + 2;
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000EFBFC File Offset: 0x000EDDFC
		public void InitCampaign(int missionId, int campaignId = 0)
		{
			this.IsCampaign = true;
			this.IsMultiplayer = false;
			this.missionId = missionId;
			this.TurnCount = 0;
			switch (this.missionId)
			{
			case 0:
				this.InitTutorialAsmB01(this);
				return;
			case 1:
				this.InitTutorial02Asm(this);
				return;
			case 2:
				this.InitTutorial03Asm(this);
				return;
			case 3:
				this.InitTutorial04Asm(this);
				return;
			case 4:
				this.InitTutorial05Asm(this);
				return;
			case 5:
				this.InitTutorial06Asm(this);
				return;
			case 6:
				this.InitTutorial07Asm(this);
				return;
			case 7:
				this.InitTutorial08Asm(this);
				return;
			case 8:
				this.InitTutorial09Asm(this);
				return;
			case 9:
				this.InitTutorial10Asm(this);
				return;
			case 10:
				this.InitTutorial11Asm(this);
				return;
			default:
				return;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060029EA RID: 10730 RVA: 0x0004347F File Offset: 0x0004167F
		public Random random
		{
			get
			{
				return GameManager._random;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060029EB RID: 10731 RVA: 0x00043486 File Offset: 0x00041686
		// (set) Token: 0x060029EC RID: 10732 RVA: 0x0004348E File Offset: 0x0004168E
		public bool SpectatorMode { get; private set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x00043497 File Offset: 0x00041697
		// (set) Token: 0x060029EE RID: 10734 RVA: 0x0004349F File Offset: 0x0004169F
		public bool GameFinished { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x000434A8 File Offset: 0x000416A8
		// (set) Token: 0x060029F0 RID: 10736 RVA: 0x000434B0 File Offset: 0x000416B0
		public bool StatsCalculated { get; private set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000434B9 File Offset: 0x000416B9
		// (set) Token: 0x060029F2 RID: 10738 RVA: 0x000434C1 File Offset: 0x000416C1
		public bool GameLoading { get; private set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000434CA File Offset: 0x000416CA
		// (set) Token: 0x060029F4 RID: 10740 RVA: 0x000434D2 File Offset: 0x000416D2
		public bool InvadersFromAfar { get; private set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060029F5 RID: 10741 RVA: 0x000434DB File Offset: 0x000416DB
		// (set) Token: 0x060029F6 RID: 10742 RVA: 0x000434E3 File Offset: 0x000416E3
		public bool GameStarted { get; private set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000434EC File Offset: 0x000416EC
		// (set) Token: 0x060029F8 RID: 10744 RVA: 0x000434F4 File Offset: 0x000416F4
		public int TurnCount { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x000434FD File Offset: 0x000416FD
		// (set) Token: 0x060029FA RID: 10746 RVA: 0x00043505 File Offset: 0x00041705
		public int UndoType { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060029FB RID: 10747 RVA: 0x0004350E File Offset: 0x0004170E
		// (set) Token: 0x060029FC RID: 10748 RVA: 0x00043516 File Offset: 0x00041716
		public bool FactoryCardsShown { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060029FD RID: 10749 RVA: 0x0004351F File Offset: 0x0004171F
		// (set) Token: 0x060029FE RID: 10750 RVA: 0x00043527 File Offset: 0x00041727
		public bool IsCampaign { get; set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x00043530 File Offset: 0x00041730
		// (set) Token: 0x06002A00 RID: 10752 RVA: 0x00043538 File Offset: 0x00041738
		public bool IsChallenge { get; set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x00043541 File Offset: 0x00041741
		// (set) Token: 0x06002A02 RID: 10754 RVA: 0x00043549 File Offset: 0x00041749
		public bool IsMultiplayer { get; set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06002A03 RID: 10755 RVA: 0x00043552 File Offset: 0x00041752
		// (set) Token: 0x06002A04 RID: 10756 RVA: 0x0004355A File Offset: 0x0004175A
		public bool IsHotSeat { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x00043563 File Offset: 0x00041763
		// (set) Token: 0x06002A06 RID: 10758 RVA: 0x0004356B File Offset: 0x0004176B
		public bool IsAIHotSeat { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06002A07 RID: 10759 RVA: 0x00043574 File Offset: 0x00041774
		// (set) Token: 0x06002A08 RID: 10760 RVA: 0x0004357C File Offset: 0x0004177C
		public bool IsRanked { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06002A09 RID: 10761 RVA: 0x00043585 File Offset: 0x00041785
		// (set) Token: 0x06002A0A RID: 10762 RVA: 0x0004358D File Offset: 0x0004178D
		public bool IsAsynchronous { get; set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06002A0B RID: 10763 RVA: 0x00043596 File Offset: 0x00041796
		// (set) Token: 0x06002A0C RID: 10764 RVA: 0x0004359E File Offset: 0x0004179E
		public bool IsPrivate { get; set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06002A0D RID: 10765 RVA: 0x000435A7 File Offset: 0x000417A7
		// (set) Token: 0x06002A0E RID: 10766 RVA: 0x000435AF File Offset: 0x000417AF
		public bool SendingToPlayer { get; set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06002A0F RID: 10767 RVA: 0x000435B8 File Offset: 0x000417B8
		// (set) Token: 0x06002A10 RID: 10768 RVA: 0x000435C0 File Offset: 0x000417C0
		public int SynchronizePlayerId { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06002A11 RID: 10769 RVA: 0x000435C9 File Offset: 0x000417C9
		// (set) Token: 0x06002A12 RID: 10770 RVA: 0x000435D1 File Offset: 0x000417D1
		public bool TestingMode { get; set; }

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06002A13 RID: 10771 RVA: 0x000EFCB8 File Offset: 0x000EDEB8
		// (remove) Token: 0x06002A14 RID: 10772 RVA: 0x000EFCF0 File Offset: 0x000EDEF0
		public event GameManager.UpdateCurrentPlayerStats UpdatePlayerStats;

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x06002A15 RID: 10773 RVA: 0x000EFD28 File Offset: 0x000EDF28
		// (remove) Token: 0x06002A16 RID: 10774 RVA: 0x000EFD60 File Offset: 0x000EDF60
		public event GameManager.HighlightObjectiveCard ObjectiveCardGetHighlighted;

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x06002A17 RID: 10775 RVA: 0x000EFD98 File Offset: 0x000EDF98
		// (remove) Token: 0x06002A18 RID: 10776 RVA: 0x000EFDD0 File Offset: 0x000EDFD0
		public event GameManager.EnableEncounter EncounterGetEnabled;

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x06002A19 RID: 10777 RVA: 0x000EFE08 File Offset: 0x000EE008
		// (remove) Token: 0x06002A1A RID: 10778 RVA: 0x000EFE40 File Offset: 0x000EE040
		public event GameManager.EnableFactory FactoryGetEnabled;

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x06002A1B RID: 10779 RVA: 0x000EFE78 File Offset: 0x000EE078
		// (remove) Token: 0x06002A1C RID: 10780 RVA: 0x000EFEB0 File Offset: 0x000EE0B0
		public event GameManager.GameEnded GameHasEnded;

		// Token: 0x140000FD RID: 253
		// (add) Token: 0x06002A1D RID: 10781 RVA: 0x000EFEE8 File Offset: 0x000EE0E8
		// (remove) Token: 0x06002A1E RID: 10782 RVA: 0x000EFF20 File Offset: 0x000EE120
		public event GameManager.BattleCardsStatus CombatCardsAmountStatus;

		// Token: 0x140000FE RID: 254
		// (add) Token: 0x06002A1F RID: 10783 RVA: 0x000EFF58 File Offset: 0x000EE158
		// (remove) Token: 0x06002A20 RID: 10784 RVA: 0x000EFF90 File Offset: 0x000EE190
		public event GameManager.ActionExecuted ObtainActionInfo;

		// Token: 0x140000FF RID: 255
		// (add) Token: 0x06002A21 RID: 10785 RVA: 0x000EFFC8 File Offset: 0x000EE1C8
		// (remove) Token: 0x06002A22 RID: 10786 RVA: 0x000F0000 File Offset: 0x000EE200
		public event Action BotTurnEnded;

		// Token: 0x14000100 RID: 256
		// (add) Token: 0x06002A23 RID: 10787 RVA: 0x000F0038 File Offset: 0x000EE238
		// (remove) Token: 0x06002A24 RID: 10788 RVA: 0x000F0070 File Offset: 0x000EE270
		public event Action TurnEnded;

		// Token: 0x14000101 RID: 257
		// (add) Token: 0x06002A25 RID: 10789 RVA: 0x000F00A8 File Offset: 0x000EE2A8
		// (remove) Token: 0x06002A26 RID: 10790 RVA: 0x000F00E0 File Offset: 0x000EE2E0
		public event GameManager.ActionFinished ActionHasFinished;

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x06002A27 RID: 10791 RVA: 0x000F0118 File Offset: 0x000EE318
		// (remove) Token: 0x06002A28 RID: 10792 RVA: 0x000F0150 File Offset: 0x000EE350
		public event GameManager.MessageSend ActionWasSent;

		// Token: 0x14000103 RID: 259
		// (add) Token: 0x06002A29 RID: 10793 RVA: 0x000F0188 File Offset: 0x000EE388
		// (remove) Token: 0x06002A2A RID: 10794 RVA: 0x000F01C0 File Offset: 0x000EE3C0
		public event GameManager.GainCombatCards GainedCombatCards;

		// Token: 0x14000104 RID: 260
		// (add) Token: 0x06002A2B RID: 10795 RVA: 0x000F01F8 File Offset: 0x000EE3F8
		// (remove) Token: 0x06002A2C RID: 10796 RVA: 0x000F0230 File Offset: 0x000EE430
		public event GameManager.EnableInput InputWasEnabled;

		// Token: 0x14000105 RID: 261
		// (add) Token: 0x06002A2D RID: 10797 RVA: 0x000F0268 File Offset: 0x000EE468
		// (remove) Token: 0x06002A2E RID: 10798 RVA: 0x000F02A0 File Offset: 0x000EE4A0
		public event GameManager.GainFactoryCards GainedFactoryCards;

		// Token: 0x14000106 RID: 262
		// (add) Token: 0x06002A2F RID: 10799 RVA: 0x000F02D8 File Offset: 0x000EE4D8
		// (remove) Token: 0x06002A30 RID: 10800 RVA: 0x000F0310 File Offset: 0x000EE510
		public event GameManager.ShowFactoryCards ShowedFactoryCards;

		// Token: 0x14000107 RID: 263
		// (add) Token: 0x06002A31 RID: 10801 RVA: 0x000F0348 File Offset: 0x000EE548
		// (remove) Token: 0x06002A32 RID: 10802 RVA: 0x000F0380 File Offset: 0x000EE580
		public event GameManager.ShowEmptyCards ShowedEmptyCards;

		// Token: 0x14000108 RID: 264
		// (add) Token: 0x06002A33 RID: 10803 RVA: 0x000F03B8 File Offset: 0x000EE5B8
		// (remove) Token: 0x06002A34 RID: 10804 RVA: 0x000F03F0 File Offset: 0x000EE5F0
		public event GameManager.OnChooseCard CardChoosen;

		// Token: 0x14000109 RID: 265
		// (add) Token: 0x06002A35 RID: 10805 RVA: 0x000F0428 File Offset: 0x000EE628
		// (remove) Token: 0x06002A36 RID: 10806 RVA: 0x000F0460 File Offset: 0x000EE660
		public event GameManager.ShowChoosenFactory ShowedFactory;

		// Token: 0x1400010A RID: 266
		// (add) Token: 0x06002A37 RID: 10807 RVA: 0x000F0498 File Offset: 0x000EE698
		// (remove) Token: 0x06002A38 RID: 10808 RVA: 0x000F04D0 File Offset: 0x000EE6D0
		public event GameManager.AfterCardAdded CardAdded;

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x06002A39 RID: 10809 RVA: 0x000F0508 File Offset: 0x000EE708
		// (remove) Token: 0x06002A3A RID: 10810 RVA: 0x000F0540 File Offset: 0x000EE740
		public event GameManager.OnEncounterButton EncounterButtonClicked;

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x06002A3B RID: 10811 RVA: 0x000F0578 File Offset: 0x000EE778
		// (remove) Token: 0x06002A3C RID: 10812 RVA: 0x000F05B0 File Offset: 0x000EE7B0
		public event GameManager.OnShowEncounterCard ShowEncounter;

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x06002A3D RID: 10813 RVA: 0x000F05E8 File Offset: 0x000EE7E8
		// (remove) Token: 0x06002A3E RID: 10814 RVA: 0x000F0620 File Offset: 0x000EE820
		public event GameManager.OnEncounterOption ChooseOption;

		// Token: 0x1400010E RID: 270
		// (add) Token: 0x06002A3F RID: 10815 RVA: 0x000F0658 File Offset: 0x000EE858
		// (remove) Token: 0x06002A40 RID: 10816 RVA: 0x000F0690 File Offset: 0x000EE890
		public event GameManager.OnEncounterClosed EncounterClosed;

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x06002A41 RID: 10817 RVA: 0x000F06C8 File Offset: 0x000EE8C8
		// (remove) Token: 0x06002A42 RID: 10818 RVA: 0x000F0700 File Offset: 0x000EE900
		public event GameManager.OnShowObjective ShowObjective;

		// Token: 0x14000110 RID: 272
		// (add) Token: 0x06002A43 RID: 10819 RVA: 0x000F0738 File Offset: 0x000EE938
		// (remove) Token: 0x06002A44 RID: 10820 RVA: 0x000F0770 File Offset: 0x000EE970
		public event GameManager.ShowStats OnShowStats;

		// Token: 0x14000111 RID: 273
		// (add) Token: 0x06002A45 RID: 10821 RVA: 0x000F07A8 File Offset: 0x000EE9A8
		// (remove) Token: 0x06002A46 RID: 10822 RVA: 0x000F07E0 File Offset: 0x000EE9E0
		public event GameManager.OnMoveUnit OnEnemyUnitMoved;

		// Token: 0x14000112 RID: 274
		// (add) Token: 0x06002A47 RID: 10823 RVA: 0x000F0818 File Offset: 0x000EEA18
		// (remove) Token: 0x06002A48 RID: 10824 RVA: 0x000F0850 File Offset: 0x000EEA50
		public event GameManager.OnEnemyMove OnEnemyMoved;

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x06002A49 RID: 10825 RVA: 0x000F0888 File Offset: 0x000EEA88
		// (remove) Token: 0x06002A4A RID: 10826 RVA: 0x000F08C0 File Offset: 0x000EEAC0
		public event GameManager.OnEnemyLoadResources OnEnemyLoadedResources;

		// Token: 0x14000114 RID: 276
		// (add) Token: 0x06002A4B RID: 10827 RVA: 0x000F08F8 File Offset: 0x000EEAF8
		// (remove) Token: 0x06002A4C RID: 10828 RVA: 0x000F0930 File Offset: 0x000EEB30
		public event GameManager.OnEnemyLoadWorker OnEnemyLoadedWorker;

		// Token: 0x14000115 RID: 277
		// (add) Token: 0x06002A4D RID: 10829 RVA: 0x000F0968 File Offset: 0x000EEB68
		// (remove) Token: 0x06002A4E RID: 10830 RVA: 0x000F09A0 File Offset: 0x000EEBA0
		public event GameManager.OnEnemyUnloadWorker OnEnemyUnloadedWorker;

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x06002A4F RID: 10831 RVA: 0x000F09D8 File Offset: 0x000EEBD8
		// (remove) Token: 0x06002A50 RID: 10832 RVA: 0x000F0A10 File Offset: 0x000EEC10
		public event GameManager.OnEnemyRetreatMove OnEnemyRetreatMoved;

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x06002A51 RID: 10833 RVA: 0x000F0A48 File Offset: 0x000EEC48
		// (remove) Token: 0x06002A52 RID: 10834 RVA: 0x000F0A80 File Offset: 0x000EEC80
		public event GameManager.OnEnemyProduce OnEnemyProduced;

		// Token: 0x14000118 RID: 280
		// (add) Token: 0x06002A53 RID: 10835 RVA: 0x000F0AB8 File Offset: 0x000EECB8
		// (remove) Token: 0x06002A54 RID: 10836 RVA: 0x000F0AF0 File Offset: 0x000EECF0
		public event GameManager.OnEnemyGainWorker OnEnemyGainedWorker;

		// Token: 0x14000119 RID: 281
		// (add) Token: 0x06002A55 RID: 10837 RVA: 0x000F0B28 File Offset: 0x000EED28
		// (remove) Token: 0x06002A56 RID: 10838 RVA: 0x000F0B60 File Offset: 0x000EED60
		public event GameManager.OnEnemyGainWorkerEnd OnEnemyGainedWorkerEnds;

		// Token: 0x1400011A RID: 282
		// (add) Token: 0x06002A57 RID: 10839 RVA: 0x000F0B98 File Offset: 0x000EED98
		// (remove) Token: 0x06002A58 RID: 10840 RVA: 0x000F0BD0 File Offset: 0x000EEDD0
		public event GameManager.OnEnemyUpgrade OnEnemyUpgraded;

		// Token: 0x1400011B RID: 283
		// (add) Token: 0x06002A59 RID: 10841 RVA: 0x000F0C08 File Offset: 0x000EEE08
		// (remove) Token: 0x06002A5A RID: 10842 RVA: 0x000F0C40 File Offset: 0x000EEE40
		public event GameManager.OnEnlistRecruit OnEnemyRecruited;

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x06002A5B RID: 10843 RVA: 0x000F0C78 File Offset: 0x000EEE78
		// (remove) Token: 0x06002A5C RID: 10844 RVA: 0x000F0CB0 File Offset: 0x000EEEB0
		public event GameManager.OnEnemyEnlistBonus OnEnemyRecruitBonusObtain;

		// Token: 0x1400011D RID: 285
		// (add) Token: 0x06002A5D RID: 10845 RVA: 0x000F0CE8 File Offset: 0x000EEEE8
		// (remove) Token: 0x06002A5E RID: 10846 RVA: 0x000F0D20 File Offset: 0x000EEF20
		public event GameManager.OnEnemysEnlistBonusEnd OnEnemysBonusEnd;

		// Token: 0x1400011E RID: 286
		// (add) Token: 0x06002A5F RID: 10847 RVA: 0x000F0D58 File Offset: 0x000EEF58
		// (remove) Token: 0x06002A60 RID: 10848 RVA: 0x000F0D90 File Offset: 0x000EEF90
		public event GameManager.OnEnemyBuild OnEnemyBuilded;

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x06002A61 RID: 10849 RVA: 0x000F0DC8 File Offset: 0x000EEFC8
		// (remove) Token: 0x06002A62 RID: 10850 RVA: 0x000F0E00 File Offset: 0x000EF000
		public event GameManager.OnEnemyDeploy OnEnemyDeployed;

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x06002A63 RID: 10851 RVA: 0x000F0E38 File Offset: 0x000EF038
		// (remove) Token: 0x06002A64 RID: 10852 RVA: 0x000F0E70 File Offset: 0x000EF070
		public event GameManager.OnEnemyTrade OnEnemyTraded;

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x06002A65 RID: 10853 RVA: 0x000F0EA8 File Offset: 0x000EF0A8
		// (remove) Token: 0x06002A66 RID: 10854 RVA: 0x000F0EE0 File Offset: 0x000EF0E0
		public event GameManager.OnEnemyPayResources OnEnemyPaidResources;

		// Token: 0x14000122 RID: 290
		// (add) Token: 0x06002A67 RID: 10855 RVA: 0x000F0F18 File Offset: 0x000EF118
		// (remove) Token: 0x06002A68 RID: 10856 RVA: 0x000F0F50 File Offset: 0x000EF150
		public event GameManager.OnEnemyGainStatsAction OnEnemyGainStats;

		// Token: 0x14000123 RID: 291
		// (add) Token: 0x06002A69 RID: 10857 RVA: 0x000F0F88 File Offset: 0x000EF188
		// (remove) Token: 0x06002A6A RID: 10858 RVA: 0x000F0FC0 File Offset: 0x000EF1C0
		public event GameManager.OnGameEnded MultiplayerGameEnded;

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x06002A6B RID: 10859 RVA: 0x000F0FF8 File Offset: 0x000EF1F8
		// (remove) Token: 0x06002A6C RID: 10860 RVA: 0x000F1030 File Offset: 0x000EF230
		public event GameManager.OnGameSynchronized GameSynchronized;

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x06002A6D RID: 10861 RVA: 0x000F1068 File Offset: 0x000EF268
		// (remove) Token: 0x06002A6E RID: 10862 RVA: 0x000F10A0 File Offset: 0x000EF2A0
		public event GameManager.OnGameLoaded GameLoaded;

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x06002A6F RID: 10863 RVA: 0x000F10D8 File Offset: 0x000EF2D8
		// (remove) Token: 0x06002A70 RID: 10864 RVA: 0x000F1110 File Offset: 0x000EF310
		public event GameManager.OnBattlefieldChose BattlefieldChoosen;

		// Token: 0x14000127 RID: 295
		// (add) Token: 0x06002A71 RID: 10865 RVA: 0x000F1148 File Offset: 0x000EF348
		// (remove) Token: 0x06002A72 RID: 10866 RVA: 0x000F1180 File Offset: 0x000EF380
		public event GameManager.OnNextTurn ChangeTurn;

		// Token: 0x14000128 RID: 296
		// (add) Token: 0x06002A73 RID: 10867 RVA: 0x000F11B8 File Offset: 0x000EF3B8
		// (remove) Token: 0x06002A74 RID: 10868 RVA: 0x000F11F0 File Offset: 0x000EF3F0
		public event GameManager.OnChangeSecondActivePlayer SecondActivePlayerChanged;

		// Token: 0x14000129 RID: 297
		// (add) Token: 0x06002A75 RID: 10869 RVA: 0x000F1228 File Offset: 0x000EF428
		// (remove) Token: 0x06002A76 RID: 10870 RVA: 0x000F1260 File Offset: 0x000EF460
		public event GameManager.OnDisableActivePlayer ActivePlayerDisabled;

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x06002A77 RID: 10871 RVA: 0x000F1298 File Offset: 0x000EF498
		// (remove) Token: 0x06002A78 RID: 10872 RVA: 0x000F12D0 File Offset: 0x000EF4D0
		public event GameManager.OnBotGainCombatCards SendCombatCards;

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x06002A79 RID: 10873 RVA: 0x000F1308 File Offset: 0x000EF508
		// (remove) Token: 0x06002A7A RID: 10874 RVA: 0x000F1340 File Offset: 0x000EF540
		public event GameManager.OnCombatAbilityUsed CombatAbilityUsed;

		// Token: 0x1400012C RID: 300
		// (add) Token: 0x06002A7B RID: 10875 RVA: 0x000F1378 File Offset: 0x000EF578
		// (remove) Token: 0x06002A7C RID: 10876 RVA: 0x000F13B0 File Offset: 0x000EF5B0
		public event Action<int, int> OnEncounterEnded;

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x06002A7D RID: 10877 RVA: 0x000F13E8 File Offset: 0x000EF5E8
		// (remove) Token: 0x06002A7E RID: 10878 RVA: 0x000F1420 File Offset: 0x000EF620
		public event Action<int> ActivePlayerChanged;

		// Token: 0x1400012E RID: 302
		// (add) Token: 0x06002A7F RID: 10879 RVA: 0x000F1458 File Offset: 0x000EF658
		// (remove) Token: 0x06002A80 RID: 10880 RVA: 0x000F1490 File Offset: 0x000EF690
		public event Action<int, int> OnSetNewActivePlayer;

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x000435DA File Offset: 0x000417DA
		public int PlayerCount
		{
			get
			{
				return this.players.Count;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000435E7 File Offset: 0x000417E7
		public Player PlayerCurrent
		{
			get
			{
				if (this.players.Count <= this.playerCurrentId)
				{
					return null;
				}
				return this.players[this.playerCurrentId];
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x0004360F File Offset: 0x0004180F
		public int PlayerCurrentId
		{
			get
			{
				return this.playerCurrentId;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x00043617 File Offset: 0x00041817
		public Player PlayerOwner
		{
			get
			{
				if (this.playerId == -1)
				{
					return null;
				}
				return this.players[this.playerId];
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06002A85 RID: 10885 RVA: 0x00043635 File Offset: 0x00041835
		public bool IsServer
		{
			get
			{
				return this.IsMultiplayer && this.PlayerOwner == null;
			}
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x0004364A File Offset: 0x0004184A
		public bool IsPlayerOwnerAlreadyLoaded()
		{
			return this.players.Count > this.playerId;
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x0004365F File Offset: 0x0004185F
		public bool IsPlayerOwnerBeingLoaded()
		{
			return this.GameLoading && this.players.Count == this.playerId;
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06002A88 RID: 10888 RVA: 0x0004367E File Offset: 0x0004187E
		public Player PlayerMaster
		{
			get
			{
				if (this.IsMultiplayer)
				{
					return this.PlayerOwner;
				}
				return this.PlayerCurrent;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x00043695 File Offset: 0x00041895
		// (set) Token: 0x06002A8A RID: 10890 RVA: 0x0004369D File Offset: 0x0004189D
		public StructureBonusCard StructureBonus { get; private set; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06002A8B RID: 10891 RVA: 0x000436A6 File Offset: 0x000418A6
		// (set) Token: 0x06002A8C RID: 10892 RVA: 0x000436AE File Offset: 0x000418AE
		public EncounterCard LastEncounterCard { get; private set; }

		// Token: 0x06002A8D RID: 10893 RVA: 0x000F14C8 File Offset: 0x000EF6C8
		public GameManager()
		{
			this.gameBoard = new GameBoard(this);
			this.combatManager = new CombatManager(this);
			this.actionManager = new ActionManager(this);
			this.actionLog = new ActionLog(this);
			this.moveManager = new MoveManager(this);
			this.tokenManager = new TokenManager(this);
			this.combatCards = new CardDeck<CombatCard>(this);
			this.usedCombatCards = new CardDeck<CombatCard>(this);
			this.objectiveCards = new CardDeck<ObjectiveCard>(this);
			this.encounterCards = new CardDeck<EncounterCard>(this);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000F15BC File Offset: 0x000EF7BC
		public void Init(List<MatAndFactionSelection.PlayerEntry> playerList, bool promoCardsOn, bool invadersFromAfar, bool balanced = true)
		{
			this.missionId = -1;
			this.TurnCount = 0;
			this.playerId = -1;
			this.IsMultiplayer = false;
			this.IsHotSeat = true;
			this.IsCampaign = false;
			this.InvadersFromAfar = invadersFromAfar;
			this.InitMap();
			this.promoCardsOn = promoCardsOn;
			this.SetCardDecks(playerList.Count);
			this.players = MatAndFactionSelection.CreatePlayerList(this, playerList, invadersFromAfar, balanced);
			this.gameBoard.UpdateHexOwnerships();
			this.SortPlayers();
			this.GameStartTime = DateTime.UtcNow;
			if (this.GetPlayersWithoutAICount() == 1)
			{
				this.IsAIHotSeat = true;
			}
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000F1650 File Offset: 0x000EF850
		public void InitServer(List<MatAndFactionSelection.PlayerEntry> playerList, bool promoCards, bool invadersFromAfar, bool allRandom)
		{
			this.promoCardsOn = promoCards;
			this.InvadersFromAfar = invadersFromAfar;
			this.gameBoard.InitMap();
			this.SetCardDecks(playerList.Count);
			this.players = MatAndFactionSelection.CreatePlayerList(this, playerList, invadersFromAfar, true);
			this.gameBoard.UpdateHexOwnerships();
			this.GameStartTime = DateTime.UtcNow;
			this.IsMultiplayer = true;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000F16B0 File Offset: 0x000EF8B0
		public void StartMultiplayer(int numberOfPlayers, int playerIndex, int structureBonus, bool isRanked, bool isAsynchronous, bool isPrivate, bool promoCards)
		{
			this.TurnCount = 0;
			this.GameStarted = false;
			this.IsRanked = isRanked;
			this.IsAsynchronous = isAsynchronous;
			this.IsPrivate = isPrivate;
			this.promoCardsOn = promoCards;
			this.missionId = -1;
			this.SetAmountOfCombatCardsLeft();
			switch (structureBonus)
			{
			case 1:
				this.StructureBonus = new TunnelNeighbourCard();
				break;
			case 2:
				this.StructureBonus = new LakeNeighbourCard();
				break;
			case 3:
				this.StructureBonus = new EncounterNeighbourCard();
				break;
			case 4:
				this.StructureBonus = new OccupationTunnelCard();
				break;
			case 5:
				this.StructureBonus = new InlineStructuresCard();
				break;
			case 6:
				this.StructureBonus = new OccupationResourceCard();
				break;
			}
			this.gameBoard.UpdateHexOwnerships();
			Faction faction = this.players[playerIndex].matFaction.faction;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (this.players[i].matFaction.faction == faction)
				{
					this.playerId = i;
				}
			}
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000F17C0 File Offset: 0x000EF9C0
		public void SetAmountOfCombatCardsLeft()
		{
			this.amountOfCombatCardsLeft = 42;
			for (int i = 0; i < this.players.Count; i++)
			{
				this.amountOfCombatCardsLeft -= this.players[i].combatCards.Count;
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000F1810 File Offset: 0x000EFA10
		public void InitMultiplayer()
		{
			this.InitMap();
			this.GameStarted = false;
			this.IsHotSeat = (this.IsAIHotSeat = (this.IsCampaign = false));
			this.IsMultiplayer = true;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000436B7 File Offset: 0x000418B7
		public void StartSpectatorMode()
		{
			this.SpectatorMode = true;
			this.SetAmountOfCombatCardsLeft();
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000F184C File Offset: 0x000EFA4C
		public void InitPlayer(int faction, int matPlayerType, List<int> combatCards, List<int> objectiveCards, int type, string playerName)
		{
			MatFaction matFaction = new MatFaction(this, (Faction)faction);
			MatPlayer matPlayer = new MatPlayer(this, (PlayerMatType)matPlayerType);
			Player player = new Player(this, matFaction, matPlayer, type);
			foreach (int num in combatCards)
			{
				player.AddCombatCard(new CombatCard(num));
			}
			foreach (int num2 in objectiveCards)
			{
				player.AddObjectiveCard(new ObjectiveCard(this, num2));
			}
			player.aiPlayer = new AiPlayer(player, this);
			player.Name = playerName;
			this.players.Add(player);
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000436C6 File Offset: 0x000418C6
		public void DisableTestingMode()
		{
			this.TestingMode = false;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000F1924 File Offset: 0x000EFB24
		public void InitMap()
		{
			this.ClearDelegates();
			this.FactoryCardsShown = false;
			this.LastEncounterCard = null;
			this.GameFinished = false;
			this.StatsCalculated = false;
			this.EndGamePerformed = false;
			this.SpectatorMode = false;
			this.players.Clear();
			this.playerCurrentId = 0;
			this.gameBoard.InitMap();
			this.actionManager.BreakSectionAction(false);
			this.combatManager.Clear();
			this.combatManager.ClearDelegates();
			this.actionLog.Clear();
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000F19AC File Offset: 0x000EFBAC
		private void ClearDelegates()
		{
			this.ObjectiveCardGetHighlighted = null;
			this.EncounterGetEnabled = null;
			this.FactoryGetEnabled = null;
			this.GameHasEnded = null;
			this.ActionHasFinished = null;
			this.GainedCombatCards = null;
			this.ShowedFactoryCards = null;
			this.ShowedEmptyCards = null;
			this.GainedFactoryCards = null;
			this.ShowedFactory = null;
			this.CardChoosen = null;
			this.CardAdded = null;
			this.EncounterButtonClicked = null;
			this.ShowEncounter = null;
			this.ChooseOption = null;
			this.ShowObjective = null;
			this.OnEnemyUnitMoved = null;
			this.OnEnemyProduced = null;
			this.OnEnemyBuilded = null;
			this.OnEnemyDeployed = null;
			this.OnEnemyGainStats = null;
			this.OnEnemyPaidResources = null;
			this.OnEnemyRecruitBonusObtain = null;
			this.OnEnemyRecruited = null;
			this.OnEnemyTraded = null;
			this.OnEnemyUpgraded = null;
			this.MultiplayerGameEnded = null;
			this.GameSynchronized = null;
			this.ChangeTurn = null;
			this.BotTurnEnded = null;
			if (!this.IsMultiplayer)
			{
				this.ActionWasSent = null;
			}
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000F1A9C File Offset: 0x000EFC9C
		public void SortPlayers()
		{
			List<Player> list = new List<Player>();
			this.players = this.players.OrderBy((Player p) => p.matPlayer.StartingOrderNumber).ToList<Player>();
			list.Add(this.players[0]);
			int factionIterator = (int)list[0].matFaction.faction;
			int i = 1;
			Predicate<Player> predicate_1 = null;
			while (i < this.players.Count)
			{
				int num = factionIterator + 1;
				factionIterator = num;
				if (factionIterator >= Enum.GetValues(typeof(Faction)).Length)
				{
					factionIterator = 0;
				}
				List<Player> list2 = this.players;
				Predicate<Player> predicate;
				if ((predicate = predicate_1) == null)
				{
					predicate = (predicate_1 = (Player p) => p.matFaction.faction == (Faction)factionIterator);
				}
				Player player = list2.Find(predicate);
				if (player != null)
				{
					list.Add(player);
					i++;
				}
			}
			this.players.Clear();
			this.players = list.GetRange(0, list.Count);
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000F1BB8 File Offset: 0x000EFDB8
		public List<CombatCard> GetCombatCards(int amount)
		{
			if (this.combatCards.CardsLeft() < amount)
			{
				this.RenewCombatCards();
			}
			List<CombatCard> list = new List<CombatCard>();
			list.AddRange(this.combatCards.GetCards(amount));
			if (!this.IsMultiplayer)
			{
				this.InformAboutCombatCardsAmount(42 - this.CombatCardsInPlayersHands() - list.Count);
			}
			return list;
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000F1C10 File Offset: 0x000EFE10
		private int CombatCardsInPlayersHands()
		{
			int num = 0;
			foreach (Player player in this.players)
			{
				num += player.GetCombatCardsCount();
			}
			return num;
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000436CF File Offset: 0x000418CF
		public void InformAboutCombatCardsAmount()
		{
			this.InformAboutCombatCardsAmount(this.combatCards.CardsLeft());
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000436E2 File Offset: 0x000418E2
		public void InformAboutCombatCardsAmount(int cards)
		{
			this.amountOfCombatCardsLeft = cards;
			if (this.CombatCardsAmountStatus != null)
			{
				this.CombatCardsAmountStatus(cards);
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000436FF File Offset: 0x000418FF
		public List<ObjectiveCard> GetObjectiveCards(int amount)
		{
			List<ObjectiveCard> list = new List<ObjectiveCard>();
			list.AddRange(this.objectiveCards.GetCards(amount));
			return list;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x00043718 File Offset: 0x00041918
		public List<FactoryCard> GetFactoryCards()
		{
			return this.factoryCards;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x00043720 File Offset: 0x00041920
		public EncounterCard GetEncounterCard()
		{
			this.LastEncounterCard = this.encounterCards.GetCard();
			this.LastEncounterCard.SetPlayer(this.PlayerCurrent);
			this.PlayerCurrent.EncountersVisited++;
			return this.LastEncounterCard;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x0004375D File Offset: 0x0004195D
		public void SetTemporaryResources(Dictionary<ResourceType, int> temporaryResources)
		{
			this.temporaryResources = temporaryResources;
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000F1C68 File Offset: 0x000EFE68
		public void UpdateTemporaryResource(ResourceType resource, int amount)
		{
			Dictionary<ResourceType, int> dictionary = this.temporaryResources;
			dictionary[resource] += amount;
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x00043766 File Offset: 0x00041966
		public Dictionary<ResourceType, int> GetTemporaryResources()
		{
			return this.temporaryResources;
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x0004376E File Offset: 0x0004196E
		public void SetTemporaryCombatCardsCount(int count)
		{
			this.temporaryCombatCards = count;
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x00043777 File Offset: 0x00041977
		public void UpdateTemporaryCombatCardsCount(int amount)
		{
			this.temporaryCombatCards += amount;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x00043787 File Offset: 0x00041987
		public int GetTemporaryCombatCardsCount()
		{
			return this.temporaryCombatCards;
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x0004378F File Offset: 0x0004198F
		public void EnableTemporaryResourcesMechanism(bool running)
		{
			this.temporaryResourcesEnabled = running;
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x00043798 File Offset: 0x00041998
		public bool TemporaryResourcesEnabled()
		{
			return this.temporaryResourcesEnabled;
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000437A0 File Offset: 0x000419A0
		public void SetCardDecks(int numberOfPlayers)
		{
			this.SetCombatCards();
			this.SetObjectiveCards();
			this.SetStructureBonusCard();
			this.SetEncounterCards();
			this.SetFactoryCards(numberOfPlayers);
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000F1C90 File Offset: 0x000EFE90
		private void SetCombatCards()
		{
			this.usedCombatCards.ClearDeck();
			this.combatCards.ClearDeck();
			this.AddCombatCards(this.combatCards, 16, 2);
			this.AddCombatCards(this.combatCards, 12, 3);
			this.AddCombatCards(this.combatCards, 8, 4);
			this.AddCombatCards(this.combatCards, 6, 5);
			this.combatCards.ShuffleDeck();
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000437C1 File Offset: 0x000419C1
		private void RenewCombatCards()
		{
			while (this.usedCombatCards.CardsLeft() > 0)
			{
				this.combatCards.AddCard(this.usedCombatCards.GetCard());
			}
			this.combatCards.ShuffleDeck();
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000F1CF8 File Offset: 0x000EFEF8
		private void AddCombatCards(CardDeck<CombatCard> deck, int amount, int value)
		{
			for (int i = 0; i < amount; i++)
			{
				deck.AddCard(new CombatCard(value));
			}
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000437F4 File Offset: 0x000419F4
		public void AddUsedCombatCard(CombatCard usedCombatCard)
		{
			this.usedCombatCards.AddCard(usedCombatCard);
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000F1D20 File Offset: 0x000EFF20
		private void FillUsedCombatCards()
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int k = 0; k < 4; k++)
			{
				dictionary.Add(k, 0);
			}
			foreach (Player player in this.players)
			{
				int num;
				int i;
				for (i = 0; i < 4; i = num)
				{
					Dictionary<int, int> dictionary2 = dictionary;
					num = i;
					dictionary2[num] += player.combatCards.Count((CombatCard card) => card.CardId == i + 2);
					num = i + 1;
				}
			}
			for (int j = 0; j < this.combatCards.CardsLeft(); j++)
			{
				Dictionary<int, int> dictionary3 = dictionary;
				int num = this.combatCards.LookupCard(j).CardId - 2;
				int num2 = dictionary3[num];
				dictionary3[num] = num2 + 1;
			}
			this.AddCombatCards(this.usedCombatCards, 16 - dictionary[0], 2);
			this.AddCombatCards(this.usedCombatCards, 12 - dictionary[1], 3);
			this.AddCombatCards(this.usedCombatCards, 8 - dictionary[2], 4);
			this.AddCombatCards(this.usedCombatCards, 6 - dictionary[3], 5);
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x00043802 File Offset: 0x00041A02
		public int CombatCardsLeft()
		{
			if (this.IsMultiplayer && this.PlayerOwner != null)
			{
				return this.amountOfCombatCardsLeft;
			}
			return this.combatCards.CardsLeft() + this.usedCombatCards.CardsLeft();
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000F1E8C File Offset: 0x000F008C
		private void SetObjectiveCards()
		{
			this.objectiveCards.ClearDeck();
			int num = (this.promoCardsOn ? 27 : 23);
			for (int i = 1; i <= num; i++)
			{
				this.objectiveCards.AddCard(new ObjectiveCard(this, i));
			}
			this.objectiveCards.ShuffleDeck();
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000F1EDC File Offset: 0x000F00DC
		private void SetStructureBonusCard()
		{
			CardDeck<StructureBonusCard> cardDeck = new CardDeck<StructureBonusCard>(this);
			cardDeck.AddCard(new EncounterNeighbourCard());
			cardDeck.AddCard(new InlineStructuresCard());
			cardDeck.AddCard(new OccupationResourceCard());
			cardDeck.AddCard(new OccupationTunnelCard());
			cardDeck.AddCard(new TunnelNeighbourCard());
			cardDeck.AddCard(new LakeNeighbourCard());
			cardDeck.ShuffleDeck();
			this.StructureBonus = cardDeck.GetCard();
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x00043832 File Offset: 0x00041A32
		public void SetStructureBonusCard(StructureBonusCard structureBonusCard)
		{
			this.StructureBonus = structureBonusCard;
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x000F1F44 File Offset: 0x000F0144
		private void SetFactoryCards(int numberOfPlayers)
		{
			this.factoryCards.Clear();
			CardDeck<FactoryCard> cardDeck = new CardDeck<FactoryCard>(this);
			int num = (this.promoCardsOn ? 18 : 12);
			for (int i = 1; i <= num; i++)
			{
				cardDeck.AddCard(new FactoryCard(i, this));
			}
			cardDeck.ShuffleDeck();
			this.factoryCards.AddRange(cardDeck.GetCards(numberOfPlayers + 1));
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x000F1FA8 File Offset: 0x000F01A8
		private void SetEncounterCards()
		{
			this.encounterCards.ClearDeck();
			int num = (this.promoCardsOn ? 36 : 28);
			for (int i = 1; i <= num; i++)
			{
				this.encounterCards.AddCard(new EncounterCard(i, this));
			}
			this.encounterCards.ShuffleDeck();
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x0004383B File Offset: 0x00041A3B
		public bool PromoCardsEnabled()
		{
			return this.promoCardsOn;
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x0004383B File Offset: 0x00041A3B
		public bool EnablePromoCardsEnabled()
		{
			return this.promoCardsOn;
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x000F1FF8 File Offset: 0x000F01F8
		public string GetUnlockedPromoCards()
		{
			if (!this.promoCardsOn)
			{
				return "";
			}
			if (!this.serializedPromoCards.Equals(""))
			{
				return this.serializedPromoCards;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 29; i <= 36; i++)
			{
				stringBuilder.Append("encounter_card_");
				stringBuilder.Append(i);
				stringBuilder.Append(",");
			}
			for (int j = 13; j <= 18; j++)
			{
				stringBuilder.Append("factory_card_");
				stringBuilder.Append(j);
				stringBuilder.Append(",");
			}
			for (int k = 24; k <= 27; k++)
			{
				stringBuilder.Append("objective_card_");
				stringBuilder.Append(k);
				if (k < 27)
				{
					stringBuilder.Append(",");
				}
			}
			this.serializedPromoCards = stringBuilder.ToString();
			return this.serializedPromoCards;
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000F20D8 File Offset: 0x000F02D8
		public void ClearLastEncounterCard()
		{
			if (this.LastEncounterCard != null)
			{
				this.LastEncounterCard = null;
				this.PlayerCurrent.character.position.encounterTaken = true;
				if (this.IsMultiplayer && (this.PlayerOwner == this.PlayerCurrent || (!this.PlayerCurrent.IsHuman && this.PlayerOwner == null)))
				{
					this.OnActionSent(new EncounterEndedMessage(this.PlayerCurrent.character.position.posX, this.PlayerCurrent.character.position.posY));
				}
				if (this.EncounterClosed != null)
				{
					this.EncounterClosed();
				}
			}
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000F2180 File Offset: 0x000F0380
		public void OverridePlayerWithAI(int faction)
		{
			Player playerByFaction = this.GetPlayerByFaction((Faction)faction);
			playerByFaction.IsHuman = false;
			if (this.GetPlayersWithoutAICount() == 0)
			{
				return;
			}
			if (this.PlayerOwner == null)
			{
				if (this.PlayerCurrent == playerByFaction)
				{
					playerByFaction.aiPlayer.ContinueLeaverAction();
					return;
				}
				if (this.combatManager.GetBattlefields().Count > 0)
				{
					if (this.combatManager.GetSelectedBattlefield() != null && this.combatManager.CanPerformStep(playerByFaction))
					{
						playerByFaction.aiPlayer.PerformCombatStage(this.combatManager.GetActualStage());
						return;
					}
				}
				else if (this.actionManager.GetLastBonusAction() != null && this.actionManager.GetLastBonusAction().GetPlayer() == playerByFaction)
				{
					GainAction lastBonusAction = this.actionManager.GetLastBonusAction();
					switch (lastBonusAction.GetGainType())
					{
					case GainType.Coin:
						((GainCoin)lastBonusAction).SetCoins(lastBonusAction.Amount);
						break;
					case GainType.Popularity:
						((GainPopularity)lastBonusAction).SetPopularity(lastBonusAction.Amount);
						break;
					case GainType.Power:
						((GainPower)lastBonusAction).SetPower(lastBonusAction.Amount);
						break;
					case GainType.CombatCard:
						((GainCombatCard)lastBonusAction).SetCards(lastBonusAction.Amount);
						break;
					}
					this.actionManager.PrepareNextAction();
				}
			}
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x00043843 File Offset: 0x00041A43
		public void OverrideAIWithPlayer(int faction)
		{
			this.GetPlayerByFaction((Faction)faction).IsHuman = true;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000F22BC File Offset: 0x000F04BC
		public Player NextTurn()
		{
			if (this.players.Count > 0 && !this.GameFinished)
			{
				this.actionManager.BreakSectionAction(false);
				this.PlayerCurrent.lastMatSection = this.PlayerCurrent.currentMatSection;
				this.PlayerCurrent.currentMatSection = -1;
				this.PlayerCurrent.topActionFinished = false;
				this.PlayerCurrent.downActionFinished = false;
				this.PlayerCurrent.topActionInProgress = false;
				this.PlayerCurrent.bottomActionInProgress = false;
				this.PlayerCurrent.wonBattle = false;
				this.ClearLastEncounterCard();
				this.tokenManager.RemoveListeners();
				this.tokenManager.Clear();
				if (this.IsMultiplayer)
				{
					if ((this.PlayerOwner == null && !this.PlayerCurrent.IsHuman) || this.IsMyTurn())
					{
						this.OnActionSent(new EndTurnMessage(this));
					}
					if (this.ChangeTurn != null && this.IsMyTurn())
					{
						this.ChangeTurn();
					}
				}
				int num = this.playerCurrentId + 1;
				this.playerCurrentId = num;
				this.playerCurrentId = num % this.players.Count;
				if (this.playerCurrentId == 0)
				{
					num = this.TurnCount;
					this.TurnCount = num + 1;
				}
				return this.players[this.playerCurrentId];
			}
			return null;
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000F2408 File Offset: 0x000F0608
		public int GetPlayerLocalId(Player player)
		{
			return this.players.FindIndex((Player p) => p == player);
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x00043852 File Offset: 0x00041A52
		public int NextPlayerId()
		{
			return (this.playerCurrentId + 1) % this.players.Count;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x00043868 File Offset: 0x00041A68
		public bool IsNextPlayerHuman()
		{
			return this.players[this.NextPlayerId()].IsHuman;
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x00043880 File Offset: 0x00041A80
		public bool IsMyTurnNext()
		{
			return !this.SpectatorMode && this.NextPlayerId() == this.playerId;
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x0004389A File Offset: 0x00041A9A
		public List<Player> GetPlayers()
		{
			return new List<Player>(this.players);
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000F243C File Offset: 0x000F063C
		public Player GetPreviousHumanPlayer()
		{
			Player player = this.PlayerCurrent;
			int num = this.playerCurrentId;
			for (;;)
			{
				num--;
				if (num < 0)
				{
					num = this.PlayerCount - 1;
				}
				if (num == this.playerCurrentId)
				{
					break;
				}
				player = this.players[num];
				if (player.IsHuman)
				{
					return player;
				}
			}
			return this.PlayerCurrent;
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000F2490 File Offset: 0x000F0690
		public Player GetNextHumanPlayer()
		{
			Player player = this.PlayerCurrent;
			int num = this.playerCurrentId;
			for (;;)
			{
				num++;
				if (num > this.PlayerCount)
				{
					num = 0;
				}
				if (num == this.playerCurrentId)
				{
					break;
				}
				player = this.players[num];
				if (player.IsHuman)
				{
					return player;
				}
			}
			return this.PlayerCurrent;
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000438A7 File Offset: 0x00041AA7
		public IEnumerable<Player> GetPlayersWithoutAI()
		{
			return this.players.Where((Player player) => player.IsHuman);
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000438D3 File Offset: 0x00041AD3
		public int GetPlayersWithoutAICount()
		{
			return this.players.Where((Player player) => player.IsHuman).Count<Player>();
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x00043904 File Offset: 0x00041B04
		public IEnumerable<Player> GetAIPlayers()
		{
			return this.players.Where((Player player) => !player.IsHuman);
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x00043930 File Offset: 0x00041B30
		public int GetAIPlayersCount()
		{
			return this.players.Where((Player player) => !player.IsHuman).Count<Player>();
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000F24E0 File Offset: 0x000F06E0
		public List<Player> GetPlayerNeighbours(Player player)
		{
			List<Player> list = new List<Player>();
			if (this.players.Count == 2)
			{
				list.AddRange(this.players);
				list.Remove(player);
			}
			else
			{
				int faction = (int)player.matFaction.faction;
				int length = Enum.GetValues(typeof(Faction)).Length;
				int factionIterator = faction;
				Predicate<Player> predicate_0 = null;
				for (int i = 0; i < length - 1; i++)
				{
					int num = factionIterator;
					factionIterator = num + 1;
					if (factionIterator == length)
					{
						factionIterator = 0;
					}
					List<Player> list2 = this.players;
					Predicate<Player> predicate;
					if ((predicate = predicate_0) == null)
					{
						predicate = (predicate_0 = (Player p) => p.matFaction.faction == (Faction)factionIterator);
					}
					Player player2 = list2.Find(predicate);
					if (player2 != null)
					{
						list.Add(player2);
						break;
					}
				}
				factionIterator = faction;
				Predicate<Player> predicate_1 = null;
				for (int j = 0; j < length - 1; j++)
				{
					int num = factionIterator;
					factionIterator = num - 1;
					if (factionIterator == -1)
					{
						factionIterator = length - 1;
					}
					List<Player> list3 = this.players;
					Predicate<Player> predicate2;
					if ((predicate2 = predicate_1) == null)
					{
						predicate2 = (predicate_1 = (Player p) => p.matFaction.faction == (Faction)factionIterator);
					}
					Player player2 = list3.Find(predicate2);
					if (player2 != null)
					{
						list.Add(player2);
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x000F2638 File Offset: 0x000F0838
		public bool CheckEncounter(bool show)
		{
			if (this.combatManager.GetBattlefields().Count > 0 || this.moveManager.IsPlayerMoving(this.PlayerCurrent))
			{
				return false;
			}
			if (this.GameFinished)
			{
				return false;
			}
			GameHex position = this.PlayerCurrent.character.position;
			if (position.hasEncounter && !position.encounterUsed)
			{
				if (show && this.EncounterGetEnabled != null)
				{
					this.EncounterGetEnabled();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x00043961 File Offset: 0x00041B61
		public bool CanPlayerDoEncounter(int number)
		{
			return this.LastEncounterCard != null && this.LastEncounterCard.CanExecute(number);
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00043979 File Offset: 0x00041B79
		public bool EncounterIsBeignResolved()
		{
			return this.LastEncounterCard != null;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000F26B4 File Offset: 0x000F08B4
		public bool CanGameBeSaved()
		{
			return !this.PlayerCurrent.ActionInProgress && this.LastEncounterCard == null && !this.FactoryCardsShown && !this.combatManager.IsPlayerInCombat() && !this.GameFinished && this.PlayerCurrent.IsHuman;
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000F2700 File Offset: 0x000F0900
		public bool CheckFactory(bool trigger)
		{
			GameHex position = this.PlayerCurrent.character.position;
			if (this.combatManager.GetBattlefields().Count > 0)
			{
				return false;
			}
			if (this.FactoryCardsShown)
			{
				return false;
			}
			if (position.hexType == HexType.factory && this.PlayerCurrent.matPlayer.GetPlayerMatSection(4) == null)
			{
				if (trigger)
				{
					this.FactoryCardsShown = true;
					if (this.FactoryGetEnabled != null)
					{
						this.FactoryGetEnabled();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000F277C File Offset: 0x000F097C
		public void CheckObjectiveCards()
		{
			if (this.ObjectiveCardGetHighlighted == null)
			{
				return;
			}
			if (!this.IsMyTurn())
			{
				return;
			}
			if (this.combatManager.GetBattlefields().Count > 0)
			{
				for (int i = 0; i < this.PlayerCurrent.objectiveCards.Count; i++)
				{
					if (this.PlayerCurrent.objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Open)
					{
						if (!this.PlayerCurrent.objectiveCards[i].CanDoActionAfterCompletingObjective())
						{
							this.ObjectiveCardGetHighlighted(i, false);
						}
						else if (this.PlayerCurrent.objectiveCards[i].CheckCondition() && !this.PlayerCurrent.ActionInProgress && this.actionManager.GetLastBonusAction() == null)
						{
							this.ObjectiveCardGetHighlighted(i, true);
						}
						else
						{
							this.ObjectiveCardGetHighlighted(i, false);
						}
					}
				}
				return;
			}
			for (int j = 0; j < this.PlayerCurrent.objectiveCards.Count; j++)
			{
				if (this.PlayerCurrent.objectiveCards[j].status == ObjectiveCard.ObjectiveStatus.Open)
				{
					if (this.PlayerCurrent.objectiveCards[j].CheckCondition() && (this.PlayerCurrent.objectiveCards[j].CanDoActionAfterCompletingObjective() || this.PlayerCurrent.currentMatSection != -1) && !this.PlayerCurrent.ActionInProgress && this.actionManager.GetLastBonusAction() == null)
					{
						this.ObjectiveCardGetHighlighted(j, true);
					}
					else
					{
						this.ObjectiveCardGetHighlighted(j, false);
					}
				}
			}
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x00043984 File Offset: 0x00041B84
		public bool CanPlayerDoActionAfterObjective(int index)
		{
			return this.PlayerCurrent.objectiveCards[index].CanDoActionAfterCompletingObjective();
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000F2910 File Offset: 0x000F0B10
		public void OnObjectiveCompleted(int index)
		{
			if (this.IsMultiplayer)
			{
				this.OnActionSent(new ObjectiveCompletedMessage(index, (int)this.PlayerCurrent.matFaction.faction));
			}
			this.PlayerCurrent.GainStar(StarType.Objective);
			this.PlayerCurrent.CompleteObjective(index);
			if (this.ObjectiveCardGetHighlighted != null)
			{
				this.ObjectiveCardGetHighlighted(index, false);
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x0004399C File Offset: 0x00041B9C
		public List<Faction> GetPlayersFactions()
		{
			return this.players.Select((Player p) => p.matFaction.faction).ToList<Faction>();
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000F2970 File Offset: 0x000F0B70
		public Player GetPlayerByFaction(Faction faction)
		{
			foreach (Player player in this.players)
			{
				if (player.matFaction.faction == faction)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000F29D4 File Offset: 0x000F0BD4
		public bool HasDLCContent()
		{
			if (this.players == null || this.players.Count == 0)
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				if (player.matFaction.faction == Faction.Albion || player.matFaction.faction == Faction.Togawa)
				{
					return true;
				}
				if (player.matPlayer.matType == PlayerMatType.Militant || player.matPlayer.matType == PlayerMatType.Innovative)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000439CD File Offset: 0x00041BCD
		public void BroadcastActionInfo(string actionInfo)
		{
			if (this.ObtainActionInfo != null)
			{
				this.ObtainActionInfo(actionInfo);
			}
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000F2A7C File Offset: 0x000F0C7C
		public void ChooseSection(int section)
		{
			if (this.PlayerCurrent.currentMatSection != -1 && this.PlayerCurrent.IsHuman)
			{
				return;
			}
			this.PlayerCurrent.currentMatSection = section;
			if (this.IsMultiplayer && ((this.PlayerOwner == null && !this.PlayerCurrent.IsHuman) || this.IsMyTurn()))
			{
				this.OnActionSent(new ChooseSectionMessage((int)this.PlayerCurrent.matFaction.faction, section));
			}
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000439E3 File Offset: 0x00041BE3
		public void OnShowPoints(List<PlayerEndGameStats> stats)
		{
			if (this.OnShowStats == null)
			{
				return;
			}
			this.OnShowStats(stats);
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000F2AF4 File Offset: 0x000F0CF4
		public void OnShowFactoryCards(List<string> cards)
		{
			this.factoryCards.Clear();
			foreach (string text in cards)
			{
				this.factoryCards.Add(new FactoryCard(int.Parse(text), this));
			}
			this.ShowedFactoryCards();
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x000439FA File Offset: 0x00041BFA
		public void OnShowEmptyCards(int amount)
		{
			if (this.ShowedEmptyCards != null)
			{
				this.ShowedEmptyCards(amount);
			}
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x00043A10 File Offset: 0x00041C10
		public void OnGainFactoryCards()
		{
			this.GainedFactoryCards();
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x00043A1D File Offset: 0x00041C1D
		public void FactoryCardChoose(int index)
		{
			this.CardChoosen(index);
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000F2B68 File Offset: 0x000F0D68
		public void AddFactoryCard(int cardIndex, int faction, int positionIndex)
		{
			this.FactoryCardsShown = false;
			FactoryCard factoryCard = new FactoryCard(cardIndex, this);
			Player playerByFaction = this.GetPlayerByFaction((Faction)faction);
			factoryCard.SetPlayer(playerByFaction);
			playerByFaction.matPlayer.AddFactoryCard(factoryCard);
			if (this.CardAdded != null)
			{
				this.CardAdded();
			}
			this.ReportLog(new FactoryLogInfo(this)
			{
				Type = LogInfoType.FactoryCardGain,
				PlayerAssigned = playerByFaction.matFaction.faction,
				IsEncounter = false,
				GainedFactoryCard = factoryCard,
				Character = playerByFaction.character
			}, false, ActionPositionType.Other);
			if (this.ShowedFactory != null && this.PlayerOwner != null && (this.PlayerOwner != this.PlayerCurrent || this.SpectatorMode))
			{
				this.ShowedFactory(cardIndex, positionIndex);
			}
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000F2C28 File Offset: 0x000F0E28
		public void AddFactoryCard(int index)
		{
			this.FactoryCardsShown = false;
			this.factoryCards[index].SetPlayer(this.PlayerCurrent);
			this.PlayerCurrent.matPlayer.AddFactoryCard(this.factoryCards[index]);
			this.ReportLog(new FactoryLogInfo(this)
			{
				Type = LogInfoType.FactoryCardGain,
				PlayerAssigned = this.PlayerCurrent.matFaction.faction,
				IsEncounter = false,
				GainedFactoryCard = this.factoryCards[index],
				Character = this.PlayerCurrent.character
			}, false, ActionPositionType.Other);
			this.factoryCards.RemoveAt(index);
			if (this.IsMultiplayer && this.PlayerOwner != null)
			{
				this.factoryCards.Clear();
			}
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000F2CF0 File Offset: 0x000F0EF0
		public void ReportLog(LogInfo logInfo, bool setActionPlacement = true, ActionPositionType placement = ActionPositionType.Other)
		{
			if (setActionPlacement)
			{
				if (logInfo.IsEncounter)
				{
					logInfo.ActionPlacement = ActionPositionType.Other;
				}
				else if (this.PlayerCurrent.currentMatSection != -1)
				{
					switch (this.PlayerCurrent.currentMatSection)
					{
					case 0:
						logInfo.ActionPlacement = ActionPositionType.Down;
						break;
					case 1:
						logInfo.ActionPlacement = ActionPositionType.Top;
						break;
					case 2:
						logInfo.ActionPlacement = ActionPositionType.Other;
						break;
					case 4:
						logInfo.ActionPlacement = ActionPositionType.Top;
						break;
					}
				}
			}
			if (placement != ActionPositionType.Other)
			{
				logInfo.ActionPlacement = placement;
			}
			if (logInfo.ActionPlacement == ActionPositionType.Top && !(logInfo is PayNonboardResourceLogInfo) && !(logInfo is PayResourceLogInfo) && this.GetPlayerByFaction(logInfo.PlayerAssigned).currentMatSection == 4)
			{
				LogInfo logInfo2 = new LogInfo(this);
				logInfo2.PlayerAssigned = logInfo.PlayerAssigned;
				logInfo2.ActionPlacement = ActionPositionType.Top;
				logInfo2.Type = LogInfoType.FactoryTopAction;
				logInfo2.AdditionalGain.Add(logInfo);
				this.actionLog.LogInfoReported(logInfo2);
				return;
			}
			if (logInfo.ActionPlacement != ActionPositionType.Down || logInfo.Type != LogInfoType.GainCombatCard)
			{
				this.actionLog.LogInfoReported(logInfo);
			}
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x00043A2B File Offset: 0x00041C2B
		public void OnEncounter()
		{
			if (this.EncounterButtonClicked != null)
			{
				this.EncounterButtonClicked();
			}
			this.PlayerCurrent.character.position.encounterUsed = true;
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x00043A56 File Offset: 0x00041C56
		public void ShowEncounterCard(int cardId, int faction)
		{
			this.LastEncounterCard = new EncounterCard(cardId, this);
			this.LastEncounterCard.SetPlayer(this.GetPlayerByFaction((Faction)faction));
			this.PlayerCurrent.EncountersVisited++;
			this.ShowEncounter();
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x00043A95 File Offset: 0x00041C95
		public void ChooseEncounterOption(int index)
		{
			this.OnActionSent(new EncounterOptionMessage(index));
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x00043AA3 File Offset: 0x00041CA3
		public void EncounterOption(int index)
		{
			if (this.ChooseOption != null)
			{
				this.ChooseOption(index);
			}
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x00043AB9 File Offset: 0x00041CB9
		public void MoveUnit(Unit unit, Dictionary<GameHex, GameHex> possibleMoves)
		{
			if (this.OnEnemyUnitMoved != null)
			{
				this.OnEnemyUnitMoved(unit, possibleMoves);
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x00043AD0 File Offset: 0x00041CD0
		public void EnemyMoveUnit(MoveEnemyActionInfo moveEnemyAction)
		{
			if (this.OnEnemyMoved != null)
			{
				this.OnEnemyMoved(moveEnemyAction);
			}
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x00043AE6 File Offset: 0x00041CE6
		public void EnemyLoadResources(LoadResourcesEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyLoadedResources != null)
			{
				this.OnEnemyLoadedResources(enemyAction);
			}
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x00043AFC File Offset: 0x00041CFC
		public void EnemyLoadWorker(LoadWorkerActionInfo enemyAction)
		{
			if (this.OnEnemyLoadedWorker != null)
			{
				this.OnEnemyLoadedWorker(enemyAction);
			}
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x00043B12 File Offset: 0x00041D12
		public void EnemyUnloadWorker(UnloadWorkerActionInfo enemyAction)
		{
			if (this.OnEnemyUnloadedWorker != null)
			{
				this.OnEnemyUnloadedWorker(enemyAction);
			}
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x00043B28 File Offset: 0x00041D28
		public void EnemyRetreatMoveUnit(MoveRetreatEnemyActionInfo moveEnemyAction)
		{
			if (this.OnEnemyRetreatMoved != null)
			{
				this.OnEnemyRetreatMoved(moveEnemyAction);
			}
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x00043B3E File Offset: 0x00041D3E
		public void EnemyProduceResources(ProduceEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyProduced != null)
			{
				this.OnEnemyProduced(enemyAction);
			}
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x00043B54 File Offset: 0x00041D54
		public void EnemyProduceWorker(GainWorkerEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyGainedWorker != null)
			{
				this.OnEnemyGainedWorker(enemyAction);
			}
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x00043B6A File Offset: 0x00041D6A
		public void EnemyProduceWorkersEnd(GainWorkersEndEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyGainedWorkerEnds != null)
			{
				this.OnEnemyGainedWorkerEnds(enemyAction);
			}
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x00043B80 File Offset: 0x00041D80
		public void EnemyUpgradeMat(UpgradeEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyUpgraded != null)
			{
				this.OnEnemyUpgraded(enemyAction);
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x00043B96 File Offset: 0x00041D96
		public void EnemyEnlist(EnlistEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyRecruited != null)
			{
				this.OnEnemyRecruited(enemyAction);
			}
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x00043BAC File Offset: 0x00041DAC
		public void EnemyEnlistBonus(EnlistBonusEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyRecruitBonusObtain != null)
			{
				this.OnEnemyRecruitBonusObtain(enemyAction);
			}
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x00043BC2 File Offset: 0x00041DC2
		public void EnemysEnlistBonusEnd()
		{
			if (this.OnEnemysBonusEnd != null)
			{
				this.OnEnemysBonusEnd();
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x00043BD7 File Offset: 0x00041DD7
		public void EnemyBuild(BuildEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyBuilded != null)
			{
				this.OnEnemyBuilded(enemyAction);
			}
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x00043BED File Offset: 0x00041DED
		public void EnemyDeploy(DeployEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyDeployed != null)
			{
				this.OnEnemyDeployed(enemyAction);
			}
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x00043C03 File Offset: 0x00041E03
		public void EnemyTrade(TradeEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyTraded != null)
			{
				this.OnEnemyTraded(enemyAction);
			}
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x00043C19 File Offset: 0x00041E19
		public void EnemyPayResouces(EnemyPayResourceFromHexInfo enemyAction)
		{
			if (this.OnEnemyPaidResources != null)
			{
				this.OnEnemyPaidResources(enemyAction);
			}
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x00043C2F File Offset: 0x00041E2F
		public void EnemyGainTopStat(GainTopStatsEnemyActionInfo enemyAction)
		{
			if (this.OnEnemyGainStats != null)
			{
				this.OnEnemyGainStats(enemyAction);
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000F2DE8 File Offset: 0x000F0FE8
		public void CompleteObjective(int index, Faction faction)
		{
			Player playerByFaction = this.GetPlayerByFaction(faction);
			Player player = playerByFaction;
			int objectivesDone = player.ObjectivesDone;
			player.ObjectivesDone = objectivesDone + 1;
			playerByFaction.GainStar(StarType.Objective);
			playerByFaction.objectiveCards[index].status = ObjectiveCard.ObjectiveStatus.Completed;
			if (playerByFaction.matFaction.factionPerk != AbilityPerk.Dominate)
			{
				for (int i = 0; i < playerByFaction.objectiveCards.Count; i++)
				{
					if (playerByFaction.objectiveCards[i].status == ObjectiveCard.ObjectiveStatus.Open)
					{
						playerByFaction.objectiveCards[i].status = ObjectiveCard.ObjectiveStatus.Disabled;
					}
				}
			}
			if (this.ShowObjective != null)
			{
				this.ShowObjective(playerByFaction.objectiveCards[index], playerByFaction);
			}
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x00043C45 File Offset: 0x00041E45
		public void OnUpdatePlayerStats()
		{
			if (this.UpdatePlayerStats != null)
			{
				this.UpdatePlayerStats();
			}
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x00043C5A File Offset: 0x00041E5A
		public void EndBotTurn()
		{
			if (this.BotTurnEnded != null)
			{
				this.BotTurnEnded();
			}
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x00043C6F File Offset: 0x00041E6F
		public void EndTurn()
		{
			if (this.TurnEnded != null)
			{
				this.TurnEnded();
			}
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x00043C84 File Offset: 0x00041E84
		public void FlushPayActionLogs()
		{
			this.actionLog.FlushAwaitingPayActions();
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x00043C91 File Offset: 0x00041E91
		public void EndEncounter(int x, int y)
		{
			if (this.OnEncounterEnded != null)
			{
				this.OnEncounterEnded(x, y);
			}
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x00043CA8 File Offset: 0x00041EA8
		public void OnActionFinished()
		{
			if (this.ActionHasFinished != null)
			{
				this.ActionHasFinished();
			}
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x00043CBD File Offset: 0x00041EBD
		public void OnGainOngoingRecruitBonus(GainAction action)
		{
			this.actionManager.PrepareInputForGainAction(action);
			if (action.ActionSelected)
			{
				this.actionManager.PrepareNextAction();
			}
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x00043CDE File Offset: 0x00041EDE
		public void OnActionSent(Message action)
		{
			if (this.ActionWasSent != null)
			{
				this.ActionWasSent(action);
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x00043CF4 File Offset: 0x00041EF4
		public void OnGainCombatCards(short amount, GainCombatCard.CombatCardGainType type)
		{
			if (this.GainedCombatCards != null)
			{
				this.GainedCombatCards(amount, type);
			}
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x00043D0B File Offset: 0x00041F0B
		public void SendCards(int amount, int faction, GainCombatCard.CombatCardGainType type)
		{
			if (this.SendCombatCards != null)
			{
				this.SendCombatCards(amount, faction, type);
			}
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x00043D23 File Offset: 0x00041F23
		public void OnInputEnabled(bool enabled)
		{
			if (this.InputWasEnabled != null)
			{
				if (this.GameFinished)
				{
					this.InputWasEnabled(false);
					return;
				}
				this.InputWasEnabled(enabled);
			}
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x00043D4E File Offset: 0x00041F4E
		public void OnPrepareNextAction()
		{
			this.actionManager.PrepareNextAction();
			if (this.TestingMode && this.IsMyTurn() && this.actionManager.GetLastBonusAction() == null)
			{
				this.PlayerCurrent.aiPlayer.ContinueLeaverAction();
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x00043D88 File Offset: 0x00041F88
		public bool IsMyTurn()
		{
			if (!this.IsMultiplayer)
			{
				return true;
			}
			if (this.SpectatorMode)
			{
				return false;
			}
			if (this.playerId == -1)
			{
				return !this.PlayerCurrent.IsHuman;
			}
			return this.playerCurrentId == this.playerId;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000F2E90 File Offset: 0x000F1090
		public void AddPower(Player player, int power, int cardsPower, List<CombatCard> cards)
		{
			PowerSelected powerSelected = default(PowerSelected);
			powerSelected.cardsPower = cardsPower;
			powerSelected.selectedCards = cards;
			powerSelected.selectedPower = power;
			this.combatManager.AddPlayerPowerInBattle(player, powerSelected);
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x00043DC4 File Offset: 0x00041FC4
		public void SynchronizeGame()
		{
			this.GameStarted = true;
			if (this.players.Count > 1 && this.GameSynchronized != null)
			{
				this.GameSynchronized();
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x00043DEE File Offset: 0x00041FEE
		public void StartGame()
		{
			this.GameStarted = true;
			if (this.GameLoaded != null)
			{
				this.GameLoaded();
			}
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x00043E0A File Offset: 0x0004200A
		public void SetOwnerIdFromFaction(Faction faction)
		{
			this.playerId = this.players.IndexOf(this.GetPlayerByFaction(faction));
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x00043E24 File Offset: 0x00042024
		public void ChangeActivePlayer(int faction)
		{
			if (this.ActivePlayerChanged != null)
			{
				this.ActivePlayerChanged(faction);
			}
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x00043E3A File Offset: 0x0004203A
		public void ChangeActivePlayer(int previousPlayer, int nextPlayer)
		{
			if (this.OnSetNewActivePlayer != null)
			{
				this.OnSetNewActivePlayer(previousPlayer, nextPlayer);
			}
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x00043E51 File Offset: 0x00042051
		public void ChangeSecondActivePlayer(int faction)
		{
			if (this.SecondActivePlayerChanged != null)
			{
				this.SecondActivePlayerChanged(faction);
			}
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x00043E67 File Offset: 0x00042067
		public void DisableActivePlayer(int faction)
		{
			if (this.ActivePlayerDisabled != null)
			{
				this.ActivePlayerDisabled(faction);
			}
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x00043E7D File Offset: 0x0004207D
		public void SetBattlefieldEffect()
		{
			if (this.BattlefieldChoosen != null)
			{
				this.BattlefieldChoosen();
			}
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x00043E92 File Offset: 0x00042092
		public void EnemyUsedCombatAbility(AbilityPerk ability)
		{
			if (this.CombatAbilityUsed != null)
			{
				this.CombatAbilityUsed(ability);
			}
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000F2ECC File Offset: 0x000F10CC
		public bool AnyPlayerHasSixStars()
		{
			using (List<Player>.Enumerator enumerator = this.players.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetNumberOfStars() == 6)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x00043EA8 File Offset: 0x000420A8
		public void CheckStars()
		{
			if (this.EndGamePerformed)
			{
				return;
			}
			if (this.AnyPlayerHasSixStars())
			{
				this.GameFinished = true;
				if (this.actionManager.GetLastBonusAction() == null && this.actionManager.GetLastSelectedGainAction() == null)
				{
					this.EndGame();
				}
			}
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000F2F28 File Offset: 0x000F1128
		private void EndGame()
		{
			this.EndGamePerformed = true;
			this.GameLength = DateTime.UtcNow - this.GameStartTime;
			if (this.GameHasEnded != null)
			{
				if (this.IsMultiplayer && this.MultiplayerGameEnded != null)
				{
					this.MultiplayerGameEnded();
					return;
				}
				this.GameHasEnded();
			}
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x00043EE2 File Offset: 0x000420E2
		public void EndTutorial()
		{
			this.GameFinished = true;
			this.EndGame();
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x00043EF1 File Offset: 0x000420F1
		public void GameFinishedState(bool gameFinished)
		{
			this.GameFinished = gameFinished;
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x00043EFA File Offset: 0x000420FA
		public List<PlayerEndGameStats> CalculateStats()
		{
			FindWinner findWinner = new FindWinner(this);
			this.StatsCalculated = true;
			return findWinner.CalculateStats();
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x00043F0E File Offset: 0x0004210E
		public void AfterLoad()
		{
			if (this.GetPlayersWithoutAICount() == 1 && this.IsHotSeat)
			{
				this.IsAIHotSeat = true;
			}
			this.gameBoard.UpdateHexOwnerships();
			this.combatManager.UpdateBattlefieldsOwners();
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000F2F84 File Offset: 0x000F1184
		public void ReadXml(XmlReader reader)
		{
			this.GameLoading = true;
			reader.MoveToContent();
			this.playerCurrentId = int.Parse(reader.GetAttribute("CurrentId"));
			if (reader.GetAttribute("TurnCount") != null)
			{
				this.TurnCount = int.Parse(reader.GetAttribute("TurnCount"));
			}
			else
			{
				this.TurnCount = 1000;
			}
			this.IsHotSeat = reader.GetAttribute("HotSeat") != null;
			this.StatsCalculated = reader.GetAttribute("StatsCalculated") != null;
			this.GameFinished = reader.GetAttribute("GameFinished") != null;
			this.EndGamePerformed = reader.GetAttribute("EndGamePerformed") != null;
			this.TestingMode = reader.GetAttribute("TestingMode") != null;
			this.IsAsynchronous = reader.GetAttribute("Asynchronous") != null;
			this.IsRanked = reader.GetAttribute("Ranked") != null;
			this.FactoryCardsShown = reader.GetAttribute("FactoryShown") != null;
			this.promoCardsOn = reader.GetAttribute("PromoCardsOn") != null;
			this.GameStarted = reader.GetAttribute("GameStarted") != null;
			this.InvadersFromAfar = reader.GetAttribute("IFA") != null;
			if (this.EndGamePerformed)
			{
				this.GameLength = TimeSpan.FromSeconds(double.Parse(reader.GetAttribute("GameLength"), CultureInfo.InvariantCulture));
			}
			if (reader.GetAttribute("StartTime") != null)
			{
				double num = double.Parse(reader.GetAttribute("StartTime").Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);
				this.GameStartTime = DateTime.FromOADate(num);
			}
			try
			{
				AnalyticsEventData.UpdateMatchSessionID(reader.GetAttribute("MatchSessionID"));
			}
			catch
			{
				AnalyticsEventData.UpdateMatchSessionID(AnalyticsEventData.MatchSessionID());
			}
			if (!this.IsMultiplayer && reader.GetAttribute("UndoType") != null)
			{
				this.UndoType = int.Parse(reader.GetAttribute("UndoType"));
			}
			switch (int.Parse(reader.GetAttribute("StrucBonus")))
			{
			case 1:
				this.StructureBonus = new TunnelNeighbourCard();
				break;
			case 2:
				this.StructureBonus = new LakeNeighbourCard();
				break;
			case 3:
				this.StructureBonus = new EncounterNeighbourCard();
				break;
			case 4:
				this.StructureBonus = new OccupationTunnelCard();
				break;
			case 5:
				this.StructureBonus = new InlineStructuresCard();
				break;
			case 6:
				this.StructureBonus = new OccupationResourceCard();
				break;
			}
			if (reader.GetAttribute("IsMulti") != null)
			{
				this.IsMultiplayer = true;
				this.playerId = int.Parse(reader.GetAttribute("OwnerId"));
				this.IsPrivate = reader.GetAttribute("Private") != null;
			}
			reader.ReadStartElement();
			if (reader.Name == "Gameboard")
			{
				((IXmlSerializable)this.gameBoard).ReadXml(reader);
			}
			if (reader.Name != "Player")
			{
				reader.ReadEndElement();
			}
			this.players.Clear();
			while (reader.Name == "Player")
			{
				Player player = new Player(this);
				((IXmlSerializable)player).ReadXml(reader);
				if (this.playerId == -1)
				{
					player.aiPlayer = new AiPlayer(player, this);
				}
				this.players.Add(player);
				reader.ReadStartElement();
				reader.ReadEndElement();
			}
			if (reader.Name == "LastEnc")
			{
				this.LastEncounterCard = new EncounterCard(0, this);
				((IXmlSerializable)this.LastEncounterCard).ReadXml(reader);
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
				else
				{
					reader.ReadStartElement();
				}
			}
			else
			{
				this.LastEncounterCard = null;
			}
			this.combatCards.ClearDeck();
			this.usedCombatCards.ClearDeck();
			this.encounterCards.ClearDeck();
			this.objectiveCards.ClearDeck();
			this.factoryCards.Clear();
			if (this.LastEncounterCard != null)
			{
				this.LastEncounterCard.SetPlayer(this.PlayerCurrent);
			}
			if (reader.Name == "CombatCards")
			{
				foreach (string text in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
				{
					if (text == "")
					{
						break;
					}
					this.combatCards.AddCard(new CombatCard(int.Parse(text)));
				}
			}
			if (reader.Name == "ObjectiveCards")
			{
				foreach (string text2 in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
				{
					if (text2 == "")
					{
						break;
					}
					this.objectiveCards.AddCard(new ObjectiveCard(this, int.Parse(text2)));
				}
			}
			if (reader.Name == "EncounterCards")
			{
				foreach (string text3 in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
				{
					if (text3 == "")
					{
						break;
					}
					this.encounterCards.AddCard(new EncounterCard(int.Parse(text3), this));
				}
			}
			if (reader.Name == "FactoryCards")
			{
				foreach (string text4 in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
				{
					if (text4 == "")
					{
						break;
					}
					this.factoryCards.Add(new FactoryCard(int.Parse(text4), this));
				}
			}
			if (reader.Name == "UsedCombatCards")
			{
				foreach (string text5 in reader.ReadElementContentAsString().Split(' ', StringSplitOptions.None))
				{
					if (text5 == "")
					{
						break;
					}
					this.usedCombatCards.AddCard(new CombatCard(int.Parse(text5)));
				}
			}
			else
			{
				this.FillUsedCombatCards();
			}
			((IXmlSerializable)this.combatManager).ReadXml(reader);
			if (reader.Name != "ActionManager")
			{
				reader.ReadStartElement();
			}
			((IXmlSerializable)this.actionManager).ReadXml(reader);
			if (reader.Name != "TokenManager")
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
				else
				{
					reader.ReadStartElement();
				}
			}
			if (reader.Name == "TokenManager")
			{
				((IXmlSerializable)this.tokenManager).ReadXml(reader);
				if (reader.Name != "MoveManager")
				{
					if (reader.NodeType == XmlNodeType.EndElement)
					{
						reader.ReadEndElement();
					}
					else
					{
						reader.ReadStartElement();
					}
				}
			}
			if (reader.Name == "MoveManager")
			{
				((IXmlSerializable)this.moveManager).ReadXml(reader);
			}
			if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "MoveManager")
			{
				reader.ReadEndElement();
			}
			if (reader.NodeType != XmlNodeType.EndElement)
			{
				if (reader.Name != "ActionLog")
				{
					reader.ReadStartElement();
				}
				if (reader.Name == "ActionLog")
				{
					((IXmlSerializable)this.actionLog).ReadXml(reader);
				}
			}
			this.GameLoading = false;
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000F367C File Offset: 0x000F187C
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("TurnCount", this.TurnCount.ToString());
			writer.WriteAttributeString("CurrentId", this.playerCurrentId.ToString());
			writer.WriteAttributeString("StrucBonus", this.StructureBonus.CardId.ToString());
			if (this.IsHotSeat)
			{
				writer.WriteAttributeString("HotSeat", "");
			}
			if (this.GameStarted)
			{
				writer.WriteAttributeString("GameStarted", "1");
			}
			if (this.GameFinished)
			{
				writer.WriteAttributeString("GameFinished", "1");
			}
			if (this.EndGamePerformed)
			{
				writer.WriteAttributeString("EndGamePerformed", "1");
			}
			if (this.StatsCalculated)
			{
				writer.WriteAttributeString("StatsCalculated", "1");
			}
			if (this.FactoryCardsShown)
			{
				writer.WriteAttributeString("FactoryShown", "1");
			}
			if (this.promoCardsOn)
			{
				writer.WriteAttributeString("PromoCardsOn", "1");
			}
			if (this.IsPrivate)
			{
				writer.WriteAttributeString("Private", "1");
			}
			if (this.InvadersFromAfar)
			{
				writer.WriteAttributeString("IFA", "1");
			}
			if (!this.SendingToPlayer)
			{
				writer.WriteAttributeString("StartTime", this.GameStartTime.ToOADate().ToString(CultureInfo.InvariantCulture));
			}
			if (this.EndGamePerformed)
			{
				writer.WriteAttributeString("GameLength", this.GameLength.TotalSeconds.ToString(CultureInfo.InvariantCulture));
			}
			if (!this.SendingToPlayer)
			{
				writer.WriteAttributeString("MatchSessionID", AnalyticsEventData.MatchSessionID());
			}
			if (!this.IsMultiplayer)
			{
				writer.WriteAttributeString("UndoType", this.UndoType.ToString());
			}
			if (this.IsMultiplayer)
			{
				if (this.TestingMode)
				{
					writer.WriteAttributeString("TestingMode", "1");
				}
				if (this.IsRanked)
				{
					writer.WriteAttributeString("Ranked", "1");
				}
				if (this.IsAsynchronous)
				{
					writer.WriteAttributeString("Asynchronous", "1");
				}
				writer.WriteAttributeString("IsMulti", "");
				if (this.playerId == -1 && this.SendingToPlayer)
				{
					writer.WriteAttributeString("OwnerId", this.SynchronizePlayerId.ToString());
				}
				else
				{
					writer.WriteAttributeString("OwnerId", this.playerId.ToString());
				}
			}
			writer.WriteStartElement("Gameboard");
			((IXmlSerializable)this.gameBoard).WriteXml(writer);
			writer.WriteEndElement();
			foreach (IXmlSerializable xmlSerializable in this.players)
			{
				writer.WriteStartElement("Player");
				xmlSerializable.WriteXml(writer);
				writer.WriteEndElement();
			}
			if (this.LastEncounterCard != null)
			{
				writer.WriteStartElement("LastEnc");
				((IXmlSerializable)this.LastEncounterCard).WriteXml(writer);
				writer.WriteEndElement();
			}
			if (!this.IsMultiplayer || this.playerId != -1 || !this.SendingToPlayer)
			{
				writer.WriteStartElement("CombatCards");
				((IXmlSerializable)this.combatCards).WriteXml(writer);
				writer.WriteEndElement();
				writer.WriteStartElement("EncounterCards");
				((IXmlSerializable)this.encounterCards).WriteXml(writer);
				writer.WriteEndElement();
				writer.WriteStartElement("FactoryCards");
				foreach (FactoryCard factoryCard in this.factoryCards)
				{
					((IXmlSerializable)factoryCard).WriteXml(writer);
				}
				writer.WriteEndElement();
				writer.WriteStartElement("UsedCombatCards");
				((IXmlSerializable)this.usedCombatCards).WriteXml(writer);
				writer.WriteEndElement();
			}
			else
			{
				writer.WriteStartElement("CombatCards");
				writer.WriteEndElement();
				writer.WriteStartElement("EncounterCards");
				writer.WriteEndElement();
				writer.WriteStartElement("FactoryCards");
				writer.WriteEndElement();
			}
			writer.WriteStartElement("CombatManager");
			((IXmlSerializable)this.combatManager).WriteXml(writer);
			writer.WriteEndElement();
			writer.WriteStartElement("ActionManager");
			((IXmlSerializable)this.actionManager).WriteXml(writer);
			writer.WriteEndElement();
			writer.WriteStartElement("TokenManager");
			((IXmlSerializable)this.tokenManager).WriteXml(writer);
			writer.WriteEndElement();
			writer.WriteStartElement("MoveManager");
			((IXmlSerializable)this.moveManager).WriteXml(writer);
			writer.WriteEndElement();
			writer.WriteStartElement("ActionLog");
			((IXmlSerializable)this.actionLog).WriteXml(writer);
			writer.WriteEndElement();
		}

		// Token: 0x04001DBE RID: 7614
		public const int NUMBER_OF_COMBAT_CARDS = 42;

		// Token: 0x04001DBF RID: 7615
		public const int NUMBER_OF_PROMO_OBJECTIVE_CARDS = 27;

		// Token: 0x04001DC0 RID: 7616
		public const int NUMBER_OF_OBJECTIVE_CARDS = 23;

		// Token: 0x04001DC1 RID: 7617
		public const int NUMBER_OF_PROMO_FACTORY_CARDS = 18;

		// Token: 0x04001DC2 RID: 7618
		public const int NUMBER_OF_FACTORY_CARDS = 12;

		// Token: 0x04001DC3 RID: 7619
		public const int NUMBER_OF_PROMO_ENCOUNTER_CARDS = 36;

		// Token: 0x04001DC4 RID: 7620
		public const int NUMBER_OF_ENCOUNTER_CARDS = 28;

		// Token: 0x04001DC5 RID: 7621
		private const int DEFAULT_PLAYER_CLOCK_TIME = 1800;

		// Token: 0x04001DC6 RID: 7622
		public int missionId = -1;

		// Token: 0x04001DC7 RID: 7623
		public int challengeId = -1;

		// Token: 0x04001DC8 RID: 7624
		public GameBoard gameBoard;

		// Token: 0x04001DC9 RID: 7625
		public CombatManager combatManager;

		// Token: 0x04001DCA RID: 7626
		public ActionManager actionManager;

		// Token: 0x04001DCB RID: 7627
		public ActionLog actionLog;

		// Token: 0x04001DCC RID: 7628
		public MoveManager moveManager;

		// Token: 0x04001DCD RID: 7629
		public TokenManager tokenManager;

		// Token: 0x04001DCE RID: 7630
		public Dictionary<Faction, FactionBasicInfo> factionBasicInfo;

		// Token: 0x04001DCF RID: 7631
		public ChallengesLogicStarter challengesLogicStarter = new ChallengesLogicStarter();

		// Token: 0x04001DD0 RID: 7632
		public List<Player> players = new List<Player>();

		// Token: 0x04001DD1 RID: 7633
		private int playerCurrentId;

		// Token: 0x04001DD2 RID: 7634
		private int playerId = -1;

		// Token: 0x04001DD3 RID: 7635
		private CardDeck<CombatCard> combatCards;

		// Token: 0x04001DD4 RID: 7636
		private CardDeck<CombatCard> usedCombatCards;

		// Token: 0x04001DD5 RID: 7637
		public CardDeck<ObjectiveCard> objectiveCards;

		// Token: 0x04001DD6 RID: 7638
		public CardDeck<EncounterCard> encounterCards;

		// Token: 0x04001DD7 RID: 7639
		private List<FactoryCard> factoryCards = new List<FactoryCard>();

		// Token: 0x04001DD8 RID: 7640
		public bool promoCardsOn;

		// Token: 0x04001DD9 RID: 7641
		private string serializedPromoCards = "";

		// Token: 0x04001DDA RID: 7642
		private Dictionary<ResourceType, int> temporaryResources = new Dictionary<ResourceType, int>();

		// Token: 0x04001DDB RID: 7643
		private int temporaryCombatCards;

		// Token: 0x04001DDC RID: 7644
		private bool temporaryResourcesEnabled;

		// Token: 0x04001DDD RID: 7645
		public List<MatFaction> availableFactions = new List<MatFaction>();

		// Token: 0x04001DDE RID: 7646
		public List<MatPlayer> availableActionMats = new List<MatPlayer>();

		// Token: 0x04001DDF RID: 7647
		private bool EndGamePerformed;

		// Token: 0x04001DE0 RID: 7648
		private int amountOfCombatCardsLeft;

		// Token: 0x04001DE1 RID: 7649
		private static int seed = Environment.TickCount;

		// Token: 0x04001DE2 RID: 7650
		private static readonly Random _random = new Random(Interlocked.Increment(ref GameManager.seed));

		// Token: 0x04001DF4 RID: 7668
		public bool showEnemyActions = true;

		// Token: 0x04001DF8 RID: 7672
		public DateTime GameStartTime;

		// Token: 0x04001DF9 RID: 7673
		public TimeSpan GameLength;

		// Token: 0x02000521 RID: 1313
		// (Invoke) Token: 0x06002B16 RID: 11030
		public delegate void UpdateCurrentPlayerStats();

		// Token: 0x02000522 RID: 1314
		// (Invoke) Token: 0x06002B1A RID: 11034
		public delegate void HighlightObjectiveCard(int index, bool focus);

		// Token: 0x02000523 RID: 1315
		// (Invoke) Token: 0x06002B1E RID: 11038
		public delegate void EnableEncounter();

		// Token: 0x02000524 RID: 1316
		// (Invoke) Token: 0x06002B22 RID: 11042
		public delegate void EnableFactory();

		// Token: 0x02000525 RID: 1317
		// (Invoke) Token: 0x06002B26 RID: 11046
		public delegate void GameEnded();

		// Token: 0x02000526 RID: 1318
		// (Invoke) Token: 0x06002B2A RID: 11050
		public delegate void BattleCardsStatus(int numberOfCards);

		// Token: 0x02000527 RID: 1319
		// (Invoke) Token: 0x06002B2E RID: 11054
		public delegate void ActionExecuted(string actionInfo);

		// Token: 0x02000528 RID: 1320
		// (Invoke) Token: 0x06002B32 RID: 11058
		public delegate void ActionFinished();

		// Token: 0x02000529 RID: 1321
		// (Invoke) Token: 0x06002B36 RID: 11062
		public delegate void MessageSend(Message message);

		// Token: 0x0200052A RID: 1322
		// (Invoke) Token: 0x06002B3A RID: 11066
		public delegate void GainCombatCards(short amount, GainCombatCard.CombatCardGainType type);

		// Token: 0x0200052B RID: 1323
		// (Invoke) Token: 0x06002B3E RID: 11070
		public delegate void EnableInput(bool enable);

		// Token: 0x0200052C RID: 1324
		// (Invoke) Token: 0x06002B42 RID: 11074
		public delegate void GainFactoryCards();

		// Token: 0x0200052D RID: 1325
		// (Invoke) Token: 0x06002B46 RID: 11078
		public delegate void ShowFactoryCards();

		// Token: 0x0200052E RID: 1326
		// (Invoke) Token: 0x06002B4A RID: 11082
		public delegate void ShowEmptyCards(int amount);

		// Token: 0x0200052F RID: 1327
		// (Invoke) Token: 0x06002B4E RID: 11086
		public delegate void OnChooseCard(int index);

		// Token: 0x02000530 RID: 1328
		// (Invoke) Token: 0x06002B52 RID: 11090
		public delegate void ShowChoosenFactory(int cardIndex, int positionIndex);

		// Token: 0x02000531 RID: 1329
		// (Invoke) Token: 0x06002B56 RID: 11094
		public delegate void AfterCardAdded();

		// Token: 0x02000532 RID: 1330
		// (Invoke) Token: 0x06002B5A RID: 11098
		public delegate void OnEncounterButton();

		// Token: 0x02000533 RID: 1331
		// (Invoke) Token: 0x06002B5E RID: 11102
		public delegate void OnShowEncounterCard();

		// Token: 0x02000534 RID: 1332
		// (Invoke) Token: 0x06002B62 RID: 11106
		public delegate void OnEncounterOption(int index);

		// Token: 0x02000535 RID: 1333
		// (Invoke) Token: 0x06002B66 RID: 11110
		public delegate void OnEncounterClosed();

		// Token: 0x02000536 RID: 1334
		// (Invoke) Token: 0x06002B6A RID: 11114
		public delegate void OnShowObjective(ObjectiveCard objectiveCard, Player player);

		// Token: 0x02000537 RID: 1335
		// (Invoke) Token: 0x06002B6E RID: 11118
		public delegate void ShowStats(List<PlayerEndGameStats> stats);

		// Token: 0x02000538 RID: 1336
		// (Invoke) Token: 0x06002B72 RID: 11122
		public delegate void OnMoveUnit(Unit unit, Dictionary<GameHex, GameHex> possibleMoves);

		// Token: 0x02000539 RID: 1337
		// (Invoke) Token: 0x06002B76 RID: 11126
		public delegate void OnEnemyMove(MoveEnemyActionInfo moveEnemyAction);

		// Token: 0x0200053A RID: 1338
		// (Invoke) Token: 0x06002B7A RID: 11130
		public delegate void OnEnemyLoadResources(LoadResourcesEnemyActionInfo moveEnemyAction);

		// Token: 0x0200053B RID: 1339
		// (Invoke) Token: 0x06002B7E RID: 11134
		public delegate void OnEnemyLoadWorker(LoadWorkerActionInfo moveEnemyAction);

		// Token: 0x0200053C RID: 1340
		// (Invoke) Token: 0x06002B82 RID: 11138
		public delegate void OnEnemyUnloadWorker(UnloadWorkerActionInfo moveEnemyAction);

		// Token: 0x0200053D RID: 1341
		// (Invoke) Token: 0x06002B86 RID: 11142
		public delegate void OnEnemyRetreatMove(MoveRetreatEnemyActionInfo moveEnemyAction);

		// Token: 0x0200053E RID: 1342
		// (Invoke) Token: 0x06002B8A RID: 11146
		public delegate void OnEnemyProduce(ProduceEnemyActionInfo produceEnemyAction);

		// Token: 0x0200053F RID: 1343
		// (Invoke) Token: 0x06002B8E RID: 11150
		public delegate void OnEnemyGainWorker(GainWorkerEnemyActionInfo gainWorkerEnemyAction);

		// Token: 0x02000540 RID: 1344
		// (Invoke) Token: 0x06002B92 RID: 11154
		public delegate void OnEnemyGainWorkerEnd(GainWorkersEndEnemyActionInfo gainWorkersEndEnemyAction);

		// Token: 0x02000541 RID: 1345
		// (Invoke) Token: 0x06002B96 RID: 11158
		public delegate void OnEnemyUpgrade(UpgradeEnemyActionInfo upgradeEnemyAction);

		// Token: 0x02000542 RID: 1346
		// (Invoke) Token: 0x06002B9A RID: 11162
		public delegate void OnEnlistRecruit(EnlistEnemyActionInfo enlistEnemyAction);

		// Token: 0x02000543 RID: 1347
		// (Invoke) Token: 0x06002B9E RID: 11166
		public delegate void OnEnemyEnlistBonus(EnlistBonusEnemyActionInfo enlistBonusEnemyAction);

		// Token: 0x02000544 RID: 1348
		// (Invoke) Token: 0x06002BA2 RID: 11170
		public delegate void OnEnemysEnlistBonusEnd();

		// Token: 0x02000545 RID: 1349
		// (Invoke) Token: 0x06002BA6 RID: 11174
		public delegate void OnEnemyBuild(BuildEnemyActionInfo buildEnemyAction);

		// Token: 0x02000546 RID: 1350
		// (Invoke) Token: 0x06002BAA RID: 11178
		public delegate void OnEnemyDeploy(DeployEnemyActionInfo deployEnemyAction);

		// Token: 0x02000547 RID: 1351
		// (Invoke) Token: 0x06002BAE RID: 11182
		public delegate void OnEnemyTrade(TradeEnemyActionInfo tradeEnemyAction);

		// Token: 0x02000548 RID: 1352
		// (Invoke) Token: 0x06002BB2 RID: 11186
		public delegate void OnEnemyPayResources(EnemyPayResourceFromHexInfo payResourceEnemyAction);

		// Token: 0x02000549 RID: 1353
		// (Invoke) Token: 0x06002BB6 RID: 11190
		public delegate void OnEnemyGainStatsAction(GainTopStatsEnemyActionInfo statsGainEnemyAction);

		// Token: 0x0200054A RID: 1354
		// (Invoke) Token: 0x06002BBA RID: 11194
		public delegate void OnGameEnded();

		// Token: 0x0200054B RID: 1355
		// (Invoke) Token: 0x06002BBE RID: 11198
		public delegate void OnGameSynchronized();

		// Token: 0x0200054C RID: 1356
		// (Invoke) Token: 0x06002BC2 RID: 11202
		public delegate void OnGameLoaded();

		// Token: 0x0200054D RID: 1357
		// (Invoke) Token: 0x06002BC6 RID: 11206
		public delegate void OnBattlefieldChose();

		// Token: 0x0200054E RID: 1358
		// (Invoke) Token: 0x06002BCA RID: 11210
		public delegate void OnNextTurn();

		// Token: 0x0200054F RID: 1359
		// (Invoke) Token: 0x06002BCE RID: 11214
		public delegate void OnChangeSecondActivePlayer(int faction);

		// Token: 0x02000550 RID: 1360
		// (Invoke) Token: 0x06002BD2 RID: 11218
		public delegate void OnDisableActivePlayer(int faction);

		// Token: 0x02000551 RID: 1361
		// (Invoke) Token: 0x06002BD6 RID: 11222
		public delegate void OnBotGainCombatCards(int amount, int faction, GainCombatCard.CombatCardGainType type);

		// Token: 0x02000552 RID: 1362
		// (Invoke) Token: 0x06002BDA RID: 11226
		public delegate void OnCombatAbilityUsed(AbilityPerk ability);
	}
}
