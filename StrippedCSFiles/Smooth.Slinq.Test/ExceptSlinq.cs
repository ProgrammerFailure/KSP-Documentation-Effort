using System.Runtime.CompilerServices;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class ExceptSlinq : MonoBehaviour
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExceptSlinq()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
