using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007AA RID: 1962
	public abstract class RequiredOnlineStatus
	{
		// Token: 0x06003893 RID: 14483
		public abstract void MeetRequirements(Action onSuccess, Action onFailure);

		// Token: 0x06003894 RID: 14484 RVA: 0x0004C5C7 File Offset: 0x0004A7C7
		public virtual void SetCallbacks(Action onSuccess, Action onFailure)
		{
			this._onSuccess = onSuccess;
			this._onFailure = onFailure;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0004C5D7 File Offset: 0x0004A7D7
		protected void CallOnSuccess()
		{
			AsmoLogger.Debug(base.GetType().Name, "Success", null);
			if (this._onSuccess != null)
			{
				this._onSuccess();
			}
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0004C602 File Offset: 0x0004A802
		protected void CallOnFailure()
		{
			AsmoLogger.Warning(base.GetType().Name, "Failure", null);
			if (this._onFailure != null)
			{
				this._onFailure();
			}
		}

		// Token: 0x04002A82 RID: 10882
		private Action _onSuccess;

		// Token: 0x04002A83 RID: 10883
		private Action _onFailure;
	}
}
