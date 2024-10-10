using System;
using EdyCommonTools;
using UnityEngine;
using UnityEngine.Rendering;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Force Cones", 2)]
public class VPForceCones : VehicleBehaviour
{
	public struct Cones
	{
		public Transform red;

		public Transform green;

		public Transform blue;

		public Transform gray;
	}

	public float baseLength = 1f;

	public bool showDownforce = true;

	public bool showTireForce = true;

	public bool combinedTireForce = true;

	public bool useLogScale = true;

	public Cones[] m_wheelCones = new Cones[0];

	public override void OnEnableVehicle()
	{
		m_wheelCones = new Cones[base.vehicle.wheelCount];
		int i = 0;
		for (int wheelCount = base.vehicle.wheelCount; i < wheelCount; i++)
		{
			m_wheelCones[i].red = CreateConeObject(i + "_red", GColor.solidRed);
			m_wheelCones[i].green = CreateConeObject(i + "_green", GColor.solidGreen);
			m_wheelCones[i].blue = CreateConeObject(i + "_blue", GColor.solidBlue);
			m_wheelCones[i].gray = CreateConeObject(i + "_gray", GColor.blue);
		}
	}

	public override void OnDisableVehicle()
	{
		Cones[] wheelCones = m_wheelCones;
		for (int i = 0; i < wheelCones.Length; i++)
		{
			Cones cones = wheelCones[i];
			UnityEngine.Object.Destroy(cones.red.gameObject);
			UnityEngine.Object.Destroy(cones.green.gameObject);
			UnityEngine.Object.Destroy(cones.blue.gameObject);
			UnityEngine.Object.Destroy(cones.gray.gameObject);
		}
	}

	public Transform CreateConeObject(string name, Color color)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		gameObject.transform.parent = base.transform;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		int num = 8;
		Vector3[] array = new Vector3[10];
		Vector2[] uv = new Vector2[10];
		int[] array2 = new int[48];
		array[8] = Vector3.zero;
		array[9] = new Vector3(0f, 0f, 1f);
		for (int i = 0; i < num; i++)
		{
			float f = (float)Math.PI * 2f * (float)i / (float)num;
			array[i] = new Vector3(Mathf.Sin(f) * 0.04f, Mathf.Cos(f) * 0.04f, 1f);
			array2[i * 6] = num;
			array2[i * 6 + 1] = i;
			array2[i * 6 + 2] = (i + 1) % num;
			array2[i * 6 + 3] = num + 1;
			array2[i * 6 + 4] = (i + 1) % num;
			array2[i * 6 + 5] = i;
		}
		Mesh mesh = meshFilter.mesh;
		mesh.vertices = array;
		mesh.uv = uv;
		mesh.triangles = array2;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshRenderer.material = new Material(Shader.Find("Self-Illumin/Diffuse"))
		{
			color = color
		};
		meshRenderer.receiveShadows = false;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		gameObject.transform.localScale = Vector3.zero;
		return gameObject.transform;
	}

	public Vector3 ScaledForce(float force)
	{
		return Vector3.one * (useLogScale ? (MathUtility.Lin2Log(force) * baseLength * 0.25f) : (force * 0.001f * baseLength));
	}

	public override void UpdateVehicle()
	{
		for (int i = 0; i < base.vehicle.wheelCount; i++)
		{
			VehicleBase.WheelState wheelState = base.vehicle.wheelState[i];
			if (wheelState.wheelCol.hidden)
			{
				continue;
			}
			WheelHit hit = wheelState.hit;
			wheelState.wheelCol.GetGroundHit(out hit);
			hit.point = wheelState.wheelCol.visualHit.point;
			Cones cones = m_wheelCones[i];
			cones.red.position = hit.point;
			cones.green.position = hit.point;
			cones.blue.position = hit.point;
			cones.gray.position = hit.point;
			if (showDownforce)
			{
				cones.gray.localScale = ScaledForce(wheelState.downforce);
				cones.gray.LookAt(hit.point + hit.normal);
			}
			else
			{
				cones.gray.localScale = Vector3.zero;
			}
			if (showTireForce)
			{
				if (combinedTireForce)
				{
					Vector3 vector = hit.forwardDir * wheelState.tireForce.y + hit.sidewaysDir * wheelState.tireForce.x;
					cones.green.localScale = ScaledForce(wheelState.tireForce.magnitude);
					cones.green.LookAt(hit.point + vector);
					cones.red.localScale = Vector3.zero;
				}
				else
				{
					cones.red.localScale = ScaledForce(wheelState.tireForce.x);
					cones.red.LookAt(hit.point + hit.sidewaysDir);
					cones.blue.localScale = ScaledForce(wheelState.tireForce.y);
					cones.blue.LookAt(hit.point + hit.forwardDir);
					cones.green.localScale = Vector3.zero;
				}
			}
			else
			{
				cones.green.localScale = Vector3.zero;
				cones.red.localScale = Vector3.zero;
				cones.blue.localScale = Vector3.zero;
			}
		}
	}
}
