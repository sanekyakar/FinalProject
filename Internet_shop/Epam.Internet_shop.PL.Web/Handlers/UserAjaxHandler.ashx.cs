using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;

using Epam.Internet_shop.Common.Entities.User;
using Epam.Internet_shop.Common;
using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;

namespace Epam.Internet_shop.PL.Web.Handlers
{
    public class UserAjaxHandler : IHttpHandler
    {
        public static HttpSessionStateBase HttpSession { get; set; } = null;

        private static ILogger _logger = DependencyResolver.Logger;
        private static IUserBll _userBll = DependencyResolver.UserBll;

        public void ProcessRequest(HttpContext context)
        {
            _logger.Info($"PL.{nameof(UserAjaxHandler)}: Request received");

            IEnumerator<User> enumerator = HttpSession["Enumerator"] as IEnumerator<User>;

            if (enumerator == null)
            {
                _logger.Info($"PL.{nameof(UserAjaxHandler)}: Creating the enumerator");

                string searchStr = (string)HttpSession["Search"];

                enumerator = _userBll.GetAllUsers()
                    .Where(user => string.IsNullOrEmpty(searchStr) ? true : Regex.IsMatch(user.Id.ToString(), searchStr.ToLower()) || 
                                                                            Regex.IsMatch(user.Login.ToLower(), searchStr.ToLower()) ||
                                                                            Regex.IsMatch(user.Name.ToLower(), searchStr.ToLower()))
                    .GetEnumerator();

                HttpSession.Add("Enumerator", enumerator);

                _logger.Info($"PL.{nameof(UserAjaxHandler)}: The enumerator was created");
            }

            var list = GetList(enumerator, 15);

            context.Response.ContentType = "application/json";

            context.Response.Write(JsonConvert.SerializeObject(list));

            _logger.Info($"PL.{nameof(UserAjaxHandler)}: Sent to client a Json");
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
            _logger.Info($"PL.{nameof(UserAjaxHandler)}.{nameof(GetList)}: Receiving a list of commodity items");

            List<T> list = new List<T>();

            for (int i = 0; i < count && enumerator.MoveNext(); i++)
            {
                list.Add(enumerator.Current);
            }

            _logger.Info($"PL.{nameof(UserAjaxHandler)}.{nameof(GetList)}: Received a list of commodityUnits in the amount of " + list.Count);

            return list;
        }
    }
}