using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace RestSharpTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;   
        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }
        public IRestResponse getEmployeeList()
        {
            //Arrange
            RestRequest request = new RestRequest("/employee", Method.GET);
            //Act
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();
            //Assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(9, dataResponse.Count);

            foreach(Employee e in dataResponse)
            {
                System.Console.WriteLine("ID: "+e.id+"\t Name: "+e.name+"\t Salary: "+e.salary);
            }
        }
        [TestMethod]
        public void GivenEmployee_OnPOSTApi_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employee", Method.POST);
            JObject jObject = new JObject();
            jObject.Add("name", "PQR");
            jObject.Add("salary","1000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponce = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("PQR", dataResponce.name);
            Assert.AreEqual("1000", dataResponce.salary);
            System.Console.WriteLine(response.Content);
        }

        
    }
}
