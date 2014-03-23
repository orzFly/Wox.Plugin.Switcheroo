using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
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

        public String IconImageDataUri(S.AppWindow self)
        {
            var key = "IconImageDataUri-" + self.HWnd;
            var iconImageDataUri = System.Runtime.Caching.MemoryCache.Default.Get(key) as String; ;
            if (iconImageDataUri == null)
            {
                var iconImage = self.IconImage;
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(iconImage));
                        encoder.Save(memoryStream);
                        var b64String = Convert.ToBase64String(memoryStream.ToArray());
                        iconImageDataUri = "data:image/png;base64," + b64String;
                        System.Runtime.Caching.MemoryCache.Default.Add(key, iconImageDataUri, DateTimeOffset.Now.AddHours(1));
                    } 
                }
                catch {
                    return null;
                }
            }
            return iconImageDataUri;
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
                    IcoPath = IconImageDataUri(o.AppWindow),
                    Action = con =>
                    {
                        context.HideApp();
                        if (con.SpecialKeyState.CtrlPressed)
                        {
                            o.AppWindow.PostClose();
                            o.AppWindow.SwitchTo();
                        }
                        else
                        {
                            o.AppWindow.SwitchTo();
                        }
                        return true;
                    }
                };
            }).ToList();
        }
    }
}
