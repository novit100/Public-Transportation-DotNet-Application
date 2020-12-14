using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Person_ComboBoxAndListBox
{
    public enum Status { SINGLE, MARRIED, DEVORSED, WIDOWER }
    public class Person
    {
        public string Name { get; set; }
        public bool IsStudent { get; set; }
        public int ID { get; set; }
        public Status PersonalStatus { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }


    public static class ToolsClass
    {
        // static generic function wich show all the properties for each class
        public static string ToStringProperty<T>(this T t)
        {
            string str = "";
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {

                str += item.Name + ": " + item.GetValue(t, null) + " ";
            }
            return str;
        }
    }
}
