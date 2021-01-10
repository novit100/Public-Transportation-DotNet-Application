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

        //public void DeleteStation(int code)
        //{
        //    try
        //    {
        //        dl.DeleteStation(code);
        //        dl.DeleteStudentFromAllCourses(code);
        //    }
        //    catch (DO.StationException ex)
        //    {
        //        throw new BO.StationException("error, cannot delete station", ex);
        //    }
        //}

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
            //sometimes we get here after the user filled ststionDO fields. thats why we copy the given stationDO to a new stationDO and check if it is legal.
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
                line.lineStations = dl.GetLineStationsListOfALine(line.LineId);
                foreach (DO.LineStation lineStationDO in dl.GetLineStationsListOfALine(line.LineId))
                {
                    BO.LineStation lineStationDoBoAdapter(lineStationDO)
                }
            }

            //BO.Line currLine = new BO.Line();



            //stationBO.lines = from lineStation in dl.GetLineStationsListThatMatchAStation(code)
            //                  let line = dl.GetLineStationsListOfALine(int lineId)
            //                  select line.CopyDOLineStationToBOLine(lineStation);

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

            

            return lineStationBO;
        }

        BO.Line lineDoBoAdapter(DO.Line lineDO)
        {

        }

        //public IEnumerable<BO.Line> GetAllLines()
        //{
        //    return from stationDO in dl.GetAllStations()
        //           orderby stationDO.Code           //order it by their code
        //           select stationDoBoAdapter(stationDO);

        //}

        public BO.Line lineDoBoAdapter(this DO.Course course, DO.StudentInCourse sic)
        {
            BO.StudentCourse result = (BO.StudentCourse)course.CopyPropertiesToNew(typeof(BO.StudentCourse));
            // propertys' names changed? copy them here...
            result.Grade = sic.Grade;
            return result;
        }

        public IEnumerable<BO.StudentCourse> GetAllCoursesPerStudent(int id)
        {
            return from sic in dl.GetStudentsInCourseList(sic => sic.PersonId == id)
                   let course = dl.GetCourse(sic.CourseId)
                   select course.CopyToStudentCourse(sic);
        }
    }
}