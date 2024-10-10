using TMPro;
using UnityEngine;

namespace ns32;

public class ScreenDeltaVPartsListInfo : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI partsListText;

	public void UpdateData(string partsText)
	{
		partsListText.text = partsText;
	}
}
