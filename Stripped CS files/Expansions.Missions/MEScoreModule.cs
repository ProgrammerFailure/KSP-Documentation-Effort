using System;

namespace Expansions.Missions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class MEScoreModule : Attribute
{
	public Type[] AllowedSystems { get; set; }

	public MEScoreModule(params Type[] allowedSystems)
	{
		AllowedSystems = allowedSystems;
	}

	public bool IsModuleAllowed(Type module)
	{
		int num = AllowedSystems.Length;
		do
		{
			if (num-- <= 0)
			{
				return false;
			}
		}
		while (!(AllowedSystems[num] == module));
		return true;
	}
}
