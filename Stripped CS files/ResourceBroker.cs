public class ResourceBroker : IResourceBroker
{
	public double AmountAvailable(Part part, string resName, double deltaTime, ResourceFlowMode flowMode)
	{
		return AmountAvailable(part, resName.GetHashCode(), deltaTime, flowMode);
	}

	public double AmountAvailable(Part part, int resID, double deltaTime, ResourceFlowMode flowMode)
	{
		if (flowMode == ResourceFlowMode.NULL)
		{
			flowMode = PartResourceLibrary.Instance.GetDefinition(resID).resourceFlowMode;
		}
		part.GetConnectedResourceTotals(resID, flowMode, out var amount, out var _);
		if (resID == PartResourceLibrary.ElectricityHashcode && deltaTime > 0.02 * (double)ResourceSetup.Instance.ResConfig.ECMinScale)
		{
			double num = deltaTime / 0.02;
			return amount * num;
		}
		return amount;
	}

	public double RequestResource(Part part, string resName, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		return RequestResource(part, resName.GetHashCode(), resAmount, deltaTime, flowMode);
	}

	public double RequestResource(Part part, int resID, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		if (flowMode == ResourceFlowMode.NULL)
		{
			flowMode = PartResourceLibrary.Instance.GetDefinition(resID).resourceFlowMode;
		}
		double num = resAmount;
		if (resID == PartResourceLibrary.ElectricityHashcode && deltaTime > 0.02 * (double)ResourceSetup.Instance.ResConfig.ECMinScale)
		{
			double num2 = deltaTime / 0.02;
			num /= num2;
		}
		return part.RequestResource(resID, num, flowMode);
	}

	public double StorageAvailable(Part part, string resName, double deltaTime, ResourceFlowMode flowMode, double FillAmount)
	{
		return StorageAvailable(part, resName.GetHashCode(), deltaTime, flowMode, FillAmount);
	}

	public double StorageAvailable(Part part, int resID, double deltaTime, ResourceFlowMode flowMode, double FillAmount)
	{
		if (flowMode == ResourceFlowMode.NULL)
		{
			flowMode = PartResourceLibrary.Instance.GetDefinition(resID).resourceFlowMode;
		}
		part.GetConnectedResourceTotals(resID, flowMode, out var amount, out var _, FillAmount, pulling: false);
		return amount;
	}

	public double StoreResource(Part part, string resName, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		return StoreResource(part, resName.GetHashCode(), resAmount, deltaTime, flowMode);
	}

	public double StoreResource(Part part, int resID, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		if (flowMode == ResourceFlowMode.NULL)
		{
			flowMode = PartResourceLibrary.Instance.GetDefinition(resID).resourceFlowMode;
		}
		return part.RequestResource(resID, 0.0 - resAmount, flowMode);
	}
}
