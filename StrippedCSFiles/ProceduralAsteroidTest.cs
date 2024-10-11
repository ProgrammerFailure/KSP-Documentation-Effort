using System.Runtime.CompilerServices;
using UnityEngine;

public class ProceduralAsteroidTest : MonoBehaviour
{
	[SerializeField]
	private string resourceURL;

	[SerializeField]
	private int seed;

	[SerializeField]
	private float radius;

	[SerializeField]
	private float density;

	private ProceduralAsteroid paPrefab;

	private PAsteroid paGenerated;

	[SerializeField]
	private UntrackedObjectClass objectSize;

	[SerializeField]
	public float minRadiusMultiplier;

	[SerializeField]
	public float maxRadiusMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProceduralAsteroidTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Randomize")]
	private void Randomize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Rebuild")]
	private void Rebuild()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float RangefinderGeneric(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Add Rigidbody")]
	private void AddRigidbody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("New Seed")]
	private void NewSeed()
	{
		throw null;
	}
}
