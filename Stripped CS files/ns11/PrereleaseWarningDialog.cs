using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class PrereleaseWarningDialog : MonoBehaviour
{
	[SerializeField]
	public Button btnContinue;

	public TextMeshProUGUI textBox;

	public void Awake()
	{
		if (!Versioning.isPrerelease)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		textBox.text = Localizer.Format("#autoLOC_1900246", Versioning.version_major + "." + Versioning.version_minor + "." + Versioning.Revision, "http://bugs.kerbalspaceprogram.com/projects/prerelease");
		base.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		btnContinue.onClick.AddListener(OnBtnContinue);
	}

	public void OnBtnContinue()
	{
		Object.Destroy(base.gameObject);
	}
}
