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

        #region Station
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

            //foreach (BO.Line line in stationBO.lines)
            //{
            //    line.lineStations = from lineStationDO in dl.GetLineStationsListOfALine(line.LineId)
            //                        let lineStationBO = lineStationDoBoAdapter(lineStationDO)
            //                        select lineStationBO;
            //}

            return stationBO;
        }

        DO.Station stationBoDoAdapter(BO.Station stationBO)
        {
            DO.Station stationDO= new DO.Station();
            //check code of the station:
            if (stationBO.Code < 1 || stationBO.Code > 999999)
                throw new BO.StationException("illegal station code");
            //check longitude and lattitude:
            if(stationBO.Lattitude < 31 || stationBO.Lattitude> 33.3 || stationBO.Longitude < 34.3 || stationBO.Longitude > 35.5)
                throw new BO.StationException("station is not is Israel's teritory. illegal longitude or lattitude.");

            stationBO.CopyPropertiesTo(stationDO);
            return stationDO;
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

            newlineStationDO.CopyPropertiesTo(lineStationBO);//copies- only flat properties.

            //copy the rest needed fields

            lineStationBO.Name = dl.GetStation(code).Name;

            if (lineStationBO.LineStationIndex == 0)
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

        public void AddStationToList(BO.Station newStat)
        {
            try
            {
                //the adapter will check if its logically possible to add the station
                dl.AddStationToList(stationBoDoAdapter(newStat));
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("error, cannot add the station", ex);
            }
        }
        #endregion

        #region Line
        public IEnumerable<BO.Line> GetAllLinesPerStation(int code)
        {
            return from lineStation in dl.GetLineStationsListThatMatchAStation(code)
                   let line = dl.GetLine(lineStation.LineId)
                   select line.CopyDOLineStationToBOLine(lineStation);
        }

        public IEnumerable<BO.Line> GetAllLines()
        {
            return from LineDO in dl.GetAllLines()
                   orderby LineDO.BusNumber           //order it by their bus number
                   select lineDoBoAdapter(LineDO);
        }

        BO.Line lineDoBoAdapter(DO.Line lineDO)
        {
            BO.Line lineBO = new BO.Line();
            DO.Line newlineDO;//before copying lineDO to lineBO, we need to ensure that lineDO is legal- legal busNumber.
            //sometimes we get here after the user filled lineDO fields. thats why we copy the given lineDO to a new lineDO and check if it is legal.
            int lineId = lineDO.LineId;
            try
            {
                newlineDO = dl.GetLine(lineId);//if code is legal, returns a new lineStationDO. if not- ecxeption.
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("Line bus number is illegal", ex);
            }

            newlineDO.CopyPropertiesTo(lineBO);//copies- only flat properties.

            //now we need to restart the "lineStations" list of each line.

            lineBO.lineStations = from lineStationDO in dl.GetLineStationsListOfALine(lineBO.LineId)
                                  let lineStationBO = lineStationDoBoAdapter(lineStationDO)
                                  select lineStationBO;
           
            return lineBO;
        }
        #endregion
    }
}