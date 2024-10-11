using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class MaterialColorUpdater
{
	public List<Renderer> renderers;

	public int propertyID;

	private Color setColor;

	private MaterialPropertyBlock mpb;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MaterialColorUpdater(Transform t, int propertyID, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MaterialColorUpdater(Transform t, int propertyID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateRendererList(Transform t, int propertyID, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update(Color color, bool force = false)
	{
		throw null;
	}
}
