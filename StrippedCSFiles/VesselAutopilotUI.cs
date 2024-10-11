using System.Runtime.CompilerServices;
using KSP.UI;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class VesselAutopilotUI : MonoBehaviour
{
	public UIStateToggleButton[] modeButtons;

	private static int updateWaitFrames;

	private int requiresUpdate;

	private VesselAutopilot.AutopilotMode[] activeModes;

	private Canvas canvas;

	private bool uiIsActive;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselAutopilotUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VesselAutopilotUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ToggleUI(bool enabledState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKerbalLevelUp(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameParametersChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickButton(int buttonIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetButtonTrue(int btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetButtonIndex(UIStateToggleButton btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPilotSkill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsSkillActive(VesselAutopilot.AutopilotMode mode)
	{
		throw null;
	}
}
