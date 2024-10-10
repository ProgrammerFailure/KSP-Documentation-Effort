using System;

namespace Contracts.Parameters;

[Serializable]
public class GClass8 : ContractParameter
{
	public override string GetHashString()
	{
		return "XOR";
	}

	public override void OnParameterStateChange(ContractParameter p)
	{
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < base.ParameterCount)
			{
				if (GetParameter(num2).State == ParameterState.Complete)
				{
					if (num > 0)
					{
						break;
					}
					num++;
				}
				num2++;
				continue;
			}
			if (num == 1)
			{
				SetComplete();
			}
			return;
		}
		SetIncomplete();
	}
}
