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
                if (DataSource.listLineStations.Exists(st => st.Code == code))
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
            if (DataSource.listStations.Exists(st => st.Code == newStatDO.Code))
                throw new DO.StationException(newStatDO.Code, $"the station that its code is: {newStatDO.Code} already exists in the list");
            DataSource.listStations.Add(newStatDO);
        }

        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetLineStationsListThatMatchAStation(int code)//returns a list of the logical stations (line stations) that match a physical station with a given code.
        {
            return from ls in DataSource.listLineStations
                   where ls.Code == code
                   select ls.Clone();
        }

        public IEnumerable<DO.LineStation> GetLineStationsListOfALine(int lineId)//returns a "line stations" list of the wanted line
        {
            return from ls in DataSource.listLineStations
                   where ls.LineId == lineId
                   select ls.Clone();
        }
        public DO.LineStation GetLineStation(int code, int lineId)//get the line stat by the line and the stat. since a few line stat can apear with the sme code but different lines.
        {
            DO.LineStation stat = DataSource.listLineStations.Find(s => s.Code == code && s.LineId == lineId);

            if (stat != null)//found the station
                return stat.Clone();
            else//didnt find the station
                throw new DO.StationException(code, $"error in line station that its code is: {code}");
        }

        public void DeleteLineStationsOfALine(int lineId)
        {
            DataSource.listLineStations.RemoveAll(ls => ls.LineId == lineId);
        }

        public void DeleteStationFromLine(int code, int lineId)
        {
            int busNumber = GetLine(lineId).BusNumber;

            DO.LineStation lineStation = DataSource.listLineStations.Find(ls => (ls.Code == code && ls.LineId == lineId));

            if (lineStation != null)//found the wanted line station
            {
                int indexInLine = lineStation.LineStationIndex;

                if (indexInLine == 0)
                    throw new DO.LineStationException(code, busNumber, $"the station {code} is the first station of the line {busNumber}");

                if (lineStation.Code == GetLine(lineId).LastStation)
                    throw new DO.LineStationException(code, busNumber, $"the station {code} is the last station of the line {busNumber}");

                DataSource.listLineStations.Remove(lineStation);

                foreach (DO.LineStation ls in DataSource.listLineStations)
                {
                    if (ls.LineId == lineId)
                    {

                        //need to update all the indexes of the line station of the line- minus 1.
                        if (ls.LineStationIndex > indexInLine)
                            ls.LineStationIndex--;
                    }
                }
            }
            else
                throw new DO.LineStationException(code, busNumber, $"the line {busNumber} doesnt pass in the station {code}");

        }
        #endregion

        #region Line
        public void UpdateLine(DO.Line newLine)
        {
            DO.Line ln = DataSource.listLines.Find(l => l.LineId == newLine.LineId);//search for the the line with the same lineId, if exist.

            if (ln != null)//if found
            {
                //check if the line's fields that were added are legal
                //check the code of the stations:
                if (!DataSource.listStations.Exists(l => l.Code == newLine.FirstStation))
                    throw new DO.StationException(newLine.FirstStation, $"the station with the code: {newLine.FirstStation} is not found");
                if (!DataSource.listStations.Exists(l => l.Code == newLine.LastStation))
                    throw new DO.StationException(newLine.LastStation, $"the station with the code: {newLine.LastStation} is not found");

                if (newLine.FirstStation == newLine.LastStation)
                    throw new DO.StationException(newLine.LastStation, $"the last station code: {newLine.LastStation} is illegal since the first and last stations must be different");

                //check if a bus with the same identifying stations (first and last stations) already exists.
                if (DataSource.listLines.Exists(l => l.FirstStation == newLine.FirstStation && l.LastStation == newLine.LastStation))
                    throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} allready exists, with the same first and last stations");
                
                //add the new lineStation and adjacent stations

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
            newLine.LineId = DO.Config.LineId;//running number
            DO.Config.LineId++;//update running number

            //check if the first and last stations are the same- if so, cannot add the bus.
            if (newLine.FirstStation == newLine.LastStation)
                throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} is not legal since the first and last stations must be different");

            //check if a bus with the same identifying stations (first and last stations) already exists.
            if (DataSource.listLines.Exists(l => l.FirstStation == newLine.FirstStation && l.LastStation == newLine.LastStation && l.BusNumber == newLine.BusNumber))
                throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} allready exists, with the same first and last stations");

            //add new lineStations of the new stations
            DataSource.listLineStations.Add(new DO.LineStation() { Code = newLine.FirstStation, LineId = newLine.LineId, LineStationIndex = 0, NextStation = newLine.LastStation, PrevStation = -1 });
            DataSource.listLineStations.Add(new DO.LineStation() { Code = newLine.LastStation, LineId = newLine.LineId, LineStationIndex = 1, NextStation = -1, PrevStation = newLine.FirstStation });

            //add new adjacent stations
            DataSource.listAdjacentStations.Add(new DO.AdjacentStations() { Station1 = newLine.FirstStation, Station2 = newLine.LastStation, Distance = 0.583, Time = new TimeSpan(00, 01, 16) });

            DataSource.listLines.Add(newLine);
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAdjacentStationsByFirstOfPair(int code)
        {
            return from adjSt in DataSource.listAdjacentStations
                   where adjSt.Station1 == code
                   //where adjSt.Station2 == code
                   select adjSt.Clone();//return a list of adjacent stations, in which the station with the given code apears as the 1st in the pair
        }
        public IEnumerable<DO.AdjacentStations> GetAdjacentStationsBySecondOfPair(int code)
        {
            return from adjSt in DataSource.listAdjacentStations
                   where adjSt.Station2 == code
                   select adjSt.Clone();//return a list of adjacent stations, in which the station with the given code apears as the 2nd in the pair
        }

        #endregion

        #region User
        public DO.AppUser GetUser(string myname,string mypassword)
        {
            DO.AppUser user = DataSource.users.FirstOrDefault(u => u.UserName == myname&& u.Password==mypassword);//changed //
            //try { Thread.Sleep(2000); } catch (ThreadInterruptedException ex) { }
            if (user != null)
            {
                return user.Clone();
            }
            else
            {
                
                throw new DO.AppUserException(myname, $"the user: {myname} does'nt exist in the curret state");
            }

        }
        //public IEnumerable<DO.AdjacentStations> GetAdjacentStationsByFirstOfPair(int code)
        //{
        //    return from adjSt in DataSource.listAdjacentStations
        //           where adjSt.Station1 == code
        //           //where adjSt.Station2 == code
        //           select adjSt.Clone();//return a list of adjacent stations, in which the station with the given code apears as the 1st in the pair
        //}
        //public IEnumerable<DO.AdjacentStations> GetAdjacentStationsBySecondOfPair(int code)
        //{
        //    return from adjSt in DataSource.listAdjacentStations
        //           where adjSt.Station2 == code
        //           select adjSt.Clone();//return a list of adjacent stations, in which the station with the given code apears as the 2nd in the pair
        //}

        public IEnumerable<DO.AppUser> GetAllUsers()
        {
            return from user in DataSource.users
                   select user.Clone();
        }
        public void AddUser(DO.AppUser user)
        {
            if (DataSource.users.Where(s => s.UserName == user.UserName).ToList().Count() > 0)
            {
                // throw new DO.BadStationCodeException(user.Name, "Duplicate user Code");
            }
            DataSource.users.Add(user);
        }
        #endregion

        #region LineTrip
        public IEnumerable<DO.LineTrip> GetAllLineTripPerLine(int lineid)
        {
            return from lnTrip in DataSource.listLineTrips//return all line trips of a specific line.
                   where lnTrip.LineID == lineid
                   select lnTrip.Clone();
        }
        #endregion

    }
}