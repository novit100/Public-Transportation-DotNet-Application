using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BO;


namespace BLAPI
{
    public interface IBL
    {
        #region Station
        BO.Station GetStation(int code);
        void UpdateStationDetails(BO.Station currStat);
        IEnumerable<BO.Station> GetAllStations();
        void DeleteStation(int code);
        void AddStationToList(BO.Station newStat);
        #endregion

        #region Line
        void UpdateLineDetails(BO.Line currLine);
        IEnumerable<BO.Line> GetAllLinesPerStation(int code);
        IEnumerable<BO.Line> GetAllLines();
        void DeleteLine(int lineId, int busNumber);
        void AddLineToList(BO.Line newLine);
        #endregion

        #region LineStation
        //DO.LineStation GetLineStation(int code);
        IEnumerable<BO.LineStation> GetAllLineStationsPerLine(int LineId);
        void DeleteStationFromLine(int Code, int LineId);
        #endregion
        #region User
        BO.AppUser GetUser(string name);
        void AddUser(DO.AppUser user);
        IEnumerable<BO.AppUser> GetAllUsers();
        #endregion
        // get user 
        // add user
        // get all users
    }
}
