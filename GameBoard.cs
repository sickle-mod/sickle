using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005CF RID: 1487
	public class GameBoard : IXmlSerializable
	{
		// Token: 0x06002F46 RID: 12102 RVA: 0x0012484C File Offset: 0x00122A4C
		public GameBoard(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x001248CC File Offset: 0x00122ACC
		public void InitMap()
		{
			this.bases.Clear();
			this.tunnels.Clear();
			this.lakes.Clear();
			this.villages.Clear();
			this.mountains.Clear();
			this.farms.Clear();
			this.tundras.Clear();
			this.forest.Clear();
			for (int i = 0; i < 9; i++)
			{
				string[] array = GameBoard.hexMapLayout[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				int j = 0;
				while (j < 8)
				{
					GameHex gameHex = new GameHex(this.gameManager);
					int num = 0;
					if (int.TryParse(array[j][0].ToString(), out num))
					{
						gameHex.riverMask = num;
						array[j] = array[j].Remove(0, 1);
					}
					char c = array[j][0];
					if (c <= 'F')
					{
						if (c != 'B')
						{
							if (c != 'F')
							{
								goto IL_02AB;
							}
							gameHex.hexType = HexType.farm;
							this.farms.Add(gameHex);
						}
						else
						{
							gameHex.hexType = HexType.capital;
							char c2 = array[j][1];
							switch (c2)
							{
							case 'b':
								gameHex.factionBase = Faction.Albion;
								this.bases.Add(Faction.Albion, gameHex);
								break;
							case 'c':
								gameHex.factionBase = Faction.Crimea;
								this.bases.Add(Faction.Crimea, gameHex);
								break;
							case 'd':
								gameHex.factionBase = Faction.Togawa;
								this.bases.Add(Faction.Togawa, gameHex);
								break;
							default:
								switch (c2)
								{
								case 'n':
									gameHex.factionBase = Faction.Nordic;
									this.bases.Add(Faction.Nordic, gameHex);
									break;
								case 'p':
									gameHex.factionBase = Faction.Polania;
									this.bases.Add(Faction.Polania, gameHex);
									break;
								case 'r':
									gameHex.factionBase = Faction.Rusviet;
									this.bases.Add(Faction.Rusviet, gameHex);
									break;
								case 's':
									gameHex.factionBase = Faction.Saxony;
									this.bases.Add(Faction.Saxony, gameHex);
									break;
								}
								break;
							}
						}
					}
					else
					{
						switch (c)
						{
						case 'L':
							gameHex.hexType = HexType.lake;
							this.lakes.Add(gameHex);
							break;
						case 'M':
							gameHex.hexType = HexType.mountain;
							this.mountains.Add(gameHex);
							break;
						case 'N':
							goto IL_02AB;
						case 'O':
							gameHex.hexType = HexType.tundra;
							this.tundras.Add(gameHex);
							break;
						default:
							switch (c)
							{
							case 'V':
								gameHex.hexType = HexType.village;
								this.villages.Add(gameHex);
								break;
							case 'W':
								gameHex.hexType = HexType.forest;
								this.forest.Add(gameHex);
								break;
							case 'X':
								gameHex.hexType = HexType.factory;
								this.factory = gameHex;
								break;
							default:
								goto IL_02AB;
							}
							break;
						}
					}
					IL_02B3:
					if (array[j].Length > 1)
					{
						if (array[j][1] == 't')
						{
							gameHex.hasTunnel = true;
							this.tunnels.Add(gameHex);
						}
						else if (array[j][1] == 'e')
						{
							gameHex.hasEncounter = true;
						}
					}
					if (int.TryParse(array[j][array[j].Length - 1].ToString(), out num))
					{
						gameHex.riverMask |= num << 3;
					}
					gameHex.gameBoard = this;
					gameHex.posX = j;
					gameHex.posY = i;
					this.hexMap[j, i] = gameHex;
					j++;
					continue;
					IL_02AB:
					gameHex.hexType = HexType.forbidden;
					goto IL_02B3;
				}
			}
		}


		// Token: 0x06002F48 RID: 12104 RVA: 0x00124C3C File Offset: 0x00122E3C
		public void RestoreEncounters()
		{
			for (int i = 0; i < 9; i++)
			{
				string[] array = GameBoard.hexMapLayout[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < 8; j++)
				{
					GameHex gameHex = this.hexMap[j, i];
					int num = 0;
					if (int.TryParse(array[j][0].ToString(), out num))
					{
						array[j] = array[j].Remove(0, 1);
					}
					if (array[j].Length > 1 && array[j][1] == 'e')
					{
						gameHex.hasEncounter = true;
					}
				}
			}
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000455A9 File Offset: 0x000437A9
		public Dictionary<GameHex, GameHex> MoveRange(Unit unit, int moveCounter, out Dictionary<GameHex, int> distance)
		{
			if (unit.position == null)
			{
				distance = new Dictionary<GameHex, int>();
				return new Dictionary<GameHex, GameHex>();
			}
			return this.MoveRange(unit, this.hexMap[unit.position.posX, unit.position.posY], moveCounter, out distance);
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x00124CDC File Offset: 0x00122EDC
		public Dictionary<GameHex, GameHex> MoveRange(Unit unit, GameHex startPosition, int moveCounter, out Dictionary<GameHex, int> distance)
		{
			Dictionary<GameHex, GameHex> dictionary = new Dictionary<GameHex, GameHex>();
			distance = new Dictionary<GameHex, int>();
			Queue<GameHex> queue = new Queue<GameHex>();
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			dictionary.Add(startPosition, null);
			distance.Add(startPosition, 0);
			queue.Enqueue(startPosition);
			while (queue.Count > 0)
			{
				GameHex gameHex = queue.Dequeue();
				if (distance[gameHex] < moveCounter)
				{
					foreach (GameHex gameHex2 in gameHex.GetFieldsAccessible(unit, hashSet.Contains(gameHex)))
					{
						if (!distance.ContainsKey(gameHex2))
						{
							queue.Enqueue(gameHex2);
							distance.Add(gameHex2, distance[gameHex] + 1);
							dictionary.Add(gameHex2, gameHex);
							if (gameHex.IsRiverBetween(gameHex2) && !gameHex.CanMoveThroughTunnels(gameHex2, unit))
							{
								hashSet.Add(gameHex2);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x00124DE0 File Offset: 0x00122FE0
		public Dictionary<GameHex, GameHex> MoveRange(Unit unit, int moveCounter)
		{
			Dictionary<GameHex, int> dictionary;
			return this.MoveRange(unit, moveCounter, out dictionary);
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x00124DF8 File Offset: 0x00122FF8
		public void UpdateHexOwnerships()
		{
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.hexMap[j, i].UpdateOwnership();
				}
			}
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x00124E30 File Offset: 0x00123030
		public bool validateOwnership(List<GameHex> hexList, Player owner)
		{
			foreach (GameHex gameHex in hexList)
			{
				if (gameHex.Owner != null && gameHex.Owner != owner)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x00124E90 File Offset: 0x00123090
		public void ReadXml(XmlReader reader)
		{
			if (this.hexMap[0, 0] == null)
			{
				this.InitMap();
			}
			else
			{
				this.RestoreEncounters();
				GameHex[,] array = this.hexMap;
				int upperBound = array.GetUpperBound(0);
				int upperBound2 = array.GetUpperBound(1);
				for (int i = array.GetLowerBound(0); i <= upperBound; i++)
				{
					for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
					{
						array[i, j].Clear();
					}
				}
			}
			reader.ReadStartElement();
			while (reader.Name == "Hex")
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				((IXmlSerializable)this.hexMap[num, num2]).ReadXml(reader);
				reader.ReadStartElement();
			}
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x00124F60 File Offset: 0x00123160
		public void WriteXml(XmlWriter writer)
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (this.hexMap[i, j].NeedSerialization())
					{
						writer.WriteStartElement("Hex");
						((IXmlSerializable)this.hexMap[i, j]).WriteXml(writer);
						writer.WriteEndElement();
					}
				}
			}
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x0002F60E File Offset: 0x0002D80E
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x04001FA6 RID: 8102
		public const int BOARD_SIZE_X = 8;

		// Token: 0x04001FA7 RID: 8103
		public const int BOARD_SIZE_Y = 9;

		// Token: 0x04001FA8 RID: 8104
		public GameHex[,] hexMap = new GameHex[8, 9];

		// Token: 0x04001FA9 RID: 8105
		public GameHex factory;

		// Token: 0x04001FAA RID: 8106
		public List<GameHex> tunnels = new List<GameHex>();

		// Token: 0x04001FAB RID: 8107
		public List<GameHex> lakes = new List<GameHex>();

		// Token: 0x04001FAC RID: 8108
		public List<GameHex> villages = new List<GameHex>();

		// Token: 0x04001FAD RID: 8109
		public List<GameHex> mountains = new List<GameHex>();

		// Token: 0x04001FAE RID: 8110
		public List<GameHex> farms = new List<GameHex>();

		// Token: 0x04001FAF RID: 8111
		public List<GameHex> tundras = new List<GameHex>();

		// Token: 0x04001FB0 RID: 8112
		public List<GameHex> forest = new List<GameHex>();

		// Token: 0x04001FB1 RID: 8113
		public Dictionary<Faction, GameHex> bases = new Dictionary<Faction, GameHex>();

		// Token: 0x04001FB2 RID: 8114
		private static readonly string[] hexMapLayout = new string[] { "   .    Bb   .    .    Bn   .    .    .", " .    M    F    Ve6 6W    O6  2V    . ", "   L   4Oe   L   1Ot3 2Me6 3F4  4Fe4  .", " Bp   W3  6Mt   W    L   1Wt6 3V1  1Br", "  4F4  4Ve5  L    X    M2  7Oe   M    .", " We5 5W5  5Ft   O    L    Vt1  L    . ", "  1M1  1Ve3 2Ve4 4Ot4 4W    Me   O    .", " Bs   O    L   1F1  1Me3 2V    F    Bd", "   .    .    Bc   V    .    .    .    ." };

		// Token: 0x04001FB3 RID: 8115
		public const int XBoardCenter = 3;

		// Token: 0x04001FB4 RID: 8116
		public const int YBoardCenter = 4;

		// Token: 0x04001FB5 RID: 8117
		private GameManager gameManager;
	}
}
