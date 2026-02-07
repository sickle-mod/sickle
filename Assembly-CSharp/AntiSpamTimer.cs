using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class AntiSpamTimer
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0002E1AF File Offset: 0x0002C3AF
	// (set) Token: 0x060008F1 RID: 2289 RVA: 0x0002E1B7 File Offset: 0x0002C3B7
	public int AllowedCallsNumber { get; protected set; }

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0002E1C0 File Offset: 0x0002C3C0
	// (set) Token: 0x060008F3 RID: 2291 RVA: 0x0002E1C8 File Offset: 0x0002C3C8
	public float RequiredInterval { get; protected set; }

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0002E1D1 File Offset: 0x0002C3D1
	// (set) Token: 0x060008F5 RID: 2293 RVA: 0x0002E1D9 File Offset: 0x0002C3D9
	public float SpamPenaltyDuration { get; protected set; }

	// Token: 0x060008F6 RID: 2294 RVA: 0x0007A7CC File Offset: 0x000789CC
	public AntiSpamTimer(int allowedCallsNumber, float requiredInterval, float spamPenaltyDuration)
	{
		this.AllowedCallsNumber = allowedCallsNumber;
		this.RequiredInterval = requiredInterval;
		this.SpamPenaltyDuration = spamPenaltyDuration;
		this.callsTimes = new float[this.AllowedCallsNumber];
		for (int i = 0; i < this.callsTimes.Length; i++)
		{
			this.callsTimes[i] = -this.RequiredInterval;
		}
		this.currentIndex = 0;
		this.spamProtectedTime = 0f;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0007A83C File Offset: 0x00078A3C
	public bool MakeACall()
	{
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime < this.spamProtectedTime)
		{
			return false;
		}
		if (unscaledTime - this.callsTimes[this.currentIndex] < this.RequiredInterval)
		{
			this.spamProtectedTime = unscaledTime + this.SpamPenaltyDuration;
			return false;
		}
		this.callsTimes[this.currentIndex] = unscaledTime;
		this.currentIndex++;
		if (this.currentIndex >= this.callsTimes.Length)
		{
			this.currentIndex = 0;
		}
		return true;
	}

	// Token: 0x0400082B RID: 2091
	private float[] callsTimes;

	// Token: 0x0400082C RID: 2092
	private int currentIndex;

	// Token: 0x0400082D RID: 2093
	private float spamProtectedTime;
}
