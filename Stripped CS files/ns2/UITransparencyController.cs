using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ns2;

public class UITransparencyController : MonoBehaviour
{
	[Serializable]
	public class ShaderFader
	{
		[SerializeField]
		public Material mat;

		[SerializeField]
		public string colorPropertyName = string.Empty;

		[SerializeField]
		public string alphaPropertyName = string.Empty;

		[SerializeField]
		public bool absoluteAlpha = true;

		public int propID;

		public bool useColorProp;

		public Color baseColor;

		public float baseAlpha;

		public bool init;

		public bool Init => init;

		public bool Setup()
		{
			if (mat != null)
			{
				useColorProp = !string.IsNullOrEmpty(colorPropertyName);
				if (useColorProp)
				{
					propID = Shader.PropertyToID(colorPropertyName);
					baseColor = mat.GetColor(propID);
				}
				else
				{
					propID = Shader.PropertyToID(alphaPropertyName);
					baseAlpha = mat.GetFloat(propID);
				}
			}
			init = true;
			return init;
		}

		public void SetOpacity(float opacity)
		{
			if (!init)
			{
				return;
			}
			if (useColorProp)
			{
				if (absoluteAlpha)
				{
					mat.SetColor(propID, baseColor.smethod_0(opacity));
				}
				else
				{
					mat.SetColor(propID, new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * opacity));
				}
			}
			else if (absoluteAlpha)
			{
				mat.SetFloat(propID, opacity);
			}
			else
			{
				mat.SetFloat(propID, baseAlpha * opacity);
			}
		}
	}

	[SerializeField]
	public float opacity;

	[SerializeField]
	public CanvasGroup[] canvasGroups;

	[SerializeField]
	public ShaderFader[] shaderFaders;

	public float Opacity => opacity;

	public void Start()
	{
		GameEvents.OnGameSettingsApplied.Add(onSettingsApplied);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes lvl)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			UpdateOpacity(GameSettings.UI_OPACITY);
		}
		else
		{
			UpdateOpacity(1f);
		}
	}

	public void OnDestroy()
	{
		GameEvents.OnGameSettingsApplied.Remove(onSettingsApplied);
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void onSettingsApplied()
	{
		UpdateOpacity(GameSettings.UI_OPACITY);
	}

	public void UpdateOpacity(float opacity)
	{
		int num = canvasGroups.Length;
		while (num-- > 0)
		{
			canvasGroups[num].alpha = opacity;
		}
		int num2 = shaderFaders.Length;
		while (num2-- > 0)
		{
			ShaderFader shaderFader = shaderFaders[num2];
			if (!shaderFader.Init)
			{
				shaderFader.Setup();
				shaderFader.SetOpacity(opacity);
			}
			else
			{
				shaderFader.SetOpacity(opacity);
			}
		}
		this.opacity = opacity;
	}
}
