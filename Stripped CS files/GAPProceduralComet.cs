using UnityEngine;

public class GAPProceduralComet : MonoBehaviour
{
	public string resourceURL;

	public uint seed;

	public float radius;

	public ProceduralComet pcPrefab;

	public UntrackedObjectClass sizeClass;

	public void Setup(uint seed, UntrackedObjectClass sizeClass)
	{
		this.seed = seed;
		this.sizeClass = sizeClass;
	}

	public void Start()
	{
		resourceURL = "Procedural/PC_A";
		pcPrefab = Resources.Load<ProceduralComet>(resourceURL);
		float num = (float)(sizeClass + 2) / 5f;
		radius = pcPrefab.radius * num;
		pcPrefab.Generate((int)seed, radius, base.transform, RangefinderGeneric, delegate
		{
		}, optimizeCollider: true);
	}

	public float RangefinderGeneric(Transform t)
	{
		return 0f;
	}
}
