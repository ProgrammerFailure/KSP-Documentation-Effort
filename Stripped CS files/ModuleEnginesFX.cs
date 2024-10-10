using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleEnginesFX : ModuleEngines
{
	[KSPField]
	public string flameoutEffectName = "flameout";

	[KSPField]
	public string runningEffectName = "running";

	[KSPField]
	public string powerEffectName = "power";

	[KSPField]
	public string engageEffectName = "engage";

	[KSPField]
	public string disengageEffectName = "disengage";

	[KSPField]
	public string directThrottleEffectName = "directThrottle";

	[KSPField]
	public string spoolEffectName = "spool";

	[KSPField]
	public float engineSpoolTime = 2f;

	[KSPField]
	public float engineSpoolIdle = 0.05f;

	public float engineSpool;

	public static string cacheAutoLOC_221082;

	public override void PlayEngageFX()
	{
		base.part.Effect(engageEffectName);
	}

	public override void PlayShutdownFX()
	{
		base.part.Effect(disengageEffectName);
		DeactivatePowerFX();
		DeactivateRunningFX();
	}

	public override void PlayFlameoutFX(bool flamingOut)
	{
		base.part.Effect(flameoutEffectName);
	}

	public override void FXReset()
	{
	}

	public override void FXUpdate()
	{
		if (EngineIgnited)
		{
			base.part.Effect(directThrottleEffectName, requestedThrottle);
		}
		else
		{
			base.part.Effect(directThrottleEffectName, 0f);
		}
		if (EngineIgnited && !flameout)
		{
			if (useEngineResponseTime)
			{
				engineSpool = Mathf.Lerp(engineSpool, Mathf.Max(engineSpoolIdle, currentThrottle), engineSpoolTime * TimeWarp.fixedDeltaTime);
			}
			else
			{
				engineSpool = currentThrottle;
			}
			base.part.Effect(spoolEffectName, engineSpool);
			base.part.Effect(runningEffectName, currentThrottle);
			if (finalThrust == 0f)
			{
				base.part.Effect(powerEffectName, 0f);
			}
			else
			{
				base.part.Effect(powerEffectName, finalThrust / maxThrust);
			}
		}
		else
		{
			if (useEngineResponseTime)
			{
				engineSpool = Mathf.Lerp(engineSpool, flameout ? 0f : currentThrottle, engineSpoolTime * TimeWarp.fixedDeltaTime);
			}
			else
			{
				engineSpool = 0f;
			}
			base.part.Effect(spoolEffectName, engineSpool);
			base.part.Effect(runningEffectName, 0f);
			base.part.Effect(powerEffectName, 0f);
		}
	}

	public override void DeactivatePowerFX()
	{
		base.part.Effect(powerEffectName, 0f);
	}

	public override void DeactivateRunningFX()
	{
		base.part.Effect(runningEffectName, 0f);
	}

	public override void InitializeFX()
	{
		DeactivateLoopingFX();
	}

	public override void SetListener()
	{
	}

	public override void DeactivateLoopingFX()
	{
		DeactivatePowerFX();
		DeactivateRunningFX();
		base.part.Effect(spoolEffectName, 0f);
	}

	public virtual bool CanStart()
	{
		if (ModifyFlow() < flameoutBar)
		{
			Flameout(cacheAutoLOC_221082, statusOnly: true);
			return false;
		}
		if (CheatOptions.InfinitePropellant)
		{
			return true;
		}
		UpdatePropellantStatus(doGauge: false);
		float num = 1f;
		if (base.vessel != null)
		{
			num = base.vessel.VesselValues.FuelUsage.value;
		}
		if (num == 0f)
		{
			num = 1f;
		}
		double requiredPropellant = RequiredPropellantMass(1f) * mixtureDensityRecip * (double)num;
		if (!CheckDeprived(requiredPropellant, out var propName))
		{
			return true;
		}
		Flameout(propName + " deprived", statusOnly: true);
		return false;
	}

	public override void OnAwake()
	{
		base.OnAwake();
		base.part.Effect(flameoutEffectName, 0f);
		base.part.Effect(runningEffectName, 0f);
		base.part.Effect(powerEffectName, 0f);
		base.part.Effect(engageEffectName, 0f);
		base.part.Effect(disengageEffectName, 0f);
		base.part.Effect(directThrottleEffectName, 0f);
		base.part.Effect(spoolEffectName, 0f);
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = propellants.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(propellants[i].name));
		}
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_221082 = Localizer.Format("#autoLOC_221082");
	}
}
