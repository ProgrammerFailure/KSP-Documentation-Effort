using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalModel : MonoBehaviour
{
	public Part part;

	public ConfigNode internalConfig;

	public List<InternalSeat> seats;

	private Vector3 tmpPos;

	private Quaternion tmpRot;

	private Vector3 tmporgPos;

	private Quaternion tmporgRot;

	public string internalName;

	public List<InternalProp> props;

	private static Quaternion internalToWorld;

	public Vessel vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int CrewCapacity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalModel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static InternalModel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AssignToSeat(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVisible(bool visible)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetAvailableSeatCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<InternalSeat> GetAvailableSeats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalSeat GetNextAvailableSeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetNextAvailableSeatIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SitKerbalAt(ProtoCrewMember kerbal, InternalSeat seat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnseatKerbalAt(InternalSeat seat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnseatKerbal(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalProp AddProp(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalProp AddPropModule(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform FindModelTransform(string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Transform FindHeirarchyTransform(Transform parent, string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindModelComponent<T>() where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindModelComponent<T>(string childName) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static T FindModelComponent<T>(Transform parent, string childName) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T[] FindModelComponents<T>() where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T[] FindModelComponents<T>(string childName) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void FindModelComponents<T>(Transform parent, string childName, List<T> tList) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Animation[] FindModelAnimators(string clipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Animation[] FindModelAnimators()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 InternalToWorld(Vector3 internalSpacePosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion InternalToWorld(Quaternion internalSpaceRotation)
	{
		throw null;
	}
}
