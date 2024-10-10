using UnityEngine;
using UnityEngine.UI;

namespace ns15;

public class LanguageText : MonoBehaviour, ILanguageKey
{
	public Text text;

	public string key = "";

	public string value = "";

	public string LanguageKey
	{
		get
		{
			return key;
		}
		set
		{
			key = value;
		}
	}

	public string LanguageValue
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public void Reset()
	{
		text = GetComponent<Text>();
	}

	public void Start()
	{
		if (text != null)
		{
			text.text = Language.Instance.Translate(key);
		}
	}
}
