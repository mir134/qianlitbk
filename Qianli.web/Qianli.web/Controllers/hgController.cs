using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api;
using NSoup.Nodes;
using NSoup.Parse;
using NSoup.Helper;
using System.Net;
using System.Text;
using NSoup.Select;
using FastJSON;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Qianli.web.Models;

namespace Qianli.web.Controllers
{
    public class hgController : Controller
    {
        [CompressAttribute]
        [OutputCache(CacheProfile = "Aggressive")]        
        public ActionResult Index(string publishId, string columnId, string subColumn)
        {
            string severUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["severUrl"];
            string appKey = System.Web.Configuration.WebConfigurationManager.AppSettings["appKey"];
            string appSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["appSecret"];
            //ViewBag.Title = "Home Page";            
            //ITopClient client = new DefaultTopClient(severUrl, appKey, appSecret, "json");
            //TbkItemGetRequest req = new TbkItemGetRequest();
            //req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url";
            //req.Cat = columnId;
            ////req.Q = "女装";
            
            //TbkItemGetResponse rsp = client.Execute(req);
           
           
            //List<NTbkItem> tbkitems = rsp.Results;
          //  Console.WriteLine(rsp.Body);
           // ViewData["NTbkItems"] = tbkitems;
            //ViewBag.strs = tbkitem;
            WebClient webClient = new WebClient();
            if (columnId == null) { columnId = "0"; }
            if (subColumn == null) { subColumn = ""; } else { subColumn = "&subColumn=" + subColumn; }
            string HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("https://headline.taobao.com/feed/feedQuery.do?columnId=" + columnId + "" + subColumn));

           // tring jsonText = "{\"beijing\":{\"zone\":\"海淀\",\"zone_en\":\"haidian\"}}";
            JObject jo = (JObject)JsonConvert.DeserializeObject(HtmlString);          

            JArray ja = (JArray)JsonConvert.DeserializeObject(jo["data"].ToString());

            ViewData["JArray"] = ja;
            ViewData["columnId"] = columnId;
            ViewData["subColumn"] = subColumn;
            return View();
        }
        [CompressAttribute]
        [OutputCache(CacheProfile = "Aggressive")]
        public ActionResult goods(string id, string publishId, string spm, string columnId)
        {
            ViewData["ID"] = id;
            ViewData["publishId"] = publishId;
            ViewData["JArray"] = new JArray();
           // WebRequest webRequest = WebRequest.Create("http://uz.taobao.com/detail/5555795696/");
           // NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webRequest.GetResponse().GetResponseStream(), "gbk");
            if (publishId != null) {
                if (columnId == null) { columnId = "0"; }
                WebClient webClient = new WebClient();
                string HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("https://headline.taobao.com/feed/feedQuery.do?columnId=" + columnId + "&publishId=" + publishId + ""));

                // tring jsonText = "{\"beijing\":{\"zone\":\"海淀\",\"zone_en\":\"haidian\"}}";
                JObject jo = (JObject)JsonConvert.DeserializeObject(HtmlString);

                JArray ja = (JArray)JsonConvert.DeserializeObject(jo["data"].ToString());

                ViewData["JArray"] = ja;
                ViewData["columnId"] = columnId;
            }
            if (spm == null)
            {
                if (id == null) { id = "454759"; }
                WebClient webClient = new WebClient();
                String HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("https://headline.taobao.com/feed/feedDetail.htm?id=" + id + "&columnId=0"));
                Document doc = NSoup.NSoupClient.Parse(HtmlString);
                Element bodys = doc.Select("div.detail").First();
                Elements items = bodys.Select("div.item,.detail-title,.detail-time");
                items.Remove();
                items = bodys.Select("img");
                items.Remove();
                items = bodys.Select("a");
                items.Remove();              
                ViewData["bodys"] = bodys.Html();
                ViewData["time"] = doc.Select("div.detail-time").Html();

                ViewBag.Title = doc.Title.Replace("淘宝", "网购") + "-千里商城";
            }else {
                if (id == null) { id = "5556414503"; }
                WebClient webClient = new WebClient();
                String HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("http://uz.taobao.com/detail/" + id + "/?spm=a21a0.7476711.0.0.19PZMI"));
                Document doc = NSoup.NSoupClient.Parse(HtmlString);
                Element bodys = doc.Select("div.site-main-content").First();
                Elements items = bodys.Select("div.item");
                items.Remove();
                items = bodys.Select("img");
                items.Remove();
                //foreach (Element item in items)
                //{
                //    item.Attr("abs:src");
                //    //item.TagName("img");

                //}
                // NSoup.Nodes.Document doc = NSoup.NSoupClient.Connect("http://uz.taobao.com/detail/5555795696/").UserAgent("Mozilla").Header("charset", "gbk").Get();
                // NSoup.Nodes.Element masthead = doc.ToString();
                ViewData["bodys"] = bodys.Html();
                ViewData["time"] = doc.Select("span.pub-time").Html();
                ViewBag.Title = doc.Title.Replace("淘宝", "网购") + "-千里商城";
                
            }
            
            return View();
        }
    }
}
