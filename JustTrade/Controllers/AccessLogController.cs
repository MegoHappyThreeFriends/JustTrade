namespace JustTrade.Controllers
{
	using System.Web.Mvc;
	using JustTrade.Tools;
	using JustTrade.Tools.Attributes;
	using System;
	using System.Collections.Generic;
	using JustTrade.Database;
	using JustTrade.Tools.Security;

	public class AccessLogController : ControllerWithTools
	{
		[FreeAccess]
		[HttpGet]
		public ActionResult Index(Guid referenceId) {
			var accessLogList = new List<AccessLog>();
			using (var accesslogs = JTSecurity.Session.Db.Find<AccessLog>(new RepoFiler("Reference", referenceId))) {
				foreach (var accesslog in accesslogs) {
					var user = (accesslog.User != null ? (User)accesslog.User.Clone() : null);
					var accesslogItem = (AccessLog)accesslog.Clone();
					accesslogItem.User = user;
					accessLogList.Add(accesslogItem);
				}
			}
			return PartialView("_Index", accessLogList);
		}
	}
}