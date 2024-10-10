using UnityEngine;

public class PropertyIDs : MonoBehaviour
{
	public static int _MinX;

	public static int _MaxX;

	public static int _MinY;

	public static int _MaxY;

	public static int _RimColor;

	public static int _RimFalloff;

	public static int _Opacity;

	public static int _EmissiveColor;

	public static int _Color;

	public static int _BumpMap;

	public static int _MainTex;

	public static int _TintColor;

	public static int _Multiplier;

	public static int upMatrix;

	public static int localMatrix;

	public static int _subdiv;

	public static int _LayerTransparentFX;

	public void Awake()
	{
		_MinX = Shader.PropertyToID("_MinX");
		_MaxX = Shader.PropertyToID("_MaxX");
		_MinY = Shader.PropertyToID("_MinY");
		_MaxY = Shader.PropertyToID("_MaxY");
		_RimColor = Shader.PropertyToID("_RimColor");
		_RimFalloff = Shader.PropertyToID("_RimFalloff");
		_Opacity = Shader.PropertyToID("_Opacity");
		_EmissiveColor = Shader.PropertyToID("_EmissiveColor");
		_Color = Shader.PropertyToID("_Color");
		_BumpMap = Shader.PropertyToID("_BumpMap");
		_MainTex = Shader.PropertyToID("_MainTex");
		_TintColor = Shader.PropertyToID("_TintColor");
		_Multiplier = Shader.PropertyToID("_Multiplier");
		upMatrix = Shader.PropertyToID("upMatrix");
		localMatrix = Shader.PropertyToID("localMatrix");
		_subdiv = Shader.PropertyToID("_subdiv");
		_LayerTransparentFX = LayerMask.NameToLayer("TransparentFX");
	}
}
