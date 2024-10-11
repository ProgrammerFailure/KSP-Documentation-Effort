using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalIndicatorPanel : InternalModule
{
	public enum IndicatorValue
	{
		SAS,
		RCS,
		Gear,
		KillRot,
		Stage,
		Warn,
		Fuel,
		Oxygen,
		MECO,
		Airlock,
		Heat,
		HighG
	}

	[Serializable]
	public class Indicator
	{
		public string name;

		public bool enabled;

		public Color colorOff;

		public Color colorOn;

		public IndicatorValue value;

		public Renderer renderer;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Indicator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Indicator Create(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Set(bool on)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetOn()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetOff()
		{
			throw null;
		}
	}

	[Serializable]
	public class IndicatorList : IConfigNode
	{
		public List<Indicator> list;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public IndicatorList()
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

	[KSPField]
	public IndicatorList indicators;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalIndicatorPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}
}
