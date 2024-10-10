using TMPro;
using UnityEngine.UI;

public class UIPartActionButton : UIPartActionEventItem
{
	public TextMeshProUGUI label;

	public Button button;

	public void Awake()
	{
		button.onClick.AddListener(OnClick);
	}

	public override void UpdateItem()
	{
		if (label != null)
		{
			if (evt.guiName != string.Empty)
			{
				label.text = evt.guiName;
			}
			else
			{
				label.text = evt.name;
			}
		}
	}

	public void OnClick()
	{
		Mouse.Left.ClearMouseState();
		if (isModule)
		{
			if (!(part != null) || !(partModule != null) || evt == null)
			{
				return;
			}
			if (!UIPartActionController.Instance.ItemListContainsCounterparts(part))
			{
				int count = part.symmetryCounterparts.Count;
				for (int i = 0; i < count; i++)
				{
					part.symmetryCounterparts[i].Events.Send(evt.id);
				}
			}
			evt.Invoke();
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			if (!(part != null) || evt == null)
			{
				return;
			}
			if (EditorLogic.SelectedPart != null)
			{
				int j = 0;
				for (int count2 = EditorLogic.SelectedPart.symmetryCounterparts.Count; j < count2; j++)
				{
					EditorLogic.SelectedPart.symmetryCounterparts[j].Events.Send(evt.id);
				}
			}
			evt.Invoke();
		}
		else
		{
			evt.Invoke();
		}
	}
}
