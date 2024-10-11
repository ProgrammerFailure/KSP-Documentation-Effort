using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

public class MissionCheckpointValidator
{
	private static List<string> modulesAreEqualErrorList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionCheckpointValidator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ValidateCheckpoint(Mission checkpointMission, Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool CheckCheckpointPathRecursive(Mission checkpointMission, Mission mission, MENode checkpointNode, MENode missionNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsCheckpointNodeDirty(MENode checkpointNode, MENode missionNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ModulesAreEqual(object checkpointModule, object missionModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CompareStructLists<T>(List<T> list1, List<T> list2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CompareObjectLists<T>(List<T> list1, List<T> list2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool CompareAnyLists<T>(List<T> list1, List<T> list2)
	{
		throw null;
	}
}
