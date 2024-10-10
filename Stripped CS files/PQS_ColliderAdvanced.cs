using System.Collections.Generic;
using UnityEngine;

public class PQS_ColliderAdvanced : MonoBehaviour
{
	public enum NormSet
	{
		Grav,
		Velocity
	}

	public GClass4 sphere;

	public Vector2 colliderSize;

	public bool useGravityCollider;

	public Collider gravityCollider;

	public bool isGravityColliderActive;

	public bool useVelocityCollider;

	public Collider velocityCollider;

	public Collider velocityCliffCollider;

	public bool isVelocityColliderActive;

	public bool collidersVisibleDEBUG;

	public bool showNormalsDEBUG;

	public static int maxVelocityIteration = 4;

	public static float velocityThinkAhead = 2f;

	public static float halfColliderHeight = 100f;

	public static float halfCliffHeight = 500f;

	public static double sampleAngle = 0.0001;

	public static QuaternionD[] sampleRotationsD;

	public static List<PQS_ColliderAdvanced> colliderList;

	public Vector3d[] samplesD;

	public Vector3d edgeABd;

	public Vector3d edgeACd;

	public Vector3d normal;

	public Rigidbody _rigidbody;

	public Vector3 relPos;

	public Vector3 relVel;

	public Vector3d relDir;

	public double radDist;

	public double surfaceHeight;

	public double altitude;

	public double radSpeed;

	public Vector3d planeVelocity;

	public double verticalSpeed;

	public Vector3d vel;

	public double speed;

	public Vector3d hitPoint;

	public double surfHeight;

	public GameObject[,] cubes;

	public void Reset()
	{
		colliderSize = new Vector2(1f, 1f);
	}

