using System.Collections;
using UnityEngine;

public class KerbalExpressionAI : MonoBehaviour
{
	public ProtoCrewMember protoCrewMember;

	public Mesh idleMesh;

	public Mesh[] wheeeMeshes;

	public Mesh[] panicMeshes;

	public Transform headRef;

	public Transform neckJoint;

	public MeshFilter headMesh;

	public float panicLevel;

	public float wheeeLevel;

	public float idleThreshold = 0.2f;

	public Mesh currentMesh;

	public float headBobAmount = 20f;

	public float updateInterval;

	public float noiseSeed;

	public Quaternion helmetInitRot;

	public float flight_velocity;

	public float flight_angularV;

	public float flight_gee;

	public float fearFactor;

	public float expression;

	public bool running;

	public bool isBadass;

	public float courage;

	public float stupidity;

	public virtual void Awake()
	{
		running = false;
		if ((bool)headMesh)
		{
			currentMesh = headMesh.mesh;
		}
		GameEvents.onPartExplode.Add(explosionReaction);
	}

	public virtual void OnDestroy()
	{
		GameEvents.onPartExplode.Remove(explosionReaction);
	}

	public virtual void explosionReaction(GameEvents.ExplosionReaction er)
	{
		fearFactor = er.magnitude * 100f - er.distance;
	}

	public virtual void Start()
	{
		Kerbal component = GetComponent<Kerbal>();
		if ((bool)component)
		{
			protoCrewMember = component.protoCrewMember;
			updateInterval = component.updateInterval;
			noiseSeed = component.noiseSeed;
		}
		else
		{
			updateInterval = Random.Range(0.35f, 0.65f);
			noiseSeed = Random.Range(0, 20);
		}
		running = true;
		float num = 0f;
		wheeeLevel = 0f;
		float num2 = num;
		num = 0f;
		panicLevel = num2;
		fearFactor = num;
		flight_gee = 1f;
		isBadass = protoCrewMember.isBadass;
		courage = protoCrewMember.courage;
		stupidity = protoCrewMember.stupidity;
		helmetInitRot = neckJoint.transform.localRotation;
		StartCoroutine(kerbalAvatarUpdateCycle());
	}

	public virtual void FixedUpdate()
	{
		flight_angularV = Mathf.Clamp((float)FlightGlobals.ship_angularVelocity.magnitude, 0f, 100f);
		flight_velocity = (float)FlightGlobals.ship_velocity.magnitude;
		flight_gee = ((FlightGlobals.ship_verticalSpeed < 0.0) ? ((float)FlightGlobals.ship_geeForce) : 1f);
	}

	public virtual IEnumerator kerbalAvatarUpdateCycle()
	{
		while (running)
		{
			UpdateExpressionAI();
			UpdateHeadMesh();
			yield return new WaitForSeconds(updateInterval);
		}
	}

	public virtual void Update()
	{
		UpdateHeadTransforms();
	}

	public virtual void UpdateExpressionAI()
	{
		wheeeLevel = 0f;
		wheeeLevel += (1f - Mathf.Clamp01(Mathf.Abs(flight_gee))) * 0.6f;
		wheeeLevel += (isBadass ? (flight_velocity / 100f) : 0f);
		wheeeLevel += Mathf.Clamp01(flight_angularV / 40f) * stupidity;
		wheeeLevel -= fearFactor;
		wheeeLevel *= 1f + stupidity * 2f;
		panicLevel = (isBadass ? 0f : (flight_velocity / 100f * (1f - stupidity)));
		panicLevel += (isBadass ? (flight_angularV / 100f) : (flight_angularV / 50f));
		panicLevel += fearFactor / (1f + stupidity);
		fearFactor = ((panicLevel < 0.3f) ? (fearFactor * (1f - courage * 0.5f)) : (fearFactor * (1f - courage * 0.2f)));
		wheeeLevel = Mathf.Clamp01(wheeeLevel);
		panicLevel = Mathf.Clamp01(panicLevel);
		expression = (wheeeLevel * 0.5f - panicLevel * 0.5f) * 2f;
	}

	public virtual void UpdateHeadMesh()
	{
		if ((bool)headMesh)
		{
			if (Mathf.Abs(expression) > idleThreshold)
			{
				currentMesh = ((expression > 0f) ? wheeeMeshes[Mathf.RoundToInt(Mathf.Abs(expression) * (float)(wheeeMeshes.Length - 1))] : panicMeshes[Mathf.RoundToInt(Mathf.Abs(expression) * (float)(panicMeshes.Length - 1))]);
			}
			else
			{
				currentMesh = idleMesh;
			}
			headMesh.mesh = currentMesh;
		}
	}

	public virtual void UpdateHeadTransforms()
	{
		if ((bool)headRef || (bool)neckJoint)
		{
			float num = Mathf.Max(Mathf.Max(panicLevel, wheeeLevel), 0.1f);
			num *= ((num == wheeeLevel) ? 0.5f : 1f);
			float num2 = Mathf.PerlinNoise(Time.time, noiseSeed) - 0.5f;
			float num3 = Mathf.PerlinNoise(Time.time, noiseSeed + 1f) - 0.5f;
			float num4 = Mathf.PerlinNoise(Time.time, noiseSeed + 2f) - 0.5f;
			if ((bool)headRef)
			{
				float x = num2 * headBobAmount * num;
				float num5 = num3 * headBobAmount * 2f * num;
				float num6 = num4 * headBobAmount * 0.1f * num;
				headRef.transform.localRotation = Quaternion.Euler(x, num6 * 0.1f, num5 * 2f);
			}
			if ((bool)neckJoint)
			{
				float x2 = num2 * headBobAmount * 0.5f * num;
				float z = num3 * headBobAmount * 0.3f * num;
				float y = num4 * headBobAmount * 0.2f * num;
				neckJoint.transform.localRotation = helmetInitRot * Quaternion.Euler(x2, y, z);
			}
		}
	}
}
