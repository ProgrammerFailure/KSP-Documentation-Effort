using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("PREFAB_SPAWN")]
public class PrefabSpawnFX : EffectBehaviour
{
	[Persistent]
	public string prefabName;

	[Persistent]
	public string transformName;

	[Persistent]
	public bool setParent;

	private Transform modelParent;

	private GameObject prefabObject;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PrefabSpawnFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent(float power)
	{
		throw null;
	}
}
