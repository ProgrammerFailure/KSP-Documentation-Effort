using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_Biomes : GAPCelestialBodyState_Base
{
	public delegate void OnBiomeSelected(CBAttributeMapSO.MapAttribute biome);

	private Shader shaderBiomes;

	private Shader shaderRegular;

	private Material scaledBodyMaterial;

	private float biomeHighlightValue;

	private CBAttributeMapSO.MapAttribute selectedBiome;

	private CBAttributeMapSO.MapAttribute hoverBiome;

	public OnBiomeSelected OnGAPBiomeSelected;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBodyState_Biomes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init(GAPCelestialBody gapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void End()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void LoadPlanet(CelestialBody newCelestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UnloadPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnClickUp(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseOver(Vector2 cameraPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectBiome(CBAttributeMapSO.MapAttribute newBiome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasSelectedBiome()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectBiomeByName(string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetBiomeIndex(CelestialBody body, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGAPText()
	{
		throw null;
	}
}
