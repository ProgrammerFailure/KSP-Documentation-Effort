using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Texture Atlas")]
public class PQSMod_TextureAtlas : PQSMod
{
	public CBTextureAtlasSO textureAtlasMap;

	private readonly int NUM_PACKED;

	public Material material1Blend;

	public Material material2Blend;

	public Material material3Blend;

	public Material material4Blend;

	private CelestialBody CurrentCelestialBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_TextureAtlas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanTextureAtlasModBeUsed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}
}
