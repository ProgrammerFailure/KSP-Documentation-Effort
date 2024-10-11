namespace KSP.UI;

public interface IPinnableTooltipController : ITooltipController
{
	void OnTooltipPinned();

	void OnTooltipUnpinned();

	bool IsPinned();

	void Unpin();
}
