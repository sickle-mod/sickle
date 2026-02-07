using System;

namespace ScytheWebRole.ServerLogic
{
	// Token: 0x0200019F RID: 415
	public class PlayerRating
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0003022D File Offset: 0x0002E42D
		// (set) Token: 0x06000C36 RID: 3126 RVA: 0x00030235 File Offset: 0x0002E435
		public Guid Id { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0003023E File Offset: 0x0002E43E
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x00030246 File Offset: 0x0002E446
		public int Rating { get; private set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0003024F File Offset: 0x0002E44F
		// (set) Token: 0x06000C3A RID: 3130 RVA: 0x00030257 File Offset: 0x0002E457
		public double NewRating { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00030260 File Offset: 0x0002E460
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x00030268 File Offset: 0x0002E468
		public int OldRating { get; private set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00030271 File Offset: 0x0002E471
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x00030279 File Offset: 0x0002E479
		public bool IsProvisional { get; private set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00030282 File Offset: 0x0002E482
		public bool IsRanked
		{
			get
			{
				return !this.IsProvisional;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0003028D File Offset: 0x0002E48D
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00030295 File Offset: 0x0002E495
		public int Place { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0003029E File Offset: 0x0002E49E
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x000302A6 File Offset: 0x0002E4A6
		public int NumberOfGamesPlayed { get; private set; }

		// Token: 0x06000C44 RID: 3140 RVA: 0x000302AF File Offset: 0x0002E4AF
		public PlayerRating(Guid id)
		{
			this.Id = id;
			this.IsProvisional = true;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x000302C5 File Offset: 0x0002E4C5
		public PlayerRating(Guid id, int place)
		{
			this.Id = id;
			this.Place = place;
			this.IsProvisional = true;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x000302E2 File Offset: 0x0002E4E2
		public void SetPlace(int newPlace)
		{
			this.Place = newPlace;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x000302EB File Offset: 0x0002E4EB
		public void IncreaseRating(int amount)
		{
			this.Rating += amount;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000302FB File Offset: 0x0002E4FB
		public void UpdateData(int rating, int numberOfGamesPlayed)
		{
			this.Rating = rating;
			this.OldRating = rating;
			this.NewRating = 0.0;
			this.NumberOfGamesPlayed = numberOfGamesPlayed;
			if ((double)numberOfGamesPlayed >= 6.0)
			{
				this.IsProvisional = false;
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00030335 File Offset: 0x0002E535
		public void IncreaseNewRating(double value)
		{
			this.NewRating += value;
			this.ratingChanged++;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000809B0 File Offset: 0x0007EBB0
		public void ApplyChange()
		{
			if (this.ratingChanged == 0)
			{
				return;
			}
			this.OldRating = this.Rating;
			this.Rating = (int)Math.Round(this.NewRating / (double)this.ratingChanged);
			this.ratingChanged = 0;
			this.NewRating = 0.0;
		}

		// Token: 0x040009B3 RID: 2483
		private int ratingChanged;
	}
}
