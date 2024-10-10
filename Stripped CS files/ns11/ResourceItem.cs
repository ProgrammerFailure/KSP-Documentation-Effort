using ns12;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class ResourceItem : MonoBehaviour
{
	public UIStateButton btnStage;

	public Button background;

	public Slider resourceBar;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI amountText;

	public TextMeshProUGUI maxText;

	public TextMeshProUGUI deltaText;

	public PointerEnterExitHandler hoverHandler;

	public double vesselResourceTotal;

	public double vesselResourceCurrent;

	public float vesselResourcePrevious;

	public int resourceID;

	public float delta;

	public double previousAmount;

	public bool isSelected;

	public bool mouseOver;

	public bool showStage;

	public float deltaSmoothed;

	public double resourceValue;

	public bool firstTime;

	public PartSet stageSet;

	public TooltipController_Text tooltip;

	public void Awake()
	{
		background.onClick.AddListener(MouseInput_click);
		hoverHandler.onPointerEnter.AddListener(MouseInput_pointerEnter);
		hoverHandler.onPointerExit.AddListener(MouseInput_pointerExit);
		isSelected = false;
		deltaSmoothed = 0f;
	}

	public void Setup(PartResourceDefinition resource, bool showStage, PartSet set)
	{
		this.showStage = showStage;
		nameText.text = resource.displayName.LocalizeRemoveGender();
		resourceID = resource.id;
		tooltip = base.gameObject.GetComponent<TooltipController_Text>();
		if (tooltip != null)
		{
			tooltip.SetText(resource.displayName.LocalizeRemoveGender());
		}
		stageSet = set;
		firstTime = true;
	}

	public void FixedUpdate()
	{
		if (FlightDriver.Pause || !ResourceDisplay.Instance.panelEnabled)
		{
			return;
		}
		vesselResourceTotal = 0.0;
		vesselResourceCurrent = 0.0;
		if (showStage)
		{
			UpdateStaged();
		}
		else
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			bool flag = activeVessel.isEVA && activeVessel.evaController != null;
			PartResource partResource = null;
			if (flag && activeVessel.evaController.PropellantResource != null && activeVessel.evaController.PropellantResource.info.id == resourceID)
			{
				vesselResourceTotal += activeVessel.evaController.PropellantResource.maxAmount;
				vesselResourceCurrent += activeVessel.evaController.PropellantResource.amount;
				partResource = activeVessel.evaController.PropellantResource;
			}
			int count = activeVessel.parts.Count;
			while (count-- > 0)
			{
				PartResource partResource2 = activeVessel.parts[count].Resources.Get(resourceID);
				if (partResource2 != null && (!flag || partResource == null || partResource2.info != partResource.info))
				{
					vesselResourceTotal += partResource2.maxAmount;
					vesselResourceCurrent += partResource2.amount;
				}
			}
		}
		resourceValue = vesselResourceCurrent / vesselResourceTotal;
		if (!firstTime)
		{
			delta = (float)(previousAmount - vesselResourceCurrent) * (1f / Time.fixedDeltaTime);
			deltaSmoothed = Mathf.Lerp(deltaSmoothed, delta, Time.fixedDeltaTime * 50f);
		}
		else
		{
			if (tooltip != null && !nameText.isTextTruncated)
			{
				tooltip.SetText(string.Empty);
				tooltip.enabled = false;
			}
			delta = 0f;
			deltaSmoothed = 0f;
			firstTime = false;
		}
		previousAmount = vesselResourceCurrent;
	}

	public void UpdateStaged()
	{
		stageSet.GetConnectedResourceTotals(resourceID, out vesselResourceCurrent, out vesselResourceTotal, pulling: true);
	}

	public void Update()
	{
		resourceBar.value = (float)resourceValue;
		if (vesselResourceCurrent > 100.0)
		{
			amountText.text = "<color=#000000>" + KSPUtil.LocalizeNumber(vesselResourceCurrent, "F0") + "</color>";
		}
		else
		{
			amountText.text = "<color=#000000>" + KSPUtil.LocalizeNumber(vesselResourceCurrent, "F2") + "</color>";
		}
		if (vesselResourceTotal > 100.0)
		{
			maxText.text = "<color=#000000>" + KSPUtil.LocalizeNumber(vesselResourceTotal, "F0") + "</color>";
		}
		else
		{
			maxText.text = "<color=#000000>" + KSPUtil.LocalizeNumber(vesselResourceTotal, "F2") + "</color>";
		}
		if (deltaSmoothed == 0f)
		{
			deltaText.text = "<color=#000000>(0)</color>";
		}
		else if (deltaSmoothed < 0f)
		{
			deltaText.text = "<color=#110000>(-" + KSPUtil.LocalizeNumber(deltaSmoothed, "F2") + ")</color>";
		}
		else
		{
			deltaText.text = "<color=#000011>(" + KSPUtil.LocalizeNumber(deltaSmoothed, "F2") + ")</color>";
		}
	}

	public void MouseInput_click()
	{
		isSelected = !isSelected;
		btnStage.SetState(isSelected ? 1 : 0);
		ResourceDisplay.Instance.SetItemSelection(this, isSelected);
	}

	public void MouseInput_pointerEnter(PointerEventData eventData)
	{
		if (!mouseOver)
		{
			mouseOver = true;
			if (!isSelected)
			{
				ResourceDisplay.Instance.SetItemSelection(this, show: true);
			}
		}
	}

	public void MouseInput_pointerExit(PointerEventData eventData)
	{
		if (mouseOver)
		{
			mouseOver = false;
			if (!isSelected)
			{
				ResourceDisplay.Instance.SetItemSelection(this, show: false);
			}
		}
	}
}
