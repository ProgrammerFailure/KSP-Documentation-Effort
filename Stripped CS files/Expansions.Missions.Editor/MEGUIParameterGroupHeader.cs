using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIParameterGroupHeader : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Button resetButton;

	public CanvasGroup resetCanvasGroup;

	public void Awake()
	{
		if (resetButton != null)
		{
			resetCanvasGroup = resetButton.GetComponent<CanvasGroup>();
		}
	}

	public void Start()
	{
		if (resetButton != null)
		{
			resetCanvasGroup.alpha = 0f;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (resetButton != null)
		{
			resetCanvasGroup.alpha = 1f;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (resetButton != null)
		{
			resetCanvasGroup.alpha = 0f;
		}
	}
}
