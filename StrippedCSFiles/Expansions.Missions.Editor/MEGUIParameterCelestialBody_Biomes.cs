using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

[MEGUI_CelestialBody_Biomes]
public class MEGUIParameterCelestialBody_Biomes : MEGUICompoundParameter
{
	private GAPCelestialBody gapCB;

	private MEGUIParameterDropdownList dropDownBodies;

	private MEGUIParameterDropdownList dropDownBiomes;

	public MissionBiome FieldValue
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
	public MEGUIParameterCelestialBody_Biomes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateValue(int bodyIndex, int biomeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDropDownBody(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDropDownBiome(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPBiomeSelected(CBAttributeMapSO.MapAttribute biome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGapLeftArrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGapRightArrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DisplayGAP()
	{
		throw null;
	}
}
