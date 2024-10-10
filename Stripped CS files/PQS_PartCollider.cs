using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Misc/Collider")]
public class PQS_PartCollider : MonoBehaviour
{
	public GClass4 sphere;

	public Vector2 colliderSize;

	public bool useGravityCollider;

	public bool useVelocityCollider;

	public int layerID;

	[HideInInspector]
	public Collider gravityCollider;

	[HideInInspector]
	public bool isGravityColliderActive;

	[HideInInspector]
	public Collider velocityCollider;

	[HideInInspector]
	public Collider velocityCliffCollider;

	[HideInInspector]
	public bool isVelocityColliderActive;

	[HideInInspector]
	public Part part;

	[HideInInspector]
	public Vector3d[] samplesD;

	[HideInInspector]
	public Collider[] colliders;

	public static Vector3 safeSpot = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

	public static List<PQS_PartCollider> colliderList;

	public static Vector3d edgeABd;

	public static Vector3d edgeACd;

	public static Vector3d normal;

	public static int maxVelocityIteration = 2;

	public static double velocityThinkAhead = 2.0;

	public static double halfColliderHeight = 100.0;

	public static double halfCliffHeight = 500.0;

	public static double sampleAngle = 0.0001;

	public static QuaternionD[] sampleRotationsD;

	public Vector3d relPos;

	public Vector3d relVel;

	public Vector3d relDir;

	public double radDist;

	public double surfaceHeight;

	public double altitude;

	public double radSpeed;

	public Vector3d planeVelocity;

	public double verticalSpeed;

	public Vector3d relVelNorm;

	public double speed;

	public Vector3d hitPoint;

	public double surfHeight;

	public int itr;

	public Quaternion q;

	public Rigidbody _rigidbody;

	public void Reset()
	{
		colliderSize = new Vector2(1f, 1f);
		useVelocityCollider = true;
		useGravityCollider = true;
	}

	public void RegisterColliders()
	{
		if (colliderList == null)
		{
			colliderList = new List<PQS_PartCollider>();
		}
		foreach (PQS_PartCollider collider2 in colliderList)
		{
			if (useGravityCollider)
			{
				Collider[] array = collider2.colliders;
				for (int i = 0; i < array.Length; i++)
				{
					Physics.IgnoreCollision(array[i], gravityCollider);
				}
				if (collider2.useGravityCollider)
				{
					Physics.IgnoreCollision(collider2.gravityCollider, gravityCollider);
				}
			}
			if (useVelocityCollider)
			{
				Collider[] array = collider2.colliders;
				foreach (Collider collider in array)
				{
					Physics.IgnoreCollision(collider, velocityCollider);
					Physics.IgnoreCollision(collider, velocityCliffCollider);
				}
				if (collider2.useVelocityCollider)
				{
					Physics.IgnoreCollision(collider2.velocityCollider, velocityCollider);
					Physics.IgnoreCollision(collider2.velocityCollider, velocityCliffCollider);
					Physics.IgnoreCollision(collider2.velocityCliffCollider, velocityCollider);
					Physics.IgnoreCollision(collider2.velocityCliffCollider, velocityCliffCollider);
				}
			}
		}
		if (useVelocityCollider)
		{
			Physics.IgnoreCollision(velocityCollider, velocityCliffCollider);
			Physics.IgnoreCollision(velocityCliffCollider, velocityCollider);
			if (useGravityCollider)
			{
				Physics.IgnoreCollision(velocityCollider, gravityCollider);
				Physics.IgnoreCollision(velocityCliffCollider, gravityCollider);
			}
		}
		colliderList.Add(this);
	}

	public void UnregisterColliders()
	{
		colliderList.Remove(this);
		colliders = null;
	}

	public void ActivateGravityCollider()
	{
		if (isGravityColliderActive)
		{
			return;
		}
		isGravityColliderActive = true;
		Collider[] array = colliders;
		foreach (Collider collider in array)
		{
			if (collider != null)
			{
				Physics.IgnoreCollision(collider, gravityCollider, ignore: false);
			}
		}
	}

	public void DeactivateGravityCollider()
	{
		if (!isGravityColliderActive)
		{
			return;
		}
		isGravityColliderActive = false;
		Collider[] array = colliders;
		foreach (Collider collider in array)
		{
			if (collider != null)
			{
				Physics.IgnoreCollision(collider, gravityCollider, ignore: true);
			}
		}
	}

