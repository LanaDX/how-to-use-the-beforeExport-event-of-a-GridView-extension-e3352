﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Razor.Models;
using DevExpress.Web.Mvc;
using System.Web.UI;

namespace DevExpress.Razor.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return BeforeExport();
        }
        public ActionResult BeforeExport() {
            return View("BeforeExport", NorthwindDataProvider.GetEmployees());
        }
        public ActionResult BeforeExportPartial() {
            return PartialView("BeforeExportPartial", NorthwindDataProvider.GetEmployees());
        }
        public ActionResult ExportTo() {
            return GridViewExtension.ExportToPdf(GridViewHelper.GetExportSettings(Request.Params["ExportColumnsNames"]), NorthwindDataProvider.GetEmployees());
        }
    }

    public static class GridViewHelper {
        public static GridViewSettings GetExportSettings(string itemsNames) {
            GridViewSettings gridVieewSettings = GetExportSettings();

            if (!string.IsNullOrEmpty(itemsNames)) {
                string[] names = itemsNames.Split(';');
                gridVieewSettings.SettingsExport.BeforeExport = (sender, e) => {
                    MVCxGridView gridView  = sender as MVCxGridView;
                    if (sender == null)
                        return;

                    gridView.Columns.Clear();

                    foreach (var name in names) {
                        if (string.IsNullOrEmpty(name)) continue;
                        gridView.Columns.Add(new MVCxGridViewColumn(name));
                    }
                };
            }
            
            return gridVieewSettings;
        }
        public static GridViewSettings GetExportSettings() {
            GridViewSettings gridVieewSettings = new GridViewSettings();
            gridVieewSettings.Name = "gridView";
            gridVieewSettings.CallbackRouteValues = new { Controller = "Home", Action = "BeforeExportPartial" };

            gridVieewSettings.Columns.Add("FirstName");
            gridVieewSettings.Columns.Add("LastName");
            gridVieewSettings.Columns.Add("BirthDate");
            gridVieewSettings.Columns.Add("Title");

            return gridVieewSettings;
        }
    }
}
