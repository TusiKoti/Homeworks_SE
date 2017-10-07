using System;
using System.Collections;
using System.Collections.Generic;

namespace CarRent
{


    public class CarRecord
    {
        public CarRecord(string model, string colour, string vin)
        {
            Model = model;
            Colour = colour;
            Vin = vin;
            endStartRentDates = new SortedList<DateTime, DateTime>();
        }

        /// <summary>
        /// Метод для аренды машины в заданном интервале времени
        /// </summary>
        /// <param name="startOfRent">дата начала аренды</param>
        /// <param name="endOfRent">дата конца аренды</param>
        /// <returns>true, если бронь прошла, иначе - false</returns>
        public bool Rent(DateTime startOfRent, DateTime endOfRent)
        {
            int i = -1;
            // проверяем, попадает ли запрошенный период на уже распланированные аренды и ТО:
            foreach (var endStartPair in endStartRentDates)
            {
                ++i;
                var s = endStartPair.Value;
                var e = endStartPair.Key;
                // Если попали на аренду по номеру кратную 10, то надо добавить ТО после
                // сразу после этой аренды
                if ((i + 1) % 10 == 0)
                {
                    e = e.AddDays(checkUpDaysNumber);
                }
                if (ArePeriodsIntersected(s, e, startOfRent, endOfRent))
                {
                    return false;
                }
            }

            // находим индекс куда вставится элемент
            //var endRentDates = new List<DateTime>(endStartRentDates.Keys);
            //i = endRentDates.BinarySearch(startOfRent);
            // Если вдруг дата найдена, то что-то пошло не так
            //if (i>0)
            //{
            //    return false;
            //}
            //i = ~i;
            //// Если индекс больше длины, то расписание не изменится
            //if (i > endStartRentDates.Count - 1)
            //    return true;
            // TODO: проверяем, что изменение расписания не приведёт к нарушению расписания после
            // него:
            // ...

            // Если всё хорошо, то добавляем в расписание
            endStartRentDates.Add(endOfRent, startOfRent);
            return true;
        
        }

        static private bool ArePeriodsIntersected(DateTime s1, DateTime e1, DateTime s2, DateTime e2)
        {
            return (IsInDateTimePeriod(s1, s2, e2) || IsInDateTimePeriod(e1, s2, e2) || IsInDateTimePeriod(s2, s1, e1));
        }

        static private bool IsInDateTimePeriod(DateTime dt, DateTime periodStart, DateTime periodEnd)
        {
            return ((dt <= periodEnd) && (periodStart <= dt));
        }


        /// <summary>
        /// Метод для проверки на то свободна ли машина для аренды в заданном интервале времени.
        /// </summary>
        /// <param name="startOfRent">дата начала аренды</param>
        /// <param name="endOfRent">дата конца аренды</param>
        /// <returns>true, если машина свободна, иначе - false</returns>
        public bool IsFreeToRent(DateTime startOfRent, DateTime endOfRent)
        {
            TimeSpan rentTerm = endOfRent.Subtract(startOfRent).Add(new TimeSpan(1, 0, 0, 0));
            // нельзя арендовать машину более чем на 60 дней
            if ((rentTerm.Days > 60) || (rentTerm.Days<1))
                return false;
            return true;
        }

        public string Model { get; }
        public string Colour { get; }
        public string Vin { get; }

        public DateTime[] getStartRentDates()
        {
            DateTime[] result = new DateTime[endStartRentDates.Count];
            endStartRentDates.Values.CopyTo(result, 0);
            return result;
        }

        private SortedList<DateTime, DateTime> endStartRentDates;
        public DateTime[] getEndRentDates()
        {
            DateTime[] result = new DateTime[endStartRentDates.Count];
            endStartRentDates.Keys.CopyTo(result, 0);
            return result;
        }
        const int checkUpDaysNumber = 7;
        
    }
}
