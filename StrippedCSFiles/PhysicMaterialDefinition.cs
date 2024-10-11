using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PhysicMaterialDefinition
{
	[SerializeField]
	private string _name;

	[SerializeField]
	private string _displayName;

	[SerializeField]
	private int _id;

	[SerializeField]
	private float _dynamicFriction;

	[SerializeField]
	private float _staticFriction;

	[SerializeField]
	private float _bounciness;

	[SerializeField]
	private PhysicMaterialCombine _frictionCombine;

	[SerializeField]
	private PhysicMaterialCombine _bounceCombine;

	[SerializeField]
	private ConfigNode _config;

	internal PhysicMaterial material;

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

	public float dynamicFriction
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float staticFriction
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float bounciness
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PhysicMaterialCombine frictionCombine
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PhysicMaterialCombine bounceCombine
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

	public PhysicMaterial Material
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PhysicMaterialDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PhysicMaterialDefinition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PhysicMaterialDefinition(string name, string displayname, float dynamicFriction, float staticFriction, float bounciness, PhysicMaterialCombine frictionCombine, PhysicMaterialCombine bounceCombine)
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
}
