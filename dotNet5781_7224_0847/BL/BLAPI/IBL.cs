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
        void UpdateStationDetails(BO.Station currStat);
        IEnumerable<BO.Station> GetAllStations();
        void DeleteStation(int code);
        #endregion

        IEnumerable<BO.Line> GetAllLinesPerStation(int code);
        //IEnumerable<BO.Line> GetAllLines();


    }
}
