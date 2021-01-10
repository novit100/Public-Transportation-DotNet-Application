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
        void UpdateStationDetails(BO.Station currStat);
        IEnumerable<BO.Station> GetAllStations();

        //IEnumerable<BO.Line> GetAllLines();

        void DeleteStation(int code);
    }
}
