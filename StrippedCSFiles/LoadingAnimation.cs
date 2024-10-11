using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
	[Serializable]
	public class RotatingObject
	{
		public Transform rotator;

		public float rotationSpeed;

		public Vector3 rotationAxis;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public RotatingObject()
		{
			throw null;
		}
	}

	public List<RotatingObject> rotators;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LoadingAnimation()
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
}
