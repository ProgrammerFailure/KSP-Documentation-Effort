using System;

namespace Contracts;

public interface IContractParameterHost
{
	string Title { get; }

	IContractParameterHost Parent { get; }

	Contract Root { get; }

	int ParameterCount { get; }

	ContractParameter this[int index] { get; }

	ContractParameter this[string id] { get; }

	ContractParameter this[Type type] { get; }

	ContractParameter AddParameter(ContractParameter parameter, string id = null);

	ContractParameter GetParameter(int index);

	ContractParameter GetParameter(string id);

	ContractParameter GetParameter(Type type);

	T GetParameter<T>(string id = null) where T : ContractParameter;

	void RemoveParameter(int index);

	void RemoveParameter(string id);

	void RemoveParameter(Type type);

	void RemoveParameter(ContractParameter parameter);

	void ParameterStateUpdate(ContractParameter p);
}
