using System;
using System.Collections.Generic;
using Expansions.Serenity.RobotArmFX;
using ns9;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRobotArmScanner : ModuleDeployablePart
{
	public enum ArmDeployState
	{
		RETRACTED,
		UNPACKING,
		EXTENDING,
		SCANNING,
		RETRACTING,
		PACKING,
		BROKEN,
		PREVIEWRANGE
	}

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001352")]
	public string scanStatus = string.Empty;

	[KSPField]
	public string unpackAnimationName;

	[KSPField]
	public string editorReachAnimationName;

	public ArmDeployState _deployState;

	[KSPField]
	public string firstJointTransformName = "LowerArm";

	public Transform firstJointTransform;

	public Quaternion firstJointOrigRot;

	[KSPField]
	public float firstJointAlignOffset;

	[KSPField]
	public string secondJointTransformName = "UpperArm";

	public Transform secondJointTransform;

	public Quaternion secondJointOrigRot;

	[KSPField]
	public float secondJointAlignOffset;

	[KSPField]
	public string gimbalTransformName = "Gimbal";

	public Transform gimbalTransform;

	public Quaternion gimbalOrigRot;

	[KSPField]
	public float gimbalAlignOffset;

	[KSPField]
	public string instTransformName = "InstrumentMount";

	public Transform instTransform;

	public Quaternion instOrigRot;

	[KSPField]
	public float instAlignOffset;

	[KSPField]
	public string instCentreTransformName = "InstrumentHousing";

	public Transform instCentreTransform;

	public Quaternion instCentreOrigRot;

	public bool emergencyRetractScanner;

	public List<Vector2> safeRetractPeriods = new List<Vector2>();

	[KSPField]
	public string rangeTriggerColliderTransformName = "rangeTrigger";

	public Transform rangeTriggerTransform;

	public MeshRenderer rangeTriggerRenderer;

	public Material rangeTriggerMaterial;

	[KSPField]
	public float distanceFromSurface = 1f;

	[KSPField]
	public float emergencyStopDistanceFromSurface = 0.35f;

	public List<GameObject> rocsInRange = new List<GameObject>();

	public float firstArmLength;

	public float secondArmLength;

	public float maxArmLength;

	public Vector3 scanPosition;

	public Vector3 instTargetPos;

	public GClass0 scanROC;

	public string scanROCDisplayName;

	public Quaternion baseExtTargetRot;

	public Quaternion firstJointExtTargetRot;

	public Quaternion secondJointExtTargetRot;

	public Quaternion gimbalExtTargetRot;

	public Quaternion instExtTargetRot;

	[KSPField]
	public float cancelScanDistance;

	[KSPField]
	public float firstJointRotStartAngleModifier;

	public Vector3 scanStartPosition;

	public ModuleScienceExperiment moduleScienceExperiment;

	public Vector3 closestScanPoint;

	[KSPField]
	public float sphereCastRadius = 0.35f;

	[KSPField]
	public float rangeTriggerRadius = 4f;

	[KSPField]
	public string rangeTriggerParentTransformName = "ROCArm_01";

	[KSPField]
	public float firstJointRotationLimit = 90f;

	public RaycastHit hitInfo;

	public static RaycastHit[] hits = new RaycastHit[10];

	[KSPField]
	public string unpackEffectName = "unpacking";

	[KSPField]
	public string extendEffectName = "extending";

	[KSPField]
	public string scanEffectName = "scanning";

	[KSPField]
	public string retractEffectName = "retracting";

	[KSPField]
	public string packEffectName = "packing";

	public AudioFX packAudioFX;

	[SerializeField]
	public List<RobotArmScannerFX> scannerEffectList;

	public bool extendingUpwards = true;

	public bool startGoingBackwards;

	public float animationStartTime;

	public float pauseStartTime;

	public float paused_anim_speed;

	public string playingAnimation;

	public static string cacheAutoLOC_8004274;

	public static string cacheAutoLOC_8004432;

	public string cacheAutoLOC_8004426;

	public static string cacheAutoLOC_8004427;

	public new ArmDeployState deployState
	{
		get
		{
			return _deployState;
		}
		set
		{
			_deployState = value;
			switch (value)
			{
			case ArmDeployState.UNPACKING:
				base.deployState = DeployState.EXTENDING;
				break;
			case ArmDeployState.EXTENDING:
				base.deployState = DeployState.EXTENDING;
				break;
			case ArmDeployState.SCANNING:
				base.deployState = DeployState.EXTENDED;
				break;
			case ArmDeployState.RETRACTING:
				base.deployState = DeployState.RETRACTING;
				break;
			case ArmDeployState.PACKING:
				base.deployState = DeployState.RETRACTING;
				break;
			case ArmDeployState.BROKEN:
				base.deployState = DeployState.BROKEN;
				break;
			default:
				base.deployState = DeployState.RETRACTED;
				break;
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		partType = Localizer.Format("#autoLOC_8004353");
		scannerEffectList = new List<RobotArmScannerFX>();
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("SCANNEREFFECTS", ref node2))
		{
			ConfigNode[] nodes = node2.GetNodes("EFFECT");
			for (int i = 0; i < nodes.Length; i++)
			{
				RobotArmScannerFX robotArmScannerFX = RobotArmScannerFX.CreateInstanceOfRobotArmScannerFX(nodes[i].GetValue("className"));
				if (robotArmScannerFX != null)
				{
					robotArmScannerFX.Load(nodes[i]);
					scannerEffectList.Add(robotArmScannerFX);
				}
			}
		}
		string[] values = node.GetValues("safeRetractPeriods");
		if (values == null)
		{
			return;
		}
		for (int j = 0; j < values.Length; j++)
		{
			string[] array = values[j].Split(' ');
			if (array.Length == 2)
			{
				float x = float.Parse(array[0]);
				float y = float.Parse(array[1]);
				safeRetractPeriods.Add(new Vector2(x, y));
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		ConfigNode configNode = new ConfigNode("SCANNEREFFECTS");
		for (int i = 0; i < scannerEffectList.Count; i++)
		{
			ConfigNode node2 = new ConfigNode("EFFECT");
			scannerEffectList[i].Save(node2);
			configNode.AddNode(node2);
		}
		node.AddNode(configNode);
	}

	public override void OnAwake()
	{
		base.OnAwake();
		if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onPartActionUICreate.Add(OnPartActionUICreate);
			if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
			{
				SetupScannerTransforms();
				base.part.Effect(unpackEffectName, 0f);
				base.part.Effect(extendEffectName, 0f);
				base.part.Effect(scanEffectName, 0f);
				base.part.Effect(retractEffectName, 0f);
				base.part.Effect(packEffectName, 0f);
			}
		}
	}

	public void SetupScannerTransforms()
	{
		firstJointTransform = base.part.FindModelTransform(firstJointTransformName);
		secondJointTransform = base.part.FindModelTransform(secondJointTransformName);
		gimbalTransform = base.part.FindModelTransform(gimbalTransformName);
		instTransform = base.part.FindModelTransform(instTransformName);
		instCentreTransform = base.part.FindModelTransform(instCentreTransformName);
	}

	public void SetupScannerEffectList()
	{
		for (int i = 0; i < scannerEffectList.Count; i++)
		{
			scannerEffectList[i].OnStart(base.part);
		}
	}

	public override void OnStart(StartState state)
	{
		if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		if (base.deployState == DeployState.BROKEN)
		{
			deployState = ArmDeployState.BROKEN;
		}
		else
		{
			deployState = ArmDeployState.RETRACTED;
		}
		base.OnStart(state);
		moduleScienceExperiment = base.part.FindModuleImplementing<ModuleScienceExperiment>();
		if (moduleScienceExperiment != null)
		{
			moduleScienceExperiment.DeployEventDisabled = true;
		}
		rangeTriggerTransform = base.part.FindModelTransform(rangeTriggerColliderTransformName);
		if (rangeTriggerTransform == null)
		{
			if (!HighLogic.LoadedSceneIsEditor)
			{
				Transform parent = base.part.FindModelTransform(rangeTriggerParentTransformName);
				GameObject obj = new GameObject(rangeTriggerColliderTransformName, typeof(SphereCollider));
				obj.transform.position = panelRotationTransform.position;
				obj.transform.SetParent(parent);
				obj.layer = LayerMask.NameToLayer("Local Scenery");
				SphereCollider component = obj.GetComponent<SphereCollider>();
				component.isTrigger = true;
				component.radius = rangeTriggerRadius;
			}
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			rangeTriggerTransform.gameObject.SetActive(value: false);
		}
		if (scannerEffectList == null)
		{
			scannerEffectList = new List<RobotArmScannerFX>();
		}
		SetupScannerEffectList();
		base.Events["Extend"].guiActiveEditor = false;
		CacheLocalStrings();
		GameEvents.onGamePause.Add(OnPause);
		GameEvents.onGameUnpause.Add(OnUnpause);
		GameEvents.onTimeWarpRateChanged.Add(OnTimeWarpRateChanged);
		GameEvents.OnInventoryPartOnMouseChanged.Add(OnCurrentMousePartChanged);
	}

	public override void OnUpdate()
	{
		if ((HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor) && (deployState == ArmDeployState.UNPACKING || deployState == ArmDeployState.EXTENDING || deployState == ArmDeployState.SCANNING) && (panelRotationTransform.position - scanStartPosition).magnitude >= cancelScanDistance)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004273", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
			Retract();
		}
	}

	public override void OnInventoryModeDisable()
	{
		base.part.Effect(unpackEffectName, 0f);
		base.part.Effect(extendEffectName, 0f);
		base.part.Effect(scanEffectName, 0f);
		base.part.Effect(retractEffectName, 0f);
		base.part.Effect(packEffectName, 0f);
		base.OnInventoryModeDisable();
		ParticleSystem[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Stop();
		}
		KSPParticleEmitter[] componentsInChildren2 = base.transform.GetComponentsInChildren<KSPParticleEmitter>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].emit = false;
		}
	}

	public override void OnInventoryModeEnable()
	{
		base.OnInventoryModeEnable();
		KSPParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<KSPParticleEmitter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].emit = true;
		}
	}

	public void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") && HighLogic.LoadedSceneIsGame)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other != null && other.gameObject != null && other.gameObject.CompareTag("ROC"))
		{
			rocsInRange.Add(other.gameObject);
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
		if (other != null && other.gameObject != null && other.gameObject.CompareTag("ROC"))
		{
			rocsInRange.Remove(other.gameObject);
		}
	}

	public override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		if (deployState == ArmDeployState.UNPACKING || deployState == ArmDeployState.EXTENDING || deployState == ArmDeployState.SCANNING)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004272", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
			Retract();
		}
	}

	public new void OnDestroy()
	{
		GameEvents.onPartActionUICreate.Remove(OnPartActionUICreate);
		GameEvents.onGamePause.Remove(OnPause);
		GameEvents.onGameUnpause.Remove(OnUnpause);
		GameEvents.onTimeWarpRateChanged.Remove(OnTimeWarpRateChanged);
		GameEvents.OnInventoryPartOnMouseChanged.Remove(OnCurrentMousePartChanged);
	}

	public virtual void OnPartActionUICreate(Part actionPart)
	{
		if (actionPart.persistentId == base.part.persistentId)
		{
			UpdatePartActionUI();
		}
	}

	public virtual void UpdatePartActionUI()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (rocsInRange.Count > 0 && CanScan() && deployState == ArmDeployState.RETRACTED)
		{
			closestScanPoint = Vector3.zero;
			scanROC = SelectROCToScan(out closestScanPoint, useRaycast: false);
			if (scanROC != null)
			{
				base.Events["Extend"].active = true;
				base.Events["Extend"].guiName = Localizer.Format("#autoLOC_8004271", scanROC.displayName);
			}
		}
		else
		{
			base.Events["Extend"].active = false;
		}
		if (deployState == ArmDeployState.RETRACTED)
		{
			if (!CanScan())
			{
				base.Fields["scanStatus"].guiActive = true;
				scanStatus = cacheAutoLOC_8004427;
			}
			else if (rocsInRange.Count == 0)
			{
				base.Fields["scanStatus"].guiActive = true;
				scanStatus = cacheAutoLOC_8004426;
			}
			else
			{
				base.Fields["scanStatus"].guiActive = false;
			}
		}
		else
		{
			base.Fields["scanStatus"].guiActive = false;
		}
	}

	public override void DoExtend()
	{
		if (deployState != 0)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004350", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		if (!HighLogic.LoadedSceneIsEditor && base.part.ShieldedFromAirstream && applyShieldingExtend)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_234422", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		if (rocsInRange.Count == 0)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004351", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		if (!CanScan())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004352", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		double amount = 0.0;
		double maxAmount = 0.0;
		int num = 0;
		float num2;
		while (true)
		{
			if (num < resHandler.inputResources.Count)
			{
				if ((!CheatOptions.InfiniteElectricity || !(resHandler.inputResources[num].name == "ElectricCharge")) && (!CheatOptions.InfinitePropellant || !(resHandler.inputResources[num].name != "ElectricCharge")))
				{
					base.part.GetConnectedResourceTotals(resHandler.inputResources[num].id, out amount, out maxAmount);
					num2 = anim[animationName].length * (float)resHandler.inputResources[num].rate;
					if (!(amount >= (double)num2))
					{
						break;
					}
				}
				num++;
				continue;
			}
			if (moduleScienceExperiment.HasExperimentData)
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("OverwriteExperiment", Localizer.Format("#autoLOC_238370", moduleScienceExperiment.experiment.experimentTitle), Localizer.Format("#autoLOC_238371", moduleScienceExperiment.experiment.experimentTitle), HighLogic.UISkin, new DialogGUIButton(moduleScienceExperiment.experimentActionName, delegate
				{
					moduleScienceExperiment.OnExperimentReset = DoExtend;
					moduleScienceExperiment.ResetExperiment();
				}), new DialogGUIButton(Localizer.Format("#autoLOC_253501"), delegate
				{
				})), persistAcrossScenes: false, HighLogic.UISkin);
				return;
			}
			closestScanPoint = Vector3.zero;
			scanROC = SelectROCToScan(out closestScanPoint, useRaycast: true);
			if (scanROC == null)
			{
				Debug.Log("[ModuleRobotArmScanner]: No suitable ROCs to raytrace to!");
				return;
			}
			scanROCDisplayName = scanROC.displayName;
			Vector3 traceDirection = closestScanPoint - panelRotationTransform.position;
			if (!RayCastToROC(panelRotationTransform.position, traceDirection, traceDirection.magnitude, ref hitInfo, performSphereCast: true, sphereCastRadius))
			{
				Debug.Log("[ModuleRobotArmScanner]: No ROC hit during trace!");
				return;
			}
			trackingTransformLocal = scanROC.transform;
			instTargetPos = hitInfo.point;
			scanPosition = panelRotationTransform.position + traceDirection.normalized * ((hitInfo.point - panelRotationTransform.position).magnitude - distanceFromSurface);
			firstArmLength = (secondJointTransform.position - firstJointTransform.position).magnitude;
			secondArmLength = (instTransform.position - secondJointTransform.position).magnitude;
			maxArmLength = firstArmLength + secondArmLength;
			Vector3 vector = scanPosition - panelRotationTransform.position;
			if (vector.magnitude > maxArmLength)
			{
				scanPosition = panelRotationTransform.position + vector.normalized * maxArmLength;
			}
			bool flag = false;
			if (Vector3.Dot(panelRotationTransform.forward, vector.normalized) < 0f)
			{
				flag = true;
			}
			Vector3 vector2 = scanPosition - firstJointTransform.position;
			float num3 = firstArmLength * firstArmLength;
			float num4 = secondArmLength * secondArmLength;
			float num5 = Mathf.Min(vector2.magnitude, maxArmLength);
			float num6 = num5 * num5;
			float num7 = Mathf.Acos((num3 + num6 - num4) / (2f * firstArmLength * num5)) * 57.29578f;
			Vector3 vector3 = scanPosition - firstJointTransform.position;
			Vector3 normalized = Vector3.Cross(vector3, firstJointTransform.up).normalized;
			Vector3 to = Vector3.ProjectOnPlane(vector3, firstJointTransform.up);
			float num8 = Vector3.SignedAngle(vector3, to, -normalized);
			float num9 = firstJointAlignOffset + num8 + num7;
			if (flag)
			{
				num9 = 180f + firstJointAlignOffset - num8 - num7;
			}
			if (!(num9 < -1f * Mathf.Abs(firstJointRotationLimit)) && num9 <= Mathf.Abs(firstJointRotationLimit))
			{
				PlayUnpackAnimation();
				scanStartPosition = panelRotationTransform.position;
				emergencyRetractScanner = false;
				deployState = ArmDeployState.UNPACKING;
				base.Events["Extend"].active = false;
				onMove.Fire(0f, 1f);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004429", partType, scanROCDisplayName), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
			return;
		}
		string text = Localizer.Format("#autoLOC_6001043", resHandler.inputResources[num].title, amount.ToString("0.0"), num2.ToString("0.0"));
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004431", partType, text), 5f, ScreenMessageStyle.UPPER_LEFT);
	}

	public virtual GClass0 SelectROCToScan(out Vector3 closestScanPoint, bool useRaycast)
	{
		GClass0 result = null;
		float num = float.MaxValue;
		closestScanPoint = Vector3.zero;
		Vector3 zero = Vector3.zero;
		float bestDistanceToPoint = 0f;
		int count = rocsInRange.Count;
		while (count-- > 0)
		{
			if (rocsInRange[count] != null)
			{
				GClass0 gClass = rocsInRange[count].GetComponent<GClass0>();
				if (gClass == null)
				{
					gClass = rocsInRange[count].GetComponentInParent<GClass0>();
				}
				if (gClass == null)
				{
					gClass = rocsInRange[count].GetComponentInChildren<GClass0>();
				}
				if (gClass == null)
				{
					Debug.Log("Could not find ROC component for " + rocsInRange[count].name);
					continue;
				}
				zero = ((!useRaycast) ? gClass.GetClosestScanPosition(panelRotationTransform.position, out bestDistanceToPoint) : gClass.GetClosestScanPositionWithRaycasts(panelRotationTransform.position, out bestDistanceToPoint));
				if (num > bestDistanceToPoint)
				{
					result = gClass;
					closestScanPoint = zero;
					num = bestDistanceToPoint;
				}
			}
			else
			{
				rocsInRange.RemoveAt(count);
			}
		}
		return result;
	}

	public static bool RayCastToROC(Vector3 origin, Vector3 traceDirection, float traceDistance, ref RaycastHit hit, bool performSphereCast, float radiusForSphereCast = 0.35f)
	{
		int num = 0;
		num = ((!performSphereCast) ? Physics.RaycastNonAlloc(origin, traceDirection.normalized, hits, traceDistance, 32768, QueryTriggerInteraction.Ignore) : Physics.SphereCastNonAlloc(origin, radiusForSphereCast, traceDirection.normalized, hits, traceDistance, 32768, QueryTriggerInteraction.Ignore));
		bool result = false;
		for (int i = 0; i < num; i++)
		{
			if (hits[i].collider.gameObject.CompareTag("ROC"))
			{
				hit = hits[i];
				if (hit.point == Vector3.zero)
				{
					hit.point = hit.collider.ClosestPoint(origin);
				}
				result = true;
				break;
			}
		}
		return result;
	}

	public void CalculateExtendedTargetRotations()
	{
		Vector3 vector = scanPosition - panelRotationTransform.position;
		bool flag = false;
		float num = Mathf.Abs(firstArmLength - secondArmLength);
		if (vector.magnitude < num)
		{
			vector = instTargetPos - panelRotationTransform.position;
			if (Vector3.Dot(panelRotationTransform.forward, vector.normalized) < 0f)
			{
				flag = true;
			}
			float num2 = Vector3.SignedAngle(Vector3.ProjectOnPlane(instTargetPos - panelRotationTransform.position, panelRotationTransform.up), panelRotationTransform.forward, -panelRotationTransform.up);
			float num3 = 180f - TrackingAlignmentOffset;
			if (flag)
			{
				num3 = TrackingAlignmentOffset;
			}
			baseExtTargetRot = originalRotation * Quaternion.Euler(0f, num3 + num2, 0f);
			firstJointExtTargetRot = firstJointOrigRot;
			secondJointExtTargetRot = secondJointOrigRot;
			Vector3 vector2 = Vector3.ProjectOnPlane(instTargetPos - panelRotationTransform.position, panelRotationTransform.up);
			float magnitude = Vector3.ProjectOnPlane(instCentreTransform.position - panelRotationTransform.position, panelRotationTransform.up).magnitude;
			float num4 = Mathf.Atan(vector2.magnitude / magnitude);
			num4 = num4 * 180f / (float)Math.PI;
			gimbalExtTargetRot = gimbalOrigRot * Quaternion.Euler(0f, 0f, num4 - gimbalAlignOffset);
			if (flag)
			{
				gimbalExtTargetRot = gimbalOrigRot * Quaternion.Euler(0f, 0f, gimbalAlignOffset - num4);
			}
			Vector3 vector3 = Vector3.Project(instCentreTransform.position - panelRotationTransform.position, panelRotationTransform.up);
			float num5 = Vector3.SignedAngle(instTargetPos - (panelRotationTransform.position + vector3), -panelRotationTransform.up, panelRotationTransform.right);
			instExtTargetRot = instOrigRot * Quaternion.Euler(instAlignOffset + num5, 0f, 0f);
			return;
		}
		if (Vector3.Dot(panelRotationTransform.forward, vector.normalized) < 0f)
		{
			flag = true;
		}
		float num6 = Vector3.SignedAngle(Vector3.ProjectOnPlane(scanPosition - panelRotationTransform.position, panelRotationTransform.up), panelRotationTransform.forward, -panelRotationTransform.up);
		Vector3 vector4 = Vector3.ProjectOnPlane(instCentreTransform.position - panelRotationTransform.position, panelRotationTransform.up);
		Vector3 vector5 = Vector3.ProjectOnPlane(instTargetPos - panelRotationTransform.position, panelRotationTransform.up);
		float magnitude2 = vector4.magnitude;
		float num7 = 90f - Mathf.Atan(vector5.magnitude / magnitude2) * 57.29578f;
		if (flag)
		{
			num7 *= -1f;
		}
		num6 += num7;
		float num8 = 180f - TrackingAlignmentOffset;
		if (flag)
		{
			num8 = TrackingAlignmentOffset;
		}
		baseExtTargetRot = originalRotation * Quaternion.Euler(0f, num8 + num6, 0f);
		Vector3 vector6 = scanPosition - firstJointTransform.position;
		float num9 = firstArmLength * firstArmLength;
		float num10 = secondArmLength * secondArmLength;
		float num11 = Mathf.Min(vector6.magnitude, maxArmLength);
		float num12 = num11 * num11;
		float num13 = Mathf.Acos((num9 + num12 - num10) / (2f * firstArmLength * num11)) * 57.29578f;
		float num14 = Mathf.Acos((num10 + num12 - num9) / (2f * secondArmLength * num11)) * 57.29578f;
		float num15 = 180f - num13 - num14;
		Vector3 vector7 = scanPosition - firstJointTransform.position;
		Vector3 normalized = Vector3.Cross(vector7, -firstJointTransform.forward).normalized;
		Vector3 to = Vector3.ProjectOnPlane(vector7, -firstJointTransform.forward);
		float num16 = Vector3.SignedAngle(vector7, to, -normalized);
		float num17 = firstJointAlignOffset + num16 + num13;
		if (flag)
		{
			num17 = 180f + firstJointAlignOffset - num16 - num13;
		}
		firstJointExtTargetRot = firstJointOrigRot * Quaternion.Euler(num17, 0f, 0f);
		secondJointExtTargetRot = secondJointOrigRot * Quaternion.Euler(secondJointAlignOffset + num15, 0f, 0f);
		if (flag)
		{
			secondJointExtTargetRot = secondJointOrigRot * Quaternion.Euler(secondJointAlignOffset - num15, 0f, 0f);
		}
		Vector3 vector8 = Quaternion.AngleAxis(num13, normalized) * ((scanPosition - firstJointTransform.position).normalized * firstArmLength);
		Vector3 to2 = scanPosition - (firstJointTransform.position + vector8);
		float num18 = Vector3.SignedAngle(instTargetPos - scanPosition, to2, normalized);
		instExtTargetRot = instOrigRot * Quaternion.Euler(instAlignOffset - num18, 0f, 0f);
		if (flag)
		{
			instExtTargetRot = instOrigRot * Quaternion.Euler(instAlignOffset + num18, 0f, 0f);
		}
		gimbalExtTargetRot = gimbalOrigRot;
		startGoingBackwards = num17 < 0f;
	}

	public override void DoRetract()
	{
		if (deployState != ArmDeployState.UNPACKING && deployState != ArmDeployState.EXTENDING && deployState != ArmDeployState.SCANNING)
		{
			return;
		}
		trackingTransformLocal = null;
		if (deployState == ArmDeployState.UNPACKING)
		{
			anim[unpackAnimationName].speed = 0f - TimeWarp.CurrentRate;
			playingAnimation = unpackAnimationName;
			base.part.Effect(unpackEffectName, 0f);
			base.part.Effect(packEffectName, 1f);
			if (packAudioFX == null)
			{
				AudioFX[] components = base.part.GetComponents<AudioFX>();
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i].effectName == packEffectName)
					{
						packAudioFX = components[i];
						break;
					}
				}
			}
			if (packAudioFX != null)
			{
				packAudioFX.SetTime(anim[unpackAnimationName].length - anim[unpackAnimationName].time);
			}
			deployState = ArmDeployState.PACKING;
		}
		else if (deployState == ArmDeployState.EXTENDING)
		{
			base.part.Effect(extendEffectName, 0f);
			base.part.Effect(retractEffectName, 1f);
			deployState = ArmDeployState.RETRACTING;
		}
		else if (deployState == ArmDeployState.SCANNING)
		{
			if (anim.IsPlaying(animationName))
			{
				emergencyRetractScanner = true;
			}
			base.part.Effect(scanEffectName, 0f);
			base.part.Effect(retractEffectName, 1f);
			for (int j = 0; j < scannerEffectList.Count; j++)
			{
				scannerEffectList[j].OnEffectStop();
			}
			deployState = ArmDeployState.RETRACTING;
		}
		base.Events["Retract"].active = false;
		onMove.Fire(1f, 0f);
	}

	public override void startFSM()
	{
		base.Events["Retract"].active = false;
	}

	public override void updateFSM()
	{
		switch (deployState)
		{
		case ArmDeployState.RETRACTED:
			status = ModuleDeployablePart.cacheAutoLOC_234861;
			break;
		case ArmDeployState.UNPACKING:
			UpdateAnimationTime(unpackAnimationName);
			UnpackArm();
			status = ModuleDeployablePart.cacheAutoLOC_234841;
			PerformScanROCNullCheck();
			break;
		case ArmDeployState.EXTENDING:
			ExtendArm();
			status = ModuleDeployablePart.cacheAutoLOC_234841;
			PerformScanROCNullCheck();
			break;
		case ArmDeployState.SCANNING:
			if (!anim.IsPlaying(animationName))
			{
				for (int i = 0; i < scannerEffectList.Count; i++)
				{
					scannerEffectList[i].OnEffectStop();
				}
				PerformExperiment();
				deployState = ArmDeployState.RETRACTING;
				base.part.Effect(retractEffectName, 1f);
			}
			else
			{
				if ((float)resHandler.UpdateModuleResourceInputs(ref status, 1.0, 0.99, returnOnFirstLack: true, average: false, stringOps: true) < 0.99f)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004431", partType, status), 5f, ScreenMessageStyle.UPPER_LEFT);
					Retract();
				}
				UpdateAnimationTime(animationName);
				for (int j = 0; j < scannerEffectList.Count; j++)
				{
					if (scannerEffectList[j].IsReady)
					{
						scannerEffectList[j].OnUpdate(anim[animationName].time, distanceFromSurface, instTargetPos);
					}
				}
			}
			status = cacheAutoLOC_8004274;
			PerformScanROCNullCheck();
			break;
		case ArmDeployState.RETRACTING:
			ReturnArmToPrePackState();
			status = ModuleDeployablePart.cacheAutoLOC_234856;
			break;
		case ArmDeployState.PACKING:
			UpdateAnimationTime(unpackAnimationName);
			PackArm();
			status = ModuleDeployablePart.cacheAutoLOC_234856;
			break;
		case ArmDeployState.BROKEN:
			if (hasPivot && firstJointTransform != null)
			{
				firstJointTransform.gameObject.SetActive(value: false);
			}
			status = ModuleDeployablePart.cacheAutoLOC_234868;
			break;
		case ArmDeployState.PREVIEWRANGE:
			if (rangeTriggerMaterial != null && rangeTriggerRenderer != null)
			{
				rangeTriggerMaterial.SetVector("_SphereCentre", rangeTriggerTransform.position);
				rangeTriggerMaterial.SetVector("_SphereUp", rangeTriggerTransform.up);
				rangeTriggerMaterial.SetFloat("_Radius", rangeTriggerRadius);
				rangeTriggerRenderer.material = rangeTriggerMaterial;
			}
			if (!anim.IsPlaying(editorReachAnimationName))
			{
				base.Events["PlayEditorAnimation"].guiActiveEditor = true;
				deployState = ArmDeployState.RETRACTED;
				if (rangeTriggerTransform != null)
				{
					rangeTriggerTransform.gameObject.SetActive(value: false);
				}
			}
			status = cacheAutoLOC_8004432;
			break;
		}
		PostFSMUpdate();
	}

	public virtual void PerformScanROCNullCheck()
	{
		if (scanROC == null)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004428", partType, scanROCDisplayName), 5f, ScreenMessageStyle.UPPER_LEFT);
			Retract();
		}
	}

	public virtual void UpdateAnimationTime(string playingAnimationName)
	{
		if (Time.fixedUnscaledDeltaTime > Time.fixedDeltaTime && anim[playingAnimationName] != null)
		{
			if (anim[playingAnimationName].speed > 0f)
			{
				anim[playingAnimationName].time = (Time.unscaledTime - animationStartTime) / anim[playingAnimationName].speed;
			}
			else if (anim[playingAnimationName].speed < 0f)
			{
				anim[playingAnimationName].time = anim[playingAnimationName].length + (Time.unscaledTime - animationStartTime) / anim[playingAnimationName].speed;
			}
		}
	}

	public virtual void PlayUnpackAnimation()
	{
		anim[unpackAnimationName].normalizedTime = 0f;
		anim[unpackAnimationName].enabled = true;
		anim[unpackAnimationName].weight = 1f;
		anim[unpackAnimationName].speed = TimeWarp.CurrentRate;
		playingAnimation = unpackAnimationName;
		anim.Play(unpackAnimationName);
		base.part.Effect(unpackEffectName, 1f);
		animationStartTime = Time.unscaledTime;
	}

	public virtual void UnpackArm()
	{
		if (!anim.IsPlaying(unpackAnimationName))
		{
			deployState = ArmDeployState.EXTENDING;
			originalRotation = panelRotationTransform.localRotation;
			if (firstJointTransform != null)
			{
				firstJointOrigRot = firstJointTransform.localRotation;
			}
			if (secondJointTransform != null)
			{
				secondJointOrigRot = secondJointTransform.localRotation;
			}
			if (gimbalTransform != null)
			{
				gimbalOrigRot = gimbalTransform.localRotation;
			}
			if (instTransform != null)
			{
				instOrigRot = instTransform.localRotation;
			}
			if (instCentreTransform != null)
			{
				instCentreOrigRot = instCentreTransform.localRotation;
			}
			base.part.Effect(extendEffectName, 1f);
			CalculateExtendedTargetRotations();
			extendingUpwards = true;
		}
	}

	public virtual void ExtendArm()
	{
		panelRotationTransform.localRotation = Quaternion.RotateTowards(panelRotationTransform.localRotation, baseExtTargetRot, TimeWarp.deltaTime * trackingSpeed);
		gimbalTransform.localRotation = Quaternion.RotateTowards(gimbalTransform.localRotation, gimbalExtTargetRot, TimeWarp.deltaTime * trackingSpeed);
		instTransform.localRotation = Quaternion.RotateTowards(instTransform.localRotation, instExtTargetRot, TimeWarp.deltaTime * trackingSpeed);
		bool flag = false;
		if (Mathf.Abs(Quaternion.Angle(panelRotationTransform.localRotation, baseExtTargetRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(gimbalTransform.localRotation, gimbalExtTargetRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(instTransform.localRotation, instExtTargetRot)) <= 0.1f)
		{
			Quaternion quaternion = Quaternion.RotateTowards(firstJointTransform.localRotation, firstJointExtTargetRot, TimeWarp.deltaTime * trackingSpeed);
			float angle = Quaternion.Angle(firstJointTransform.localRotation, quaternion);
			Vector3 vector = Quaternion.AngleAxis(angle, -firstJointTransform.right) * -firstJointTransform.forward;
			if (!startGoingBackwards)
			{
				vector = Quaternion.AngleAxis(angle, firstJointTransform.right) * -firstJointTransform.forward;
			}
			if (extendingUpwards)
			{
				if (Vector3.Dot(vector.normalized, -base.vessel.graviticAcceleration) > Vector3.Dot(-firstJointTransform.forward, -base.vessel.graviticAcceleration))
				{
					firstJointTransform.localRotation = quaternion;
					if (Mathf.Abs(Quaternion.Angle(firstJointTransform.localRotation, firstJointExtTargetRot)) <= 0.1f)
					{
						extendingUpwards = false;
					}
				}
				else
				{
					extendingUpwards = false;
				}
			}
			else
			{
				if (Mathf.Abs(Quaternion.Angle(firstJointTransform.localRotation, firstJointExtTargetRot)) > Mathf.Abs(Quaternion.Angle(secondJointTransform.localRotation, secondJointExtTargetRot)) + firstJointRotStartAngleModifier || Vector3.Dot(vector.normalized, -base.vessel.graviticAcceleration) > Vector3.Dot(-firstJointTransform.forward, -base.vessel.graviticAcceleration))
				{
					firstJointTransform.localRotation = quaternion;
				}
				secondJointTransform.localRotation = Quaternion.RotateTowards(secondJointTransform.localRotation, secondJointExtTargetRot, TimeWarp.deltaTime * trackingSpeed);
			}
			if (Mathf.Abs(Quaternion.Angle(panelRotationTransform.localRotation, baseExtTargetRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(firstJointTransform.localRotation, firstJointExtTargetRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(secondJointTransform.localRotation, secondJointExtTargetRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(gimbalTransform.localRotation, gimbalExtTargetRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(instTransform.localRotation, instExtTargetRot)) <= 0.1f)
			{
				flag = true;
			}
			if (!flag)
			{
				Vector3 traceDirection = instTargetPos - instCentreTransform.position;
				if (RayCastToROC(instCentreTransform.position, traceDirection, traceDirection.magnitude, ref hitInfo, performSphereCast: true, sphereCastRadius) && (hitInfo.point - instCentreTransform.position).magnitude < emergencyStopDistanceFromSurface)
				{
					instTargetPos = hitInfo.point;
					flag = true;
				}
			}
		}
		if (flag)
		{
			deployState = ArmDeployState.SCANNING;
			PlayScanAnimation();
		}
		Vector3 normalized = (scanPosition - panelRotationTransform.position).normalized;
		PostCalculateTracking(trackingLOS: true, normalized);
	}

	public virtual void PlayScanAnimation()
	{
		anim[animationName].normalizedTime = 0f;
		anim[animationName].enabled = true;
		anim[animationName].weight = 1f;
		anim[animationName].speed = TimeWarp.CurrentRate;
		playingAnimation = animationName;
		anim.Play(animationName);
		base.part.Effect(extendEffectName, 0f);
		base.part.Effect(scanEffectName, 1f);
		for (int i = 0; i < scannerEffectList.Count; i++)
		{
			if (scannerEffectList[i].IsReady)
			{
				scannerEffectList[i].OnEffectStart();
			}
		}
		animationStartTime = Time.unscaledTime;
	}

	public virtual void PerformExperiment()
	{
		if (scanROC != null)
		{
			scanROC.PerformExperiment(moduleScienceExperiment);
		}
	}

	public virtual void ReturnArmToPrePackState()
	{
		if (emergencyRetractScanner)
		{
			for (int i = 0; i < safeRetractPeriods.Count; i++)
			{
				if (anim[animationName].time > safeRetractPeriods[i].x && anim[animationName].time < safeRetractPeriods[i].y)
				{
					emergencyRetractScanner = false;
					anim.Stop(animationName);
				}
			}
		}
		firstJointTransform.localRotation = Quaternion.RotateTowards(firstJointTransform.localRotation, firstJointOrigRot, TimeWarp.deltaTime * trackingSpeed);
		secondJointTransform.localRotation = Quaternion.RotateTowards(secondJointTransform.localRotation, secondJointOrigRot, TimeWarp.deltaTime * trackingSpeed);
		gimbalTransform.localRotation = Quaternion.RotateTowards(gimbalTransform.localRotation, gimbalOrigRot, TimeWarp.deltaTime * trackingSpeed);
		instTransform.localRotation = Quaternion.RotateTowards(instTransform.localRotation, instOrigRot, TimeWarp.deltaTime * trackingSpeed);
		instCentreTransform.localRotation = Quaternion.RotateTowards(instCentreTransform.localRotation, instCentreOrigRot, TimeWarp.deltaTime * trackingSpeed);
		if (Mathf.Abs(Quaternion.Angle(firstJointTransform.localRotation, firstJointOrigRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(secondJointTransform.localRotation, secondJointOrigRot)) <= 0.1f)
		{
			panelRotationTransform.localRotation = Quaternion.RotateTowards(panelRotationTransform.localRotation, originalRotation, TimeWarp.deltaTime * trackingSpeed);
		}
		if (Mathf.Abs(Quaternion.Angle(panelRotationTransform.localRotation, originalRotation)) <= 0.1f && Mathf.Abs(Quaternion.Angle(firstJointTransform.localRotation, firstJointOrigRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(secondJointTransform.localRotation, secondJointOrigRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(gimbalTransform.localRotation, gimbalOrigRot)) <= 0.1f && Mathf.Abs(Quaternion.Angle(instTransform.localRotation, instOrigRot)) <= 0.1f && !emergencyRetractScanner && Mathf.Abs(Quaternion.Angle(instCentreTransform.localRotation, instCentreOrigRot)) <= 0.1f)
		{
			deployState = ArmDeployState.PACKING;
			PlayPackAnimation();
		}
	}

	public virtual void PlayPackAnimation()
	{
		anim[unpackAnimationName].normalizedTime = 1f;
		anim[unpackAnimationName].enabled = true;
		anim[unpackAnimationName].weight = 1f;
		anim[unpackAnimationName].speed = 0f - TimeWarp.CurrentRate;
		playingAnimation = unpackAnimationName;
		anim.Play(unpackAnimationName);
		base.part.Effect(retractEffectName, 0f);
		base.part.Effect(packEffectName, 1f);
		animationStartTime = Time.unscaledTime;
	}

	public virtual void PackArm()
	{
		if (!anim.IsPlaying(unpackAnimationName))
		{
			deployState = ArmDeployState.RETRACTED;
		}
	}

	public override void breakPanels()
	{
		base.breakPanels();
		if (base.deployState == DeployState.BROKEN)
		{
			deployState = ArmDeployState.BROKEN;
		}
	}

	public override bool DoRepair()
	{
		bool flag;
		if (flag = base.DoRepair())
		{
			ParticleSystem[] componentsInChildren = panelBreakTransform.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Stop();
				ParticleSystem.MainModule main = componentsInChildren[i].main;
				main.playOnAwake = false;
			}
			KSPParticleEmitter[] componentsInChildren2 = panelBreakTransform.GetComponentsInChildren<KSPParticleEmitter>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].emit = false;
			}
			Light[] componentsInChildren3 = panelBreakTransform.GetComponentsInChildren<Light>(includeInactive: true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].enabled = false;
			}
			deployState = ArmDeployState.RETRACTED;
			SetupScannerTransforms();
			SetupScannerEffectList();
		}
		deployState = ((!flag) ? ArmDeployState.BROKEN : ArmDeployState.RETRACTED);
		return flag;
	}

	public virtual bool CanScan()
	{
		if (base.vessel != null)
		{
			return base.vessel.srfSpeed < 0.10000000149011612;
		}
		return false;
	}

	public override string GetInfo()
	{
		string text = Localizer.Format("#autoLOC_234569", trackingSpeed.ToString("0.0###"));
		if (isBreakable)
		{
			text += Localizer.Format("#autoLOC_234572", windResistance.ToString("0.0###"));
			if (gResistance < double.PositiveInfinity)
			{
				text = text + ", " + gResistance.ToString("F0") + "G";
			}
		}
		text += Localizer.Format("#autoLOC_8004425", rangeTriggerRadius.ToString("0.0###"));
		if (resHandler.inputResources.Count != 0)
		{
			text += resHandler.PrintModuleResources();
		}
		return text;
	}

	public virtual void OnPause()
	{
		pauseStartTime = Time.unscaledTime;
		if (!string.IsNullOrEmpty(playingAnimation))
		{
			paused_anim_speed = anim[playingAnimation].speed;
			anim[playingAnimation].speed = 0f;
		}
	}

	public virtual void OnUnpause()
	{
		animationStartTime += Time.unscaledTime - pauseStartTime;
		if (!string.IsNullOrEmpty(playingAnimation))
		{
			float f = paused_anim_speed;
			anim[playingAnimation].speed = Mathf.Sign(f) * TimeWarp.CurrentRate;
		}
	}

	public virtual void OnTimeWarpRateChanged()
	{
		if (!string.IsNullOrEmpty(playingAnimation))
		{
			float speed = anim[playingAnimation].speed;
			anim[playingAnimation].speed = Mathf.Sign(speed) * TimeWarp.CurrentRate;
		}
	}

	public void OnCurrentMousePartChanged(Part p)
	{
		bool flag = p != null;
		if (HighLogic.LoadedSceneIsFlight && EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.IsOpen && EVAConstructionModeController.Instance.panelMode == EVAConstructionModeController.PanelMode.Construction)
		{
			if (rangeTriggerTransform == null)
			{
				rangeTriggerTransform = base.part.FindModelTransform(rangeTriggerColliderTransformName);
			}
			if (rangeTriggerTransform != null)
			{
				rangeTriggerTransform.gameObject.SetActive(!flag);
			}
		}
	}

	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8004430")]
	public void PlayEditorAnimation()
	{
		deployState = ArmDeployState.PREVIEWRANGE;
		base.Events["PlayEditorAnimation"].guiActiveEditor = false;
		anim[editorReachAnimationName].normalizedTime = 0f;
		anim[editorReachAnimationName].enabled = true;
		anim[editorReachAnimationName].weight = 1f;
		anim[editorReachAnimationName].speed = 1f;
		anim.Play(editorReachAnimationName);
		if (rangeTriggerTransform == null)
		{
			Transform parent = base.part.FindModelTransform(rangeTriggerParentTransformName);
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			float num = rangeTriggerRadius * 2f;
			gameObject.transform.localScale = new Vector3(num, num, num);
			gameObject.name = rangeTriggerColliderTransformName;
			gameObject.transform.position = panelRotationTransform.position;
			gameObject.transform.rotation = panelRotationTransform.rotation;
			gameObject.transform.SetParent(parent);
			SphereCollider component = gameObject.GetComponent<SphereCollider>();
			if (component != null)
			{
				component.enabled = false;
			}
			rangeTriggerRenderer = gameObject.GetComponent<MeshRenderer>();
			if (rangeTriggerRenderer != null)
			{
				rangeTriggerMaterial = new Material(Shader.Find("KSP/Alpha/Alpha Half Sphere"));
				rangeTriggerRenderer.material = rangeTriggerMaterial;
			}
			rangeTriggerTransform = gameObject.transform;
		}
		else
		{
			rangeTriggerTransform.gameObject.SetActive(value: true);
		}
	}

	public new void CacheLocalStrings()
	{
		cacheAutoLOC_8004426 = Localizer.Format("#autoLOC_8004426", rangeTriggerRadius);
		if (cacheAutoLOC_8004274 == null)
		{
			cacheAutoLOC_8004274 = Localizer.Format("#autoLOC_8004274");
			cacheAutoLOC_8004427 = Localizer.Format("#autoLOC_8004427");
			cacheAutoLOC_8004432 = Localizer.Format("#autoLOC_8004432");
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004353");
	}
}
