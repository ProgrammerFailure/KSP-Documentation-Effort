using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public abstract class PerformanceChart
{
	public static Color[] wheelChartColors = new Color[4]
	{
		GColor.yellowA100,
		GColor.lightBlue200,
		GColor.red800,
		GColor.blue800
	};

	public VehicleBase vehicle { get; set; }

	public DataLogger dataLogger { get; set; }

	public ReferenceSpecs reference { get; set; }

	public PerformanceChart()
	{
	}

	public virtual string Title()
	{
		return "unnamed";
	}

	public virtual void Initialize()
	{
	}

	public virtual void ResetView()
	{
		dataLogger.rect = new Rect(0f, -0.5f, 30f, 15f);
	}

	public abstract void SetupChannels();

	public abstract void RecordData();
}
