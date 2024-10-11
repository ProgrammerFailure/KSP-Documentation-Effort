using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

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

	protected bool pinned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool appIsLive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected KbApp()
	{
		throw null;
	}

	protected abstract void DisplayApp();

	protected abstract void HideApp();

	public abstract void ActivateApp(MapObject target);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Restore()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Hover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HoverOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnablePanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisablePanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void displayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void hideApp()
	{
		throw null;
	}
}
