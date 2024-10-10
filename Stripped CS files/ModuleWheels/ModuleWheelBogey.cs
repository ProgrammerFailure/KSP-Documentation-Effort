using System;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelBogey : ModuleWheelSubmodule
{
	[KSPField]
	public string wheelTransformRefName = "";

	[KSPField]
	public string wheelTransformBaseName = "";

	[KSPField]
	public string bogeyTransformName = "";

	[KSPField]
	public string bogeyRefTransformName = "";

	[KSPField]
	public float maxPitch = 25f;

	[KSPField]
	public float minPitch = -25f;

	[KSPField]
	public float restPitch = -25f;

	[KSPField]
	public float pitchResponse = 8f;

	[KSPField]
	public Vector3 bogeyAxis = Vector3.right;

	[KSPField]
	public Vector3 bogeyUpAxis = Vector3.up;

	[KSPField]
	public int deployModuleIndex = -1;

	public Transform wheelTransformRef;

	public Transform[] wheelTrfs;

	public Transform bogey;

	public Transform bogeyRef;

	public Vector3 bogeyUp;

	public Vector3 bogeyUp0;

	public Quaternion bogeyRot;

	public Quaternion bogeyRot0;

	public Quaternion bogeyRotLast;

	public float bogeyAngle;

	[NonSerialized]
	public ModuleWheelDeployment deployModule;

	public override void OnStart(StartState state)
	{
		if (!string.IsNullOrEmpty(bogeyTransformName))
		{
			Transform transform = base.part.FindModelTransform(bogeyTransformName);
			if (transform != null)
			{
				bogey = transform;
			}
			else
			{
				Debug.LogError("[ModuleWheelBogey]: No bogey transform found called " + bogeyTransformName + " in " + base.part.partName + "!", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(bogeyRefTransformName))
		{
			Transform transform2 = base.part.FindModelTransform(bogeyRefTransformName);
			if (transform2 != null)
			{
				bogeyRef = transform2;
			}
			else
			{
				Debug.LogError("[ModuleWheelBogey]: No bogey refe transform found called " + bogeyRefTransformName + " in " + base.part.partName + "!", base.gameObject);
			}
		}
		else
		{
			bogeyRef = bogey;
		}
		if (!string.IsNullOrEmpty(wheelTransformRefName))
		{
			Transform transform3 = base.part.FindModelTransform(wheelTransformRefName);
			if (transform3 != null)
			{
				wheelTransformRef = transform3;
			}
			else
			{
				Debug.LogError("[ModuleWheelBogey]: No wheel ref transform found called " + wheelTransformRefName + " in " + base.part.partName + "!", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(wheelTransformBaseName))
		{
			if (wheelTransformRef == null)
			{
				Debug.LogError("[ModuleWheelBogey]: No reference transform defined for " + base.part.partName + "! Wheels won't function properly!", base.gameObject);
			}
			Transform[] array = base.part.FindModelTransforms(wheelTransformBaseName);
			if (array.Length != 0)
			{
				wheelTrfs = array;
			}
			else
			{
				Debug.LogError("[ModuleWheelBogey]: No wheel ref transforms found containing id " + wheelTransformRefName + " in " + base.part.partName + "!", base.gameObject);
			}
		}
		if (deployModuleIndex != -1)
		{
			ModuleWheelDeployment moduleWheelDeployment = base.part.Modules[deployModuleIndex] as ModuleWheelDeployment;
			if (moduleWheelDeployment != null)
			{
				deployModule = moduleWheelDeployment;
				return;
			}
			Debug.LogError("[ModuleWheelBogey]: No deployment module found at index " + deployModuleIndex + " in " + base.part.partName + "!", base.gameObject);
		}
	}

	public override void OnWheelSetup()
	{
		if (bogeyRef != null)
		{
			bogeyRot0 = bogeyRef.localRotation;
			bogeyUp0 = bogeyRef.localRotation * bogeyUpAxis;
		}
		else
		{
			bogeyRot0 = Quaternion.identity;
			bogeyUp0 = bogeyUpAxis;
		}
		bogeyRotLast = bogeyRot0;
	}

	public void LateUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (bogey != null && !base.part.packed && baseSetup && (deployModule == null || deployModule.Position > 0.8f) && !wheelBase.InopSystems.HasType(WheelSubsystem.SystemTypes.Bogey))
		{
			if (wheel.IsGrounded)
			{
				bogeyUp = Vector3.ProjectOnPlane(wheel.currentState.hit.normal, bogey.parent.rotation * bogeyAxis);
				bogeyAngle = Mathf.Clamp(KSPUtil.BearingDegrees(bogeyUp0, Quaternion.Inverse(bogey.parent.rotation) * bogeyUp, bogeyAxis), minPitch, maxPitch);
			}
			else
			{
				bogeyAngle = restPitch;
			}
			bogeyRot = Quaternion.AngleAxis(bogeyAngle, bogeyAxis) * bogeyRot0;
			Quaternion quaternion = Quaternion.RotateTowards(bogeyRotLast, bogeyRot, pitchResponse * TimeWarp.deltaTime);
			if (!float.IsNaN(quaternion.x) && !float.IsNaN(quaternion.y) && !float.IsNaN(quaternion.z) && !float.IsNaN(quaternion.w))
			{
				if (deployModule != null)
				{
					bogey.localRotation = Quaternion.Lerp(bogey.localRotation, quaternion, Mathf.InverseLerp(0.8f, 1f, deployModule.Position));
				}
				else
				{
					bogey.localRotation = quaternion;
				}
			}
			bogeyRotLast = bogey.localRotation;
		}
		if (wheelTransformRef != null && !wheelBase.InopSystems.HasType(WheelSubsystem.SystemTypes.Tire))
		{
			int num = wheelTrfs.Length;
			while (num-- > 0)
			{
				wheelTrfs[num].localRotation = wheelTransformRef.localRotation;
			}
		}
	}

	public override string OnGatherInfo()
	{
		return null;
	}
}
