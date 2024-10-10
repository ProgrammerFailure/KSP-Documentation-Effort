using ns11;
using UnityEngine;
using UnityEngine.UI;

public class DeltaVAppStageInfo : MonoBehaviour
{
	[SerializeField]
	public Button showAllButton;

	[SerializeField]
	public Button hideAllButton;

	[SerializeField]
	public DeltaVAppStageInfoToggle infoTogglePrefab;

	[SerializeField]
	public GridLayoutGroup infoToggles;

	public int infoLineHeight;

	public void Awake()
	{
	}

	public void Start()
	{
		showAllButton.onClick.AddListener(ShowAllClicked);
		hideAllButton.onClick.AddListener(HideAllClicked);
		FillInfoToggles();
	}

	public void OnDestroy()
	{
		showAllButton.onClick.RemoveListener(ShowAllClicked);
		hideAllButton.onClick.RemoveListener(HideAllClicked);
	}

	public void FillInfoToggles()
	{
		DeltaVAppValues deltaVAppValues = DeltaVGlobals.DeltaVAppValues;
		for (int i = 0; i < deltaVAppValues.infoLines.Count; i++)
		{
			DeltaVAppValues.InfoLine info = deltaVAppValues.infoLines[i];
			Object.Instantiate(infoTogglePrefab).Setup(info, infoToggles.gameObject.transform);
		}
	}

	public void ShowAllClicked()
	{
		if (!(StageManager.Instance == null))
		{
			StageManager.Instance.ToggleInfoPanels(showPanels: true);
			StageManager.Instance.usageDV.allStagesShow++;
		}
	}

	public void HideAllClicked()
	{
		if (!(StageManager.Instance == null))
		{
			StageManager.Instance.ToggleInfoPanels(showPanels: false);
			StageManager.Instance.usageDV.allStagesHide++;
		}
	}

	public void SetColumnLayout(bool multiColumn)
	{
		int num = ((!multiColumn) ? 1 : 2);
		float num2 = (infoToggles.transform as RectTransform).sizeDelta.x;
		if (num2 == 0f)
		{
			num2 = (base.transform as RectTransform).rect.width - 10f;
		}
		infoToggles.constraintCount = num;
		infoToggles.cellSize = new Vector2(num2 / (float)num, infoLineHeight);
	}
}
