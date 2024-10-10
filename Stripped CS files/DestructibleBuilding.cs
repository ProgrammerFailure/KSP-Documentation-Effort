using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DestructibleBuilding : MonoBehaviour, ICollisionEvents
{
	[Serializable]
	public class CollapsibleObject
	{
		public enum Behaviour
		{
			Disable,
			Collapse
		}

		public GameObject collapseObject;

		public GameObject replacementObject;

		public Behaviour collapseBehaviour;

		public float collapseDuration;

		public float repairDuration;

		public float replaceDelay = 1f;

		public Vector3 collapseOffset = Vector3.zero;

		public Vector3 collapseTiltMax = Vector3.zero;

		public VFXSequencer SecondaryFXPrefab;

		public DestructibleBuilding sharedWith;

		public Transform cTrf;

		public Vector3 stPos;

		public Vector3 endPos;

		public Quaternion stRot;

		public bool intact = true;

		public bool destroyed;

		public bool init;

		public bool setup => init;

		public void Init()
		{
			cTrf = collapseObject.transform;
			if (!init)
			{
				stPos = cTrf.transform.localPosition;
				stRot = cTrf.transform.localRotation;
				endPos = stPos + collapseOffset;
				init = true;
			}
		}

		public void Load(ConfigNode node)
		{
			if (node.HasValue("intact"))
			{
				intact = bool.Parse(node.GetValue("intact"));
				destroyed = !intact;
			}
			if (destroyed)
			{
				SetDestroyed(destroyed);
			}
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("intact", intact);
		}

		public void Collapse(DestructibleBuilding host)
		{
			if (!intact)
			{
				return;
			}
			intact = false;
			if (sharedWith != null && !sharedWith.IsIntact)
			{
				destroyed = true;
				return;
			}
			if (SecondaryFXPrefab != null)
			{
				host.spawnVFX(SecondaryFXPrefab).Play(host.onFXComplete);
			}
			Behaviour behaviour = collapseBehaviour;
			if (behaviour != 0 && behaviour == Behaviour.Collapse)
			{
				host.StartCoroutine(collapseCoroutine());
			}
			else
			{
				host.StartCoroutine(disableCoroutine());
			}
		}

		public IEnumerator disableCoroutine()
		{
			yield return new WaitForSeconds(collapseDuration);
			SetDestroyed(st: true);
			destroyed = true;
		}

		public IEnumerator collapseCoroutine()
		{
			float startT = Time.timeSinceLevelLoad;
			float endT = Time.timeSinceLevelLoad + collapseDuration;
			Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
			Vector3 euler = new Vector3(collapseTiltMax.x * onUnitSphere.x, collapseTiltMax.y * onUnitSphere.y, collapseTiltMax.z * onUnitSphere.z);
			Quaternion endRot = Quaternion.Euler(euler) * stRot;
			while (Time.timeSinceLevelLoad <= endT)
			{
				float t = Mathfx.XLerp(0f, 1f, Mathf.InverseLerp(startT, endT, Time.timeSinceLevelLoad), 3f);
				cTrf.localPosition = Vector3.Lerp(stPos, endPos, t);
				cTrf.localRotation = Quaternion.Lerp(stRot, endRot, t);
				if (replacementObject != null && !replacementObject.activeSelf && Time.timeSinceLevelLoad > startT + replaceDelay)
				{
					replacementObject.SetActive(value: true);
				}
				yield return null;
			}
			collapseObject.SetActive(value: false);
			destroyed = true;
		}

		public void Repair(DestructibleBuilding host)
		{
			if (destroyed)
			{
				destroyed = false;
				if (sharedWith != null && sharedWith.IsDestroyed)
				{
					intact = true;
				}
				else
				{
					host.StartCoroutine(repairCoroutine());
				}
			}
		}

		public IEnumerator repairCoroutine()
		{
			yield return new WaitForSeconds(repairDuration);
			SetDestroyed(st: false);
			intact = true;
		}

		public void SetDestroyed(bool st)
		{
			if (st)
			{
				collapseObject.SetActive(value: false);
				if (replacementObject != null)
				{
					replacementObject.SetActive(value: true);
				}
			}
			else
			{
				collapseObject.SetActive(value: true);
				cTrf.localPosition = stPos;
				cTrf.localRotation = stRot;
				if (replacementObject != null)
				{
					replacementObject.SetActive(value: false);
				}
			}
			destroyed = st;
			intact = !st;
		}

		public void Reset()
		{
			SetDestroyed(st: false);
		}
	}

	public CollapsibleObject[] CollapsibleObjects;

	public VFXSequencer DemolitionFXPrefab;

	public VFXSequencer RepairFXPrefab;

	public Transform FxTarget;

	public bool intact = true;

	public bool destroyed;

	public float FacilityDamageFraction;

	public float RepairCost;

	public float CollapseReputationHit;

	public float impactMomentumThreshold = 500f;

	public float damageDecay = 5f;

	public float damage;

	public float invulnerableTime;

	public static float BuildingToughnessFactor = 0.75f;

	public static EventData<DestructibleBuilding> OnLoaded = new EventData<DestructibleBuilding>("OnDestructibleLoaded");

	public bool preCompiledId;

	public string id;

	public bool indestructible;

	public bool registered;

	public VFXSequencer spawnedVFX;

	public bool needsResetOnReEnable;

	public bool IsIntact => intact;

	public bool IsDestroyed => destroyed;

	public float Damage => damage;

	public void Awake()
	{
		CollapsibleObject[] collapsibleObjects = CollapsibleObjects;
		for (int i = 0; i < collapsibleObjects.Length; i++)
		{
			collapsibleObjects[i].Init();
		}
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.AddComponent<CollisionEventsHandler>().SetHandle(this);
		}
		GameEvents.onGameSceneLoadRequested.Add(OnLeavingScene);
		invulnerableTime = 0f;
		GameEvents.onPhysicsEaseStart.Add(OnPhysicsEase);
		OnLevelLoaded(HighLogic.LoadedScene);
		GameEvents.onLevelWasLoaded.Add(OnSceneStart);
	}

	public void Start()
	{
		if (!RegisterInstance())
		{
			Reset();
		}
	}

	public void OnPhysicsEase(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel && invulnerableTime < PhysicsGlobals.BuildingEasingInvulnerableTime)
		{
			invulnerableTime = PhysicsGlobals.BuildingEasingInvulnerableTime;
		}
	}

	public void OnLevelLoaded(GameScenes scn)
	{
		if (scn == GameScenes.FLIGHT)
		{
			StartFlightNoDamagePeriod();
		}
	}

	public void OnSceneStart(GameScenes scn)
	{
		OnLevelLoaded(scn);
		if ((uint)(scn - 5) <= 2u && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(LoadOnSceneStart());
		}
	}

	public IEnumerator LoadOnSceneStart()
	{
		yield return new WaitForEndOfFrame();
		if (!RegisterInstance())
		{
			Reset();
		}
	}

	[ContextMenu("Compile ID")]
	public void CompileID()
	{
		id = HierarchyUtil.CompileID(base.transform, "SpaceCenter");
		preCompiledId = true;
	}

	[ContextMenu("Clear ID")]
	public void ClearID()
	{
		id = "";
		preCompiledId = false;
	}

	public bool RegisterInstance()
	{
		if (!preCompiledId)
		{
			id = HierarchyUtil.CompileID(base.transform, "SpaceCenter");
		}
		bool result = false;
		if (string.IsNullOrEmpty(id))
		{
			Debug.LogError("[DestructibleBuilding]: ID for this building is not defined correctly. Make sure all IDs are compiled and serialized before running the game.", base.gameObject);
		}
		else
		{
			result = ScenarioDestructibles.RegisterDestructible(this, id);
			registered = true;
		}
		return result;
	}

	public void UnregisterInstance(bool saveState)
	{
		if (registered)
		{
			if (string.IsNullOrEmpty(id))
			{
				Debug.LogError(string.Concat("[DestructibleBuilding]: ID for this building is not defined correctly. Cannot unregister.", base.gameObject, "[", base.gameObject.name, "]"), base.gameObject);
			}
			else
			{
				ScenarioDestructibles.UnregisterDestructible(this, id, saveState);
				registered = false;
			}
		}
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("intact"))
		{
			intact = bool.Parse(node.GetValue("intact"));
			destroyed = !intact;
		}
		int num = CollapsibleObjects.Length;
		while (num-- > 0)
		{
			CollapsibleObjects[num].Init();
			if (CollapsibleObjects[num].sharedWith != null)
			{
				CollapsibleObjects[num].SetDestroyed(!intact || !CollapsibleObjects[num].sharedWith.intact);
			}
			else
			{
				CollapsibleObjects[num].SetDestroyed(!intact);
			}
		}
		OnLoaded.Fire(this);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("intact", intact);
	}

	public void FixedUpdate()
	{
		if (invulnerableTime > 0f)
		{
			invulnerableTime -= TimeWarp.fixedDeltaTime;
		}
		damageUpdate();
	}

	public void damageUpdate()
	{
		if (damage > 0f)
		{
			damage -= impactMomentumThreshold * BuildingToughnessFactor / damageDecay * TimeWarp.fixedDeltaTime;
			if (damage < 0f)
			{
				damage = 0f;
			}
		}
	}

	[ContextMenu("Demolish")]
	public void Demolish()
	{
		if (destroyed || !intact)
		{
			return;
		}
		CollapsibleObject[] collapsibleObjects = CollapsibleObjects;
		for (int i = 0; i < collapsibleObjects.Length; i++)
		{
			collapsibleObjects[i].Collapse(this);
		}
		if (DemolitionFXPrefab != null)
		{
			spawnedVFX = spawnVFX(DemolitionFXPrefab);
			spawnedVFX.Play(onDemolitionComplete);
		}
		else
		{
			float num = float.MinValue;
			int num2 = CollapsibleObjects.Length;
			while (num2-- > 0)
			{
				float collapseDuration = CollapsibleObjects[num2].collapseDuration;
				if (collapseDuration > num)
				{
					num = collapseDuration;
				}
			}
			StartCoroutine(delayedCallback<VFXSequencer>(num, onDemolitionComplete, null));
		}
		intact = false;
		GameEvents.OnKSCStructureCollapsing.Fire(this);
	}

	public void onFXComplete(VFXSequencer vfx)
	{
		UnityEngine.Object.Destroy(vfx.gameObject);
	}

	public void onDemolitionComplete(VFXSequencer vfx)
	{
		destroyed = true;
		clearVFX();
		GameEvents.OnKSCStructureCollapsed.Fire(this);
	}

	[ContextMenu("Repair")]
	public void Repair()
	{
		if (intact)
		{
			Debug.LogWarning("[Destructible Building]: Building does not require repairs.", base.gameObject);
			return;
		}
		if (!destroyed)
		{
			Debug.LogWarning("[Destructible Building]: Building is already undergoing repairs.", base.gameObject);
			return;
		}
		CollapsibleObject[] collapsibleObjects = CollapsibleObjects;
		for (int i = 0; i < collapsibleObjects.Length; i++)
		{
			collapsibleObjects[i].Repair(this);
		}
		if (RepairFXPrefab != null)
		{
			spawnedVFX = spawnVFX(RepairFXPrefab);
			spawnedVFX.Play(onRepairComplete);
		}
		else
		{
			float num = float.MinValue;
			int num2 = CollapsibleObjects.Length;
			while (num2-- > 0)
			{
				float repairDuration = CollapsibleObjects[num2].repairDuration;
				if (repairDuration > num)
				{
					num = repairDuration;
				}
			}
			StartCoroutine(delayedCallback<VFXSequencer>(num, onRepairComplete, null));
		}
		destroyed = false;
		GameEvents.OnKSCStructureRepairing.Fire(this);
	}

	public void onRepairComplete(VFXSequencer vfx)
	{
		intact = true;
		clearVFX();
		GameEvents.OnKSCStructureRepaired.Fire(this);
	}

	public void Reset()
	{
		intact = true;
		destroyed = false;
		damage = 0f;
		invulnerableTime = 0f;
		clearVFX();
		if (CollapsibleObjects != null)
		{
			CollapsibleObject[] collapsibleObjects = CollapsibleObjects;
			for (int i = 0; i < collapsibleObjects.Length; i++)
			{
				collapsibleObjects[i].Reset();
			}
		}
	}

	public float GetRepairSequenceDuration()
	{
		float num = float.MinValue;
		int num2 = CollapsibleObjects.Length;
		while (num2-- > 0)
		{
			float repairDuration = CollapsibleObjects[num2].repairDuration;
			if (repairDuration > num)
			{
				num = repairDuration;
			}
		}
		return num;
	}

	public List<ProtoVessel> FindVesselsOverStructure(FlightState st)
	{
		return ShipConstruction.FindVesselsLandedAt(st, base.tag);
	}

	public VFXSequencer spawnVFX(VFXSequencer vfxPrefab)
	{
		GameObject obj = UnityEngine.Object.Instantiate(vfxPrefab.gameObject, FxTarget.transform.position, FxTarget.transform.rotation);
		obj.transform.parent = FxTarget;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		return obj.GetComponent<VFXSequencer>();
	}

	public void clearVFX()
	{
		if (spawnedVFX != null)
		{
			UnityEngine.Object.Destroy(spawnedVFX.gameObject);
		}
	}

	public void OnLeavingScene(GameScenes scn)
	{
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.FLIGHT)
		{
			UnregisterInstance(saveState: true);
			clearVFX();
			StopAllCoroutines();
		}
		if (scn == GameScenes.MAINMENU || scn == GameScenes.EDITOR || scn == GameScenes.FLIGHT || scn == GameScenes.TRACKSTATION)
		{
			Reset();
		}
	}

	public void OnDisable()
	{
		needsResetOnReEnable = true;
	}

	public void OnEnable()
	{
		if (needsResetOnReEnable)
		{
			if (!RegisterInstance())
			{
				Reset();
			}
			needsResetOnReEnable = false;
		}
	}

	public void OnDestroy()
	{
		if (registered)
		{
			UnregisterInstance(saveState: false);
		}
		clearVFX();
		GameEvents.onGameSceneLoadRequested.Remove(OnLeavingScene);
		GameEvents.onLevelWasLoaded.Remove(OnSceneStart);
		GameEvents.onVesselGoOffRails.Remove(EndFlightNoDamagePeriod);
		GameEvents.onVesselGoOnRails.Remove(restartNoDamagePeriod);
		GameEvents.onPhysicsEaseStart.Remove(OnPhysicsEase);
	}

	public IEnumerator delayedCallback(float delay, Callback cb)
	{
		yield return new WaitForSeconds(delay);
		cb();
	}

	public IEnumerator delayedCallback<T>(float delay, Callback<T> cb, T arg1)
	{
		yield return new WaitForSeconds(delay);
		cb(arg1);
	}

	public void OnCollisionEnter(Collision c)
	{
		if (c.contacts.Length == 0 || invulnerableTime > 0f)
		{
			return;
		}
		Vector3 lhs = Vector3.zero;
		float num = float.MinValue;
		int num2 = c.contacts.Length;
		while (num2-- > 0)
		{
			Vector3 normal = c.contacts[num2].normal;
			float num3 = Mathf.Abs(Vector3.Dot(normal, c.relativeVelocity.normalized));
			if (num3 > num)
			{
				num = num3;
				lhs = normal;
			}
		}
		float f = Vector3.Dot(lhs, c.relativeVelocity.normalized);
		float num4 = c.relativeVelocity.sqrMagnitude;
		float num5 = float.MaxValue;
		float num6 = float.MaxValue;
		Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(c.gameObject);
		if (partUpwardsCached != null)
		{
			if (partUpwardsCached.Rigidbody != null)
			{
				num5 = partUpwardsCached.Rigidbody.velocity.magnitude;
			}
			if (partUpwardsCached.vessel != null)
			{
				num6 = partUpwardsCached.vessel.rb_velocity.sqrMagnitude;
			}
		}
		if (num4 * PhysicsGlobals.BuildingImpactDamageMaxVelocityMult > num5)
		{
			num4 = num5;
			f = Vector3.Dot(lhs, partUpwardsCached.Rigidbody.velocity.normalized);
		}
		if (num4 * PhysicsGlobals.BuildingImpactDamageMaxVelocityMult > num6)
		{
			num4 = num6;
			f = Vector3.Dot(lhs, partUpwardsCached.vessel.rb_velocity.normalized);
		}
		if (PhysicsGlobals.BuildingImpactDamageUseMomentum)
		{
			num4 = Mathf.Sqrt(num4);
		}
		f = Mathfx.Ease(Mathf.Abs(f), 3f);
		float d = num4 * c.rigidbody.mass * f * HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().BuildingImpactDamageMult;
		Debug.DrawRay(c.contacts[0].point, c.contacts[0].normal * 5f, Color.yellow, 5f);
		if (damage > 0f && partUpwardsCached != null)
		{
			if (PhysicsGlobals.BuildingImpactDamageUseMomentum)
			{
				num6 = Mathf.Sqrt(num6);
			}
			d = num6 * (float)partUpwardsCached.vessel.totalMass * f * HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().BuildingImpactDamageMult;
		}
		AddDamage(d);
	}

	public void OnCollisionStay(Collision c)
	{
	}

	public void OnCollisionExit(Collision c)
	{
	}

	public MonoBehaviour GetInstance()
	{
		return this;
	}

	public void AddDamage(float d)
	{
		if (!HighLogic.LoadedSceneIsGame || (!HighLogic.CurrentGame.Parameters.Difficulty.IndestructibleFacilities && !indestructible && d > PhysicsGlobals.BuildingImpactDamageMinDamageFraction))
		{
			damage += d;
			if (damage > impactMomentumThreshold * BuildingToughnessFactor && intact)
			{
				Demolish();
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			}
		}
	}

	public void StartFlightNoDamagePeriod()
	{
		indestructible = true;
		GameEvents.onVesselGoOffRails.Add(EndFlightNoDamagePeriod);
	}

	public void EndFlightNoDamagePeriod(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel)
		{
			GameEvents.onVesselGoOffRails.Remove(EndFlightNoDamagePeriod);
			v.StartCoroutine(CallbackUtil.DelayedCallback(1f, delegate
			{
				indestructible = false;
			}));
		}
	}

	public void restartNoDamagePeriod(Vessel v)
	{
		StartFlightNoDamagePeriod();
		GameEvents.onVesselGoOnRails.Remove(restartNoDamagePeriod);
	}
}
