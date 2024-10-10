using UnityEngine;

public class GAPProceduralAsteroid : MonoBehaviour
{
	public string resourceURL;

	public int seed;

	public float radius;

	public ProceduralAsteroid paPrefab;

	public UntrackedObjectClass sizeClass;

	public bool isGlimmeroid;

	public void Setup(int seed, bool isGlimmeroid, UntrackedObjectClass sizeClass)
	{
		this.seed = seed;
		this.isGlimmeroid = isGlimmeroid;
		this.sizeClass = sizeClass;
	}

	public void Start()
	{
		resourceURL = "Procedural/PA_A";
		paPrefab = Resources.Load<ProceduralAsteroid>(resourceURL);
		float num = (float)(sizeClass + 2) / 5f;
		radius = paPrefab.radius * Random.Range(num, num);
		paPrefab.Generate(seed, radius, base.transform, RangefinderGeneric, delegate
		{
		}, isGlimmeroid);
	}

	public float RangefinderGeneric(Transform t)
	{
		return 0f;
	}
}
