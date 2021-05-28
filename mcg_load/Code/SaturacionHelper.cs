using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace mcg_load.Code
{
    public class SaturacionHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<vw_demanda> GetSaturacions()
        {
            List<vw_demanda> tempoDemandas = db.vw_demanda.ToList();
            return tempoDemandas;
        }

        public static List<CAT_Plantas> GetPlantas()
        {
            return db.CAT_Plantas.ToList();
        }


        public static List<Contact> GetCustomers()
        {
            return DataProvider.GetContacts();
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(vw_demanda vwDemanda)
        {
            //DataProvider.AddNewIssue(issue);
        }

        public static void UpdateRecord(vw_demanda vwDemanda)
        {
            //DataProvider.UpdateIssue(issue);
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            DataProvider.DeleteIssues(selectedIds);
        }
    }
}