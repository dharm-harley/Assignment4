using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Assignment4; 
//using Assignment4.Models;
using static Assignment4.Models.EF_Models;
using Assignment4.Models;
using Assignment4.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    public class HomeController : Controller
    {
        //base url for the API call
        string BASE_URL = "https://chronicdata.cdc.gov/resource/vba9-s8jp.json?";
        //based on the API documentation, we are filtering out these fields.
        string apiFields = "&_fields=locationdesc,yearstart,race_ethnicity,data_value,sample_size,&per_page=30";
        //API key to access the API data
        string apiKey = "&api_key=tK0cOMLCIB736DDnIdSqbKRv17sFH6dspKl9MZwY";
        HttpClient httpClient;
       

        private readonly ILogger<HomeController> _logger;
         ApplicationDbContext applicationDbContext;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

// method to convert he feild names and values that we recieve in the HTTP response from the API.
        public string replaceString(string responseString)
        {
            string responseStringModified = responseString;
            responseStringModified = responseStringModified.Replace("school.name", "schoolName");
            responseStringModified = responseStringModified.Replace("school.school_url", "schoolUrl");
            responseStringModified = responseStringModified.Replace("2018.student.size", "studentSize");
            responseStringModified = responseStringModified.Replace("school.city", "schoolCity");
            responseStringModified = responseStringModified.Replace("latest.cost.tuition.out_of_state", "tuitionOutState");
            responseStringModified = responseStringModified.Replace("school.accreditor_code", "accCode");
            responseStringModified = responseStringModified.Replace("id", "uId");
            responseStringModified = responseStringModified.Replace("FloruIda", "Florida");
            responseStringModified = responseStringModified.Replace("floruIda", "florida");

            return responseStringModified;
        }

        [HttpPost]
        public IActionResult GetReportsByYear(string year)
        {
            string apiExtension = "yearstart=" + year;
           
            string API_PATH = BASE_URL + apiExtension;

            string responseString = "";
            Results[] data = null;
      
            httpClient.BaseAddress = new Uri(API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(API_PATH).GetAwaiter().GetResult();

            // Read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                
                responseString = replaceString(responseString);
            }

            // Parse the Json strings as C# objects
            if (!responseString.Equals(""))
            {
                data = System.Text.Json.JsonSerializer.Deserialize<Results[]>(responseString);

                //data = JsonConvert.DeserializeObject<UniversityData>(responseString);
            }
            ViewBag.search = "name";

            //foreach (Results item in data)
            //{
              //  applicationDbContext.Results.Add(item);
            //}
          
            return View("Explore", data);
        }

        [HttpPost]
        public IActionResult GetReportsByState(string state)
        {
            string apiExtension = "locationabbr=" + state;
          //  apiFields = "&_fields=id,school.school_url,school.name,2018.student.size,school.city,latest.cost.tuition.out_of_state,school.accreditor_code,&per_page=30";
            string API_PATH = BASE_URL + apiExtension;

            string responseString = "";
            Results[] data = null;

            
            httpClient.BaseAddress = new Uri(API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(API_PATH).GetAwaiter().GetResult();

            // Read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                responseString = replaceString(responseString);

            }

            // Parse the Json strings as C# objects
            
            if (!responseString.Equals(""))
            {
                 data = System.Text.Json.JsonSerializer.Deserialize<Results[]>(responseString);
               
            }
            return View("Explore", data);
        }

        [HttpPost]
        public IActionResult GetUniversitiesByStateChart(string state)
        {
            string apiExtension = "locationabbr=" + state;
                //+ "&latest.cost.tuition.out_of_state__range=100..";
            //apiFields = "&_fields=id,school.school_url,school.name,2018.student.size,school.city,latest.cost.tuition.out_of_state,school.accreditor_code,&per_page=5";

            string API_PATH = BASE_URL + apiExtension;
                //+ apiFields + apiKey;
            string responseString = "";
            Results[] data = null;

            // Connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(API_PATH).GetAwaiter().GetResult();

            // Read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                responseString = replaceString(responseString);
            }

            // Parse the Json strings as C# objects
            if (!responseString.Equals(""))
            {
                data = System.Text.Json.JsonSerializer.Deserialize<Results[]>(responseString);               
            }
            ViewBag.State = state;
            
            List<Results> newData = new List<Results>();
            foreach (var item in data) {
                if (!String.IsNullOrEmpty(item.data_value) && float.Parse(item.data_value) != 0.0)
                {

                    newData.Add(item);
                }
                
            }
            Results[] dat1 = newData.ToArray();
            
            return View("Charts", dat1);
        }

       

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Charts()
        {
            ViewBag.State = "AL";
            return GetUniversitiesByStateChart("AL");
        }

        
        /*public IActionResult Details(int id)
        {
            string apiExtension = "id=" + id;
            apiFields = "&_fields=id,school.school_url,school.name,2018.student.size,school.city,latest.cost.tuition.out_of_state,school.accreditor_code,&per_page=5";

            string API_PATH = BASE_URL + apiExtension + apiFields + apiKey;
            string responseString = "";
            UniversityData data = null;

            
            httpClient.BaseAddress = new Uri(API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(API_PATH).GetAwaiter().GetResult();

            
            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                responseString = replaceString(responseString);
            }

           
            if (!responseString.Equals(""))
            {
                data = System.Text.Json.JsonSerializer.Deserialize<UniversityData>(responseString);              
            }
            
            return View(data);
        }*/


        [HttpPost]
        public IActionResult SignUp(SignUp signUp)
        {
            if(ModelState.IsValid)
            {
                SignUp k = applicationDbContext.SignUp.Find(signUp.email);
                if (k == null)
                {
                    applicationDbContext.SignUp.Add(signUp);
                    applicationDbContext.SaveChanges();
                }
                else
                {
                    ViewBag.errorCode = 1;
                    ViewBag.errorMessage = "You have already registered, Thank you!";
                    return View();
                }
            }
            return View();
        }

     
        public IActionResult mainpage()
        {
            return View();
        }

        public IActionResult aboutUs()
        {
            return View();
        }
        public IActionResult create()
        {
            return View();
        }
        public IActionResult update()
        {
            return View();
        }
        public IActionResult delete()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Explore()
        {
            return View();
        }

        public IActionResult RegisteredUsers()
        {
            IEnumerable<SignUp> allUsers = applicationDbContext.SignUp;
            return View(allUsers);
        }

        public IActionResult DeleteUser(string email)
        {
            SignUp user = applicationDbContext.SignUp.Find(email);
            if (user != null)
            {
                applicationDbContext.SignUp.Remove(user);
                applicationDbContext.SaveChanges();
            }
            IEnumerable<SignUp> allUsers = applicationDbContext.SignUp;
            return View("RegisteredUsers",allUsers);
        }

        public IActionResult UpdateUser(string email)
        {
            ViewBag.email = email;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateUser(SignUp userChanges)
        {
            var user = applicationDbContext.Attach(userChanges);
            user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            applicationDbContext.SaveChanges();
            IEnumerable<SignUp> allUsers = applicationDbContext.SignUp;
            return View("RegisteredUsers", allUsers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
    
}
