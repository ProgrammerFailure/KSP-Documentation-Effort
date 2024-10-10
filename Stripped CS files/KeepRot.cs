using UnityEngine;

public class KeepRot : MonoBehaviour
{
	public Quaternion rot;

	public void Start()
	{
		rot = base.transform.rotation;
	}

	public void LateUpdate()
	{
		base.transform.rotation = rot;
	}
}
