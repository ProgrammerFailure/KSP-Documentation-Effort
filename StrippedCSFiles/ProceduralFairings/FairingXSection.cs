using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProceduralFairings;

[Serializable]
public class FairingXSection : IConfigNode, IComparable<FairingXSection>
{
	public float h;

	public float r;

	public bool isLast;

	public bool isCap;

	public bool isValid;

	public Color color;

	[NonSerialized]
	private FairingXSection lerp1;

	[NonSerialized]
	private FairingXSection lerp2;

	public List<FairingPanelFlags> fairingPanelFlags;

	private bool isLerp;

	public bool IsLerp
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FairingXSection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FairingXSection(bool isCap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FairingXSection(FairingXSection from, FairingXSection to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FairingXSection(FairingXSection cloneOf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddAttachedFlag(int panelIndex, uint flagPartID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddNewFairingPanel(int panelIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetPanelIndexFromFlag(uint FlagID, uint placementID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveAttachedFlag(uint FlagID)
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
	public void UpdateLerp(float t, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetSlopeAngle(FairingXSection from, FairingXSection to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float CircleCast(FairingXSection xs, Vector3 wAxis, Vector3 wPivot, Vector3 wRadial, int nRays, float rLength, int layerMask, out float lVariance, out RaycastHit hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ConeCast(FairingXSection xsFrom, FairingXSection xsTo, Vector3 wAxis, Vector3 wPivot, Vector3 wRadial, float radiusOffset, int nRays, int layerMask, out float hitLengthScalar, float aOffset = 0f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CompareTo(FairingXSection b)
	{
		throw null;
	}
}
