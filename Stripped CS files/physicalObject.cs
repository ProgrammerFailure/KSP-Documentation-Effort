using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicalObject : MonoBehaviour
{
	public float maxDistance = 1000f;

	public float origDrag = 1f;

	public Rigidbody rb;

	public float colliderDelay = 0.05f;

	public List<Renderer> renderers = new List<Renderer>();

	public void Awake()
	{
		FlightGlobals.addPhysicalObject(this);
	}

	public IEnumerator Start()
	{
		if (rb == null)
		{
			rb = GetComponent<Rigidbody>();
			if (rb == null)
			{
				rb = base.gameObject.AddComponent<Rigidbody>();
			}
		}
		rb.detectCollisions = false;
		for (float time = 0f; time < colliderDelay; time += Time.fixedDeltaTime)
		{
			yield return new WaitForFixedUpdate();
		}
		rb.detectCollisions = true;
		base.gameObject.SetActive(value: false);
		base.gameObject.SetActive(value: true);
	}

	public void OnDestroy()
	{
		FlightGlobals.removePhysicalObject(this);
		CleanupMaterials();
	}

	public void Update()
	{
		if (TimeWarp.CurrentRate > 1f && TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
		{
			Object.Destroy(base.gameObject);
		}
		if (FlightGlobals.ActiveVessel != null && Vector3.Distance(base.gameObject.transform.position, FlightGlobals.ActiveVessel.gameObject.transform.position) > maxDistance)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public static physicalObject ConvertToPhysicalObject(Part part, GameObject obj)
	{
		physicalObject physicalObject2 = obj.AddComponent<physicalObject>();
		obj.transform.parent = null;
		obj.SetLayerRecursive(LayerMask.NameToLayer("PhysicalObjects"));
		physicalObject2.rb = obj.AddComponent<Rigidbody>();
		physicalObject2.renderers = new List<Renderer>(obj.GetComponentsInChildren<Renderer>());
		physicalObject2.renderers.RemoveNonHighlightableRenderers();
		int i = 0;
		for (int count = physicalObject2.renderers.Count; i < count; i++)
		{
			part.HighlightRenderer.Remove(physicalObject2.renderers[i]);
		}
		part.RefreshHighlighter();
		return physicalObject2;
	}

	public void CleanupMaterials()
	{
		if (renderers == null)
		{
			return;
		}
		int i = 0;
		for (int count = renderers.Count; i < count; i++)
		{
			Renderer renderer = renderers[i];
			if (!(renderer == null))
			{
				Material[] materials = renderer.materials;
				int j = 0;
				for (int num = materials.Length; j < num; j++)
				{
					Object.Destroy(materials[j]);
				}
			}
		}
	}
}
