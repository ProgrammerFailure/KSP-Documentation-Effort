using TMPro;
using UnityEngine;

namespace ns26;

public class DebugScreenInputDouble : MonoBehaviour
{
	public TextMeshProUGUI labelText;

	public TMP_InputField inputField;

	public string label = "";

	public double defaultValue;

	public double value;

	public double Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
			inputField.text = value.ToString();
		}
	}

	public void Awake()
	{
		inputField.onValueChanged.AddListener(OnInputChanged);
	}

	public void Start()
	{
		inputField.text = value.ToString();
	}

	public virtual void OnInputChanged(string state)
	{
		double result = 0.0;
		if (double.TryParse(state, out result))
		{
			value = result;
		}
	}
}
