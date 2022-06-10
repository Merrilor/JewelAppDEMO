using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF2022User02Lib
{
    public static class Calculations
    {

        public static string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {

            if (startTimes.Length != durations.Length)
            {
                return new string[] { "Массивы не совпадают по размерам" };
            }



            if (!startTimes.SequenceEqual(startTimes.OrderBy(t => t)))
            {
                return new string[] { "Массив начал не отсортирован" };
            }

            var spansArray = new DateTime[startTimes.Length, 2];


            for (int i = 0; i < startTimes.Length; i++)
            {

                DateTime startDateTime = new DateTime();
                spansArray[i, 0] = startDateTime.Add(startTimes[i]);
                spansArray[i, 1] = startDateTime.Add(startTimes[i]).AddMinutes(durations[i]);

            }

            if (endWorkingTime < beginWorkingTime)
            {
                return new string[] { "Время начала позже времени окончания" };
            }

            if (durations.Any(d => d == (endWorkingTime.Subtract(beginWorkingTime).TotalMinutes)))
            {
                return new string[] { "Длительность равна рабочему дню" };

            }

            if (durations.Any(d => d > (endWorkingTime.Subtract(beginWorkingTime).TotalMinutes)))
            {
                return new string[] { "Длительность больше рабочего дня" };

            }


            var resultArray = new List<string>();

            DateTime beginWorkDatetime = new DateTime();
            beginWorkDatetime = beginWorkDatetime.Add(beginWorkingTime);

            DateTime endWorkDatetime = new DateTime();
            endWorkDatetime = endWorkDatetime.Add(endWorkingTime);

            for (int i = 0; i < (endWorkingTime.Subtract(beginWorkingTime)).TotalMinutes / consultationTime; i++)
            {

                bool isIntersecting = false;

                for (int j = 0; j < spansArray.GetLength(0) - 1; j++)
                {

                    if (spansArray[j, 0] >= beginWorkDatetime.AddMinutes(i * consultationTime).AddMinutes(consultationTime) || beginWorkDatetime.AddMinutes(i * consultationTime) >= spansArray[j, 1])
                    {
                        isIntersecting = false;
                    }
                    else
                    {
                        isIntersecting = true;
                        continue;
                    }


                }

                if (isIntersecting == false)
                {
                    resultArray.Add($"{beginWorkDatetime.AddMinutes(i * consultationTime).ToString("HH:mm")}-{beginWorkDatetime.AddMinutes(i * consultationTime).AddMinutes(consultationTime).ToString("HH:mm")}");
                }


            }

            return resultArray.ToArray();


        }



    }
}
