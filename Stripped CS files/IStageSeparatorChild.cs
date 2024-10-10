using System.Collections.Generic;

public interface IStageSeparatorChild
{
	bool PartDetaches(out List<Part> decoupledChildParts);

	bool IsEnginePlate();
}
