﻿using System;
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
using Qianli.web.Models.v;
using System.Data.Entity;


namespace Qianli.web.Controllers
{
    public class vController : Controller
    {
        QianliDataEntities _db; //数据库连接

        public vController()
        {
            _db = new QianliDataEntities();
        }

        [CompressAttribute]
        [OutputCache(CacheProfile = "Aggressive")]
        public ActionResult I(string id, string publishId, string subColumn)
        {
            ViewData.Model = _db.Toutiao.Where(t=>t.content.ToString()=="").Take(20).ToList();
            string severUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["severUrl"];
            string appKey = System.Web.Configuration.WebConfigurationManager.AppSettings["appKey"];
            string appSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["appSecret"];
            //ViewBag.Title = "Home Page";            
            //ITopClient client = new DefaultTopClient(severUrl, appKey, appSecret, "json");
            //TbkItemGetRequest req = new TbkItemGetRequest();
            //req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url";
            //req.Cat = id;
            ////req.Q = "女装";

            //TbkItemGetResponse rsp = client.Execute(req);


            //List<NTbkItem> tbkitems = rsp.Results;
            //  Console.WriteLine(rsp.Body);
            // ViewData["NTbkItems"] = tbkitems;
            //ViewBag.strs = tbkitem;
            WebClient webClient = new WebClient();
            if (id == null) { id = "0"; }
            if (subColumn == null) { subColumn = ""; } else { subColumn = "&subColumn=" + subColumn; }
            string HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("https://headline.taobao.com/feed/feedQuery.do?columnId=" + id + "" + subColumn));

            // tring jsonText = "{\"beijing\":{\"zone\":\"海淀\",\"zone_en\":\"haidian\"}}";
            JObject jo = (JObject)JsonConvert.DeserializeObject(HtmlString);

            JArray ja = (JArray)JsonConvert.DeserializeObject(jo["data"].ToString());

            List<Toutiao> toutiaos = new List<Toutiao>();
            foreach (var j in ja)
            {
                j["picList"] = j["picList"][0].ToString();
                Toutiao p = help.ParseFromJson<Toutiao>(j.ToString());
                p.columnId = Convert.ToInt32(id);
                if (p.detailUrl.IndexOf("uz.taobao") > 0)
                {
                    string feedid = help.GetValueAnd("/detail/", "/", p.detailUrl);
                    p.feedId = feedid;
                }
                Toutiao toutiao = _db.Toutiao.FirstOrDefault(t => t.feedId == p.feedId);
                if (toutiao== null)
                {
                    // TryUpdateModel(p);
                    p.isDouble11 = false;
                    _db.Toutiao.Add(p);
                }
                else {
                    if ((toutiao.columnId == 0) && (id != "0"))
                    {
                        toutiao.columnId = Convert.ToInt32(id);
                        TryUpdateModel(toutiao, new string[] { "columnId"});
                        if (ModelState.IsValid)
                        {
                            _db.Entry(toutiao).State = EntityState.Modified;                            
                        }
                    }                 
                
                }
                //列表入库
                try { _db.SaveChanges(); }
                catch (Exception e) { string ss = e.Message; }
               
                toutiaos.Add(p);
            }

