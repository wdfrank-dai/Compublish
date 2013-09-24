using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WidgetDate.Models;


namespace WidgetDate.Repository
{
    public class AppRepository
    {
        
        public Application GetApplicationByID(int id)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                return db.Application.FirstOrDefault(o=>o.id == id);
            }
            catch
            {
                return null;
            }
        }

        public ApplicationPage GetOneApplicationPageByAppID(int id)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();
                return db.ApplicationPage.FirstOrDefault(o=>o.id == id);
            }
            catch {
                return null;
            }
        }

        public ApplicationPage GetOneApplicationPageByID(int id)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();
                return db.ApplicationPage.FirstOrDefault(o => o.id == id);
            }
            catch
            {
                return null;
            }
        }

        public ApplicationUnits GetAppApplicationUnitByID(int id)
        {
            try
            { 
                WidgetDataContext db = new WidgetDataContext();
                return db.ApplicationUnits.FirstOrDefault(o => o.id == id);
            }
            catch
            {
                return null;   
            }
        }

        public ApplicationDatasource GetApplicationDatasourceByID(int id)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();
                return db.ApplicationDatasource.FirstOrDefault(o => o.id == id);
            }
            catch
            {
                return null;
            }

        }

        public ApplicationShowType ApplicationShowTypeByID(int id)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();
                return db.ApplicationShowType.FirstOrDefault(o => o.id == id);
            }
            catch
            {
                return null;
            }

        }


        public List<ApplicationUnitsCss> ApplicationUnitsCssByID(int id)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();
   
                return db.ApplicationUnitsCss.Where(o => o.unitsid == id).ToList();
            }
            catch
            {
                return null;
            }

        }

        public ApplicationUnitsShowStyle GetApplicationUnitsShowTypeByID(int id)
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                return db.ApplicationUnitsShowStyle.FirstOrDefault(o => o.id == id);
            }
            catch
            {
                return null;
            }
        }
        public List<ApplicationUnitsShowStyle> GetAllApplicationUnitsShowType()
        {
            try
            {
                WidgetDataContext db = new WidgetDataContext();

                return db.ApplicationUnitsShowStyle.ToList();
            }
            catch
            {
                return null;
            }
        }


    }
}