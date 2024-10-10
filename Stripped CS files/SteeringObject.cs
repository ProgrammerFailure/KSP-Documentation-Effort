using UnityEngine;

public class SteeringObject
{
	public enum ControlAxis
	{
		Forward,
		Right,
		Up
	}

	public enum AlignmentAxis
	{
		Forward,
		Up,
		Right,
		None
	}

	public Transform pivot;

	public Quaternion neutralAngle;

	public ControlAxis controlAxis;

	public AlignmentAxis alignmentAxis = AlignmentAxis.None;

	public float wheelDriveInvert = 1f;

	public void FindAlignmentAxis(Transform referenceTransform, Transform partTransform)
	{
		alignmentAxis = AlignmentAxis.None;
		float f = 0f;
		float f2 = 0f;
		float f3 = 0f;
		switch (controlAxis)
		{
		case ControlAxis.Forward:
			f = Vector3.Dot(referenceTransform.forward, partTransform.forward);
			f2 = Vector3.Dot(referenceTransform.right, partTransform.forward);
			f3 = Vector3.Dot(referenceTransform.up, partTransform.forward);
			break;
		case ControlAxis.Right:
			f = Vector3.Dot(referenceTransform.forward, partTransform.right);
			f2 = Vector3.Dot(referenceTransform.right, partTransform.right);
			f3 = Vector3.Dot(referenceTransform.up, partTransform.right);
			break;
		case ControlAxis.Up:
			f = Vector3.Dot(referenceTransform.forward, partTransform.up);
			f2 = Vector3.Dot(referenceTransform.right, partTransform.up);
			f3 = Vector3.Dot(referenceTransform.up, partTransform.up);
			break;
		}
		if (Mathf.Abs(f) > Mathf.Abs(f3))
		{
			if (Mathf.Abs(f) > Mathf.Abs(f2))
			{
				alignmentAxis = AlignmentAxis.Forward;
				wheelDriveInvert = f;
			}
			else
			{
				alignmentAxis = AlignmentAxis.Right;
				wheelDriveInvert = f2;
			}
		}
		else if (Mathf.Abs(f3) > Mathf.Abs(f2))
		{
			alignmentAxis = AlignmentAxis.Up;
			wheelDriveInvert = f3;
		}
		else
		{
			alignmentAxis = AlignmentAxis.Right;
			wheelDriveInvert = f2;
		}
		if ((double)Mathf.Abs(wheelDriveInvert) >= 0.7)
		{
			wheelDriveInvert /= Mathf.Abs(wheelDriveInvert);
		}
		else
		{
			wheelDriveInvert = 0f;
		}
	}
}
