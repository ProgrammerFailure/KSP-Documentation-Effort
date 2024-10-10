using UnityEngine;
using UnityEngine.EventSystems;

public class PieChartSlice : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	public PieChart.Slice slice;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (slice.onTap != null)
		{
			slice.onTap(slice);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (slice.onOver != null)
		{
			slice.onOver(slice);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (slice.onExit != null)
		{
			slice.onExit(slice);
		}
	}
}
