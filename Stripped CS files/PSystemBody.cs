using System.Collections.Generic;
using UnityEngine;

public class PSystemBody : MonoBehaviour
{
	public CelestialBody celestialBody;

	public OrbitDriver orbitDriver;

	public OrbitRenderer orbitRenderer;

	public GameObject scaledVersion;

	public GClass4 pqsVersion;

	public int flightGlobalsIndex = -1;

	public bool planetariumCameraInitial;

	public List<PSystemBody> children = new List<PSystemBody>();
}
