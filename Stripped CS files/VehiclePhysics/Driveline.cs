using System;
using UnityEngine;

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
		public DrivenAxles drivenAxles = DrivenAxles.TwoAxles;

		public int firstDrivenAxle = 1;

		public int secondDrivenAxle;

		public int thirdDrivenAxle = 2;

		public int fourthDrivenAxle = 3;

		public TwoAxlesConfig twoDrivenAxlesConfig = TwoAxlesConfig.TorqueSplitter;

		public ThreeAxlesConfig threeDrivenAxlesConfig = ThreeAxlesConfig.HDriveAndTorqueSplitterAtLinkedAxles;

		public FourAxlesConfig fourDrivenAxlesConfig = FourAxlesConfig.DualHDriveAndTorqueSplitter;

		public bool hasInterAxleDifferential
		{
			get
			{
				if (drivenAxles == DrivenAxles.FourAxles)
				{
					if (fourDrivenAxlesConfig != 0)
					{
						return fourDrivenAxlesConfig == FourAxlesConfig.DualInterAxleDifferentialAndTorqueSplitter;
					}
					return true;
				}
				return false;
			}
		}

		public bool hasCenterDifferential
		{
			get
			{
				if ((drivenAxles == DrivenAxles.TwoAxles && twoDrivenAxlesConfig == TwoAxlesConfig.CenterDifferential) || (drivenAxles == DrivenAxles.ThreeAxles && threeDrivenAxlesConfig == ThreeAxlesConfig.HDriveAndCenterDifferential))
				{
					return true;
				}
				if (drivenAxles == DrivenAxles.FourAxles)
				{
					if (fourDrivenAxlesConfig != 0)
					{
						return fourDrivenAxlesConfig == FourAxlesConfig.DualHDriveAndCenterDifferential;
					}
					return true;
				}
				return false;
			}
		}

		public bool hasTorqueSplitter
		{
			get
			{
				if ((drivenAxles == DrivenAxles.TwoAxles && twoDrivenAxlesConfig == TwoAxlesConfig.TorqueSplitter) || (drivenAxles == DrivenAxles.ThreeAxles && (threeDrivenAxlesConfig == ThreeAxlesConfig.HDriveAndTorqueSplitterAtLinkedAxles || threeDrivenAxlesConfig == ThreeAxlesConfig.HDriveAndTorqueSplitterAtIndependentAxle)))
				{
					return true;
				}
				if (drivenAxles == DrivenAxles.FourAxles)
				{
					if (fourDrivenAxlesConfig != FourAxlesConfig.DualInterAxleDifferentialAndTorqueSplitter)
					{
						return fourDrivenAxlesConfig == FourAxlesConfig.DualHDriveAndTorqueSplitter;
					}
					return true;
				}
				return false;
			}
		}
	}

	public Settings settings = new Settings();

	public Differential.Settings axleDifferential = new Differential.Settings();

	public Differential.Settings centerDifferential = new Differential.Settings();

	public Differential.Settings interAxleDifferential = new Differential.Settings();

	public TorqueSplitter.Settings torqueSplitter = new TorqueSplitter.Settings();

	public Override m_differentialOverride;

	public Differential.Type m_differentialType;

	public Override m_drivelineOverride;

	public Differential.Type m_drivelineDifferentialType;

	public float m_torqueSplitterStiffness;

	public Differential[] m_detachableAxleDifferentials = new Differential[0];

	public Differential.Settings m_openDifferential = new Differential.Settings
	{
		type = Differential.Type.Open
	};

	public Override differentialOverride
	{
		get
		{
			return m_differentialOverride;
		}
		set
		{
			if (value != m_differentialOverride)
			{
				if (m_differentialOverride == Override.None)
				{
					m_differentialType = axleDifferential.type;
				}
				switch (value)
				{
				case Override.None:
					axleDifferential.type = m_differentialType;
					break;
				case Override.ForceLocked:
					axleDifferential.type = Differential.Type.Locked;
					break;
				case Override.ForceUnlocked:
					axleDifferential.type = Differential.Type.Open;
					break;
				}
				m_differentialOverride = value;
				UpdateDetachableDifferentials();
			}
		}
	}

	public Override drivelineOverride
	{
		get
		{
			return m_drivelineOverride;
		}
		set
		{
			if (value != m_drivelineOverride)
			{
				if (m_drivelineOverride == Override.None)
				{
					m_drivelineDifferentialType = centerDifferential.type;
					m_torqueSplitterStiffness = torqueSplitter.stiffness;
				}
				switch (value)
				{
				case Override.None:
					centerDifferential.type = m_drivelineDifferentialType;
					torqueSplitter.stiffness = m_torqueSplitterStiffness;
					break;
				case Override.ForceLocked:
					centerDifferential.type = Differential.Type.Locked;
					torqueSplitter.stiffness = 1f;
					break;
				case Override.ForceUnlocked:
					centerDifferential.type = Differential.Type.Open;
					torqueSplitter.stiffness = 0f;
					break;
				}
				m_drivelineOverride = value;
				UpdateDetachableDifferentials();
			}
		}
	}

	public void UpdateDetachableDifferentials()
	{
		if (m_drivelineOverride == Override.ForceUnlocked)
		{
			Differential[] detachableAxleDifferentials = m_detachableAxleDifferentials;
			for (int i = 0; i < detachableAxleDifferentials.Length; i++)
			{
				detachableAxleDifferentials[i].settings = m_openDifferential;
			}
		}
		else
		{
			Differential[] detachableAxleDifferentials = m_detachableAxleDifferentials;
			for (int i = 0; i < detachableAxleDifferentials.Length; i++)
			{
				detachableAxleDifferentials[i].settings = axleDifferential;
			}
		}
	}

	public float GetAxleFinalRatio(int axle)
	{
		return settings.drivenAxles switch
		{
			DrivenAxles.SingleAxle => GetSingleAxleFinalRatio(axle), 
			DrivenAxles.TwoAxles => GetTwoAxlesFinalRatio(axle), 
			DrivenAxles.ThreeAxles => GetThreeAxlesFinalRatio(axle), 
			DrivenAxles.FourAxles => GetFourAxlesFinalRatio(axle), 
			_ => float.NaN, 
		};
	}

	public float GetSingleAxleFinalRatio(int axle)
	{
		if (axle == settings.firstDrivenAxle)
		{
			return axleDifferential.gearRatio;
		}
		return float.NaN;
	}

	public float GetTwoAxlesFinalRatio(int axle)
	{
		if (axle != settings.firstDrivenAxle && axle != settings.secondDrivenAxle)
		{
			return float.NaN;
		}
		float num = axleDifferential.gearRatio;
		if (settings.twoDrivenAxlesConfig == TwoAxlesConfig.CenterDifferential)
		{
			num *= centerDifferential.gearRatio;
		}
		return num;
	}

	public float GetThreeAxlesFinalRatio(int axle)
	{
		if (axle != settings.firstDrivenAxle && axle != settings.secondDrivenAxle && axle != settings.thirdDrivenAxle)
		{
			return float.NaN;
		}
		float num = axleDifferential.gearRatio;
		if (settings.threeDrivenAxlesConfig == ThreeAxlesConfig.HDriveAndCenterDifferential)
		{
			num *= centerDifferential.gearRatio;
		}
		return num;
	}

	public float GetFourAxlesFinalRatio(int axle)
	{
		if (axle != settings.firstDrivenAxle && axle != settings.secondDrivenAxle && axle != settings.thirdDrivenAxle && axle != settings.fourthDrivenAxle)
		{
			return float.NaN;
		}
		float num = axleDifferential.gearRatio;
		if (settings.fourDrivenAxlesConfig == FourAxlesConfig.DualInterAxleAndCenterDifferential || settings.fourDrivenAxlesConfig == FourAxlesConfig.DualInterAxleDifferentialAndTorqueSplitter)
		{
			num *= interAxleDifferential.gearRatio;
		}
		if (settings.fourDrivenAxlesConfig == FourAxlesConfig.DualInterAxleAndCenterDifferential || settings.fourDrivenAxlesConfig == FourAxlesConfig.DualHDriveAndCenterDifferential)
		{
			num *= centerDifferential.gearRatio;
		}
		return num;
	}

	public void SetupDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		switch (settings.drivenAxles)
		{
		case DrivenAxles.SingleAxle:
			SetupSingleAxleDriveline(wheels, powerTrainOutput);
			break;
		case DrivenAxles.TwoAxles:
			SetupTwoAxlesDriveline(wheels, powerTrainOutput);
			break;
		case DrivenAxles.ThreeAxles:
			SetupThreeAxlesDriveline(wheels, powerTrainOutput);
			break;
		case DrivenAxles.FourAxles:
			SetupFourAxlesDriveline(wheels, powerTrainOutput);
			break;
		}
	}

	public void SetupSingleAxleDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		if (settings.firstDrivenAxle * 2 >= wheels.Length)
		{
			Debug.LogError("Invalid driven axle. Configure Driveline > Axle to a valid axle value.");
			return;
		}
		Differential differential = new Differential();
		differential.settings = axleDifferential;
		int num = settings.firstDrivenAxle * 2;
		int num2 = settings.firstDrivenAxle * 2 + 1;
		Block.Connect(wheels[num], 0, differential, 0);
		Block.Connect(wheels[num2], 0, differential, 1);
		Block.Connect(differential, 0, powerTrainOutput, 0);
		m_detachableAxleDifferentials = new Differential[0];
	}

	public void SetupTwoAxlesDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		if (settings.firstDrivenAxle * 2 < wheels.Length && settings.secondDrivenAxle * 2 < wheels.Length)
		{
			int num = settings.firstDrivenAxle * 2;
			int num2 = settings.firstDrivenAxle * 2 + 1;
			int num3 = settings.secondDrivenAxle * 2;
			int num4 = settings.secondDrivenAxle * 2 + 1;
			switch (settings.twoDrivenAxlesConfig)
			{
			case TwoAxlesConfig.CenterDifferential:
			{
				Differential differential6 = new Differential();
				Differential differential7 = new Differential();
				Differential differential8 = new Differential();
				differential6.settings = axleDifferential;
				differential7.settings = axleDifferential;
				differential8.settings = centerDifferential;
				Block.Connect(wheels[num], 0, differential6, 0);
				Block.Connect(wheels[num2], 0, differential6, 1);
				Block.Connect(wheels[num3], 0, differential7, 0);
				Block.Connect(wheels[num4], 0, differential7, 1);
				Block.Connect(differential6, 0, differential8, 0);
				Block.Connect(differential7, 0, differential8, 1);
				Block.Connect(differential8, 0, powerTrainOutput, 0);
				m_detachableAxleDifferentials = new Differential[0];
				break;
			}
			case TwoAxlesConfig.TorqueSplitter:
			{
				Differential differential4 = new Differential();
				Differential differential5 = new Differential();
				TorqueSplitter torqueSplitter = new TorqueSplitter();
				differential4.settings = axleDifferential;
				differential5.settings = axleDifferential;
				torqueSplitter.settings = this.torqueSplitter;
				Block.Connect(wheels[num], 0, differential4, 0);
				Block.Connect(wheels[num2], 0, differential4, 1);
				Block.Connect(wheels[num3], 0, differential5, 0);
				Block.Connect(wheels[num4], 0, differential5, 1);
				Block.Connect(differential4, 0, torqueSplitter, 0);
				Block.Connect(differential5, 0, torqueSplitter, 1);
				Block.Connect(torqueSplitter, 0, powerTrainOutput, 0);
				m_detachableAxleDifferentials = new Differential[1];
				m_detachableAxleDifferentials[0] = differential5;
				break;
			}
			case TwoAxlesConfig.HDrive:
			{
				Differential differential = new Differential();
				Differential differential2 = new Differential();
				differential.settings.gearRatio = 1f;
				differential2.settings.gearRatio = 1f;
				differential.settings.type = Differential.Type.Locked;
				differential2.settings.type = Differential.Type.Locked;
				Block.Connect(wheels[num], 0, differential, 0);
				Block.Connect(wheels[num3], 0, differential, 1);
				Block.Connect(wheels[num2], 0, differential2, 0);
				Block.Connect(wheels[num4], 0, differential2, 1);
				Differential differential3 = new Differential();
				differential3.settings = axleDifferential;
				Block.Connect(differential, 0, differential3, 0);
				Block.Connect(differential2, 0, differential3, 1);
				Block.Connect(differential3, 0, powerTrainOutput, 0);
				m_detachableAxleDifferentials = new Differential[0];
				break;
			}
			}
		}
		else
		{
			Debug.LogError("Invalid driven axles. Configure Driveline > First and Second axles to valid values.");
		}
	}

	public void SetupThreeAxlesDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		if (settings.firstDrivenAxle * 2 < wheels.Length && settings.secondDrivenAxle * 2 < wheels.Length && settings.thirdDrivenAxle * 2 < wheels.Length)
		{
			int num = settings.firstDrivenAxle * 2;
			int num2 = settings.firstDrivenAxle * 2 + 1;
			int num3 = settings.secondDrivenAxle * 2;
			int num4 = settings.secondDrivenAxle * 2 + 1;
			int num5 = settings.thirdDrivenAxle * 2;
			int num6 = settings.thirdDrivenAxle * 2 + 1;
			Differential differential = new Differential();
			Differential differential2 = new Differential();
			differential.settings.gearRatio = 1f;
			differential2.settings.gearRatio = 1f;
			differential.settings.type = Differential.Type.Locked;
			differential2.settings.type = Differential.Type.Locked;
			Block.Connect(wheels[num3], 0, differential, 0);
			Block.Connect(wheels[num5], 0, differential, 1);
			Block.Connect(wheels[num4], 0, differential2, 0);
			Block.Connect(wheels[num6], 0, differential2, 1);
			Differential differential3 = new Differential();
			Differential differential4 = new Differential();
			differential3.settings = axleDifferential;
			differential4.settings = axleDifferential;
			Block.Connect(wheels[num], 0, differential3, 0);
			Block.Connect(wheels[num2], 0, differential3, 1);
			Block.Connect(differential, 0, differential4, 0);
			Block.Connect(differential2, 0, differential4, 1);
			switch (settings.threeDrivenAxlesConfig)
			{
			case ThreeAxlesConfig.HDriveAndCenterDifferential:
			{
				Differential differential5 = new Differential();
				differential5.settings = centerDifferential;
				Block.Connect(differential3, 0, differential5, 0);
				Block.Connect(differential4, 0, differential5, 1);
				Block.Connect(differential5, 0, powerTrainOutput, 0);
				m_detachableAxleDifferentials = new Differential[0];
				break;
			}
			case ThreeAxlesConfig.HDriveAndTorqueSplitterAtLinkedAxles:
			{
				TorqueSplitter torqueSplitter2 = new TorqueSplitter();
				torqueSplitter2.settings = this.torqueSplitter;
				Block.Connect(differential4, 0, torqueSplitter2, 0);
				Block.Connect(differential3, 0, torqueSplitter2, 1);
				Block.Connect(torqueSplitter2, 0, powerTrainOutput, 0);
				m_detachableAxleDifferentials = new Differential[1];
				m_detachableAxleDifferentials[0] = differential3;
				break;
			}
			case ThreeAxlesConfig.HDriveAndTorqueSplitterAtIndependentAxle:
			{
				TorqueSplitter torqueSplitter = new TorqueSplitter();
				torqueSplitter.settings = this.torqueSplitter;
				Block.Connect(differential3, 0, torqueSplitter, 0);
				Block.Connect(differential4, 0, torqueSplitter, 1);
				Block.Connect(torqueSplitter, 0, powerTrainOutput, 0);
				m_detachableAxleDifferentials = new Differential[1];
				m_detachableAxleDifferentials[0] = differential4;
				break;
			}
			}
		}
		else
		{
			Debug.LogError("Invalid driven axles. Configure Driveline > First and Second axles to valid values.");
		}
	}

	public void SetupFourAxlesDriveline(Wheel[] wheels, Block powerTrainOutput)
	{
		if (settings.firstDrivenAxle * 2 < wheels.Length && settings.secondDrivenAxle * 2 < wheels.Length && settings.thirdDrivenAxle * 2 < wheels.Length && settings.fourthDrivenAxle * 2 < wheels.Length)
		{
			int num = settings.firstDrivenAxle * 2;
			int num2 = settings.firstDrivenAxle * 2 + 1;
			int num3 = settings.secondDrivenAxle * 2;
			int num4 = settings.secondDrivenAxle * 2 + 1;
			int num5 = settings.thirdDrivenAxle * 2;
			int num6 = settings.thirdDrivenAxle * 2 + 1;
			int num7 = settings.fourthDrivenAxle * 2;
			int num8 = settings.fourthDrivenAxle * 2 + 1;
			switch (settings.fourDrivenAxlesConfig)
			{
			case FourAxlesConfig.DualHDriveAndCenterDifferential:
			case FourAxlesConfig.DualHDriveAndTorqueSplitter:
			case FourAxlesConfig.FullHDrive:
			{
				Differential differential8 = new Differential();
				Differential differential9 = new Differential();
				Differential differential10 = new Differential();
				Differential differential11 = new Differential();
				differential8.settings.gearRatio = 1f;
				differential9.settings.gearRatio = 1f;
				differential10.settings.gearRatio = 1f;
				differential11.settings.gearRatio = 1f;
				differential8.settings.type = Differential.Type.Locked;
				differential9.settings.type = Differential.Type.Locked;
				differential10.settings.type = Differential.Type.Locked;
				differential11.settings.type = Differential.Type.Locked;
				Block.Connect(wheels[num], 0, differential8, 0);
				Block.Connect(wheels[num3], 0, differential8, 1);
				Block.Connect(wheels[num2], 0, differential9, 0);
				Block.Connect(wheels[num4], 0, differential9, 1);
				Block.Connect(wheels[num5], 0, differential10, 0);
				Block.Connect(wheels[num7], 0, differential10, 1);
				Block.Connect(wheels[num6], 0, differential11, 0);
				Block.Connect(wheels[num8], 0, differential11, 1);
				if (settings.fourDrivenAxlesConfig != FourAxlesConfig.DualHDriveAndCenterDifferential && settings.fourDrivenAxlesConfig != FourAxlesConfig.DualHDriveAndTorqueSplitter)
				{
					if (settings.fourDrivenAxlesConfig == FourAxlesConfig.FullHDrive)
					{
						Differential differential12 = new Differential();
						Differential differential13 = new Differential();
						differential12.settings.gearRatio = 1f;
						differential13.settings.gearRatio = 1f;
						differential12.settings.type = Differential.Type.Locked;
						differential13.settings.type = Differential.Type.Locked;
						Block.Connect(differential8, 0, differential12, 0);
						Block.Connect(differential10, 0, differential12, 1);
						Block.Connect(differential9, 0, differential13, 0);
						Block.Connect(differential11, 0, differential13, 1);
						Differential differential14 = new Differential();
						differential14.settings = axleDifferential;
						Block.Connect(differential12, 0, differential14, 0);
						Block.Connect(differential13, 0, differential14, 1);
						Block.Connect(differential14, 0, powerTrainOutput, 0);
						m_detachableAxleDifferentials = new Differential[0];
					}
					break;
				}
				Differential differential15 = new Differential();
				Differential differential16 = new Differential();
				differential15.settings = axleDifferential;
				differential16.settings = axleDifferential;
				Block.Connect(differential8, 0, differential15, 0);
				Block.Connect(differential9, 0, differential15, 1);
				Block.Connect(differential10, 0, differential16, 0);
				Block.Connect(differential11, 0, differential16, 1);
				if (settings.fourDrivenAxlesConfig == FourAxlesConfig.DualHDriveAndCenterDifferential)
				{
					Differential differential17 = new Differential();
					differential17.settings = centerDifferential;
					Block.Connect(differential15, 0, differential17, 0);
					Block.Connect(differential16, 0, differential17, 1);
					Block.Connect(differential17, 0, powerTrainOutput, 0);
					m_detachableAxleDifferentials = new Differential[0];
				}
				else if (settings.fourDrivenAxlesConfig == FourAxlesConfig.DualHDriveAndTorqueSplitter)
				{
					TorqueSplitter torqueSplitter2 = new TorqueSplitter();
					torqueSplitter2.settings = this.torqueSplitter;
					Block.Connect(differential15, 0, torqueSplitter2, 0);
					Block.Connect(differential16, 0, torqueSplitter2, 1);
					Block.Connect(torqueSplitter2, 0, powerTrainOutput, 0);
					m_detachableAxleDifferentials = new Differential[1];
					m_detachableAxleDifferentials[0] = differential16;
				}
				break;
			}
			case FourAxlesConfig.DualInterAxleAndCenterDifferential:
			case FourAxlesConfig.DualInterAxleDifferentialAndTorqueSplitter:
			{
				Differential differential = new Differential();
				Differential differential2 = new Differential();
				Differential differential3 = new Differential();
				Differential differential4 = new Differential();
				differential.settings = axleDifferential;
				differential2.settings = axleDifferential;
				differential3.settings = axleDifferential;
				differential4.settings = axleDifferential;
				Block.Connect(wheels[num], 0, differential, 0);
				Block.Connect(wheels[num2], 0, differential, 1);
				Block.Connect(wheels[num3], 0, differential2, 0);
				Block.Connect(wheels[num4], 0, differential2, 1);
				Block.Connect(wheels[num5], 0, differential3, 0);
				Block.Connect(wheels[num6], 0, differential3, 1);
				Block.Connect(wheels[num7], 0, differential4, 0);
				Block.Connect(wheels[num8], 0, differential4, 1);
				Differential differential5 = new Differential();
				Differential differential6 = new Differential();
				differential5.settings = interAxleDifferential;
				differential6.settings = interAxleDifferential;
				Block.Connect(differential, 0, differential5, 0);
				Block.Connect(differential2, 0, differential5, 1);
				Block.Connect(differential3, 0, differential6, 0);
				Block.Connect(differential4, 0, differential6, 1);
				if (settings.fourDrivenAxlesConfig == FourAxlesConfig.DualInterAxleAndCenterDifferential)
				{
					Differential differential7 = new Differential();
					differential7.settings = centerDifferential;
					Block.Connect(differential5, 0, differential7, 0);
					Block.Connect(differential6, 0, differential7, 1);
					Block.Connect(differential7, 0, powerTrainOutput, 0);
					m_detachableAxleDifferentials = new Differential[0];
				}
				else if (settings.fourDrivenAxlesConfig == FourAxlesConfig.DualInterAxleDifferentialAndTorqueSplitter)
				{
					TorqueSplitter torqueSplitter = new TorqueSplitter();
					torqueSplitter.settings = this.torqueSplitter;
					Block.Connect(differential5, 0, torqueSplitter, 0);
					Block.Connect(differential6, 0, torqueSplitter, 1);
					Block.Connect(torqueSplitter, 0, powerTrainOutput, 0);
					m_detachableAxleDifferentials = new Differential[2];
					m_detachableAxleDifferentials[0] = differential3;
					m_detachableAxleDifferentials[1] = differential4;
				}
				break;
			}
			}
		}
		else
		{
			Debug.LogError("Invalid driven axles. Configure Driveline > First and Second axles to valid values.");
		}
	}
}
