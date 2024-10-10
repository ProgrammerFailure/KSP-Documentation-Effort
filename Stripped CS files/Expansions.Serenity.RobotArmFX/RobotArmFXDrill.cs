using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Serenity.RobotArmFX;

public class RobotArmFXDrill : RobotArmScannerFX
{
	public RaycastHit hitInfo;

	public string drillBaseTransformName = "DrillBaseTransform";

	public string drillTipTransformName = "DrillHeadTransform";

	public string drillEffectTransformName = "DrillParticles";

	public Transform drillBaseTransform;

	public Transform drillTipTransform;

	public Transform drillEffectTransform;

	public List<KSPParticleEmitter> drillImpactParticleEmitters = new List<KSPParticleEmitter>();

	public RobotArmFXDrill()
	{
		base.IsReady = false;
	}

	public override void OnStart(Part part)
	{
		base.OnStart(part);
		drillBaseTransform = part.FindModelTransform(drillBaseTransformName);
		if (drillBaseTransform == null)
		{
			Debug.LogError("[RobotArmFXDrill]: Unable to setup FX - cannot find drill base transform");
			return;
		}
		drillTipTransform = part.FindModelTransform(drillTipTransformName);
		if (drillTipTransform == null)
		{
			Debug.LogError("[RobotArmFXDrill]: Unable to setup FX - cannot find drill tip transform");
			return;
		}
		drillEffectTransform = part.FindModelTransform(drillEffectTransformName);
		if (drillEffectTransform == null)
		{
			Debug.LogError("[RobotArmFXDrill]: Unable to setup FX - cannot find drill effect transform");
			return;
		}
		KSPParticleEmitter component = drillEffectTransform.GetComponent<KSPParticleEmitter>();
		if (component != null)
		{
			drillImpactParticleEmitters.Add(component);
		}
		KSPParticleEmitter[] componentsInChildren = drillEffectTransform.GetComponentsInChildren<KSPParticleEmitter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				drillImpactParticleEmitters.Add(componentsInChildren[i]);
			}
		}
		if (drillImpactParticleEmitters.Count == 0)
		{
			Debug.Log("[RobotArmFXDrill]: Unable to setup FX - cannot find KSP Particle Emitter on drill effect transform.");
			return;
		}
		for (int j = 0; j < drillImpactParticleEmitters.Count; j++)
		{
			drillImpactParticleEmitters[j].emit = false;
		}
		base.IsReady = true;
	}

	public override void OnEffectStart()
	{
	}

	public override void OnUpdate(float animationTime, float distanceFromSurface, Vector3 instrumentTargetPosition)
	{
		if (animationTime > effectStartTime && animationTime < effectStopTime)
		{
			Vector3 vector = drillTipTransform.position - drillBaseTransform.position;
			if (ModuleRobotArmScanner.RayCastToROC(drillBaseTransform.position, vector.normalized, vector.magnitude, ref hitInfo, performSphereCast: false))
			{
				for (int i = 0; i < drillImpactParticleEmitters.Count; i++)
				{
					drillImpactParticleEmitters[i].emit = true;
				}
				drillEffectTransform.position = hitInfo.point;
			}
			else
			{
				for (int j = 0; j < drillImpactParticleEmitters.Count; j++)
				{
					drillImpactParticleEmitters[j].emit = false;
				}
			}
		}
		else
		{
			for (int k = 0; k < drillImpactParticleEmitters.Count; k++)
			{
				drillImpactParticleEmitters[k].emit = false;
			}
		}
	}

	public override void OnEffectStop()
	{
		for (int i = 0; i < drillImpactParticleEmitters.Count; i++)
		{
			drillImpactParticleEmitters[i].emit = false;
		}
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("drillBaseTransformName", ref drillBaseTransformName);
		node.TryGetValue("drillTipTransformName", ref drillTipTransformName);
		node.TryGetValue("drillEffectTransformName", ref drillEffectTransformName);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("drillBaseTransformName", drillBaseTransformName);
		node.AddValue("drillTipTransformName", drillTipTransformName);
		node.AddValue("drillEffectTransformName", drillEffectTransformName);
	}
}
