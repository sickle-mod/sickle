using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008F7 RID: 2295
	[Serializable]
	public class User
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06003E4F RID: 15951 RVA: 0x000501DE File Offset: 0x0004E3DE
		public int UserId
		{
			get
			{
				return this._userId;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06003E50 RID: 15952 RVA: 0x000501E6 File Offset: 0x0004E3E6
		public string LoginName
		{
			get
			{
				return this._loginName;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x000501EE File Offset: 0x0004E3EE
		public string Country
		{
			get
			{
				return this._country;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x000501F6 File Offset: 0x0004E3F6
		public bool? EmailValid
		{
			get
			{
				return this._emailValid;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06003E53 RID: 15955 RVA: 0x000501FE File Offset: 0x0004E3FE
		public User.UserLanguages? Language
		{
			get
			{
				return this._language;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x00050206 File Offset: 0x0004E406
		public string TimeZone
		{
			get
			{
				return this._timeZone;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06003E55 RID: 15957 RVA: 0x0005020E File Offset: 0x0004E40E
		public int PostedMsgCount
		{
			get
			{
				return this._postedMsgCount;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x00050216 File Offset: 0x0004E416
		public int LastPostId
		{
			get
			{
				return this._lastPostId;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06003E57 RID: 15959 RVA: 0x0005021E File Offset: 0x0004E41E
		public bool? Validated
		{
			get
			{
				return this._validated;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06003E58 RID: 15960 RVA: 0x00050226 File Offset: 0x0004E426
		public string Avatar
		{
			get
			{
				return this._avatar;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06003E59 RID: 15961 RVA: 0x0005022E File Offset: 0x0004E42E
		public DateTime? JoinDate
		{
			get
			{
				return this._joinDate;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06003E5A RID: 15962 RVA: 0x00050236 File Offset: 0x0004E436
		public DateTime? LastVisit
		{
			get
			{
				return this._lastVisit;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06003E5B RID: 15963 RVA: 0x0005023E File Offset: 0x0004E43E
		public string Zipcode
		{
			get
			{
				return this._zipcode;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06003E5C RID: 15964 RVA: 0x00050246 File Offset: 0x0004E446
		public DateTime? Birthday
		{
			get
			{
				return this._birthday;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06003E5D RID: 15965 RVA: 0x0005024E File Offset: 0x0004E44E
		public string Email
		{
			get
			{
				return this._email;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06003E5E RID: 15966 RVA: 0x00050256 File Offset: 0x0004E456
		public User.UserGender? Gender
		{
			get
			{
				return this._gender;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06003E5F RID: 15967 RVA: 0x0005025E File Offset: 0x0004E45E
		public bool? Coppa
		{
			get
			{
				return this._coppa;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06003E60 RID: 15968 RVA: 0x00050266 File Offset: 0x0004E466
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0005026E File Offset: 0x0004E46E
		public string[] Features
		{
			get
			{
				return this._features;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x00050276 File Offset: 0x0004E476
		public User.BoardGame[] Boardgames
		{
			get
			{
				return this._boardGames;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x0005027E File Offset: 0x0004E47E
		public User.OnlineGame[] Onlinegames
		{
			get
			{
				return this._onlineGames;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06003E64 RID: 15972 RVA: 0x00050286 File Offset: 0x0004E486
		public PartnerAccount[] Partners
		{
			get
			{
				return this._partners;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x0005028E File Offset: 0x0004E48E
		public bool? Newsletter
		{
			get
			{
				return this._newsletter;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06003E66 RID: 15974 RVA: 0x00050296 File Offset: 0x0004E496
		public string Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06003E67 RID: 15975 RVA: 0x0005029E File Offset: 0x0004E49E
		public string Wc
		{
			get
			{
				return this._wc;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06003E68 RID: 15976 RVA: 0x000502A6 File Offset: 0x0004E4A6
		public bool? SendSignUpEmail
		{
			get
			{
				return this._sendSignUpEmail;
			}
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00158F8C File Offset: 0x0015718C
		private User(Builder<User> builder)
		{
			User.Builder builder2 = builder as User.Builder;
			this._email = builder2.GetEmail;
			this._loginName = builder2.GetLoginName;
			this._password = builder2.GetPassword;
			this._name = builder2.GetName;
			this._gender = builder2.GetGender;
			this._birthday = builder2.GetBirthday;
			this._zipcode = builder2.GetZipcode;
			this._country = builder2.GetCountry;
			this._language = builder2.GetLanguage;
			this._timeZone = builder2.GetTimeZone;
			this._coppa = builder2.GetCoppa;
			this._newsletter = builder2.GetNewsletter;
			this._avatar = builder2.GetAvatar;
			this._sendSignUpEmail = builder2.GetSendSignUpEmail;
			this._wc = builder2.GetWC;
			this._postedMsgCount = builder2.GetPostedMsgCount;
			this._emailValid = builder2.GetEmailValid;
			this._userId = builder2.GetUserId;
			this._lastPostId = builder2.GetLastPostId;
			this._validated = builder2.GetValidated;
			this._joinDate = builder2.GetJoinDate;
			this._lastVisit = builder2.GetLastVisit;
			this._features = builder2.GetFeatures;
			this._boardGames = builder2.GetBoardGame;
			this._onlineGames = builder2.GetOnlineGames;
			this._partners = builder2.GetPartners;
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x001590F4 File Offset: 0x001572F4
		public override bool Equals(object obj)
		{
			User user = obj as User;
			if (user == null)
			{
				return false;
			}
			if (this.UserId == user.UserId && this.LoginName == user.LoginName && this.Country == user.Country)
			{
				bool? flag = this.EmailValid;
				bool? flag2 = user.EmailValid;
				if ((flag.GetValueOrDefault() == flag2.GetValueOrDefault()) & (flag != null == (flag2 != null)))
				{
					User.UserLanguages? language = this.Language;
					User.UserLanguages? language2 = user.Language;
					if (((language.GetValueOrDefault() == language2.GetValueOrDefault()) & (language != null == (language2 != null))) && this.TimeZone == user.TimeZone && this.PostedMsgCount == user.PostedMsgCount && this.LastPostId == user.LastPostId)
					{
						flag2 = this.Validated;
						flag = user.Validated;
						if (((flag2.GetValueOrDefault() == flag.GetValueOrDefault()) & (flag2 != null == (flag != null))) && this.JoinDate == user.JoinDate && this.LastVisit == user.LastVisit && this.Zipcode == user.Zipcode && this.Birthday == user.Birthday && this.Email == user.Email)
						{
							User.UserGender? gender = this.Gender;
							User.UserGender? gender2 = user.Gender;
							if ((gender.GetValueOrDefault() == gender2.GetValueOrDefault()) & (gender != null == (gender2 != null)))
							{
								flag = this.Coppa;
								flag2 = user.Coppa;
								if (((flag.GetValueOrDefault() == flag2.GetValueOrDefault()) & (flag != null == (flag2 != null))) && this.Name == user.Name)
								{
									flag2 = this.Newsletter;
									flag = user.Newsletter;
									if (((flag2.GetValueOrDefault() == flag.GetValueOrDefault()) & (flag2 != null == (flag != null))) && this.Password == user.Password && this.Wc == user.Wc)
									{
										flag = this.SendSignUpEmail;
										flag2 = user.SendSignUpEmail;
										if (((flag.GetValueOrDefault() == flag2.GetValueOrDefault()) & (flag != null == (flag2 != null))) && ((this.Features == null && user.Features == null) || this.Features.Length == user.Features.Length) && ((this.Boardgames == null && user.Boardgames == null) || this.Boardgames.Length == user.Boardgames.Length) && ((this.Onlinegames == null && user.Onlinegames == null) || this.Onlinegames.Length == user.Onlinegames.Length))
										{
											return (this.Partners == null && user.Partners == null) || this.Partners.Length == user.Partners.Length;
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x0002F718 File Offset: 0x0002D918
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04002FDC RID: 12252
		private string _email;

		// Token: 0x04002FDD RID: 12253
		private string _loginName;

		// Token: 0x04002FDE RID: 12254
		private string _password;

		// Token: 0x04002FDF RID: 12255
		private string _name;

		// Token: 0x04002FE0 RID: 12256
		private User.UserGender? _gender;

		// Token: 0x04002FE1 RID: 12257
		private DateTime? _birthday;

		// Token: 0x04002FE2 RID: 12258
		private string _zipcode;

		// Token: 0x04002FE3 RID: 12259
		private string _country;

		// Token: 0x04002FE4 RID: 12260
		private User.UserLanguages? _language;

		// Token: 0x04002FE5 RID: 12261
		private string _timeZone;

		// Token: 0x04002FE6 RID: 12262
		private bool? _coppa;

		// Token: 0x04002FE7 RID: 12263
		private bool? _newsletter;

		// Token: 0x04002FE8 RID: 12264
		private string _avatar;

		// Token: 0x04002FE9 RID: 12265
		private bool? _sendSignUpEmail;

		// Token: 0x04002FEA RID: 12266
		private string _wc;

		// Token: 0x04002FEB RID: 12267
		private int _userId = -1;

		// Token: 0x04002FEC RID: 12268
		private bool? _emailValid;

		// Token: 0x04002FED RID: 12269
		private int _postedMsgCount = -1;

		// Token: 0x04002FEE RID: 12270
		private int _lastPostId = -1;

		// Token: 0x04002FEF RID: 12271
		private bool? _validated;

		// Token: 0x04002FF0 RID: 12272
		private DateTime? _joinDate;

		// Token: 0x04002FF1 RID: 12273
		private DateTime? _lastVisit;

		// Token: 0x04002FF2 RID: 12274
		private string[] _features;

		// Token: 0x04002FF3 RID: 12275
		private User.BoardGame[] _boardGames;

		// Token: 0x04002FF4 RID: 12276
		private User.OnlineGame[] _onlineGames;

		// Token: 0x04002FF5 RID: 12277
		private PartnerAccount[] _partners;

		// Token: 0x020008F8 RID: 2296
		public enum UserGender
		{
			// Token: 0x04002FF7 RID: 12279
			MALE,
			// Token: 0x04002FF8 RID: 12280
			FEMALE,
			// Token: 0x04002FF9 RID: 12281
			UNSPECIFIED
		}

		// Token: 0x020008F9 RID: 2297
		public enum UserLanguages
		{
			// Token: 0x04002FFB RID: 12283
			en,
			// Token: 0x04002FFC RID: 12284
			fr,
			// Token: 0x04002FFD RID: 12285
			de,
			// Token: 0x04002FFE RID: 12286
			it,
			// Token: 0x04002FFF RID: 12287
			es
		}

		// Token: 0x020008FA RID: 2298
		public class BoardGame
		{
			// Token: 0x06003E6C RID: 15980 RVA: 0x000502AE File Offset: 0x0004E4AE
			public BoardGame(string code, string name, DateTime? registeredDate)
			{
				this.code = code;
				this.name = name;
				this.registeredDate = registeredDate;
			}

			// Token: 0x04003000 RID: 12288
			public string code;

			// Token: 0x04003001 RID: 12289
			public string name;

			// Token: 0x04003002 RID: 12290
			public DateTime? registeredDate;
		}

		// Token: 0x020008FB RID: 2299
		public class OnlineGame
		{
			// Token: 0x06003E6D RID: 15981 RVA: 0x000502CB File Offset: 0x0004E4CB
			public OnlineGame(string game, int nbGames, int karma, float rankScore, int rank, DateTime? lastGame, string variant)
			{
				this.game = game;
				this.nbGames = nbGames;
				this.karma = karma;
				this.rankScore = rankScore;
				this.rank = rank;
				this.lastGame = lastGame;
				this.variant = variant;
			}

			// Token: 0x04003003 RID: 12291
			public string game;

			// Token: 0x04003004 RID: 12292
			public int nbGames;

			// Token: 0x04003005 RID: 12293
			public int karma;

			// Token: 0x04003006 RID: 12294
			public float rankScore;

			// Token: 0x04003007 RID: 12295
			public int rank;

			// Token: 0x04003008 RID: 12296
			public DateTime? lastGame;

			// Token: 0x04003009 RID: 12297
			public string variant;
		}

		// Token: 0x020008FC RID: 2300
		public class Builder : Builder<User>
		{
			// Token: 0x06003E6E RID: 15982 RVA: 0x00050308 File Offset: 0x0004E508
			public User.Builder Email(string email)
			{
				this._email = email;
				return this;
			}

			// Token: 0x06003E6F RID: 15983 RVA: 0x00050312 File Offset: 0x0004E512
			public User.Builder LoginName(string loginName)
			{
				this._loginName = loginName;
				return this;
			}

			// Token: 0x06003E70 RID: 15984 RVA: 0x0005031C File Offset: 0x0004E51C
			public User.Builder Password(string password)
			{
				this._password = password;
				return this;
			}

			// Token: 0x06003E71 RID: 15985 RVA: 0x00050326 File Offset: 0x0004E526
			public User.Builder Name(string name)
			{
				this._name = name;
				return this;
			}

			// Token: 0x06003E72 RID: 15986 RVA: 0x00050330 File Offset: 0x0004E530
			public User.Builder Gender(User.UserGender? gender)
			{
				this._gender = gender;
				return this;
			}

			// Token: 0x06003E73 RID: 15987 RVA: 0x0005033A File Offset: 0x0004E53A
			public User.Builder Birthday(DateTime? birthday)
			{
				this._birthday = birthday;
				return this;
			}

			// Token: 0x06003E74 RID: 15988 RVA: 0x00050344 File Offset: 0x0004E544
			public User.Builder Zipcode(string zipcode)
			{
				this._zipcode = zipcode;
				return this;
			}

			// Token: 0x06003E75 RID: 15989 RVA: 0x0005034E File Offset: 0x0004E54E
			public User.Builder Country(string country)
			{
				this._country = country;
				return this;
			}

			// Token: 0x06003E76 RID: 15990 RVA: 0x00050358 File Offset: 0x0004E558
			public User.Builder Language(User.UserLanguages? language)
			{
				this._language = language;
				return this;
			}

			// Token: 0x06003E77 RID: 15991 RVA: 0x00050362 File Offset: 0x0004E562
			public User.Builder TimeZone(string timezone)
			{
				this._timeZone = timezone;
				return this;
			}

			// Token: 0x06003E78 RID: 15992 RVA: 0x0005036C File Offset: 0x0004E56C
			public User.Builder Coppa(bool? coppa)
			{
				this._coppa = coppa;
				return this;
			}

			// Token: 0x06003E79 RID: 15993 RVA: 0x00050376 File Offset: 0x0004E576
			public User.Builder Newsletter(bool? newsletter)
			{
				this._newsletter = newsletter;
				return this;
			}

			// Token: 0x06003E7A RID: 15994 RVA: 0x00050380 File Offset: 0x0004E580
			public User.Builder Avatar(string avatar)
			{
				this._avatar = avatar;
				return this;
			}

			// Token: 0x06003E7B RID: 15995 RVA: 0x0005038A File Offset: 0x0004E58A
			public User.Builder SendSignUpEmail(bool? sendSignUpEmail)
			{
				this._sendSignUpEmail = sendSignUpEmail;
				return this;
			}

			// Token: 0x06003E7C RID: 15996 RVA: 0x00050394 File Offset: 0x0004E594
			public User.Builder WC(string webcode)
			{
				this._wc = webcode;
				return this;
			}

			// Token: 0x06003E7D RID: 15997 RVA: 0x0005039E File Offset: 0x0004E59E
			public User.Builder UserId(int userId)
			{
				this._userId = userId;
				return this;
			}

			// Token: 0x06003E7E RID: 15998 RVA: 0x000503A8 File Offset: 0x0004E5A8
			public User.Builder EmailValid(bool? emailValid)
			{
				this._emailValid = emailValid;
				return this;
			}

			// Token: 0x06003E7F RID: 15999 RVA: 0x000503B2 File Offset: 0x0004E5B2
			public User.Builder PostMsgCount(int postMsgCount)
			{
				this._postedMsgCount = postMsgCount;
				return this;
			}

			// Token: 0x06003E80 RID: 16000 RVA: 0x000503BC File Offset: 0x0004E5BC
			public User.Builder LastPostId(int lastPostId)
			{
				this._lastPostId = lastPostId;
				return this;
			}

			// Token: 0x06003E81 RID: 16001 RVA: 0x000503C6 File Offset: 0x0004E5C6
			public User.Builder Validated(bool validated)
			{
				this._validated = new bool?(validated);
				return this;
			}

			// Token: 0x06003E82 RID: 16002 RVA: 0x000503D5 File Offset: 0x0004E5D5
			public User.Builder JoinDate(DateTime joinDate)
			{
				this._joinDate = new DateTime?(joinDate);
				return this;
			}

			// Token: 0x06003E83 RID: 16003 RVA: 0x000503E4 File Offset: 0x0004E5E4
			public User.Builder LastVisit(DateTime lastVisit)
			{
				this._lastVisit = new DateTime?(lastVisit);
				return this;
			}

			// Token: 0x06003E84 RID: 16004 RVA: 0x000503F3 File Offset: 0x0004E5F3
			public User.Builder Features(string[] features)
			{
				this._features = features;
				return this;
			}

			// Token: 0x06003E85 RID: 16005 RVA: 0x000503FD File Offset: 0x0004E5FD
			public User.Builder BoardGames(User.BoardGame[] boardGames)
			{
				this._boardGames = boardGames;
				return this;
			}

			// Token: 0x06003E86 RID: 16006 RVA: 0x00050407 File Offset: 0x0004E607
			public User.Builder OnlineGames(User.OnlineGame[] onlineGames)
			{
				this._onlineGames = onlineGames;
				return this;
			}

			// Token: 0x06003E87 RID: 16007 RVA: 0x00050411 File Offset: 0x0004E611
			public User.Builder Partners(PartnerAccount[] partners)
			{
				this._partners = partners;
				return this;
			}

			// Token: 0x170005A1 RID: 1441
			// (get) Token: 0x06003E88 RID: 16008 RVA: 0x0005041B File Offset: 0x0004E61B
			public string GetLoginName
			{
				get
				{
					return this._loginName;
				}
			}

			// Token: 0x170005A2 RID: 1442
			// (get) Token: 0x06003E89 RID: 16009 RVA: 0x00050423 File Offset: 0x0004E623
			public string GetCountry
			{
				get
				{
					return this._country;
				}
			}

			// Token: 0x170005A3 RID: 1443
			// (get) Token: 0x06003E8A RID: 16010 RVA: 0x0005042B File Offset: 0x0004E62B
			public User.UserLanguages? GetLanguage
			{
				get
				{
					return this._language;
				}
			}

			// Token: 0x170005A4 RID: 1444
			// (get) Token: 0x06003E8B RID: 16011 RVA: 0x00050433 File Offset: 0x0004E633
			public string GetTimeZone
			{
				get
				{
					return this._timeZone;
				}
			}

			// Token: 0x170005A5 RID: 1445
			// (get) Token: 0x06003E8C RID: 16012 RVA: 0x0005043B File Offset: 0x0004E63B
			public string GetAvatar
			{
				get
				{
					return this._avatar;
				}
			}

			// Token: 0x170005A6 RID: 1446
			// (get) Token: 0x06003E8D RID: 16013 RVA: 0x00050443 File Offset: 0x0004E643
			public string GetZipcode
			{
				get
				{
					return this._zipcode;
				}
			}

			// Token: 0x170005A7 RID: 1447
			// (get) Token: 0x06003E8E RID: 16014 RVA: 0x0005044B File Offset: 0x0004E64B
			public DateTime? GetBirthday
			{
				get
				{
					return this._birthday;
				}
			}

			// Token: 0x170005A8 RID: 1448
			// (get) Token: 0x06003E8F RID: 16015 RVA: 0x00050453 File Offset: 0x0004E653
			public string GetEmail
			{
				get
				{
					return this._email;
				}
			}

			// Token: 0x170005A9 RID: 1449
			// (get) Token: 0x06003E90 RID: 16016 RVA: 0x0005045B File Offset: 0x0004E65B
			public User.UserGender? GetGender
			{
				get
				{
					return this._gender;
				}
			}

			// Token: 0x170005AA RID: 1450
			// (get) Token: 0x06003E91 RID: 16017 RVA: 0x00050463 File Offset: 0x0004E663
			public bool? GetCoppa
			{
				get
				{
					return this._coppa;
				}
			}

			// Token: 0x170005AB RID: 1451
			// (get) Token: 0x06003E92 RID: 16018 RVA: 0x0005046B File Offset: 0x0004E66B
			public string GetName
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x170005AC RID: 1452
			// (get) Token: 0x06003E93 RID: 16019 RVA: 0x00050473 File Offset: 0x0004E673
			public bool? GetNewsletter
			{
				get
				{
					return this._newsletter;
				}
			}

			// Token: 0x170005AD RID: 1453
			// (get) Token: 0x06003E94 RID: 16020 RVA: 0x0005047B File Offset: 0x0004E67B
			public string GetPassword
			{
				get
				{
					return this._password;
				}
			}

			// Token: 0x170005AE RID: 1454
			// (get) Token: 0x06003E95 RID: 16021 RVA: 0x00050483 File Offset: 0x0004E683
			public string GetWC
			{
				get
				{
					return this._wc;
				}
			}

			// Token: 0x170005AF RID: 1455
			// (get) Token: 0x06003E96 RID: 16022 RVA: 0x0005048B File Offset: 0x0004E68B
			public bool? GetSendSignUpEmail
			{
				get
				{
					return this._sendSignUpEmail;
				}
			}

			// Token: 0x170005B0 RID: 1456
			// (get) Token: 0x06003E97 RID: 16023 RVA: 0x00050493 File Offset: 0x0004E693
			public int GetUserId
			{
				get
				{
					return this._userId;
				}
			}

			// Token: 0x170005B1 RID: 1457
			// (get) Token: 0x06003E98 RID: 16024 RVA: 0x0005049B File Offset: 0x0004E69B
			public bool? GetEmailValid
			{
				get
				{
					return this._emailValid;
				}
			}

			// Token: 0x170005B2 RID: 1458
			// (get) Token: 0x06003E99 RID: 16025 RVA: 0x000504A3 File Offset: 0x0004E6A3
			public int GetPostedMsgCount
			{
				get
				{
					return this._postedMsgCount;
				}
			}

			// Token: 0x170005B3 RID: 1459
			// (get) Token: 0x06003E9A RID: 16026 RVA: 0x000504AB File Offset: 0x0004E6AB
			public int GetLastPostId
			{
				get
				{
					return this._lastPostId;
				}
			}

			// Token: 0x170005B4 RID: 1460
			// (get) Token: 0x06003E9B RID: 16027 RVA: 0x000504B3 File Offset: 0x0004E6B3
			public bool? GetValidated
			{
				get
				{
					return this._validated;
				}
			}

			// Token: 0x170005B5 RID: 1461
			// (get) Token: 0x06003E9C RID: 16028 RVA: 0x000504BB File Offset: 0x0004E6BB
			public DateTime? GetJoinDate
			{
				get
				{
					return this._joinDate;
				}
			}

			// Token: 0x170005B6 RID: 1462
			// (get) Token: 0x06003E9D RID: 16029 RVA: 0x000504C3 File Offset: 0x0004E6C3
			public DateTime? GetLastVisit
			{
				get
				{
					return this._lastVisit;
				}
			}

			// Token: 0x170005B7 RID: 1463
			// (get) Token: 0x06003E9E RID: 16030 RVA: 0x000504CB File Offset: 0x0004E6CB
			public string[] GetFeatures
			{
				get
				{
					return this._features;
				}
			}

			// Token: 0x170005B8 RID: 1464
			// (get) Token: 0x06003E9F RID: 16031 RVA: 0x000504D3 File Offset: 0x0004E6D3
			public User.BoardGame[] GetBoardGame
			{
				get
				{
					return this._boardGames;
				}
			}

			// Token: 0x170005B9 RID: 1465
			// (get) Token: 0x06003EA0 RID: 16032 RVA: 0x000504DB File Offset: 0x0004E6DB
			public User.OnlineGame[] GetOnlineGames
			{
				get
				{
					return this._onlineGames;
				}
			}

			// Token: 0x170005BA RID: 1466
			// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x000504E3 File Offset: 0x0004E6E3
			public PartnerAccount[] GetPartners
			{
				get
				{
					return this._partners;
				}
			}

			// Token: 0x06003EA2 RID: 16034 RVA: 0x000504EB File Offset: 0x0004E6EB
			public Builder()
			{
			}

			// Token: 0x06003EA3 RID: 16035 RVA: 0x001594C0 File Offset: 0x001576C0
			public Builder(ApiGetUserDetailsResponse raw)
			{
				this._userId = raw.data.user.user_id;
				this._loginName = ((raw.data.user.login_name == string.Empty) ? null : raw.data.user.login_name);
				this._country = ((raw.data.user.country == string.Empty) ? null : raw.data.user.country);
				this._emailValid = new bool?(raw.data.user.email_valid);
				this._language = (string.IsNullOrEmpty(raw.data.user.language) ? null : new User.UserLanguages?((User.UserLanguages)Enum.Parse(typeof(User.UserLanguages), raw.data.user.language)));
				this._timeZone = ((raw.data.user.time_zone == string.Empty) ? null : raw.data.user.time_zone);
				this._postedMsgCount = raw.data.user.posted_msg_count;
				this._lastPostId = raw.data.user.last_post_id;
				this._validated = new bool?(raw.data.user.validated);
				this._avatar = ((raw.data.user.avatar == string.Empty) ? null : raw.data.user.avatar);
				this._joinDate = (string.IsNullOrEmpty(raw.data.user.join_date) ? null : new DateTime?(DateTime.Parse(raw.data.user.join_date)));
				this._lastVisit = (string.IsNullOrEmpty(raw.data.user.last_visit) ? null : new DateTime?(DateTime.Parse(raw.data.user.last_visit)));
				this._zipcode = ((raw.data.user.zipcode == string.Empty) ? null : raw.data.user.zipcode);
				this._email = ((raw.data.user.email == string.Empty) ? null : raw.data.user.email);
				this._gender = (string.IsNullOrEmpty(raw.data.user.gender) ? null : new User.UserGender?((User.UserGender)Enum.Parse(typeof(User.UserGender), raw.data.user.gender)));
				this._coppa = new bool?(raw.data.user.coppa);
				this._name = ((raw.data.user.name == string.Empty) ? null : raw.data.user.name);
				if (raw.data.user.features != null)
				{
					this._features = raw.data.user.features.Clone() as string[];
				}
				if (raw.data.user.boardgames != null)
				{
					this._boardGames = raw.data.user.boardgames.Select((ApiGetUserDetailsResponse.Data.User.BoardGame x) => new User.BoardGame(x.code, x.name, string.IsNullOrEmpty(x.registered_date) ? null : new DateTime?(DateTime.Parse(x.registered_date)))).ToArray<User.BoardGame>();
				}
				if (raw.data.user.onlinegames != null)
				{
					this._onlineGames = raw.data.user.onlinegames.Select((ApiGetUserDetailsResponse.Data.User.OnlineGame x) => new User.OnlineGame(x.game, x.nbgames, x.karma, x.rankscore, x.rank, string.IsNullOrEmpty(x.lastgame) ? null : new DateTime?(DateTime.Parse(x.lastgame)), x.variant)).ToArray<User.OnlineGame>();
				}
				if (raw.data.user.partners != null)
				{
					this._partners = raw.data.user.partners.Select((ApiGetUserDetailsResponse.Data.User.Partners x) => new PartnerAccount(x.partner_id, x.partner_user_id, string.IsNullOrEmpty(x.created_at) ? null : new DateTime?(DateTime.Parse(x.created_at)))).ToArray<PartnerAccount>();
				}
				try
				{
					this._birthday = (string.IsNullOrEmpty(raw.data.user.birthday) ? null : new DateTime?(DateTime.Parse(raw.data.user.birthday)));
				}
				catch (FormatException)
				{
					this._birthday = null;
				}
			}

			// Token: 0x06003EA4 RID: 16036 RVA: 0x001599A0 File Offset: 0x00157BA0
			public override Builder<User>.BuilderErrors[] Validate()
			{
				List<Builder<User>.BuilderErrors> list = new List<Builder<User>.BuilderErrors>();
				if (this._loginName != null && (this._loginName.Length < 5 || this._loginName.Length > 50))
				{
					list.Add(new Builder<User>.BuilderErrors("LoginName", string.Format("User LoginName length must be not be less than {0} characters and more than {1} characters", 5, 50)));
				}
				if (this._loginName != null)
				{
					if (this._loginName.Any((char x) => "()#|@^*%§!?:;.,$~".Contains(x)))
					{
						list.Add(new Builder<User>.BuilderErrors("LoginName", string.Format("User LoginName cannot contain any character in the following set : '{0}'", "()#|@^*%§!?:;.,$~")));
					}
				}
				if (this._name != null && (this._name.Length < 5 || this._name.Length > 80))
				{
					list.Add(new Builder<User>.BuilderErrors("Name", string.Format("User Name length must be not be less than {0} characters and more than {1} characters", 5, 80)));
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
				return null;
			}

			// Token: 0x0400300A RID: 12298
			private string _email;

			// Token: 0x0400300B RID: 12299
			private string _loginName;

			// Token: 0x0400300C RID: 12300
			private string _password;

			// Token: 0x0400300D RID: 12301
			private string _name;

			// Token: 0x0400300E RID: 12302
			private User.UserGender? _gender;

			// Token: 0x0400300F RID: 12303
			private DateTime? _birthday;

			// Token: 0x04003010 RID: 12304
			private string _zipcode;

			// Token: 0x04003011 RID: 12305
			private string _country;

			// Token: 0x04003012 RID: 12306
			private User.UserLanguages? _language;

			// Token: 0x04003013 RID: 12307
			private string _timeZone;

			// Token: 0x04003014 RID: 12308
			private bool? _coppa;

			// Token: 0x04003015 RID: 12309
			private bool? _newsletter;

			// Token: 0x04003016 RID: 12310
			private string _avatar;

			// Token: 0x04003017 RID: 12311
			private bool? _sendSignUpEmail;

			// Token: 0x04003018 RID: 12312
			private string _wc;

			// Token: 0x04003019 RID: 12313
			private int _userId = -1;

			// Token: 0x0400301A RID: 12314
			private bool? _emailValid;

			// Token: 0x0400301B RID: 12315
			private int _postedMsgCount = -1;

			// Token: 0x0400301C RID: 12316
			private int _lastPostId = -1;

			// Token: 0x0400301D RID: 12317
			private bool? _validated;

			// Token: 0x0400301E RID: 12318
			private DateTime? _joinDate;

			// Token: 0x0400301F RID: 12319
			private DateTime? _lastVisit;

			// Token: 0x04003020 RID: 12320
			private string[] _features;

			// Token: 0x04003021 RID: 12321
			private User.BoardGame[] _boardGames;

			// Token: 0x04003022 RID: 12322
			private User.OnlineGame[] _onlineGames;

			// Token: 0x04003023 RID: 12323
			private PartnerAccount[] _partners;

			// Token: 0x04003024 RID: 12324
			public const int kLoginNameMinimalLength = 5;

			// Token: 0x04003025 RID: 12325
			public const int kLoginNameMaximalLength = 50;

			// Token: 0x04003026 RID: 12326
			public const int kNameMinimalLength = 5;

			// Token: 0x04003027 RID: 12327
			public const int kNameMaximalLength = 80;

			// Token: 0x04003028 RID: 12328
			public const string kLoginNameForbbidenCharacters = "()#|@^*%§!?:;.,$~";
		}
	}
}
