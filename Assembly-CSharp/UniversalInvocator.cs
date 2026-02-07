using System;

// Token: 0x02000170 RID: 368
public static class UniversalInvocator
{
	// Token: 0x06000A80 RID: 2688 RVA: 0x0002F350 File Offset: 0x0002D550
	public static void Event_Invocator(Action action)
	{
		if (action != null)
		{
			action();
		}
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0002F35B File Offset: 0x0002D55B
	public static void Event_Invocator<T1>(Action<T1> action, params object[] parameters)
	{
		if (action != null)
		{
			action((T1)((object)parameters[0]));
		}
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0002F36E File Offset: 0x0002D56E
	public static void Event_Invocator<T1, T2>(Action<T1, T2> action, params object[] parameters)
	{
		if (action != null)
		{
			action((T1)((object)parameters[0]), (T2)((object)parameters[1]));
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0002F389 File Offset: 0x0002D589
	public static void Event_Invocator<T1, T2, T3>(Action<T1, T2, T3> action, params object[] parameters)
	{
		if (action != null)
		{
			action((T1)((object)parameters[0]), (T2)((object)parameters[1]), (T3)((object)parameters[2]));
		}
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0002F3AC File Offset: 0x0002D5AC
	public static void Event_Invocator<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, params object[] parameters)
	{
		if (action != null)
		{
			action((T1)((object)parameters[0]), (T2)((object)parameters[1]), (T3)((object)parameters[2]), (T4)((object)parameters[3]));
		}
	}
}
