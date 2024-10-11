using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class VesselIconSprite : MonoBehaviour
{
	[SerializeField]
	private Image image;

	[SerializeField]
	private Sprite[] vesselTypeIcons;

	private VesselType type;

	public VesselType vesselType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselIconSprite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetType(VesselType t)
	{
		throw null;
	}
}
