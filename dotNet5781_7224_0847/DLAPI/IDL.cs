﻿using System;
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
        #region Station
        DO.Station GetStation(int code);
        void UpdateStation(DO.Station station);
        void DeleteStation(int code);
        IEnumerable<DO.Station> GetAllStations();
        void AddStationToList(DO.Station newStat);
        #endregion

        #region LineStation
        void AddLineStation(DO.LineStation newLineStat);
        IEnumerable<DO.LineStation> GetLineStationsListThatMatchAStation(int code);
        IEnumerable<DO.LineStation> GetLineStationsListOfALine(int lineId);
        DO.LineStation GetLineStation(int code);
        void DeleteLineStationsOfALine(int lineId);
        #endregion

        #region Line
        void UpdateLine(DO.Line line);
        DO.Line GetLine(int lineId);
        IEnumerable<DO.Line> GetAllLines();
        void DeleteLine(int lineId, int busNumber);
        void AddLineToList(DO.Line newLine);
        #endregion

        #region AdjacentStations
        IEnumerable<DO.AdjacentStations> GetAdjacentStations(int code);
        #endregion
        
    }
}
