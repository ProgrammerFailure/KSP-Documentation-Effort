using System.Runtime.CompilerServices;
using UnityEngine;

public class SimpleOrbitController : MonoBehaviour
{
	public Transform satellite;

	public double distance;

	public double period;

	private double orbitalSpeed;

	private Vector3d orbitalPosition;

	private double orbitalAngle;

	private QuaternionD orbitalRot;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SimpleOrbitController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}
}
