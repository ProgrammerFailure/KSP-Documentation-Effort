using System;
using System.Collections.Generic;
using Expansions;
using UnityEngine;

public class GClass0 : MonoBehaviour
{
	public class ScanPoint : IComparable<ScanPoint>
	{
		public Vector3 point;

		public Vector3 traceDirection;

		public float distance;

		public int CompareTo(ScanPoint otherPoint)
		{
			if (distance < otherPoint.distance)
			{
				return -1;
			}
			if (distance > otherPoint.distance)
			{
				return 1;
			}
			return 0;
		}
	}

	public string type;

	public string displayName;

	public string prefabName;

	public string modelName;

	public Transform modelTransform;

	public bool orientateup;

	public float depth;

	public bool canbetaken;

	public float frequency;

	public List<RocCBDefinition> myCelestialBodies;

	public bool castShadows;

	public bool recieveShadows;

	public float collisionThreshold;

	public bool smallROC;

	public int rocID;

	public bool randomDepth;

	public bool randomOrientation;

	public bool randomRotation;

	public List<Vector3> localSpaceScanPoints = new List<Vector3>();

	public float burstEmitterMinWait = 60f;

	public float burstEmitterMaxWait = 300f;

	public ROCEmitter rocEmitter;

	[SerializeField]
	public float scale = 1f;

	public float sfxVolume;

	public string idleClipPath;

	public string burstClipPath;

	public Vector3 upDirection;

	public float vfxBaseForce;

	public FloatCurve vfxCurveForce;

	public bool applyForces;

	public Vector2 vfxForceRadius;

	public Vector3 forceDirection;

	public Vector3 radiusCenter;

	public float rocArrowLength = 60f;

	public float rocArrowXZScale = 50f;

	public ArrowPointer rocArrow;

	[SerializeField]
	public Color rocArrowColor = Color.red;

	public float rocTransformLength = 5f;

	public float rocScanpointLength = 2f;

	public List<ArrowPointer> arrowScanPoints;

	[SerializeField]
	public Color rocTransformColor = Color.green;

