using UnityEngine;

public class KSPWheelGravity
{
	public Vector3 gravityValue = Vector3.down * 9.80665f;

	public Vector3 gravityUp;

	public bool gravityCached;

	public float gravityMagnitude;

	public Vector3 Value
	{
		get
		{
			return gravityValue;
		}
		set
		{
			if (gravityValue != value)
			{
				gravityValue = value;
				Refresh();
			}
		}
	}

	public float Magnitude
	{
		get
		{
			if (!gravityCached)
			{
				Refresh();
			}
			return gravityMagnitude;
		}
	}

	public Vector3 Up
	{
		get
		{
			if (!gravityCached)
			{
				Refresh();
			}
			return gravityUp;
		}
	}

	public void Refresh()
	{
		gravityMagnitude = gravityValue.magnitude;
		gravityUp = (-gravityValue).normalized;
		gravityCached = true;
	}
}
