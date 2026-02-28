using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020005B5 RID: 1461
	public class EncounterCard : Card
	{
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06002E77 RID: 11895
		// (set) Token: 0x06002E78 RID: 11896
		public int AmountChoosen { get; private set; }

		// Token: 0x06002E79 RID: 11897
		public EncounterCard(int cardId, GameManager gameManager)
		{
			this.cardId = cardId;
			this.AmountChoosen = 0;
			this.gameManager = gameManager;
			this.SetSections();
		}

		// Token: 0x06002E7A RID: 11898
		public void SetPlayer(Player player)
		{
			foreach (SectionAction sectionAction in this.actions)
			{
				sectionAction.SetPlayer(player);
			}
		}

		// Token: 0x06002E7B RID: 11899
		public bool CanExecute(int action)
		{
			return this.actions[action].GetNumberOfPayActions() == 0 || this.actions[action].GetPayAction(0).CanPlayerPay();
		}

		// Token: 0x06002E7C RID: 11900
		public SectionAction GetAction(int id)
		{
			this.currentAction = id;
			return this.actions[id];
		}

		// Token: 0x06002E7D RID: 11901
		public SectionAction GetCurrentAction()
		{
			return this.actions[this.currentAction];
		}

		// Token: 0x06002E7E RID: 11902
		public int GetCurrentActionId()
		{
			return this.currentAction;
		}

		// Token: 0x06002E7F RID: 11903
		public void ChooseOption()
		{
			int num = this.AmountChoosen + 1;
			this.AmountChoosen = num;
		}

		// Token: 0x06002E80 RID: 11904
		public void FinishAction()
		{
			this.previousAction = this.currentAction;
			this.currentAction = -1;
		}

		// Token: 0x06002E81 RID: 11905
		public int GetPreviousActionId()
		{
			return this.previousAction;
		}

		// Token: 0x06002E82 RID: 11906
		private void SetSections()
		{
			switch (this.cardId)
			{
			case 1:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				return;
			case 2:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCombatCard(this.gameManager, 1, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.wood, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainRecruit(this.gameManager, 1, 0, true)
				}));
				return;
			case 3:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.food, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				return;
			case 4:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.metal, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainRecruit(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.metal, 4, 0, true)
				}));
				return;
			case 5:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 4, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainResource(this.gameManager, ResourceType.metal, 2, 0, true)
				}));
				return;
			case 6:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainRecruit(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 4, 0, true)
				}));
				return;
			case 7:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainUpgrade(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 4, 0, true)
				}));
				return;
			case 8:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainRecruit(this.gameManager, 1, 0, true)
				}));
				return;
			case 9:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 4, 0, true)
				}));
				return;
			case 10:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 4, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainAnyResource(this.gameManager, 2, 0, true)
				}));
				return;
			case 11:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.food, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				return;
			case 12:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.wood, 3, 0, true)
				}));
				return;
			case 13:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainAnyResource(this.gameManager, 5, 0, true)
				}));
				return;
			case 14:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainAnyResource(this.gameManager, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.metal, 3, 0, true)
				}));
				return;
			case 15:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainPopularity(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainAnyResource(this.gameManager, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainAnyResource(this.gameManager, 5, 0, true)
				}));
				return;
			case 16:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainAnyResource(this.gameManager, 2, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainRecruit(this.gameManager, 1, 0, true)
				}));
				return;
			case 17:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCombatCard(this.gameManager, 1, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainCombatCard(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				return;
			case 18:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainAnyResource(this.gameManager, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.wood, 3, 0, true)
				}));
				return;
			case 19:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainCombatCard(this.gameManager, 2, 0, true, false, false),
					new GainPower(this.gameManager, 3, 0, true, false, false)
				}));
				return;
			case 20:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCombatCard(this.gameManager, 1, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPopularity(this.gameManager, 3, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.wood, 4, 0, true)
				}));
				return;
			case 21:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainPopularity(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 4, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainCombatCard(this.gameManager, 2, 0, true, false, false),
					new GainPower(this.gameManager, 3, 0, true, false, false)
				}));
				return;
			case 22:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.metal, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				return;
			case 23:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainRecruit(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.food, 3, 0, true)
				}));
				return;
			case 24:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCombatCard(this.gameManager, 1, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainAnyResource(this.gameManager, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				return;
			case 25:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 4, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainUpgrade(this.gameManager, 1, 0, true),
					new GainAnyResource(this.gameManager, 2, 0, true)
				}));
				return;
			case 26:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPopularity(this.gameManager, 3, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				return;
			case 27:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.metal, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				return;
			case 28:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainAnyResource(this.gameManager, 2, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				return;
			case 29:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainUpgrade(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 3, 0, true),
					new GainResource(this.gameManager, ResourceType.food, 1, 0, true)
				}));
				return;
			case 30:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 4, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainBuilding(this.gameManager, 1, 0, true)
				}));
				return;
			case 31:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.wood, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPopularity(this.gameManager, 3, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPeekCombatCards(this.gameManager, 1, 0, true),
					new GainCombatCard(this.gameManager, 3, 0, true, false, false)
				}));
				return;
			case 32:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainCombatCard(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainUpgrade(this.gameManager, 1, 0, true),
					new GainAnyResource(this.gameManager, 2, 0, true)
				}));
				return;
			case 33:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainCombatCard(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainUpgrade(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.food, 2, 0, true)
				}));
				return;
			case 34:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.metal, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainRecruit(this.gameManager, 1, 0, true)
				}));
				return;
			case 35:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainPopularity(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.wood, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.oil, 3, 0, true)
				}));
				return;
			case 36:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 2, 0, true),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 4, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainWorker(this.gameManager, 1, 0, true),
					new GainResource(this.gameManager, ResourceType.oil, 3, 0, true)
				}));
				return;
			case 37:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainCombatCard(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 4, 0, true)
				}));
				return;
			case 38:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.oil, 3, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayResource(this.gameManager, true, 2, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 1, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				return;
			case 39:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainCombatCard(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.food, 4, 0, true)
				}));
				return;
			case 40:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.wood, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainResource(this.gameManager, ResourceType.metal, 4, 0, true)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainMech(this.gameManager, 1, 0, true)
				}));
				return;
			case 41:
				this.actions.Add(new SectionAction(this.gameManager, (PayAction)null, new GainAction[]
				{
					new GainCoin(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 1, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayCoin(this.gameManager, 2, 0, false, true), new GainAction[]
				{
					new GainPower(this.gameManager, 2, 0, true, false, false),
					new GainPopularity(this.gameManager, 2, 0, true, false, false)
				}));
				this.actions.Add(new SectionAction(this.gameManager, new PayPopularity(this.gameManager, 3, 0, false, true), new GainAction[]
				{
					new GainAnyResource(this.gameManager, 5, 0, true)
				}));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002E83 RID: 11907
		public override void ReadXml(XmlReader reader)
		{
			if (reader.GetAttribute("Current") != null || reader.GetAttribute("Previous") != null)
			{
				this.cardId = int.Parse(reader.GetAttribute("Id"));
				this.AmountChoosen = int.Parse(reader.GetAttribute("Amount"));
				this.SetSections();
				this.currentAction = int.Parse(reader.GetAttribute("Current"));
				if (reader.GetAttribute("Previous") != null)
				{
					this.previousAction = int.Parse(reader.GetAttribute("Previous"));
				}
				if (this.currentAction != -1)
				{
					int numberOfPayActions = this.actions[this.currentAction].GetNumberOfPayActions();
					int numberOfGainActions = this.actions[this.currentAction].GetNumberOfGainActions();
					reader.ReadStartElement();
					if (reader.Name == "Action")
					{
						for (int i = 0; i < numberOfPayActions; i++)
						{
							reader.ReadStartElement();
							((IXmlSerializable)this.actions[this.currentAction].GetPayAction(i)).ReadXml(reader);
						}
						for (int j = 0; j < numberOfGainActions; j++)
						{
							reader.ReadStartElement();
							((IXmlSerializable)this.actions[this.currentAction].GetGainAction(j)).ReadXml(reader);
						}
						if (this.actions[this.currentAction].GetGainAction(numberOfGainActions - 1).GetGainType() != GainType.AnyResource)
						{
							reader.ReadStartElement();
						}
						reader.ReadEndElement();
						return;
					}
				}
			}
			else
			{
				base.ReadXml(reader);
				reader.Read();
				this.SetSections();
			}
		}

		// Token: 0x06002E84 RID: 11908
		public override void WriteXml(XmlWriter writer)
		{
			if (this.currentAction != -1 || this.previousAction != -1)
			{
				writer.WriteAttributeString("Id", this.cardId.ToString());
				writer.WriteAttributeString("Current", this.currentAction.ToString());
				writer.WriteAttributeString("Amount", this.AmountChoosen.ToString());
				if (this.previousAction != -1)
				{
					writer.WriteAttributeString("Previous", this.previousAction.ToString());
				}
				if (this.currentAction != -1)
				{
					writer.WriteStartElement("Action");
					int numberOfPayActions = this.actions[this.currentAction].GetNumberOfPayActions();
					int numberOfGainActions = this.actions[this.currentAction].GetNumberOfGainActions();
					for (int i = 0; i < numberOfPayActions; i++)
					{
						writer.WriteStartElement("Pay");
						((IXmlSerializable)this.actions[this.currentAction].GetPayAction(i)).WriteXml(writer);
						writer.WriteEndElement();
					}
					for (int j = 0; j < numberOfGainActions; j++)
					{
						writer.WriteStartElement("Gain");
						((IXmlSerializable)this.actions[this.currentAction].GetGainAction(j)).WriteXml(writer);
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
					return;
				}
			}
			else
			{
				base.WriteXml(writer);
			}
		}

		// Token: 0x04001F5B RID: 8027
		private List<SectionAction> actions = new List<SectionAction>();

		// Token: 0x04001F5C RID: 8028

		private int currentAction = -1;

		// Token: 0x04001F5D RID: 8029
		private int previousAction = -1;

		// Token: 0x04001F5F RID: 8031
		private GameManager gameManager;
	}
}
