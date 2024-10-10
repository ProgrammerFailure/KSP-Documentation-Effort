using UnityEngine;

namespace Expansions.Serenity.RobotArmFX;

public class RobotArmFXLaser : RobotArmScannerFX
{
	public string laserEffectTransformName = "LaserTransform";

	public string configLaserEffectColor = "#FF0000";

	public Color laserEffectColor = Color.red;

	public float laserEffectWidth = 0.01f;

	public Transform laserEffectTransform;

	public LineRenderer laserLineRenderer;

	public bool hasPerformedSetupRaycast;

	public Vector3 laserTarget;

	public RobotArmFXLaser()
	{
		base.IsReady = false;
	}

	public override void OnStart(Part part)
	{
		base.OnStart(part);
		laserEffectTransform = part.FindModelTransform(laserEffectTransformName);
		if (laserEffectTransform == null)
		{
			Debug.LogError("[RobotArmFXLaser]: Unable to setup FX - cannot find laser effect transform");
		}
		else
		{
			base.IsReady = true;
		}
	}

	public override void OnEffectStart()
	{
		if (!string.IsNullOrEmpty(configLaserEffectColor) && configLaserEffectColor.StartsWith("#"))
		{
			laserEffectColor = XKCDColors.ColorTranslator.FromHtml(configLaserEffectColor);
		}
		if (laserLineRenderer == null)
		{
			laserLineRenderer = laserEffectTransform.gameObject.AddComponent<LineRenderer>();
			if (laserLineRenderer == null)
			{
				Debug.LogError("[RobotArmFXLaser]: Unable to start FX - cannot create line renderer");
				base.IsReady = false;
				return;
			}
			laserLineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
			laserLineRenderer.startColor = laserEffectColor;
			laserLineRenderer.endColor = laserEffectColor;
			laserLineRenderer.startWidth = laserEffectWidth;
			laserLineRenderer.endWidth = laserEffectWidth;
		}
		laserLineRenderer.enabled = false;
		hasPerformedSetupRaycast = false;
	}

	public override void OnUpdate(float animationTime, float distanceFromSurface, Vector3 instrumentTargetPosition)
	{
		if (animationTime > effectStartTime && animationTime < effectStopTime)
		{
			if (!hasPerformedSetupRaycast)
			{
				laserTarget = instrumentTargetPosition;
				RaycastHit hit = default(RaycastHit);
				if (ModuleRobotArmScanner.RayCastToROC(laserEffectTransform.position, laserEffectTransform.forward, distanceFromSurface * 2f, ref hit, performSphereCast: false))
				{
					laserTarget = hit.point;
				}
				hasPerformedSetupRaycast = true;
			}
			laserLineRenderer.SetPosition(0, laserEffectTransform.position);
			laserLineRenderer.SetPosition(1, laserTarget + (laserTarget - laserEffectTransform.position).normalized * 0.1f);
			laserLineRenderer.enabled = true;
		}
		else
		{
			laserLineRenderer.enabled = false;
		}
	}

	public override void OnEffectStop()
	{
		laserLineRenderer.enabled = false;
		hasPerformedSetupRaycast = false;
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("laserEffectTransformName", ref laserEffectTransformName);
		node.TryGetValue("configLaserEffectColor", ref configLaserEffectColor);
		node.TryGetValue("laserEffectWidth", ref laserEffectWidth);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("laserEffectTransformName", laserEffectTransformName);
		node.AddValue("configLaserEffectColor", configLaserEffectColor);
		node.AddValue("laserEffectWidth", laserEffectWidth);
	}
}
