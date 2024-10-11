using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PSystemBody : MonoBehaviour
{
	public CelestialBody celestialBody;

	public OrbitDriver orbitDriver;

	public OrbitRenderer orbitRenderer;

	public GameObject scaledVersion;

	public PQS pqsVersion;

	public int flightGlobalsIndex;

	public bool planetariumCameraInitial;

	public List<PSystemBody> children;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PSystemBody()
	{
		throw null;
	}
}
