using UnityEngine;

public class LookAtObject : MonoBehaviour
{
	public Transform target;

	public Vector3 fwdAxis = Vector3.forward;

	public KFSMUpdateMode updateMode = KFSMUpdateMode.UPDATE;

	public Transform trf;

	public void Awake()
	{
		trf = base.transform;
	}

	public void FixedUpdate()
	{
		if (updateMode == KFSMUpdateMode.FIXEDUPDATE)
		{
			onUpdate();
		}
	}

	public void Update()
	{
		if (updateMode == KFSMUpdateMode.UPDATE)
		{
			onUpdate();
		}
	}

	public void LateUpdate()
	{
		if (updateMode == KFSMUpdateMode.LATEUPDATE)
		{
			onUpdate();
		}
	}

	public void onUpdate()
	{
		trf.rotation = Quaternion.LookRotation(Quaternion.FromToRotation(trf.rotation * fwdAxis, target.position - trf.position) * trf.forward, target.up);
	}
}
