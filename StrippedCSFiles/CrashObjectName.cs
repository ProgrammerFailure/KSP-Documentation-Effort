using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
[AddComponentMenu("CrashObjectName")]
public class CrashObjectName : MonoBehaviour
{
	public string objectName;

	public string displayName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrashObjectName()
	{
		throw null;
	}
}
