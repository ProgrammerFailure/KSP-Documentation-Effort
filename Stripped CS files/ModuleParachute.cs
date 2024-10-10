using System;
using ns9;
using UnityEngine;

public class ModuleParachute : PartModule, IMultipleDragCube
{
	public enum deploymentStates
	{
		STOWED,
		ACTIVE,
		SEMIDEPLOYED,
		DEPLOYED,
		const_4
	}

	public enum deploymentSafeStates
	{
		SAFE,
		RISKY,
		UNSAFE,
		NONE
	}

	public deploymentStates deploymentState;

	public deploymentSafeStates deploymentSafeState;

	[KSPField]
	public bool invertCanopy = true;

	[KSPField]
	public string semiDeployedAnimation = "semiDeploy";

	[KSPField]
	public string fullyDeployedAnimation = "fullyDeploy";

	[KSPField]
	public float autoCutSpeed = 0.5f;

	public float rotationSpeedDPS = 90f;

	[KSPField]
	public string capName = "cap";

	[KSPField]
	public string canopyName = "canopy";

	[KSPField(isPersistant = true)]
	public string persistentState = "STOWED";

	[KSPField]
	public float stowedDrag = 0.22f;

	[KSPField]
	public float semiDeployedDrag = 1f;

	[KSPField]
	public float fullyDeployedDrag = 450f;

	[KSPField(isPersistant = true)]
	public float animTime;

	[KSPField]
	public float clampMinAirPressure = 0.01f;

