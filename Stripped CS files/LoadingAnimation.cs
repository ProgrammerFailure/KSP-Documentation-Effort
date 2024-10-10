using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
	[Serializable]
	public class RotatingObject
	{
		public Transform rotator;

		public float rotationSpeed;

		public Vector3 rotationAxis;
	}

	public List<RotatingObject> rotators;

	public void Start()
	{
		if (rotators == null)
		{
			rotators = new List<RotatingObject>();
		}
	}

	public void Update()
	{
		int count = rotators.Count;
		while (count-- > 0)
		{
			RotatingObject rotatingObject = rotators[count];
			rotatingObject.rotator.Rotate(rotatingObject.rotationAxis, 0f - rotatingObject.rotationSpeed);
		}
	}
}
