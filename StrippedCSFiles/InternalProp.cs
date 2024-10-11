using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalProp : MonoBehaviour
{
	public InternalModel internalModel;

	public string propName;

	public int propID;

	public bool hasModel;

	public List<InternalModule> internalModules;

	public Part part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vessel vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalProp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalModule AddModule(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalModule AddModule(string moduleName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnAwake()
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
	public Transform[] FindModelTransforms(string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void FindHeirarchyTransforms(Transform parent, string childName, List<Transform> tList)
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
}