	[SerializeField]
	public Color rocScanColor = Color.blue;

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			UnityEngine.Object.Destroy(this);
		}
		if (upDirection == Vector3.zero)
		{
			upDirection = base.transform.up;
		}
	}

	public void Start()
	{
		SetRocEmitters(base.gameObject.transform);
		if (HighLogic.LoadedSceneIsFlight)
		{
			ROCManager.Instance.AddROCStats_ROCs(type, 1, frequency);
		}
		if (ROCManager.Instance.debugROCFinder || ROCManager.Instance.debugROCScanPoints)
		{
			UpdateRocArrow();
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.OnDebugROCFinderToggled.Add(UpdateRocArrow);
			GameEvents.OnDebugROCScanPointsToggled.Add(UpdateRocArrow);
		}
	}

	public void SetRocEmitters(Transform objectTransform)
	{
		for (int i = 0; i < objectTransform.childCount; i++)
		{
			Transform child = objectTransform.GetChild(i);
			if (child != null)
			{
				rocEmitter = child.GetComponent<ROCEmitter>();
				if (rocEmitter != null)
				{
					rocEmitter.SetBurstEmitterMinWait(burstEmitterMinWait);
					rocEmitter.SetBurstEmitterMaxWait(burstEmitterMaxWait);
					rocEmitter.SetSFXPower(sfxVolume);
					rocEmitter.SetSFXClipPath(idleClipPath, burstClipPath);
				}
			}
			SetRocEmitters(child);
		}
	}

	public void SetStats(string type, string displayName, string prefabName, string modelName, bool orientateup, float depth, bool canbetaken, float frequency, List<RocCBDefinition> myCelestialBodies, bool castShadows, bool recieveShadows, float collisionthreshold, bool smallroc, bool randomdepth, bool randomorientation, List<Vector3> localSpaceScanPoints, float burstEmitterMinWait, float burstEmitterMaxWait, bool randomRotation, float scale, float sfxVolume, string idleClipPath, string burstClipPath, FloatCurve vfxCurveForce, float vfxBaseForce, bool applyForces, Vector2 vfxForceRadius, Vector3 forceDirection, Vector3 radiusCenter)
	{
		this.type = type;
		this.displayName = displayName;
		this.prefabName = prefabName;
		this.modelName = modelName;
		this.orientateup = orientateup;
		this.depth = depth;
		this.canbetaken = canbetaken;
		this.frequency = frequency;
		this.myCelestialBodies = myCelestialBodies;
		this.castShadows = castShadows;
		this.recieveShadows = recieveShadows;
		collisionThreshold = collisionthreshold;
		smallROC = smallroc;
		randomDepth = randomdepth;
		randomOrientation = randomorientation;
		this.localSpaceScanPoints = localSpaceScanPoints;
		this.burstEmitterMinWait = burstEmitterMinWait;
		this.burstEmitterMaxWait = burstEmitterMaxWait;
		this.randomRotation = randomRotation;
		this.sfxVolume = sfxVolume;
		this.idleClipPath = idleClipPath;
		this.burstClipPath = burstClipPath;
		this.scale = scale;
		this.vfxBaseForce = vfxBaseForce;
		this.vfxCurveForce = vfxCurveForce;
		this.applyForces = applyForces;
		this.vfxForceRadius = vfxForceRadius;
		this.forceDirection = forceDirection;
		this.radiusCenter = radiusCenter;
	}

	public void OnDestroy()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			ROCManager.Instance.SubtractROCStats_ROCs(type, 1);
		}
		GameEvents.OnDebugROCFinderToggled.Remove(UpdateRocArrow);
		GameEvents.OnDebugROCScanPointsToggled.Remove(UpdateRocArrow);
	}

	public Vector3 GetClosestScanPosition(Vector3 testPosition, out float bestDistanceToPoint)
	{
		UpdateModelTransform();
		Vector3 result = modelTransform.position;
		bestDistanceToPoint = float.MaxValue;
		Vector3 zero = Vector3.zero;
		bool flag = false;
		for (int i = 0; i < localSpaceScanPoints.Count; i++)
		{
			zero = modelTransform.TransformPoint(localSpaceScanPoints[i]);
			float magnitude = (zero - testPosition).magnitude;
			if (bestDistanceToPoint > magnitude)
			{
				result = zero;
				bestDistanceToPoint = magnitude;
				flag = true;
			}
		}
		if (!flag)
		{
			bestDistanceToPoint = (modelTransform.position - testPosition).magnitude;
		}
		return result;
	}

	public Vector3 GetClosestScanPositionWithRaycasts(Vector3 testPosition, out float distanceToPoint)
	{
		UpdateModelTransform();
		_ = modelTransform.position;
		distanceToPoint = float.MaxValue;
		Vector3 zero = Vector3.zero;
		List<ScanPoint> list = new List<ScanPoint>();
		for (int i = 0; i < localSpaceScanPoints.Count; i++)
		{
			zero = modelTransform.TransformPoint(localSpaceScanPoints[i]);
			Vector3 traceDirection = zero - testPosition;
			ScanPoint scanPoint = new ScanPoint();
			scanPoint.point = zero;
			scanPoint.traceDirection = traceDirection;
			scanPoint.distance = traceDirection.magnitude;
			list.Add(scanPoint);
		}
		list.Sort();
		RaycastHit hitInfo = default(RaycastHit);
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (Physics.Raycast(testPosition, list[num].traceDirection.normalized, out hitInfo, list[num].distance, 32768, QueryTriggerInteraction.Ignore) && hitInfo.collider.gameObject.CompareTag("ROC"))
				{
					break;
				}
				num++;
				continue;
			}
			return GetClosestScanPosition(testPosition, out distanceToPoint);
		}
		distanceToPoint = hitInfo.distance;
		return list[num].point;
	}

	public void CheckCollision(Vessel vessel)
	{
		if (vessel.speed > (double)collisionThreshold && smallROC)
		{
			if ((bool)ROCManager.Instance)
			{
				ROCManager.Instance.RemoveROC(rocID);
			}
			UpdateModelTransform();
			FXMonger.ROCExplode(modelTransform.position, 0.30000001192092896);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void PickUpROC()
	{
		if ((bool)ROCManager.Instance)
		{
			ROCManager.Instance.RemoveROC(rocID);
		}
		Collider componentInChildren = GetComponentInChildren<Collider>();
		List<Part> valuesList = FlightGlobals.PersistentLoadedPartIds.ValuesList;
		for (int i = 0; i < valuesList.Count; i++)
		{
			if (valuesList[i].currentCollisions.ContainsKey(componentInChildren))
			{
				valuesList[i].ResetCollisions();
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public bool PerformExperiment(ModuleScienceExperiment moduleScienceExperiment)
	{
		if (moduleScienceExperiment == null)
		{
			Debug.LogWarningFormat("[ROC]: Unable to run Experiment on ROC '{0}' ModuleScienceExperiment PartModule is not defined in this Parts cfg file.", type);
		}
		string experimentID = moduleScienceExperiment.experimentID + "_" + type;
		moduleScienceExperiment.experiment = ResearchAndDevelopment.GetExperiment(experimentID);
		if (moduleScienceExperiment.experiment != null)
		{
			moduleScienceExperiment.DeployExperiment();
			return true;
		}
		return false;
	}

	public virtual void UpdateModelTransform()
	{
		if (modelTransform == null)
		{
			modelTransform = base.transform.Find(modelName);
		}
	}

	public virtual void UpdateRocArrow()
	{
		if (ROCManager.Instance != null && ROCManager.Instance.debugROCFinder)
		{
			if (rocArrow == null)
			{
				rocArrow = ArrowPointer.Create(base.transform, Vector3.up * ((rocArrowLength + 20f) / scale), base.transform.up * -1f, rocArrowLength / scale, rocArrowColor, worldSpace: true);
				rocArrow.transform.localScale = new Vector3(rocArrowXZScale / scale, 1f, rocArrowXZScale / scale);
				Vector3 toDirection = base.transform.InverseTransformDirection(upDirection);
				rocArrow.transform.localRotation = Quaternion.FromToRotation(Vector3.up, toDirection);
				rocArrow.gameObject.name = "ArrowPointer-ROCME";
			}
		}
		else
		{
			DestroyRocFinderArrow();
		}
		if (ROCManager.Instance != null && ROCManager.Instance.debugROCScanPoints)
		{
			if (arrowScanPoints == null)
			{
				arrowScanPoints = new List<ArrowPointer>();
			}
			if (arrowScanPoints.Count < 1)
			{
				UpdateModelTransform();
				ArrowPointer arrowPointer = ArrowPointer.Create(modelTransform, Vector3.up * rocTransformLength, modelTransform.up * -1f, rocTransformLength, rocTransformColor, worldSpace: true);
				arrowPointer.gameObject.name = "TransformPointer-ROCME";
				arrowScanPoints.Add(arrowPointer);
				arrowPointer = ArrowPointer.Create(modelTransform, Vector3.right * rocTransformLength, modelTransform.right * -1f, rocTransformLength, rocTransformColor, worldSpace: true);
				arrowPointer.gameObject.name = "TransformPointer-ROCME";
				arrowScanPoints.Add(arrowPointer);
				arrowPointer = ArrowPointer.Create(modelTransform, Vector3.forward * rocTransformLength, modelTransform.forward * -1f, rocTransformLength, rocTransformColor, worldSpace: true);
				arrowPointer.gameObject.name = "TransformPointer-ROCME";
				arrowScanPoints.Add(arrowPointer);
				arrowPointer = ArrowPointer.Create(modelTransform, Vector3.down * rocTransformLength, modelTransform.up, rocTransformLength, rocTransformColor, worldSpace: true);
				arrowPointer.gameObject.name = "TransformPointer-ROCME";
				arrowScanPoints.Add(arrowPointer);
				arrowPointer = ArrowPointer.Create(modelTransform, Vector3.left * rocTransformLength, modelTransform.right, rocTransformLength, rocTransformColor, worldSpace: true);
				arrowPointer.gameObject.name = "TransformPointer-ROCME";
				arrowScanPoints.Add(arrowPointer);
				arrowPointer = ArrowPointer.Create(modelTransform, Vector3.back * rocTransformLength, modelTransform.forward, rocTransformLength, rocTransformColor, worldSpace: true);
				arrowPointer.gameObject.name = "TransformPointer-ROCME";
				arrowScanPoints.Add(arrowPointer);
				for (int i = 0; i < localSpaceScanPoints.Count; i++)
				{
					arrowPointer = ArrowPointer.Create(modelTransform, localSpaceScanPoints[i] + Vector3.up * rocScanpointLength, modelTransform.up * -1f, rocScanpointLength, rocScanColor, worldSpace: true);
					arrowPointer.gameObject.name = "ScanPointer-ROCME";
					arrowScanPoints.Add(arrowPointer);
					arrowPointer = ArrowPointer.Create(modelTransform, localSpaceScanPoints[i] + Vector3.right * rocScanpointLength, modelTransform.right * -1f, rocScanpointLength, rocScanColor, worldSpace: true);
					arrowPointer.gameObject.name = "ScanPointer-ROCME";
					arrowScanPoints.Add(arrowPointer);
					arrowPointer = ArrowPointer.Create(modelTransform, localSpaceScanPoints[i] + Vector3.forward * rocScanpointLength, modelTransform.forward * -1f, rocScanpointLength, rocScanColor, worldSpace: true);
					arrowPointer.gameObject.name = "ScanPointer-ROCME";
					arrowScanPoints.Add(arrowPointer);
					arrowPointer = ArrowPointer.Create(modelTransform, localSpaceScanPoints[i] + Vector3.down * rocScanpointLength, modelTransform.up, rocScanpointLength, rocScanColor, worldSpace: true);
					arrowPointer.gameObject.name = "ScanPointer-ROCME";
					arrowScanPoints.Add(arrowPointer);
					arrowPointer = ArrowPointer.Create(modelTransform, localSpaceScanPoints[i] + Vector3.left * rocScanpointLength, modelTransform.right, rocScanpointLength, rocScanColor, worldSpace: true);
					arrowPointer.gameObject.name = "ScanPointer-ROCME";
					arrowScanPoints.Add(arrowPointer);
					arrowPointer = ArrowPointer.Create(modelTransform, localSpaceScanPoints[i] + Vector3.back * rocScanpointLength, modelTransform.forward, rocScanpointLength, rocScanColor, worldSpace: true);
					arrowPointer.gameObject.name = "ScanPointer-ROCME";
					arrowScanPoints.Add(arrowPointer);
				}
			}
		}
		else
		{
			DestroyScanPointArrows();
		}
	}

	public void DestroyRocFinderArrow()
	{
		if ((object)rocArrow != null)
		{
			UnityEngine.Object.Destroy(rocArrow.gameObject);
			rocArrow = null;
		}
	}

	public void DestroyScanPointArrows()
	{
		if (arrowScanPoints != null)
		{
			int count = arrowScanPoints.Count;
			while (count-- > 0)
			{
				UnityEngine.Object.Destroy(arrowScanPoints[count].gameObject);
				arrowScanPoints[count] = null;
			}
			arrowScanPoints.Clear();
			arrowScanPoints = null;
		}
	}

	public float GetVFXForceScale(float burstClipTime)
	{
		return vfxCurveForce.Evaluate(burstClipTime);
	}
}
