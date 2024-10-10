using ns2;
using TMPro;
using UnityEngine;

namespace CommNet;

public class Tooltip_SignalStrengthItem : MonoBehaviour
{
	public UIStateImage signalState;

	public TextMeshProUGUI label;

	public void Setup(SignalStrength signal, string text)
	{
		if (label.text != text)
		{
			label.text = text;
		}
		signalState.SetState((int)signal);
	}
}
