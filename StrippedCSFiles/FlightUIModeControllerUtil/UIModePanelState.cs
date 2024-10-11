using System;
using System.Runtime.CompilerServices;

namespace FlightUIModeControllerUtil;

[Serializable]
public class UIModePanelState
{
	[NonSerialized]
	public FlightUIModeController controller;

	public TabAction altimeter;

	public TabAction timeFrame;

	public TabAction MapOptionsQuadrant;

	public TabAction stagingQuadrant;

	public TabAction dockingLinQuadrant;

	public TabAction dockingRotQuadrant;

	public TabAction manNodeHandles;

	public TabAction manNodeEditor;

	public TabAction crew;

	public TabAction navBall;

	public TabAction resources;

	public int uiModeFrame;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIModePanelState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyMode()
	{
		throw null;
	}
}
