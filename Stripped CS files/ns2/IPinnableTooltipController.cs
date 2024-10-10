namespace ns2;

public interface IPinnableTooltipController : ITooltipController
{
	void OnTooltipPinned();

	void OnTooltipUnpinned();

	bool IsPinned();

	void Unpin();
}
