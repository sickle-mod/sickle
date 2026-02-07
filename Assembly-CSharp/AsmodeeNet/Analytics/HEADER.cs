using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200096F RID: 2415
	public struct HEADER
	{
		// Token: 0x0400313A RID: 12602
		public string api_key;

		// Token: 0x0400313B RID: 12603
		public string @event;

		// Token: 0x0400313C RID: 12604
		public string user_id;

		// Token: 0x0400313D RID: 12605
		public string device_id;

		// Token: 0x0400313E RID: 12606
		public string event_type;

		// Token: 0x0400313F RID: 12607
		public string time;

		// Token: 0x04003140 RID: 12608
		public string event_properties;

		// Token: 0x04003141 RID: 12609
		public string user_properties;

		// Token: 0x04003142 RID: 12610
		public string app_version;

		// Token: 0x04003143 RID: 12611
		public string platform;

		// Token: 0x04003144 RID: 12612
		public string os_name;

		// Token: 0x04003145 RID: 12613
		public string os_version;

		// Token: 0x04003146 RID: 12614
		public string device_model;

		// Token: 0x04003147 RID: 12615
		public string language;

		// Token: 0x04003148 RID: 12616
		public string ip;

		// Token: 0x04003149 RID: 12617
		public string event_id;

		// Token: 0x0400314A RID: 12618
		public string session_id;

		// Token: 0x0400314B RID: 12619
		public string version_build_number;

		// Token: 0x0400314C RID: 12620
		public string app_boot_session_id;

		// Token: 0x0400314D RID: 12621
		public string client_local_time;

		// Token: 0x0400314E RID: 12622
		public string first_party;

		// Token: 0x0400314F RID: 12623
		public long time_session;

		// Token: 0x04003150 RID: 12624
		public long time_session_gameplay;

		// Token: 0x04003151 RID: 12625
		public string screen_resolution;

		// Token: 0x04003152 RID: 12626
		public string unity_sdk_version;

		// Token: 0x04003153 RID: 12627
		public string backend_platform;

		// Token: 0x04003154 RID: 12628
		public string backend_user_id;

		// Token: 0x04003155 RID: 12629
		public string ua_platform;

		// Token: 0x04003156 RID: 12630
		public string ua_user_id;

		// Token: 0x04003157 RID: 12631
		public string ua_channel;

		// Token: 0x04003158 RID: 12632
		public string push_platform;

		// Token: 0x04003159 RID: 12633
		public string push_user_id;

		// Token: 0x0400315A RID: 12634
		public string user_id_first_party;

		// Token: 0x0400315B RID: 12635
		public int timezone_client;

		// Token: 0x0400315C RID: 12636
		public long time_ltd;

		// Token: 0x0400315D RID: 12637
		public long time_ltd_gameplay;

		// Token: 0x0400315E RID: 12638
		public string ab_test_group;

		// Token: 0x0400315F RID: 12639
		public int karma;

		// Token: 0x04003160 RID: 12640
		public int elo_rating;

		// Token: 0x04003161 RID: 12641
		public bool is_payer;

		// Token: 0x02000970 RID: 2416
		public enum environment
		{
			// Token: 0x04003163 RID: 12643
			dev,
			// Token: 0x04003164 RID: 12644
			prod
		}

		// Token: 0x02000971 RID: 2417
		public enum connection_type
		{
			// Token: 0x04003166 RID: 12646
			carrier_data_network,
			// Token: 0x04003167 RID: 12647
			local_area_network,
			// Token: 0x04003168 RID: 12648
			no_connection
		}
	}
}