	public void ActivateVelocityCollider()
	{
		if (isVelocityColliderActive)
		{
			return;
		}
		isVelocityColliderActive = true;
		Collider[] array = colliders;
		foreach (Collider collider in array)
		{
			if (collider != null)
			{
				Physics.IgnoreCollision(collider, velocityCollider, ignore: false);
				Physics.IgnoreCollision(collider, velocityCliffCollider, ignore: false);
			}
		}
	}

	public void DeactivateVelocityCollider()
	{
		if (!isVelocityColliderActive)
		{
			return;
		}
		isVelocityColliderActive = false;
		Collider[] array = colliders;
		foreach (Collider collider in array)
		{
			if (collider != null)
			{
				Physics.IgnoreCollision(collider, velocityCollider, ignore: true);
				Physics.IgnoreCollision(collider, velocityCliffCollider, ignore: true);
			}
		}
	}

	public void Awake()
	{
		if (sampleRotationsD == null)
		{
			sampleRotationsD = new QuaternionD[3]
			{
				QuaternionD.AngleAxis(sampleAngle, Vector3d.right),
				QuaternionD.AngleAxis(sampleAngle, Vector3d.left),
				QuaternionD.AngleAxis(sampleAngle, Vector3d.forward)
			};
		}
		samplesD = new Vector3d[3];
		colliders = null;
	}

	public void Start()
	{
		if (useGravityCollider)
		{
			gravityCollider = new GameObject().AddComponent<BoxCollider>();
			gravityCollider.gameObject.name = base.gameObject.name + "ColGrav";
			gravityCollider.transform.localScale = new Vector3(colliderSize.x, (float)(halfColliderHeight * 2.0), colliderSize.y);
			gravityCollider.gameObject.layer = layerID;
			isGravityColliderActive = true;
		}
		if (useVelocityCollider)
		{
			velocityCollider = new GameObject().AddComponent<BoxCollider>();
			velocityCollider.gameObject.name = base.gameObject.name + "ColVel";
			velocityCollider.transform.localScale = new Vector3(colliderSize.x, (float)(halfColliderHeight * 2.0), colliderSize.y);
			velocityCollider.gameObject.layer = layerID;
			velocityCliffCollider = new GameObject().AddComponent<BoxCollider>();
			velocityCliffCollider.gameObject.name = base.gameObject.name + "ColVelCliff";
			velocityCliffCollider.transform.localScale = new Vector3(colliderSize.x, (float)(halfColliderHeight * 2.0), colliderSize.y);
			velocityCliffCollider.gameObject.layer = layerID;
			isVelocityColliderActive = true;
		}
		colliders = GetComponentsInChildren<Collider>();
		RegisterColliders();
		SetTargetSphere(sphere);
		if (part != null)
		{
			UpdateColliders();
		}
	}

	public void OnDestroy()
	{
		UnregisterColliders();
		if (gravityCollider != null)
		{
			Object.Destroy(gravityCollider.gameObject);
		}
		if (velocityCollider != null)
		{
			Object.Destroy(velocityCollider.gameObject);
		}
		if (velocityCliffCollider != null)
		{
			Object.Destroy(velocityCliffCollider.gameObject);
		}
	}

	public void SetTargetSphere(GClass4 sphere)
	{
		this.sphere = sphere;
		if (colliders == null)
		{
			return;
		}
		if (sphere == null)
		{
			if (useGravityCollider)
			{
				gravityCollider.transform.position = safeSpot;
				DeactivateGravityCollider();
			}
			if (useVelocityCollider)
			{
				velocityCollider.transform.position = safeSpot;
				velocityCliffCollider.transform.position = safeSpot;
				DeactivateVelocityCollider();
			}
		}
		else
		{
			if (useGravityCollider)
			{
				gravityCollider.transform.position = safeSpot;
			}
			if (useVelocityCollider)
			{
				velocityCollider.transform.position = safeSpot;
				velocityCliffCollider.transform.position = safeSpot;
			}
		}
	}

	public void FixedUpdate()
	{
		if (part == null)
		{
			if (GetComponent<Rigidbody>() == null || this.GetComponentCached(ref _rigidbody).isKinematic)
			{
				return;
			}
		}
		else
		{
			if (part.packed || GetComponent<Rigidbody>() == null || this.GetComponentCached(ref _rigidbody).isKinematic || (part.vessel != null && part.vessel.Landed))
			{
				return;
			}
			if (part.vessel.orbitDriver.referenceBody.pqsController != sphere)
			{
				SetTargetSphere(part.vessel.orbitDriver.referenceBody.pqsController);
			}
		}
		UpdateColliders();
	}

