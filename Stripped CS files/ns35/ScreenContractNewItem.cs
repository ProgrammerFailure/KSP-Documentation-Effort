using System;
using Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns35;

public class ScreenContractNewItem : MonoBehaviour
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI assemblytext;

	public Button trivialButton;

	public Button significantButton;

	public Button exceptionalButton;

	public Type ContractType { get; set; }

	public void Start()
	{
		trivialButton.onClick.AddListener(OnTrivialClicked);
		significantButton.onClick.AddListener(OnSignificantClicked);
		exceptionalButton.onClick.AddListener(OnExceptionalClicked);
	}

	public void Setup(Type contractType)
	{
		if (!(contractType == null))
		{
			ContractType = contractType;
			titleText.text = contractType.Name;
			assemblytext.text = contractType.Assembly.FullName;
		}
	}

	public static bool GenerateContract(Type type, Contract.ContractPrestige prestige)
	{
		if (ContractSystem.Instance == null)
		{
			Debug.LogError("Contract of type '" + type.Name + "' could not be generated.");
			return false;
		}
		Contract contract = ContractSystem.Instance.GenerateContract(UnityEngine.Random.Range(int.MinValue, int.MaxValue), prestige, type);
		if (contract != null)
		{
			ContractSystem.Instance.Contracts.Add(contract);
			contract.Offer();
			return true;
		}
		Debug.LogError("Contract of type '" + type.Name + "' could not be generated.");
		return false;
	}

	public void SetupError(string errorText)
	{
		ContractType = null;
		titleText.text = "<color=red>" + errorText + "</color>";
		titleText.enableWordWrapping = false;
		titleText.overflowMode = TextOverflowModes.Overflow;
		assemblytext.text = string.Empty;
		trivialButton.gameObject.SetActive(value: false);
		significantButton.gameObject.SetActive(value: false);
		exceptionalButton.gameObject.SetActive(value: false);
	}

	public void OnTrivialClicked()
	{
		if (!(ContractType == null))
		{
			GenerateContract(ContractType, Contract.ContractPrestige.Trivial);
		}
	}

	public void OnSignificantClicked()
	{
		if (!(ContractType == null))
		{
			GenerateContract(ContractType, Contract.ContractPrestige.Significant);
		}
	}

	public void OnExceptionalClicked()
	{
		if (!(ContractType == null))
		{
			GenerateContract(ContractType, Contract.ContractPrestige.Exceptional);
		}
	}
}
