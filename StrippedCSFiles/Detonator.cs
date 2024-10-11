using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Detonator/Detonator")]
public class Detonator : MonoBehaviour
{
	private static float _baseSize;

	private static Color _baseColor;

	private static float _baseDuration;

	public float size;

	public Color color;

	public bool explodeOnStart;

	public float duration;

	public float detail;

	public float upwardsBias;

	public float destroyTime;

	public bool useWorldSpace;

	public Vector3 direction;

	public Material fireballAMaterial;

	public Material fireballBMaterial;

	public Material smokeAMaterial;

	public Material smokeBMaterial;

	public Material shockwaveMaterial;

	public Material sparksMaterial;

	public Material glowMaterial;

	public Material heatwaveMaterial;

	private Component[] components;

	private DetonatorFireball _fireball;

	private DetonatorSparks _sparks;

	private DetonatorShockwave _shockwave;

	private DetonatorSmoke _smoke;

	private DetonatorGlow _glow;

	private DetonatorLight _light;

	private DetonatorForce _force;

	private DetonatorHeatwave _heatwave;

	public bool autoCreateFireball;

	public bool autoCreateSparks;

	public bool autoCreateShockwave;

	public bool autoCreateSmoke;

	public bool autoCreateGlow;

	public bool autoCreateLight;

	public bool autoCreateForce;

	public bool autoCreateHeatwave;

	private float _lastExplosionTime;

	private bool _firstComponentUpdate;

	private Component[] _subDetonators;

	public static Material defaultFireballAMaterial;

	public static Material defaultFireballBMaterial;

	public static Material defaultSmokeAMaterial;

	public static Material defaultSmokeBMaterial;

	public static Material defaultShockwaveMaterial;

	public static Material defaultSparksMaterial;

	public static Material defaultGlowMaterial;

	public static Material defaultHeatwaveMaterial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Detonator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Detonator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FillDefaultMaterials()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateComponents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Explode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultFireballAMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultFireballBMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultSmokeAMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultSmokeBMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultSparksMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultShockwaveMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultGlowMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material DefaultHeatwaveMaterial()
	{
		throw null;
	}
}
