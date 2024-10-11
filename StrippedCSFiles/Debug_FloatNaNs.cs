using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class Debug_FloatNaNs : MonoBehaviour
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public Debug_FloatNaNs()
	{
		throw null;
	}

	[DllImport("msvcrt.dll")]
	public static extern uint _control87(uint a, uint b);

	[DllImport("msvcrt.dll")]
	public static extern uint _clearfp();

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}
}
