using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns25;

public class DebugScreenCheats : MonoBehaviour
{
	public Toggle hackGravity;

	public Slider hackGravityFactor;

	public TextMeshProUGUI hackGravityText;

	public Toggle pauseOnVesselUnpack;

	public Toggle unbreakableJoints;

	public Toggle noCrashDamage;

	public Toggle ignoreMaxTemperature;

	public Toggle infinitePropellant;

	public Toggle infiniteElectricity;

	public Toggle biomesVisibleInMap;

	public Toggle allowPartClippingInEditors;

	public Toggle nonStrictPartAttachmentOrientationChecks;

	public static DebugScreenCheats Instance { get; set; }

	public void Awake()
	{
		Instance = this;
	}

	public void OnDestroy()
	{
		Instance = null;
	}
}
