using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.ModelLogic;
using DAL.Repository;
using DomainModel;
using MusicManager.ViewModels;

namespace MusicManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<FileMetaData> fileList;
            int fileCount;

            using (Repository<FileMetaData> rep = new Repository<FileMetaData>())
            {
                fileList = rep.GetAll().ToList();
            }

            fileCount = fileList.Count();
            
            ViewBag.FileCount = fileCount;

            var myList = fileList.OrderByDescending(x => x.ID).Take(7);
            
            return View(myList);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}