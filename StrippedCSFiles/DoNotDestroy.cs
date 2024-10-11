using System.Runtime.CompilerServices;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public DoNotDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}
}
