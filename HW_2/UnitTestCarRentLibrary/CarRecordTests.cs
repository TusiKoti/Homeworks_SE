using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent.Tests
{
    [TestClass()]
    public class CarRecordTests
    {
        /// <summary>
        /// Проверка того, что вновь созданная запись о машине позволяет арендовать её
        /// на допустимый (по условиям организации) срок
        /// </summary>
        [TestMethod()]
        public void NewCarIsFreeToRentForNormalTermTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            DateTime startOfRent = new DateTime(2017, 1, 1);
            DateTime endOfRent = new DateTime(2017, 1, 2);
            bool isCarFreeToRent = carRecord.IsFreeToRent(startOfRent, endOfRent);
            Assert.AreEqual(isCarFreeToRent, true);
        }

        /// <summary>
        /// Проверка того, что даже вновь созданная запись о машине не позволяет арендовать её
        /// на срок больший чем на 60 дней (2 месяца)
        /// </summary>
        [TestMethod()]
        public void NewCarCantBeRentedForMoreThan60daysTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            DateTime startOfRent = new DateTime(2017, 1, 1);
            DateTime endOfRent = startOfRent.AddDays(61);
            bool isCarFreeToRent = carRecord.IsFreeToRent(startOfRent, endOfRent);
            Assert.AreEqual(isCarFreeToRent, false);
        }

        /// <summary>
        /// Проверка того, что даже вновь созданная запись о машине не позволяет арендовать её
        /// на срок окончание которого раньше начала
        /// </summary>
        [TestMethod()]
        public void NewCarCantBeRentedWhenStartisLaterEndTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            DateTime startOfRent = new DateTime(2017, 1, 2);
            DateTime endOfRent = new DateTime(2017, 1, 1);
            bool isCarFreeToRent = carRecord.IsFreeToRent(startOfRent, endOfRent);
            Assert.AreEqual(isCarFreeToRent, false);
        }

        /// <summary>
        /// Проверка того, что даже вновь созданная запись о машине не позволяет арендовать её
        /// на 0 дней
        /// </summary>
        [TestMethod()]
        public void NewCarCanBeRentedFor1DayTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            DateTime startOfRent = new DateTime(2017, 1, 1);
            DateTime endOfRent = startOfRent;
            bool isCarFreeToRent = carRecord.IsFreeToRent(startOfRent, endOfRent);
            Assert.AreEqual(isCarFreeToRent, true);
        }

        /// <summary>
        /// Проверка того, что при удачной аренде нового авто расписание аренд изменяется
        /// </summary>
        [TestMethod()]
        public void NewCarIsRentedFoProperTermTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            DateTime startOfRent = new DateTime(2017, 1, 1);
            DateTime endOfRent = startOfRent;
            bool isCarRented = carRecord.Rent(startOfRent, endOfRent);
            Assert.AreEqual(isCarRented, true);
            Assert.AreEqual(carRecord.getStartRentDates()[0], startOfRent);
            Assert.AreEqual(carRecord.getEndRentDates()[0], endOfRent);
        }


        /// <summary>
        /// Проверка того, что машина не может быть арендована на срок,
        /// перекрывающие период, который уже забронирован
        /// </summary>
        [TestMethod()]
        public void CarCantBeRentedForOverlapingTermsTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            DateTime startOfRent = new DateTime(2017, 1, 1);
            DateTime endOfRent = new DateTime(2017, 1, 20); ;
            // арендуем первый раз
            bool carRentSuccess = carRecord.Rent(startOfRent, endOfRent);
            DateTime[] startRentDates = carRecord.getStartRentDates();
            DateTime[] endRentDates = carRecord.getEndRentDates();

            // 1. пробуем арендовать второй раз
            carRentSuccess = carRecord.Rent(startOfRent.AddDays(5), endOfRent.AddDays(5));
            Assert.AreEqual(carRentSuccess, false);
            CollectionAssert.AreEqual(carRecord.getStartRentDates(), startRentDates);
            CollectionAssert.AreEqual(carRecord.getEndRentDates(), endRentDates);
            // 2. пробуем арендовать второй раз
            carRentSuccess = carRecord.Rent(startOfRent.AddDays(-5), endOfRent.AddDays(-5));
            Assert.AreEqual(carRentSuccess, false);
            CollectionAssert.AreEqual(carRecord.getStartRentDates(), startRentDates);
            CollectionAssert.AreEqual(carRecord.getEndRentDates(), endRentDates);
            // 3. пробуем арендовать второй раз
            carRentSuccess = carRecord.Rent(startOfRent.AddDays(-5), endOfRent.AddDays(5));
            Assert.AreEqual(carRentSuccess, false);
            CollectionAssert.AreEqual(carRecord.getStartRentDates(), startRentDates);
            CollectionAssert.AreEqual(carRecord.getEndRentDates(), endRentDates);
            // 4. пробуем арендовать второй раз
            carRentSuccess = carRecord.Rent(startOfRent.AddDays(-5), startOfRent);
            Assert.AreEqual(carRentSuccess, false);
            CollectionAssert.AreEqual(carRecord.getStartRentDates(), startRentDates);
            CollectionAssert.AreEqual(carRecord.getEndRentDates(), endRentDates);
        }

        /// <summary>
        /// Проверка того, что машина может быть арендована на второй срок,
        /// если он правильный
        /// </summary>
        [TestMethod()]
        public void CarCanBeRentedSecondTimeForProperTermTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            DateTime[] startRentDates = {new DateTime(2017, 1, 1), new DateTime(2017, 1, 21) };
            DateTime[] endRentDates = {new DateTime(2017, 1, 20), new DateTime(2017, 1, 22) };
            // арендуем первый раз
            bool carRentSuccess = carRecord.Rent(startRentDates[0], endRentDates[0]);
            Assert.AreEqual(carRentSuccess, true);
            // арендуем 2 раз
            carRentSuccess = carRecord.Rent(startRentDates[1], endRentDates[1]);
            Assert.AreEqual(carRentSuccess, true);

            CollectionAssert.AreEqual(carRecord.getStartRentDates(), startRentDates);
            CollectionAssert.AreEqual(carRecord.getEndRentDates(), endRentDates);
        }

        /// <summary>
        /// Проверка того, что
        /// 1. машина не может быть арендована на время проведения уже запланированного ТО
        /// 2. машина может быть арендована на 10 раз после очередного ТО на время,
        /// которое ни с чем не пересекается
        /// 3. машина не может быть арендована на такое время,
        /// при котором новые времема проведения ТО в полученном после такой аренды
        /// расписании пересекутся с уже заброннированными временами аренды
        /// </summary>
        [TestMethod()]
        public void CarRentWithCarCheckUpTest()
        {
            CarRecord carRecord = new CarRecord("Lada Granta", "cherry", "1234321");
            //                                  0                           1                       2                           3                           4                           5                       6                           7                           8                       9
            DateTime[] startRentDates = {   new DateTime(2017, 1, 1),  new DateTime(2017, 2, 1), new DateTime(2017, 2, 3), new DateTime(2017, 2, 5), new DateTime(2017, 2, 6), new DateTime(2017, 2, 7), new DateTime(2017, 2, 8), new DateTime(2017, 2, 9), new DateTime(2017, 2, 10), new DateTime(2017, 2, 21) };
            DateTime[] endRentDates = {     new DateTime(2017, 1, 20), new DateTime(2017, 2, 2), new DateTime(2017, 2, 4), new DateTime(2017, 2, 5), new DateTime(2017, 2, 6), new DateTime(2017, 2, 7), new DateTime(2017, 2, 8), new DateTime(2017, 2, 9), new DateTime(2017, 2, 10), new DateTime(2017, 2, 23) };
            bool carRentSuccess;

            for (int i = 0; i < startRentDates.Count(); i++)
            {
                carRentSuccess = carRecord.Rent(startRentDates[i], endRentDates[i]);
                Assert.AreEqual(carRentSuccess, true);
            }

            // Время аренды попадает на уже запланированное ТО
            DateTime startOfRent = new DateTime(2017, 2, 24);
            DateTime endOfRent = new DateTime(2017, 2, 25);
            // пробуем арендовать во время запланированного ТО 
            carRentSuccess = carRecord.Rent(startOfRent, endOfRent);
            Assert.AreEqual(carRentSuccess, false);

            // Аренда, после которой придётся делать ТО, время которого пересечётся
            // с уже запланированной бронью:
            startOfRent = new DateTime(2017, 2, 19);
            endOfRent = new DateTime(2017, 2, 20);
            // пробуем арендовать
            carRentSuccess = carRecord.Rent(startOfRent, endOfRent);
            Assert.AreEqual(carRentSuccess, false);

            // Нормальная аренда:
            startOfRent = new DateTime(2017, 2, 12);
            endOfRent = new DateTime(2017, 2, 12);
            // пробуем арендовать на время, которое ни с чем не пересекается 
            carRentSuccess = carRecord.Rent(startOfRent, endOfRent);
            Assert.AreEqual(carRentSuccess, true);

            // Аренда, сдвигающая расписание так, что только что добавленная нормальная аренда
            // попадёт на период ТО:
            startOfRent = new DateTime(2017, 1, 21);
            endOfRent = new DateTime(2017, 1, 21);
            // пробуем арендовать 
            carRentSuccess = carRecord.Rent(startOfRent, endOfRent);
            Assert.AreEqual(carRentSuccess, false);
        }

    }
}