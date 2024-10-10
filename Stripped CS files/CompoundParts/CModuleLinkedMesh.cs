using UnityEngine;

namespace CompoundParts;

public class CModuleLinkedMesh : CompoundPartModule, ICMTweakTarget
{
	[KSPField]
	public string lineObjName = "obj_line";

	[KSPField]
	public string mainAnchorName = "obj_mainAnchor";

	[KSPField]
	public string targetAnchorName = "obj_targetAnchor";

	[KSPField]
	public string anchorCapName = "obj_anchorCap";

	[KSPField]
	public string targetCapName = "obj_targetCap";

	[KSPField]
	public string targetColliderName = "obj_targetCollider";

	public Transform line;

	public Transform mainAnchor;

	public Transform targetAnchor;

	public Transform startCap;

	public Transform endCap;

	public Transform targetCollider;

	public bool activeInPreview;

	public bool tweakingTarget;

	public bool targetIsRobotic;

	public bool setupFinished;

	public float lineMinimumLength = 0.01f;

	public bool TweakingTarget
	{
		get
		{
			return tweakingTarget;
		}
		set
		{
			tweakingTarget = value;
		}
	}

	public Transform GetReferenceTransform()
	{
		if ((tweakingTarget || base.part.PartTweakerSelected) && targetCollider != null && targetAnchor != null)
		{
			return targetAnchor;
		}
		return base.part.partTransform;
	}

	public bool SetSymmetryValues(Vector3 newPosition, Quaternion newRotation)
	{
		if (tweakingTarget)
		{
			targetAnchor.transform.position = newPosition;
			targetAnchor.transform.rotation = newRotation;
		}
		return tweakingTarget;
	}

	public Collider[] GetSelectedColliders()
	{
		if (tweakingTarget)
		{
			return targetAnchor.GetComponentsInChildren<Collider>();
		}
		return mainAnchor.GetComponentsInChildren<Collider>();
	}

	public void SelectTweakTarget(Vector3 mousePosition)
	{
		Camera camera = (HighLogic.LoadedSceneIsEditor ? EditorLogic.fetch.editorCamera : FlightCamera.fetch.mainCamera);
		float num = Vector3.Distance(mousePosition, camera.WorldToScreenPoint(base.transform.position));
		float num2 = Vector3.Distance(mousePosition, camera.WorldToScreenPoint(targetAnchor.position));
		TweakingTarget = num2 < num;
	}

	public override void OnStart(StartState state)
	{
		line = base.part.FindModelTransform(lineObjName);
		mainAnchor = base.part.FindModelTransform(mainAnchorName);
		if (targetAnchor == null)
		{
			targetAnchor = base.part.FindModelTransform(targetAnchorName);
		}
		startCap = base.part.FindModelTransform(anchorCapName);
		endCap = base.part.FindModelTransform(targetCapName);
		targetCollider = base.part.FindModelTransform(targetColliderName);
		if (!line)
		{
			Debug.LogError(("[CompoundPart]: Cannot find line object called " + lineObjName) ?? "");
			return;
		}
		if (!targetAnchor)
		{
			Debug.LogError(("[CompoundPart]: Cannot find targetAnchor object called " + targetAnchorName) ?? "");
			return;
		}
		line.gameObject.SetActive(value: false);
		targetAnchor.gameObject.SetActive(value: false);
		SetTargetPointer();
		if ((bool)startCap)
		{
			startCap.gameObject.SetActive(value: true);
		}
		if ((bool)targetCollider)
		{
			targetCollider.gameObject.SetActive(value: false);
		}
		tweakingTarget = false;
	}

