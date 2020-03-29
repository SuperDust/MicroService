using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string[] userAry = null;
            string[] commodityAry = null;
            using (HttpClient httpClient = new HttpClient()) 
            {
                HttpResponseMessage httpResponseMessage  = httpClient.GetAsync("http://192.168.31.137:7000/dust1/user").Result;

                userAry = JsonConvert.DeserializeObject<string[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage = httpClient.GetAsync("http://192.168.31.137:7000/dust/commodity").Result;

                commodityAry = JsonConvert.DeserializeObject<string[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
            this.ViewBag.msgText = $"{userAry[0]}和{userAry[1]}讨论项目,去烧烤摊点了{commodityAry[0]},{commodityAry[1]},{commodityAry[2]},第二天凌晨5点两个人支起了早餐摊！";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
