using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SuitCombos : MonoBehaviour
{
	[SerializeField]
	private List<SuitCombo> defaultCombos;

	[SerializeField]
	private List<SuitCombo> stockCombos;

	public ConfigNode[] nodeNewCombos;

	[SerializeField]
	private List<SuitCombo> extraCombos;

	[SerializeField]
	private HelmetSuitPickerWindow helmetSuitPickerWindowPrefab;

	public HelmetSuitPickerWindow helmetSuitPickerWindow;

	public List<SuitCombo> DefaultCombos
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<SuitCombo> StockCombos
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<SuitCombo> ExtraCombos
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SuitCombos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetSuitCombos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ValidateCombo(SuitCombo suitCombo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private HelmetSuitPickerWindow CreateHelmetSuitPickerWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture GetDefaultTexture(ProtoCrewMember crew, SuitCombo.TextureTarget textureTarget, Material partMaterial, SuitCombo.MaterialProperty materialProperty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTextureScale(Material suitMaterial, Vector2 scaleUpdate, SuitCombo.MaterialProperty materialProperty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SuitCombo GetCombo(string comboId)
	{
		throw null;
	}
}
