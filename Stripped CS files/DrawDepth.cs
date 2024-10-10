using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class DrawDepth : MonoBehaviour
{
	public bool isSupported = true;

	public bool supportHDRTextures = true;

	public Shader depthShader;

	public Material depthMaterial;

	public void OnEnable()
	{
		if (depthShader == null)
		{
			depthShader = Shader.Find("Hidden/AdvancedSS/DrawDepth");
		}
		isSupported = true;
	}

	public bool CheckSupport()
	{
		return CheckSupport(needDepth: true);
	}

	public void Start()
	{
		CheckResources();
	}

	public void OnDisable()
	{
		if ((bool)depthMaterial)
		{
			Object.DestroyImmediate(depthMaterial);
		}
	}

	public bool CheckResources()
	{
		CheckSupport(needDepth: false);
		depthMaterial = CheckShaderAndCreateMaterial(depthShader, depthMaterial);
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	public bool CheckSupport(bool needDepth)
	{
		isSupported = true;
		supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		if (SystemInfo.supportsImageEffects && SystemInfo.supportsRenderTextures)
		{
			if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				NotSupported();
				return false;
			}
			if (needDepth)
			{
				GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			return true;
		}
		NotSupported();
		return false;
	}

	public bool CheckSupport(bool needDepth, bool needHdr)
	{
		if (!CheckSupport(needDepth))
		{
			return false;
		}
		if (needHdr && !supportHDRTextures)
		{
			NotSupported();
			return false;
		}
		return true;
	}

	public void ReportAutoDisable()
	{
		Debug.LogWarning("The image effect " + ToString() + " has been disabled as it's not supported on the current platform.");
	}

	public bool CheckShader(Shader s)
	{
		Debug.Log("The shader " + s.ToString() + " on effect " + ToString() + " is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package.");
		if (!s.isSupported)
		{
			NotSupported();
			return false;
		}
		return false;
	}

	public void NotSupported()
	{
		base.enabled = false;
		isSupported = false;
	}

	public void LateUpdate()
	{
		DoDepthRender();
	}

	public void DoDepthRender()
	{
		GetComponent<Camera>().RenderWithShader(depthShader, "");
	}

	public Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log("Missing shader in " + ToString());
			base.enabled = false;
			return null;
		}
		if (s.isSupported && (bool)m2Create && m2Create.shader == s)
		{
			return m2Create;
		}
		if (!s.isSupported)
		{
			NotSupported();
			Debug.LogError("The shader " + s.ToString() + " on effect " + ToString() + " is not supported on this platform!");
			return null;
		}
		m2Create = new Material(s);
		m2Create.hideFlags = HideFlags.DontSave;
		if ((bool)m2Create)
		{
			return m2Create;
		}
		return null;
	}

	public Material CreateMaterial(Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log("Missing shader in " + ToString());
			return null;
		}
		if ((bool)m2Create && m2Create.shader == s && s.isSupported)
		{
			return m2Create;
		}
		if (!s.isSupported)
		{
			return null;
		}
		m2Create = new Material(s);
		m2Create.hideFlags = HideFlags.DontSave;
		if ((bool)m2Create)
		{
			return m2Create;
		}
		return null;
	}
}
