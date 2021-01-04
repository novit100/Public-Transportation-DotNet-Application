using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using BLAPI;
using System.Threading;

namespace BL
{
    class BLImp : IBL //internal
    {
        IDL dl = DLFactory.GetDL();//we create an "object" of IDL interface in order to use DL functions and classes

        public void UpdateStationDetails(BO.Station currStat)
        {
            //Update DO.Station            
            DO.Station stationDO = new DO.Station();
            currStat.CopyPropertiesTo(stationDO);
            try
            {
                dl.UpdateStation(stationDO);
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("Station Code is illegal", ex);
            }

        }

        public void DeleteStation(int code)
        {
            try
            {
                dl.DeleteStation(code);
                dl.DeleteStudentFromAllCourses(code);
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("error, cannot delete station", ex);
            }
        }

    }
}
