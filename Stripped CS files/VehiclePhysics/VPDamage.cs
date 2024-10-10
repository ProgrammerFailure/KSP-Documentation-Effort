using System;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Effects/Damage", 1)]
public class VPDamage : VehicleBehaviour
{
	public MeshFilter[] meshes = new MeshFilter[0];

	public MeshCollider[] colliders = new MeshCollider[0];

	public Transform[] nodes = new Transform[0];

	[Space(5f)]
	public float minVelocity = 1f;

	public float multiplier = 1f;

	[Space(5f)]
	public float damageRadius = 1f;

	public float maxDisplacement = 0.5f;

	public float maxVertexFracture = 0.1f;

	[Space(5f)]
	public float nodeDamageRadius = 0.5f;

	public float maxNodeRotation = 14f;

	public float nodeRotationRate = 10f;

	[Space(5f)]
	public float vertexRepairRate = 0.1f;

	public bool enableRepairKey = true;

	public KeyCode repairKey = KeyCode.R;

	public Vector3[][] m_originalMeshes;

	public Vector3[][] m_originalColliders;

	public Vector3[] m_originalNodePositions;

	public Quaternion[] m_originalNodeRotations;

	public bool m_repairing;

	public float m_meshDamage;

	public float m_colliderDamage;

	public float m_nodeDamage;

	public bool isRepairing => m_repairing;

	public float meshDamage => m_meshDamage;

	public float colliderDamage => m_colliderDamage;

	public float nodeDamage => m_nodeDamage;

	public override void OnEnableComponent()
	{
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onImpact = (Action)Delegate.Combine(vehicleBase.onImpact, new Action(ProcessImpact));
		base.vehicle.cachedRigidbody.inertiaTensor = base.vehicle.cachedRigidbody.inertiaTensor;
		m_originalMeshes = new Vector3[meshes.Length][];
		for (int i = 0; i < meshes.Length; i++)
		{
			Mesh mesh = meshes[i].mesh;
			m_originalMeshes[i] = mesh.vertices;
			mesh.MarkDynamic();
		}
		m_originalColliders = new Vector3[colliders.Length][];
		for (int j = 0; j < colliders.Length; j++)
		{
			Mesh sharedMesh = colliders[j].sharedMesh;
			m_originalColliders[j] = sharedMesh.vertices;
		}
		m_originalNodePositions = new Vector3[nodes.Length];
		m_originalNodeRotations = new Quaternion[nodes.Length];
		for (int k = 0; k < nodes.Length; k++)
		{
			m_originalNodePositions[k] = nodes[k].transform.localPosition;
			m_originalNodeRotations[k] = nodes[k].transform.localRotation;
		}
		m_repairing = false;
		m_meshDamage = 0f;
		m_colliderDamage = 0f;
		m_nodeDamage = 0f;
	}

	public override void OnDisableComponent()
	{
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onImpact = (Action)Delegate.Remove(vehicleBase.onImpact, new Action(ProcessImpact));
		RestoreMeshes();
		RestoreNodes();
		RestoreColliders();
		m_repairing = false;
		m_meshDamage = 0f;
		m_colliderDamage = 0f;
		m_nodeDamage = 0f;
	}

	public void Update()
	{
		if (enableRepairKey && Input.GetKeyDown(repairKey))
		{
			m_repairing = true;
		}
		ProcessRepair();
	}

	public void Repair()
	{
		m_repairing = true;
	}

	public void ProcessImpact()
	{
		Vector3 vector = Vector3.zero;
		if (base.vehicle.localImpactVelocity.sqrMagnitude > minVelocity * minVelocity)
		{
			vector = base.vehicle.cachedTransform.TransformDirection(base.vehicle.localImpactVelocity) * multiplier * 0.02f;
		}
		if (vector.sqrMagnitude > 0f)
		{
			Vector3 contactPoint = base.transform.TransformPoint(base.vehicle.localImpactPosition);
			int i = 0;
			for (int num = meshes.Length; i < num; i++)
			{
				m_meshDamage += DeformMesh(meshes[i].mesh, m_originalMeshes[i], meshes[i].transform, contactPoint, vector);
			}
			m_colliderDamage = DeformColliders(contactPoint, vector);
			int j = 0;
			for (int num2 = nodes.Length; j < num2; j++)
			{
				m_nodeDamage += DeformNode(nodes[j], m_originalNodePositions[j], m_originalNodeRotations[j], contactPoint, vector * 0.5f);
			}
		}
	}

