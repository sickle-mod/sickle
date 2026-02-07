using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000855 RID: 2133
	[Serializable]
	public class Transition
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06003C30 RID: 15408 RVA: 0x0004EF3F File Offset: 0x0004D13F
		// (set) Token: 0x06003C31 RID: 15409 RVA: 0x0004EF47 File Offset: 0x0004D147
		public ActionState ActionStateStart { get; set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06003C32 RID: 15410 RVA: 0x0004EF50 File Offset: 0x0004D150
		// (set) Token: 0x06003C33 RID: 15411 RVA: 0x0004EF58 File Offset: 0x0004D158
		public ActionState ActionStateEnd { get; set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06003C34 RID: 15412 RVA: 0x0004EF61 File Offset: 0x0004D161
		// (set) Token: 0x06003C35 RID: 15413 RVA: 0x0004EF69 File Offset: 0x0004D169
		public Func<bool> Condition { get; set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06003C36 RID: 15414 RVA: 0x0004EF72 File Offset: 0x0004D172
		// (set) Token: 0x06003C37 RID: 15415 RVA: 0x0004EF7A File Offset: 0x0004D17A
		public TransitionType TransitionType { get; set; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x0004EF83 File Offset: 0x0004D183
		// (set) Token: 0x06003C39 RID: 15417 RVA: 0x0004EF8B File Offset: 0x0004D18B
		public float TransitionDuration { get; set; }

		// Token: 0x06003C3A RID: 15418 RVA: 0x0004EF94 File Offset: 0x0004D194
		public Transition(ActionState actionStateStart, ActionState actionStateEnd, Func<bool> condition, TransitionType transitionType)
		{
			this.ActionStateStart = actionStateStart;
			this.ActionStateEnd = actionStateEnd;
			this.Condition = condition;
			this.TransitionType = transitionType;
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x0004EFB9 File Offset: 0x0004D1B9
		public Transition(ActionState actionStateStart, ActionState actionStateEnd, float transitionDuration, TransitionType transitionType)
		{
			this.ActionStateStart = actionStateStart;
			this.ActionStateEnd = actionStateEnd;
			this.TransitionDuration = transitionDuration;
			this.TransitionType = transitionType;
		}
	}
}
