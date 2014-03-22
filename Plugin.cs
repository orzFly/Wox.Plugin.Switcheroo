using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using S = Switcheroo;

namespace Wox.Plugin.Switcheroo
{
    public class Plugin : IPlugin
    {
        protected PluginInitContext context;

        public void Init(PluginInitContext context)
        {
            this.context = context;
            S.CoreStuff.Initialize();
        }

        public List<Result> Query(Query query)
        {
            var queryString = query.GetAllRemainingParameter();
            S.CoreStuff.WindowList.Clear();
            S.CoreStuff.GetWindows();

            var filterResults = S.CoreStuff.FilterList(queryString).ToList();

            return filterResults.Select(o =>
            {
                return new Result()
                {
                    Title = o.AppWindow.Title,
                    SubTitle = o.AppWindow.ProcessTitle,
                    Action = con =>
                    {
                        context.HideApp();
                        o.AppWindow.SwitchTo();
                        return true;
                    }
                };
            }).ToList();
        }
    }
}
