using UnityEngine;

public class ProceduralCometTest : MonoBehaviour
{
	[SerializeField]
	public string resourceURL;

	[SerializeField]
	public int seed;

	[SerializeField]
	public float radius;

	[SerializeField]
	public float density;

	public ProceduralComet pcPrefab;

	public PComet pcGenerated;

	[SerializeField]
	public UntrackedObjectClass objectSize;

	[SerializeField]
	public float minRadiusMultiplier = 0.75f;

	[SerializeField]
	public float maxRadiusMultiplier = 1.25f;

	public void Start()
	{
		if (resourceURL.Contains("Procedural"))
		{
			resourceURL = resourceURL + "_" + objectSize;
			pcPrefab = Resources.Load<ProceduralComet>(resourceURL);
			if (pcPrefab == null)
			{
				Debug.Log("Cannot find PC at URL '" + resourceURL + "'");
				return;
			}
			radius = pcPrefab.radius * Random.Range(minRadiusMultiplier, maxRadiusMultiplier);
			pcGenerated = pcPrefab.Generate(seed, radius, base.transform, RangefinderGeneric, delegate
			{
			}, optimizeCollider: false);
		}
		else
		{
			GameObject component = Object.Instantiate(Resources.Load<PComet>(resourceURL)).GetComponent<GameObject>();
			component.name = "Comet";
			component.transform.SetParent(base.transform);
			component.transform.localPosition = Vector3.zero;
			component.transform.localRotation = Quaternion.identity;
		}
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
		if (!(pcPrefab == null) && !(pcGenerated == null) && Application.isPlaying)
		{
			Object.Destroy(pcGenerated.gameObject);
			if ((bool)GetComponent<Rigidbody>())
			{
				Object.Destroy(GetComponent<Rigidbody>());
			}
			radius = pcPrefab.radius * Random.Range(minRadiusMultiplier, maxRadiusMultiplier);
			pcGenerated = pcPrefab.Generate(seed, radius, base.transform, RangefinderGeneric, delegate
			{
			}, optimizeCollider: false);
		}
	}

	public float RangefinderGeneric(Transform t)
	{
		return t.position.magnitude;
	}

	[ContextMenu("Add Rigidbody")]
	public void AddRigidbody()
	{
		if (!(pcPrefab == null) && !(pcGenerated == null) && Application.isPlaying)
		{
			base.gameObject.AddComponent<Rigidbody>().mass = density * pcGenerated.volume;
		}
	}

	[ContextMenu("New Seed")]
	public void NewSeed()
	{
		seed = Random.Range(int.MinValue, int.MaxValue);
	}
}
