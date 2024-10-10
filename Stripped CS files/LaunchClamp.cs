using System;
using System.Collections;
using UnityEngine;

public class LaunchClamp : PartModule, IStageSeparator
{
	[KSPField]
	public string releaseFxGroupName = "activate";

	[KSPField]
	public string trf_towerPivot_name;

	public Transform towerPivot;

	[KSPField]
	public string trf_towerStretch_name;

	public Transform towerStretch;

	[KSPField]
	public string trf_anchor_name;

	public Transform anchor;

	[KSPField]
	public string trf_animationRoot_name;

	public Transform animRoot;

	public Animation anim;

	[KSPField]
	public string anim_decouple_name;

	public ConfigurableJoint clampJoint;

	public FXGroup releaseFx;

	public float initialHeight = -1f;

	[KSPField(isPersistant = true)]
	public float scaleFactor = 1f;

	[KSPField(isPersistant = true)]
	public float height;

	public Quaternion towerRotation;

	public Vector3[] points;

	public bool extension_enabled;

	public Material towerMaterial;

	public void EnableExtension()
	{
		extension_enabled = true;
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("towerRot"))
		{
			towerRotation = KSPUtil.ParseQuaternion(node.GetValue("towerRot"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("towerRot", KSPUtil.WriteQuaternion(towerRotation));
	}

	public Collider FindCollider(Transform xform)
	{
		Collider collider = xform.GetComponent<Collider>();
		if (!collider)
		{
			foreach (Transform item in xform)
			{
				collider = FindCollider(item);
				if ((bool)collider)
				{
					return collider;
				}
			}
		}
		return collider;
	}

	public Vector3 MakePoint(Transform xform, Vector3 c, float x, float y, float z)
	{
		return new Vector3(c.x + x, c.y + y, c.z + z) - xform.position;
	}

	public void OnPutToGround(PartHeightQuery qr)
	{
		qr.lowestOnParts[base.part] = 0f;
		if (qr.lowestPoint < qr.lowestOnParts[base.part])
		{
			height += qr.lowestOnParts[base.part] - qr.lowestPoint;
			initialHeight = -1f;
		}
		else
		{
			qr.lowestPoint = qr.lowestOnParts[base.part];
		}
	}

	public void CreatePoints()
	{
		if ((bool)anchor)
		{
			Collider collider = FindCollider(anchor);
			if ((bool)collider)
			{
				Vector3 min = collider.bounds.min;
				Vector3 vector = collider.bounds.max - min;
				points = new Vector3[4]
				{
					MakePoint(anchor, min, 0f, vector.y / 2f, 0f),
					MakePoint(anchor, min, vector.x, vector.y / 2f, 0f),
					MakePoint(anchor, min, 0f, vector.y / 2f, vector.z),
					MakePoint(anchor, min, vector.x, vector.y / 2f, vector.z)
				};
			}
		}
	}

	public void ExtendTower()
	{
		if (points == null)
		{
			return;
		}
		float num = -1f;
		for (int num2 = 3; num2 >= 0; num2--)
		{
			if (Physics.Raycast(anchor.TransformPoint(points[num2]), -anchor.up, out var hitInfo, 100f, 32768))
			{
				num = ((num < 0f || num2 == 0) ? hitInfo.distance : Math.Min(num, hitInfo.distance));
			}
			Debug.Log($"[ExtendTower] {points[num2]} {hitInfo.distance} {num}");
		}
		if (!(num < 0f))
		{
			float num3 = Vector3.Distance(anchor.position, towerStretch.position);
			height = num3 + num;
			Debug.Log($"[ExtendTower] {num3} {height}");
		}
	}

	public override void OnInitialize()
	{
		base.part.PermanentGroundContact = true;
	}

	public override void OnStart(StartState state)
	{
		if (base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = "STRUT";
		}
		base.part.PermanentGroundContact = true;
		towerPivot = base.part.FindModelTransform(trf_towerPivot_name);
		if (!towerPivot)
		{
			Debug.LogWarning("[Launch Clamp Warning]: No 'towerPivot' transform defined", base.gameObject);
		}
		towerStretch = base.part.FindModelTransform(trf_towerStretch_name);
		if (!towerStretch)
		{
			Debug.LogWarning("[Launch Clamp Warning]: No 'towerStretch' transform defined", base.gameObject);
		}
		anchor = base.part.FindModelTransform(trf_anchor_name);
		if (!anchor)
		{
			Debug.LogWarning("[Launch Clamp Warning]: No 'anchor' transform defined", base.gameObject);
		}
		animRoot = base.part.FindModelTransform(trf_animationRoot_name);
		if (!animRoot)
		{
			Debug.LogWarning("[Launch Clamp Warning]: No animation root transform defined", base.gameObject);
		}
		else
		{
			anim = animRoot.GetComponent<Animation>();
			if (!anim)
			{
				Debug.LogWarning("[Launch Clamp Warning]: Animation root is defined, but doesn't contain animation", base.gameObject);
			}
			else if (anim[anim_decouple_name] == null)
			{
				Debug.LogWarning("[Launch Clamp Warning]: Animation component doesn't have an animation called " + anim_decouple_name, base.gameObject);
			}
			else
			{
				anim[anim_decouple_name].wrapMode = WrapMode.ClampForever;
				anim[anim_decouple_name].weight = 1f;
				anim[anim_decouple_name].enabled = true;
				anim[anim_decouple_name].speed = 0f;
			}
		}
		if (HighLogic.LoadedSceneIsFlight && extension_enabled)
		{
			CreatePoints();
			ExtendTower();
		}
		base.Events["Release"].active = false;
		if ((bool)anchor && (bool)towerPivot && (bool)towerStretch)
		{
			if (initialHeight == -1f)
			{
				initialHeight = Vector3.Distance(anchor.position, towerStretch.position);
				if (float.IsInfinity(initialHeight))
				{
					initialHeight = -1f;
				}
			}
			if (initialHeight != -1f)
			{
				scaleFactor = height / initialHeight;
			}
			towerMaterial = towerStretch.GetComponentInChildren<MeshRenderer>().material;
			if ((state & StartState.Landed) != 0)
			{
				towerMaterial.mainTextureScale = new Vector2(1f, scaleFactor);
				towerStretch.localScale = new Vector3(1f, scaleFactor, 1f);
				towerPivot.localRotation = towerRotation;
				anchor.localRotation = towerRotation;
				anchor.position = towerStretch.position + -towerStretch.up * height;
			}
		}
		if ((state & StartState.Landed) == 0)
		{
			return;
		}
		base.gameObject.SetActive(value: true);
		StartCoroutine(SetJointWhenPartStarted());
		if ((bool)base.part.parent)
		{
			if ((bool)anim)
			{
				anim[anim_decouple_name].normalizedTime = 0f;
			}
			base.Events["Release"].active = true;
			releaseFx = base.part.findFxGroup(releaseFxGroupName);
		}
		else if ((bool)anim)
		{
			anim[anim_decouple_name].normalizedTime = 1f;
		}
		if (!HighLogic.LoadedSceneIsEditor)
		{
			base.part.PermanentGroundContact = true;
		}
	}

	public IEnumerator SetJointWhenPartStarted()
	{
		while (base.part.packed)
		{
			yield return null;
		}
		yield return null;
		SetJoint();
	}

	public void SetJoint()
	{
		clampJoint = base.gameObject.AddComponent<ConfigurableJoint>();
		clampJoint.angularXMotion = ConfigurableJointMotion.Locked;
		clampJoint.angularYMotion = ConfigurableJointMotion.Locked;
		clampJoint.angularZMotion = ConfigurableJointMotion.Locked;
		clampJoint.xMotion = ConfigurableJointMotion.Locked;
		clampJoint.yMotion = ConfigurableJointMotion.Locked;
		clampJoint.zMotion = ConfigurableJointMotion.Locked;
		clampJoint.configuredInWorldSpace = false;
		clampJoint.autoConfigureConnectedAnchor = false;
		clampJoint.connectedAnchor = base.part.transform.position;
	}

	public void UnsetJoint()
	{
		UnityEngine.Object.Destroy(clampJoint);
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			if ((bool)towerPivot && (bool)towerStretch && (bool)anchor && (bool)towerMaterial)
			{
				towerPivot.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
				height = towerStretch.position.y;
				if (initialHeight != -1f)
				{
					scaleFactor = height / initialHeight;
				}
				towerStretch.localScale = new Vector3(1f, scaleFactor, 1f);
				towerRotation = towerPivot.localRotation;
				towerMaterial.mainTextureScale = new Vector2(1f, scaleFactor);
				anchor.position = towerStretch.position + Vector3.down * height;
				anchor.rotation = towerPivot.rotation;
			}
		}
		else
		{
			if (base.vessel != null)
			{
				base.vessel.permanentGroundContact = true;
			}
			base.part.PermanentGroundContact = true;
		}
	}

	public override void OnActive()
	{
		if (stagingEnabled)
		{
			Release();
		}
	}

	public void OnVesselPack()
	{
		if ((bool)clampJoint)
		{
			UnsetJoint();
		}
	}

	public void OnVesselUnpack()
	{
		if (!clampJoint)
		{
			SetJoint();
		}
	}

	[KSPAction("#autoLOC_6001865", activeEditor = false)]
	public void ReleaseClamp(KSPActionParam param)
	{
		Release();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001865")]
	public void Release()
	{
		if (!(base.part.parent == null) && HighLogic.LoadedSceneIsFlight)
		{
			Vessel vessel = base.vessel;
			base.part.decouple();
			if (vessel != null && vessel.persistentId != base.vessel.persistentId)
			{
				vessel.GroundContacts.Remove(base.part);
			}
			if (releaseFx != null)
			{
				releaseFx.Burst();
			}
			if ((bool)anim)
			{
				anim[anim_decouple_name].speed = 1f;
				anim.Play(anim_decouple_name);
			}
		}
	}

	public int GetStageIndex(int fallback)
	{
		return base.part.inverseStage;
	}
}
