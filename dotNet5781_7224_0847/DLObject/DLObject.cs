using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DLAPI;
//using DO;
using DS;

namespace DL
{   
    sealed class DLObject : IDL    //internal

    {
        //----------
        #region singelton
        static readonly DLObject instance = new DLObject();
        static DLObject() { }// static ctor to ensure instance init is done just before first usage
        DLObject() { } // default => private
        public static DLObject Instance { get => instance; }// The public Instance property to use
        #endregion

        //Implement IDL methods, CRUD

        #region Station
        public DO.Station GetStation(int code)
        {
            DO.Station stat = DataSource.listStations.Find(s => s.Code == code);
            
            if (stat != null)//found the station
                return stat.Clone();
            else//didnt find the station
                throw new DO.StationException(code, $"error in station that its code is: {code}");
        }

        public void UpdateStation(DO.Station newStat)
        {
            DO.Station st = DataSource.listStations.Find(s => s.Code == newStat.Code);//search for the the station with the same code, if exist.

            if (st != null)//if found
            {
                DataSource.listStations.Remove(st);
                DataSource.listStations.Add(newStat.Clone());
            }
            else
                throw new DO.StationException(newStat.Code, $"the station that its code is: {newStat.Code} was not found");
        }

        public void DeleteStation(int code)
        {
            DO.Station stationToDel = DataSource.listStations.Find(st => st.Code == code);

            if (stationToDel != null)//station was found
            {
                //if there are already lines that pass in this station, we cannot delete the station. we need to delete those lines first.
                if(DataSource.listLineStations.Exists(st => st.Code == code))
                    throw new DO.StationException(code, $"the station that its code is: {code} is in the path of bus/es");

                DataSource.listStations.Remove(stationToDel);
            }
            else
                throw new DO.StationException(code, $"the station that its code is: {code} wasnt found");
        }

        public IEnumerable<DO.Station> GetAllStations()
        {
            return from station in DataSource.listStations
                   select station.Clone();
        }

        public void AddStationToList(DO.Station newStatDO)
        {
            if(DataSource.listStations.Exists(st=>st.Code==newStatDO.Code))
                throw new DO.StationException(newStatDO.Code, $"the station that its code is: {newStatDO.Code} already exists in the list");
            DataSource.listStations.Add(newStatDO);
        }

        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetLineStationsListThatMatchAStation(int code)//returns a list of the logical stations (line stations) that match a physical station with a given code.
        {
            return from ls in DataSource.listLineStations
                   where ls.Code==code
                   select ls.Clone();
        }

        public IEnumerable<DO.LineStation> GetLineStationsListOfALine(int lineId)//returns a "line stations" list of the wanted line
        {
            return from ls in DataSource.listLineStations
                   where ls.LineId == lineId
                   select ls.Clone();
        }
        public DO.LineStation GetLineStation(int code)
        {
            DO.LineStation stat = DataSource.listLineStations.Find(s => s.Code == code);
            
            if (stat != null)//found the station
                return stat.Clone();
            else//didnt find the station
                throw new DO.StationException(code, $"error in line station that its code is: {code}");
        }

        public void DeleteLineStationsOfALine(int lineId)
        {
            DataSource.listLineStations.RemoveAll(ls => ls.LineId == lineId);
        }
        #endregion

        #region Line
        public void UpdateLine(DO.Line newLine)
        {
            DO.Line ln = DataSource.listLines.Find(l => l.LineId == newLine.LineId);//search for the the line with the same lineId, if exist.

            if (ln != null)//if found
            {
                DataSource.listLines.Remove(ln);
                DataSource.listLines.Add(newLine.Clone());
            }
            else
                throw new DO.LineException(newLine.BusNumber, $"the station that its code is: {newLine.BusNumber} was not found");

        }

        public DO.Line GetLine(int lineId, int busNumber)
        {
            DO.Line line = DataSource.listLines.Find(l => l.LineId == lineId);

            if (line != null)//the line was found
                return line.Clone();
            else
                throw new DO.LineException(busNumber, $"error in line: {busNumber}");
        }

        public DO.Line GetLine(int lineId)
        {
            return DataSource.listLines.Find(l => l.LineId == lineId).Clone();
        }

        public IEnumerable<DO.Line> GetAllLines()
        {
            return from line in DataSource.listLines
                   select line.Clone();
        }

        public void DeleteLine(int lineId, int busNumber)
        {
            DO.Line lineToDel = DataSource.listLines.Find(ln => ln.LineId == lineId);

            if (lineToDel != null)//line was found
            {
                //delete all line stations of the line
                DeleteLineStationsOfALine(lineId);
                //then delete the line itself
                DataSource.listLines.Remove(lineToDel);
            }
            else
                throw new DO.LineException(busNumber, $"the line: {busNumber} wasnt found");
        }

        public void AddLineToList(DO.Line newLine)
        {
            //check if a bus with the same identifying stations (first and last stations) already exists.
            if (DataSource.listLines.Exists(l => l.FirstStation == newLine.FirstStation && l.LastStation==newLine.LastStation))
                throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} allready exists, with the same first and last stations");

            //check the code of the stations:
            if (newLine.FirstStation < 1 || newLine.FirstStation > 999999)
                throw new DO.StationException(newLine.FirstStation, $"the first station code: {newLine.FirstStation} is illegal");
            if (newLine.LastStation < 1 || newLine.LastStation > 999999)
                throw new DO.StationException(newLine.LastStation, $"the last station code: {newLine.LastStation} is illegal");

            //create new stations and fill them with default values. 
            //(only if the first or last station dont exist yet))
            if (!DataSource.listStations.Exists(st => st.Code == newLine.FirstStation))
                DataSource.listStations.Add(new DO.Station() { Code = newLine.FirstStation, Lattitude=31.5, Longitude=35, Name="First station of line "+newLine.BusNumber });
            if (!DataSource.listStations.Exists(st => st.Code == newLine.LastStation))
                DataSource.listStations.Add(new DO.Station() { Code = newLine.LastStation, Lattitude = 32, Longitude = 35.3, Name = "Last station of line " + newLine.BusNumber });
            
            //add new lineStations of the new stations
            DataSource.listLineStations.Add(new DO.LineStation() { Code = newLine.FirstStation, LineId = newLine.LineId, LineStationIndex = 0, NextStation = newLine.LastStation, PrevStation = -1 });
            DataSource.listLineStations.Add(new DO.LineStation() { Code=newLine.LastStation, LineId= newLine.LineId, LineStationIndex=1, NextStation=-1, PrevStation=newLine.FirstStation});

            //add new adjacent stations
            DataSource.listAdjacentStations.Add(new DO.AdjacentStations() { Station1=newLine.FirstStation, Station2=newLine.LastStation, Distance=0.583, Time=new TimeSpan(00,01,16)});

            newLine.LineId= DO.Config.LineId++;//update running number

            DataSource.listLines.Add(newLine); 
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAdjacentStations(int code)
        {
            return from adjSt in DataSource.listAdjacentStations
                   where adjSt.Station1 == code
                   where adjSt.Station2 == code
                   select adjSt.Clone();//return a list of adjacent stations, in which the station with the given code apears as the 1st/2nd in the pair
        }
        #endregion

    }
}