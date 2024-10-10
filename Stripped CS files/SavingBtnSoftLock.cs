using UnityEngine;
using UnityEngine.EventSystems;

public class SavingBtnSoftLock : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK, "Saving");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		InputLockManager.RemoveControlLock("Saving");
	}
}