            ViewData["JArray"] = toutiaos;
            ViewData["columnId"] = id;
            ViewData["subColumn"] = subColumn;
            return View();
        }
        [CompressAttribute]
        [OutputCache(CacheProfile = "Aggressive")]
        public ActionResult ip(string id, string publishId, string spm, string columnId)
        {
            ViewData["ID"] = id;
            ViewData["publishId"] = publishId;
            ViewData["JArray"] = new JArray();
            if (columnId == null) { columnId = "0"; }
            if (columnId == "") { columnId = "0"; }
            Toutiao toutiao= _db.Toutiao.FirstOrDefault(m => m.feedId == id);
            Toutiao toutiaoToAdd = new Toutiao();  
            // WebRequest webRequest = WebRequest.Create("http://uz.taobao.com/detail/5555795696/");
            // NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webRequest.GetResponse().GetResponseStream(), "gbk");
            if (publishId != null)
            {
               
                WebClient webClient = new WebClient();
                string HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("https://headline.taobao.com/feed/feedQuery.do?columnId=" + columnId + "&publishId=" + publishId + ""));

                // tring jsonText = "{\"beijing\":{\"zone\":\"海淀\",\"zone_en\":\"haidian\"}}";
                JObject jo = (JObject)JsonConvert.DeserializeObject(HtmlString);

                JArray ja = (JArray)JsonConvert.DeserializeObject(jo["data"].ToString());

                List<Toutiao> toutiaos = new List<Toutiao>();
                Toutiao p = new Toutiao();
                foreach (var j in ja)
                {
                   j["picList"]=j["picList"][0].ToString();
                    p = help.ParseFromJson<Toutiao>(j.ToString());
                    //列表入库
                    p.columnId = Convert.ToInt32(columnId);
                    if (p.detailUrl.IndexOf("uz.taobao") > 0)
                    {
                        string feedid = help.GetValueAnd("/detail/", "/", p.detailUrl);
                        p.feedId = feedid;
                    }
                     Toutiao toutiaolist = _db.Toutiao.FirstOrDefault(t => t.feedId == p.feedId);
                     if (toutiaolist == null)
                    {
                        // TryUpdateModel(p);
                        p.isDouble11 = false;
                        _db.Toutiao.Add(p);
                    }
                    else
                    {
                        if ((toutiaolist.columnId == 0) && (columnId != "0"))
                        {
                            toutiaolist.columnId = Convert.ToInt32(id);
                            TryUpdateModel(toutiaolist, new string[] { "columnId" });
                            if (ModelState.IsValid)
                            {
                                _db.Entry(toutiaolist).State = EntityState.Modified;
                            }
                        }

                    }

                    try { _db.SaveChanges(); }
                    catch (Exception e) { string ss = e.Message; }
                    toutiaos.Add(p);
                   
                }

                ViewData["toutiaos"] = toutiaos;                
            }
            //读数据库信息出来
            if (toutiao != null )
            {
                if (toutiao.content != null)
                {
                    // ViewData["bodys"] = toutiao.content;
                    // ViewData["time"] = toutiao.showDate.ToString();
                    ViewBag.Title = toutiao.name;
                    //  ViewData["columnid"] = toutiao.columnId;
                    ViewData.Model = toutiao;
                    toutiao.viewNum += 1; //y访问量加1
                    TryUpdateModel(toutiao, new string[] { "viewNum" });
                    if (ModelState.IsValid)
                    {
                        _db.Entry(toutiao).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                    return View();
                }
            }

            if (spm == null)
            {
                if (id == null) { id = "454759"; }  
               
                    //数据库没有就插入
                    WebClient webClient = new WebClient();
                    String HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("https://headline.taobao.com/feed/feedDetail.htm?id=" + id + "&columnId=0"));
                    Document doc = NSoup.NSoupClient.Parse(HtmlString);
                    Element bodys = doc.Select("div.detail").First();                    
                    //存入数据库 
                    toutiaoToAdd.columnId = Convert.ToInt32(columnId);
                    toutiaoToAdd.name = doc.Title.Replace("淘宝", "网购VIP");
                    toutiaoToAdd.showDate = doc.Select("div.detail-time").Html();
                    toutiaoToAdd.feedId = id;
                    ViewData["time"] = doc.Select("div.detail-time").Html();
                    //删除title 时间 底部连接再入库
                    Elements items = bodys.Select("div.link,.detail-title,.detail-time");
                    items.Remove();
                    toutiaoToAdd.content = bodys.Html();
                    toutiaoToAdd.publishId = publishId;

                    if (toutiao == null)
                    {
                        //TryUpdateModel(toutiaoToAdd);
                        //if (ModelState.IsValid)
                        //{
                            _db.Toutiao.Add(toutiaoToAdd);
                            _db.SaveChanges();
                        //}
                    }
                    else
                    {
                        toutiao.content = toutiaoToAdd.content;
                        if (columnId != "0")
                        {
                            toutiao.columnId = toutiaoToAdd.columnId;
                        }
                        toutiao.subColumn = toutiaoToAdd.subColumn;
                        TryUpdateModel(toutiao, new string[] { "subColumn", "content", "columnId" });
                        if (ModelState.IsValid)
                        {
                            _db.Entry(toutiao).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                    //删除淘宝连接
                    items = bodys.Select("div.item");
                    items.Remove();
                    // items = bodys.Select("img");
                    // items.Remove();
                    items = bodys.Select("a");
                    items.Remove();
                    ViewBag.Title = toutiaoToAdd.name;
                    toutiaoToAdd.content = bodys.Html();
                    

                   // ViewBag.Title = doc.Title.Replace("淘宝", "网购VIP");
              
               
                
            }
            else
            {
                if (id == null) { id = "5556414503"; }
                WebClient webClient = new WebClient();
                String HtmlString = Encoding.GetEncoding("gbk").GetString(webClient.DownloadData("http://uz.taobao.com/detail/" + id + "/?spm=a21a0.7476711.0.0.19PZMI"));
                Document doc = NSoup.NSoupClient.Parse(HtmlString);
                Element bodys = doc.Select("div.site-main-content").First();

                toutiaoToAdd.columnId = Convert.ToInt32(columnId);
                toutiaoToAdd.name = doc.Title.Replace("淘宝", "网购VIP");
                toutiaoToAdd.showDate = doc.Select("span.pub-time").Html();
                toutiaoToAdd.feedId = id;
                ViewData["time"] = doc.Select("div.detail-time").Html();
                //删除title 时间 底部连接再入库
               
                toutiaoToAdd.content = bodys.Html();
                toutiaoToAdd.publishId = publishId;

                if (toutiao == null)
                {

                    //TryUpdateModel(toutiaoToAdd);
                    //if (ModelState.IsValid)
                    //{
                    
                        _db.Toutiao.Add(toutiaoToAdd);
                        _db.SaveChanges();
                    //}
                }
                else {
                    //更新文章
                    toutiao.content= toutiaoToAdd.content;
                    if (columnId != "0")
                    {
                        toutiao.columnId = toutiaoToAdd.columnId;
                    }
                    toutiao.subColumn = toutiaoToAdd.subColumn;
                    TryUpdateModel(toutiao, new string[] { "columnId", "content", "subColumn" });
                    if (ModelState.IsValid)
                    {
                        _db.Entry(toutiao).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                    
                }
                Elements items = bodys.Select("div.item");
                items.Remove();
               // items = bodys.Select("img");
               // items.Remove();
                //foreach (Element item in items)
                //{
                //    item.Attr("abs:src");
                //    //item.TagName("img");

                //}
                // NSoup.Nodes.Document doc = NSoup.NSoupClient.Connect("http://uz.taobao.com/detail/5555795696/").UserAgent("Mozilla").Header("charset", "gbk").Get();
                // NSoup.Nodes.Element masthead = doc.ToString();
                ViewBag.Title = toutiaoToAdd.name;
                toutiaoToAdd.content = bodys.Html();              
                
            }
            ViewData.Model = toutiaoToAdd;
            return View();



        }
        
       
         

	}
}