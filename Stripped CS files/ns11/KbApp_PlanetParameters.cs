using System.Collections.Generic;
using ns2;
using ns9;
using UnityEngine;

namespace ns11;

public class KbApp_PlanetParameters : KbApp
{
	public delegate bool boolDelegate_KbApp_PlanetParameters(KbApp_PlanetParameters kbapp, MapObject tgt);

	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	public CelestialBody currentBody;

	public static boolDelegate_KbApp_PlanetParameters CallbackActivate;

	public static boolDelegate_KbApp_PlanetParameters CallbackAfterActivate;

	public override void ActivateApp(MapObject target)
	{
		if (CallbackActivate == null || CallbackActivate(this, target))
		{
			currentBody = target.celestialBody;
			appFrame.appName.text = Localizer.Format("#autoLOC_7001301", currentBody.displayName).ToUpper();
			appFrame.scrollList.Clear(destroyElements: true);
			cascadingList = Object.Instantiate(cascadingListPrefab);
			cascadingList.Setup(appFrame.scrollList);
			cascadingList.transform.SetParent(base.transform, worldPositionStays: false);
			UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_462403"), out var button, scaleBg: true);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), CreatePhysicalCharacteristics(), button);
			header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_462406"), out button, scaleBg: true);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), CreateAtmosphericCharacteristics(), button);
		}
		if (CallbackAfterActivate != null)
		{
			CallbackAfterActivate(this, target);
		}
	}

	public List<UIListItem> CreatePhysicalCharacteristics()
	{
		List<UIListItem> list = new List<UIListItem>();
		UIListItem item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462417"), " <color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.Radius / 1000.0, "N0") + " " + Localizer.Format("#autoLOC_7001405") + "</color>");
		list.Add(item);
		item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462420"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(12.566370964050293 * currentBody.Radius * currentBody.Radius, "0.###E+0") + " " + Localizer.Format("#autoLOC_7001402") + "</color>");
		list.Add(item);
		item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462423"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.Mass, "0.###E+0") + " " + Localizer.Format("#autoLOC_7001403") + "</color>");
		list.Add(item);
		item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462426"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.gravParameter, "0.###E+0") + " " + Localizer.Format("#autoLOC_7001404") + "</color>");
		list.Add(item);
		item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462429"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.GeeASL, "0.#####") + " " + Localizer.Format("#autoLOC_7001413") + "</color>");
		list.Add(item);
		item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462432"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(Mathf.Sqrt((float)(2.0 * currentBody.gravParameter / currentBody.Radius)), "0.0") + Localizer.Format("#autoLOC_7001415") + "</color>");
		list.Add(item);
		item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462435"), "<color=#b8f4d1>" + KSPUtil.PrintTime(currentBody.rotationPeriod, 3, explicitPositive: false) + "</color>");
		list.Add(item);
		item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462438"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.sphereOfInfluence / 1000.0, "N0") + " " + Localizer.Format("#autoLOC_7001405") + "</color>");
		list.Add(item);
		return list;
	}

	public List<UIListItem> CreateAtmosphericCharacteristics()
	{
		List<UIListItem> list = new List<UIListItem>();
		UIListItem item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462448"), "<color=#b8f4d1>" + Localizer.Format(currentBody.atmosphere ? "#autoLOC_439855" : "#autoLOC_439856") + "</color>");
		list.Add(item);
		if (currentBody.atmosphere)
		{
			item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462453"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.atmosphereDepth, "N0") + " " + Localizer.Format("#autoLOC_7001411") + "</color>");
			list.Add(item);
			item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462456"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.atmospherePressureSeaLevel * PhysicsGlobals.KpaToAtmospheres, "0.#####") + " " + Localizer.Format("#autoLOC_7001419") + "</color>");
			list.Add(item);
			item = cascadingList.CreateBody(Localizer.Format("#autoLOC_462459"), "<color=#b8f4d1>" + KSPUtil.LocalizeNumber(currentBody.atmosphereTemperatureSeaLevel, "0.##") + " " + Localizer.Format("#autoLOC_7001406") + "</color>");
			list.Add(item);
		}
		return list;
	}

	public override void DisplayApp()
	{
	}

	public override void HideApp()
	{
	}
}
