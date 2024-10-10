using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

public class DialogMouseEnterControlLock : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public string lockName = "DialogMouseEnterControlLock";

	public ulong locktype = 1152921504606846975uL;

	public void SetLockType(ControlTypes type)
	{
		locktype = (ulong)type;
	}

	public void SetLockName(string name)
	{
		lockName = name;
	}

	public void Setup(ControlTypes type, string name)
	{
		SetLockType(type);
		SetLockName(name);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		InputLockManager.SetControlLock((ControlTypes)locktype, lockName);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		InputLockManager.RemoveControlLock(lockName);
	}

	public void OnDestroy()
	{
		InputLockManager.RemoveControlLock(lockName);
	}
}
