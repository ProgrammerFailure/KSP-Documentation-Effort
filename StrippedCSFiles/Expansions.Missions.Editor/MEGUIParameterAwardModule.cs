using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIParameterAwardModule : MEGUIParameterDynamicModule
{
	public Image awardIcon;

	public TextMeshProUGUI awardDescription;

	public Toggle moduleEnabledToggle;

	public TMP_InputField awardedPoints;

	protected AwardModule awardModule;

	private bool isDirty;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterAwardModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value, Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupAwardModule(AwardModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModuleEnabledToggleValueChange(bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAwardedPointsValueChange(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAwardedPointsEndEdit(string value)
	{
		throw null;
	}
}
