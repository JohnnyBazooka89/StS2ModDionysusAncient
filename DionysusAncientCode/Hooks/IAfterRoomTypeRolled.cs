using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DionysusAncient.DionysusAncientCode.Hooks;

public interface IAfterRoomTypeRolled
{
    void AfterRoomTypeRolled(IRunState runState, RoomType roomType);
}