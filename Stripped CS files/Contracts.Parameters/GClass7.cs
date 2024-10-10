using System;

namespace Contracts.Parameters;

[Serializable]
public class GClass7 : ContractParameter
{
	public override string GetHashString()
	{
		return "OR";
	}

	public override void OnParameterStateChange(ContractParameter p)
	{
		int num = 0;
		while (true)
		{
			if (num < base.ParameterCount)
			{
				if (GetParameter(num).State == ParameterState.Complete)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		SetComplete();
	}
}
