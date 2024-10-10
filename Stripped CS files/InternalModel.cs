using System.Collections.Generic;
using UnityEngine;

public class InternalModel : MonoBehaviour
{
	public Part part;

	public ConfigNode internalConfig;

	public List<InternalSeat> seats = new List<InternalSeat>();

	public Vector3 tmpPos;

	public Quaternion tmpRot;

	public Vector3 tmporgPos;

	public Quaternion tmporgRot;

	public string internalName;

	public List<InternalProp> props = new List<InternalProp>();

	public static Quaternion internalToWorld = Quaternion.Euler(90f, 180f, 0f);

	public Vessel vessel => part.vessel;

	public int CrewCapacity => seats.Count;

	public void Initialize(Part p)
	{
		part = p;
		base.transform.parent = InternalSpace.Instance.transform;
		tmporgPos = part.orgPos;
		tmporgRot = part.orgRot;
		tmpPos = part.vessel.transform.position + part.vessel.transform.rotation * part.orgPos;
		tmpRot = part.vessel.transform.rotation * part.orgRot;
		base.transform.position = InternalSpace.WorldToInternal(tmpPos);
		base.transform.rotation = InternalSpace.WorldToInternal(tmpRot) * Quaternion.Euler(90f, 180f, 0f);
		int i = 0;
		for (int count = props.Count; i < count; i++)
		{
			props[i].OnAwake();
		}
		if (p.CrewCapacity != seats.Count)
		{
			Debug.LogWarning("InternalModel error: Part Crew capacity is " + p.CrewCapacity + ", but " + seats.Count + " seats are defined in internal model", part);
		}
		int j = 0;
		for (int count2 = seats.Count; j < count2; j++)
		{
			InternalSeat internalSeat = seats[j];
			internalSeat.taken = false;
			internalSeat.crew = null;
		}
		int k = 0;
		for (int count3 = part.protoModuleCrew.Count; k < count3; k++)
		{
			ProtoCrewMember protoCrewMember = part.protoModuleCrew[k];
			if (protoCrewMember.seatIdx != -1)
			{
				AssignToSeat(protoCrewMember);
			}
		}
		int l = 0;
		for (int count4 = part.protoModuleCrew.Count; l < count4; l++)
		{
			ProtoCrewMember protoCrewMember = part.protoModuleCrew[l];
			if (protoCrewMember.seatIdx == -1)
			{
				protoCrewMember.seatIdx = GetNextAvailableSeatIndex();
				if (protoCrewMember.seatIdx == -1)
				{
					Debug.LogError("Internal Model error: Crewmember " + protoCrewMember.name + " couldn't be seated because the internal model doesn't have any more seats defined.", part);
				}
				else
				{
					AssignToSeat(protoCrewMember);
				}
			}
		}
	}

	public void AssignToSeat(ProtoCrewMember crew)
	{
		if (crew.seatIdx < seats.Count)
		{
			crew.seat = seats[crew.seatIdx];
			seats[crew.seatIdx].crew = crew;
			seats[crew.seatIdx].taken = true;
		}
		else
		{
			Debug.LogError("Internal Model error: Crewmember " + crew.name + "'s seat index exceeds internal model seat count!", part);
		}
	}

