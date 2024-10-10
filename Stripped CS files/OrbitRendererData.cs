using UnityEngine;

public class OrbitRendererData
{
	public CelestialBody cb;

	public Color orbitColor;

	public float lowerCamVsSmaRatio;

	public float upperCamVsSmaRatio;

	public Color nodeColor;

	public OrbitRendererData(CelestialBody cb)
	{
		this.cb = cb;
	}
}
