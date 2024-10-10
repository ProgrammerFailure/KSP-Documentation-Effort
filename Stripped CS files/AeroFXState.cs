using System;

[Serializable]
public class AeroFXState
{
	public MinMaxColor color;

	public MinMaxFloat length;

	public MinMaxFloat edgeFade;

	public MinMaxFloat falloff1;

	public MinMaxFloat falloff2;

	public MinMaxFloat falloff3;

	public MinMaxFloat intensity;

	public MinMaxFloat wobble;

	public MinMaxFloat lightPower;

	public MinMaxFloat airspeedNoiseVolume;

	public MinMaxFloat airspeedNoisePitch;
}
