using UnityEngine;

public class PQS_Collider : MonoBehaviour
{
	public GClass4 sphere;

	public float colliderDepth;

	public float colliderMaxDistance;

	public bool collidersVisibleDEBUG;

	public GameObject colliderObject;

	public bool isColliderActive;

	public float halfDepth;

	public void Reset()
	{
		colliderDepth = 100f;
		colliderMaxDistance = 10f;
	}

	public void Start()
	{
		if (collidersVisibleDEBUG)
		{
			colliderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		else
		{
			colliderObject = new GameObject();
			colliderObject.AddComponent<BoxCollider>();
		}
		colliderObject.transform.localScale = new Vector3(1f, colliderDepth, 1f);
		halfDepth = colliderDepth / 2f;
		colliderObject.gameObject.name = base.gameObject.name + "PQSCollider";
		colliderObject.SetActive(value: false);
		isColliderActive = false;
		SetTargetSphere(sphere);
	}

	public void SetTargetSphere(GClass4 sphere)
	{
		this.sphere = sphere;
		if (sphere == null)
		{
			colliderObject.transform.parent = base.transform;
			colliderObject.transform.localPosition = Vector3.zero;
			DeactivateCollider();
		}
		else
		{
			colliderObject.transform.parent = sphere.transform;
			UpdateCollider();
		}
	}

	public void FixedUpdate()
	{
		UpdateCollider();
	}

	public void UpdateCollider()
	{
		if (!(sphere == null))
		{
			Vector3 vector = sphere.GetRelativePosition(base.transform.position);
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			float num = (float)sphere.GetSurfaceHeight(vector);
			if (magnitude - num < colliderMaxDistance)
			{
				ActivateCollider();
				colliderObject.transform.localPosition = normalized * (num - halfDepth);
				colliderObject.transform.up = normalized;
			}
			else
			{
				DeactivateCollider();
			}
		}
	}

	public void ActivateCollider()
	{
		if (!isColliderActive)
		{
			colliderObject.SetActive(value: true);
			isColliderActive = true;
		}
	}

	public void DeactivateCollider()
	{
		if (isColliderActive)
		{
			colliderObject.SetActive(value: false);
			isColliderActive = false;
		}
	}
}
