namespace Expansions.Missions;

public interface IMissionKerbal
{
	void NodeDeleted();

	void KerbalRosterStatusChange(ProtoCrewMember kerbal, ProtoCrewMember.RosterStatus oldStatus, ProtoCrewMember.RosterStatus newStatus);

	void KerbalAdded(ProtoCrewMember kerbal);

	void KerbalRemoved(ProtoCrewMember kerbal);

	void KerbalTypeChange(ProtoCrewMember kerbal, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType);

	void KerbalNameChange(ProtoCrewMember kerbal, string oldName, string newName);
}
