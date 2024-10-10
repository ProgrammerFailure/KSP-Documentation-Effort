using ns9;
using UnityEngine;

public class InternalSpeed : InternalModule
{
	[KSPField]
	public string buttonName = "toggleButton";

	[KSPField]
	public bool useToggleButton = true;

	[KSPField]
	public bool surfaceMode = true;

	[KSPField]
	public string textTransform = "textLabel";

	[KSPField]
	public float textSize = 0.1333333f;

	[KSPField]
	public string textFont = "Arial";

	[KSPField]
	public string textAlign = "Left";

	[KSPField]
	public bool textWrapping;

	[KSPField]
	public Color textColor = new Color(0.8627f, 0.8627f, 0.8627f, 1f);

	public Transform buttonObject;

	public Transform textObjectTransform;

	public InternalText textObject;

	public static string cacheAutoLOC_7003234;

	public static string cacheAutoLOC_7003235;

	public static string cacheAutoLOC_7003236;

	public static string cacheAutoLOC_7003237;

	public static string cacheAutoLOC_180095;

	public static string cacheAutoLOC_180098;

	public static string cacheAutoLOC_180103;

	public override void OnAwake()
	{
		if (useToggleButton && buttonObject == null)
		{
			buttonObject = internalProp.FindModelTransform(buttonName);
			useToggleButton = buttonObject != null;
		}
		if (useToggleButton && buttonObject != null)
		{
			InternalButton.Create(buttonObject.gameObject).OnDown(OnButtonTap);
		}
		if (textObjectTransform == null)
		{
			textObjectTransform = internalProp.FindModelTransform(textTransform);
			if (textObjectTransform == null)
			{
				Debug.Log("InternalSpeed: Cannot find textTransform '" + textTransform + "'");
				return;
			}
		}
		if (textObjectTransform != null && InternalComponents.Instance != null)
		{
			textObject = InternalComponents.Instance.CreateText(textFont, textSize, textObjectTransform, "", textColor, textWrapping, textAlign);
		}
	}

	public void OnButtonTap()
	{
		FlightGlobals.CycleSpeedModes();
	}

	public override void OnUpdate()
	{
		if (textObject != null)
		{
			string arg = cacheAutoLOC_7003234;
			double num = 0.0;
			switch (FlightGlobals.speedDisplayMode)
			{
			case FlightGlobals.SpeedDisplayModes.Orbit:
				arg = cacheAutoLOC_7003235;
				num = FlightGlobals.ship_obtSpeed;
				break;
			case FlightGlobals.SpeedDisplayModes.Surface:
				arg = cacheAutoLOC_7003236;
				num = FlightGlobals.ship_srfSpeed;
				break;
			case FlightGlobals.SpeedDisplayModes.Target:
				arg = cacheAutoLOC_7003237;
				num = FlightGlobals.ship_tgtSpeed;
				break;
			}
			string arg2 = cacheAutoLOC_180095;
			if (num >= 10000000.0)
			{
				arg2 = cacheAutoLOC_180098;
				num /= 1000000.0;
			}
			if (num >= 10000.0)
			{
				arg2 = cacheAutoLOC_180103;
				num /= 1000.0;
			}
			string format = ((num >= 1000.0) ? "N0" : ((num >= 100.0) ? "F1" : "F2"));
			textObject.text.text = $"{arg}: {KSPUtil.LocalizeNumber(num, format)} {arg2}";
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("textAlign"))
		{
			textAlign = node.GetValue("textAlign");
		}
		if (node.HasValue("textWrapping"))
		{
			node.TryGetValue("textWrapping", ref textWrapping);
		}
		if (node.HasValue("textColor"))
		{
			node.TryGetValue("textColor", ref textColor);
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7003234 = Localizer.Format("#autoLOC_7003234");
		cacheAutoLOC_7003235 = Localizer.Format("#autoLOC_7003235");
		cacheAutoLOC_7003236 = Localizer.Format("#autoLOC_7003236");
		cacheAutoLOC_7003237 = Localizer.Format("#autoLOC_7003237");
		cacheAutoLOC_180095 = Localizer.Format("#autoLOC_180095");
		cacheAutoLOC_180098 = Localizer.Format("#autoLOC_180098");
		cacheAutoLOC_180103 = Localizer.Format("#autoLOC_180103");
	}
}
