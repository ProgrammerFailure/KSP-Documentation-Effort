using System.Runtime.CompilerServices;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class ExceptLinq : MonoBehaviour
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExceptLinq()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
