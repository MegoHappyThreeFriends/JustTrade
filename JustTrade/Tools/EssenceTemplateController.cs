using System;
using System.Web.Mvc;
using JustTrade.Database;
using JustTrade.Tools;

namespace JastTrade
{
    public abstract class EssenceTemplateController<T> : Controller
    {
        public virtual JsonResult Add(T item)
        {
            try
            {
                Repository<T>.Add(item);
            }
            catch(Exception ex)
            {
                return Json(JsonData.Create(ex));
            }
            return Json(JsonData.Create(true));
        }


    }
}

