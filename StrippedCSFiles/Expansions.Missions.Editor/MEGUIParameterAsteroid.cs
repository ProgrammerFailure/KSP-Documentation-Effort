using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_Asteroid]
public class MEGUIParameterAsteroid : MEGUICompoundParameter
{
	public TMP_InputField textSeed;

	public Button buttonRegenSeed;

	private MEGUIParameterDropdownList dropDownAsteroidClass;

	private MEGUIParameterDropdownList dropDownAsteroidType;

	private MEGUIParameterInputField inputName;

	private Asteroid asteroid;

	protected GAPPrefabDisplay prefabDisplay;

	public Asteroid FieldValue
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
	public MEGUIParameterAsteroid()
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
