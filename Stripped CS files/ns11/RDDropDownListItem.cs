using ns2;
using TMPro;
using UnityEngine;

namespace ns11;

[RequireComponent(typeof(UIListItem))]
public class RDDropDownListItem : MonoBehaviour
{
	public TextMeshProUGUI header;

	public TextMeshProUGUI description;

	public UIRadioButton radioButton;

	public void Setup(string header, string description)
	{
		this.header.text = header;
		this.description.text = description;
	}
}
