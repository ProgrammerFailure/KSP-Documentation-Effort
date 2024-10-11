using System.Runtime.CompilerServices;
using UnityEngine;

public class PropObject : MonoBehaviour
{
	public PropTools.Prop prop;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PropObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PropObject Create(Transform parent, PropTools.Prop prop)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CreateProxies(GameObject go, PropTools.Prop prop)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RecreateProxies(PropTools.Prop newProp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshProxy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Component GetComponentOnParent(string type, GameObject obj)
	{
		throw null;
	}
}
