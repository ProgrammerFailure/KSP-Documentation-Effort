using System;
using System.Collections.Generic;
using ns23;
using ns9;
using UnityEngine;
using Vectrosity;

public class PatchRendering
{
	public enum RelativityMode
	{
		LOCAL_TO_BODIES,
		LOCAL_AT_SOI_ENTRY_UT,
		LOCAL_AT_SOI_EXIT_UT,
		RELATIVE,
		DYNAMIC
	}

	public MapNode mnAp;

	public MapNode mnPe;

	public MapNode mnCB;

	public MapNode mnCBatUT;

	public MapNode mnEnd;

	public MapNode mnNextStart;

	public MapObject moAp;

	public MapObject moPe;

	public MapObject moCB;

	public MapObject moCBatUT;

	public MapObject moEnd;

	public MapObject moNextStart;

	public CelestialBody currentMainBody;

	public CelestialBody relativeTo;

	public Orbit patch;

	public Trajectory trajectory;

	public RelativityMode relativityMode = RelativityMode.RELATIVE;

	public Vector3d pe;

	public Vector3d ap;

	public Vector3d st;

	public Vector3d end;

	public Vector3d cb;

	public double UTpe;

	public double UTap;

	public double UTcb;

	public Vector3d[] tPoints;

	public Vector3[] scaledTPoints;

	public List<Vector3> vectorPoints;

	public Color[] colors;

	public Color patchColor;

	public Color nodeColor;

	public List<Color32> vectorColours;

	public VectorLine vectorLine;

	public Material lineMaterial;

	public int samples;

	public int interpolations;

	public double dynamicLinearity = 2.0;

	public string vectorName;

	public float lineWidth;

	public bool smoothLineTexture;

	public bool draw3dLines;

	public bool visible;

	public bool hasMapNodes;

	public PatchedConicRenderer pcrCache;

	public double trailOffset;

	public double eccOffset;

	public float twkOffset;

	public List<Color32> VectorColours
	{
		get
		{
			if (vectorColours == null)
			{
				vectorColours = new List<Color32>(samples * interpolations);
				for (int i = 0; i < samples; i++)
				{
					for (int j = 0; j < interpolations; j++)
					{
						vectorColours.Add(patchColor);
					}
				}
			}
			return vectorColours;
		}
	}

	public bool enabled => vectorLine != null;

	public PatchRendering(string name, int nSamples, int nInterpolations, Orbit patchRef, Material lineMat, float LineWidth, bool smoothTexture, PatchedConicRenderer pcr)
	{
		tPoints = new Vector3d[nSamples];
		scaledTPoints = new Vector3[nSamples];
		vectorPoints = new List<Vector3>(samples * interpolations * 2);
		colors = new Color[nSamples];
		trajectory = new Trajectory();
		samples = nSamples;
		interpolations = nInterpolations;
		patch = patchRef;
		lineMaterial = lineMat;
		relativeTo = patchRef.referenceBody;
		vectorName = name;
		lineWidth = LineWidth;
		smoothLineTexture = smoothTexture;
		pcrCache = pcr;
		visible = true;
	}

	public void SetColor(Color color)
	{
		nodeColor = color;
		patchColor = (nodeColor * 0.5f).smethod_0(nodeColor.a);
	}

	public void Terminate()
	{
		DestroyVector();
		DestroyUINodes();
	}

	public void DestroyUINodes()
	{
		if (hasMapNodes)
		{
			DetachUINodes();
			DestroyMapObjects();
			hasMapNodes = false;
		}
	}

