using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x020009D0 RID: 2512
	public class ApplicationAnalyticsContext : AnalyticsContext
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x0005224D File Offset: 0x0005044D
		// (set) Token: 0x060041C9 RID: 16841 RVA: 0x00052255 File Offset: 0x00050455
		public string AppBootSessionId { get; private set; }

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x0005225E File Offset: 0x0005045E
		public int TimeSession
		{
			get
			{
				return (int)(Time.unscaledTime - this._startApplicationTime);
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x0005226D File Offset: 0x0005046D
		public int TimeSessionGamePlay
		{
			get
			{
				return (int)(this._finishedTimeSessionGameplay + this._RunningTimeSessionGameplay);
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x0005227D File Offset: 0x0005047D
		public int TimeLifeToDate
		{
			get
			{
				return KeyValueStore.GetInt("CumulatedTimeFinishedSession", 0) + this.TimeSession;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x00052291 File Offset: 0x00050491
		public int TimeLifeToDateGameplay
		{
			get
			{
				return KeyValueStore.GetInt("CumulatedTimeGameplayFinishedSession", 0) + this.TimeSessionGamePlay;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x00162724 File Offset: 0x00160924
		private float _RunningTimeSessionGameplay
		{
			get
			{
				return (float)((this._startGameplayTime != null) ? ((int)(Time.unscaledTime - this._startGameplayTime).Value) : 0);
			}
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x0016277C File Offset: 0x0016097C
		public ApplicationAnalyticsContext()
		{
			this.AppBootSessionId = Guid.NewGuid().ToString();
			this._startApplicationTime = Time.unscaledTime;
			int @int = KeyValueStore.GetInt("TimePreviousSession", 0);
			KeyValueStore.SetInt("CumulatedTimeFinishedSession", KeyValueStore.GetInt("CumulatedTimeFinishedSession", 0) + @int);
			int int2 = KeyValueStore.GetInt("TimeGameplayPreviousSession", 0);
			KeyValueStore.SetInt("CumulatedTimeGameplayFinishedSession", KeyValueStore.GetInt("CumulatedTimeGameplayFinishedSession", 0) + int2);
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x000522A5 File Offset: 0x000504A5
		public override void Pause()
		{
			base.Pause();
			this._SaveLifeToDateInfo();
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x001627FC File Offset: 0x001609FC
		public override void Resume()
		{
			base.Resume();
			float num = Time.unscaledTime - this._startPauseTime;
			this._startApplicationTime += num;
			if (this._startGameplayTime != null)
			{
				this._startGameplayTime += num;
			}
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x000522B3 File Offset: 0x000504B3
		public override void Quit()
		{
			base.Quit();
			this._SaveLifeToDateInfo();
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x000522C1 File Offset: 0x000504C1
		private void _SaveLifeToDateInfo()
		{
			KeyValueStore.SetInt("TimePreviousSession", this.TimeSession);
			KeyValueStore.SetInt("TimeGameplayPreviousSession", this.TimeSessionGamePlay);
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x000522E3 File Offset: 0x000504E3
		public void StartGameplay()
		{
			this._startGameplayTime = new float?(Time.unscaledTime);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x000522F5 File Offset: 0x000504F5
		public void StopGameplay()
		{
			this._finishedTimeSessionGameplay += this._RunningTimeSessionGameplay;
			this._startGameplayTime = null;
		}

		// Token: 0x040032E4 RID: 13028
		private const string _kTimePreviousSession = "TimePreviousSession";

		// Token: 0x040032E5 RID: 13029
		private const string _kCumulatedTimeFinishedSession = "CumulatedTimeFinishedSession";

		// Token: 0x040032E6 RID: 13030
		private const string _kTimeGameplayPreviousSession = "TimeGameplayPreviousSession";

		// Token: 0x040032E7 RID: 13031
		private const string _kCumulatedTimeGameplayFinishedSession = "CumulatedTimeGameplayFinishedSession";

		// Token: 0x040032E9 RID: 13033
		private float _startApplicationTime;

		// Token: 0x040032EA RID: 13034
		private float? _startGameplayTime;

		// Token: 0x040032EB RID: 13035
		private float _finishedTimeSessionGameplay;
	}
}
