using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public abstract class UICameraBase : MonoBehaviour
{
	[SerializeField]
	protected Camera cam;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected UICameraBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}
}
