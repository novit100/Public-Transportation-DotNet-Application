using System;
using System.Collections.Generic;

//using DO;

namespace DLAPI
{
    //CRUD Logic:
    // Create - add new instance
    // Request - ask for an instance or for a collection
    // Update - update properties of an instance
    // Delete - delete an instance
    public interface IDL
    {
        DO.Station GetStation(int code);
        void UpdateStation(DO.Station station);
        //void DeleteStation(int code);
        IEnumerable<DO.Station> GetAllStations();

        IEnumerable<DO.LineStation> GetLineStationsListThatMatchAStation(int code);
        IEnumerable<DO.LineStation> GetLineStationsListOfALine(int lineId);

        DO.LineStation GetLineStation(int code);

        DO.Line GetLine(int lineId);

        //IEnumerable<DO.Line> GetAllLines();
    }
}
