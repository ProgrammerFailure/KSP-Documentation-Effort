using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro.Examples;

public class TMP_ExampleScript_01 : MonoBehaviour
{
	public enum objectType
	{
		TextMeshPro,
		TextMeshProUGUI
	}

	public objectType ObjectType;

	public bool isStatic;

	private TMP_Text m_text;

	private const string k_label = "The count is <#0080ff>{0}</color>";

	private int count;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_ExampleScript_01()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
