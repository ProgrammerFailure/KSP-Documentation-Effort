using System;

namespace SaveUpgradePipeline;

[Flags]
public enum LoadContext
{
	None = 0,
	flag_1 = 1,
	Craft = 2,
	Any = -1
}
