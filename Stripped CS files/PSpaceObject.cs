using System;
using UnityEngine;

public abstract class PSpaceObject : MonoBehaviour
{
	[SerializeField]
	public GameObject visualObject;

	[SerializeField]
	public Mesh visualMesh;

	[SerializeField]
	public bool procedural = true;

	public Renderer visualRenderer;

	[SerializeField]
	public GameObject colliderObject;

	[SerializeField]
	public Mesh colliderMesh;

	public SpaceObjectCollider soc;

	public Part partComponent;

	[SerializeField]
	public GameObject convexObject;

	[SerializeField]
	public Mesh convexColliderMesh;

	[SerializeField]
	public int genTime;

	public float volume;

	public float highestPoint;

	[SerializeField]
	public float maxRange = 2500f;

	[SerializeField]
	public float minRange;

	public Callback onGenComplete;

	public Renderer VisualRenderer
	{
		get
		{
			if (visualRenderer == null && visualObject != null)
			{
				visualObject.GetComponentCached(ref visualRenderer);
			}
			return visualRenderer;
		}
	}

	public Mesh ConvexColliderMesh => convexColliderMesh;

	public PSpaceObject()
	{
	}

	public void Awake()
	{
		if (genTime != 0)
		{
			visualMesh = null;
			colliderMesh = null;
			convexColliderMesh = null;
		}
	}

	public void Setup(Mesh visualMesh, Material visualMaterial, string visualLayer, string visualTag, Mesh colliderMesh, PhysicMaterial colliderMaterial, string colliderLayer, string colliderTag, Mesh convexMesh, PhysicMaterial convexMaterial, string convexLayer, string convexTag, Func<Transform, float> rangefinder, Callback onGenComplete)
	{
		this.visualMesh = visualMesh;
		this.colliderMesh = colliderMesh;
		convexColliderMesh = convexMesh;
		this.onGenComplete = onGenComplete;
		CreateVisual(visualMesh, visualMaterial, visualLayer, visualTag);
		CreateCollider(convexMesh, colliderMaterial, colliderLayer, colliderTag, rangefinder);
		genTime = Time.frameCount;
	}

	public void CreateVisual(Mesh visualMesh, Material visualMaterial, string visualLayer, string visualTag)
	{
		if (!(visualMesh == null))
		{
			visualObject = new GameObject("Visual");
			visualObject.transform.NestToParent(base.transform);
			visualObject.layer = LayerMask.NameToLayer(visualLayer);
			visualObject.tag = visualTag;
			visualObject.AddComponent<MeshFilter>().sharedMesh = visualMesh;
			visualObject.AddComponent<MeshRenderer>().sharedMaterial = visualMaterial;
		}
	}

	public void CreateCollider(Mesh colliderMesh, PhysicMaterial colliderMaterial, string colliderLayer, string colliderTag, Func<Transform, float> rangefinder)
	{
		if (!(colliderMesh == null))
		{
			colliderObject = new GameObject("Collider");
			colliderObject.transform.NestToParent(base.transform);
			colliderObject.layer = LayerMask.NameToLayer(colliderLayer);
			colliderObject.tag = colliderTag;
			soc = colliderObject.AddComponent<SpaceObjectCollider>();
			soc.Setup(this, colliderMesh, Vector3.zero, rangefinder, maxRange, minRange, onSOCCSetupComplete);
		}
	}

	public abstract void SetupPartParameters();

	public void CreateConvexCollider(Mesh convexMesh, PhysicMaterial convexMaterial, string convexLayer, string convexTag)
	{
		if (!(convexMesh == null))
		{
			convexObject = new GameObject("ColliderConvex");
			convexObject.transform.NestToParent(base.transform);
			convexObject.layer = LayerMask.NameToLayer(convexLayer);
			convexObject.tag = convexTag;
			MeshCollider meshCollider = convexObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = convexMesh;
			meshCollider.sharedMaterial = convexMaterial;
			meshCollider.convex = true;
		}
	}

	public void onSOCCSetupComplete()
	{
		if (convexObject != null)
		{
			convexObject.SetActive(value: false);
		}
		onGenComplete();
	}

	public void OnDestroy()
	{
		if (procedural)
		{
			if (visualMesh != null)
			{
				UnityEngine.Object.Destroy(visualMesh);
			}
			if (colliderMesh != null)
			{
				UnityEngine.Object.Destroy(colliderMesh);
			}
			if (convexColliderMesh != null)
			{
				UnityEngine.Object.Destroy(convexColliderMesh);
			}
		}
	}

	public void SetMaterialColor(string name, Color value)
	{
		if (!(visualObject == null))
		{
			Renderer componentCached = visualObject.GetComponentCached(ref visualRenderer);
			if (componentCached != null)
			{
				componentCached.SetupProperties(new ColorMaterialProperty(name, value));
			}
		}
	}

	public Renderer GetVisualObjectRenderer()
	{
		visualObject.GetComponentCached(ref visualRenderer);
		return visualRenderer;
	}
}
