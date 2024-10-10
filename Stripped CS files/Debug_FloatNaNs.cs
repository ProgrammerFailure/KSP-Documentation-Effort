using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Debug_FloatNaNs : MonoBehaviour
{
	[DllImport("msvcrt.dll")]
	public static extern uint _control87(uint a, uint b);

	[DllImport("msvcrt.dll")]
	public static extern uint _clearfp();

	public void Start()
	{
		uint num = _control87(0u, 0u);
		Console.WriteLine(num.ToString());
		num &= 0xFFFFFFEFu;
		_clearfp();
		_control87(num, 524319u);
		Console.WriteLine(num.ToString());
	}
}
