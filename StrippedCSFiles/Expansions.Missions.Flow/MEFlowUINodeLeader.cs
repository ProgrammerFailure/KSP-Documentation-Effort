using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Flow;

[Serializable]
public class MEFlowUINodeLeader
{
	public string name;

	public Sprite image;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEFlowUINodeLeader()
	{
		throw null;
	}
}