	public float DeformMesh(Mesh mesh, Vector3[] originalMesh, Transform localTransform, Vector3 contactPoint, Vector3 contactVelocity)
	{
		Vector3[] vertices = mesh.vertices;
		float num = damageRadius * damageRadius;
		float num2 = maxDisplacement * maxDisplacement;
		Vector3 vector = localTransform.InverseTransformPoint(contactPoint);
		Vector3 vector2 = localTransform.InverseTransformDirection(contactVelocity);
		float num3 = 0f;
		int num4 = 0;
		for (int i = 0; i < vertices.Length; i++)
		{
			float sqrMagnitude = (vector - vertices[i]).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				Vector3 vector3 = vector2 * (damageRadius - Mathf.Sqrt(sqrMagnitude)) / damageRadius + UnityEngine.Random.onUnitSphere * maxVertexFracture;
				vertices[i] += vector3;
				Vector3 vector4 = vertices[i] - originalMesh[i];
				if (vector4.sqrMagnitude > num2)
				{
					vertices[i] = originalMesh[i] + vector4.normalized * maxDisplacement;
				}
				num3 += vector3.magnitude;
				num4++;
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		if (num4 <= 0)
		{
			return 0f;
		}
		return num3 / (float)num4;
	}

	public float DeformNode(Transform transform_0, Vector3 originalLocalPos, Quaternion originalLocalRot, Vector3 contactPoint, Vector3 contactVelocity)
	{
		float sqrMagnitude = (contactPoint - transform_0.position).sqrMagnitude;
		float num = (damageRadius - Mathf.Sqrt(sqrMagnitude)) / damageRadius;
		float num2 = 0f;
		if (sqrMagnitude < damageRadius * damageRadius)
		{
			Vector3 vector = contactVelocity * num + UnityEngine.Random.onUnitSphere * maxVertexFracture;
			transform_0.position += vector;
			Vector3 vector2 = transform_0.localPosition - originalLocalPos;
			if (vector2.sqrMagnitude > maxDisplacement * maxDisplacement)
			{
				transform_0.localPosition = originalLocalPos + vector2.normalized * maxDisplacement;
			}
			num2 += vector.magnitude;
		}
		if (sqrMagnitude < nodeDamageRadius * nodeDamageRadius)
		{
			Vector3 vector3 = AnglesToVector(transform_0.localEulerAngles);
			Vector3 vector4 = new Vector3(maxNodeRotation, maxNodeRotation, maxNodeRotation);
			Vector3 vector5 = vector3 + vector4;
			Vector3 vector6 = vector3 - vector4;
			Vector3 vector7 = num * nodeRotationRate * UnityEngine.Random.onUnitSphere;
			vector3 += vector7;
			transform_0.localEulerAngles = new Vector3(Mathf.Clamp(vector3.x, vector6.x, vector5.x), Mathf.Clamp(vector3.y, vector6.y, vector5.y), Mathf.Clamp(vector3.z, vector6.z, vector5.z));
			num2 += vector7.magnitude / 45f;
		}
		return num2;
	}

	public Vector3 AnglesToVector(Vector3 Angles)
	{
		if (Angles.x > 180f)
		{
			Angles.x = -360f + Angles.x;
		}
		if (Angles.y > 180f)
		{
			Angles.y = -360f + Angles.y;
		}
		if (Angles.z > 180f)
		{
			Angles.z = -360f + Angles.z;
		}
		return Angles;
	}

	public float DeformColliders(Vector3 contactPoint, Vector3 impactVelocity)
	{
		if (colliders.Length != 0)
		{
			Vector3 centerOfMass = base.vehicle.cachedRigidbody.centerOfMass;
			float num = 0f;
			int i = 0;
			for (int num2 = colliders.Length; i < num2; i++)
			{
				Mesh mesh = new Mesh();
				mesh.vertices = colliders[i].sharedMesh.vertices;
				mesh.triangles = colliders[i].sharedMesh.triangles;
				num += DeformMesh(mesh, m_originalColliders[i], colliders[i].transform, contactPoint, impactVelocity);
				colliders[i].sharedMesh = mesh;
			}
			base.vehicle.cachedRigidbody.centerOfMass = centerOfMass;
			return num;
		}
		return 0f;
	}

	public void ProcessRepair()
	{
		if (m_repairing)
		{
			float repairedThreshold = 0.002f;
			bool flag = true;
			int i = 0;
			for (int num = meshes.Length; i < num; i++)
			{
				flag = RepairMesh(meshes[i].mesh, m_originalMeshes[i], vertexRepairRate, repairedThreshold) && flag;
			}
			int j = 0;
			for (int num2 = nodes.Length; j < num2; j++)
			{
				flag = RepairNode(nodes[j], m_originalNodePositions[j], m_originalNodeRotations[j], vertexRepairRate, repairedThreshold) && flag;
			}
			if (flag)
			{
				m_repairing = false;
				m_meshDamage = 0f;
				m_colliderDamage = 0f;
				m_nodeDamage = 0f;
				RestoreNodes();
				RestoreColliders();
			}
		}
	}

	public bool RepairMesh(Mesh mesh, Vector3[] originalMesh, float repairRate, float repairedThreshold)
	{
		bool result = true;
		Vector3[] vertices = mesh.vertices;
		repairRate *= Time.deltaTime;
		repairedThreshold *= repairedThreshold;
		int i = 0;
		for (int num = vertices.Length; i < num; i++)
		{
			vertices[i] = Vector3.MoveTowards(vertices[i], originalMesh[i], repairRate);
			if ((originalMesh[i] - vertices[i]).sqrMagnitude >= repairedThreshold)
			{
				result = false;
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return result;
	}

	public bool RepairNode(Transform transform_0, Vector3 originalLocalPosition, Quaternion originalLocalRotation, float repairRate, float repairedThreshold)
	{
		repairRate *= Time.deltaTime;
		transform_0.localPosition = Vector3.MoveTowards(transform_0.localPosition, originalLocalPosition, repairRate);
		transform_0.localRotation = Quaternion.RotateTowards(transform_0.localRotation, originalLocalRotation, repairRate * 50f);
		if ((originalLocalPosition - transform_0.localPosition).sqrMagnitude < repairedThreshold * repairedThreshold)
		{
			return Quaternion.Angle(originalLocalRotation, transform_0.localRotation) < repairedThreshold;
		}
		return false;
	}

	public void RestoreMeshes()
	{
		int i = 0;
		for (int num = meshes.Length; i < num; i++)
		{
			Mesh mesh = meshes[i].mesh;
			mesh.vertices = m_originalMeshes[i];
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}
	}

	public void RestoreNodes()
	{
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			nodes[i].localPosition = m_originalNodePositions[i];
			nodes[i].localRotation = m_originalNodeRotations[i];
		}
	}

	public void RestoreColliders()
	{
		if (colliders.Length != 0)
		{
			Vector3 centerOfMass = base.vehicle.cachedRigidbody.centerOfMass;
			int i = 0;
			for (int num = colliders.Length; i < num; i++)
			{
				Mesh mesh = new Mesh();
				mesh.vertices = m_originalColliders[i];
				mesh.triangles = colliders[i].sharedMesh.triangles;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				colliders[i].sharedMesh = mesh;
			}
			base.vehicle.cachedRigidbody.centerOfMass = centerOfMass;
		}
	}
}
