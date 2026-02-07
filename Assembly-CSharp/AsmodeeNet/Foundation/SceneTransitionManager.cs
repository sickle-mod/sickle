using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000948 RID: 2376
	public class SceneTransitionManager : MonoBehaviour
	{
		// Token: 0x14000155 RID: 341
		// (add) Token: 0x06003FE0 RID: 16352 RVA: 0x0015CF68 File Offset: 0x0015B168
		// (remove) Token: 0x06003FE1 RID: 16353 RVA: 0x0015CFA0 File Offset: 0x0015B1A0
		public event Action<string> SceneWillLoad;

		// Token: 0x14000156 RID: 342
		// (add) Token: 0x06003FE2 RID: 16354 RVA: 0x0015CFD8 File Offset: 0x0015B1D8
		// (remove) Token: 0x06003FE3 RID: 16355 RVA: 0x0015D010 File Offset: 0x0015B210
		public event Action<Scene> SceneDidLoad;

		// Token: 0x14000157 RID: 343
		// (add) Token: 0x06003FE4 RID: 16356 RVA: 0x0015D048 File Offset: 0x0015B248
		// (remove) Token: 0x06003FE5 RID: 16357 RVA: 0x0015D080 File Offset: 0x0015B280
		public event Action<Scene> SceneWillUnload;

		// Token: 0x14000158 RID: 344
		// (add) Token: 0x06003FE6 RID: 16358 RVA: 0x0015D0B8 File Offset: 0x0015B2B8
		// (remove) Token: 0x06003FE7 RID: 16359 RVA: 0x0015D0F0 File Offset: 0x0015B2F0
		public event Action<Scene> SceneDidUnload;

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x0005100B File Offset: 0x0004F20B
		// (set) Token: 0x06003FE9 RID: 16361 RVA: 0x00051013 File Offset: 0x0004F213
		public bool IsTransitioning
		{
			get
			{
				return this._isTransitioning;
			}
			private set
			{
				this._isTransitioning = value;
				if (this._isTransitioning)
				{
					CoreApplication.Instance.UINavigationManager.BeginIgnoringInteractionEvents("SceneTransitionManager");
					return;
				}
				CoreApplication.Instance.UINavigationManager.EndIgnoringInteractionEvents("SceneTransitionManager");
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06003FEA RID: 16362 RVA: 0x0005104D File Offset: 0x0004F24D
		private bool IsInstantTransition
		{
			get
			{
				return (this.IsTransitioning && this._transitionType == SceneTransitionManager.TransitionType.None) || Mathf.Approximately(this._transitionSpeed, 0f);
			}
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0015D128 File Offset: 0x0015B328
		public bool IsCurrentScene(string sceneName)
		{
			return SceneManager.GetActiveScene().name == sceneName;
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x00051071 File Offset: 0x0004F271
		public void PreLoadScene(string sceneName)
		{
			if (this.IsTransitioning)
			{
				AsmoLogger.Warning("SceneTransitionManager", "Pre loading a scene during a transition is not supported", null);
				return;
			}
			CoreApplication.Instance.StartCoroutine(this.LoadScene(sceneName, false));
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0015D148 File Offset: 0x0015B348
		public void DisplayScene(string sceneName, SceneTransitionManager.TransitionType transitionType = SceneTransitionManager.TransitionType.FadeOutIn, float transitionDuration = 1f, bool forceReload = false)
		{
			if (this.IsTransitioning)
			{
				AsmoLogger.Warning("SceneTransitionManager", "Displaying a scene during a transition is not supported", null);
				return;
			}
			this._nextSceneName = sceneName;
			this._transitionType = transitionType;
			transitionDuration = Mathf.Max(0f, transitionDuration);
			float num = ((this._transitionType == SceneTransitionManager.TransitionType.FadeOutIn) ? 2f : 1f);
			this._transitionSpeed = num / transitionDuration;
			this.IsTransitioning = !this.IsInstantTransition;
			Hashtable hashtable = new Hashtable
			{
				{ "type", transitionType },
				{ "duration", transitionDuration },
				{ "isInstant", this.IsInstantTransition }
			};
			Scene sceneByName = SceneManager.GetSceneByName(this._nextSceneName);
			if (sceneByName.IsValid() && !forceReload)
			{
				if (sceneByName.isLoaded)
				{
					AsmoLogger.Debug("SceneTransitionManager", () => "DisplayScene: " + this._nextSceneName + " [Already Loaded]", hashtable);
					if (this.IsInstantTransition)
					{
						this.SetSceneActive(sceneByName);
					}
				}
				else
				{
					AsmoLogger.Debug("SceneTransitionManager", () => "DisplayScene: " + this._nextSceneName + " [Loading]", hashtable);
					if (this.IsInstantTransition)
					{
						this.FinishLoadingScene(this._nextSceneName);
					}
				}
			}
			else
			{
				AsmoLogger.Debug("SceneTransitionManager", () => "DisplayScene: " + this._nextSceneName + " [Not Loaded]", hashtable);
				bool isInstantTransition = this.IsInstantTransition;
				CoreApplication.Instance.StartCoroutine(this.LoadScene(this._nextSceneName, isInstantTransition));
			}
			if (!this.IsInstantTransition)
			{
				this.StartFadeOut();
			}
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0005109F File Offset: 0x0004F29F
		public IEnumerator LoadScene(string sceneName, bool allowSceneActivation = false)
		{
			this.CallSceneWillLoad(sceneName);
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			if (!allowSceneActivation)
			{
				operation.allowSceneActivation = false;
				this._loadingOperations.Add(sceneName, operation);
			}
			float progress = -1f;
			while (!operation.isDone)
			{
				if (operation.progress > progress)
				{
					progress = operation.progress;
					AsmoLogger.Debug("SceneTransitionManager", () => string.Format("Loading scene: {0} [{1}%]", sceneName, progress * 100f), null);
				}
				yield return null;
			}
			Scene? scene = null;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.name == sceneName && sceneAt != SceneManager.GetActiveScene())
				{
					scene = new Scene?(sceneAt);
					break;
				}
			}
			if (scene != null)
			{
				this.CallSceneDidLoad(scene.Value);
				this.SetSceneActive(scene.Value);
			}
			else
			{
				AsmoLogger.Error("SceneTransitionManager", "Scene not found during loading", null);
			}
			yield break;
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x000510BC File Offset: 0x0004F2BC
		private void FinishLoadingScene(string sceneName)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Finish loading scene: " + sceneName, null);
			this._loadingOperations[sceneName].allowSceneActivation = true;
			this._loadingOperations.Remove(sceneName);
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0015D2B0 File Offset: 0x0015B4B0
		private void SetSceneActive(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Set scene active: " + scene.name, null);
			Scene activeScene = SceneManager.GetActiveScene();
			SceneManager.SetActiveScene(scene);
			this.CallSceneWillUnload(activeScene);
			SceneManager.UnloadSceneAsync(activeScene);
			Resources.UnloadUnusedAssets();
			this.CallSceneDidUnload(activeScene);
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x000510F3 File Offset: 0x0004F2F3
		private void StartFadeOut()
		{
			this._fadingDirection = 1;
			if (this._transitionType == SceneTransitionManager.TransitionType.FadeOut || this._transitionType == SceneTransitionManager.TransitionType.FadeOutIn)
			{
				this._fadingAlpha = 0f;
				return;
			}
			this._fadingAlpha = 1f;
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x00051125 File Offset: 0x0004F325
		private void StartFadeIn()
		{
			this._fadingDirection = -1;
			if (this._transitionType == SceneTransitionManager.TransitionType.FadeIn || this._transitionType == SceneTransitionManager.TransitionType.FadeOutIn)
			{
				this._fadingAlpha = 1f;
				return;
			}
			this._fadingAlpha = 0f;
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x00051157 File Offset: 0x0004F357
		private void PauseFade()
		{
			this._fadingDirection = 0;
			this._fadingAlpha = 1f;
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0005116B File Offset: 0x0004F36B
		private void StopFade()
		{
			this._fadingDirection = 0;
			this._fadingAlpha = 0f;
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x0015D304 File Offset: 0x0015B504
		private Texture2D FadingTexture
		{
			get
			{
				if (this._fadingTexture == null)
				{
					this._fadingTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
					this._fadingTexture.SetPixel(0, 0, this.fadingColor);
					this._fadingTexture.Apply();
				}
				return this._fadingTexture;
			}
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0015D354 File Offset: 0x0015B554
		private void OnGUI()
		{
			if (!this.IsTransitioning)
			{
				return;
			}
			this._fadingAlpha += (float)this._fadingDirection * this._transitionSpeed * Time.deltaTime;
			if (this._fadingDirection > 0 && this._fadingAlpha > 1f)
			{
				this.PauseFade();
				this.FinishLoadingScene(this._nextSceneName);
			}
			else if (this._fadingDirection == 0)
			{
				if (SceneManager.GetActiveScene().name == this._nextSceneName && SceneManager.sceneCount == 1)
				{
					this.StartFadeIn();
				}
			}
			else if (this._fadingDirection < 0 && this._fadingAlpha < 0f)
			{
				this.StopFade();
				this.IsTransitioning = false;
			}
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.Clamp01(this._fadingAlpha));
			GUI.depth = -1000;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.FadingTexture);
			if (!this._isTransitioning)
			{
				this._fadingTexture = null;
			}
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0005117F File Offset: 0x0004F37F
		private void CallSceneWillLoad(string sceneName)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Will load scene: " + sceneName, null);
			if (this.SceneWillLoad != null)
			{
				this.SceneWillLoad(sceneName);
			}
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x000511AB File Offset: 0x0004F3AB
		private void CallSceneDidLoad(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Did load scene: " + scene.name, null);
			if (this.SceneDidLoad != null)
			{
				this.SceneDidLoad(scene);
			}
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x000511DD File Offset: 0x0004F3DD
		private void CallSceneWillUnload(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Will unload scene: " + scene.name, null);
			if (this.SceneWillUnload != null)
			{
				this.SceneWillUnload(scene);
			}
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x0005120F File Offset: 0x0004F40F
		private void CallSceneDidUnload(Scene scene)
		{
			AsmoLogger.Debug("SceneTransitionManager", "Did unload scene: " + scene.name, null);
			if (this.SceneDidUnload != null)
			{
				this.SceneDidUnload(scene);
			}
		}

		// Token: 0x040030C8 RID: 12488
		private const string _documentation = "<b>SceneTransitionManager</b> loads and displays <b>Scene</b>s with transitions by using <i>Multi Scene Editing</i> system";

		// Token: 0x040030CD RID: 12493
		private Dictionary<string, AsyncOperation> _loadingOperations = new Dictionary<string, AsyncOperation>();

		// Token: 0x040030CE RID: 12494
		private bool _isTransitioning;

		// Token: 0x040030CF RID: 12495
		private SceneTransitionManager.TransitionType _transitionType;

		// Token: 0x040030D0 RID: 12496
		private float _transitionSpeed;

		// Token: 0x040030D1 RID: 12497
		private string _nextSceneName;

		// Token: 0x040030D2 RID: 12498
		private const string _debugModuleName = "SceneTransitionManager";

		// Token: 0x040030D3 RID: 12499
		private Texture2D _fadingTexture;

		// Token: 0x040030D4 RID: 12500
		private float _fadingAlpha;

		// Token: 0x040030D5 RID: 12501
		private int _fadingDirection;

		// Token: 0x040030D6 RID: 12502
		public Color fadingColor = Color.black;

		// Token: 0x02000949 RID: 2377
		public enum TransitionType
		{
			// Token: 0x040030D8 RID: 12504
			None,
			// Token: 0x040030D9 RID: 12505
			FadeOut,
			// Token: 0x040030DA RID: 12506
			FadeIn,
			// Token: 0x040030DB RID: 12507
			FadeOutIn
		}
	}
}
