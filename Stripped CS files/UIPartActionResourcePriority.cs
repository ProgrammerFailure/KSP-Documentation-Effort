using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_Label]
public class UIPartActionResourcePriority : UIPartActionItem
{
	[SerializeField]
	public TextMeshProUGUI txtPriority;

	[SerializeField]
	public TextMeshProUGUI txtPriorityOffset;

	[SerializeField]
	public Button btnDec;

	[SerializeField]
	public Button btnInc;

	[SerializeField]
	public Button btnReset;

	public int resPriority = int.MaxValue;

	public int resPriorityOffset = int.MaxValue;

	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		SetupItem(window, part, null, scene, null);
		btnDec.onClick.AddListener(OnDecClick);
		btnInc.onClick.AddListener(OnIncClick);
		btnReset.onClick.AddListener(OnResetClick);
	}

	public void Awake()
	{
	}

	public override void UpdateItem()
	{
		int resourcePriority = part.GetResourcePriority();
		if (resPriority != resourcePriority)
		{
			resPriority = resourcePriority;
			txtPriority.text = resPriority.ToString();
		}
		int resourcePriorityOffset = part.resourcePriorityOffset;
		if (resPriorityOffset != resourcePriorityOffset)
		{
			resPriorityOffset = resourcePriorityOffset;
			txtPriorityOffset.text = "(";
			if (resourcePriorityOffset > 0)
			{
				txtPriorityOffset.text += "+";
			}
			TextMeshProUGUI textMeshProUGUI = txtPriorityOffset;
			textMeshProUGUI.text = textMeshProUGUI.text + resPriorityOffset + ")";
		}
	}

	public void OnDecClick()
	{
		if (GameSettings.MODIFIER_KEY.GetKey())
		{
			part.ChangeResourcePriority(-10);
		}
		else
		{
			part.ChangeResourcePriority(-1);
		}
	}

	public void OnIncClick()
	{
		if (GameSettings.MODIFIER_KEY.GetKey())
		{
			part.ChangeResourcePriority(10);
		}
		else
		{
			part.ChangeResourcePriority(1);
		}
	}

	public void OnResetClick()
	{
		part.ResetPri();
	}
}
