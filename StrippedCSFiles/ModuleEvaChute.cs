using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleEvaChute : ModuleParachute
{
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteYawRateAtMaxSpeed")]
	public float chuteYawRateAtMaxSpeed;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteMaxSpeedForYawRate")]
	public float chuteMaxSpeedForYawRate;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteYawRateAtMinSpeed")]
	public float chuteYawRateAtMinSpeed;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteMinSpeedForYawRate")]
	public float chuteMinSpeedForYawRate;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteRollRate")]
	public float chuteRollRate;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chutePitchRate")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	public float chutePitchRate;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteDefaultForwardPitch")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 50f, minValue = 5f)]
	public float chuteDefaultForwardPitch;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 50f, minValue = 5f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "semiDeployedChuteForwardPitch")]
	public float semiDeployedChuteForwardPitch;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chutePitchRateDivisorWhenTurning")]
	public float chutePitchRateDivisorWhenTurning;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteRollRateDivisorWhenPitching")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	public float chuteRollRateDivisorWhenPitching;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteYawRateDivisorWhenPitching")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	public float chuteYawRateDivisorWhenPitching;

	private bool showEVAChuteParams;

	private KerbalEVA kerbalEVA;

	private ModuleInventoryPart inventory;

	[KSPField]
	public string baseName;

	private string flagDecalsTagName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleEvaChute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCanopy(Transform chuteTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateFullyDeployedParachuteMovement(Vector3 parachuteInput, Rigidbody kerbalRB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateSemiDeployedParachuteMovement(Vector3 parachuteInput, Rigidbody kerbalRB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanCrewMemberUseParachute(ProtoCrewMember crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanCrewMemberUseParachute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Quaternion GetSemiDeployedCanopyRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Quaternion GetFullyDeployedCanopyRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = false, guiActive = false, guiName = "ToggleEVAChuteParams")]
	public void ShowEVAChuteParamsChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ToggleEVAChuteParams(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEVAChuteActive(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnParachuteSemiDeployed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnParachuteFullyDeployed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool PassedAdditionalDeploymentChecks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void CutParachute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AllowRepack(bool allowRepack)
	{
		throw null;
	}
}
