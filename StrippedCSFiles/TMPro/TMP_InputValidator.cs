using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public abstract class TMP_InputValidator : ScriptableObject
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	protected TMP_InputValidator()
	{
		throw null;
	}

	public abstract char Validate(ref string text, ref int pos, char ch);
}
