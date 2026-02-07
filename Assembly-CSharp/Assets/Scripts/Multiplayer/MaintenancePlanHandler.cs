using System;
using I2.Loc;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Multiplayer
{
	// Token: 0x020001AC RID: 428
	public class MaintenancePlanHandler : MonoBehaviour
	{
		// Token: 0x06000C80 RID: 3200 RVA: 0x00030534 File Offset: 0x0002E734
		public void ClosePopup()
		{
			this.serverInfoPopup.SetActive(false);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00030542 File Offset: 0x0002E742
		private void Start()
		{
			if (!ServerInfoPlanDisplayed.GetInstance().InfoPlanDisplayed)
			{
				this.CheckIfMaintenancePlanned();
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00030556 File Offset: 0x0002E756
		private void CheckIfMaintenancePlanned()
		{
			RequestController.RequestGetCallForAzureFunction("MaintenancePlanned?", delegate(string response)
			{
				ServerMaintenancePlan serverMaintenancePlan = GameSerializer.JsonMessageDeserializerWithStringDeserialization<ServerMaintenancePlan>(response);
				if (serverMaintenancePlan.IsPlanned)
				{
					this.FormatAndChangeServerInfoPopupText(serverMaintenancePlan.StartDateTime, serverMaintenancePlan.EndDateTime);
					this.serverInfoPopup.SetActive(true);
					ServerInfoPlanDisplayed.GetInstance().InfoPlanDisplayed = true;
					return;
				}
				ServerInfoPlanDisplayed.GetInstance().InfoPlanDisplayed = false;
				this.serverInfoPopup.SetActive(false);
			}, delegate(Exception er)
			{
				Debug.LogError(er);
			});
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00080E50 File Offset: 0x0007F050
		private void FormatAndChangeServerInfoPopupText(DateTime startDateTime, DateTime endDateTime)
		{
			string text = string.Format("{0}", startDateTime.ToString("dd.MM.yyyy hh:mm"));
			string text2 = string.Format("{0}", endDateTime.ToString("hh:mm"));
			this.serverInfoPopupText.text = string.Format("{0}{1}{2} - {3} GMT", new object[]
			{
				ScriptLocalization.Get("MainMenu/ServerMaintenancePlan"),
				Environment.NewLine,
				text,
				text2
			});
		}

		// Token: 0x040009DB RID: 2523
		[SerializeField]
		private GameObject serverInfoPopup;

		// Token: 0x040009DC RID: 2524
		[SerializeField]
		private TextMeshProUGUI serverInfoPopupText;
	}
}
