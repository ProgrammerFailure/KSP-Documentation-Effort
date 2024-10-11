using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_Comet]
public class MEGUIParameterComet : MEGUICompoundParameter
{
	public TMP_InputField textSeed;

	public Button buttonRegenSeed;

	private MEGUIParameterDropdownList dropDownCometType;

	private MEGUIParameterDropdownList dropDownCometClass;

	private MEGUIParameterInputField inputName;

	private Comet comet;

	protected GAPPrefabDisplay prefabDisplay;

	public Comet FieldValue
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
	public MEGUIParameterComet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Button_RegenerateSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DisplayGAP()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputText_NewSeed(string inputValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshGapPrefab(int foo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshGapPrefab()
	{
		throw null;
	}
}
