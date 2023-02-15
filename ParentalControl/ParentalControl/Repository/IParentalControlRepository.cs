using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Repository
{
    public interface IParentalControlRepository
    {
        IOnlineActivitiesRepository OnlineActivitiesRepository { get; }
        IApplicationActivitiesRepository ApplicationActivitiesRepository { get; }
        IChildDevicesRepository ChildDevicesRepository { get; }
        ILocationActivitiesRepository LocationActivitiesRepository { get; }
        ICommunicationActivitiesRepository CommunicationActivitiesRepository { get; }
    }
}
