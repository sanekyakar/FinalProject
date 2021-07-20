using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;

using Epam.Internet_shop.Common.Entities.CommodityUnit;
using Epam.Internet_shop.Common;
using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;

namespace Epam.Internet_shop.PL.Web.Handlers
{
    public class ShowcaseAjaxHandler : IHttpHandler
    {
        public static HttpSessionStateBase HttpSession { get; set; } = null;

        private static ILogger _logger = DependencyResolver.Logger;
        private static IStatusBll _statusBll = DependencyResolver.StatusBll;
        private static ICommodityUnitBll _commodityUnitBll = DependencyResolver.CommodityUnitBll;
        private static Status _status = _statusBll.GetAllStatuses().First(stat => stat.Name == "В наличии");

        public void ProcessRequest(HttpContext context)
        {
            _logger.Info($"PL.{nameof(ShowcaseAjaxHandler)}: Request received");

            IEnumerator<CommodityUnit> enumerator = HttpSession["Enumerator"] as IEnumerator<CommodityUnit>;

            if (enumerator == null)
            {
                _logger.Info($"PL.{nameof(ShowcaseAjaxHandler)}: Creating the enumerator");

                int? catalogueId = (int) HttpSession["Catalogue"];
                string searchStr = (string) HttpSession["Search"];

                //Regex.IsMatch(, searchStr);

                enumerator = _commodityUnitBll.GetCommodityUnitsByStatus(_status?.Id ?? 0)
                            .Where(unit => catalogueId == 0 ? true : (unit.Product.Category.Id == catalogueId))
                            .Where(unit => string.IsNullOrEmpty(searchStr) ? true : Regex.IsMatch(unit.Product.Name.ToLower(), searchStr.ToLower()))
                            .GetEnumerator();

                HttpSession.Add("Enumerator", enumerator);

                _logger.Info($"PL.{nameof(ShowcaseAjaxHandler)}: The enumerator was created");
            }

            var list = GetList(enumerator, 8);
            
            context.Response.ContentType = "application/json";

            context.Response.Write(JsonConvert.SerializeObject(list));

            _logger.Info($"PL.{nameof(ShowcaseAjaxHandler)}: Sent to client a Json");
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        private List<T> GetList<T>(IEnumerator<T> enumerator, int count)
        {
            _logger.Info($"PL.{nameof(ShowcaseAjaxHandler)}.{nameof(GetList)}: Receiving a list of commodity items");

            List <T> list = new List<T>();

            for (int i = 0; i < count && enumerator.MoveNext(); i++)
            {
                list.Add(enumerator.Current);
            }

            _logger.Info($"PL.{nameof(ShowcaseAjaxHandler)}.{nameof(GetList)}: Received a list of commodityUnits in the amount of " + list.Count);

            return list;
        }
    }
}