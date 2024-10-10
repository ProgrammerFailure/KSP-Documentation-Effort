using UnityEngine;

public class ProceduralAsteroidTest : MonoBehaviour
{
	[SerializeField]
	public string resourceURL;

	[SerializeField]
	public int seed;

	[SerializeField]
	public float radius;

	[SerializeField]
	public float density;

	public ProceduralAsteroid paPrefab;

	public PAsteroid paGenerated;

	[SerializeField]
	public UntrackedObjectClass objectSize;

	[SerializeField]
	public float minRadiusMultiplier = 0.75f;

	[SerializeField]
	public float maxRadiusMultiplier = 1.25f;

	public void Start()
	{
		resourceURL = resourceURL + "_" + objectSize;
		paPrefab = Resources.Load<ProceduralAsteroid>(resourceURL);
		if (paPrefab == null)
		{
			Debug.Log("Cannot find PA at URL '" + resourceURL + "'");
			return;
		}
		radius = paPrefab.radius * Random.Range(minRadiusMultiplier, maxRadiusMultiplier);
		paGenerated = paPrefab.Generate(seed, radius, base.transform, RangefinderGeneric, delegate
		{
		});
	}

	[ContextMenu("Randomize")]
	public void Randomize()
	{
		NewSeed();
		Rebuild();
	}

	[ContextMenu("Rebuild")]
	public void Rebuild()
	{
		if (!(paPrefab == null) && !(paGenerated == null) && Application.isPlaying)
		{
			Object.Destroy(paGenerated.gameObject);
			if ((bool)GetComponent<Rigidbody>())
			{
				Object.Destroy(GetComponent<Rigidbody>());
			}
			radius = paPrefab.radius * Random.Range(minRadiusMultiplier, maxRadiusMultiplier);
			paGenerated = paPrefab.Generate(seed, radius, base.transform, RangefinderGeneric, delegate
			{
			});
		}
	}

	public float RangefinderGeneric(Transform t)
	{
		return t.position.magnitude;
	}

	[ContextMenu("Add Rigidbody")]
	public void AddRigidbody()
	{
		if (!(paPrefab == null) && !(paGenerated == null) && Application.isPlaying)
		{
			base.gameObject.AddComponent<Rigidbody>().mass = density * paGenerated.volume;
		}
	}

	[ContextMenu("New Seed")]
	public void NewSeed()
	{
		seed = Random.Range(int.MinValue, int.MaxValue);
	}
}
