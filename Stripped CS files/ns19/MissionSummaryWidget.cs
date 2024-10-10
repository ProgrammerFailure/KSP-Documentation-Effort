using ns11;
using TMPro;
using UnityEngine;

namespace ns19;

public class MissionSummaryWidget : MonoBehaviour
{
	public RectTransform rTrf;

	public MissionRecoveryDialog host;

	[SerializeField]
	public TextMeshProUGUI header;

	public RectTransform RTrf => rTrf;

	public void Init(MissionRecoveryDialog host)
	{
		this.host = host;
		rTrf = GetComponent<RectTransform>();
	}
}
