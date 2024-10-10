using ns9;
using UnityEngine;

public class ModuleAnimationSetter : PartModule, IScalarModule
{
	[KSPField]
	public string animName = "HeatAnimationEmissive";

	[KSPField]
	public string moduleID = "animSetter";

	[KSPField]
	public int layer = 1;

	[KSPField]
	public FloatCurve animStateCurve;

	public AnimationState[] animStates;

	public float animState;

	public float inputState;

	public int stateCount;

	public EventData<float, float> OnMove = new EventData<float, float>("OnMove");

	public EventData<float> OnStopped = new EventData<float>("OnStop");

	public static string cacheAutoLOC_6003028;

	public string ScalarModuleID => moduleID;

	public float GetScalar => inputState;

	public bool CanMove => true;

	public EventData<float, float> OnMoving => OnMove;

	public EventData<float> OnStop => OnStopped;

	public override void OnAwake()
	{
		if (animStateCurve == null)
		{
			animStateCurve = new FloatCurve();
			animStateCurve.Add(0f, 0f);
			animStateCurve.Add(1f, 1f);
		}
	}

	public override void OnStart(StartState state)
	{
		AnimStartup();
	}

	public void Update()
	{
		UpdateAnim();
	}

	public virtual void AnimStartup()
	{
		Animation[] array = base.part.FindModelAnimators(animName);
		stateCount = array.Length;
		animStates = new AnimationState[stateCount];
		for (int i = 0; i < stateCount; i++)
		{
			Animation obj = array[i];
			AnimationState animationState = obj[animName];
			animationState.speed = 0f;
			animationState.enabled = true;
			obj[animName].layer = layer;
			obj.Play(animName);
			animStates[i] = animationState;
		}
	}

	public virtual void SetScalar(float inputVal)
	{
		if (!float.IsNaN(inputVal))
		{
			inputState = inputVal;
			animState = animStateCurve.Evaluate(inputVal);
		}
	}

	public virtual void UpdateAnim()
	{
		if (animStates != null)
		{
			for (int i = 0; i < stateCount; i++)
			{
				animStates[i].normalizedTime = animState;
			}
		}
	}

	public void SetUIRead(bool state)
	{
	}

	public void SetUIWrite(bool state)
	{
	}

	public bool IsMoving()
	{
		return false;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003028;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6003028 = Localizer.Format("#autoLoc_6003028");
	}
}
