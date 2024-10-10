using System;
using UnityEngine;

public class ModuleEvaChute : ModuleParachute
{
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteYawRateAtMaxSpeed")]
	public float chuteYawRateAtMaxSpeed = 0.1f;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteMaxSpeedForYawRate")]
	public float chuteMaxSpeedForYawRate = 50f;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteYawRateAtMinSpeed")]
	public float chuteYawRateAtMinSpeed = 0.5f;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteMinSpeedForYawRate")]
	public float chuteMinSpeedForYawRate = 10f;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteRollRate")]
	public float chuteRollRate = 1f;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chutePitchRate")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	public float chutePitchRate = 1f;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteDefaultForwardPitch")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 50f, minValue = 5f)]
	public float chuteDefaultForwardPitch = 15f;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 50f, minValue = 5f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "semiDeployedChuteForwardPitch")]
	public float semiDeployedChuteForwardPitch = 25f;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	[KSPField(isPersistant = true, guiActive = false, guiName = "chutePitchRateDivisorWhenTurning")]
	public float chutePitchRateDivisorWhenTurning = 3f;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteRollRateDivisorWhenPitching")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	public float chuteRollRateDivisorWhenPitching = 2f;

	[KSPField(isPersistant = true, guiActive = false, guiName = "chuteYawRateDivisorWhenPitching")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 10f, minValue = 0.1f)]
	public float chuteYawRateDivisorWhenPitching = 1.5f;

	public bool showEVAChuteParams;

	public KerbalEVA kerbalEVA;

	public ModuleInventoryPart inventory;

	[KSPField]
	public string baseName = "base";

	public string flagDecalsTagName = "FlagDecal";

	public override void OnStart(StartState state)
	{
		if (base.part.protoModuleCrew[0].ChuteNode != null)
		{
			Load(base.part.protoModuleCrew[0].ChuteNode);
		}
		base.Events["Repack"].guiActive = true;
		base.OnStart(state);
		kerbalEVA = GetComponent<KerbalEVA>();
		inventory = base.part.FindModuleImplementing<ModuleInventoryPart>();
		Shader shader = Shader.Find("KSP/Bumped Specular (Stencil)");
		Transform[] array = base.part.FindModelTransforms(canopyName);
		for (int i = 0; i < array.Length; i++)
		{
			MeshRenderer component = array[i].GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.material.shader = shader;
			}
		}
		Shader shader2 = Shader.Find("KSP/Bumped Specular");
		array = base.part.FindModelTransforms(baseName);
		for (int j = 0; j < array.Length; j++)
		{
			MeshRenderer[] componentsInChildren = array[j].GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer component in componentsInChildren)
			{
				if (component != null)
				{
					component.material.shader = shader2;
				}
			}
		}
		Shader shader3 = Shader.Find("KSP/Scenery/Decal/Multiply");
		array = base.part.FindModelTransformsWithTag(flagDecalsTagName);
		for (int l = 0; l < array.Length; l++)
		{
			MeshRenderer component = array[l].GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.material.shader = shader3;
			}
		}
		dontRotateParachute = true;
		deactivateOnRepack = false;
		base.Fields["spreadAngle"].guiActive = false;
		base.part.PartValues.Update();
		if (!CanCrewMemberUseParachute(base.part.protoModuleCrew[0]))
		{
			SetEVAChuteActive(active: false);
		}
		if (deploymentState == deploymentStates.DEPLOYED)
		{
			canopy.rotation = GetFullyDeployedCanopyRotation();
			lastRot = canopy.rotation;
		}
		else if (deploymentState == deploymentStates.SEMIDEPLOYED)
		{
			canopy.rotation = GetSemiDeployedCanopyRotation();
			lastRot = canopy.rotation;
		}
	}

	public void SetCanopy(Transform chuteTransform)
	{
		canopy = Part.FindHeirarchyTransform(chuteTransform.transform, canopyName);
		Anim = chuteTransform.transform.GetComponentInChildren<Animation>();
		canopy.gameObject.SetActive(value: false);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override bool IsStageable()
	{
		return false;
	}

	public override void OnActive()
	{
	}

	public override void FixedUpdate()
	{
		if (deploymentState == deploymentStates.DEPLOYED)
		{
			canopy.rotation = GetFullyDeployedCanopyRotation();
		}
		else if (deploymentState == deploymentStates.SEMIDEPLOYED)
		{
			canopy.rotation = GetSemiDeployedCanopyRotation();
		}
		base.FixedUpdate();
	}

	public virtual void UpdateFullyDeployedParachuteMovement(Vector3 parachuteInput, Rigidbody kerbalRB)
	{
		Vector3 axis = Vector3.zero;
		float angle = 0f;
		Vector3 vector = -base.vessel.graviticAcceleration.normalized;
		vector = Quaternion.AngleAxis(chuteDefaultForwardPitch, base.transform.right) * vector;
		Quaternion.FromToRotation(base.transform.up, vector).ToAngleAxis(out angle, out axis);
		float num = (Mathf.Clamp((float)base.vessel.speed, chuteMinSpeedForYawRate, chuteMaxSpeedForYawRate) - chuteMinSpeedForYawRate) / (chuteMaxSpeedForYawRate - chuteMinSpeedForYawRate) * (chuteYawRateAtMaxSpeed - chuteYawRateAtMinSpeed) + chuteYawRateAtMinSpeed;
		bool num2 = parachuteInput.y != 0f;
		bool flag;
		float num3 = ((flag = parachuteInput.x != 0f) ? (chuteRollRate / chuteRollRateDivisorWhenPitching) : chuteRollRate);
		float num4 = (num2 ? (chutePitchRate / chutePitchRateDivisorWhenTurning) : chutePitchRate);
		float num5 = (flag ? (num / chuteYawRateDivisorWhenPitching) : num);
		Vector3 zero = Vector3.zero;
		zero += base.transform.right * parachuteInput.x * num4;
		zero += base.transform.up * parachuteInput.y * num5;
		zero += base.transform.forward * -1f * parachuteInput.y * num3;
		if (!axis.IsInvalid())
		{
			kerbalRB.angularVelocity = axis * angle * ((float)Math.PI / 180f) + zero;
		}
	}

	public virtual void UpdateSemiDeployedParachuteMovement(Vector3 parachuteInput, Rigidbody kerbalRB)
	{
		Vector3 toDirection = Quaternion.AngleAxis(semiDeployedChuteForwardPitch, base.transform.right) * -base.part.dragVectorDir;
		Quaternion.FromToRotation(base.transform.up, toDirection).ToAngleAxis(out var angle, out var axis);
		if (!axis.IsInvalid())
		{
			this.GetComponentCached(ref kerbalRB).angularVelocity = axis * angle * ((float)Math.PI / 180f);
		}
	}

	public bool CanCrewMemberUseParachute(ProtoCrewMember crewMember)
	{
		if (inventory != null && inventory.InventoryItemCount == 0)
		{
			return false;
		}
		return crewMember.experienceLevel >= base.vessel.VesselValues.EVAChuteSkill.value;
	}

	public bool CanCrewMemberUseParachute()
	{
		if (!(base.part == null) && base.part.protoModuleCrew.Count >= 1)
		{
			return CanCrewMemberUseParachute(base.part.protoModuleCrew[0]);
		}
		return false;
	}

	public Quaternion GetSemiDeployedCanopyRotation()
	{
		return Quaternion.AngleAxis(0f - semiDeployedChuteForwardPitch, kerbalEVA.transform.right) * Quaternion.LookRotation(kerbalEVA.transform.up, -kerbalEVA.transform.forward);
	}

	public Quaternion GetFullyDeployedCanopyRotation()
	{
		return Quaternion.LookRotation(kerbalEVA.transform.up, -kerbalEVA.transform.forward);
	}

	[KSPEvent(active = false, guiActive = false, guiName = "ToggleEVAChuteParams")]
	public void ShowEVAChuteParamsChanged()
	{
		showEVAChuteParams = !showEVAChuteParams;
		ToggleEVAChuteParams(showEVAChuteParams);
	}

	public void ToggleEVAChuteParams(bool show)
	{
		base.Fields["chuteYawRateAtMaxSpeed"].guiActive = show;
		base.Fields["chuteMaxSpeedForYawRate"].guiActive = show;
		base.Fields["chuteYawRateAtMinSpeed"].guiActive = show;
		base.Fields["chuteMinSpeedForYawRate"].guiActive = show;
		base.Fields["chuteRollRate"].guiActive = show;
		base.Fields["chutePitchRate"].guiActive = show;
		base.Fields["chuteDefaultForwardPitch"].guiActive = show;
		base.Fields["semiDeployedChuteForwardPitch"].guiActive = show;
		base.Fields["chutePitchRateDivisorWhenTurning"].guiActive = show;
		base.Fields["chuteRollRateDivisorWhenPitching"].guiActive = show;
		base.Fields["chuteYawRateDivisorWhenPitching"].guiActive = show;
		base.Fields["rotationSpeedDPS"].guiActive = show;
	}

	public void SetEVAChuteActive(bool active)
	{
		moduleIsEnabled = active;
		base.enabled = active;
		base.Fields["minAirPressureToOpen"].guiActive = active;
		base.Fields["deployAltitude"].guiActive = active;
		base.Fields["spreadAngle"].guiActive = active;
		base.Fields["automateSafeDeploy"].guiActive = active;
		base.Events["ShowEVAChuteParamsChanged"].active = active;
		if (active)
		{
			SetUIEVents();
			return;
		}
		base.Events["Deploy"].active = false;
		base.Events["CutParachute"].active = false;
		base.Events["Repack"].active = false;
		base.Events["Disarm"].active = false;
	}

	public override void OnParachuteSemiDeployed()
	{
		if (!kerbalEVA.IsChuteState)
		{
			kerbalEVA.OnParachuteSemiDeployed();
		}
		if (ShouldDeploy())
		{
			canopy.rotation = Quaternion.LookRotation(kerbalEVA.transform.up, -kerbalEVA.transform.forward);
		}
		else
		{
			canopy.rotation = Quaternion.AngleAxis(0f - semiDeployedChuteForwardPitch, kerbalEVA.transform.right) * Quaternion.LookRotation(kerbalEVA.transform.up, -kerbalEVA.transform.forward);
		}
		lastRot = canopy.rotation;
		base.OnParachuteSemiDeployed();
	}

	public override void OnParachuteFullyDeployed()
	{
		kerbalEVA.OnParachuteFullyDeployed();
		base.OnParachuteFullyDeployed();
	}

	public override bool PassedAdditionalDeploymentChecks()
	{
		if (kerbalEVA.IsKerbalInStateAbleToDeployParachute() && !kerbalEVA.JetpackDeployed && (!base.vessel.Splashed || (kerbalEVA.IsSeated() && (!kerbalEVA.IsSeated() || !(base.part.submergedPortion > 0.0)))) && (!base.vessel.Landed || kerbalEVA.IsSeated()))
		{
			return base.PassedAdditionalDeploymentChecks();
		}
		return false;
	}

	public override void CutParachute()
	{
		if (deploymentState != deploymentStates.const_4 && deploymentState != deploymentStates.ACTIVE)
		{
			base.CutParachute();
			if (kerbalEVA.IsChuteState || kerbalEVA.IsSeated())
			{
				kerbalEVA.OnParachuteCut();
			}
		}
	}

	public void AllowRepack(bool allowRepack)
	{
		if (allowRepack && deploymentState == deploymentStates.const_4)
		{
			base.Events["Repack"].active = true;
			base.Events["Repack"].guiActive = true;
		}
		else
		{
			base.Events["Repack"].active = false;
		}
	}
}
