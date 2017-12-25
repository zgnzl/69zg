﻿using System;

namespace _69zg.Common
{
    public class GuidManager
    {
        #region 带时间的guid，有顺序，可索引  可获取时间

        public static Guid GenerateComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build      
            //the byte string      
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array          
            // Note that SQL Server is accurate to 1/300th of a      
            // millisecond so we divide by 3.333333      
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)
              (msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering      
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid      
            Array.Copy(daysArray, daysArray.Length - 2, guidArray,
              guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray,
              guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }



        public static DateTime GetDateFromComb(System.Guid guid)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = guid.ToByteArray();  // Copy the date parts of the guid to the respective byte arrays. <br>&nbsp;&nbsp;&nbsp;&nbsp; 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);
            // Reverse the arrays to put them into the appropriate order <br>&nbsp;&nbsp;&nbsp;&nbsp; 
            Array.Reverse(daysArray);

            Array.Reverse(msecsArray);
            // Convert the bytes to ints <br>&nbsp;&nbsp;&nbsp;&nbsp; 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);
            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);
            return date;
        }

        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long GuidToLongID(Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static string GuidTo16String(Guid guid)
        {
            long i = 1;
            foreach (byte b in guid.ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        #endregion

    }
}
