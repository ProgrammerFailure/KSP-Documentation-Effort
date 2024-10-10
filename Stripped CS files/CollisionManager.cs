using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
	public class VesselColliderList
	{
		public Guid vesselId;

		public List<PartColliderList> colliderList;

		public VesselColliderList(Guid vId)
		{
			colliderList = new List<PartColliderList>();
			vesselId = vId;
		}
	}

	public class PartColliderList
	{
		public uint partPersistentId;

		public bool sameVesselCollision;

		public List<Collider> colliders;

		public PartColliderList(uint persistentId, bool sameVslCollision)
		{
			colliders = new List<Collider>();
			partPersistentId = persistentId;
			sameVesselCollision = sameVslCollision;
		}
	}

	public bool requireUpdate;

	public static List<VesselColliderList> vesselsList = new List<VesselColliderList>(32);

	public static CollisionManager Instance { get; set; }

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		GameEvents.OnCollisionIgnoreUpdate.Add(OnCollisionIgnoreUpdate);
	}

	public void OnDestroy()
	{
		GameEvents.OnCollisionIgnoreUpdate.Remove(OnCollisionIgnoreUpdate);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnCollisionIgnoreUpdate()
	{
		requireUpdate = true;
	}

	public void FixedUpdate()
	{
		UpdateCollisionIgnores();
	}

	public void UpdateCollisionIgnores()
	{
		if (FlightGlobals.ready && requireUpdate)
		{
			UpdatePartCollisionIgnores();
			requireUpdate = false;
		}
	}

	public List<VesselColliderList> GetAllVesselColliders()
	{
		List<VesselColliderList> list = vesselsList;
		list.Clear();
		bool flag = false;
		int count = FlightGlobals.VesselsLoaded.Count;
		while (count-- > 0)
		{
			if (FlightGlobals.VesselsLoaded[count].isEVA)
			{
				flag = true;
				break;
			}
		}
		int i = 0;
		for (int count2 = FlightGlobals.Vessels.Count; i < count2; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			VesselColliderList vesselColliderList = new VesselColliderList(vessel.id);
			int j = 0;
			for (int count3 = vessel.parts.Count; j < count3; j++)
			{
				List<Collider> list2 = new List<Collider>();
				Part part = vessel.parts[j];
				PartColliderList partColliderList = new PartColliderList(part.persistentId, part.sameVesselCollision);
				Collider[] componentsInChildren = part.partTransform.GetComponentsInChildren<Collider>(flag);
				if (componentsInChildren != null)
				{
					int num = componentsInChildren.Length;
					for (int k = 0; k < num; k++)
					{
						Collider collider = componentsInChildren[k];
						if ((collider.gameObject.activeInHierarchy && collider.enabled) || (flag && (collider.CompareTag("Ladder") || collider.CompareTag("Airlock"))))
						{
							list2.Add(collider);
						}
					}
				}
				partColliderList.colliders = list2;
				vesselColliderList.colliderList.Add(partColliderList);
			}
			list.Add(vesselColliderList);
		}
		return list;
	}

	public void UpdatePartCollisionIgnores()
	{
		List<VesselColliderList> allVesselColliders = GetAllVesselColliders();
		int i = 0;
		for (int count = allVesselColliders.Count; i < count; i++)
		{
			int j = i;
			for (int count2 = allVesselColliders.Count; j < count2; j++)
			{
				List<PartColliderList> colliderList = allVesselColliders[i].colliderList;
				List<PartColliderList> colliderList2 = allVesselColliders[j].colliderList;
				bool flag = i == j;
				int k = 0;
				for (int count3 = colliderList.Count; k < count3; k++)
				{
					int l = (flag ? (k + 1) : 0);
					for (int count4 = colliderList2.Count; l < count4; l++)
					{
						int m = 0;
						for (int count5 = colliderList[k].colliders.Count; m < count5; m++)
						{
							int n = 0;
							for (int count6 = colliderList2[l].colliders.Count; n < count6; n++)
							{
								Collider collider = colliderList[k].colliders[m];
								Collider collider2 = colliderList2[l].colliders[n];
								if (!(collider.attachedRigidbody == collider2.attachedRigidbody))
								{
									bool ignore;
									if ((ignore = flag) && colliderList[k].sameVesselCollision && colliderList2[l].sameVesselCollision && colliderList[k].partPersistentId != colliderList2[l].partPersistentId)
									{
										ignore = false;
									}
									Physics.IgnoreCollision(collider, collider2, ignore);
								}
							}
						}
					}
				}
			}
		}
		allVesselColliders.Clear();
	}

	public static void UpdateAllColliders()
	{
		Instance.requireUpdate = true;
	}

	public static void IgnoreCollidersOnVessel(Vessel vessel, params Collider[] ignoreColliders)
	{
		SetCollidersOnVessel(vessel, ignore: true, ignoreColliders);
	}

	public static void SetCollidersOnVessel(Vessel vessel, bool ignore, params Collider[] ignoreColliders)
	{
		for (int num = vessel.parts.Count - 1; num >= 0; num--)
		{
			Collider[] componentsInChildren = vessel.parts[num].partTransform.GetComponentsInChildren<Collider>(includeInactive: false);
			if (componentsInChildren != null)
			{
				for (int num2 = componentsInChildren.Length - 1; num2 >= 0; num2--)
				{
					Collider collider = componentsInChildren[num2];
					if (collider.gameObject.activeInHierarchy && collider.enabled)
					{
						for (int num3 = ignoreColliders.Length - 1; num3 >= 0; num3--)
						{
							Physics.IgnoreCollision(ignoreColliders[num3], collider, ignore);
						}
					}
				}
			}
		}
	}
}
