using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PartResourceDefinition
{
	[SerializeField]
	private string _name;

	[SerializeField]
	private string _displayName;

	[SerializeField]
	private int _id;

	[SerializeField]
	private float _density;

	[SerializeField]
	private float _volume;

	[SerializeField]
	private string _abbreviation;

	[SerializeField]
	private float _unitCost;

	[SerializeField]
	private float _specificHeatCapacity;

	[SerializeField]
	private ConfigNode _config;

	[SerializeField]
	private Color _color;

	[SerializeField]
	private ResourceFlowMode _resourceFlowMode;

	[SerializeField]
	private ResourceTransferMode _resourceTransferMode;

	[SerializeField]
	private bool _isTweakable;

	private bool _isVisible;

	public PartResourceDrainDefinition partResourceDrainDefinition;

	public string name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string displayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int id
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float density
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float volume
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string abbreviation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float unitCost
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float specificHeatCapacity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConfigNode Config
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Color color
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ResourceFlowMode resourceFlowMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ResourceTransferMode resourceTransferMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isTweakable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isVisible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDefinition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDefinition(string name, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetShortName(int length = 2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetFlowModeDescription()
	{
		throw null;
	}
}
