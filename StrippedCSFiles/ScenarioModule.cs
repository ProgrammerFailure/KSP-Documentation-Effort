using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScenarioModule : MonoBehaviour, IConfigNode
{
	public ScenarioRunner runner;

	public ProtoScenarioModule snapshot;

	public List<GameScenes> targetScenes;

	[SerializeField]
	[HideInInspector]
	private string className;

	[HideInInspector]
	[SerializeField]
	private int classID;

	[SerializeField]
	[HideInInspector]
	private BaseEventList events;

	[SerializeField]
	[HideInInspector]
	private BaseFieldList fields;

	public string ClassName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int ClassID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseEventList Events
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseFieldList Fields
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ModularSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}
}
