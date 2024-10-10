public interface ILiftProvider
{
	bool DisableBodyLift { get; }

	bool IsLifting { get; }

	void OnCenterOfLiftQuery(CenterOfLiftQuery qry);
}
