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


        public DO.Station GetStation(int code)
        {
            DO.Station stat = DataSource.listStations.Find(s => s.Code == code);
            try { Thread.Sleep(2000); } catch (ThreadInterruptedException e) { }
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
                throw new DO.StationException(newStat.Code, $"error in station that its code is: {newStat.Code}");
        }

        //public void DeleteStation(int id)
        //{
        //    DO.Person per = DataSource.ListPersons.Find(p => p.ID == id);

        //    if (per != null)
        //    {
        //        DataSource.ListPersons.Remove(per);
        //    }
        //    else
        //        throw new DO.BadPersonIdException(id, $"bad person id: {id}");
        //}

        public IEnumerable<DO.Station> GetAllStations()
        {
            return from station in DataSource.listStations
                   select station.Clone();
        }

        //public IEnumerable<DO.Line> GetAllLines(int code)
        //{
        //    var lines=from line in DataSource.listLines
        //              select

        //    return from line in DataSource.listLines
        //           select line.Clone();

        //    IEnumerable<int> lineNumbers=
        //}

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

        public DO.Line GetLine(int lineId)
        {
            return DataSource.listLines.Find(l => l.LineId == lineId).Clone();
        }

        DO.LineStation GetLineStation(int code)
        {
            DO.LineStation stat = DataSource.listLineStations.Find(s => s.Code == code);
            try { Thread.Sleep(2000); } catch (ThreadInterruptedException e) { }
            if (stat != null)//found the station
                return stat.Clone();
            else//didnt find the station
                throw new DO.StationException(code, $"error in line station that its code is: {code}");
        }

    }
}