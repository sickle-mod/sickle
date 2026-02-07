using System;
using Scythe.UI;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200021B RID: 539
	public class ConnectionProblemsWindowActivator : MonoBehaviour
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x0008EA74 File Offset: 0x0008CC74
		private void Start()
		{
			ConnectionProblem.ConnectionProblemOccured += this.ShowErrorPanel;
			ConnectionProblem.Disconnected += this.Disconnected;
			ConnectionProblem.OutOfTime += this.RunOutOfTime;
			ReconnectManager.ShowReconnectError += this.ShowReconnectError;
			ReconnectManager.ShowReconnectPanel += this.ShowReconnectPanel;
			ReconnectManager.HideReconnectPanel += this.HideReconnectPanel;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0008EAE8 File Offset: 0x0008CCE8
		private void OnDestroy()
		{
			ConnectionProblem.ConnectionProblemOccured -= this.ShowErrorPanel;
			ConnectionProblem.Disconnected -= this.Disconnected;
			ConnectionProblem.OutOfTime -= this.RunOutOfTime;
			ReconnectManager.ShowReconnectError -= this.ShowReconnectError;
			ReconnectManager.ShowReconnectPanel -= this.ShowReconnectPanel;
			ReconnectManager.HideReconnectPanel -= this.HideReconnectPanel;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0008EB5C File Offset: 0x0008CD5C
		private void Update()
		{
			if (this.changeConnectionProblemPresenterState && !ReconnectManager.IsActive)
			{
				this.connectionProblemPresenter.ChangeActiveState();
				this.changeConnectionProblemPresenterState = false;
			}
			if (this.changeReconnectPresenterState)
			{
				this.reconnectPresenter.ChangeActiveState();
				this.changeReconnectPresenterState = false;
				return;
			}
			if (this.showReconnectError)
			{
				this.reconnectPresenter.ShowError();
				this.showReconnectError = false;
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0003247C File Offset: 0x0003067C
		private void ShowReconnectPanel()
		{
			this.changeReconnectPresenterState = true;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0003247C File Offset: 0x0003067C
		private void HideReconnectPanel()
		{
			this.changeReconnectPresenterState = true;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00032485 File Offset: 0x00030685
		private void ShowReconnectError()
		{
			this.showReconnectError = true;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003248E File Offset: 0x0003068E
		private void ShowErrorPanel()
		{
			this.changeConnectionProblemPresenterState = true;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00032497 File Offset: 0x00030697
		private void Disconnected()
		{
			this.connectionProblemPresenter.Disconnected();
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x000324A4 File Offset: 0x000306A4
		private void RunOutOfTime()
		{
			this.connectionProblemPresenter.OnRunOutOfTime();
		}

		// Token: 0x04000C44 RID: 3140
		[SerializeField]
		private ReconnectPresenter reconnectPresenter;

		// Token: 0x04000C45 RID: 3141
		[SerializeField]
		private ConnectionProblemPresenter connectionProblemPresenter;

		// Token: 0x04000C46 RID: 3142
		private bool changeConnectionProblemPresenterState;

		// Token: 0x04000C47 RID: 3143
		private bool changeReconnectPresenterState;

		// Token: 0x04000C48 RID: 3144
		private bool showReconnectError;
	}
}
