using Expansions;
using ns9;
using TMPro;
using UnityEngine;

namespace ns32;

public class VersionInfo : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI version_text;

	public void Start()
	{
		if (version_text != null)
		{
			version_text.text = Versioning.GetVersionStringFull() + " " + Localizer.CurrentLanguage;
			version_text.text += ExpansionsLoader.GetInstalledExpansionsString();
		}
	}
}
