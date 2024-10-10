public interface IResourceBroker
{
	double AmountAvailable(Part part, string resName, double deltaTime, ResourceFlowMode flowMode);

	double RequestResource(Part part, string resName, double resAmount, double deltaTime, ResourceFlowMode flowMode);

	double StorageAvailable(Part part, string resName, double deltaTime, ResourceFlowMode flowMode, double FillAmount);

	double StoreResource(Part part, string resName, double resAmount, double deltaTime, ResourceFlowMode flowMode);

	double AmountAvailable(Part part, int resID, double deltaTime, ResourceFlowMode flowMode);

	double RequestResource(Part part, int resID, double resAmount, double deltaTime, ResourceFlowMode flowMode);

	double StorageAvailable(Part part, int resID, double deltaTime, ResourceFlowMode flowMode, double FillAmount);

	double StoreResource(Part part, int resID, double resAmount, double deltaTime, ResourceFlowMode flowMode);
}
