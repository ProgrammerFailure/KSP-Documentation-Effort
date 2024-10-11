using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class SimpleAppFrame : MonoBehaviour
{
	public PointerEnterExitHandler hoverController;

	private ApplicationLauncherButton appLauncherButton;

	private RectTransform rectTransform;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SimpleAppFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ApplicationLauncherButton appLauncherButton, string appName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddGlobalInputDelegate(UnityAction<PointerEventData> pointerEnter, UnityAction<PointerEventData> pointerExit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reposition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
