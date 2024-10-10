using UnityEngine;

public class ModuleDisplaceTweak : PartModule
{
	[KSPField]
	public string tgtTransformName = "";

	[KSPField]
	public string tweakName = "Tweak";

	[KSPField]
	public Vector3 axis;

	[KSPField]
	public float displaceMax = 1f;

	[KSPField]
	public float displaceMin = -1f;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001809")]
	public float scalar = 0.5f;

	public Transform tgtTrf;

	public Vector3 p0;

	public Vector3 pMax;

	public Vector3 pMin;

	public int mIndex;

	public ModuleDisplaceTweak MDprev;

	public ModuleDisplaceTweak MDnext;

	public ModuleDisplaceTweak mD;

	public override void OnStart(StartState state)
	{
		tgtTrf = base.part.FindModelTransform(tgtTransformName);
		if (!(tgtTrf != null))
		{
			return;
		}
		p0 = tgtTrf.localPosition;
		pMax = axis * displaceMax;
		pMin = axis * displaceMin;
		GameEvents.onEditorPartEvent.Add(OnPartEvent);
		MDprev = null;
		MDnext = null;
		mIndex = base.part.Modules.IndexOf(this);
		int num = 0;
		while (true)
		{
			if (num < base.part.Modules.Count)
			{
				if (num < mIndex && base.part.Modules[num] is ModuleDisplaceTweak)
				{
					MDprev = base.part.Modules[num] as ModuleDisplaceTweak;
				}
				if (num != mIndex && num > mIndex && base.part.Modules[num] is ModuleDisplaceTweak)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		MDnext = base.part.Modules[num] as ModuleDisplaceTweak;
	}

	public void OnDestroy()
	{
		GameEvents.onEditorPartEvent.Remove(OnPartEvent);
	}

	public void OnPartEvent(ConstructionEventType evt, Part p)
	{
		if (evt == ConstructionEventType.PartTweaked && (p == base.part || base.part.symmetryCounterparts.Contains(p)))
		{
			mD = ((p != base.part) ? (p.Modules[mIndex] as ModuleDisplaceTweak) : this);
			UpdateMorph(mD.scalar);
		}
	}

	public void UpdateMorph(float t)
	{
		scalar = t;
		if (MDnext == null)
		{
			tgtTrf.localPosition = GetFinalPosition();
		}
	}

	public Vector3 GetFinalPosition()
	{
		return ((MDprev != null) ? MDprev.GetFinalPosition() : p0) + Vector3.Lerp(pMin, pMax, scalar);
	}
}
