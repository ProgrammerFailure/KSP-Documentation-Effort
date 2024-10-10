using UnityEngine;

namespace EdyCommonTools;

public class BindLocalRotation : MonoBehaviour
{
	public Transform source;

	public Transform[] targets;

	public void LateUpdate()
	{
		Quaternion localRotation = ((source != null) ? source.localRotation : base.transform.localRotation);
		Transform[] array = targets;
		foreach (Transform transform in array)
		{
			if (transform != null)
			{
				transform.localRotation = localRotation;
			}
		}
	}
}
