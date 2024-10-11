using System.Runtime.CompilerServices;

public class ResourceBroker : IResourceBroker
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceBroker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double AmountAvailable(Part part, string resName, double deltaTime, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double AmountAvailable(Part part, int resID, double deltaTime, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RequestResource(Part part, string resName, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RequestResource(Part part, int resID, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double StorageAvailable(Part part, string resName, double deltaTime, ResourceFlowMode flowMode, double FillAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double StorageAvailable(Part part, int resID, double deltaTime, ResourceFlowMode flowMode, double FillAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double StoreResource(Part part, string resName, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double StoreResource(Part part, int resID, double resAmount, double deltaTime, ResourceFlowMode flowMode)
	{
		throw null;
	}
}
