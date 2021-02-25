using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLAPI;
using DO;

namespace DL
{
    class DLXML : IDL
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }// static ctor to ensure instance init is done just before first usage
        DLXML() { } // default => private
        public static DLXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        string stationsPath = @"StationsXml.xml"; //XMLSerializer
        string lineStationsPath = @"LineStationsXml.xml"; //XMLSerializer
        string linesPath = @"LinesXml.xml"; //XMLSerializer
        string adjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement
        string usersPath = @"UsersXml.xml"; //XMLSerializer
        string lineTripPath = @"LineTripXml.xml"; //XElement

        string runningNumberPath = @"RunningNumberXml.xml";//=config of lineID++

        #endregion

        #region Station
        public DO.Station GetStation(int code)
        {
            List<Station> listStations= XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            DO.Station stat = listStations.Find(s => s.Code == code);

            if (stat != null)//found the station
                return stat;//no need to Clone(), cloning is needed when we want to copy the object when we bring it to the upper layer, to prevent the ability to change the dataSource. but here we copy it from the xml file anyway.
            else//didnt find the station
                throw new DO.StationException(code, $"error in station that its code is: {code}");
        }

        public void UpdateStation(DO.Station newStat)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            DO.Station st = listStations.Find(s => s.Code == newStat.Code);//search for the the station with the same code, if exist.

            if (st != null)//if found
            {
                listStations.Remove(st);
                listStations.Add(newStat);
            }
            else
                throw new DO.StationException(newStat.Code, $"the station that its code is: {newStat.Code} was not found");
            XMLTools.SaveListToXMLSerializer(listStations, stationsPath);
        }

        public void DeleteStation(int code)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            DO.Station stationToDel = listStations.Find(st => st.Code == code);

            if (stationToDel != null)//station was found
            {
                //if there are already lines that pass in this station, we cannot delete the station. we need to delete those lines first.
                if (listLineStations.Exists(st => st.Code == code))
                    throw new DO.StationException(code, $"the station that its code is: {code} is in the path of bus/es");

                listStations.Remove(stationToDel);
            }
            else
                throw new DO.StationException(code, $"the station that its code is: {code} wasnt found");
            XMLTools.SaveListToXMLSerializer(listStations, stationsPath);
        }

        public IEnumerable<DO.Station> GetAllStations()
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            return from station in listStations
                   select station;
        }

        public void AddStationToList(DO.Station newStatDO)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);

            if (listStations.Exists(st => st.Code == newStatDO.Code))
                throw new DO.StationException(newStatDO.Code, $"the station that its code is: {newStatDO.Code} already exists in the list");
            listStations.Add(newStatDO);
            XMLTools.SaveListToXMLSerializer(listStations, stationsPath);
        }

        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetLineStationsListThatMatchAStation(int code)//returns a list of the logical stations (line stations) that match a physical station with a given code.
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            return from ls in listLineStations
                   where ls.Code == code
                   select ls;
        }

        public IEnumerable<DO.LineStation> GetLineStationsListOfALine(int lineId)//returns a "line stations" list of the wanted line
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            return from ls in listLineStations
                   where ls.LineId == lineId
                   orderby ls.LineStationIndex
                   select ls;
        }
        public DO.LineStation GetLineStation(int code, int lineId)//get the line stat by the line and the stat. since a few line stat can apear with the sme code but different lines.
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            DO.LineStation stat = listLineStations.Find(s => s.Code == code && s.LineId == lineId);

            if (stat != null)//found the station
                return stat;
            else//didnt find the station
                throw new DO.StationException(code, $"error in line station that its code is: {code}");
        }

        public void DeleteLineStationsOfALine(int lineId)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            listLineStations.RemoveAll(ls => ls.LineId == lineId);

            XMLTools.SaveListToXMLSerializer(listLineStations, lineStationsPath);

        }

        public void DeleteStationFromLine(int code, int lineId)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            int busNumber = GetLine(lineId).BusNumber;

            DO.LineStation lineStation = listLineStations.Find(ls => (ls.Code == code && ls.LineId == lineId));

            if (lineStation != null)//found the wanted line station
            {
                int indexInLine = lineStation.LineStationIndex;

                if (indexInLine == 0)
                    throw new DO.LineStationException(code, busNumber, $"the station {code} is the first station of the line {busNumber}");

                if (lineStation.Code == GetLine(lineId).LastStation)
                    throw new DO.LineStationException(code, busNumber, $"the station {code} is the last station of the line {busNumber}");


                listLineStations.Remove(lineStation);

                foreach (DO.LineStation ls in listLineStations)
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

            XMLTools.SaveListToXMLSerializer(listLineStations, lineStationsPath);

        }
        #endregion

        #region Line
        public void UpdateLine(DO.Line newLine)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);


            DO.Line ln = listLines.Find(l => l.LineId == newLine.LineId);//search for the the line with the same lineId, if exist.

            if (ln != null)//if found
            {
                //check if the line's fields that were added are legal
                //check the code of the stations:
                if (!listStations.Exists(l => l.Code == newLine.FirstStation))
                    throw new DO.StationException(newLine.FirstStation, $"the station with the code: {newLine.FirstStation} is not found");
                if (!listStations.Exists(l => l.Code == newLine.LastStation))
                    throw new DO.StationException(newLine.LastStation, $"the station with the code: {newLine.LastStation} is not found");

                if (newLine.FirstStation == newLine.LastStation)
                    throw new DO.StationException(newLine.LastStation, $"the last station code: {newLine.LastStation} is illegal since the first and last stations must be different");

                ////check if a bus with the same identifying stations (first and last stations) already exists.
                //if (DataSource.listLines.Exists(l => l.FirstStation == newLine.FirstStation && l.LastStation == newLine.LastStation))
                //    throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} allready exists, with the same first and last stations");

                //add new lineStations of the new stations
                //delete the first linestation and second, and rewrite their details.
                DO.LineStation ls1 = listLineStations.Find(ls => ls.LineStationIndex == 0 && ls.LineId == ln.LineId);//change the original first station
                listLineStations.Remove(ls1);
                ls1.Code = newLine.FirstStation;
                listLineStations.Add(ls1);

                DO.LineStation ls2 = listLineStations.Find(ls => ls.LineStationIndex == 1 && ls.LineId == ln.LineId);//change the original 2nd station
                listLineStations.Remove(ls2);
                ls2.PrevStation = newLine.FirstStation;
                listLineStations.Add(ls2);

                //delete the last linestation and the one before the last, and rewrite their details.
                DO.LineStation lsBeforeLast = listLineStations.Find(ls => ls.NextStation == ln.LastStation && ls.LineId == ln.LineId);//change the original station before last
                listLineStations.Remove(lsBeforeLast);
                lsBeforeLast.NextStation = newLine.LastStation;
                listLineStations.Add(lsBeforeLast);

                DO.LineStation lsLast = listLineStations.Find(ls => ls.NextStation == -1 && ls.LineId == ln.LineId);//change the original last station
                listLineStations.Remove(lsLast);
                lsLast.Code = newLine.LastStation;
                listLineStations.Add(lsLast);

                #region add new adjacent stations
                //listAdjacentStations.Add(new DO.AdjacentStations() { Station1 = newLine.FirstStation, Station2 = ls2.Code, Distance = 0.583, Time = new TimeSpan(00, 01, 16) });
                //listAdjacentStations.Add(new DO.AdjacentStations() { Station1 = lsBeforeLast.Code, Station2 = newLine.LastStation, Distance = 0.702, Time = new TimeSpan(00, 03, 45) });

                XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);//load from xml to Exelement root

                TimeSpan ts1 = new TimeSpan(00, 01, 16);
                TimeSpan ts2 = new TimeSpan(00, 03, 45);

                XElement adjStat1Elem = new XElement("AdjacentStations",
                      new XElement("Station1", newLine.FirstStation.ToString()),
                      new XElement("Station2", ls2.Code.ToString()),
                      new XElement("Distance", "0.583"),
                      new XElement("Time", ts1.ToString()));

                XElement adjStat2Elem = new XElement("AdjacentStations",
                      new XElement("Station1", lsBeforeLast.Code.ToString()),
                      new XElement("Station2", newLine.LastStation.ToString()),
                      new XElement("Distance", "0.702"),
                      new XElement("Time", ts2.ToString()));

                adjacentStationsRootElem.Add(adjStat1Elem);
                adjacentStationsRootElem.Add(adjStat1Elem);

                XMLTools.SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);//save the new root in the xml file
                #endregion

                listLines.Remove(ln);
                listLines.Add(newLine);
            }
            else
                throw new DO.LineException(newLine.BusNumber, $"the station that its code is: {newLine.BusNumber} was not found");
            
            XMLTools.SaveListToXMLSerializer(listLineStations, lineStationsPath);
            XMLTools.SaveListToXMLSerializer(listLines, linesPath);
        }

        public DO.Line GetLine(int lineId, int busNumber)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);

            DO.Line line = listLines.Find(l => l.LineId == lineId);

            if (line != null)//the line was found
                return line;
            else
                throw new DO.LineException(busNumber, $"error in line: {busNumber}");
        }

        public DO.Line GetLine(int lineId)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);

            return listLines.Find(l => l.LineId == lineId);
        }

        public IEnumerable<DO.Line> GetAllLines()
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);

            return from line in listLines
                   select line;
        }

        public void DeleteLine(int lineId, int busNumber)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);

            DO.Line lineToDel = listLines.Find(ln => ln.LineId == lineId);

            if (lineToDel != null)//line was found
            {
                //delete all line stations of the line
                DeleteLineStationsOfALine(lineId);
                //then delete the line itself
                listLines.Remove(lineToDel);
            }
            else
                throw new DO.LineException(busNumber, $"the line: {busNumber} wasnt found");
           
            XMLTools.SaveListToXMLSerializer(listLines, linesPath);
        }

        private static Random r = new Random();

        public void AddLineToList(DO.Line newLine)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);

            #region running number:
            //DO.Config.LineId++;//update running number

            //running number- since "Config" in DO is a static class,
            //in order to save the running number from haraza to haraza, we need to save it each time in a file.
            //we will use xElement for that:

            XElement runningNumberRootElem = XMLTools.LoadListFromXMLElement(runningNumberPath);//load from xml to Exelement root

            XElement getRunNumElem = (from run in runningNumberRootElem.Elements()
                             select run).FirstOrDefault();

            int newRunNum = int.Parse(getRunNumElem.Element("LineId").Value) + 1;//add 1 to the line id that was before.

            getRunNumElem.Element("LineId").Value = newRunNum.ToString();

            XMLTools.SaveListToXMLElement(runningNumberRootElem, runningNumberPath);//save the new root in the xml file

            newLine.LineId = newRunNum;//running number
            #endregion

            //check if the first and last stations are the same- if so, cannot add the bus.
            if (newLine.FirstStation == newLine.LastStation)
                throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} is not legal since the first and last stations must be different");

            //check if a bus with the same identifying stations (first and last stations) already exists.
            if (listLines.Exists(l => l.FirstStation == newLine.FirstStation && l.LastStation == newLine.LastStation && l.BusNumber == newLine.BusNumber))
                throw new DO.LineException(newLine.BusNumber, $"the line: {newLine.BusNumber} allready exists, with the same first and last stations");

            //add new lineStations of the new stations
            listLineStations.Add(new DO.LineStation() { Code = newLine.FirstStation, LineId = newLine.LineId, LineStationIndex = 0, NextStation = newLine.LastStation, PrevStation = -1 });
            listLineStations.Add(new DO.LineStation() { Code = newLine.LastStation, LineId = newLine.LineId, LineStationIndex = 1, NextStation = -1, PrevStation = newLine.FirstStation });

            #region add new adjacent stations

            DO.AdjacentStations newAdj = new DO.AdjacentStations();

            newAdj.Station1 = newLine.FirstStation;
            newAdj.Station2 = newLine.LastStation;

            double long1 = listStations.Find(st => st.Code == newAdj.Station1).Longitude;
            double long2 = listStations.Find(st => st.Code == newAdj.Station2).Longitude;
            double lat1 = listStations.Find(st => st.Code == newAdj.Station1).Lattitude;
            double lat2 = listStations.Find(st => st.Code == newAdj.Station2).Lattitude;

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
            
            //listAdjacentStations.Add(newAdj);

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);//load from xml to Exelement root
            XElement adjStatElem = new XElement("AdjacentStations",
                   new XElement("Station1", newAdj.Station1.ToString()),
                   new XElement("Station2", newAdj.Station2.ToString()),
                   new XElement("Distance", newAdj.Distance.ToString()),
                   new XElement("Time", newAdj.Time.ToString()));

            adjacentStationsRootElem.Add(adjStatElem);

            XMLTools.SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);//save the new root in the xml file

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

                //listLineTrips.Add(lnTrip);

                XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);//load from xml to Exelement root
                XElement lnTripElem = new XElement("LineTrip",
                       new XElement("LineTripID", lnTrip.LineTripID.ToString()),
                       new XElement("LineID", lnTrip.LineID.ToString()),
                       new XElement("StartAt", lnTrip.StartAt.ToString()));

                lineTripRootElem.Add(lnTripElem);

                XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath);//save the new root in the xml file

            }

            listLines.Add(newLine);

            XMLTools.SaveListToXMLSerializer(listLineStations, lineStationsPath);
            XMLTools.SaveListToXMLSerializer(listLines, linesPath);
        }
        #endregion

        #region User
        public DO.AppUser GetUser(string myname, string mypassword)
        {
            List<AppUser> users = XMLTools.LoadListFromXMLSerializer<AppUser>(usersPath);

            DO.AppUser user = users.FirstOrDefault(u => u.UserName == myname && u.Password == mypassword);

            if (user != null)//if user was found
            {
                return user;
            }
            else
            {
                throw new DO.AppUserException(myname, $"the user: {myname} does'nt exist in the curret state");
            }

        }

        public IEnumerable<DO.AppUser> GetAllUsers()
        {
            List<AppUser> users = XMLTools.LoadListFromXMLSerializer<AppUser>(usersPath);

            return from user in users
                   select user;
        }

        public void AddUser(DO.AppUser user)
        {
            List<AppUser> users = XMLTools.LoadListFromXMLSerializer<AppUser>(usersPath);

            if (users.Where(s => s.UserName == user.UserName).ToList().Count() > 0)
            {
                throw new DO.AppUserException(user.UserName, $"the user: {user.UserName} allready exists. Choose another name");
            }
            users.Add(user);

            XMLTools.SaveListToXMLSerializer(users, usersPath);
        }

        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAdjacentStationsByFirstOfPair(int code)
        {
            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            //return from adjSt in DataSource.listAdjacentStations
            //       where adjSt.Station1 == code
            //       //where adjSt.Station2 == code
            //       select adjSt.Clone();//return a list of adjacent stations, in which the station with the given code apears as the 1st in the pair


            return from p in adjacentStationsRootElem.Elements()
                   let adjStat = new AdjacentStations()
                   {
                       Station1 = Int32.Parse(p.Element("Station1").Value),
                       Station2 = Int32.Parse(p.Element("Station2").Value),
                       Distance = double.Parse(p.Element("Distance").Value),
                       Time = TimeSpan.ParseExact(p.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where(adjStat.Station1==code)
                   select adjStat;

        }
        public IEnumerable<DO.AdjacentStations> GetAdjacentStationsBySecondOfPair(int code)
        {
            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            //return from adjSt in DataSource.listAdjacentStations
            //       where adjSt.Station2 == code
            //       select adjSt.Clone();//return a list of adjacent stations, in which the station with the given code apears as the 2nd in the pair

            return from p in adjacentStationsRootElem.Elements()
                   let adjStat = new AdjacentStations()
                   {
                       Station1 = Int32.Parse(p.Element("Station1").Value),
                       Station2 = Int32.Parse(p.Element("Station2").Value),
                       Distance = double.Parse(p.Element("Distance").Value),
                       Time = TimeSpan.ParseExact(p.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where (adjStat.Station2 == code)
                   select adjStat;
        }

        #endregion

        #region LineTrip
        public IEnumerable<DO.LineTrip> GetAllLineTripPerLine(int lineid)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            //return from lnTrip in DataSource.listLineTrips//return all line trips of a specific line.
            //       where lnTrip.LineID == lineid
            //       select lnTrip.Clone();

            return from p in lineTripRootElem.Elements()
                   let lnTrip = new LineTrip()
                   {
                       LineTripID = Int32.Parse(p.Element("LineTripID").Value),
                       LineID = Int32.Parse(p.Element("LineID").Value),
                       StartAt = TimeSpan.ParseExact(p.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where (lnTrip.LineID == lineid)
                   select lnTrip;

        }

        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            //return from lTrip in DataSource.listLineTrips
            //       where predicate(lTrip)
            //       select lTrip.Clone();

            return from p in lineTripRootElem.Elements()
                   let lnTrip = new LineTrip()
                   {
                       LineTripID = Int32.Parse(p.Element("LineTripID").Value),
                       LineID = Int32.Parse(p.Element("LineID").Value),
                       StartAt = TimeSpan.ParseExact(p.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where (predicate(lnTrip))
                   select lnTrip;
        }
        #endregion
    }
}
