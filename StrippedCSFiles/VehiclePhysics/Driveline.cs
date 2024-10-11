using System;
using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class Driveline
{
	public enum DrivenAxles
	{
		None,
		SingleAxle,
		TwoAxles,
		ThreeAxles,
		FourAxles
	}

	public enum TwoAxlesConfig
	{
		CenterDifferential,
		TorqueSplitter,
		HDrive
	}

	public enum ThreeAxlesConfig
	{
		HDriveAndCenterDifferential,
		HDriveAndTorqueSplitterAtLinkedAxles,
		HDriveAndTorqueSplitterAtIndependentAxle
	}

	public enum FourAxlesConfig
	{
		DualInterAxleAndCenterDifferential,
		DualInterAxleDifferentialAndTorqueSplitter,
		DualHDriveAndCenterDifferential,
		DualHDriveAndTorqueSplitter,
		FullHDrive
	}

	public enum Override
	{
		None,
		ForceLocked,
		ForceUnlocked
	}

	[Serializable]
	public class Settings
	{
		public DrivenAxles drivenAxles;

		public int firstDrivenAxle;

		public int secondDrivenAxle;

		public int thirdDrivenAxle;

		public int fourthDrivenAxle;

		public TwoAxlesConfig twoDrivenAxlesConfig;

		public ThreeAxlesConfig threeDrivenAxlesConfig;

		public FourAxlesConfig fourDrivenAxlesConfig;

		public bool hasInterAxleDifferential
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public bool hasCenterDifferential
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public bool hasTorqueSplitter
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	public Settings settings;

	public Differential.Settings axleDifferential;

	public Differential.Settings centerDifferential;

	public Differential.Settings interAxleDifferential;

	public TorqueSplitter.Settings torqueSplitter;

	private Override m_differentialOverride;

	private Differential.Type m_differentialType;

	private Override m_drivelineOverride;

	private Differential.Type m_drivelineDifferentialType;

	private float m_torqueSplitterStiffness;

	private Differential[] m_detachableAxleDifferentials;

	private Differential.Settings m_openDifferential;

	public Override differentialOverride
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Override drivelineOverride
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Driveline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDetachableDifferentials()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAxleFinalRatio(int axle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetSingleAxleFinalRatio(int axle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetTwoAxlesFinalRatio(int axle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetThreeAxlesFinalRatio(int axle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetFourAxlesFinalRatio(int axle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupSingleAxleDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupTwoAxlesDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupThreeAxlesDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupFourAxlesDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		throw null;
	}
}
