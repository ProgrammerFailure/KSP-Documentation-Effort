using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EditorLogicBase : MonoBehaviour
{
	[SerializeField]
	protected KerbalFSM fsm;

	public Vector3 selPartGrabOffset;

	public Vector3 dragPlaneCenter;

	public Vector3 srfAttachCursorOffset;

	protected ScreenMessage modeMsg;

	protected ScreenMessage interactMsg;

	[SerializeField]
	protected GameObject attachNodePrefab;

	protected Part selectedPart;

	protected CompoundPart selectedCompoundPart;

	protected Quaternion stackRotation;

	protected bool partTweaked;

	protected bool checkInputLocks;

	public EditorScreen editorScreen;

	protected bool isCurrentPartFlag;

	protected static string cacheAutoLOC_125488;

	protected static string cacheAutoLOC_125583;

	protected static string cacheAutoLOC_125724;

	protected static string cacheAutoLOC_125784;

	protected static string cacheAutoLOC_125798;

	protected static string cacheAutoLOC_6001217;

	protected static string cacheAutoLOC_6001218;

	protected static string cacheAutoLOC_6001219;

	protected static string cacheAutoLOC_6001220;

	protected static string cacheAutoLOC_6001221;

	protected static string cacheAutoLOC_6001222;

	protected static string cacheAutoLOC_6001223;

	protected static string cacheAutoLOC_6001224;

	protected static string cacheAutoLOC_6001225;

	protected static string cacheAutoLOC_6001226;

	protected static string cacheAutoLOC_6004038;

	protected static string cacheAutoLOC_6006095;

	public bool IsCurrentPartFlag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorLogicBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void partRotationInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void partRotationResetUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void displayAttachNodeIcons(List<Part> parts, Part selectedPart, bool stackNodes, bool srfNodes, bool dockNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void handleChildNodeIcons(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void handleAttachNodeIcons(Part part, bool stackNodes, bool srfNodes, bool dockNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AssignAttachIcon(Part part, AttachNode node, bool stackNodes, bool srfNodes, bool dockNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void clearAttachNodes(Part part, Part otherPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool PartInHierarchy(Part part, Part tgtPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float FindPartSurface(Part p, Vector3 origin, Vector3 direction, Vector3 fromParent, RaycastHit hit, out Vector3 srfNormal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float FindPartSurface(Part p, Vector3 origin, Vector3 direction, Vector3 fromParent, RaycastHit hit, out Vector3 srfNormal, int layerMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Part> FindPartsInChildren(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindPartsInChildren(ref List<Part> parts, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateSelectedPartIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
