using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class EditorPanels : MonoBehaviour
{
	public enum Panel
	{
		None,
		Parts,
		Actions,
		Crew,
		PartsAndCargo
	}

	public UIPanelTransitionManager panelManager;

	public UIPanelTransition partsEditor;

	public UIPanelTransition actions;

	public UIPanelTransition crew;

	public UIPanelTransition cargo;

	public UIPanelTransition partsEditorModes;

	public UIPanelTransition partcategorizerModes;

	public UIPanelTransition searchField;

	private Panel panel;

	private List<UIPanelTransition> partsAndCargo;

	public static EditorPanels Instance
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
	public EditorPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void updatePartsListMode(Action onFinished = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ShowPartsList(Action onFinished = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ShowActionGroups(Action onFinished = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ShowCrewAssignment(Action onFinished = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ShowCargo(Action onFinished = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsMouseOver()
	{
		throw null;
	}
}
