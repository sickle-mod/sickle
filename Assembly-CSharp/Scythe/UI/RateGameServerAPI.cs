using System;
using Newtonsoft.Json;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;

namespace Scythe.UI
{
	// Token: 0x020004EA RID: 1258
	public static class RateGameServerAPI
	{
		// Token: 0x06002871 RID: 10353 RVA: 0x00042328 File Offset: 0x00040528
		public static void SendReview(int starsAmount, Action<string> onSuccess, Action<Exception> onError)
		{
			RateGameServerAPI.SendReview(new Review(starsAmount, string.Empty), onSuccess, onError);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x0004233C File Offset: 0x0004053C
		public static void SendReview(int starsAmount, string feedback, Action<string> onSuccess, Action<Exception> onError)
		{
			RateGameServerAPI.SendReview(new Review(starsAmount, feedback), onSuccess, onError);
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000EA8D8 File Offset: 0x000E8AD8
		public static void GetReview(Action<Review> onSuccess, Action<Exception> onError)
		{
			string text = GameServiceController.Instance.PlayerId();
			RequestController.RequestGetCall(string.Format("{0}Review?reviewerId={1}", Uri.EscapeDataString("Reviews/"), text), delegate(string response)
			{
				if (string.IsNullOrEmpty(response))
				{
					onSuccess(null);
					return;
				}
				Review review = JsonConvert.DeserializeObject<Review>(response);
				onSuccess(review);
			}, onError);
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x0004234C File Offset: 0x0004054C
		private static void SendReview(Review review, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall("Reviews/Review", JsonConvert.SerializeObject(review), true, onSuccess, onError);
		}
	}
}
