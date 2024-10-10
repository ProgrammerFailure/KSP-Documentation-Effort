using System;

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

	public void ApplyMode()
	{
		controller.altimeterFrame.Transition(altimeter.TransitionStateName());
		controller.timeFrame.Transition(timeFrame.TransitionStateName());
		controller.MapOptionsQuadrant.Transition(MapOptionsQuadrant.TransitionStateName());
		controller.stagingQuadrant.Transition(stagingQuadrant.TransitionStateName());
		controller.dockingLinQuadrant.Transition(dockingLinQuadrant.TransitionStateName());
		controller.dockingRotQuadrant.Transition(dockingRotQuadrant.TransitionStateName());
		controller.crew.Transition(crew.TransitionStateName());
		if (controller.navBall.State != "Out")
		{
			controller.navBall.Transition(navBall.TransitionStateName());
		}
		controller.manNodeHandleEditor.Transition(manNodeHandles.TransitionStateName());
		controller.manNodeEditor.Transition(manNodeEditor.TransitionStateName(), FlightUIModeController.Instance.finishedAllTransitions);
		if (uiModeFrame != -1)
		{
			controller.uiModeFrame.Transition(uiModeFrame);
		}
	}
}
