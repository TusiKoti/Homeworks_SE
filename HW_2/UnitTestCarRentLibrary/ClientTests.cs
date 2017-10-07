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
    public class ClientTests
    {
        [TestMethod()]
        public void CarCantBeRentedFromServiceWithNoCarsTest()
        {
            RentService service = new RentService(); // сервис без машин
            string required_model = "Lada Kalina";
            string required_color = "yellow";
            DateTime startOfRent = new DateTime(2017, 1, 1);
            DateTime endOfRent = new DateTime(2017, 1, 2);
            Client client = new Client("Иванов Иван", "1234567890");
            bool rentResult = client.RentCar(service, required_model, required_color, startOfRent, endOfRent);
            Assert.AreEqual(rentResult,false);
        }
    }
}