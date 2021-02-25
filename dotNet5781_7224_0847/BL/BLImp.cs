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

        public void restartXmlLists()
        {
            dl.restartXmlLists();
        }

        #region Station
        public BO.Station GetStation(int code)
        {
            DO.Station stationDO;
            try
            {
                stationDO = dl.GetStation(code);
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("station does not exist\n", ex);
            }
            return stationDoBoAdapter(stationDO);//adopt each do to bo
        }

        public void UpdateStationDetails(BO.Station currStat)
        {
            //Update DO.Station            
            DO.Station stationDO;
            //currStat.CopyPropertiesTo(stationDO);
            try
            {
                stationDO = stationBoDoAdapter(currStat);
                dl.UpdateStation(stationDO);
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("cannot update the station, illegal value/s were inserted\n", ex);
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
                throw new BO.StationException("error, cannot delete station\n", ex);
            }
        }

        public IEnumerable<BO.Station> GetAllStations()//move through all stationsDO, make them stationsBO and return the list of stationBO
        {
            return from stationDO in dl.GetAllStations()//ask dl to provide all the DO.stations, make them BO.stations and return the list.
                   orderby stationDO.Name           //order it by their code
                   select stationDoBoAdapter(stationDO);//adopt each do to bo
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
                throw new BO.StationException("Station code is illegal\n", ex);
            }
            newStationDO.CopyPropertiesTo(stationBO);//copies from stationDO to stationBO- only flat properties.

            ////now we still need to fill the "lines" list:

            stationBO.lines = from lineStation in dl.GetLineStationsListThatMatchAStation(code)
                              let line = dl.GetLine(lineStation.LineId)
                              select line.CopyDOLineStationToBOLine(lineStation);

            ////now we need to restart the "lineStations" list of each line.

            return stationBO;
        }

        DO.Station stationBoDoAdapter(BO.Station stationBO)
        {
            DO.Station stationDO = new DO.Station();
            //check code of the station:
            if (stationBO.Code < 1 || stationBO.Code > 999999)
                throw new DO.StationException(stationBO.Code, "illegal station code");
            //check longitude and lattitude:
            if (stationBO.Lattitude < 31 || stationBO.Lattitude > 33.3 || stationBO.Longitude < 34.3 || stationBO.Longitude > 35.5)
                throw new DO.StationException(stationBO.Code, "station is not is Israel's teritory. illegal longitude or lattitude " +
                    "insert latiatude from 31 to 33.3, and longitude from 34.3 to 35.5");

            stationBO.CopyPropertiesTo(stationDO);
            return stationDO;
        }

        BO.LineStation lineStationDoBoAdapter(DO.LineStation lineStationDO)
        {
            BO.LineStation lineStationBO = new BO.LineStation();
            DO.LineStation newlineStationDO;//before copying lineStationDO to lineStationBO, we need to ensure that lineStationDO is legal- legal code.
            //sometimes we get here after the user filled lineStationDO fields. thats why we copy the given lineStationDO to a new lineStationDO and check if it is legal.
            int code = lineStationDO.Code;
            int lineId = lineStationDO.LineId;
            try
            {
                newlineStationDO = dl.GetLineStation(code, lineId);//if code is legal, returns a new lineStationDO. if not- ecxeption.
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("Station code is illegal\n", ex);
            }

            //copy "Code" and "LinestationIndex":
            //newlineStationDO.CopyPropertiesTo(lineStationBO);//copies- only flat properties.
            lineStationBO.Code = lineStationDO.Code;
            lineStationBO.LineStationIndex = lineStationDO.LineStationIndex;

            //copy "Name":
            lineStationBO.Name = dl.GetStation(code).Name;

            //copy "Distance" and "Time":
            if (lineStationBO.LineStationIndex == 0)//if its the 1st station in the line, the distance and time from the former station =0.
            {
                //lineStationBO.Time = 00:00:00 - default
                //lineStationBO.Distance = 0 - default
            }
            else
            {
                //distance from the former station: we look for the stations pair in which our current stat is the second in the pair. 
                //it will let us find the distance and time from the former station to her.
                lineStationBO.Distance = dl.GetAdjacentStationsBySecondOfPair(code).FirstOrDefault(adj => adj.Station2 == code).Distance;
                //time from the former station:
                lineStationBO.Time = dl.GetAdjacentStationsBySecondOfPair(code).FirstOrDefault(adj => adj.Station2 == code).Time;
            }

            return lineStationBO;
        }

        public void AddStationToList(BO.Station newStat)
        {
            try
            {
                //the adapter will check if its logically possible to add the station
                DO.Station statToAdd = stationBoDoAdapter(newStat);
                dl.AddStationToList(statToAdd);
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("error, cannot add the station\n", ex);
            }
        }
        #endregion

        #region Line
        public void UpdateLineDetails(BO.Line currLine)
        {
            //Update DO.Line            
            DO.Line lineDO = new DO.Line();
            currLine.CopyPropertiesTo(lineDO);
            try
            {
                dl.UpdateLine(lineDO);

            }
            catch (DO.LineException ex)
            {
                throw new BO.LineException("Line Number is illegal\n", ex);
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("stations are illegal\n", ex);
            }
        }

        public IEnumerable<BO.Line> GetAllLinesPerStation(int code)
        {
            return from lineStation in dl.GetLineStationsListThatMatchAStation(code)
                   let line = dl.GetLine(lineStation.LineId)
                   select line.CopyDOLineStationToBOLine(lineStation);
        }

        public IEnumerable<BO.Line> GetAllLinesByArea(BO.Areas area)
        {
            return from LineDO in dl.GetAllLines()
                   where LineDO.Area.CompareTo((DO.Areas)area) == 0//if the erea is equal to the given area
                   orderby LineDO.BusNumber           //order it by their bus number
                   select lineDoBoAdapter(LineDO);
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
            int busNumber = lineDO.BusNumber;
            try
            {
                newlineDO = dl.GetLine(lineId, busNumber);//if code is legal, returns a new lineStationDO. if not- ecxeption.
            }
            catch (DO.LineException ex)
            {
                throw new BO.LineException("Line bus number is illegal\n", ex);
            }

            newlineDO.CopyPropertiesTo(lineBO);//copies- only flat properties.

            //now we need to restart the "lineStations" list of each line.

            lineBO.lineStations = from lineStationDO in dl.GetLineStationsListOfALine(lineId)
                                  let lineStationBO = lineStationDoBoAdapter(lineStationDO)
                                  select lineStationBO;

            return lineBO;
        }

        DO.Line lineBoDoAdapter(BO.Line lineBO)
        {
            DO.Line lineDO = new DO.Line();

            //copy all relevant properties
            lineBO.CopyPropertiesTo(lineDO);
            //lineBO.LineId = DO.Config.LineId++;

            return lineDO;
        }

        public void DeleteLine(int lineId, int busNumber)
        {
            try
            {
                dl.DeleteLine(lineId, busNumber);
            }
            catch (DO.LineException ex)
            {
                throw new BO.LineException("error, cannot delete line\n", ex);
            }
        }

        public void AddLineToList(BO.Line newLine)
        {
            try
            {
                //the adapter/ the adding func will check if its logically possible to add the line
                DO.Line lineToAdd = lineBoDoAdapter(newLine);
                dl.AddLineToList(lineToAdd);
            }
            catch (DO.StationException ex)
            {
                throw new BO.StationException("error, cannot add the line\n", ex);
            }
            catch (DO.LineException ex)
            {
                throw new BO.LineException("error, cannot add the line\n", ex);
            }
        }
        #endregion

        #region LineStation
        public IEnumerable<BO.LineStation> GetAllLineStationsPerLine(int lineId)
        {
            return from DOlineStation in dl.GetLineStationsListOfALine(lineId)
                   let BOlineStation = lineStationDoBoAdapter(DOlineStation)
                   select BOlineStation;
        }

        public void DeleteStationFromLine(int code, int lineId)
        {
            try
            {
                dl.DeleteStationFromLine(code, lineId);

            }
            catch (DO.LineStationException ex)
            {
                throw new BO.LineStationException("cannot delete the station from the line\n", ex);
            }
        }
        #endregion

        #region User
        /// <summary>
        /// Conversion between do and bo
        /// </summary>
        /// <param name="userDO">user do</param>
        /// <returns>user bo</returns>
        public BO.AppUser userDoBoAdapter(DO.AppUser userDO)
        {
            BO.AppUser userBO = new BO.AppUser();
            DO.AppUser newUserDO;
            string name = userDO.UserName;
            string password = userDO.Password;
            try
            {
                newUserDO = dl.GetUser(name,password);
            }
            catch (DO.AppUserException ex)
            {
                throw new BO.AppUserException("ERROR!\n", ex);
            }
            newUserDO.CopyPropertiesTo(userBO);

            userDO.CopyPropertiesTo(userBO);

            return userBO;
        }
        /// <summary>
        /// returns a user
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>user bo</returns>
        public BO.AppUser GetUser(string name,string password)
        {
            DO.AppUser userDO;
            try
            {
                userDO = dl.GetUser(name,password);
            }
            catch (DO.AppUserException ex)
            {
                throw new BO.AppUserException("The user with this password wasn't found\n", ex);
            }
            return userDoBoAdapter(userDO);
        }
        /// <summary>
        /// add a user
        /// </summary>
        /// <param name="user">user</param>
        public void AddUser(BO.AppUser user)
        {
            try
            {
                dl.AddUser(userBoDoAdapter(user));
            }
            catch (DO.AppUserException ex)
            {
                throw new BO.AppUserException("couldn't add the user\n", ex);
            }
        }

        private DO.AppUser userBoDoAdapter(BO.AppUser userBO)
        {
            DO.AppUser userDO= new DO.AppUser();
            userBO.CopyPropertiesTo(userDO);
            return userDO;
        }

        /// <summary>
        /// returns all the users
        /// </summary>
        /// <returns>users</returns>
        public IEnumerable<BO.AppUser> GetAllUsers()
        {
            return from item in dl.GetAllUsers()
                   select userDoBoAdapter(item);
        }
        #endregion

        #region LineTrip
        public IEnumerable<BO.LineTrip> GetAllLineTripPerLine(int lineid)
        {
            return from lnTripDO in dl.GetAllLineTripPerLine(lineid)//get all line trips of a specific line
                   let lnTripBO = lineTripDoBoAdapter(lnTripDO)//adopt each do to bo
                   orderby lnTripBO.StartAt
                   select lnTripBO;//return bo
        }

        BO.LineTrip lineTripDoBoAdapter(DO.LineTrip lnTripDO)
        {
            BO.LineTrip lnTripBO = new BO.LineTrip();

            //copy all relevant properties
            lnTripDO.CopyPropertiesTo(lnTripBO);

            return lnTripBO;
        }
        #endregion

        #region LineAndTime
        public IEnumerable<BO.LineAndTime> GetLineAndTimePerStation(BO.Station stationBO, TimeSpan currentTime)
        {
            //list of lines that pass in the station
            List<BO.Line> listLines = (from l in GetAllLines()
                                       where (GetAllLineStationsPerLine(l.LineId).ToList().Find(s => s.Code == stationBO.Code) != null)
                                       select l).ToList();

            List<BO.LineAndTime> times = new List<BO.LineAndTime>();//list of the lines that the function will return
            TimeSpan hour = new TimeSpan(1, 0, 0);//help to find the times that in the range of one hour from currentTime                           
            for (int i = 0; i < listLines.Count(); i++)//for all the lines that pass in the station
            {
                //calculate the times 
                TimeSpan tmp;//the current time
                int currentLineid = listLines[i].LineId;// line id of the current line
                
                List<DO.LineTrip> lineSchedual = dl.GetAllLineTripsBy(trip => trip.LineID == currentLineid).ToList();// times of the current Line
                TimeSpan timeTilStatin = travelTime(stationBO.Code, currentLineid);
                int numOfTimes = 0;
                List<int> timesOfCurrentLine = new List<int>();
                
                for (int j = 0; j < lineSchedual.Count && numOfTimes < 3; j++)//for all the times in line sSchedual
                {
                    //check if currentTime-LeavingTime-travelTime more than zero and in the range of hour
                    if (lineSchedual[j].StartAt + timeTilStatin <= currentTime + hour
                        && lineSchedual[j].StartAt + timeTilStatin >= currentTime)
                    //check if the bus already passed the statioin   
                    {
                        if (currentTime - lineSchedual[j].StartAt >= TimeSpan.Zero)//if the line already get out from the station
                        {
                            tmp = timeTilStatin - (currentTime - lineSchedual[j].StartAt);
                        }
                        else//if the line didnt get out from the station
                            tmp = timeTilStatin + (lineSchedual[j].StartAt - currentTime);

                        timesOfCurrentLine.Add(tmp.Minutes);
                        //timesString = timesString + tmp.Minutes+ ", ";                                                                                                  
                        numOfTimes++;
                    }
                }

                if (timesOfCurrentLine.Count != 0)
                {
                    string timesString = "";//the string of times
                    timesOfCurrentLine = timesOfCurrentLine.OrderBy(s => s).ToList();//order the times in ascending order
                    for (int k = 0; k < timesOfCurrentLine.Count - 1; k++)
                    {
                        timesString = timesString + timesOfCurrentLine[k] + ", ";
                    }
                    timesString = timesString + timesOfCurrentLine[timesOfCurrentLine.Count - 1];//add the last one without ","
                    times.Add(new BO.LineAndTime
                    {
                        LineId = currentLineid,
                        LineNum = listLines[i].BusNumber,
                        DestinationStation = (GetAllLineStationsPerLine(listLines[i].LineId).ToList())[GetAllLineStationsPerLine(listLines[i].LineId).Count() - 1].Name,
                        Stringtimes = timesString,
                    });

                }
                numOfTimes = 0;
                //timesString = "";
            }
            times = times.OrderBy(lt => lt.LineNum).ToList();//order the list by the number of the lines in ascending order
            return times;

        }

        private TimeSpan travelTime(int stationCode, int lineID)
        {//func that return the time from first station in line to specific station
            TimeSpan sumTime = TimeSpan.Zero;
            BO.Line line = GetAllLines().FirstOrDefault(l=>l.LineId == lineID);
            foreach (var s in GetAllLineStationsPerLine(line.LineId).ToList())
            {
                if (s.Code == stationCode)
                    sumTime += s.Time;//Time from next station
                else
                {
                    //sumTime += s.TimeFromNext;
                    break;
                }
            }
            return sumTime;
        }
        #endregion
    }
}