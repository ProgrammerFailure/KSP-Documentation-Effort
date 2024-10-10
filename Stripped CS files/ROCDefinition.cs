using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ROCDefinition
{
	[SerializeField]
	public string type;

	[SerializeField]
	public string displayName;

	[SerializeField]
	public string prefabName;

	[SerializeField]
	public string modelName;

	[SerializeField]
	public bool orientateUp;

	[SerializeField]
	public float depth;

	[SerializeField]
	public bool canBeTaken;

	[SerializeField]
	public float frequency;

	[SerializeField]
	public bool castShadows = true;

	[SerializeField]
	public bool receiveShadows = true;

	[SerializeField]
	public float collisionThreshold;

	[SerializeField]
	public bool smallRoc;

	[SerializeField]
	public bool randomDepth;

	[SerializeField]
	public bool randomOrientation;

	[SerializeField]
	public bool randomRotation;

	[SerializeField]
	public float burstEmitterMinWait;

	[SerializeField]
	public float burstEmitterMaxWait;

	[SerializeField]
	public float sfxVolume;

	[SerializeField]
	public string idleClipPath;

	[SerializeField]
	public string burstClipPath;

	public List<RocCBDefinition> myCelestialBodies;

	[SerializeField]
	public List<Vector3> localSpaceScanPoints;

	[SerializeField]
	public float scale = 1f;

	public float vfxBaseForce;

	public FloatCurve vfxCurveForce;

	public bool applyForces;

	public Vector2 vfxForceRadius;

	public Vector3 forceDirection;

	public Vector3 radiusCenter;

	public ROCDefinition()
	{
		localSpaceScanPoints = new List<Vector3>();
		myCelestialBodies = new List<RocCBDefinition>();
	}

	public ROCDefinition(string type, string displayName, string prefabName, string modelName, bool orientateUp, float depth, bool canBeTaken, float frequency, bool castShadows, bool receiveShadows, float collisionThreshold, bool smallRoc, bool randomDepth, bool randomOrientation, List<Vector3> localSpaceScanPoints, float burstEmitterMinWait, float burstEmitterMaxWait, bool randomRotation, float scale, float sfxVolume, string idleClipPath, string burstClipPath, FloatCurve vfxCurveForce, float vfxBaseForce, Vector2 vfxForceRadius, Vector3 forceDirection, Vector3 radiusCenter)
		: this()
	{
		this.type = type;
		this.displayName = displayName;
		this.prefabName = prefabName;
		this.modelName = modelName;
		this.orientateUp = orientateUp;
		this.depth = depth;
		this.canBeTaken = canBeTaken;
		this.frequency = frequency;
		this.castShadows = castShadows;
		this.receiveShadows = receiveShadows;
		this.collisionThreshold = collisionThreshold;
		this.smallRoc = smallRoc;
		this.randomDepth = randomDepth;
		this.randomOrientation = randomOrientation;
		this.localSpaceScanPoints = localSpaceScanPoints;
		this.burstEmitterMinWait = burstEmitterMinWait;
		this.burstEmitterMaxWait = burstEmitterMaxWait;
		this.randomRotation = randomRotation;
		this.sfxVolume = sfxVolume;
		this.idleClipPath = idleClipPath;
		this.burstClipPath = burstClipPath;
		this.scale = scale;
		this.vfxBaseForce = vfxBaseForce;
		this.vfxCurveForce = vfxCurveForce;
		this.vfxForceRadius = vfxForceRadius;
		this.forceDirection = forceDirection;
		this.radiusCenter = radiusCenter;
	}

	public bool ContainsCBBiome(string cbName, string biomeName)
	{
		for (int i = 0; i < myCelestialBodies.Count; i++)
		{
			if (!(myCelestialBodies[i].name == cbName))
			{
				continue;
			}
			for (int j = 0; j < myCelestialBodies[i].biomes.Count; j++)
			{
				if (myCelestialBodies[i].biomes[j] == biomeName)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("Type", ref type);
		node.TryGetValue("displayName", ref displayName);
		node.TryGetValue("prefabName", ref prefabName);
		node.TryGetValue("modelName", ref modelName);
		node.TryGetValue("OrientateUp", ref orientateUp);
		node.TryGetValue("Depth", ref depth);
		node.TryGetValue("CanBeTaken", ref canBeTaken);
		node.TryGetValue("Frequency", ref frequency);
		node.TryGetValue("CastShadows", ref castShadows);
		node.TryGetValue("ReceiveShadows", ref receiveShadows);
		node.TryGetValue("CollisionThreshold", ref collisionThreshold);
		node.TryGetValue("SmallROC", ref smallRoc);
		node.TryGetValue("RandomDepth", ref randomDepth);
		node.TryGetValue("RandomOrientation", ref randomOrientation);
		node.TryGetValue("RandomRotation", ref randomRotation);
		node.TryGetValue("burstEmitterMinWait", ref burstEmitterMinWait);
		node.TryGetValue("burstEmitterMaxWait", ref burstEmitterMaxWait);
		node.TryGetValue("sfxVolume", ref sfxVolume);
		node.TryGetValue("idleClip", ref idleClipPath);
		node.TryGetValue("burstClip", ref burstClipPath);
		node.TryGetValue("vfxBaseForce", ref vfxBaseForce);
		ConfigNode node2 = null;
		if (node.TryGetNode("VFX_CURVEFORCE", ref node2) && node2.values.Count > 0)
		{
			vfxCurveForce = new FloatCurve();
			vfxCurveForce.Load(node2);
		}
		string[] values = node.GetValues("localSpaceScanPoints");
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i] != null)
			{
				localSpaceScanPoints.Add(ConfigNode.ParseVector3(values[i]));
			}
		}
		node.TryGetValue("applyForces", ref applyForces);
		node.TryGetValue("vfxForceRadius", ref vfxForceRadius);
		node.TryGetValue("forceDirection", ref forceDirection);
		node.TryGetValue("vfxRadiusCenter", ref radiusCenter);
		node.TryGetValue("Scale", ref scale);
	}
}
