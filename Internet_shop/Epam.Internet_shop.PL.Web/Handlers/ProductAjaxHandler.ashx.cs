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
    public class ProductAjaxHandler : IHttpHandler
    {
        public static HttpSessionStateBase HttpSession { get; set; } = null;

        private static ILogger _logger = DependencyResolver.Logger;
        private static ICommodityUnitBll _commodityUnitBll = DependencyResolver.CommodityUnitBll;

        public void ProcessRequest(HttpContext context)
        {
            _logger.Info($"PL.{nameof(ProductAjaxHandler)}: Request received");

            IEnumerator<CommodityUnit> enumerator = HttpSession["Enumerator"] as IEnumerator<CommodityUnit>;

            if (enumerator == null)
            {
                _logger.Info($"PL.{nameof(ProductAjaxHandler)}: Creating the enumerator");

                string searchStr = (string)HttpSession["Search"];

                enumerator = _commodityUnitBll.GetAllCommodityUnits()
                    .Where(unit => string.IsNullOrEmpty(searchStr) ? true : Regex.IsMatch(unit.Product.Id.ToString(), searchStr.ToLower()) ||
                                                                            Regex.IsMatch(unit.Product.Name.ToLower(), searchStr.ToLower()))
                    .GetEnumerator();

                    HttpSession.Add("Enumerator", enumerator);

                    _logger.Info($"PL.{nameof(ProductAjaxHandler)}: The enumerator was created");
            }

            var list = GetList(enumerator, 15);

            context.Response.ContentType = "application/json";

            context.Response.Write(JsonConvert.SerializeObject(list));

            _logger.Info($"PL.{nameof(ProductAjaxHandler)}: Sent to client a Json");
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
            _logger.Info($"PL.{nameof(ProductAjaxHandler)}.{nameof(GetList)}: Receiving a list of commodity items");

            List<T> list = new List<T>();

            for (int i = 0; i < count && enumerator.MoveNext(); i++)
            {
                list.Add(enumerator.Current);
            }

            _logger.Info($"PL.{nameof(ProductAjaxHandler)}.{nameof(GetList)}: Received a list of commodityUnits in the amount of " + list.Count);

            return list;
        }
    }
}