using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleFlightDisplay : PartModule
{
	private static Rect _windowPosition;

	private GUIStyle _windowStyle;

	private GUIStyle _labelStyle;

	private GUIStyle _centerStyle;

	private bool _hasInitStyles;

	private bool _guiIsActive;

	public bool GuiIsActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleFlightDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ModuleFlightDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001811")]
	public void ToggleGui()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TurnOffGui()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ModuleFlightDisplay GetActiveGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDraw()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWindow(int windowId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetSignalStrength()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetFirstHop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetTotalHops()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetLastHop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetControlState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitStyles()
	{
		throw null;
	}
}
