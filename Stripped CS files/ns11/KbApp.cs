using System;
using ns2;
using UnityEngine;

namespace ns11;

public abstract class KbApp : MonoBehaviour
{
	public Texture appIcon;

	public string appName;

	public string appTitle;

	public KnowledgeBase.KbTargetType targetType;

	[NonSerialized]
	public ApplicationLauncherButton appLauncherButton;

	public KbAppFrame appFramePrefab;

	[NonSerialized]
	public KbAppFrame appFrame;

	public Color headerColor;

	public bool pinned { get; set; }

	public bool appIsLive { get; set; }

	public KbApp()
	{
	}

	public abstract void DisplayApp();

	public abstract void HideApp();

	public abstract void ActivateApp(MapObject target);

	public virtual void Awake()
	{
	}

	public virtual void Start()
	{
		if (appFrame != null)
		{
			appFrame.header.color = headerColor;
		}
	}

	public virtual void OnDestroy()
	{
		Debug.Log("KbApp.OnDestroy " + appName);
		if (appFrame != null)
		{
			appFrame.gameObject.DestroyGameObject();
		}
		if (ApplicationLauncher.Instance != null)
		{
			ApplicationLauncher.Instance.DisableMutuallyExclusive(appLauncherButton);
		}
	}

	public virtual void Setup()
	{
		appLauncherButton = UnityEngine.Object.Instantiate(KnowledgeBase.Instance.appLauncherButtonPrefab);
		appLauncherButton.Setup(Show, Hide, Hover, HoverOut, EnablePanel, DisablePanel, appIcon);
		if (appFramePrefab != null)
		{
			appFrame = UnityEngine.Object.Instantiate(appFramePrefab);
			appFrame.Setup(appLauncherButton, appName, appTitle);
		}
		ApplicationLauncher.Instance.EnableMutuallyExclusive(appLauncherButton);
	}

	public void Restore()
	{
		if (appLauncherButton.toggleButton.CurrentState == UIRadioButton.State.True)
		{
			displayApp();
		}
	}

	public void Show()
	{
		if (!appIsLive && ApplicationLauncher.Instance.DetermineVisibility(appLauncherButton))
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
			hideApp();
		}
	}

	public void EnablePanel()
	{
		Show();
	}

	public void DisablePanel()
	{
		Hide();
	}

	public void displayApp()
	{
		appIsLive = true;
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: true);
			appFrame.Reposition();
		}
		DisplayApp();
	}

	public void hideApp()
	{
		appIsLive = false;
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: false);
		}
		HideApp();
	}
}
