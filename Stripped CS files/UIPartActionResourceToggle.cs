using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPartActionResourceToggle : MonoBehaviour
{
	public delegate void PartStatus(PartResource pR, bool b);

	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldStatus;

	public Toggle toggle;

	public PartResource partResource;

	public PartStatus sendPartStatus;

	public static string cacheAutoLOC_439839;

	public static string cacheAutoLOC_439840;

	public void Awake()
	{
		CacheLocalStrings();
	}

	public void OnDestroy()
	{
		toggle.onValueChanged.RemoveListener(PartStatusToggle);
	}

	public void InitializeItem(string displayName, PartResource resource, bool active)
	{
		UpdateItemName(displayName);
		partResource = resource;
		toggle.isOn = active;
		toggle.onValueChanged.AddListener(PartStatusToggle);
	}

	public void UpdateItemName(string name)
	{
		fieldName.text = name;
		UpdateStatusText();
	}

	public void UpdateItemNameAndStatus(string name, bool status)
	{
		fieldName.text = name;
		UpdateStatusTextAndStatus(status);
	}

	public void UpdateStatusText()
	{
		if (toggle.isOn)
		{
			fieldStatus.text = cacheAutoLOC_439839;
		}
		else
		{
			fieldStatus.text = cacheAutoLOC_439840;
		}
	}

	public void UpdateStatusTextAndStatus(bool status)
	{
		if (status)
		{
			toggle.isOn = true;
			fieldStatus.text = cacheAutoLOC_439839;
		}
		else
		{
			toggle.isOn = false;
			fieldStatus.text = cacheAutoLOC_439840;
		}
	}

	public void PartStatusToggle(bool b)
	{
		sendPartStatus(partResource, toggle.isOn);
		UpdateStatusText();
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_439839 = Localizer.Format("#autoLOC_439839");
		cacheAutoLOC_439840 = Localizer.Format("#autoLOC_439840");
	}
}
