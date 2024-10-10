using ns2;
using ns9;
using TMPro;
using UnityEngine.EventSystems;

namespace ns11;

public class EditorActionPartReset : UISelectableGridLayoutGroupItem, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, ISubmitHandler
{
	public TextMeshProUGUI text;

	public EditorActionPartSelector selector { get; set; }

	public void Setup(EditorActionPartSelector selector)
	{
		if (selector == null)
		{
			text.text = Localizer.Format("#autoLOC_7000071");
		}
		else
		{
			text.text = Localizer.Format("#autoLOC_900305");
		}
		this.selector = selector;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		EditorActionGroups.Instance.ResetPart(selector);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (selector != null)
		{
			selector.HighLight(highLight: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (selector != null)
		{
			selector.HighLight(highLight: false);
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		EditorActionGroups.Instance.ResetPart(selector);
	}
}
