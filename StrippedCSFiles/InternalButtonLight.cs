using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalButtonLight : InternalModule
{
	[KSPField]
	public string buttonName;

	[KSPField]
	public bool defaultValue;

	[KSPField]
	public string lightName;

	[KSPField]
	public Color lightColor;

	[KSPField]
	public float lightIntensityOn;

	[KSPField]
	public float lightIntensityOff;

	[KSPField]
	public bool useButtonColor;

	[KSPField]
	public Color buttonColorOn;

	[KSPField]
	public Color buttonColorOff;

	public Collider buttonObject;

	public Light[] lightObjects;

	public bool lightsOn;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalButtonLight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Button_OnTap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLight(bool on)
	{
		throw null;
	}
}
