using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DynamicAmbientLight : MonoBehaviour
{
	private class AmbientLightColor
	{
		public Color ambientLight;

		public Color ambientEquatorColor;

		public Color ambientGroundColor;

		public Color ambientSkyColor;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AmbientLightColor()
		{
			throw null;
		}
	}

	public Color vacuumAmbientColor;

	public Color defaultAmbientColor;

	public bool disableDynamicAmbient;

	private float normalizedAtmosphericPressure;

	private double pressureAtSeaLevel;

	private double currentPressure;

	private CelestialBody currentBody;

	private AmbientLightColor originalLightColor;

	private Dictionary<GameScenes, AmbientLightColor> staticAmbientColors;

	public float boostFactor;

	public static DynamicAmbientLight Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DynamicAmbientLight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GrabDefaultAmbient(GameScenes g)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