	public override void OnTargetSet(Part target)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			Transform parent = target.transform;
			if (!string.IsNullOrEmpty(base.compoundPart.targetMeshColName))
			{
				target.isRobotic(out var servo);
				if (servo != null && servo.ServoTransformCollider(base.compoundPart.targetMeshColName) && (bool)servo.MovingObject())
				{
					parent = servo.MovingObject().transform;
				}
			}
			targetAnchor.parent = parent;
			targetAnchor.gameObject.SetLayerRecursive(0);
			SetTargetPointer();
		}
		if ((bool)endCap)
		{
			endCap.gameObject.SetActive(value: true);
		}
		if ((bool)startCap)
		{
			startCap.gameObject.SetActive(value: true);
		}
		if ((bool)targetCollider)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				targetCollider.gameObject.SetActive(value: true);
			}
			if (HighLogic.LoadedSceneIsFlight)
			{
				targetCollider.gameObject.SetActive(value: true);
				if (base.vessel != null)
				{
					CollisionManager.SetCollidersOnVessel(base.vessel, true, targetCollider.GetComponent<Collider>());
				}
				if (target != null && target.vessel != null)
				{
					CollisionManager.SetCollidersOnVessel(target.vessel, true, targetCollider.GetComponent<Collider>());
				}
			}
		}
		line.gameObject.SetActive(value: true);
		targetAnchor.gameObject.SetActive(value: true);
		TrackAnchor(setTgtAnchor: true, base.compoundPart.direction, base.compoundPart.targetPosition, base.compoundPart.targetRotation);
	}

	public override void OnTargetLost()
	{
		if (targetAnchor != null)
		{
			targetAnchor.gameObject.SetActive(value: false);
		}
		if (line != null)
		{
			line.gameObject.SetActive(value: false);
		}
		if (startCap != null && HighLogic.LoadedSceneIsFlight)
		{
			startCap.gameObject.SetActive(value: false);
		}
		if (endCap != null)
		{
			endCap.gameObject.SetActive(value: false);
		}
		if (targetCollider != null)
		{
			targetCollider.gameObject.SetActive(value: false);
			if (HighLogic.LoadedSceneIsEditor && EditorLogic.fetch != null)
			{
				EditorLogic.fetch.selPartGrabOffset = Vector3.zero;
			}
		}
	}

	public override void OnPreviewAttachment(Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
		activeInPreview = rDir != Vector3.zero && rPos.sqrMagnitude <= base.compoundPart.maxLength * base.compoundPart.maxLength;
		line.gameObject.SetActive(activeInPreview);
		if (targetAnchor != null)
		{
			targetAnchor.gameObject.SetActive(activeInPreview);
		}
		if (endCap != null)
		{
			endCap.gameObject.SetActive(activeInPreview);
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			TrackAnchor(HighLogic.LoadedSceneIsFlight, rDir, rPos, rRot);
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			TrackAnchor(HighLogic.LoadedSceneIsEditor, rDir, rPos, rRot);
		}
	}

	public override void OnPreviewEnd()
	{
		OnTargetLost();
	}

	public override void OnTargetUpdate()
	{
		TrackAnchor(HighLogic.LoadedSceneIsEditor, base.compoundPart.direction, base.compoundPart.targetPosition, base.compoundPart.targetRotation);
	}

	public void TrackAnchor(bool setTgtAnchor, Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
		if (targetAnchor != null)
		{
			if (!tweakingTarget && !base.part.PartTweakerSelected)
			{
				if (setTgtAnchor && base.compoundPart != null && base.compoundPart.transform != null)
				{
					targetAnchor.position = base.compoundPart.transform.TransformPoint(rPos);
					targetAnchor.rotation = base.compoundPart.transform.rotation * rRot;
				}
			}
			else
			{
				base.compoundPart.targetPosition = base.transform.InverseTransformPoint(targetAnchor.position);
				base.compoundPart.targetRotation = targetAnchor.localRotation;
				base.compoundPart.UpdateWorldValues();
			}
		}
		if (endCap != null)
		{
			Vector3 vector = line.position - endCap.position;
			if (vector != Vector3.zero)
			{
				if (vector.magnitude < lineMinimumLength)
				{
					line.gameObject.SetActive(value: false);
				}
				else
				{
					line.gameObject.SetActive(value: true);
					float magnitude = vector.magnitude;
					line.rotation = Quaternion.LookRotation(vector / magnitude, base.transform.forward);
					line.localScale = new Vector3(line.localScale.x, line.localScale.y, magnitude * base.part.scaleFactor);
					endCap.rotation = line.rotation;
				}
			}
		}
		else if (base.transform != null && targetAnchor != null)
		{
			Vector3 vector2 = base.transform.position - targetAnchor.position;
			if (vector2 != Vector3.zero)
			{
				if (vector2.magnitude < lineMinimumLength)
				{
					line.gameObject.SetActive(value: false);
				}
				else
				{
					line.gameObject.SetActive(value: true);
					float magnitude2 = vector2.magnitude;
					if (float.IsNaN(magnitude2) || float.IsInfinity(magnitude2))
					{
						Debug.LogError(string.Concat("[CModuleLinkedMesh]: Object ", base.name, ": Look vector magnitude invalid. Vector is (", vector2.x, ", ", vector2.y, ", ", vector2.z, "). Transform ", base.transform.position.IsInvalid() ? "invalid" : "valid", " ", base.transform.position, ", target ", targetAnchor.position.IsInvalid() ? "invalid" : "valid", ", ", targetAnchor.position));
						return;
					}
					line.rotation = Quaternion.LookRotation(vector2 / magnitude2, base.transform.forward);
					line.localScale = new Vector3(line.localScale.x, line.localScale.y, magnitude2 * base.part.scaleFactor);
				}
			}
		}
		if (startCap != null)
		{
			startCap.rotation = line.rotation;
		}
	}

	public void SetTargetPointer()
	{
		if (targetAnchor != null)
		{
			PartPointer partPointer = targetAnchor.GetComponent<PartPointer>();
			if (partPointer == null)
			{
				partPointer = targetAnchor.gameObject.AddComponent<PartPointer>();
			}
			partPointer.SetPart(base.part);
		}
	}
}
