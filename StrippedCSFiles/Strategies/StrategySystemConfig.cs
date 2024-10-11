using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Strategies;

[Serializable]
public class StrategySystemConfig
{
	private List<DepartmentConfig> departments;

	private List<StrategyConfig> strategies;

	public List<DepartmentConfig> Departments
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<StrategyConfig> Strategies
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StrategySystemConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadDepartmentConfigs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadStrategyConfigs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StrategyConfig GetStrategyConfig(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DepartmentConfig GetDepartmentConfig(string name)
	{
		throw null;
	}
}
