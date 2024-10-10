using UnityEngine;

namespace ns2;

public class UIListToggleController : MonoBehaviour
{
	public UIList[] lists;

	public int startList;

	public int currentList;

	public void Start()
	{
		ActivateList(startList);
	}

	public void ActivateList(int index)
	{
		int num = lists.Length;
		while (num-- > 0)
		{
			lists[num].SetActive(index == num);
		}
		currentList = index;
	}

	public void RefreshCurrent()
	{
		ActivateList(currentList);
		lists[currentList].Refresh();
	}
}
