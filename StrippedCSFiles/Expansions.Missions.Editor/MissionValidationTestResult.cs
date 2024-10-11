using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

public class MissionValidationTestResult
{
	public Guid nodeId;

	public ValidationStatus status;

	public string nodeName;

	public string type;

	public string message;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionValidationTestResult()
	{
		throw null;
	}
}
