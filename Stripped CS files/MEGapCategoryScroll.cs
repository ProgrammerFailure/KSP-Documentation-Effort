using Expansions.Missions.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MEGapCategoryScroll : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		if ((bool)MissionEditorLogic.Instance)
		{
			MissionEditorLogic.Instance.isMouseOverGAPScroll = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if ((bool)MissionEditorLogic.Instance)
		{
			MissionEditorLogic.Instance.isMouseOverGAPScroll = false;
		}
	}

	public void OnDestroy()
	{
		if ((bool)MissionEditorLogic.Instance)
		{
			MissionEditorLogic.Instance.isMouseOverGAPScroll = false;
		}
	}
}
