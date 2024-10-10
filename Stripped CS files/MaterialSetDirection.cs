using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialSetDirection : MonoBehaviour
{
	public Transform target;

	public string valueName;

	public Renderer r;

	public MaterialPropertyBlock mpb;

	public void Reset()
	{
		valueName = "_sunLightDirection";
	}

	public void Start()
	{
		r = GetComponent<Renderer>();
		mpb = new MaterialPropertyBlock();
		if (target == null && Sun.Instance != null)
		{
			target = Sun.Instance.transform;
		}
	}

	public void Update()
	{
		if (target != null)
		{
			mpb.SetVector(valueName, base.transform.InverseTransformDirection(target.transform.forward).normalized);
			r.SetPropertyBlock(mpb);
		}
	}
}
