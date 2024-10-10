using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionBriefingTag : MonoBehaviour
{
	public TMP_Text tagText;

	public Image image;

	public Button removeMe;

	public bool isDummyContent;

	public MissionBriefingTag Spawn(string tag)
	{
		MissionBriefingTag component = Object.Instantiate(this).GetComponent<MissionBriefingTag>();
		component.tagText.text = tag;
		component.removeMe.onClick.AddListener(delegate
		{
			OnRemoveMe(tag);
		});
		return component;
	}

	public MissionBriefingTag Spawn(bool _isDummyContent)
	{
		MissionBriefingTag component = Object.Instantiate(this).GetComponent<MissionBriefingTag>();
		component.isDummyContent = _isDummyContent;
		component.tagText.text = "";
		SetupContent(component);
		return component;
	}

	public void SetupContent(MissionBriefingTag mbt)
	{
		if (mbt.isDummyContent)
		{
			mbt.tagText.enabled = false;
			mbt.image.enabled = false;
		}
		else
		{
			mbt.tagText.enabled = true;
			mbt.image.enabled = true;
		}
	}

	public void OnRemoveMe(string tag)
	{
		GameEvents.Mission.onMissionTagRemoved.Fire(tag);
	}
}
