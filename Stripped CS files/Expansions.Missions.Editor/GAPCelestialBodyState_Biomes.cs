using System;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_Biomes : GAPCelestialBodyState_Base
{
	public delegate void OnBiomeSelected(CBAttributeMapSO.MapAttribute biome);

	public Shader shaderBiomes;

	public Shader shaderRegular;

	public Material scaledBodyMaterial;

	public float biomeHighlightValue;

	public CBAttributeMapSO.MapAttribute selectedBiome;

	public CBAttributeMapSO.MapAttribute hoverBiome;

	public OnBiomeSelected OnGAPBiomeSelected;

	public override void Init(GAPCelestialBody gapRef)
	{
		base.Init(gapRef);
		shaderBiomes = gapRef.Selector.biomeShaderMaterial.shader;
		gapRef.Selector.containerFooter.SetActive(value: true);
	}

	public override void Update()
	{
		if (hoverBiome != null)
		{
			Color mapColor = hoverBiome.mapColor;
			biomeHighlightValue = Mathf.Clamp(biomeHighlightValue + Time.deltaTime * 2f, 0f, 1f);
			mapColor.a = Mathf.Pow(biomeHighlightValue, 2f);
			scaledBodyMaterial.SetColor("_HoverBiomeColor", mapColor);
		}
	}

	public override void End()
	{
	}

	public override void LoadPlanet(CelestialBody newCelestialBody)
	{
		base.LoadPlanet(newCelestialBody);
		selectedBiome = null;
		hoverBiome = null;
		UpdateGAPText();
		if (gapRef.CelestialBody.BiomeMap != null)
		{
			shaderRegular = gapRef.CelestialBody.scaledBody.GetComponent<Renderer>().material.shader;
			scaledBodyMaterial = gapRef.CelestialBody.scaledBody.GetComponent<Renderer>().material;
			scaledBodyMaterial.shader = shaderBiomes;
			scaledBodyMaterial.SetTexture("_BiomesMap", gapRef.CelestialBody.BiomeMap.CompileToTexture());
		}
	}

	public override void UnloadPlanet()
	{
		base.UnloadPlanet();
		if (gapRef.CelestialBody.BiomeMap != null)
		{
			scaledBodyMaterial.SetTexture("_BiomeMap", null);
			scaledBodyMaterial.shader = shaderRegular;
		}
	}

	public override void OnClickUp(RaycastHit? hit)
	{
		if (hit.HasValue && gapRef.CelestialBody.BiomeMap != null)
		{
			Vector3 normalized = (hit.Value.point - gapRef.CelestialBody.scaledBody.transform.position).normalized;
			double degrees = Math.Asin(0f - normalized.y) * -57.295780181884766;
			CBAttributeMapSO.MapAttribute mapAttribute = ResourceUtilities.GetBiome(lon: ResourceUtilities.Deg2Rad(Math.Atan2(normalized.z, normalized.x) * 57.295780181884766), lat: ResourceUtilities.Deg2Rad(degrees), body: gapRef.CelestialBody);
			SelectBiome(mapAttribute);
			if (OnGAPBiomeSelected != null)
			{
				OnGAPBiomeSelected(mapAttribute);
			}
		}
		else
		{
			scaledBodyMaterial.SetColor("_SelectedBiomeColor", Color.black);
			selectedBiome = null;
			hoverBiome = null;
			UpdateGAPText();
			if (OnGAPBiomeSelected != null)
			{
				OnGAPBiomeSelected(null);
			}
		}
	}

	public override void OnMouseOver(Vector2 cameraPoint)
	{
		if (gapRef.CelestialBody.BiomeMap != null && gapRef.DoLocalSpaceRay(cameraPoint, out var rayHit) && rayHit.collider != null)
		{
			Vector3 normalized = (rayHit.point - gapRef.CelestialBody.scaledBody.transform.position).normalized;
			double degrees = Math.Asin(0f - normalized.y) * -57.295780181884766;
			CBAttributeMapSO.MapAttribute mapAttribute = ResourceUtilities.GetBiome(lon: ResourceUtilities.Deg2Rad(Math.Atan2(normalized.z, normalized.x) * 57.295780181884766), lat: ResourceUtilities.Deg2Rad(degrees), body: gapRef.CelestialBody);
			if (mapAttribute != hoverBiome && mapAttribute != selectedBiome)
			{
				hoverBiome = mapAttribute;
				UpdateGAPText();
				biomeHighlightValue = 0f;
			}
		}
		else
		{
			hoverBiome = null;
			scaledBodyMaterial.SetColor("_HoverBiomeColor", Color.black);
		}
	}

	public void SelectBiome(CBAttributeMapSO.MapAttribute newBiome)
	{
		if (selectedBiome != newBiome)
		{
			selectedBiome = newBiome;
			UpdateGAPText();
			hoverBiome = null;
			scaledBodyMaterial.SetColor("_HoverBiomeColor", Color.black);
			Color value = ((newBiome != null) ? selectedBiome.mapColor : Color.black);
			scaledBodyMaterial.SetColor("_SelectedBiomeColor", value);
		}
	}

	public bool HasSelectedBiome()
	{
		return selectedBiome != null;
	}

	public void SelectBiomeByName(string biomeName)
	{
		if (gapRef.CelestialBody.BiomeMap == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < gapRef.CelestialBody.BiomeMap.Attributes.Length; i++)
		{
			if (biomeName == gapRef.CelestialBody.BiomeMap.Attributes[i].name)
			{
				SelectBiome(gapRef.CelestialBody.BiomeMap.Attributes[i]);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			SelectBiome(null);
		}
	}

	public static int GetBiomeIndex(CelestialBody body, string biomeName)
	{
		int result = -1;
		if (body.BiomeMap != null)
		{
			for (int i = 0; i < body.BiomeMap.Attributes.Length; i++)
			{
				if (body.BiomeMap.Attributes[i].name == biomeName)
				{
					result = i;
					break;
				}
			}
		}
		return result;
	}

	public void UpdateGAPText()
	{
		string text = ((selectedBiome != null) ? selectedBiome.displayname : "-");
		string text2 = ((hoverBiome != null) ? hoverBiome.displayname : "-");
		gapRef.SetFooterText(Localizer.Format("#autoLOC_8007301", text, text2));
	}
}
