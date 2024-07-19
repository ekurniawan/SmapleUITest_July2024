using CreditCards.APITest.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CreditCards.APITest
{
    public class TestAPIDepartment
    {
        private readonly HttpClient _httpClient;
        public TestAPIDepartment()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7072/api/");
        }

        [Fact]
        public async Task Test_GetAllDepartment_Return200OK()
        {
            HttpStatusCode expected = HttpStatusCode.OK;
            HttpResponseMessage response = await _httpClient.GetAsync("Departments");
            HttpStatusCode actual = response.StatusCode;

            Assert.Equal(expected, actual);
        }

        private async Task<IEnumerable<Department>> GetAllData()
        {
            List<Department> departments = new List<Department>();

            HttpResponseMessage response = await _httpClient.GetAsync("Departments");
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                departments = JsonSerializer.Deserialize<List<Department>>(responseString);
            }

            return departments;
        }

        /*[Fact]
        public async Task Test_GetAllDepartment_Return4()
        {
            int expectedRow = 4;
            var actual = await GetAllData();
            Assert.Equal(actual.ToList().Count, expectedRow);
        }*/

        [Fact]
        public async Task Test_InsertSuccess()
        {
            //get data sebelum insert
            var dataBefore = await GetAllData();
            int rowBefore = dataBefore.ToList().Count;


            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            var newDepartment = new Department
            {
                DepartmentName = "Research"
            };

            var json = JsonSerializer.Serialize(newDepartment);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("Departments", data);

            HttpStatusCode actual = response.StatusCode;

            var expectedRowCount = (await GetAllData()).ToList().Count;

            Assert.Equal(httpStatusCode, actual);
            Assert.Equal(expectedRowCount, rowBefore + 1);

        }
    }
}