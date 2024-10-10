using TMPro;
using UnityEngine;

namespace ns32;

public class ScreenDeltaVPropellantInfo : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI propellantName;

	[SerializeField]
	public TextMeshProUGUI amountNumber;

	[SerializeField]
	public TextMeshProUGUI engineBurnsNumber;

	public DeltaVPropellantInfo propellantInfo;

	public void UpdateData(DeltaVPropellantInfo propellantInfo, CalcType type)
	{
		this.propellantInfo = propellantInfo;
		propellantName.text = propellantInfo.propellant.displayName;
		amountNumber.text = propellantInfo.amountAvailable.ToString("N2");
		if ((uint)type > 1u && type == CalcType.Actual)
		{
			if (propellantInfo.amountPerSecondCurrentThrottle <= 0.0)
			{
				engineBurnsNumber.text = propellantInfo.amountPerSecondSetThrottle.ToString("N4");
			}
			else
			{
				engineBurnsNumber.text = propellantInfo.amountPerSecondCurrentThrottle.ToString("N4");
			}
		}
		else
		{
			engineBurnsNumber.text = propellantInfo.amountPerSecondSetThrottle.ToString("N4");
		}
	}
}
