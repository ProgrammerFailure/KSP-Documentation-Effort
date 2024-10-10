using UnityEngine;

public class FollowRot : MonoBehaviour
{
	public Transform tgt;

	public bool followX;

	public bool followY;

	public bool followZ;

	public void LateUpdate()
	{
		if (followX && followY && followZ)
		{
			base.transform.rotation = tgt.rotation;
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(followX ? tgt.rotation.eulerAngles.x : base.transform.rotation.eulerAngles.x, followY ? tgt.rotation.eulerAngles.y : base.transform.rotation.eulerAngles.y, followZ ? tgt.rotation.eulerAngles.z : base.transform.rotation.eulerAngles.z);
		}
	}
}
