﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dotNet5781_03B_7224_0847
{
    public enum Status
    {
     TRY_ME  ,DRIVING  ,FUELING  ,IN_CARE  //STATUS OF BUS
    }
  public  class Bus
    {
        private string licenseNumber;
        public string LicenseNumber //license number
        {
            get { return licenseNumber; }
            set 
            {
                int tmp;
                bool flag = int.TryParse(value, out tmp);
                if (flag)//if its integers
                {
                    if (tmp <= 9999999)//7 digits (we dont allow typing more than 7 digits if the year<2018)
                    {
                        if (Start_d.Year < 2018)
                        {
                            if (tmp < 1000000)//less than 7 digits
                            {
                                string str;
                                str = "0000000";
                                str=str.Insert(7 - (tmp.ToString().Length), tmp.ToString());
                                str = str.Remove(7, tmp.ToString().Length);
                                licenseNumber = str.Substring(0, 2) + "-" + str.Substring(2, 3) + "-" + str.Substring(5, 2);
                            }
                            else licenseNumber = value.Substring(0, 2) + "-" + value.Substring(2, 3) + "-" + value.Substring(5, 2);
                        }
                        //else { throw new BusException("license number does not match the year!"); }

                    }
                    if (tmp >= 10000000 && tmp <= 99999999)//8 digits and if year>=2018
                    {
                        if (Start_d.Year >= 2018)
                            licenseNumber = value.Substring(0, 3) + "-" + value.Substring(3, 2) + "-" + value.Substring(5, 3);
                        //else { throw new BusException("license number does not match the year!"); }
                    }
                }
                //else//value is allready a string (with "-")
                //    licenseNumber = value;

            }
        }         
        public DateTime Start_d { get; set; }           //starting activity day
        public DateTime last_care_d { get; set; }
        public long Km { get; set; }                    //kilometrage of one bus
        public int Km_since_care { get; set; }
        public int Km_since_fuel { get; set; }
        public Status status { get; set; }

        //public int FuelProgressTime                     //shows in ints the time passes in fuel progress bar
        //{
        //    get
        //    {
        //        return (int)GetValue(FuelProgressTimeProperty);
        //    }
        //    set
        //    {
        //        SetValue(FuelProgressTimeProperty, value); 
        //    }
        //}
        //public static readonly System.Windows.DependencyProperty FuelProgressTimeProperty =
        // System.Windows.DependencyProperty.Register("FuelProgressTimeProperty",
        //  typeof(int), typeof(Bus), new System.Windows.UIPropertyMetadata(0));

        //public int Precentage { get; set; }


        //public int Precentage
        //{
        //    get { return (int)GetValue(PrecentageProperty); }
        //    set { SetValue(PrecentageProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Precentage.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PrecentageProperty =
        //    DependencyProperty.Register("Precentage", typeof(int), typeof(Bus), new PropertyMetadata(0));



    }
}
