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
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("error, cannot delete station", ex);
            }
        }

        public IEnumerable<BO.Station> GetAllStations()//move through all stationsDO, make them stationsBO and return the list of stationBO
        {
            //return from item in dl.GetStudentListWithSelectedFields( (stud) => { return GetStudent(stud.ID); } )
            //       let student = item as BO.Student
            //       orderby student.ID
            //       select student;
            return from stationDO in dl.GetAllStations()//ask dl to provide all the DO.stations, make them BO.stations and return the list.
                   orderby stationDO.Code           //order it by their code
                   select stationDoBoAdapter(stationDO);
        }

        BO.Station stationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            DO.Station newStationDO;//before copying stationDO to stationBO, we need to ensure that stationDO is legal- legal code.
            //sometimes we get here after the user filled stationDO fields. thats why we copy the given stationDO to a new stationDO and check if it is legal.
            int code = stationDO.Code;
            try
            {
                newStationDO = dl.GetStation(code);//if code is legal, returns a new stationDO. if not- ecxeption.
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("Station code is illegal", ex);
            }
            newStationDO.CopyPropertiesTo(stationBO);//copies from stationDO to stationBO- only flat properties.

            //now we still need to fill the "lines" list:

            stationBO.lines = from lineStation in dl.GetLineStationsListThatMatchAStation(code)
                              let line = dl.GetLine(lineStation.LineId)
                              select line.CopyDOLineStationToBOLine(lineStation);

            //now we need to restart the "lineStations" list of each line.

            foreach (BO.Line line in stationBO.lines)
            {
                line.lineStations = from lineStationDO in dl.GetLineStationsListOfALine(line.LineId)
                                    let lineStationBO = lineStationDoBoAdapter(lineStationDO)
                                    select lineStationBO;
            }

            return stationBO;
        }

        BO.LineStation lineStationDoBoAdapter(DO.LineStation lineStationDO)
        {
            BO.LineStation lineStationBO = new BO.LineStation();
            DO.LineStation newlineStationDO;//before copying lineStationDO to lineStationBO, we need to ensure that lineStationDO is legal- legal code.
            //sometimes we get here after the user filled lineStationDO fields. thats why we copy the given lineStationDO to a new lineStationDO and check if it is legal.
            int code = lineStationDO.Code;
            try
            {
                newlineStationDO = dl.GetLineStation(code);//if code is legal, returns a new lineStationDO. if not- ecxeption.
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("Station code is illegal", ex);
            }

            newlineStationDO.CopyPropertiesTo(lineStationBO);//copies from stationDO to stationBO- only flat properties.

            //copy the rest needed fields

            lineStationBO.Name = dl.GetStation(code).Name;

            if (lineStationBO.LineStationIndex == 1)
            {
                //lineStationBO.Time = 00:00:00 - default
                //lineStationBO.Distance = 0 - default
            }
            else
            {
                //distance from the former station:
                lineStationBO.Distance = dl.GetAdjacentStations(code).FirstOrDefault(adj => adj.Station2 == code).Distance;
                //time from the former station:
                lineStationBO.Time = dl.GetAdjacentStations(code).FirstOrDefault(adj => adj.Station2 == code).Time;
            }

            return lineStationBO;
        }

        //public IEnumerable<BO.Line> GetAllLines()
        //{
        //    return from stationDO in dl.GetAllStations()
        //           orderby stationDO.Code           //order it by their code
        //           select stationDoBoAdapter(stationDO);

        //}

        public void AddStationToList()
        {

        }
    }
}