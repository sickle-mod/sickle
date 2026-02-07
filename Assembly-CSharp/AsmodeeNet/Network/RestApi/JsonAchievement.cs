using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000898 RID: 2200
	[Serializable]
	public class JsonAchievement
	{
		// Token: 0x04002E5B RID: 11867
		public int id;

		// Token: 0x04002E5C RID: 11868
		public string tag;

		// Token: 0x04002E5D RID: 11869
		public string status;

		// Token: 0x04002E5E RID: 11870
		public string unicity;

		// Token: 0x04002E5F RID: 11871
		public string game;

		// Token: 0x04002E60 RID: 11872
		public string type;

		// Token: 0x04002E61 RID: 11873
		public bool secret;

		// Token: 0x04002E62 RID: 11874
		public int treasure;

		// Token: 0x04002E63 RID: 11875
		public int category;

		// Token: 0x04002E64 RID: 11876
		public string picture;

		// Token: 0x04002E65 RID: 11877
		public string ribbon;

		// Token: 0x04002E66 RID: 11878
		public JsonAchievement.Text[] texts;

		// Token: 0x02000899 RID: 2201
		[Serializable]
		public class Text
		{
			// Token: 0x04002E67 RID: 11879
			public string lang;

			// Token: 0x04002E68 RID: 11880
			public string name;

			// Token: 0x04002E69 RID: 11881
			public string description;
		}
	}
}
