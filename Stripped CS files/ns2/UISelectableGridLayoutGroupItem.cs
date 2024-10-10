using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Selectable))]
public class UISelectableGridLayoutGroupItem : MonoBehaviour
{
	public Color colorSelected = Color.green;

	public Color colorDeselected = Color.gray;

	public Transform itemParent;

	public int mySetIndex;

	public bool groupAction;

	public int index;

	public Selectable sel;

	public int Index => index;

	public Selectable SelectableComponent => sel;

	public virtual void Select()
	{
		sel.image.color = colorSelected;
	}

	public virtual void Deselect()
	{
		sel.image.color = colorDeselected;
	}

	public virtual void Setup(UISelectableGridLayoutGroup selectableGroup, int index)
	{
		this.index = index;
		sel = GetComponent<Selectable>();
		Button button = sel as Button;
		if (button != null)
		{
			button.onClick.AddListener(delegate
			{
				selectableGroup.ClickItem(index);
			});
		}
	}
}
