using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005EA RID: 1514
	public class MatFaction : IXmlSerializable
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06002FB6 RID: 12214 RVA: 0x0004578F File Offset: 0x0004398F
		// (set) Token: 0x06002FB7 RID: 12215 RVA: 0x00045797 File Offset: 0x00043997
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

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x000457B9 File Offset: 0x000439B9
		// (set) Token: 0x06002FB9 RID: 12217 RVA: 0x000457C1 File Offset: 0x000439C1
		public TokenSupply FactionTokens { get; private set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000457CA File Offset: 0x000439CA
		// (set) Token: 0x06002FBB RID: 12219 RVA: 0x000457D2 File Offset: 0x000439D2
		public Dictionary<GainType, bool> OneTimeBonuses { get; private set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000457DB File Offset: 0x000439DB
		public int StartingPower
		{
			get
			{
				return this.startingPower;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000457E3 File Offset: 0x000439E3
		public int StartingCombatCards
		{
			get
			{
				return this.startingCombatCards;
			}
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000457EB File Offset: 0x000439EB
		public MatFaction(GameManager gameManager, Faction faction, int startingPower, int startingCombatCards, AbilityPerk factionPerk, params AbilityPerk[] abilities)
		{
			this.gameManager = gameManager;
			this.SetStartingParameters(faction, startingPower, startingCombatCards, factionPerk, abilities);
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0004582A File Offset: 0x00043A2A
		public MatFaction(GameManager gameManager, Faction faction)
		{
			this.gameManager = gameManager;
			this.SetFaction(faction);
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x00128D3C File Offset: 0x00126F3C
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
				this.SetStartingParameters(Faction.Rusviet, MatFaction.GetStartingPower(faction), MatFaction.GetStartingAmmo(faction), AbilityPerk.Relentless, new AbilityPerk[]
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

		// Token: 0x06002FC1 RID: 12225 RVA: 0x00128E7C File Offset: 0x0012707C
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

		// Token: 0x06002FC2 RID: 12226 RVA: 0x00045862 File Offset: 0x00043A62
		public bool OneTimeBonusUsed(GainType bonusType)
		{
			return this.OneTimeBonuses[bonusType];
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x00128F18 File Offset: 0x00127118
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

		// Token: 0x06002FC4 RID: 12228 RVA: 0x00128FE0 File Offset: 0x001271E0
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

		// Token: 0x06002FC5 RID: 12229 RVA: 0x00045870 File Offset: 0x00043A70
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

		// Token: 0x06002FC6 RID: 12230 RVA: 0x0004589D File Offset: 0x00043A9D
		public bool CanRiverwalk(HexType to)
		{
			return this.SkillUnlocked[0] && (this.abilities[0] & (AbilityPerk)to) == (AbilityPerk)to;
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x00129054 File Offset: 0x00127254
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

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000458BC File Offset: 0x00043ABC
		private bool CanRiverwalkAlbion(GameHex from, GameHex to, Unit unit)
		{
			return from.HasTunnelAccess(unit) || to.HasTunnelAccess(unit);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000458D0 File Offset: 0x00043AD0
		private bool CanRiverwalkTogawa(GameHex from, GameHex to)
		{
			return this.CanRiverwalkAgain();
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000458D8 File Offset: 0x00043AD8
		public bool CanRiverwalkAgain()
		{
			return this.faction != Faction.Togawa || (this.faction == Faction.Togawa && !this.RiverwalkMoveUsed());
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x000458F9 File Offset: 0x00043AF9
		public bool DiversionAbilityUnlocked()
		{
			return this.SkillUnlocked[2] || (this.faction == Faction.Albion && this.SkillUnlocked[1]);
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x001290A4 File Offset: 0x001272A4
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

		// Token: 0x06002FCD RID: 12237 RVA: 0x0004591A File Offset: 0x00043B1A
		public void SetRiverwalkUsed(bool used)
		{
			this.riverwalkMoveUsed = used;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x00045923 File Offset: 0x00043B23
		public bool RiverwalkMoveUsed()
		{
			return this.riverwalkMoveUsed;
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x0004592B File Offset: 0x00043B2B
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

		// Token: 0x06002FD0 RID: 12240 RVA: 0x00045960 File Offset: 0x00043B60
		public bool DidPlayerUsedMatLastTurn(int currentMatSection, int sectionID)
		{
			return currentMatSection == sectionID && (this.factionPerk != AbilityPerk.Relentless || sectionID == 4);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x00045977 File Offset: 0x00043B77
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

		// Token: 0x06002FD2 RID: 12242 RVA: 0x0002F60E File Offset: 0x0002D80E
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x00129100 File Offset: 0x00127300
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

		// Token: 0x06002FD4 RID: 12244 RVA: 0x00129308 File Offset: 0x00127508
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

		// Token: 0x0400206A RID: 8298
		private Player matOwner;

		// Token: 0x0400206B RID: 8299
		public Faction faction;

		// Token: 0x0400206C RID: 8300
		public List<Mech> mechs = new List<Mech>();

		// Token: 0x0400206D RID: 8301
		public bool[] SkillUnlocked = new bool[4];

		// Token: 0x0400206E RID: 8302
		public AbilityPerk factionPerk;

		// Token: 0x0400206F RID: 8303
		public List<AbilityPerk> abilities = new List<AbilityPerk>();

		// Token: 0x04002070 RID: 8304
		private int startingPower;

		// Token: 0x04002071 RID: 8305
		private int startingCombatCards;

		// Token: 0x04002072 RID: 8306
		private bool riverwalkMoveUsed;

		// Token: 0x04002075 RID: 8309
		private GameManager gameManager;
	}
}
