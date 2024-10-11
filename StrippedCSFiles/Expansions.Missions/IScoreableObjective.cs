using System;

namespace Expansions.Missions;

public interface IScoreableObjective
{
	object GetScoreModifier(Type scoreModule);
}
