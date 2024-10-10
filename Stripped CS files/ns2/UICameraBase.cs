using UnityEngine;

namespace ns2;

public abstract class UICameraBase : MonoBehaviour
{
	[SerializeField]
	public Camera cam;

	public UICameraBase()
	{
	}

	public void Reset()
	{
		cam = GetComponent<Camera>();
	}
}
