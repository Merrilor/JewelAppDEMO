using Microsoft.VisualStudio.TestTools.UnitTesting;
using SF2022User02Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF2022User02Lib.Tests
{
    [TestClass()]
    public class CalculationsTests
    {
        [TestMethod()]
        public void Check_DifferenLengthTimeArrays_ReturnsErrorArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = new int[] { 60 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "Массивы не совпадают по размерам" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void Check_EndTimeShorterThanStartTime_ReturnsErrorArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = new int[] { 60, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(18, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(8, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "Время начала позже времени окончания" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void Check_StartTimesNotSorted_ReturnsErrorArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(17, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = new int[] { 60, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "Массив начал не отсортирован" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Check_ConsultationTime300_ReturnsArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = new int[] { 60, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 300;

            string[] expected = new string[] { "13:00-18:00" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Check_ConsultationTime3000_ReturnsEmptyArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = new int[] { 60, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 3000;

            string[] expected = new string[] { };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Check_Duration600_ReturnArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0) };
            int[] durations = new int[] { 600 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "Длительность равна рабочему дню" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Check_Duration900StartAt8_ReturnErrorArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(8, 0, 0) };
            int[] durations = new int[] { 900 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "Длительность больше рабочего дня" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Check_EndWorkingTime10_ReturnArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(8, 0, 0) };
            int[] durations = new int[] { 60 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(10, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "08:00-08:30", "08:30-09:00", "09:00-09:30", "09:30-10:00" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Check_BeginWorkingTime10End12_ReturnErrorArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(8, 0, 0) };
            int[] durations = new int[] { 60 };
            TimeSpan beginWorkingTime = new TimeSpan(10, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(12, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Check_BeginWorkingTime16End19_ReturnErrorArray()
        {

            //Arrange
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(17, 0, 0), new TimeSpan(18, 0, 0) };
            int[] durations = new int[] { 60, 10 };
            TimeSpan beginWorkingTime = new TimeSpan(16, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(19, 0, 0);
            int consultationTime = 30;

            string[] expected = new string[] { "16:00-16:30", "16:30-17:00", "18:00-18:30", "18:30-19:00" };


            //Act
            string[] actual = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            //Assert
            CollectionAssert.AreEqual(expected, actual);

        }
    }
}