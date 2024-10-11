using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_LaunchSiteSituation]
public class MEGUIParameterLaunchSiteSituation : MEGUICompoundParameter
{
	public RectTransform containerHeader;

	public Image imageHeader;

	public Image imageBackGround;

	public TMP_InputField inputLaunchSiteName;

	public Button buttonExpand;

	public string guiSkinName;

	[SerializeField]
	private GameObject CollapsibleGroup;

	[SerializeField]
	private GameObject CollapsibleButtonIcon;

	private MEGUIParameterCelestialBody_VesselGroundLocation launchSiteGroundLocation;

	private LaunchSiteSituation launchSiteSituation;

	private MEGUIParameterDropdownList editorFacility;

	public LaunchSiteSituation FieldValue
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
	public MEGUIParameterLaunchSiteSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Display()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InputTextAction_SetLaunchSiteName(string newTitle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void toggleExpand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override ConfigNode GetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onHistorysetLaunchSiteName(ConfigNode data, HistoryType type)
	{
		throw null;
	}
}
