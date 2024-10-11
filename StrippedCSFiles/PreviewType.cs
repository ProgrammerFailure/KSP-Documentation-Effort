using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PreviewType
{
	public GameObject prefabPreview;

	public ProtoCrewMember.KerbalSuit suitType;

	public ProtoCrewMember.Gender gender;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PreviewType()
	{
		throw null;
	}
}
