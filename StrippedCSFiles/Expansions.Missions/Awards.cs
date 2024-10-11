using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Awards : MonoBehaviour
{
	[SerializeField]
	private List<AwardDefinition> internalAwards;

	private Dictionary<string, AwardDefinition> awardsDictionary;

	public List<AwardDefinition> AwardDefinitions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Awards()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupAwards()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardDefinition GetAwardDefinition(string id)
	{
		throw null;
	}
}