	public void SetVisible(bool visible)
	{
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		int num = componentsInChildren.Length;
		while (num-- > 0)
		{
			componentsInChildren[num].enabled = visible;
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		int num2 = componentsInChildren2.Length;
		while (num2-- > 0)
		{
			componentsInChildren2[num2].enabled = visible;
		}
	}

	public int GetAvailableSeatCount()
	{
		if (seats == null)
		{
			return 0;
		}
		return GetAvailableSeats().Count;
	}

	public List<InternalSeat> GetAvailableSeats()
	{
		List<InternalSeat> list = new List<InternalSeat>();
		int i = 0;
		for (int count = seats.Count; i < count; i++)
		{
			InternalSeat internalSeat = seats[i];
			if (!internalSeat.taken)
			{
				list.Add(internalSeat);
			}
		}
		return list;
	}

	public InternalSeat GetNextAvailableSeat()
	{
		int num = 0;
		int count = seats.Count;
		InternalSeat internalSeat;
		while (true)
		{
			if (num < count)
			{
				internalSeat = seats[num];
				if (!internalSeat.taken)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return internalSeat;
	}

	public int GetNextAvailableSeatIndex()
	{
		return seats.IndexOf(GetNextAvailableSeat());
	}

	public bool SitKerbalAt(ProtoCrewMember kerbal, InternalSeat seat)
	{
		if (seat != null && !seat.taken)
		{
			seat.crew = kerbal;
			seat.taken = true;
			kerbal.seatIdx = seats.IndexOf(seat);
			kerbal.seat = seat;
			return true;
		}
		return false;
	}

	public void UnseatKerbalAt(InternalSeat seat)
	{
		if (seat.taken)
		{
			seat.crew.seatIdx = -1;
			seat.crew.seat = null;
			seat.crew = null;
			seat.taken = false;
			if ((bool)seat.kerbalRef)
			{
				seat.DespawnCrew();
			}
		}
		else
		{
			Debug.LogWarning("Internal Model Error: Seat is Empty, Cannot unseat.", seat.transform);
		}
	}

	public void UnseatKerbal(ProtoCrewMember crew)
	{
		int i = 0;
		for (int count = seats.Count; i < count; i++)
		{
			InternalSeat internalSeat = seats[i];
			if (internalSeat.crew == crew)
			{
				UnseatKerbalAt(internalSeat);
			}
		}
		crew.seatIdx = -1;
		crew.seat = null;
	}

	public void SpawnCrew()
	{
		int i = 0;
		for (int count = seats.Count; i < count; i++)
		{
			seats[i].SpawnCrew();
		}
	}

	public void DespawnCrew()
	{
		int i = 0;
		for (int count = seats.Count; i < count; i++)
		{
			seats[i].DespawnCrew();
		}
	}

	public void Load(ConfigNode node)
	{
		internalConfig = new ConfigNode();
		node.CopyTo(internalConfig);
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (configNode.name == "PROP")
			{
				AddProp(configNode);
			}
			if (configNode.name == "MODULE")
			{
				AddPropModule(configNode);
			}
		}
		if (node.HasValue("scaleAll"))
		{
			Vector3 b = ConfigNode.ParseVector3(node.GetValue("scaleAll"));
			base.transform.localScale = Vector3.Scale(base.transform.localScale, b);
		}
		Transform transform = FindModelTransform("model");
		if (transform != null)
		{
			Vector3 vector = Vector3.zero;
			Vector3 b2 = Vector3.one;
			if (node.HasValue("offset"))
			{
				vector = ConfigNode.ParseVector3(node.GetValue("offset"));
				transform.localPosition += vector;
			}
			if (node.HasValue("scale"))
			{
				b2 = ConfigNode.ParseVector3(node.GetValue("scale"));
				transform.localScale = Vector3.Scale(transform.localScale, b2);
			}
			int count2 = props.Count;
			for (int j = 0; j < count2; j++)
			{
				props[j].transform.localPosition += vector;
				props[j].transform.localScale = Vector3.Scale(props[j].transform.localScale, b2);
			}
			Light[] componentsInChildren = transform.GetComponentsInChildren<Light>();
			int k = 0;
			for (int num = componentsInChildren.Length; k < num; k++)
			{
				componentsInChildren[k].cullingMask &= -268469250;
			}
		}
	}

	public InternalProp AddProp(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("Cannot add a Prop because ConfigNode contains no prop name");
			return null;
		}
		InternalProp internalProp = PartLoader.GetInternalProp(node.GetValue("name"));
		if (internalProp == null)
		{
			return null;
		}
		internalProp.propID = props.Count;
		internalProp.internalModel = this;
		internalProp.transform.parent = base.transform;
		internalProp.hasModel = true;
		internalProp.Load(node);
		int i = 0;
		for (int count = internalProp.internalModules.Count; i < count; i++)
		{
			InternalModule internalModule = internalProp.internalModules[i];
			if (internalModule.ClassName == "InternalSeat")
			{
				seats.Add((InternalSeat)internalModule);
			}
		}
		props.Add(internalProp);
		return internalProp;
	}

	public InternalProp AddPropModule(ConfigNode node)
	{
		GameObject gameObject = new GameObject();
		gameObject.gameObject.name = "Module";
		InternalProp internalProp = gameObject.AddComponent<InternalProp>();
		internalProp.propID = props.Count;
		internalProp.internalModel = this;
		internalProp.hasModel = false;
		internalProp.transform.parent = base.transform;
		internalProp.AddModule(node);
		if (internalProp.internalModules.Count > 0)
		{
			gameObject.gameObject.name = "Module " + internalProp.internalModules[0].ClassName;
		}
		int i = 0;
		for (int count = internalProp.internalModules.Count; i < count; i++)
		{
			InternalModule internalModule = internalProp.internalModules[i];
			if (internalModule.ClassName == "InternalSeat")
			{
				seats.Add((InternalSeat)internalModule);
			}
		}
		props.Add(internalProp);
		return internalProp;
	}

	public void OnUpdate()
	{
		if (tmporgPos != part.orgPos || tmporgRot != part.orgRot)
		{
			tmpPos = part.vessel.transform.position + part.vessel.transform.rotation * part.orgPos;
			tmpRot = part.vessel.transform.rotation * part.orgRot;
			base.transform.position = InternalSpace.WorldToInternal(tmpPos);
			base.transform.rotation = InternalSpace.WorldToInternal(tmpRot) * Quaternion.Euler(90f, 180f, 0f);
			tmporgPos = part.orgPos;
			tmporgRot = part.orgRot;
		}
		if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.const_3 || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal)
		{
			int count = props.Count;
			for (int i = 0; i < count; i++)
			{
				props[i].OnUpdate();
			}
		}
	}

