using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using Employee_PayRoll_RestSharp;
namespace RestSharp_Test
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient(" http://localhost:3000");
        }

        /// <summary>
        /// Gets the employee list.
        /// </summary>
        /// <returns></returns>
        //interface class to get list of employees
        private IRestResponse GetEmployeeList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            //passing rest request for employees list api using get method
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Act
            //Execute the request
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC 1 : Retrieve all employee details in the json file
        /// </summary>
        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            //calling the method
            IRestResponse response = GetEmployeeList();
            //checks if the status code of response equals the employee code for the method requested
            //and checks response as okay or not
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //convert the response object to list of employees
            //get 
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            //checking whether list is equal to count
            Assert.AreEqual(5, employeeList.Count);

            foreach (Employee emp in employeeList)
            {
                Console.WriteLine("Id: " + emp.Id + "\t" + "Name: " + emp.Name + "\t" + "Salary: " + emp.Salary);
            }
        }

        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            //Arrange
            //Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("/employees/list", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("name", "Rajendra");
            jsonObj.Add("salary", "714034");
            jsonObj.Add("id", "5");

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Rajendra", employee.Name);
            Assert.AreEqual("714034", employee.Salary);
            Console.WriteLine(response.Content);
        }
    }
}

