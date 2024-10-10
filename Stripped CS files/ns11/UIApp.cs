using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns11;

public abstract class UIApp : MonoBehaviour
{
	[SerializeField]
	public string appName;

	public Texture appLauncherIcon;

	public Animator appLauncherAnim;

	[SerializeField]
	public int AppStartFrameDelay = 5;

	[SerializeField]
	public bool enableOnHover = true;

	[SerializeField]
	public bool enableMutuallyExclusive = true;

	public Callback defaultCallback = delegate
	{
	};

	public ApplicationLauncherButton appLauncherButton { get; set; }

	public bool hover { get; set; }

	public bool pinned { get; set; }

	public bool appIsLive { get; set; }

	public bool IsShowing => appIsLive;

	public UIApp()
	{
	}

	public abstract ApplicationLauncher.AppScenes GetAppScenes();

	public abstract Vector3 GetAppScreenPos(Vector3 defaultAnchorPos);

	public abstract bool OnAppAboutToStart();

	public abstract void OnAppInitialized();

	public abstract void OnAppDestroy();

	public abstract void DisplayApp();

	public abstract void HideApp();

	public virtual void OnAppStarted()
	{
	}

	public virtual void Awake()
	{
		Debug.Log("[UiApp] Awake: " + appName);
		GameEvents.onGUIApplicationLauncherReady.Add(OnAppLauncherReady);
	}

	public void OnDestroy()
	{
		Debug.Log("[UIApp] OnDestroy: " + appName);
		GameEvents.onGUIApplicationLauncherReady.Remove(OnAppLauncherReady);
		OnAppDestroy();
		if (appLauncherButton != null && ApplicationLauncher.Instance != null)
		{
			ApplicationLauncher.Instance.RemoveOnHideCallback(OnAppLauncherHide);
			ApplicationLauncher.Instance.RemoveOnShowCallback(OnAppLauncherShow);
			ApplicationLauncher.Instance.RemoveOnRepositionCallback(Reposition);
			ApplicationLauncher.Instance.RemoveApplication(appLauncherButton);
		}
	}

	public void OnAppLauncherReady()
	{
		StartCoroutine(AddToAppLauncher());
	}

	public IEnumerator AddToAppLauncher()
	{
		for (int i = 0; i < AppStartFrameDelay; i++)
		{
			yield return null;
		}
		yield return null;
		StartCoroutine(CallbackUtil.DelayedCallback(2, delegate
		{
			OnAppStarted();
		}));
		if (!OnAppAboutToStart())
		{
			yield break;
		}
		Debug.Log("[UIApp] Adding " + appName + " to Application Launcher");
		if (appLauncherAnim != null)
		{
			Animator sprite = Object.Instantiate(appLauncherAnim);
			appLauncherButton = ApplicationLauncher.Instance.AddApplication(Show, Hide, enableOnHover ? new Callback(Hover) : defaultCallback, enableOnHover ? new Callback(HoverOut) : defaultCallback, EnablePanel, DisablePanel, sprite);
		}
		else
		{
			if (!(appLauncherIcon != null))
			{
				Debug.LogError("UIApp: No Applauncher button specified, aborting");
				Object.Destroy(base.gameObject);
				yield break;
			}
			appLauncherButton = ApplicationLauncher.Instance.AddApplication(Show, Hide, enableOnHover ? new Callback(Hover) : defaultCallback, enableOnHover ? new Callback(HoverOut) : defaultCallback, EnablePanel, DisablePanel, appLauncherIcon);
		}
		appLauncherButton.VisibleInScenes = GetAppScenes();
		ApplicationLauncher.Instance.AddOnHideCallback(OnAppLauncherHide);
		ApplicationLauncher.Instance.AddOnShowCallback(OnAppLauncherShow);
		ApplicationLauncher.Instance.AddOnRepositionCallback(Reposition);
		if (!enableMutuallyExclusive)
		{
			ApplicationLauncher.Instance.DisableMutuallyExclusive(appLauncherButton);
		}
		base.gameObject.transform.position = GetAppScreenPos(new Vector3(appLauncherButton.GetAnchor().x - 150f + 38f, appLauncherButton.GetAnchor().y, appLauncherButton.GetAnchor().z));
		StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			OnAppInitialized();
		}));
	}

	public void ForceAddToAppLauncher()
	{
		StartCoroutine(AddToAppLauncher());
	}

	public virtual void Reposition()
	{
	}

	public void MouseInput_PointerEnter(PointerEventData eventData)
	{
		if (!hover)
		{
			Hover();
			appLauncherButton.onHoverBtn(appLauncherButton.toggleButton);
			hover = true;
		}
	}

	public void MouseInput_PointerExit(PointerEventData eventData)
	{
		if (!appLauncherButton.IsHovering)
		{
			HoverOut();
			appLauncherButton.onHoverOutBtn(appLauncherButton.toggleButton);
			hover = false;
		}
	}

	public void Show()
	{
		if (!appIsLive)
		{
			displayApp();
		}
		pinned = true;
	}

	public void Hide()
	{
		if (appIsLive)
		{
			hideApp();
		}
		pinned = false;
	}

	public void Hover()
	{
		if (!appIsLive)
		{
			displayApp();
		}
	}

	public void HoverOut()
	{
		if (appIsLive && !pinned)
		{
			StartCoroutine(HoverOutCoroutine());
		}
	}

	public IEnumerator HoverOutCoroutine()
	{
		yield return null;
		if (!appLauncherButton.IsHovering && appLauncherButton.toggleButton.CurrentState != 0)
		{
			hideApp();
		}
	}

	public void EnablePanel()
	{
		if (appIsLive)
		{
			Show();
		}
	}

	public void DisablePanel()
	{
		Hide();
	}

	public void OnAppLauncherHide()
	{
		if (appIsLive)
		{
			HideApp();
		}
	}

	public void OnAppLauncherShow()
	{
		if (appIsLive)
		{
			DisplayApp();
		}
	}

	public void displayApp()
	{
		appIsLive = true;
		DisplayApp();
	}

	public void hideApp()
	{
		if (!hover)
		{
			appIsLive = false;
			HideApp();
		}
	}
}
