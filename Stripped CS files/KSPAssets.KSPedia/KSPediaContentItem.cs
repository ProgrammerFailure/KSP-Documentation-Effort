using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSPAssets.KSPedia;

public class KSPediaContentItem : MonoBehaviour
{
	public TextMeshProUGUI text;

	public Button btn;

	public string screenName;

	public void Setup(string text, KSPediaDatabase.Screen screen)
	{
		this.text.text = text;
		if (screen != null)
		{
			screenName = screen.name;
			btn.onClick.AddListener(OnClick);
			btn.interactable = true;
		}
		else
		{
			btn.interactable = false;
		}
	}

	public virtual void OnClick()
	{
		Debug.Log("LOL?");
		KSPediaController.Instance.ShowScreen(screenName);
	}
}