	[UI_FloatRange(stepIncrement = 0.01f, maxValue = 0.75f, minValue = 0.01f)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001340")]
	public float minAirPressureToOpen = 0.01f;

	public static double pressureRecip = 0.009869232667160128;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_438841")]
	[UI_FloatRange(stepIncrement = 50f, maxValue = 5000f, minValue = 50f)]
	public float deployAltitude = 500f;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 10f, minValue = 0f)]
	[KSPField(guiActiveEditor = true, isPersistant = true, guiActive = true, advancedTweakable = true, guiName = "#autoLOC_6001341")]
	public float spreadAngle = 7f;

	[KSPField]
	public float deploymentSpeed = 1f;

	[KSPField]
	public float deploymentCurve = 2.5f;

	[KSPField]
	public float semiDeploymentSpeed = 1f;

	[KSPField]
	public double chuteMaxTemp = 600.0;

	[KSPField]
	public double chuteThermalMassPerArea = 0.06;

	[KSPField]
	public double startingTemp = 200.0;

	[KSPField]
	public double chuteEmissivity = 0.2;

	[KSPField]
	public double chuteTemp = 200.0;

	[KSPField(guiName = "#autoLOC_6001342")]
	public string deploySafe = "";

	[KSPField]
	public double machHeatMultBase = 1.0;

	[KSPField]
	public double machHeatMultScalar = 1.75;

	[KSPField]
	public double machHeatMultPow = 1.5;

	[KSPField]
	public double machHeatDensityFadeoutMult = 1.0;

	[KSPField]
	public double secondsForRisky = 0.35;

	[KSPField]
	public double safeMult = 0.95;

	[UI_Cycle(stateNames = new string[] { "#autoLOC_6001344", "#autoLOC_6001345", "#autoLOC_6001346" }, controlEnabled = true, scene = UI_Scene.All, affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001343")]
	public int automateSafeDeploy;

	[KSPField]
	public bool shieldedCanDeploy;

	[KSPField]
	public double refDensity = 1.05;

	[KSPField]
	public double refSpeedOfSound = 320.0;

	public Transform canopy;

	public Transform cap;

	public FXGroup deployFx;

	public float lerpTime;

	public float baseDrag;

	public int symmetryCount;

	public Animation Anim;

	public Quaternion lastRot;

	public double areaSemi;

	public double areaDeployed;

	public double maxSafeSpeedAtRef;

	public double invThermalMass;

	public double machHeatMult;

	public double convectivekW;

	public double shockTemp;

	public double convectionArea;

	public double finalTemp;

	public bool dontRotateParachute;

	public bool deactivateOnRepack = true;

	public double timeUnpacked;

	public Vessel currentVessel;

	public BaseField deployfield;

	public static string cacheAutoLOC_7003409;

	public static string cacheAutoLOC_7003410;

	public static string cacheAutoLOC_7003411;

	public static string cacheAutoLOC_6003048;

	public bool IsMultipleCubesActive => true;

	[KSPAction("#autoLOC_6001348", activeEditor = false)]
	public void DeployAction(KSPActionParam param)
	{
		Deploy();
	}

	[KSPAction("#autoLOC_6001347", activeEditor = false)]
	public void CutAction(KSPActionParam param)
	{
		CutParachute();
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001348")]
	public void Deploy()
	{
		if (deploymentState != 0)
		{
			return;
		}
		if (base.part.ShieldedFromAirstream && !shieldedCanDeploy)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_214578", base.part.partInfo.title), 6f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		deploymentState = deploymentStates.ACTIVE;
		persistentState = "ACTIVE";
		base.part.stackIcon.SetIconColor(XKCDColors.LightCyan);
		base.Events["Deploy"].active = false;
		base.Events["CutParachute"].active = false;
		base.Events["Disarm"].active = true;
		if (this is ModuleEvaChute)
		{
			AnalyticsUtil.LogEVAChuteDeployed((ModuleEvaChute)this);
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001349")]
	public virtual void CutParachute()
	{
		if (deploymentState != deploymentStates.const_4 && deploymentState != deploymentStates.ACTIVE)
		{
			deploymentState = deploymentStates.const_4;
			persistentState = "CUT";
			base.part.maximum_drag = baseDrag + stowedDrag;
			SetDragCubes_Stowed();
			base.part.stackIcon.SetIconColor(XKCDColors.Red);
			base.Events["CutParachute"].active = false;
			base.Events["Repack"].active = true;
			base.Events["Disarm"].active = false;
			canopy.gameObject.SetActive(value: false);
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4f, guiName = "#autoLOC_6001350")]
	public void Repack()
	{
		if (deploymentState != deploymentStates.const_4)
		{
			return;
		}
		if (!(this is ModuleEvaChute) && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode) && FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value < 1)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_214609", 1.ToString()));
			return;
		}
		if (cap != null)
		{
			cap.gameObject.SetActive(value: true);
		}
		canopy.gameObject.SetActive(value: false);
		deploymentState = deploymentStates.STOWED;
		persistentState = "STOWED";
		if (deactivateOnRepack)
		{
			base.part.deactivate();
		}
		base.part.maximum_drag = baseDrag + stowedDrag;
		SetDragCubes_Stowed();
		base.part.stackIcon.SetIconColor(Color.white);
		base.Events["Deploy"].active = true;
		base.Events["CutParachute"].active = false;
		base.Events["Repack"].active = false;
		base.Events["Disarm"].active = false;
	}

	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001351")]
	public void Disarm()
	{
		if (deploymentState == deploymentStates.ACTIVE)
		{
			deploymentState = deploymentStates.STOWED;
			persistentState = "STOWED";
			base.part.deactivate();
			base.part.stackIcon.SetIconColor(Color.white);
			base.Events["Deploy"].active = true;
			base.Events["CutParachute"].active = false;
			base.Events["Repack"].active = false;
			base.Events["Disarm"].active = false;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		deployFx = new FXGroup("deploy");
		base.part.fxGroups.Add(deployFx);
		deploymentState = (deploymentStates)Enum.Parse(typeof(deploymentStates), persistentState);
		SetDragCubes_Stowed();
	}

	public override void OnAwake()
	{
		base.OnAwake();
		chuteTemp = startingTemp;
		deployfield = base.Fields["deploySafe"];
	}

	public override void OnStart(StartState state)
	{
		if (base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = "PARACHUTES";
		}
		canopy = base.part.FindModelTransform(canopyName);
		cap = base.part.FindModelTransform(capName);
		deployFx = base.part.findFxGroup("deploy");
		baseDrag = base.part.maximum_drag;
		lerpTime = 0f;
		base.Events["Repack"].active = false;
		((UI_FloatRange)base.Fields["minAirPressureToOpen"].uiControlEditor).minValue = clampMinAirPressure;
		SetupAnimation();
		if (Anim != null)
		{
			Anim.wrapMode = WrapMode.Once;
		}
		if (cap != null)
		{
			cap.gameObject.SetActive(value: true);
		}
		if (canopy != null)
		{
			canopy.gameObject.SetActive(value: false);
		}
		if (state != StartState.Editor)
		{
			if (base.vessel != null)
			{
				currentVessel = base.vessel;
			}
			SetDragCubes_Stowed();
			SetUIEVents();
			base.part.ActivatesEvenIfDisconnected = true;
			lastRot = canopy.rotation;
			GameEvents.onVesselWasModified.Add(OnVesselWasModified);
			UpdateSymmetry(base.vessel);
		}
	}

	public void SetUIEVents()
	{
		base.Events["Deploy"].active = false;
		base.Events["CutParachute"].active = false;
		base.Events["Repack"].active = false;
		base.Events["Disarm"].active = false;
		if (deploymentState == deploymentStates.STOWED)
		{
			base.part.maximum_drag = baseDrag + stowedDrag;
			base.Events["Deploy"].active = true;
			return;
		}
		if (deploymentState == deploymentStates.ACTIVE)
		{
			base.part.stackIcon.SetIconColor(XKCDColors.LightCyan);
			base.Events["Disarm"].active = true;
			return;
		}
		if (deploymentState != deploymentStates.SEMIDEPLOYED && deploymentState != deploymentStates.DEPLOYED)
		{
			if (deploymentState == deploymentStates.const_4)
			{
				base.Events["Repack"].active = true;
				if (cap != null)
				{
					cap.gameObject.SetActive(value: false);
				}
				base.part.stackIcon.SetIconColor(XKCDColors.Red);
			}
			return;
		}
		if (canopy != null)
		{
			canopy.gameObject.SetActive(value: true);
		}
		if (cap != null)
		{
			cap.gameObject.SetActive(value: false);
		}
		base.Events["CutParachute"].active = true;
		if (deploymentState == deploymentStates.SEMIDEPLOYED)
		{
			Anim[semiDeployedAnimation].speed = 1f;
			Anim[semiDeployedAnimation].normalizedTime = animTime;
			Anim[semiDeployedAnimation].enabled = true;
			Anim.Play(semiDeployedAnimation);
			base.part.stackIcon.SetIconColor(XKCDColors.BrightYellow);
			SetDragCube_SemiDeployed(Mathf.Pow(animTime, deploymentCurve));
		}
		else if (deploymentState == deploymentStates.DEPLOYED)
		{
			Anim[fullyDeployedAnimation].speed = 1f;
			Anim[fullyDeployedAnimation].normalizedTime = animTime;
			Anim[fullyDeployedAnimation].enabled = true;
			Anim.Play(fullyDeployedAnimation);
			base.part.stackIcon.SetIconColor(XKCDColors.RadioactiveGreen);
			SetDragCube_Deployed(Mathf.Pow(animTime, deploymentCurve));
		}
	}

	public virtual void OnDestroy()
	{
		GameEvents.onVesselWasModified.Remove(OnVesselWasModified);
	}

	public void SetupAnimation()
	{
		if (!(Anim == null))
		{
			return;
		}
		Animation[] componentsInChildren = base.part.GetComponentsInChildren<Animation>();
		if (componentsInChildren.Length == 1)
		{
			Anim = componentsInChildren[0];
		}
		else if (componentsInChildren.Length > 1)
		{
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				if (componentsInChildren[i][semiDeployedAnimation] != null && componentsInChildren[i][fullyDeployedAnimation] != null)
				{
					Anim = componentsInChildren[i];
					break;
				}
			}
			if (Anim == null)
			{
				Debug.LogError("[ModuleParachute]: " + componentsInChildren.Length + " Animation components found in child objects, but none contain states " + semiDeployedAnimation + " and " + fullyDeployedAnimation, base.gameObject);
			}
		}
		else
		{
			Debug.LogError("[ModuleParachute]: No animation components found in child objects", base.gameObject);
		}
	}

	public override void OnActive()
	{
		if (stagingEnabled)
		{
			Deploy();
		}
	}

	public double GetArea(DragCube cube)
	{
		Vector3 faceDirection = DragCubeList.GetFaceDirection(DragCube.DragFace.const_3);
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			Vector3 rhs = DragCubeList.faceDirections[i];
			float dotNormalized = (Vector3.Dot(faceDirection, rhs) + 1f) * 0.5f;
			float num2 = PhysicsGlobals.DragCurveValue(PhysicsGlobals.SurfaceCurves, dotNormalized, 0f);
			float num3 = cube.Area[i] * num2;
			num += num3 * cube.Drag[i];
		}
		return (double)num * (double)PhysicsGlobals.DragCubeMultiplier * (double)PhysicsGlobals.DragMultiplier;
	}

	public void CalcBaseStats()
	{
		if (base.part.DragCubes.None)
		{
			return;
		}
		int count = base.part.DragCubes.Cubes.Count;
		for (int i = 0; i < count; i++)
		{
			DragCube dragCube = base.part.DragCubes.Cubes[i];
			if (dragCube.Name == "DEPLOYED")
			{
				areaDeployed = GetArea(dragCube);
			}
			else if (dragCube.Name == "SEMIDEPLOYED")
			{
				areaSemi = GetArea(dragCube);
			}
		}
		invThermalMass = 1.0 / (areaDeployed * chuteThermalMassPerArea);
		double num = 0.0;
		int num2 = 401;
		double num3 = 100.0;
		bool flag = true;
		double num4 = 1.0 / refSpeedOfSound;
		double num5 = chuteMaxTemp * safeMult;
		SetConvectiveStats(refDensity, num * PhysicsGlobals.NewtonianTemperatureFactor, num * num4, 0.0);
		while (!(Math.Abs(shockTemp - num5) <= 0.1) && --num2 >= 0)
		{
			if (shockTemp > num5)
			{
				if (flag)
				{
					num3 *= 0.5;
				}
				flag = false;
				num -= num3;
				if (num < 0.0)
				{
					num = 0.0;
				}
			}
			else
			{
				if (!flag)
				{
					num3 *= 0.5;
				}
				flag = true;
				num += num3;
			}
			SetConvectiveStats(refDensity, num * PhysicsGlobals.NewtonianTemperatureFactor, num * num4, 0.0);
		}
		maxSafeSpeedAtRef = Math.Round(num);
	}

	public override string GetInfo()
	{
		string text = "";
		CalcBaseStats();
		if (base.part.dragModel != 0 && base.part.dragModel != Part.DragModel.CUBE)
		{
			text += Localizer.Format("#autoLOC_214903", stowedDrag.ToString("0.0###"));
			text += Localizer.Format("#autoLOC_214904", semiDeployedDrag.ToString("0.0###"));
			text += Localizer.Format("#autoLOC_214905", fullyDeployedDrag.ToString("0.####"));
		}
		else
		{
			text += Localizer.Format("#autoLOC_214897", (Math.Sqrt(areaSemi / Math.PI) * 2.0).ToString("N1"));
			text += Localizer.Format("#autoLOC_214898", (Math.Sqrt(areaDeployed / Math.PI) * 2.0).ToString("N1"));
			text += Localizer.Format("#autoLOC_214899", maxSafeSpeedAtRef.ToString("F0"));
		}
		text += Localizer.Format("#autoLOC_214907", deployAltitude.ToString("0"));
		return text + Localizer.Format("#autoLOC_214908", minAirPressureToOpen.ToString("0.0###"));
	}

	public virtual void FixedUpdate()
	{
		deployfield.guiActive = false;
		deploySafe = "";
		Vessel vessel = base.vessel;
		if (!base.part.packed)
		{
			if (vessel != null)
			{
				SetConvectiveStats(vessel.atmDensity, vessel.externalTemperature, vessel.mach, vessel.convectiveCoefficient);
			}
			else
			{
				SetConvectiveStats(0.0, 4.0, 0.0, 0.0);
			}
			deploymentSafeState = deploymentSafeStates.NONE;
			if (vessel != null && vessel.atmDensity > 0.0 && (deploymentState == deploymentStates.STOWED || deploymentState == deploymentStates.ACTIVE))
			{
				deployfield.guiActive = true;
				deploySafe = DeploySafe(out deploymentSafeState);
			}
			switch (deploymentSafeState)
			{
			default:
				base.part.stackIcon.SetBackgroundColor(XKCDColors.White);
				break;
			case deploymentSafeStates.UNSAFE:
				base.part.stackIcon.SetBackgroundColor(XKCDColors.Red);
				break;
			case deploymentSafeStates.RISKY:
				base.part.stackIcon.SetBackgroundColor(XKCDColors.KSPNotSoGoodOrange);
				break;
			}
			if (deploymentState != 0)
			{
				bool flag = false;
				if (deploymentState != deploymentStates.const_4 && deploymentState != deploymentStates.DEPLOYED)
				{
					flag = ShouldDeploy();
				}
				switch (deploymentState)
				{
				case deploymentStates.ACTIVE:
					animTime = 0f;
					if ((FlightGlobals.getStaticPressure(base.part.transform.position, vessel.mainBody) * pressureRecip > (double)minAirPressureToOpen || flag) && IsMovingFastEnoughToDeploy() && automateSafeDeploy >= (int)deploymentSafeState && PassedAdditionalDeploymentChecks())
					{
						if (cap != null)
						{
							cap.gameObject.SetActive(value: false);
						}
						canopy.gameObject.SetActive(value: true);
						deploymentState = deploymentStates.SEMIDEPLOYED;
						persistentState = "SEMIDEPLOYED";
						base.Events["CutParachute"].active = true;
						base.Events["Disarm"].active = false;
						base.part.stackIcon.SetIconColor(XKCDColors.BrightYellow);
						lerpTime = 0f;
						deployFx.Burst();
						PlayDeployAnimation(semiDeployedAnimation, semiDeploymentSpeed);
						OnParachuteSemiDeployed();
					}
					break;
				case deploymentStates.SEMIDEPLOYED:
					if (IsAnimationPlaying())
					{
						animTime = (lerpTime = GetAnimationNormalizedTime(semiDeployedAnimation));
						lerpTime = Mathf.Pow(lerpTime, deploymentCurve);
						base.part.maximum_drag = baseDrag + Mathf.Lerp(stowedDrag, semiDeployedDrag, lerpTime);
						SetDragCube_SemiDeployed(lerpTime);
					}
					else
					{
						if (lerpTime < 1f)
						{
							SetDragCube_SemiDeployed(1f);
						}
						float num = 1f;
						lerpTime = 1f;
						animTime = num;
						if (flag)
						{
							base.part.stackIcon.SetIconColor(XKCDColors.RadioactiveGreen);
							deployFx.Burst();
							deploymentState = deploymentStates.DEPLOYED;
							persistentState = "DEPLOYED";
							lerpTime = 0f;
							base.Events["CutParachute"].active = true;
							base.Events["Disarm"].active = false;
							animTime = 0f;
							PlayDeployAnimation(fullyDeployedAnimation, deploymentSpeed);
							OnParachuteFullyDeployed();
						}
					}
					UpdateCut();
					break;
				case deploymentStates.DEPLOYED:
					if (IsAnimationPlaying())
					{
						animTime = (lerpTime = GetAnimationNormalizedTime(fullyDeployedAnimation));
						lerpTime = Mathf.Pow(lerpTime, deploymentCurve);
						base.part.maximum_drag = baseDrag + Mathf.Lerp(semiDeployedDrag, fullyDeployedDrag, lerpTime);
						SetDragCube_Deployed(lerpTime);
					}
					else
					{
						if (lerpTime < 1f)
						{
							SetDragCube_Deployed(1f);
						}
						float num = 1f;
						lerpTime = 1f;
						animTime = num;
					}
					UpdateCut();
					break;
				}
				if (deploymentState != deploymentStates.SEMIDEPLOYED && deploymentState != deploymentStates.DEPLOYED)
				{
					base.part.DragCubes.SetDragVectorRotation(rotateDragVector: false);
					if (base.part.dragVectorDir != Vector3.zero)
					{
						lastRot = Quaternion.LookRotation((invertCanopy ? (-1f) : 1f) * base.part.dragVectorDir, base.transform.forward);
					}
					else
					{
						lastRot = canopy.rotation;
					}
					return;
				}
				if (base.part.dragVectorDir != Vector3.zero)
				{
					if (!dontRotateParachute)
					{
						Vector3 upwards = base.transform.forward;
						if (vessel != null && symmetryCount > 0)
						{
							upwards = Vector3.Normalize(vessel.CurrentCoM - base.part.transform.localPosition);
						}
						canopy.rotation = Quaternion.LookRotation((invertCanopy ? (-1f) : 1f) * base.part.dragVectorDir, upwards);
						if (symmetryCount > 0 && spreadAngle > 0f)
						{
							float num2 = Mathf.Clamp(spreadAngle * (float)symmetryCount, 0f, 45f);
							if (deploymentState == deploymentStates.SEMIDEPLOYED)
							{
								num2 /= 3f;
							}
							else if (IsAnimationPlaying())
							{
								num2 /= 3f - 2f * Anim[fullyDeployedAnimation].normalizedTime;
							}
							canopy.Rotate(num2, 0f, 0f, Space.Self);
						}
					}
					float x = Time.time + (float)(base.part.craftID % 32);
					canopy.Rotate(new Vector3(10f * (Mathf.PerlinNoise(x, 0f) - 0.5f), 10f * (Mathf.PerlinNoise(x, 8f) - 0.5f), 10f * (Mathf.PerlinNoise(x, 16f) - 0.5f)));
					Quaternion dragVectorRotation = Quaternion.LookRotation(base.part.partTransform.InverseTransformDirection(canopy.forward));
					base.part.DragCubes.SetDragVectorRotation(dragVectorRotation);
					canopy.rotation = Quaternion.RotateTowards(lastRot, canopy.rotation, rotationSpeedDPS * TimeWarp.fixedDeltaTime);
				}
				lastRot = canopy.rotation;
			}
			else
			{
				animTime = 0f;
				if (base.part.dragVectorDir != Vector3.zero)
				{
					lastRot = Quaternion.LookRotation((invertCanopy ? (-1f) : 1f) * base.part.dragVectorDir, base.transform.forward);
				}
				else
				{
					lastRot = canopy.rotation;
				}
			}
		}
		else
		{
			if (HighLogic.LoadedSceneIsFlight && vessel != null && vessel.LandedOrSplashed && (deploymentState == deploymentStates.DEPLOYED || deploymentState == deploymentStates.SEMIDEPLOYED))
			{
				CutParachute();
			}
			lerpTime = animTime;
		}
	}

	public virtual void OnParachuteSemiDeployed()
	{
	}

	public virtual void OnParachuteFullyDeployed()
	{
	}

	public bool ShouldDeploy()
	{
		if (!(FlightGlobals.getAltitudeAtPos(base.part.transform.position, base.vessel.mainBody) < deployAltitude))
		{
			return Physics.Raycast(base.part.transform.position, -base.vessel.upAxis, deployAltitude, 32768, QueryTriggerInteraction.Ignore);
		}
		return true;
	}

	public bool IsMovingFastEnoughToDeploy()
	{
		return (base.part.Rigidbody.velocity + Krakensbane.GetFrameVelocity()).sqrMagnitude > 1.0;
	}

	public virtual bool PassedAdditionalDeploymentChecks()
	{
		return true;
	}

	public virtual bool IsAnimationPlaying()
	{
		return Anim.isPlaying;
	}

	public virtual float GetAnimationNormalizedTime(string animationName)
	{
		return Anim[animationName].normalizedTime;
	}

	public virtual void PlayDeployAnimation(string animationName, float deploymentSpeed)
	{
		Anim[animationName].speed = deploymentSpeed;
		Anim[animationName].normalizedTime = 0f;
		Anim[animationName].enabled = true;
		Anim.Play(animationName);
	}

	public void OnVesselWasModified(Vessel v)
	{
		if (currentVessel == null && base.vessel != null)
		{
			currentVessel = base.vessel;
		}
		if (currentVessel == null || base.vessel == null)
		{
			return;
		}
		UpdateSymmetry(v);
		if (v.persistentId == currentVessel.persistentId && base.vessel.persistentId != currentVessel.persistentId)
		{
			currentVessel = base.vessel;
			if (deploymentState == deploymentStates.STOWED || deploymentState == deploymentStates.ACTIVE)
			{
				timeUnpacked = Planetarium.GetUniversalTime();
			}
		}
	}

	public void UpdateSymmetry(Vessel v)
	{
		if (base.vessel != null && v == base.vessel)
		{
			if (base.part.symmetryCounterparts != null && base.part.symmetryCounterparts.Count > 0)
			{
				symmetryCount = base.part.symmetryCounterparts.Count + 1;
			}
			else
			{
				symmetryCount = 0;
			}
		}
	}

	public void OnPartUnpack()
	{
		timeUnpacked = Planetarium.GetUniversalTime();
	}

	public void SetDragCubes_Stowed()
	{
		base.part.DragCubes.SetCubeWeight("PACKED", 1f);
		base.part.DragCubes.SetCubeWeight("SEMIDEPLOYED", 0f);
		base.part.DragCubes.SetCubeWeight("DEPLOYED", 0f);
		base.part.DragCubes.SetOcclusionMultiplier(1f);
	}

	public void SetDragCube_SemiDeployed(float semiDeployed)
	{
		base.part.DragCubes.SetCubeWeight("PACKED", 1f - semiDeployed);
		base.part.DragCubes.SetCubeWeight("SEMIDEPLOYED", semiDeployed);
		base.part.DragCubes.SetCubeWeight("DEPLOYED", 0f);
		base.part.DragCubes.SetOcclusionMultiplier(0f);
	}

	public void SetDragCube_Deployed(float deployed)
	{
		base.part.DragCubes.SetCubeWeight("PACKED", 0f);
		base.part.DragCubes.SetCubeWeight("SEMIDEPLOYED", 1f - deployed);
		base.part.DragCubes.SetCubeWeight("DEPLOYED", deployed);
		base.part.DragCubes.SetOcclusionMultiplier(0f);
	}

	public string[] GetDragCubeNames()
	{
		SetupAnimation();
		if (Anim == null)
		{
			return null;
		}
		return new string[3] { "PACKED", "SEMIDEPLOYED", "DEPLOYED" };
	}

	public void AssumeDragCubePosition(string name)
	{
		switch (name)
		{
		case "DEPLOYED":
			Anim[semiDeployedAnimation].normalizedTime = 0f;
			Anim[semiDeployedAnimation].normalizedSpeed = 0f;
			Anim[semiDeployedAnimation].enabled = false;
			Anim[fullyDeployedAnimation].normalizedTime = 1f;
			Anim[fullyDeployedAnimation].normalizedSpeed = 0f;
			Anim[fullyDeployedAnimation].enabled = true;
			Anim.Play(fullyDeployedAnimation);
			break;
		case "SEMIDEPLOYED":
			Anim[semiDeployedAnimation].normalizedTime = 1f;
			Anim[semiDeployedAnimation].normalizedSpeed = 0f;
			Anim[semiDeployedAnimation].enabled = true;
			Anim[fullyDeployedAnimation].normalizedTime = 0f;
			Anim[fullyDeployedAnimation].normalizedSpeed = 0f;
			Anim[fullyDeployedAnimation].enabled = false;
			Anim.Play(semiDeployedAnimation);
			break;
		case "PACKED":
			Anim[semiDeployedAnimation].normalizedTime = 0f;
			Anim[semiDeployedAnimation].normalizedSpeed = 0f;
			Anim[semiDeployedAnimation].enabled = true;
			Anim[fullyDeployedAnimation].normalizedTime = 0f;
			Anim[fullyDeployedAnimation].normalizedSpeed = 0f;
			Anim[fullyDeployedAnimation].enabled = false;
			Anim.Play(semiDeployedAnimation);
			break;
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public void SetConvectiveStats(double density, double extTemp, double mach, double convCoeff)
	{
		if (density > 0.0)
		{
			convectionArea = areaDeployed;
			machHeatMult = Math.Pow(machHeatMultBase + mach, machHeatMultPow);
			double fullToCrossSectionLerpStart = PhysicsGlobals.FullToCrossSectionLerpStart;
			machHeatMult = UtilMath.Lerp(1.0, machHeatMult, (mach * machHeatMultScalar - fullToCrossSectionLerpStart) / (PhysicsGlobals.FullToCrossSectionLerpEnd - fullToCrossSectionLerpStart));
			machHeatMult = UtilMath.Lerp(1.0, machHeatMult, density * machHeatDensityFadeoutMult);
			convectivekW = 0.001 * convCoeff * convectionArea * machHeatMult;
			shockTemp = extTemp * machHeatMult;
		}
		else
		{
			convectivekW = 0.0;
			shockTemp = PhysicsGlobals.SpaceTemperature;
		}
	}

	public string DeploySafe(out deploymentSafeStates state)
	{
		if (Planetarium.GetUniversalTime() - timeUnpacked < 1.0)
		{
			state = deploymentSafeStates.UNSAFE;
			return cacheAutoLOC_7003411;
		}
		if (shockTemp <= chuteMaxTemp * safeMult)
		{
			state = deploymentSafeStates.SAFE;
			return cacheAutoLOC_7003409;
		}
		double num = (shockTemp - chuteTemp) * UtilMath.Clamp01(convectivekW * invThermalMass * secondsForRisky);
		finalTemp = chuteTemp + num;
		if (finalTemp <= chuteMaxTemp)
		{
			state = deploymentSafeStates.RISKY;
			return cacheAutoLOC_7003410;
		}
		state = deploymentSafeStates.UNSAFE;
		return cacheAutoLOC_7003411;
	}

	public void UpdateCut()
	{
		if ((base.part.Rigidbody.velocity + Krakensbane.GetFrameVelocity()).sqrMagnitude < (double)(autoCutSpeed * autoCutSpeed) && base.part.vessel.LandedOrSplashed)
		{
			CutParachute();
		}
		else if (base.part.atmDensity == 0.0)
		{
			CutParachute();
		}
		else if (base.vessel.situation == Vessel.Situations.SPLASHED)
		{
			chuteTemp = base.vessel.atmosphericTemperature;
		}
		else if ((!base.part.ShieldedFromAirstream || shieldedCanDeploy) && (deploymentState == deploymentStates.SEMIDEPLOYED || deploymentState == deploymentStates.DEPLOYED))
		{
			if (chuteTemp < PhysicsGlobals.SpaceTemperature)
			{
				chuteTemp = startingTemp;
			}
			double num = chuteTemp;
			double num2 = 0.001 * PhysicsGlobals.StefanBoltzmanConstant * areaDeployed * chuteEmissivity * PhysicsGlobals.RadiationFactor;
			double num3 = convectivekW * (shockTemp - chuteTemp);
			double num4 = Math.Min(chuteTemp, chuteMaxTemp);
			num4 *= num4;
			num3 -= num2 * num4 * num4;
			chuteTemp += num3 * invThermalMass * (double)TimeWarp.fixedDeltaTime;
			num3 = convectivekW * (shockTemp - chuteTemp);
			num4 = Math.Min(chuteTemp, chuteMaxTemp);
			num4 *= num4;
			num3 -= num2 * num4 * num4;
			chuteTemp += num3 * invThermalMass * (double)TimeWarp.fixedDeltaTime;
			chuteTemp = (num + chuteTemp) * 0.5;
			if (double.IsNaN(chuteTemp) || chuteTemp < PhysicsGlobals.SpaceTemperature)
			{
				chuteTemp = PhysicsGlobals.SpaceTemperature;
			}
			if (chuteTemp > chuteMaxTemp)
			{
				ScreenMessages.PostScreenMessage("<color=orange>" + Localizer.Format("#autoLOC_215326", base.part.partInfo.title) + "</color>", 6f, ScreenMessageStyle.UPPER_CENTER);
				FlightLogger.fetch.LogEvent(Localizer.Format("#autoLOC_215327", base.part.partInfo.title));
				CutParachute();
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003048;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7003409 = Localizer.Format("#autoLOC_7003409");
		cacheAutoLOC_7003410 = Localizer.Format("#autoLOC_7003410");
		cacheAutoLOC_7003411 = Localizer.Format("#autoLOC_7003411");
		cacheAutoLOC_6003048 = Localizer.Format("#autoLoc_6003048");
	}
}
