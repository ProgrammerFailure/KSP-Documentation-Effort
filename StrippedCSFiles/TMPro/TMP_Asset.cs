using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_Asset : ScriptableObject
{
	public int hashCode;

	public Material material;

	public int materialHashCode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_Asset()
	{
		throw null;
	}
}
