using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FinePrint.Utilities;

public class ConstructionPart
{
	public string partName;

	public AvailablePart availablePart;

	public List<string> contractTypes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConstructionPart(string name, AvailablePart aPart, List<string> contractTypes)
	{
		throw null;
	}
}
