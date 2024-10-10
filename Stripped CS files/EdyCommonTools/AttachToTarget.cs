using UnityEngine;

namespace EdyCommonTools;

public class AttachToTarget : MonoBehaviour
{
	public Transform target;

	public void LateUpdate()
	{
		if ((bool)target)
		{
			base.transform.position = target.position;
			base.transform.rotation = target.rotation;
		}
	}
}
