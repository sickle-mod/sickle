using System;
using System.Linq;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200088B RID: 2187
	[Serializable]
	public class Achievement
	{
		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x0004F771 File Offset: 0x0004D971
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x0004F779 File Offset: 0x0004D979
		public string Tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x0004F781 File Offset: 0x0004D981
		public AchievementStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06003D0D RID: 15629 RVA: 0x0004F789 File Offset: 0x0004D989
		public AchievementUnicity Unicity
		{
			get
			{
				return this._unicity;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06003D0E RID: 15630 RVA: 0x0004F791 File Offset: 0x0004D991
		public string Game
		{
			get
			{
				return this._game;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06003D0F RID: 15631 RVA: 0x0004F799 File Offset: 0x0004D999
		public AchievementType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06003D10 RID: 15632 RVA: 0x0004F7A1 File Offset: 0x0004D9A1
		public bool Secret
		{
			get
			{
				return this._secret;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06003D11 RID: 15633 RVA: 0x0004F7A9 File Offset: 0x0004D9A9
		public int Treasure
		{
			get
			{
				return this._treasure;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x0004F7B1 File Offset: 0x0004D9B1
		public int Category
		{
			get
			{
				return this._category;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06003D13 RID: 15635 RVA: 0x0004F7B9 File Offset: 0x0004D9B9
		public string Picture
		{
			get
			{
				return this._picture;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06003D14 RID: 15636 RVA: 0x0004F7C1 File Offset: 0x0004D9C1
		public string Ribbon
		{
			get
			{
				return this._ribbon;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06003D15 RID: 15637 RVA: 0x0004F7C9 File Offset: 0x0004D9C9
		public Achievement.Text[] Texts
		{
			get
			{
				return this._texts;
			}
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x00157820 File Offset: 0x00155A20
		public Achievement(JsonAchievement raw)
		{
			this._id = raw.id;
			this._tag = raw.tag;
			this._status = ((raw.status == null) ? AchievementStatus.Null : ((AchievementStatus)Enum.Parse(typeof(AchievementStatus), this._FirstWordLetterToUpper(raw.status))));
			this._unicity = ((raw.unicity == null) ? AchievementUnicity.Null : ((AchievementUnicity)Enum.Parse(typeof(AchievementUnicity), this._FirstWordLetterToUpper(raw.unicity))));
			this._game = raw.game;
			this._type = ((raw.type == null) ? AchievementType.Null : ((AchievementType)Enum.Parse(typeof(AchievementType), this._FirstWordLetterToUpper(raw.type))));
			this._secret = raw.secret;
			this._treasure = raw.treasure;
			this._category = raw.category;
			this._picture = raw.picture;
			this._ribbon = raw.ribbon;
			this._texts = raw.texts.Select((JsonAchievement.Text x) => new Achievement.Text(x)).ToArray<Achievement.Text>();
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x0015795C File Offset: 0x00155B5C
		public override bool Equals(object obj)
		{
			Achievement achievement = obj as Achievement;
			return achievement != null && (this.Id == achievement.Id && this.Tag == achievement.Tag && this.Status == achievement.Status && this.Unicity == achievement.Unicity && this.Game == achievement.Game && this.Type == achievement.Type && this.Secret == achievement.Secret && this.Treasure == achievement.Treasure && this.Category == achievement.Category && this.Picture == achievement.Picture && this.Ribbon == achievement.Ribbon) && this.Texts.Diff(achievement.Texts).Count<Achievement.Text>() == 0;
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x00157A4C File Offset: 0x00155C4C
		public override int GetHashCode()
		{
			return this.Id ^ ((this.Tag == null) ? 0 : this.Tag.GetHashCode()) ^ (int)this.Status ^ (int)this.Unicity ^ ((this.Game == null) ? 0 : this.Game.GetHashCode()) ^ (int)this.Type ^ this.Secret.GetHashCode() ^ this.Treasure ^ this.Category ^ ((this.Picture == null) ? 0 : this.Picture.GetHashCode()) ^ ((this.Ribbon == null) ? 0 : this.Ribbon.GetHashCode());
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x00157AF0 File Offset: 0x00155CF0
		private string _FirstWordLetterToUpper(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			return char.ToUpper(s[0]).ToString() + s.Substring(1).ToLower();
		}

		// Token: 0x04002E29 RID: 11817
		[SerializeField]
		private int _id;

		// Token: 0x04002E2A RID: 11818
		[SerializeField]
		private string _tag;

		// Token: 0x04002E2B RID: 11819
		[SerializeField]
		private AchievementStatus _status;

		// Token: 0x04002E2C RID: 11820
		[SerializeField]
		private AchievementUnicity _unicity;

		// Token: 0x04002E2D RID: 11821
		[SerializeField]
		private string _game;

		// Token: 0x04002E2E RID: 11822
		[SerializeField]
		private AchievementType _type;

		// Token: 0x04002E2F RID: 11823
		[SerializeField]
		private bool _secret;

		// Token: 0x04002E30 RID: 11824
		[SerializeField]
		private int _treasure;

		// Token: 0x04002E31 RID: 11825
		[SerializeField]
		private int _category;

		// Token: 0x04002E32 RID: 11826
		[SerializeField]
		private string _picture;

		// Token: 0x04002E33 RID: 11827
		[SerializeField]
		private string _ribbon;

		// Token: 0x04002E34 RID: 11828
		[SerializeField]
		private Achievement.Text[] _texts;

		// Token: 0x0200088C RID: 2188
		[Serializable]
		public class Text
		{
			// Token: 0x1700051D RID: 1309
			// (get) Token: 0x06003D1A RID: 15642 RVA: 0x0004F7D1 File Offset: 0x0004D9D1
			public string Lang
			{
				get
				{
					return this._lang;
				}
			}

			// Token: 0x1700051E RID: 1310
			// (get) Token: 0x06003D1B RID: 15643 RVA: 0x0004F7D9 File Offset: 0x0004D9D9
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x1700051F RID: 1311
			// (get) Token: 0x06003D1C RID: 15644 RVA: 0x0004F7E1 File Offset: 0x0004D9E1
			public string Description
			{
				get
				{
					return this._description;
				}
			}

			// Token: 0x06003D1D RID: 15645 RVA: 0x0004F7E9 File Offset: 0x0004D9E9
			public Text(JsonAchievement.Text raw)
			{
				this._lang = raw.lang;
				this._name = raw.name;
				this._description = raw.description;
			}

			// Token: 0x06003D1E RID: 15646 RVA: 0x00157B2C File Offset: 0x00155D2C
			public override bool Equals(object obj)
			{
				Achievement.Text text = obj as Achievement.Text;
				return text != null && (this.Lang == text.Lang && this.Name == text.Name) && this.Description == text.Description;
			}

			// Token: 0x06003D1F RID: 15647 RVA: 0x00157B80 File Offset: 0x00155D80
			public override int GetHashCode()
			{
				return ((this.Lang == null) ? 0 : this.Lang.GetHashCode()) ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ ((this.Description == null) ? 0 : this.Description.GetHashCode());
			}

			// Token: 0x04002E35 RID: 11829
			[SerializeField]
			private string _lang;

			// Token: 0x04002E36 RID: 11830
			[SerializeField]
			private string _name;

			// Token: 0x04002E37 RID: 11831
			[SerializeField]
			private string _description;
		}
	}
}
