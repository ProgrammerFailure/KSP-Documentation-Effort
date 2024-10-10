using Contracts;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class MCListItem : MonoBehaviour
{
	public UIListItem container;

	public UIRadioButton radioButton;

	public TextMeshProUGUI title;

	public RawImage logoSprite;

	public UIStateImage difficulty;

	public void Awake()
	{
	}

	public void Setup(Contract contract, string label)
	{
		title.text = label;
		if (contract.Agent != null)
		{
			logoSprite.texture = contract.Agent.LogoScaled;
		}
		if (contract.Prestige == Contract.ContractPrestige.Trivial)
		{
			difficulty.SetState("EASY");
		}
		else if (contract.Prestige == Contract.ContractPrestige.Significant)
		{
			difficulty.SetState("MEDIUM");
		}
		else if (contract.Prestige == Contract.ContractPrestige.Exceptional)
		{
			difficulty.SetState("HARD");
		}
	}
}
