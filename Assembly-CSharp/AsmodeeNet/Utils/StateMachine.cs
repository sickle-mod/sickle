using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000853 RID: 2131
	[Serializable]
	public class StateMachine
	{
		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x0004EE4D File Offset: 0x0004D04D
		// (set) Token: 0x06003C1B RID: 15387 RVA: 0x0004EE55 File Offset: 0x0004D055
		public bool FirstUpdate { get; set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06003C1C RID: 15388 RVA: 0x0004EE5E File Offset: 0x0004D05E
		// (set) Token: 0x06003C1D RID: 15389 RVA: 0x0004EE66 File Offset: 0x0004D066
		public string FSMName { get; set; }

		// Token: 0x06003C1E RID: 15390 RVA: 0x0004EE6F File Offset: 0x0004D06F
		public StateMachine(string fsmName)
		{
			this.FSMName = fsmName;
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x001549D0 File Offset: 0x00152BD0
		public void Update()
		{
			if (!this.Enabled)
			{
				return;
			}
			if (this.CurrentState.ActionUpdate != null)
			{
				this.CurrentState.ActionUpdate();
			}
			this.FirstUpdate = false;
			foreach (Transition transition in this._listTransition)
			{
				if ((transition.ActionStateStart == this.CurrentState || (transition.ActionStateEnd != this.CurrentState && transition.ActionStateStart == null)) && transition.Condition())
				{
					if (this.IsDebug)
					{
						Debug.Log(string.Concat(new string[]
						{
							"FSM ",
							this.FSMName,
							" : ",
							this.CurrentState.Name,
							" -> ",
							transition.ActionStateEnd.Name
						}));
					}
					if (transition.TransitionType == TransitionType.WithDuration)
					{
						if (this.CurrentState.ActionExit != null)
						{
							this.CurrentState.ActionExit();
						}
						this.PreviousState = transition.ActionStateStart;
						this.CurrentState = transition.ActionStateEnd;
						this.FirstUpdate = true;
						if (this.OnStateChanged != null)
						{
							this.OnStateChanged();
						}
						if (this.CurrentState.ActionEnter != null)
						{
							this.CurrentState.ActionEnter();
							break;
						}
						break;
					}
					else
					{
						if (this.CurrentState.ActionExit != null)
						{
							this.CurrentState.ActionExit();
						}
						this.PreviousState = transition.ActionStateStart;
						this.CurrentState = transition.ActionStateEnd;
						this.FirstUpdate = true;
						if (this.OnStateChanged != null)
						{
							this.OnStateChanged();
						}
						if (this.CurrentState.ActionEnter != null)
						{
							this.CurrentState.ActionEnter();
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x0004EE9B File Offset: 0x0004D09B
		public ActionState AddActionState(string name)
		{
			return this.AddActionState(name, null, null, null);
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x0004EEA7 File Offset: 0x0004D0A7
		public ActionState AddActionState(string name, Action actionEnter)
		{
			return this.AddActionState(name, actionEnter, null, null);
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x00154BD8 File Offset: 0x00152DD8
		public ActionState AddActionState(string name, Action actionEnter, Action actionUpdate, Action actionExit)
		{
			ActionState actionState = new ActionState(name, actionEnter, actionUpdate, actionExit);
			this._listState.Add(actionState);
			return actionState;
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x0004EEB3 File Offset: 0x0004D0B3
		public Transition AddTransition(ActionState actionStateEnd, Func<bool> condition)
		{
			return this.AddTransition(null, actionStateEnd, condition);
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x00154C00 File Offset: 0x00152E00
		public Transition AddTransition(ActionState actionStateStart, ActionState actionStateEnd, Func<bool> condition)
		{
			Transition transition = new Transition(actionStateStart, actionStateEnd, condition, TransitionType.Normal);
			this._listTransition.Add(transition);
			return transition;
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x00154C24 File Offset: 0x00152E24
		public Transition AddTransition(ActionState actionStateStart, ActionState actionStateEnd, float transitionDuration)
		{
			Transition transition = new Transition(actionStateStart, actionStateEnd, transitionDuration, TransitionType.WithDuration);
			this._listTransition.Add(transition);
			return transition;
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x0004EEBE File Offset: 0x0004D0BE
		public void Reset()
		{
			this._listState = new List<ActionState>();
			this._listTransition = new List<Transition>();
		}

		// Token: 0x04002D9A RID: 11674
		private List<ActionState> _listState = new List<ActionState>();

		// Token: 0x04002D9B RID: 11675
		private List<Transition> _listTransition = new List<Transition>();

		// Token: 0x04002D9C RID: 11676
		public ActionState CurrentState;

		// Token: 0x04002D9D RID: 11677
		public ActionState PreviousState;

		// Token: 0x04002D9E RID: 11678
		public bool IsDebug;

		// Token: 0x04002DA1 RID: 11681
		public Action OnStateChanged;

		// Token: 0x04002DA2 RID: 11682
		public bool Enabled = true;
	}
}
