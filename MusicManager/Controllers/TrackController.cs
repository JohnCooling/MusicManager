using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.ModelLogic;
using DAL.Repository;
using DomainModel;
using MusicManager.ViewModels;
using MvcPaging;

namespace MusicManager.Controllers
{
    public class TrackController : Controller
    {
        Repository<FileMetaData> _repo = new Repository<FileMetaData>();
        private const int _defaultPageSize = 10;
        private IList<FileMetaData> allFiles = new List<FileMetaData>();

        public ActionResult Index(string song_name, int? page)
        {
            allFiles = _repo.GetAll().ToList();
            int currentPageIndex = page.HasValue ? page.Value : 1;

            ViewData["song_name"] = song_name;

            if (string.IsNullOrWhiteSpace(song_name))
            {
                allFiles = allFiles.ToPagedList(currentPageIndex, _defaultPageSize);
            }
            else
            {
                allFiles = allFiles.Where(p => p.Name.ToLower() == song_name.ToLower()).ToPagedList(currentPageIndex, _defaultPageSize);
            }
            
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxEmployeeList", allFiles);
            else
                return View(allFiles);
        }


        public ActionResult Edit(int id)
        {

            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {


            return View();
        }
    }
}