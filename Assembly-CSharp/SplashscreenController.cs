using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000164 RID: 356
public class SplashscreenController : MonoBehaviour
{
	// Token: 0x06000A57 RID: 2647 RVA: 0x0002F0EA File Offset: 0x0002D2EA
	private void Awake()
	{
		this.currentStep = SplashscreenController.Step.Uninitialized;
		this.currentState = SplashscreenController.State.Special;
		this.currentStateTime = 0.1f;
		this.timer = 0f;
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0002F110 File Offset: 0x0002D310
	private void Update()
	{
		this.timer += Time.unscaledDeltaTime;
		this.FadeUpdate();
		if (this.timer >= this.currentStateTime)
		{
			this.NextState();
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0007CEA0 File Offset: 0x0007B0A0
	private void NextStep()
	{
		switch (this.currentStep)
		{
		case SplashscreenController.Step.Uninitialized:
			this.currentStep = SplashscreenController.Step.LogosSplashscreen;
			this.currentStepFadeInTime = 0.5f;
			this.currentStepDuration = 2f;
			this.currentStepFadeOutTime = 0.5f;
			this.imagesToFade = new Image[] { this.background, this.companiesLogo };
			this.SetImagesToFadeTo0Alpha();
			this.SetImagesToFadeActive(true);
			return;
		case SplashscreenController.Step.LogosSplashscreen:
			this.currentStep = SplashscreenController.Step.ScytheSplashscreen;
			this.currentStepFadeInTime = 0.5f;
			this.currentStepDuration = 2f;
			this.currentStepFadeOutTime = 0.5f;
			this.SetImagesToFadeActive(false);
			this.imagesToFade = new Image[] { this.labelLogo };
			this.SetImagesToFadeTo0Alpha();
			this.SetImagesToFadeActive(true);
			return;
		case SplashscreenController.Step.ScytheSplashscreen:
			this.currentStep = SplashscreenController.Step.Loading;
			this.currentStepFadeInTime = float.MaxValue;
			this.currentState = SplashscreenController.State.Special;
			this.loader.SetActive(true);
			break;
		case SplashscreenController.Step.Loading:
			break;
		default:
			return;
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0007CF98 File Offset: 0x0007B198
	private void NextState()
	{
		this.timer = 0f;
		switch (this.currentState)
		{
		case SplashscreenController.State.FadeIn:
			this.currentState = SplashscreenController.State.Visible;
			this.currentStateTime = this.currentStepDuration;
			break;
		case SplashscreenController.State.Visible:
			this.currentState = SplashscreenController.State.FadeOut;
			this.currentStateTime = this.currentStepFadeOutTime;
			break;
		case SplashscreenController.State.FadeOut:
			this.currentState = SplashscreenController.State.FadeIn;
			this.NextStep();
			this.currentStateTime = this.currentStepFadeInTime;
			break;
		case SplashscreenController.State.Special:
			if (this.currentStep == SplashscreenController.Step.Uninitialized)
			{
				this.currentState = SplashscreenController.State.FadeIn;
				this.NextStep();
				this.currentStateTime = this.currentStepFadeInTime;
			}
			else
			{
				SplashscreenController.Step step = this.currentStep;
			}
			break;
		}
		if (this.currentStep == SplashscreenController.Step.LogosSplashscreen && this.currentState == SplashscreenController.State.Visible)
		{
			this.imagesToFade = new Image[] { this.companiesLogo };
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0007D068 File Offset: 0x0007B268
	private void FadeUpdate()
	{
		if (this.currentState == SplashscreenController.State.FadeIn)
		{
			Image[] array = this.imagesToFade;
			for (int i = 0; i < array.Length; i++)
			{
				SplashscreenController.SetAlpha(array[i], this.timer / this.currentStateTime);
			}
			return;
		}
		if (this.currentState == SplashscreenController.State.FadeOut)
		{
			Image[] array = this.imagesToFade;
			for (int i = 0; i < array.Length; i++)
			{
				SplashscreenController.SetAlpha(array[i], 1f - this.timer / this.currentStateTime);
			}
		}
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0007D0E4 File Offset: 0x0007B2E4
	private void SetImagesToFadeActive(bool active)
	{
		Image[] array = this.imagesToFade;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(active);
		}
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0007D114 File Offset: 0x0007B314
	private void SetImagesToFadeTo0Alpha()
	{
		Image[] array = this.imagesToFade;
		for (int i = 0; i < array.Length; i++)
		{
			SplashscreenController.SetAlpha(array[i], 0f);
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0002F13E File Offset: 0x0002D33E
	private static void SetAlpha(Image image, float alpha)
	{
		image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0002F16D File Offset: 0x0002D36D
	public bool HasEnded()
	{
		return this.currentStep == SplashscreenController.Step.Loading;
	}

	// Token: 0x040008F9 RID: 2297
	[Header("Scythe Splashscreens")]
	[SerializeField]
	private Image background;

	// Token: 0x040008FA RID: 2298
	[SerializeField]
	private Image companiesLogo;

	// Token: 0x040008FB RID: 2299
	[SerializeField]
	private Image labelLogo;

	// Token: 0x040008FC RID: 2300
	[SerializeField]
	private GameObject loader;

	// Token: 0x040008FD RID: 2301
	private const float INITIAL_DELAY = 0.1f;

	// Token: 0x040008FE RID: 2302
	private const float LOGOS_FADE_IN_TIME = 0.5f;

	// Token: 0x040008FF RID: 2303
	private const float LOGOS_DURATION = 2f;

	// Token: 0x04000900 RID: 2304
	private const float LOGOS_FADE_OUT_TIME = 0.5f;

	// Token: 0x04000901 RID: 2305
	private const float LABEL_FADE_IN_TIME = 0.5f;

	// Token: 0x04000902 RID: 2306
	private const float LABEL_DURATION = 2f;

	// Token: 0x04000903 RID: 2307
	private const float LABEL_FADE_OUT_TIME = 0.5f;

	// Token: 0x04000904 RID: 2308
	private const float LOADER_DURATION_BEFORE_ENTERING_MENU = 1f;

	// Token: 0x04000905 RID: 2309
	private SplashscreenController.Step currentStep;

	// Token: 0x04000906 RID: 2310
	private SplashscreenController.State currentState;

	// Token: 0x04000907 RID: 2311
	private float timer;

	// Token: 0x04000908 RID: 2312
	private float currentStateTime;

	// Token: 0x04000909 RID: 2313
	private float currentStepFadeInTime;

	// Token: 0x0400090A RID: 2314
	private float currentStepDuration;

	// Token: 0x0400090B RID: 2315
	private float currentStepFadeOutTime;

	// Token: 0x0400090C RID: 2316
	private Image[] imagesToFade;

	// Token: 0x02000165 RID: 357
	private enum Step
	{
		// Token: 0x0400090E RID: 2318
		Uninitialized,
		// Token: 0x0400090F RID: 2319
		LogosSplashscreen,
		// Token: 0x04000910 RID: 2320
		ScytheSplashscreen,
		// Token: 0x04000911 RID: 2321
		Loading
	}

	// Token: 0x02000166 RID: 358
	private enum State
	{
		// Token: 0x04000913 RID: 2323
		FadeIn,
		// Token: 0x04000914 RID: 2324
		Visible,
		// Token: 0x04000915 RID: 2325
		FadeOut,
		// Token: 0x04000916 RID: 2326
		Special
	}
}
