using UnityEngine;

public class FXModuleThrottleEffects : PartModuleFXSetter
{
	[KSPField]
	public float responseSpeed = 0.5f;

	[KSPField]
	public bool dependOnEngineState;

	[KSPField]
	public bool dependOnOutput;

	[KSPField]
	public bool dependOnThrottle;

	[KSPField]
	public bool preferMultiMode;

	[KSPField]
	public int engineIndex;

	[KSPField]
	public string engineName;

	[KSPField(isPersistant = true)]
	public float state;

	public Animation anim;

	public IEngineStatus engineReference;

	public override void OnStart(StartState state)
	{
		if (dependOnEngineState)
		{
			engineReference = base.part.Modules.FindEngineNearby(engineName, engineIndex, preferMultiMode);
			if (engineReference == null)
			{
				dependOnEngineState = false;
			}
		}
		base.OnStart(state);
	}

	public void FixedUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && !(base.vessel == null))
		{
			float num = 0f;
			num = ((!dependOnEngineState) ? Mathf.Clamp01(base.vessel.ctrlState.mainThrottle) : ((!engineReference.isOperational) ? 0f : (dependOnOutput ? Mathf.Clamp01(engineReference.normalizedOutput) : ((!dependOnThrottle) ? Mathf.Clamp01(base.vessel.ctrlState.mainThrottle) : Mathf.Clamp01(engineReference.throttleSetting)))));
			float num2 = Mathf.Lerp(state, num, responseSpeed * 25f * TimeWarp.fixedDeltaTime);
			if (num2 > 0.99995f && state < num)
			{
				num2 = 1f;
			}
			if (num2 < 5E-05f && num < state)
			{
				num2 = 0f;
			}
			state = num2;
		}
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsFlight && !(base.vessel == null))
		{
			SetFXModules(state);
		}
	}
}
