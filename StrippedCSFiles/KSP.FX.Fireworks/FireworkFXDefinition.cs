using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.FX.Fireworks;

[Serializable]
public class FireworkFXDefinition
{
	[Persistent]
	[SerializeField]
	public string name;

	[SerializeField]
	[Persistent]
	public FireworkEffectType fwType;

	[SerializeField]
	[Persistent]
	public string prefabName;

	[SerializeField]
	[Persistent]
	public string displayName;

	[SerializeField]
	[Persistent]
	public string color1Name;

	[Persistent]
	[SerializeField]
	public string color2Name;

	[SerializeField]
	[Persistent]
	public string color3Name;

	[Persistent]
	[SerializeField]
	public string crackleSFX;

	public int id;

	public bool randomizeBurstOrientation;

	public float minTrailLifetime;

	public float maxTrailLifetime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FireworkFXDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
