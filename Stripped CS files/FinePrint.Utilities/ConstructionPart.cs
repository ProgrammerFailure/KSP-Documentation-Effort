using System.Collections.Generic;

namespace FinePrint.Utilities;

public class ConstructionPart
{
	public string partName;

	public AvailablePart availablePart;

	public List<string> contractTypes;

	public ConstructionPart(string name, AvailablePart aPart, List<string> contractTypes)
	{
		partName = name;
		availablePart = aPart;
		this.contractTypes = contractTypes;
	}
}
