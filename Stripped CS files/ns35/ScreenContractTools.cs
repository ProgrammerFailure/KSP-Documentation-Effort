using Contracts;
using UnityEngine;
using UnityEngine.UI;

namespace ns35;

public class ScreenContractTools : MonoBehaviour
{
	public Button resetWeights;

	public Button regenerateCurrent;

	public Button clearCurrent;

	public Button clearFinished;

	public bool interactable;

	public void Start()
	{
		resetWeights.onClick.AddListener(OnResetWeightsClicked);
		regenerateCurrent.onClick.AddListener(OnRegenerateCurrentClicked);
		clearCurrent.onClick.AddListener(OnClearCurrentClicked);
		clearFinished.onClick.AddListener(OnClearFinishedClicked);
		SetInteractable(ContractSystem.Instance != null);
	}

	public void Update()
	{
		if (interactable && ContractSystem.Instance == null)
		{
			SetInteractable(value: false);
		}
		else if (!interactable && ContractSystem.Instance != null)
		{
			SetInteractable(value: true);
		}
	}

	public void SetInteractable(bool value)
	{
		interactable = value;
		resetWeights.interactable = interactable;
		regenerateCurrent.interactable = interactable;
		clearCurrent.interactable = interactable;
		clearFinished.interactable = interactable;
	}

	public void OnResetWeightsClicked()
	{
		ContractSystem.ResetWeights();
	}

	public void OnRegenerateCurrentClicked()
	{
		ContractSystem.Instance.RebuildContracts();
	}

	public void OnClearCurrentClicked()
	{
		ContractSystem.Instance.ClearContractsCurrent();
	}

	public void OnClearFinishedClicked()
	{
		ContractSystem.Instance.ClearContractsFinished();
	}
}
