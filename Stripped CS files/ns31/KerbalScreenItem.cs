using TMPro;
using UnityEngine;

namespace ns31;

public class KerbalScreenItem : MonoBehaviour
{
	public TextMeshProUGUI nameText;

	public TextMeshProUGUI traitText;

	public TextMeshProUGUI typeText;

	public TextMeshProUGUI levelText;

	public TextMeshProUGUI experienceText;

	public TextMeshProUGUI rosterStatusText;

	public ProtoCrewMember ProtoCrewMember { get; set; }

	public void Setup(ProtoCrewMember pcm)
	{
		ProtoCrewMember = pcm;
		nameText.text = pcm.name;
		typeText.text = pcm.GetLocalizedType();
		traitText.text = pcm.GetLocalizedTrait();
		levelText.text = pcm.experienceLevel.ToString();
		experienceText.text = pcm.experience.ToString();
		rosterStatusText.text = pcm.GetLocalizedStatus();
	}
}
