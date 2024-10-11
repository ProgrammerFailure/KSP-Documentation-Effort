using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionValidationEntry : MonoBehaviour
{
	[SerializeField]
	private TMP_Text validationText;

	[SerializeField]
	private Toggle toggleComponent;

	private MissionValidationTestResult result;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionValidationEntry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionValidationEntry Create(MissionValidationTestResult result, ToggleGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggleValueChange(bool value)
	{
		throw null;
	}
}
