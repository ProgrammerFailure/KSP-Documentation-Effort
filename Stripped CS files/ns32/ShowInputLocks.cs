using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns32;

public class ShowInputLocks : MonoBehaviour
{
	public TextMeshProUGUI text;

	public Button clearButton;

	public void Awake()
	{
		clearButton.onClick.AddListener(OnClearClick);
	}

	public void Update()
	{
		string text = GetText();
		if (this.text.text != text)
		{
			this.text.text = text;
		}
	}

	public void OnClearClick()
	{
		InputLockManager.ClearControlLocks();
	}

	public virtual string GetText()
	{
		return InputLockManager.PrintLockStack();
	}
}