	public void CreateMapObjects()
	{
		if (moAp == null)
		{
			moAp = MapObject.Create(vectorName + " Apoapsis", vectorName + " Apoapsis", patch, MapObject.ObjectType.Apoapsis);
			if (pcrCache != null)
			{
				moAp.vesselRef = pcrCache.vessel;
			}
		}
		if (moPe == null)
		{
			moPe = MapObject.Create(vectorName + " Periapsis", vectorName + " Apoapsis", patch, MapObject.ObjectType.Periapsis);
			if (pcrCache != null)
			{
				moPe.vesselRef = pcrCache.vessel;
			}
		}
		if (moCB == null)
		{
			moCB = MapObject.Create(vectorName + " Reference Body", vectorName + " Apoapsis", patch, MapObject.ObjectType.Generic);
		}
		if (moCBatUT == null)
		{
			moCBatUT = MapObject.Create(vectorName + "Reference Body at Patch End", vectorName + " Apoapsis", patch, MapObject.ObjectType.CelestialBodyAtUT);
		}
		if (moEnd == null)
		{
			moEnd = MapObject.Create(vectorName + " Patch End", vectorName + " Apoapsis", patch, MapObject.ObjectType.PatchTransition);
			if (pcrCache != null)
			{
				moEnd.vesselRef = pcrCache.vessel;
			}
		}
		if (moNextStart == null)
		{
			moNextStart = MapObject.Create(vectorName + " Next Patch Start", vectorName + " Apoapsis", patch, MapObject.ObjectType.PatchTransition);
			if (pcrCache != null)
			{
				moNextStart.vesselRef = pcrCache.vessel;
			}
		}
	}

	public void DestroyMapObjects()
	{
		if (moAp != null)
		{
			moAp.Terminate();
		}
		if (moPe != null)
		{
			moPe.Terminate();
		}
		if (moCB != null)
		{
			moCB.Terminate();
		}
		if (moCBatUT != null)
		{
			moCBatUT.Terminate();
		}
		if (moEnd != null)
		{
			moEnd.Terminate();
		}
		if (moNextStart != null)
		{
			moNextStart.Terminate();
		}
	}

	public void AttachUINodes()
	{
		bool pinnable = HighLogic.LoadedScene != GameScenes.TRACKSTATION;
		mnAp = MapNode.Create(moAp, nodeColor, 32, hoverable: true, pinnable, blocksInput: false);
		mnAp.OnUpdateVisible += mnAp_OnUpdateVisible;
		mnAp.OnUpdatePosition += (MapNode n) => ap;
		mnAp.OnUpdateCaption += mnAp_OnUpdateCaption;
		mnPe = MapNode.Create(moPe, nodeColor, 32, hoverable: true, pinnable, blocksInput: false);
		mnPe.OnUpdateVisible += mnPe_OnUpdateVisible;
		mnPe.OnUpdatePosition += (MapNode n) => pe;
		mnPe.OnUpdateCaption += mnPe_OnUpdateCaption;
		mnCB = MapNode.Create(moCB, nodeColor, 20, hoverable: true, pinnable, blocksInput: false);
		mnCB.OnUpdateVisible += mnCB_OnUpdateVisible;
		mnCB.OnUpdatePosition += (MapNode n) => cb;
		mnCB.OnUpdateCaption += mnCB_OnUpdateCaption;
		mnCBatUT = MapNode.Create(moCBatUT, nodeColor, 24, hoverable: false, pinnable: false, blocksInput: false);
		mnCBatUT.OnUpdatePosition += mnCBatUT_OnUpdatePosition;
		mnCBatUT.OnUpdateVisible += mnCBatUT_OnUpdateVisible;
		mnEnd = MapNode.Create(moEnd, nodeColor, 24, hoverable: true, pinnable, blocksInput: false);
		mnEnd.OnUpdateVisible += mnEnd_OnUpdateVisible;
		mnEnd.OnUpdateCaption += mnEnd_OnUpdateCaption;
		mnEnd.OnUpdatePosition += (MapNode n) => end;
		mnEnd.OnUpdateType += mnEnd_OnUpdateType;
		mnNextStart = MapNode.Create(moNextStart, nodeColor, 24, hoverable: false, pinnable: false, blocksInput: false);
		mnNextStart.OnUpdateVisible += mnNextStart_OnUpdateVisible;
		mnNextStart.OnUpdateType += mnNextStart_OnUpdateType;
		mnNextStart.OnUpdatePosition += (MapNode n) => ScaledSpace.LocalToScaledSpace(trajectory.GetPatchEndRelative(patch.referenceBody.referenceBody));
	}

	public void DetachUINodes()
	{
		if (mnAp != null)
		{
			mnAp.Terminate();
		}
		if (mnPe != null)
		{
			mnPe.Terminate();
		}
		if (mnCB != null)
		{
			mnCB.Terminate();
		}
		if (mnCBatUT != null)
		{
			mnCBatUT.Terminate();
		}
		if (mnEnd != null)
		{
			mnEnd.Terminate();
		}
		if (mnNextStart != null)
		{
			mnNextStart.Terminate();
		}
	}