	public void UpdateColliders()
	{
		if (!(sphere == null))
		{
			relPos = sphere.GetRelativePosition(base.transform.position);
			relVel = sphere.GetRelativeDirection(this.GetComponentCached(ref _rigidbody).velocity);
			if (useGravityCollider)
			{
				UpdateGravityCollider();
			}
			if (useVelocityCollider)
			{
				UpdateVelocityCollider();
			}
		}
	}

	public void UpdateGravityCollider()
	{
		relDir = relPos.normalized;
		radDist = relPos.magnitude;
		surfaceHeight = sphere.GetSurfaceHeight(relPos);
		altitude = radDist - surfaceHeight;
		if (altitude < -0.25)
		{
			ActivateGravityCollider();
			UpdateVerticalCollider(gravityCollider.transform, surfaceHeight - halfColliderHeight, relDir);
			base.transform.position = sphere.GetWorldPosition(relDir * (surfaceHeight + 1.0));
			this.GetComponentCached(ref _rigidbody).velocity *= 0.8f;
			return;
		}
		radSpeed = (relPos + relVel).magnitude - radDist;
		if (radDist > sphere.radiusMax && radDist + radSpeed > sphere.radiusMax)
		{
			DeactivateGravityCollider();
			return;
		}
		q = Quaternion.FromToRotation(relDir, Vector3d.up);
		planeVelocity = q * relVel;
		verticalSpeed = planeVelocity.y;
		if (verticalSpeed < 0.0 && altitude + verticalSpeed < sphere.radiusMax)
		{
			ActivateGravityCollider();
			UpdateNormalCollider(gravityCollider.transform, halfColliderHeight, relDir);
		}
		else
		{
			DeactivateGravityCollider();
		}
	}

	public void UpdateVelocityCollider()
	{
		relVelNorm = relVel.normalized;
		speed = relVel.magnitude * velocityThinkAhead;
		hitPoint = Vector3.zero;
		surfHeight = 0.0;
		itr = 0;
		if (speed > 0.25 && IterateVelocity())
		{
			ActivateVelocityCollider();
			hitPoint = hitPoint.normalized;
			UpdateNormalCollider(velocityCollider.transform, halfColliderHeight, hitPoint);
			UpdateVerticalCollider(velocityCliffCollider.transform, surfHeight - halfCliffHeight - 1.0, hitPoint);
		}
		else
		{
			DeactivateVelocityCollider();
		}
	}

	public bool IterateVelocity()
	{
		hitPoint = relPos + relVelNorm * speed;
		surfHeight = sphere.GetSurfaceHeight(hitPoint);
		if (hitPoint.magnitude <= surfHeight + 0.1)
		{
			if (itr < maxVelocityIteration)
			{
				itr++;
				speed *= 0.5;
				return IterateVelocity();
			}
			return true;
		}
		return false;
	}

	public void UpdateVerticalCollider(Transform col, double surfaceHeight, Vector3d radianNormalized)
	{
		col.position = sphere.GetWorldPosition(radianNormalized * surfaceHeight);
		col.up = radianNormalized;
	}

	public void UpdateNormalCollider(Transform col, double depth, Vector3d radianNormalized)
	{
		GetNormalAtPoint(radianNormalized);
		col.position = sphere.GetWorldPosition(radianNormalized * sphere.GetSurfaceHeight(radianNormalized) - normal * depth);
		col.up = normal;
	}

	public void GetNormalAtPoint(Vector3d radian)
	{
		for (itr = 0; itr < 3; itr++)
		{
			samplesD[itr] = sampleRotationsD[itr] * radian;
			samplesD[itr] *= sphere.GetSurfaceHeight(samplesD[itr]);
		}
		edgeABd = samplesD[1] - samplesD[0];
		edgeACd = samplesD[2] - samplesD[0];
		if (radian.y >= 0.0)
		{
			normal = new Vector3d(edgeABd.y * edgeACd.z - edgeABd.z * edgeACd.y, edgeABd.z * edgeACd.x - edgeABd.x * edgeACd.z, edgeABd.x * edgeACd.y - edgeABd.y * edgeACd.x).normalized;
		}
		else
		{
			normal = new Vector3d(edgeABd.z * edgeACd.y - edgeABd.y * edgeACd.z, edgeABd.x * edgeACd.z - edgeABd.z * edgeACd.x, edgeABd.y * edgeACd.x - edgeABd.x * edgeACd.y).normalized;
		}
	}

	public void OnReferenceBodyChange(CelestialBody body)
	{
		SetTargetSphere(body.pqsController);
	}
}
