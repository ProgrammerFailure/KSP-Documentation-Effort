using UnityEngine;

public class InternalAnimationAltimeter : InternalModule
{
	[KSPField]
	public string animationName = "anim";

	[KSPField]
	public float altitudeStart;

	[KSPField]
	public float altitudeEnd = 100000f;

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
			normTime = Mathf.Lerp(altitudeStart, altitudeEnd, (float)base.vessel.altitude);
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
