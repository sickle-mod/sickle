using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005DC RID: 1500
	public class MatFaction : IXmlSerializable
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x00045735 File Offset: 0x00043935
		// (set) Token: 0x06002F64 RID: 12132 RVA: 0x0004573D File Offset: 0x0004393D
		public Player MatOwner
		{
			get
			{
				return this.matOwner;
			}
			set
			{
				this.matOwner = value;
				if (this.FactionTokens != null)
				{
					this.FactionTokens.SetOwner(this.matOwner);
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06002F65 RID: 12133 RVA: 0x0004575F File Offset: 0x0004395F
		// (set) Token: 0x06002F66 RID: 12134 RVA: 0x00045767 File Offset: 0x00043967
		public TokenSupply FactionTokens { get; private set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06002F67 RID: 12135 RVA: 0x00045770 File Offset: 0x00043970
		// (set) Token: 0x06002F68 RID: 12136 RVA: 0x00045778 File Offset: 0x00043978
		public Dictionary<GainType, bool> OneTimeBonuses { get; private set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06002F69 RID: 12137 RVA: 0x00045781 File Offset: 0x00043981
		public int StartingPower
		{
			get
			{
				return this.startingPower;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06002F6A RID: 12138 RVA: 0x00045789 File Offset: 0x00043989
		public int StartingCombatCards
		{
			get
			{
				return this.startingCombatCards;
			}
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x00045791 File Offset: 0x00043991
		public MatFaction(GameManager gameManager, Faction faction, int startingPower, int startingCombatCards, AbilityPerk factionPerk, params AbilityPerk[] abilities)
		{
			this.gameManager = gameManager;
			this.SetStartingParameters(faction, startingPower, startingCombatCards, factionPerk, abilities);
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000457D0 File Offset: 0x000439D0
		public MatFaction(GameManager gameManager, Faction faction)
		{
			this.gameManager = gameManager;
			this.SetFaction(faction);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x00120C14 File Offset: 0x0011EE14
		private void SetFaction(Faction faction)
		{
			switch (faction)
			{
			case Faction.Polania:
				this.SetStartingParameters(Faction.Polania, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Meander, new AbilityPerk[]
				{
					(AbilityPerk)17,
					AbilityPerk.Submerge,
					AbilityPerk.Camaraderie,
					AbilityPerk.Speed
				});
				return;
			case Faction.Albion:
				this.SetStartingParameters(Faction.Albion, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Exalt, new AbilityPerk[]
				{
					AbilityPerk.Burrow,
					AbilityPerk.Sword,
					AbilityPerk.Shield,
					AbilityPerk.Rally
				});
				return;
			case Faction.Nordic:
				this.SetStartingParameters(Faction.Nordic, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Swim, new AbilityPerk[]
				{
					(AbilityPerk)3,
					AbilityPerk.Seaworthy,
					AbilityPerk.Artillery,
					AbilityPerk.Speed
				});
				return;
			case Faction.Rusviet:
				this.SetStartingParameters(Faction.Rusviet, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Relenteless, new AbilityPerk[]
				{
					(AbilityPerk)20,
					AbilityPerk.Township,
					AbilityPerk.PeoplesArmy,
					AbilityPerk.Speed
				});
				return;
			case Faction.Togawa:
				this.SetStartingParameters(Faction.Togawa, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Maifuku, new AbilityPerk[]
				{
					AbilityPerk.Toka,
					AbilityPerk.Suiton,
					AbilityPerk.Ronin,
					AbilityPerk.Shinobi
				});
				return;
			case Faction.Crimea:
				this.SetStartingParameters(Faction.Crimea, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Coercion, new AbilityPerk[]
				{
					(AbilityPerk)12,
					AbilityPerk.Wayfare,
					AbilityPerk.Scout,
					AbilityPerk.Speed
				});
				return;
			case Faction.Saxony:
				this.SetStartingParameters(Faction.Saxony, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Dominate, new AbilityPerk[]
				{
					(AbilityPerk)3,
					AbilityPerk.Underpass,
					AbilityPerk.Disarm,
					AbilityPerk.Speed
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x00120D54 File Offset: 0x0011EF54
		private void SetStartingParameters(Faction faction, int startingPower, int startingCombatCards, AbilityPerk factionPerk, params AbilityPerk[] perks)
		{
			this.faction = faction;
			this.startingPower = startingPower;
			this.startingCombatCards = startingCombatCards;
			this.OneTimeBonuses = new Dictionary<GainType, bool>();
			this.OneTimeBonuses.Add(GainType.Power, false);
			this.OneTimeBonuses.Add(GainType.Coin, false);
			this.OneTimeBonuses.Add(GainType.Popularity, false);
			this.OneTimeBonuses.Add(GainType.CombatCard, false);
			this.factionPerk = factionPerk;
			foreach (AbilityPerk abilityPerk in perks)
			{
				this.abilities.Add(abilityPerk);
			}
			this.FactionTokens = TokenSupplyFactory.GetTokenSupply(faction, this.gameManager);
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x00045808 File Offset: 0x00043A08
		public bool OneTimeBonusUsed(GainType bonusType)
		{
			return this.OneTimeBonuses[bonusType];
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x00120DF0 File Offset: 0x0011EFF0
		public GainAction GetOneTimeBonus(GainType bonusType)
		{
			short num = 2;
			GainAction gainAction = null;
			switch (bonusType)
			{
			case GainType.Coin:
				gainAction = new GainCoin(this.gameManager, num, 0, false, true, false);
				gainAction.SetPlayer(this.MatOwner);
				gainAction.LogInfoType = LogInfoType.Enlist;
				break;
			case GainType.Popularity:
				gainAction = new GainPopularity(this.gameManager, num, 0, false, true, false);
				gainAction.SetPlayer(this.MatOwner);
				gainAction.LogInfoType = LogInfoType.Enlist;
				break;
			case GainType.Power:
				gainAction = new GainPower(this.gameManager, num, 0, false, true, false);
				gainAction.SetPlayer(this.MatOwner);
				gainAction.LogInfoType = LogInfoType.Enlist;
				break;
			case GainType.CombatCard:
				gainAction = new GainCombatCard(this.gameManager, num, 0, false, true, false);
				gainAction.SetPlayer(this.MatOwner);
				gainAction.LogInfoType = LogInfoType.Enlist;
				break;
			}
			return gainAction;
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x00120EB8 File Offset: 0x0011F0B8
		public void CheckRecruitStar()
		{
			foreach (KeyValuePair<GainType, bool> keyValuePair in this.OneTimeBonuses)
			{
				if (!keyValuePair.Value)
				{
					return;
				}
			}
			if (this.matOwner.GetNumberOfStars(StarType.Recruits) == 1)
			{
				return;
			}
			this.matOwner.GainStar(StarType.Recruits);
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x00045816 File Offset: 0x00043A16
		public void CheckMechStar()
		{
			if (this.mechs.Count < 4)
			{
				return;
			}
			if (this.matOwner.GetNumberOfStars(StarType.Mechs) == 1)
			{
				return;
			}
			this.matOwner.GainStar(StarType.Mechs);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x00045843 File Offset: 0x00043A43
		public bool CanRiverwalk(HexType to)
		{
			return this.SkillUnlocked[0] && (this.abilities[0] & (AbilityPerk)to) == (AbilityPerk)to;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x00120F2C File Offset: 0x0011F12C
		public bool CanRiverwalk(GameHex from, GameHex to, Unit unit)
		{
			if (!this.SkillUnlocked[0] || from == null || to == null)
			{
				return false;
			}
			if (this.faction == Faction.Albion)
			{
				return this.CanRiverwalkAlbion(from, to, unit);
			}
			if (this.faction == Faction.Togawa)
			{
				return this.CanRiverwalkTogawa(from, to);
			}
			return this.CanRiverwalk(to.hexType);
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x00045862 File Offset: 0x00043A62
		private bool CanRiverwalkAlbion(GameHex from, GameHex to, Unit unit)
		{
			return from.HasTunnelAccess(unit) || to.HasTunnelAccess(unit);
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x00045876 File Offset: 0x00043A76
		private bool CanRiverwalkTogawa(GameHex from, GameHex to)
		{
			return this.CanRiverwalkAgain();
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x0004587E File Offset: 0x00043A7E
		public bool CanRiverwalkAgain()
		{
			return this.faction != Faction.Togawa || (this.faction == Faction.Togawa && !this.RiverwalkMoveUsed());
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x0004589F File Offset: 0x00043A9F
		public bool DiversionAbilityUnlocked()
		{
			return this.SkillUnlocked[2] || (this.faction == Faction.Albion && this.SkillUnlocked[1]);
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x00120F7C File Offset: 0x0011F17C
		public bool CanEnlist()
		{
			foreach (KeyValuePair<GainType, bool> keyValuePair in this.OneTimeBonuses)
			{
				if (!keyValuePair.Value)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000458C0 File Offset: 0x00043AC0
		public void SetRiverwalkUsed(bool used)
		{
			this.riverwalkMoveUsed = used;
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000458C9 File Offset: 0x00043AC9
		public bool RiverwalkMoveUsed()
		{
			return this.riverwalkMoveUsed;
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000458D1 File Offset: 0x00043AD1
		public static int GetStartingPower(Faction faction)
		{
			switch (faction)
			{
			case Faction.Polania:
				return 2;
			case Faction.Albion:
				return 3;
			case Faction.Nordic:
				return 4;
			case Faction.Rusviet:
				return 3;
			case Faction.Togawa:
				return 0;
			case Faction.Crimea:
				return 5;
			case Faction.Saxony:
				return 1;
			default:
				return 0;
			}
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x00045906 File Offset: 0x00043B06
		public bool DidPlayerUsedMatLastTurn(int currentMatSection, int sectionID)
		{
			return currentMatSection == sectionID && (this.factionPerk != AbilityPerk.Relenteless || sectionID == 4);
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x0004591D File Offset: 0x00043B1D
		public static int GetStartingAmmo(Faction faction)
		{
			switch (faction)
			{
			case Faction.Polania:
				return 3;
			case Faction.Albion:
				return 0;
			case Faction.Nordic:
				return 1;
			case Faction.Rusviet:
				return 2;
			case Faction.Togawa:
				return 2;
			case Faction.Crimea:
				return 0;
			case Faction.Saxony:
				return 4;
			default:
				return 0;
			}
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x00120FD8 File Offset: 0x0011F1D8
		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			if (reader.GetAttribute("RiverwalkMoveUsed") != null)
			{
				this.riverwalkMoveUsed = true;
			}
			reader.ReadStartElement();
			while (reader.Name == "Mech")
			{
				Mech mech = new Mech(this.gameManager, this.MatOwner, 1);
				((IXmlSerializable)mech).ReadXml(reader);
				this.mechs.Add(mech);
				reader.ReadStartElement();
			}
			if (reader.Name == "Skills")
			{
				string text = reader.ReadElementContentAsString();
				if (text != "")
				{
					foreach (string text2 in text.Split(' ', StringSplitOptions.None))
					{
						if (text2 == "")
						{
							break;
						}
						int num = int.Parse(text2);
						this.SkillUnlocked[num] = true;
						if (num == 3 && this.faction != Faction.Albion && this.faction != Faction.Togawa && this.MatOwner.matFaction.faction != Faction.Albion && this.MatOwner.matFaction.faction != Faction.Togawa)
						{
							this.MatOwner.character.UpgradeMaxMoveCount();
							foreach (Mech mech2 in this.mechs)
							{
								mech2.UpgradeMaxMoveCount();
							}
						}
					}
				}
			}
			if (reader.Name == "Bonuses")
			{
				string text3 = reader.ReadElementContentAsString();
				if (text3 != "")
				{
					foreach (string text4 in text3.Split(' ', StringSplitOptions.None))
					{
						if (text4 == "")
						{
							break;
						}
						this.OneTimeBonuses[(GainType)int.Parse(text4)] = true;
					}
				}
			}
			if (reader.Name == "TokenSupply")
			{
				((IXmlSerializable)this.FactionTokens).ReadXml(reader);
				this.FactionTokens.SetOwner(this.MatOwner);
			}
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x001211E0 File Offset: 0x0011F3E0
		public void WriteXml(XmlWriter writer)
		{
			if (this.riverwalkMoveUsed)
			{
				writer.WriteAttributeString("RiverwalkMoveUsed", "");
			}
			foreach (IXmlSerializable xmlSerializable in this.mechs)
			{
				writer.WriteStartElement("Mech");
				xmlSerializable.WriteXml(writer);
				writer.WriteEndElement();
			}
			writer.WriteStartElement("Skills");
			for (int i = 0; i < 4; i++)
			{
				if (this.SkillUnlocked[i])
				{
					writer.WriteValue(i.ToString() + " ");
				}
			}
			writer.WriteEndElement();
			writer.WriteStartElement("Bonuses");
			for (int j = 0; j < 4; j++)
			{
				if (this.OneTimeBonuses[(GainType)j])
				{
					writer.WriteValue(j.ToString() + " ");
				}
			}
			writer.WriteEndElement();
			if (this.FactionTokens != null)
			{
				((IXmlSerializable)this.FactionTokens).WriteXml(writer);
			}
		}

		// Token: 0x04002048 RID: 8264
		private Player matOwner;

		// Token: 0x04002049 RID: 8265
		public Faction faction;

		// Token: 0x0400204A RID: 8266
		public List<Mech> mechs = new List<Mech>();

		// Token: 0x0400204B RID: 8267
		public bool[] SkillUnlocked = new bool[4];

		// Token: 0x0400204C RID: 8268
		public AbilityPerk factionPerk;

		// Token: 0x0400204D RID: 8269
		public List<AbilityPerk> abilities = new List<AbilityPerk>();

		// Token: 0x0400204E RID: 8270
		private int startingPower;

		// Token: 0x0400204F RID: 8271
		private int startingCombatCards;

		// Token: 0x04002050 RID: 8272
		private bool riverwalkMoveUsed;

		// Token: 0x04002053 RID: 8275
		private GameManager gameManager;
	}
}
