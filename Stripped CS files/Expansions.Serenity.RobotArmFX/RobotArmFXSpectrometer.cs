using UnityEngine;

namespace Expansions.Serenity.RobotArmFX;

public class RobotArmFXSpectrometer : RobotArmScannerFX
{
	public Light spectrometerLight;

	public string lightTransformName = "LightTransform";

	public Transform spectrometerTransform;

	public RobotArmFXSpectrometer()
	{
		base.IsReady = false;
	}

	public override void OnStart(Part part)
	{
		base.OnStart(part);
		spectrometerTransform = part.FindModelTransform(lightTransformName);
		if (spectrometerTransform == null)
		{
			Debug.LogError("[RobotArmFXSpectrometer]: Unable to setup FX - cannot find light transform");
			return;
		}
		spectrometerLight = spectrometerTransform.GetComponent<Light>();
		if (spectrometerLight == null)
		{
			Debug.LogError("[RobotArmFXSpectrometer]: Unable to setup FX - cannot find light component");
			return;
		}
		spectrometerLight.enabled = false;
		base.IsReady = true;
	}

	public override void OnEffectStart()
	{
	}

	public override void OnUpdate(float animationTime, float distanceFromSurface, Vector3 instrumentTargetPosition)
	{
		if (animationTime > effectStartTime && animationTime < effectStopTime)
		{
			spectrometerLight.enabled = true;
		}
		else
		{
			spectrometerLight.enabled = false;
		}
	}

	public override void OnEffectStop()
	{
		spectrometerLight.enabled = false;
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("lightTransformName", ref lightTransformName);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("lightTransformName", lightTransformName);
	}
}
