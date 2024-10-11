using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProceduralStarfield : MonoBehaviour
{
	[Serializable]
	public class StarClass
	{
		public float minSize;

		public float maxSize;

		public float minMagnitude;

		public float maxMagnitude;

		public Color color;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public StarClass()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public StarClass(float minSize, float maxSize, float minMag, float maxMag, Color color)
		{
			throw null;
		}
	}

	public int seed;

	public Vector3 fieldSize;

	public int numStars;

	public bool useStaticStarSize;

	public float staticStarSize;

	public bool useExclusionZone;

	public Vector3 exclusionZone;

	public Vector3 exclusionZoneOffset;

	public StarClass[] starClasses;

	public Material starMaterial;

	private Vector3 exclusionZoneMin;

	private Vector3 exclusionZoneMax;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProceduralStarfield()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateStarFields()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ParticleSystem.Particle[] CreateStarsWithNewSystem(int numStars)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ExclusionZoneTest(Vector3 position)
	{
		throw null;
	}
}
