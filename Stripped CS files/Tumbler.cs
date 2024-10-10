using System;
using UnityEngine;

public class Tumbler : MonoBehaviour
{
	public enum TumbleDirection
	{
		Up,
		Down
	}

	public float sharpness;

	public double tgtRot;

	public double currRot;

	public double double_0;

	public void tumbleTo(double n, TumbleDirection tumble)
	{
		n = Math.Floor(n % 10.0);
		tgtRot = 36.0 * n;
		switch (tumble)
		{
		case TumbleDirection.Down:
			if (n > double_0)
			{
				currRot += 360.0;
			}
			break;
		case TumbleDirection.Up:
			if (n < double_0)
			{
				currRot -= 360.0;
			}
			break;
		}
		double_0 = n;
	}

	public void Update()
	{
		currRot += (tgtRot - currRot) * (double)(sharpness * Time.deltaTime);
		base.transform.localRotation = Quaternion.Euler((float)(0.0 - currRot), 0f, 0f);
	}
}
