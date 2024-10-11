using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

[Obsolete("Deprecated. Use TimeScale and/or SceneReload instead")]
public class SceneTools : MonoBehaviour
{
	public bool slowTimeMode;

	public float slowTime;

	public KeyCode hotkeyReset;

	public KeyCode hotkeyTime;

	private bool m_prevSlowTimeMode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SceneTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