	public void mnNextStart_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		iData.visible = visible && CanDrawAnyNode() && mnEnd.HoverOrPinned && patch.patchEndTransition == Orbit.PatchTransitionType.ESCAPE && relativityMode != RelativityMode.RELATIVE && relativityMode != RelativityMode.DYNAMIC;
	}

	public void mnCBatUT_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		iData.visible = visible && CanDrawAnyNode() && mnEnd.HoverOrPinned && (patch.patchEndTransition == Orbit.PatchTransitionType.ESCAPE || (patch.patchEndTransition == Orbit.PatchTransitionType.ENCOUNTER && relativityMode != RelativityMode.LOCAL_AT_SOI_ENTRY_UT));
		iData.pixelSize = ((patch.patchEndTransition == Orbit.PatchTransitionType.ESCAPE) ? 24 : 20);
		iData.color = ((patch.patchEndTransition == Orbit.PatchTransitionType.ESCAPE) ? Color.gray : nodeColor);
	}

	public void mnEnd_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		iData.visible = visible && CanDrawAnyNode() && (patch.patchEndTransition == Orbit.PatchTransitionType.ENCOUNTER || patch.patchEndTransition == Orbit.PatchTransitionType.ESCAPE);
		iData.color = nodeColor;
	}

	public void mnAp_OnUpdateVisible(MapNode n, MapNode.IconData vData)
	{
		vData.visible = visible && CanDrawAnyNode() && patch.eccentricity < 1.0 && patch.timeToAp > 0.0 && (patch.StartUT + patch.timeToAp < patch.EndUT || patch.patchEndTransition == Orbit.PatchTransitionType.FINAL) && (patch.patchStartTransition != 0 || !(patch.vel.sqrMagnitude < 25.0));
		vData.color = nodeColor;
	}

	public void mnPe_OnUpdateVisible(MapNode n, MapNode.IconData vData)
	{
		vData.visible = visible && CanDrawAnyNode() && patch.timeToPe > 0.0 && (patch.StartUT + patch.timeToPe < patch.EndUT || patch.patchEndTransition == Orbit.PatchTransitionType.FINAL) && patch.PeR > patch.referenceBody.Radius;
		vData.color = nodeColor;
	}

	public void mnCB_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		iData.visible = visible && CanDrawAnyNode() && (patch.patchStartTransition == Orbit.PatchTransitionType.ENCOUNTER || patch.patchEndTransition == Orbit.PatchTransitionType.MANEUVER);
	}

	public void mnEnd_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		switch (patch.patchEndTransition)
		{
		case Orbit.PatchTransitionType.ENCOUNTER:
			cData.Header = Localizer.Format("#autoLOC_200007", patch.nextPatch.referenceBody.displayName);
			break;
		case Orbit.PatchTransitionType.ESCAPE:
			cData.Header = Localizer.Format("#autoLOC_200010", patch.referenceBody.displayName);
			break;
		case Orbit.PatchTransitionType.IMPACT:
			cData.Header = Localizer.Format("#autoLOC_200013", patch.referenceBody.displayName);
			break;
		}
		cData.captionLine1 = "T " + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - patch.EndUT, 3, explicitPositive: true);
	}

	public void mnAp_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		cData.Header = Localizer.Format("#autoLOC_200022", patch.referenceBody.displayName, patch.ApA.ToString("N0"));
		cData.captionLine1 = "T " + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - (patch.StartUT + patch.timeToAp), 3, explicitPositive: true);
	}

	public void mnPe_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		cData.Header = Localizer.Format("#autoLOC_200028", patch.referenceBody.displayName, patch.PeA.ToString("N0"));
		cData.captionLine1 = "T " + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - (patch.StartUT + patch.timeToPe), 3, explicitPositive: true);
	}

	public void mnCB_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		cData.Header = Localizer.Format("#autoLOC_7001301", patch.referenceBody.displayName);
		cData.captionLine1 = ((relativityMode != 0) ? Localizer.Format("#autoLOC_200035", KSPUtil.PrintDate(UTcb, includeTime: true)) : string.Empty);
	}

	public Vector3d mnCBatUT_OnUpdatePosition(MapNode n)
	{
		return patch.patchEndTransition switch
		{
			Orbit.PatchTransitionType.ENCOUNTER => ScaledSpace.LocalToScaledSpace(patch.nextPatch.referenceBody.getPositionAtUT(patch.EndUT)), 
			_ => ScaledSpace.LocalToScaledSpace(patch.referenceBody.getPositionAtUT(patch.EndUT)), 
		};
	}

	public void mnNextStart_OnUpdateType(MapNode n, MapNode.TypeData tData)
	{
		tData.oType = MapObject.ObjectType.PatchTransition;
		switch (patch.patchEndTransition)
		{
		case Orbit.PatchTransitionType.ESCAPE:
			tData.pType = MapNode.PatchTransitionNodeType.EscapeNextPatch;
			break;
		case Orbit.PatchTransitionType.ENCOUNTER:
			tData.pType = MapNode.PatchTransitionNodeType.EncounterNextPatch;
			break;
		}
	}

	public void mnEnd_OnUpdateType(MapNode n, MapNode.TypeData tData)
	{
		tData.oType = MapObject.ObjectType.PatchTransition;
		switch (patch.patchEndTransition)
		{
		case Orbit.PatchTransitionType.ENCOUNTER:
			tData.pType = MapNode.PatchTransitionNodeType.Encounter;
			break;
		case Orbit.PatchTransitionType.ESCAPE:
			tData.pType = MapNode.PatchTransitionNodeType.Escape;
			break;
		case Orbit.PatchTransitionType.IMPACT:
			tData.pType = MapNode.PatchTransitionNodeType.Impact;
			break;
		case Orbit.PatchTransitionType.MANEUVER:
			break;
		}
	}

	public bool CanDrawAnyNode()
	{
		if (MapView.MapIsEnabled && enabled && patch.activePatch)
		{
			return true;
		}
		return false;
	}

	public void MakeVector()
	{
		if (vectorLine != null)
		{
			DestroyVector();
		}
		vectorLine = new VectorLine(vectorName, vectorPoints, lineWidth, LineType.Discrete);
		vectorLine.texture = lineMaterial.mainTexture;
		vectorLine.material = lineMaterial;
		vectorLine.continuousTexture = smoothLineTexture;
		vectorLine.SetColor(patchColor);
		vectorLine.smoothColor = true;
		vectorLine.rectTransform.gameObject.layer = 31;
		vectorLine.UpdateImmediate = true;
		if (!hasMapNodes)
		{
			CreateMapObjects();
			AttachUINodes();
			hasMapNodes = true;
		}
	}

	public void DestroyVector()
	{
		if (vectorLine != null)
		{
			VectorLine.Destroy(ref vectorLine);
			vectorLine = null;
		}
	}

	public virtual void UpdatePR()
	{
		if (samples != tPoints.Length || vectorPoints.Count != samples * interpolations * 2)
		{
			tPoints = new Vector3d[samples];
			scaledTPoints = new Vector3[samples];
			vectorPoints = new List<Vector3>(samples * interpolations * 2);
			colors = new Color[samples];
			MakeVector();
		}
		if (vectorLine == null)
		{
			MakeVector();
		}
		else
		{
			vectorLine.active = patch.activePatch && visible;
		}
		if (patch.activePatch)
		{
			trajectory.TPoints = tPoints;
			trajectory.UpdateFromOrbit(patch, samples);
			tPoints = trajectory.TPoints;
			relativeTo = patch.referenceBody.referenceBody;
			tPoints = trajectory.TPoints;
			if (relativityMode == RelativityMode.RELATIVE && patch.patchEndTransition == Orbit.PatchTransitionType.FINAL)
			{
				relativityMode = RelativityMode.LOCAL_AT_SOI_ENTRY_UT;
			}
			if (currentMainBody.HasParent(patch.referenceBody))
			{
				relativityMode = RelativityMode.LOCAL_TO_BODIES;
			}
			if (patch.referenceBody == currentMainBody)
			{
				relativityMode = RelativityMode.LOCAL_TO_BODIES;
			}
			if (patch.referenceBody == PlanetariumCamera.fetch.target.GetReferenceBody() || patch.referenceBody.HasChild(PlanetariumCamera.fetch.target.GetReferenceBody()))
			{
				relativityMode = RelativityMode.LOCAL_TO_BODIES;
			}
			double universalTime = Planetarium.GetUniversalTime();
			UTap = patch.StartUT + patch.timeToAp;
			UTpe = patch.StartUT + patch.GetTimeToPeriapsis();
			switch (relativityMode)
			{
			case RelativityMode.LOCAL_TO_BODIES:
				trajectory.GetPointsLocal(tPoints);
				pe = trajectory.GetPeriapsisLocal();
				ap = trajectory.GetApoapsisLocal();
				st = trajectory.GetPatchStartLocal();
				end = trajectory.GetPatchEndLocal();
				cb = trajectory.GetRefBodyPosLocal();
				UTcb = universalTime;
				break;
			case RelativityMode.LOCAL_AT_SOI_ENTRY_UT:
				trajectory.GetPointsLocalAtUT(tPoints, relativeTo, patch.StartUT);
				pe = trajectory.GetPeriapsisLocalAtUT(relativeTo, patch.StartUT);
				ap = trajectory.GetApoapsisLocalAtUT(relativeTo, patch.StartUT);
				st = trajectory.GetPatchStartLocalAtUT(relativeTo, patch.StartUT);
				end = trajectory.GetPatchEndLocalAtUT(relativeTo, patch.StartUT);
				cb = trajectory.GetRefBodyPosLocalAtUT(relativeTo, patch.StartUT);
				UTcb = patch.StartUT;
				break;
			case RelativityMode.LOCAL_AT_SOI_EXIT_UT:
				trajectory.GetPointsLocalAtUT(tPoints, relativeTo, patch.EndUT);
				pe = trajectory.GetPeriapsisLocalAtUT(relativeTo, patch.EndUT);
				ap = trajectory.GetApoapsisLocalAtUT(relativeTo, patch.EndUT);
				st = trajectory.GetPatchStartLocalAtUT(relativeTo, patch.EndUT);
				end = trajectory.GetPatchEndLocalAtUT(relativeTo, patch.EndUT);
				cb = trajectory.GetRefBodyPosLocalAtUT(relativeTo, patch.EndUT);
				UTcb = patch.EndUT;
				break;
			case RelativityMode.RELATIVE:
				trajectory.GetPointsRelative(tPoints, relativeTo);
				pe = trajectory.GetPeriapsisRelative(relativeTo);
				ap = trajectory.GetApoapsisRelative(relativeTo);
				st = trajectory.GetPatchStartRelative(relativeTo);
				end = trajectory.GetPatchEndRelative(relativeTo);
				cb = trajectory.GetRefBodyPosRelative(relativeTo);
				UTcb = UTpe;
				break;
			case RelativityMode.DYNAMIC:
				trajectory.GetPointsLerped(tPoints, patch.referenceBody, relativeTo, Math.Max(universalTime, patch.StartUT), patch.EndUT, dynamicLinearity);
				pe = trajectory.GetPeriapsisLerped(patch.referenceBody, relativeTo, Math.Max(universalTime, patch.StartUT), patch.EndUT, dynamicLinearity);
				ap = trajectory.GetApoapsisLerped(patch.referenceBody, relativeTo, Math.Max(universalTime, patch.StartUT), patch.EndUT, dynamicLinearity);
				st = trajectory.GetPatchStartLerped(patch.referenceBody, relativeTo, Math.Max(universalTime, patch.StartUT), patch.EndUT, dynamicLinearity);
				end = trajectory.GetPatchEndLerped(patch.referenceBody, relativeTo, Math.Max(universalTime, patch.StartUT), patch.EndUT, dynamicLinearity);
				cb = trajectory.GetRefBodyPosLerped(patch.referenceBody, relativeTo, Math.Max(universalTime, patch.StartUT), patch.EndUT, dynamicLinearity);
				UTcb = UTpe;
				break;
			}
			pe = ScaledSpace.LocalToScaledSpace(pe);
			ap = ScaledSpace.LocalToScaledSpace(ap);
			st = ScaledSpace.LocalToScaledSpace(st);
			end = ScaledSpace.LocalToScaledSpace(end);
			cb = ScaledSpace.LocalToScaledSpace(cb);
			int num = tPoints.Length;
			while (num-- > 0)
			{
				scaledTPoints[num] = ScaledSpace.LocalToScaledSpace(tPoints[num]);
			}
			trajectory.GetColors(patchColor, colors);
			if (vectorLine != null)
			{
				UpdateSpline();
				DrawSplines();
			}
			UpdateUINodes();
		}
	}

	public void UpdateSpline()
	{
		vectorLine.MakeSpline(scaledTPoints, patch.eccentricity < 1.0 && patch.patchEndTransition == Orbit.PatchTransitionType.FINAL);
		vectorLine.SetColors(SetColorSegments(colors, interpolations));
	}

	public virtual void DrawSplines()
	{
		if (smoothLineTexture)
		{
			if (patch.eccentricity < 1.0 && patch.patchEndTransition == Orbit.PatchTransitionType.FINAL)
			{
				double num = 0.01745329238474369 - UtilMath.TwoPI;
				eccOffset = (patch.eccentricAnomaly - num) % UtilMath.TwoPI / UtilMath.TwoPI;
				twkOffset = (float)eccOffset * MapView.GetEccOffset((float)eccOffset, (float)patch.eccentricity, 4f);
				vectorLine.ContinuousTextureOffset = 1f - twkOffset;
			}
			else
			{
				vectorLine.ContinuousTextureOffset = 0f;
			}
		}
		else
		{
			vectorLine.textureScale = 1f;
		}
		if (MapView.Draw3DLines != draw3dLines)
		{
			MakeVector();
			UpdateSpline();
			draw3dLines = MapView.Draw3DLines;
		}
		if (MapView.Draw3DLines)
		{
			vectorLine.Draw3D();
		}
		else
		{
			vectorLine.Draw();
		}
	}

	public List<Color32> SetColorSegments(Color[] pointColors, int interpolations)
	{
		int num = samples;
		while (num-- > 0)
		{
			int num2 = interpolations;
			while (num2-- > 0)
			{
				VectorColours[num * interpolations + num2] = pointColors[num];
			}
		}
		return VectorColours;
	}

	public void UpdateUINodes()
	{
		if (mnAp != null)
		{
			mnAp.NodeUpdate();
		}
		if (mnPe != null)
		{
			mnPe.NodeUpdate();
		}
		if (mnCB != null)
		{
			mnCB.NodeUpdate();
		}
		if (mnEnd != null)
		{
			mnEnd.NodeUpdate();
		}
		if (mnNextStart != null)
		{
			mnNextStart.NodeUpdate();
		}
		if (mnCBatUT != null)
		{
			mnCBatUT.NodeUpdate();
		}
	}

	public Vector3 GetScaledSpacePointFromTA(double double_0, double double_1)
	{
		if (trajectory == null)
		{
			Debug.LogWarning("Warning: PatchRendering Trajectory is Null");
			return Vector3.zero;
		}
		return ScaledSpace.LocalToScaledSpace(relativityMode switch
		{
			RelativityMode.LOCAL_AT_SOI_ENTRY_UT => trajectory.ConvertPointToLocalAtUT(patch.getRelativePositionFromTrueAnomaly(double_0).xzy, patch.StartUT, relativeTo), 
			RelativityMode.LOCAL_AT_SOI_EXIT_UT => trajectory.ConvertPointToLocalAtUT(patch.getRelativePositionFromTrueAnomaly(double_0).xzy, patch.EndUT, relativeTo), 
			RelativityMode.RELATIVE => trajectory.ConvertPointToRelative(patch.getRelativePositionFromTrueAnomaly(double_0).xzy, double_1, relativeTo), 
			RelativityMode.DYNAMIC => trajectory.ConvertPointToLerped(patch.getRelativePositionFromTrueAnomaly(double_0).xzy, double_1, patch.referenceBody, relativeTo, Planetarium.GetUniversalTime() + patch.timeToPe, patch.EndUT, dynamicLinearity), 
			_ => trajectory.ConvertPointToLocal(patch.getRelativePositionFromTrueAnomaly(double_0).xzy), 
		});
	}
}
