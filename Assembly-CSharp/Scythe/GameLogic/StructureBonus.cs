using System;
using System.Collections.Generic;

namespace Scythe.GameLogic
{
	// Token: 0x020005EF RID: 1519
	public class StructureBonus
	{
		// Token: 0x17000377 RID: 887
		public int this[int index]
		{
			get
			{
				if (index <= 0)
				{
					return 0;
				}
				if (index >= this.bonusList.Count)
				{
					return this.bonusList[this.bonusList.Count - 1];
				}
				return this.bonusList[index - 1];
			}
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000462A7 File Offset: 0x000444A7
		public int HighestBonus()
		{
			return this.bonusList[this.bonusList.Count - 1];
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x001269C0 File Offset: 0x00124BC0
		public StructureBonus(params int[] bonuses)
		{
			foreach (int num in bonuses)
			{
				this.bonusList.Add(num);
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000462C1 File Offset: 0x000444C1
		public int BonusListCount()
		{
			return this.bonusList.Count;
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x00126A00 File Offset: 0x00124C00
		public override string ToString()
		{
			string text = string.Empty;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 1;
			for (int i = 0; i < this.bonusList.Count + 1; i++)
			{
				if (i == this.bonusList.Count || num != this.bonusList[i])
				{
					if (num != 0)
					{
						text += (num2 + 1).ToString();
						if (num2 != num3)
						{
							text = text + " - " + (num3 + 1).ToString();
						}
						text = string.Concat(new string[]
						{
							text,
							": <color=",
							this.GetColorForDescription(num4++),
							">",
							this.AsciCoin(num).ToString(),
							"</color>"
						});
						if (i < this.bonusList.Count)
						{
							text += "    ";
						}
					}
					if (i < this.bonusList.Count)
					{
						num = this.bonusList[i];
						num2 = i;
					}
				}
				num3 = i;
			}
			return text;
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x00126B20 File Offset: 0x00124D20
		private string GetColorForDescription(int tier)
		{
			int num = this.NumberOfTiers();
			if (tier == 1)
			{
				return "#F82424FF";
			}
			if (tier == 2 && num == 4)
			{
				return "#F66110FF";
			}
			if ((tier == 2 && num == 3) || (tier == 3 && num == 4))
			{
				return "#F6C010FF";
			}
			if (tier == num)
			{
				return "#0BFF2BFF";
			}
			return "\"white\"";
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x00126B74 File Offset: 0x00124D74
		private char AsciCoin(int n)
		{
			switch (n)
			{
			case 1:
				return '➊';
			case 2:
				return '➋';
			case 3:
				return '➌';
			case 4:
				return '➍';
			case 5:
				return '➎';
			case 6:
				return '➏';
			case 7:
				return '➐';
			case 8:
				return '➑';
			case 9:
				return '➒';
			case 10:
				return '➓';
			default:
				return '⓿';
			}
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x00126BF4 File Offset: 0x00124DF4
		public int NumberOfTiers()
		{
			int num = 0;
			int num2 = 0;
			foreach (int num3 in this.bonusList)
			{
				if (num2 < num3)
				{
					num++;
					num2 = num3;
				}
			}
			return num;
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x00126C50 File Offset: 0x00124E50
		public int TierForValue(int value)
		{
			if (value == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			foreach (int num3 in this.bonusList)
			{
				if (num2 < num3)
				{
					num++;
					num2 = num3;
					if (value == num3)
					{
						return num;
					}
				}
			}
			return num;
		}

		// Token: 0x040020D8 RID: 8408
		private List<int> bonusList = new List<int>();
	}
}
