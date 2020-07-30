using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CovidTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
 
using Nancy.Json;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

// for serialize and deserialize

namespace CovidTracker.Controllers
{
    public class CovidController : Controller
    {

        private const string UrlCovidData = "https://coronavirus-19-api.herokuapp.com/countries";
        private const string UrlGlobalSummary = "https://coronavirus-19-api.herokuapp.com/all";
        public void LoadCountriesData()
        {
           
            using (StreamReader r = new StreamReader(@"Data\countries.json"))
            {
                string json = r.ReadToEnd();
                List<CountriesModel> items = JsonConvert.DeserializeObject<List<CountriesModel>>(json);

                dynamic array = JsonConvert.DeserializeObject(json);
                //foreach (var item in array)
                //{
                //    Console.WriteLine("{0} {1}", item.name, item.name);
                     
                    
                //}
                
               // ViewBag.memberList = items.ToList();


            }
        }
        public async Task <IList<CovidModel>> Covid(string strCountryName = null)
        {

          
            List<CovidModel> covidModelsList = new List<CovidModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(UrlCovidData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();


                    covidModelsList = JsonConvert.DeserializeObject<List<CovidModel>>(apiResponse);


                }


                if (!string.IsNullOrEmpty(strCountryName))
                {

                    covidModelsList = covidModelsList.Where(x => x.Country.Trim().ToLower() == strCountryName.Trim().ToLower()).ToList();
                    var count = covidModelsList.Count();
                }
            }

            return covidModelsList;

        }

        [HttpGet]
        public async Task<IActionResult> Index( string strCountryName)
        {

            LoadCountriesData();

            CovidGlobal covidGlobalList = new CovidGlobal();
            IList<CovidModel> CovidData = new List<CovidModel>();
            CovidData = await Covid();


            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(UrlGlobalSummary))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    covidGlobalList = JsonConvert.DeserializeObject<CovidGlobal>(apiResponse);


                }



            }


            ViewBag.GlobalData = covidGlobalList;
            return View(CovidData);
        }
       
        public async Task<JsonResult> GetCoronaRecordByCountry(string strCountryName)
        {

           
            IList<CovidModel> CovidData = new List<CovidModel>();
            CovidData = await Covid(strCountryName);

            return Json(new{ data = CovidData}) ;
        }


        //public async Task<JsonResult> GlobalData()
        //{


        //    //CovidGlobal covidGlobalList = new CovidGlobal();
        //    //using (var httpClient = new HttpClient())
        //    //{
        //    //    using (var response = await httpClient.GetAsync(UrlGlobalSummary))
        //    //    {
        //    //        string apiResponse = await response.Content.ReadAsStringAsync();
        //    //        covidGlobalList = JsonConvert.DeserializeObject<CovidGlobal>(apiResponse);

                   
        //    //    }


                
        //    //}

        //    //StringBuilder sb = new StringBuilder();
        //    //StringWriter sw = new StringWriter();

        //    //using (JsonTextWriter jw = new JsonTextWriter(sw))
        //    //{
        //    //    foreach (var item in covidGlobalList)
        //    //    {
        //    //        jw.WriteStartObject();
        //    //        var globalData = new CovidGlobal
        //    //        {
        //    //            cases = item.cases,
        //    //            deaths = item.deaths,
        //    //            recovered = item.recovered

        //    //        };
        //    //    }


        //    //    jw.WriteEndObject();
        //    //    jw.WriteRaw("\n");

        //    //}

            

        //    //ViewBag.GlobalData = covidGlobalList;
        //    //return RedirectToAction("Index", "Covid", covidGlobalList);

        //    //return Json(new { data = covidGlobalList });
        //}



    }
}