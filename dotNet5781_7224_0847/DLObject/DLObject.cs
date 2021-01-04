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

        public void DeleteStation(int id)
        {
            DO.Person per = DataSource.ListPersons.Find(p => p.ID == id);

            if (per != null)
            {
                DataSource.ListPersons.Remove(per);
            }
            else
                throw new DO.BadPersonIdException(id, $"bad person id: {id}");
        }
    }
}