	public void RegisterColliders()
	{
		if (colliderList == null)
		{
			colliderList = new List<PQS_ColliderAdvanced>();
		}
		foreach (PQS_ColliderAdvanced collider in colliderList)
		{
			if (useGravityCollider)
			{
				Physics.IgnoreCollision(collider.GetComponent<Collider>(), gravityCollider);
				if (collider.useGravityCollider)
				{
					Physics.IgnoreCollision(collider.gravityCollider, gravityCollider);
				}
			}
			if (useVelocityCollider)
			{
				Physics.IgnoreCollision(collider.GetComponent<Collider>(), velocityCollider);
				Physics.IgnoreCollision(collider.GetComponent<Collider>(), velocityCliffCollider);
				if (collider.useVelocityCollider)
				{
					Physics.IgnoreCollision(collider.velocityCollider, velocityCollider);
					Physics.IgnoreCollision(collider.velocityCollider, velocityCliffCollider);
					Physics.IgnoreCollision(collider.velocityCliffCollider, velocityCollider);
					Physics.IgnoreCollision(collider.velocityCliffCollider, velocityCliffCollider);
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
	}

	public void ActivateGravityCollider()
	{
		if (!isGravityColliderActive)
		{
			isGravityColliderActive = true;
			Physics.IgnoreCollision(GetComponent<Collider>(), gravityCollider, ignore: false);
		}
	}

	public void DeactivateGravityCollider()
	{
		if (isGravityColliderActive)
		{
			isGravityColliderActive = false;
			Physics.IgnoreCollision(GetComponent<Collider>(), gravityCollider, ignore: true);
		}
	}

	public void ActivateVelocityCollider()
	{
		if (!isVelocityColliderActive)
		{
			isVelocityColliderActive = true;
			Physics.IgnoreCollision(GetComponent<Collider>(), velocityCollider, ignore: false);
			Physics.IgnoreCollision(GetComponent<Collider>(), velocityCliffCollider, ignore: false);
		}
	}

	public void DeactivateVelocityCollider()
	{
		if (isVelocityColliderActive)
		{
			isVelocityColliderActive = false;
			Physics.IgnoreCollision(GetComponent<Collider>(), velocityCollider, ignore: true);
			Physics.IgnoreCollision(GetComponent<Collider>(), velocityCliffCollider, ignore: true);
		}
	}

	public void Start()
	{
		if (sampleRotationsD == null)
		{
			sampleRotationsD = new QuaternionD[3]
			{
				QuaternionD.AngleAxis(sampleAngle, Vector3.right),
				QuaternionD.AngleAxis(sampleAngle, Vector3.left),
				QuaternionD.AngleAxis(sampleAngle, Vector3.forward)
			};
		}
		if (useGravityCollider)
		{
			if (collidersVisibleDEBUG)
			{
				gravityCollider = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<BoxCollider>();
			}
			else
			{
				gravityCollider = new GameObject().AddComponent<BoxCollider>();
			}
			gravityCollider.gameObject.name = base.gameObject.name + "ColGrav";
			gravityCollider.transform.localScale = new Vector3(colliderSize.x, halfColliderHeight * 2f, colliderSize.y);
			isGravityColliderActive = true;
		}
		if (useVelocityCollider)
		{
			if (collidersVisibleDEBUG)
			{
				velocityCollider = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<BoxCollider>();
			}
			else
			{
				velocityCollider = new GameObject().AddComponent<BoxCollider>();
			}
			velocityCollider.gameObject.name = base.gameObject.name + "ColVel";
			velocityCollider.transform.localScale = new Vector3(colliderSize.x, halfColliderHeight * 2f, colliderSize.y);
			if (collidersVisibleDEBUG)
			{
				velocityCliffCollider = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<BoxCollider>();
			}
			else
			{
				velocityCliffCollider = new GameObject().AddComponent<BoxCollider>();
			}
			velocityCliffCollider.gameObject.name = base.gameObject.name + "ColVelCliff";
			velocityCliffCollider.transform.localScale = new Vector3(colliderSize.x, halfCliffHeight * 2f, colliderSize.y);
			isVelocityColliderActive = true;
		}
		if (collidersVisibleDEBUG)
		{
			cubes = new GameObject[2, 3];
			for (int i = 0; i < 2; i++)
			{
				Color color = i switch
				{
					0 => Color.red, 
					1 => Color.green, 
					_ => Color.blue, 
				};
				for (int j = 0; j < 3; j++)
				{
					cubes[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cubes[i, j].transform.localScale = new Vector3(0.1f, 2f, 0.1f);
					cubes[i, j].GetComponent<Renderer>().material.color = color * ((float)j / 4f + 0.25f);
					Object.Destroy(cubes[i, j].GetComponent<Collider>());
				}
			}
		}
		RegisterColliders();
		samplesD = new Vector3d[3];
		SetTargetSphere(sphere);
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
		if (sphere == null)
		{
			if (useGravityCollider)
			{
				gravityCollider.transform.parent = base.transform;
				gravityCollider.transform.localPosition = Vector3.zero;
				DeactivateGravityCollider();
			}
			if (useVelocityCollider)
			{
				velocityCollider.transform.parent = base.transform;
				velocityCollider.transform.localPosition = Vector3.zero;
				velocityCliffCollider.transform.parent = base.transform;
				velocityCliffCollider.transform.localPosition = Vector3.zero;
				DeactivateVelocityCollider();
			}
		}
		else
		{
			if (useGravityCollider)
			{
				gravityCollider.transform.parent = sphere.transform;
				gravityCollider.transform.localPosition = Vector3.zero;
			}
			if (useVelocityCollider)
			{
				velocityCollider.transform.parent = sphere.transform;
				velocityCollider.transform.localPosition = Vector3.zero;
				velocityCliffCollider.transform.parent = sphere.transform;
				velocityCliffCollider.transform.localPosition = Vector3.zero;
			}
			UpdateColliders();
		}
	}

	public void FixedUpdate()
	{
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
		altitude = radDist - sphere.GetSurfaceHeight(relPos);
		if (altitude < -0.25)
		{
			ActivateGravityCollider();
			UpdateVerticalCollider(gravityCollider.transform, surfaceHeight - (double)halfColliderHeight, relDir);
			base.transform.position = relDir * (surfaceHeight + 1.0);
			this.GetComponentCached(ref _rigidbody).velocity *= 0.8f;
			return;
		}
		radSpeed = (double)(relPos + relVel).magnitude - radDist;
		if (radDist > sphere.radiusMax && radDist + radSpeed > sphere.radiusMax)
		{
			DeactivateGravityCollider();
			return;
		}
		planeVelocity = Quaternion.FromToRotation(relDir, Vector3d.up) * relVel;
		verticalSpeed = planeVelocity.y;
		if (verticalSpeed < 100.0 && altitude + verticalSpeed < sphere.radiusMax)
		{
			ActivateGravityCollider();
			UpdateNormalCollider(gravityCollider.transform, halfColliderHeight, relDir);
			ShowDebugNormals(0, relPos);
		}
		else
		{
			DeactivateGravityCollider();
		}
	}

	public void UpdateVelocityCollider()
	{
		vel = relVel.normalized;
		speed = relVel.magnitude * velocityThinkAhead;
		hitPoint = Vector3.zero;
		surfHeight = 0.0;
		if (speed > 0.25 && IterateVelocity(relPos, vel, speed, 0, maxVelocityIteration))
		{
			ActivateVelocityCollider();
			ShowDebugNormals(1, hitPoint);
			hitPoint = hitPoint.normalized;
			UpdateNormalCollider(velocityCollider.transform, halfColliderHeight, hitPoint);
			UpdateVerticalCollider(velocityCliffCollider.transform, (float)surfHeight - halfCliffHeight - 1f, hitPoint);
		}
		else
		{
			DeactivateVelocityCollider();
		}
	}

	public bool IterateVelocity(Vector3d start, Vector3d vel, double length, int itr, int maxItr)
	{
		hitPoint = start + vel * length;
		surfHeight = sphere.GetSurfaceHeight(hitPoint);
		if (hitPoint.magnitude <= surfHeight + 0.10000000149011612)
		{
			if (itr < maxItr)
			{
				return IterateVelocity(start, vel, length / 2.0, itr + 1, maxItr);
			}
			return true;
		}
		return false;
	}

	public void UpdateVerticalCollider(Transform col, double surfaceHeight, Vector3d radianNormalized)
	{
		col.localPosition = radianNormalized * surfaceHeight;
		col.up = radianNormalized;
	}

	public void UpdateNormalCollider(Transform col, double depth, Vector3d radianNormalized)
	{
		normal = GetNormalAtPoint(radianNormalized);
		col.localPosition = radianNormalized * sphere.GetSurfaceHeight(radianNormalized) - normal * depth;
		col.up = normal;
	}

	public void ShowDebugNormals(int set, Vector3d radPos)
	{
		if (showNormalsDEBUG)
		{
			Vector3d normalized = radPos.normalized;
			for (int i = 0; i < 3; i++)
			{
				samplesD[i] = sampleRotationsD[i] * normalized;
				samplesD[i] *= sphere.GetSurfaceHeight(samplesD[i]);
				Vector3d normalized2 = samplesD[i].normalized;
				cubes[set, i].transform.position = samplesD[i] + normalized2;
				cubes[set, i].transform.up = normalized2;
			}
		}
	}

	public Vector3d GetNormalAtPoint(Vector3d radian)
	{
		for (int i = 0; i < 3; i++)
		{
			samplesD[i] = sampleRotationsD[i] * radian;
			samplesD[i] *= sphere.GetSurfaceHeight(samplesD[i]);
		}
		edgeABd = samplesD[1] - samplesD[0];
		edgeACd = samplesD[2] - samplesD[0];
		if (radian.y >= 0.0)
		{
			return new Vector3d(edgeABd.y * edgeACd.z - edgeABd.z * edgeACd.y, edgeABd.z * edgeACd.x - edgeABd.x * edgeACd.z, edgeABd.x * edgeACd.y - edgeABd.y * edgeACd.x).normalized;
		}
		return new Vector3d(edgeABd.z * edgeACd.y - edgeABd.y * edgeACd.z, edgeABd.x * edgeACd.z - edgeABd.z * edgeACd.x, edgeABd.y * edgeACd.x - edgeABd.x * edgeACd.y).normalized;
	}

	public void OnReferenceBodyChange(CelestialBody body)
	{
		SetTargetSphere(body.pqsController);
	}
}