	public void OnFixedUpdate()
	{
		if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.const_3 || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal)
		{
			int count = props.Count;
			for (int i = 0; i < count; i++)
			{
				props[i].OnFixedUpdate();
			}
		}
	}

	public Transform FindModelTransform(string childName)
	{
		return FindHeirarchyTransform(base.transform, childName);
	}

	public static Transform FindHeirarchyTransform(Transform parent, string childName)
	{
		if (parent.gameObject.name == childName)
		{
			return parent;
		}
		Transform transform = null;
		int num = 0;
		while (true)
		{
			if (num < parent.childCount)
			{
				transform = FindHeirarchyTransform(parent.GetChild(num), childName);
				if (transform != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return transform;
	}

	public T FindModelComponent<T>() where T : Component
	{
		return FindModelComponent<T>(base.transform.Find("model"), "");
	}

	public T FindModelComponent<T>(string childName) where T : Component
	{
		return FindModelComponent<T>(base.transform.Find("model"), childName);
	}

	public static T FindModelComponent<T>(Transform parent, string childName) where T : Component
	{
		if (parent == null)
		{
			return null;
		}
		if (childName == string.Empty || parent.gameObject.name == childName)
		{
			T component = parent.gameObject.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
		}
		int num = 0;
		T val;
		while (true)
		{
			if (num < parent.childCount)
			{
				val = FindModelComponent<T>(parent.GetChild(num), childName);
				if (val != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return val;
	}

	public T[] FindModelComponents<T>() where T : Component
	{
		List<T> list = new List<T>();
		FindModelComponents(base.transform, "", list);
		return list.ToArray();
	}

	public T[] FindModelComponents<T>(string childName) where T : Component
	{
		List<T> list = new List<T>();
		FindModelComponents(base.transform, "", list);
		return list.ToArray();
	}

	public static void FindModelComponents<T>(Transform parent, string childName, List<T> tList) where T : Component
	{
		if (parent == null)
		{
			return;
		}
		if (childName == string.Empty || parent.gameObject.name == childName)
		{
			T component = parent.gameObject.GetComponent<T>();
			if (component != null)
			{
				tList.Add(component);
			}
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			FindModelComponents(parent.GetChild(i), childName, tList);
		}
	}

	public Animation[] FindModelAnimators(string clipName)
	{
		List<Animation> list = new List<Animation>(FindModelComponents<Animation>());
		int count = list.Count;
		while (count-- > 0)
		{
			if (list[count].GetClip(clipName) == null)
			{
				list.RemoveAt(count);
			}
		}
		return list.ToArray();
	}

	public Animation[] FindModelAnimators()
	{
		return new List<Animation>(FindModelComponents<Animation>()).ToArray();
	}

	public Vector3 InternalToWorld(Vector3 internalSpacePosition)
	{
		return InternalToWorld(Quaternion.identity) * (internalSpacePosition - InternalSpace.Instance.transform.position) + part.transform.position;
	}

	public Quaternion InternalToWorld(Quaternion internalSpaceRotation)
	{
		return part.transform.rotation * internalToWorld * internalSpaceRotation;
	}
}
