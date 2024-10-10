using Contracts;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns35;

public class ScreenContractExistingItem : MonoBehaviour
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI stateText;

	public TextMeshProUGUI prestigeText;

	public Button leftButton;

	public TextMeshProUGUI leftButtonText;

	public Button rightButton;

	public TextMeshProUGUI rightButtonText;

	public TextMeshProUGUI errorText;

	public ScreenContractList contractList;

	public Contract contract { get; set; }

	public void Start()
	{
		leftButton.onClick.AddListener(OnLeftButtonClicked);
		rightButton.onClick.AddListener(OnRightButtonClicked);
	}

	public void Setup(Contract contract, ScreenContractList contractList)
	{
		if (contract == null)
		{
			return;
		}
		this.contract = contract;
		titleText.text = contract.Title;
		stateText.text = contract.LocalizedContractState;
		prestigeText.text = contract.Prestige.displayDescription();
		switch (contract.ContractState)
		{
		case Contract.State.Offered:
			leftButtonText.text = Localizer.Format("#autoLOC_6001205");
			rightButtonText.text = Localizer.Format("#autoLOC_900700");
			if (!contract.CanBeDeclined())
			{
				rightButton.interactable = false;
				errorText.text = Localizer.Format("#autoLOC_901088");
			}
			break;
		case Contract.State.Active:
			leftButtonText.text = Localizer.Format("#autoLOC_7001028");
			rightButtonText.text = Localizer.Format("#autoLOC_129951");
			if (!contract.CanBeCancelled())
			{
				rightButton.interactable = false;
				errorText.text = Localizer.Format("#autoLOC_901087");
			}
			break;
		case Contract.State.OfferExpired:
		case Contract.State.Declined:
		case Contract.State.Cancelled:
		case Contract.State.Completed:
		case Contract.State.DeadlineExpired:
		case Contract.State.Failed:
		case Contract.State.Withdrawn:
			rightButtonText.text = Localizer.Format("#autoLOC_901086");
			leftButton.gameObject.SetActive(value: false);
			break;
		}
		this.contractList = contractList;
	}

	public void SetupError(string errorText)
	{
		contract = null;
		titleText.text = "<color=red>" + errorText + "</color>";
		titleText.enableWordWrapping = false;
		titleText.overflowMode = TextOverflowModes.Ellipsis;
		stateText.text = string.Empty;
		prestigeText.text = string.Empty;
		leftButton.gameObject.SetActive(value: false);
		rightButton.gameObject.SetActive(value: false);
	}

	public void OnLeftButtonClicked()
	{
		if (contract != null)
		{
			switch (contract.ContractState)
			{
			case Contract.State.Active:
				contract.Complete();
				break;
			case Contract.State.Offered:
				contract.Accept();
				break;
			}
			if (contractList != null)
			{
				contractList.SetDirty();
			}
		}
	}

	public void OnRightButtonClicked()
	{
		if (contract != null)
		{
			switch (contract.ContractState)
			{
			case Contract.State.Offered:
				contract.Decline();
				break;
			case Contract.State.Active:
				contract.Cancel();
				break;
			case Contract.State.OfferExpired:
			case Contract.State.Declined:
			case Contract.State.Cancelled:
			case Contract.State.Completed:
			case Contract.State.DeadlineExpired:
			case Contract.State.Failed:
			case Contract.State.Withdrawn:
				ContractSystem.Instance.ContractsFinished.Remove(contract);
				break;
			}
			if (contractList != null)
			{
				contractList.SetDirty();
			}
		}
	}
}
