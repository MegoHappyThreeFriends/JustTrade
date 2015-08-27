namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Threading;
	using System.Web.Helpers;
	using System.Web.Mvc;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Tools;

	public class DatabaseController : ControllerWithTools
	{
		private static readonly ProgressInformation _deleteUnusesObjectProgress = new ProgressInformation();

		[HttpGet]
		public ActionResult Index() {
			return PartialView("../Administrator/Database/_Index");
		}

		[HttpGet]
		public ActionResult GenerateDatabase() {
			try {
				NHibernateHelper.CreateDb();
			} catch (Exception ex) {
				ViewBag.Header = Lang.Get("Error generate database");
				ViewBag.Description = ex.ToString();
				return PartialView("../Administrator/Database/_ErrorGenerateDatabase");
			}
			return PartialView("../Administrator/Database/_SuccessGeneratedDatabase");
		}

		public ActionResult DeleteUnusedObject() {
			if (_deleteUnusesObjectProgress.IsRunning) {
				return GenerateErrorMessage(Lang.Get("Removal process has already begun"), "");
			}
			_deleteUnusesObjectProgress.Progress = 0;
			var worker = new Thread(DeleteUnusedObjectWorker) {
				IsBackground  = true
			};
			worker.Start();
			return new EmptyResult();
		}

		public ActionResult DeletingProgress() {
			return Json(_deleteUnusesObjectProgress, JsonRequestBehavior.AllowGet);
		}

		private void DeleteUnusedObjectWorker() {
			try {
				_deleteUnusesObjectProgress.IsRunning = true;
				for (int i = 0; i <= 100; i++) {
					Thread.Sleep(200);
					_deleteUnusesObjectProgress.Progress = i;
					_deleteUnusesObjectProgress.Information = i + " % Complete ";
				}
			} catch (Exception ex) {
				_deleteUnusesObjectProgress.Progress = -1;
				_deleteUnusesObjectProgress.Error = Lang.Get("Error during remove unused object");
				_deleteUnusesObjectProgress.Information = ex.ToString();
			} finally {
				_deleteUnusesObjectProgress.IsRunning = false;
			}
		}


	}
}