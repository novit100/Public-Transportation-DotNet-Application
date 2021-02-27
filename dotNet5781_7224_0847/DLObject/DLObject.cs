using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
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

        public void restartXmlLists()
        {
            //this func is called by the firstwindow of PL, we want to save all the lists in xml files, for DLXML.

            #region save all list from data source to xml files

            //string stationsPath = @"StationsXml.xml"; //XMLSerializer
            //string lineStationsPath = @"LineStationsXml.xml"; //XMLSerializer
            //string linesPath = @"LinesXml.xml"; //XMLSerializer
            //string adjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement
            //string usersPath = @"UsersXml.xml"; //XMLSerializer
            //string lineTripPath = @"LineTripXml.xml"; //XElement

            //string runningNumberPath = @"RunningNumberXml.xml";//=config of lineID++

            //SaveListToXMLSerializer(DataSource.listStations, stationsPath);
            //SaveListToXMLSerializer(DataSource.listLineStations, lineStationsPath);
            //SaveListToXMLSerializer(DataSource.listLines, linesPath);
            //SaveListToXMLSerializer(DataSource.users, usersPath);


            //XElement adjacentStationsRootElem = new XElement("listAdjacentStations");//the root

            //foreach (DO.AdjacentStations adj in DataSource.listAdjacentStations)
            //{
            //    //each time add another "adjacent stations" to the root
            //    adjacentStationsRootElem.Add(new XElement("AdjacentStations",
            //        new XElement("Station1", adj.Station1.ToString()),
            //        new XElement("Station2", adj.Station2.ToString()),
            //        new XElement("Distance", adj.Distance.ToString()),
            //        new XElement("Time", adj.Time.ToString())));
            //}

            //SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);//save the new root in the xml file


            //XElement lineTripRootElem = new XElement("listLineTrips");//the root

            //foreach (DO.LineTrip lnTrip in DataSource.listLineTrips)
            //{
            //    //each time add another "line trip" to the root
            //    lineTripRootElem.Add(new XElement("LineTrip",
            //        new XElement("LineTripID", lnTrip.LineTripID.ToString()),
            //        new XElement("LineID", lnTrip.LineID.ToString()),
            //        new XElement("StartAt", lnTrip.StartAt.ToString())));
            //}

            //SaveListToXMLElement(lineTripRootElem, lineTripPath);//save the new root in the xml file


            //XElement runningNumberRootElem = new XElement("runningNumber");//the root

            //runningNumberRootElem.Add(new XElement("LineId", "13"));//as there is in "Config" in the begining.
            //SaveListToXMLElement(runningNumberRootElem, runningNumberPath);//save the new root in the xml file


            #endregion
        }

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
                   orderby ls.LineStationIndex
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

                ////check if a bus with the same identifying stations (first and last stations) already exists.
                //if (DataSource.listLines.Exists(l => l.FirstStation == newLine.FirstStation && l.LastStation == newLine.LastStation))
                //    throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} allready exists, with the same first and last stations");

                //add new lineStations of the new stations
                //delete the first linestation and second, and rewrite their details.
                DO.LineStation ls1 = DataSource.listLineStations.Find(ls => ls.LineStationIndex == 0 && ls.LineId == ln.LineId);//change the original first station
                DataSource.listLineStations.Remove(ls1);
                ls1.Code = newLine.FirstStation;
                DataSource.listLineStations.Add(ls1);

                DO.LineStation ls2 = DataSource.listLineStations.Find(ls => ls.LineStationIndex == 1 && ls.LineId == ln.LineId);//change the original 2nd station
                DataSource.listLineStations.Remove(ls2);
                ls2.PrevStation = newLine.FirstStation;
                DataSource.listLineStations.Add(ls2);

                //delete the last linestation and the one before the last, and rewrite their details.
                DO.LineStation lsBeforeLast = DataSource.listLineStations.Find(ls => ls.NextStation==ln.LastStation && ls.LineId == ln.LineId);//change the original station before last
                DataSource.listLineStations.Remove(lsBeforeLast);
                lsBeforeLast.NextStation = newLine.LastStation;
                DataSource.listLineStations.Add(lsBeforeLast);

                DO.LineStation lsLast = DataSource.listLineStations.Find(ls => ls.NextStation == -1 && ls.LineId == ln.LineId);//change the original last station
                DataSource.listLineStations.Remove(lsLast);
                lsLast.Code = newLine.LastStation;
                DataSource.listLineStations.Add(lsLast);

                //add new adjacent stations
                DataSource.listAdjacentStations.Add(new DO.AdjacentStations() { Station1 = newLine.FirstStation, Station2 = ls2.Code, Distance = 0.583, Time = new TimeSpan(00, 01, 16) });
                DataSource.listAdjacentStations.Add(new DO.AdjacentStations() { Station1 = lsBeforeLast.Code, Station2 = newLine.LastStation, Distance = 0.702, Time = new TimeSpan(00, 03, 45) });

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

        private static Random r = new Random();

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

            //check if a bus with the same number and the same area already exists.
            if (DataSource.listLines.Exists(l => l.Area==newLine.Area && l.BusNumber == newLine.BusNumber))
                throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} allready exists in this area");


            //add new lineStations of the new stations
            DataSource.listLineStations.Add(new DO.LineStation() { Code = newLine.FirstStation, LineId = newLine.LineId, LineStationIndex = 0, NextStation = newLine.LastStation, PrevStation = -1 });
            DataSource.listLineStations.Add(new DO.LineStation() { Code = newLine.LastStation, LineId = newLine.LineId, LineStationIndex = 1, NextStation = -1, PrevStation = newLine.FirstStation });

            #region add new adjacent stations

            DO.AdjacentStations newAdj = new DO.AdjacentStations();

            newAdj.Station1 = newLine.FirstStation;
            newAdj.Station2 = newLine.LastStation;

            double long1 = DataSource.listStations.Find(st => st.Code == newAdj.Station1).Longitude;
            double long2 = DataSource.listStations.Find(st => st.Code == newAdj.Station2).Longitude;
            double lat1 = DataSource.listStations.Find(st => st.Code == newAdj.Station1).Lattitude;
            double lat2 = DataSource.listStations.Find(st => st.Code == newAdj.Station2).Lattitude;

            //the distance between each 2 cordinates is 111 km.
            //the hefresh between lat1-lat2 and long1-long2, is a part of the distance between the lat lines and long lines witch is 111 each.
            newAdj.Distance = (Math.Sqrt((Math.Pow(long1 - long2, 2) * 111) + (Math.Pow(lat1 - lat2, 2)) * 111));

            double timeInSec = ((newAdj.Distance * 1.5) / r.Next(20, 50)) * 60 * 60;//dis*1.5= לחישוב מרחק אמיתי ולא אוירי
            int hours = (int)(timeInSec / 3600);
            int min = (int)(timeInSec / 60);
            int sec = (int)timeInSec;

            if (sec == 0)
            {
                sec = 1;//since we dont want the TimeSpan to be all 00:00:00. at least 00:00:01.
            }

            TimeSpan time = new TimeSpan(hours, min, sec);
            newAdj.Time = time;
            
            DataSource.listAdjacentStations.Add(newAdj);
            #endregion

            //add line trips to the line
            int numTrips = r.Next(20, 30);
            for (int i = 0; i < numTrips; i++)
            {
                DO.LineTrip lnTrip = new DO.LineTrip();
                lnTrip.LineID = newLine.LineId;
                lnTrip.LineTripID = i;
                TimeSpan start = new TimeSpan(r.Next(5, 24), r.Next(0, 11) * 5, 0);
                lnTrip.StartAt = start;
                DataSource.listLineTrips.Add(lnTrip);
            }

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
            DO.AppUser user = DataSource.users.FirstOrDefault(u => u.UserName == myname && u.Password==mypassword);

            if (user != null)//if user was found
            {
                return user.Clone();
            }
            else
            {
                throw new DO.AppUserException(myname, $"the user: {myname} does'nt exist in the curret state");
            }

        }

        public IEnumerable<DO.AppUser> GetAllUsers()
        {
            return from user in DataSource.users
                   select user.Clone();
        }

        #region "save" functions of serializer and XElenent
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            string dir = @"xml\";
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            string dir = @"xml\";
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        #endregion

        public void AddUser(DO.AppUser user)
        {
            if (DataSource.users.Where(s => s.UserName == user.UserName).ToList().Count() > 0)
            {
                throw new DO.AppUserException(user.UserName, $"the user: {user.UserName} allready exists. Choose another name");
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

        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {
            return from lTrip in DataSource.listLineTrips
                   where predicate(lTrip)
                   select lTrip.Clone();
        }
        #endregion

    }
}