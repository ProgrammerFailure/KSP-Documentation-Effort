using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Upgradeables;

public class TimeOfDayAnimation : MonoBehaviour
{
	[Serializable]
	public class MaterialProperty
	{
		public Material material;

		public string propertyName;

		private List<Renderer> instances;

		private int propertyID;

		private MaterialPropertyBlock mpb;

		public bool isDirty
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MaterialProperty()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void UpdateInstances(Renderer[] renderers)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetColor(Color c)
		{
			throw null;
		}
	}

	public Transform target;

	public float dot;

	public Color emissiveColor;

	public Color emissiveTgtColor;

	public string emissiveColorProperty;

	public AnimationCurve emissivesCurve;

	public List<MaterialProperty> emissives;

	private Renderer[] renderers;

	private bool facilityDirty;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TimeOfDayAnimation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onFacilityEvent(UpgradeableObject upObj, int lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onDestructibleEvent(DestructibleBuilding dB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateRenderers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool MaterialPropertiesDirty()
	{
		throw null;
	}
}
