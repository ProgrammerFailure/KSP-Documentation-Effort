using UnityEngine;

public class InternalAnimationAirSpeed : InternalModule
{
	[KSPField]
	public string animationName = "anim";

	[KSPField]
	public float airSpeedStart;

	[KSPField]
	public float airSpeedEnd = 100000f;

	[KSPField]
	public bool atmospheric;

	public Animation[] animations;

	public float normTime;

	public override void OnAwake()
	{
		if (animations == null)
		{
			animations = internalProp.FindModelAnimators(animationName);
			if (animations.Length == 0)
			{
				animations = null;
			}
		}
	}

	public override void OnUpdate()
	{
		if (animations != null)
		{
			if (atmospheric && base.vessel.staticPressurekPa <= 0.0)
			{
				normTime = 0f;
			}
			else
			{
				normTime = Mathf.Lerp(airSpeedStart, airSpeedEnd, (float)base.vessel.srf_velocity.magnitude);
			}
			int i = 0;
			for (int num = animations.Length; i < num; i++)
			{
				Animation obj = animations[i];
				obj[animationName].speed = 0f;
				obj[animationName].normalizedTime = normTime;
			}
		}
	}
}
