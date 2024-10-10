using UnityEngine;

namespace ns2;

public class UISelectableListItem : MonoBehaviour
{
	public int index;

	public int Index => index;

	public virtual void Select()
	{
	}

	public virtual void Deselect()
	{
	}

	public virtual void SetupListItem(UISelectableList selectableList, int index)
	{
		this.index = index;
	}
}
