using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("KSP/SolarSystem/PSystem")]
public class PSystem : MonoBehaviour
{
	[HideInInspector]
	public int mainToolbarSelected;

	public double systemScale;

	public double systemTimeScale;

	public string systemName;

	public PSystemBody rootBody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PSystemBody AddBody(PSystemBody parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmosBody(PSystemBody parent, PSystemBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Load Celestial Bodies Database")]
	public void LoadDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadBody(PSystemBody body, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PSystemBody FindBody(string name, PSystemBody parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<PSystemBody> GetBodies(PSystemBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetBodies(List<PSystemBody> list, PSystemBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Save Celestial Bodies Database")]
	public void SaveDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveBody(PSystemBody body, ConfigNode node)
	{
		throw null;
	}
}
