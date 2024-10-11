using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KSP.UI;

public class UITransparencyController : MonoBehaviour
{
	[Serializable]
	public class ShaderFader
	{
		[SerializeField]
		private Material mat;

		[SerializeField]
		private string colorPropertyName;

		[SerializeField]
		private string alphaPropertyName;

		[SerializeField]
		private bool absoluteAlpha;

		private int propID;

		private bool useColorProp;

		private Color baseColor;

		private float baseAlpha;

		private bool init;

		public bool Init
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ShaderFader()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetOpacity(float opacity)
		{
			throw null;
		}
	}

	[SerializeField]
	private float opacity;

	[SerializeField]
	private CanvasGroup[] canvasGroups;

	[SerializeField]
	private ShaderFader[] shaderFaders;

	public float Opacity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UITransparencyController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateOpacity(float opacity)
	{
		throw null;
	}
}
