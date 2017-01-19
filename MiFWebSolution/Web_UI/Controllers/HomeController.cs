﻿using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer;
using Web_UI.Models;
using System.Reflection;
using System.Text;
using Web_UI.Infrastructure;

namespace Web_UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new DbMifEF())
            {
                SongsModel model = new SongsModel();
                model.Songs = context.Songs.ToList();
                WriteToLog();
                return View(model);
            }
            
        }

        void WriteToLog()
        {
            string logMessage = "";
            if (Session["userName"] != null)
            {
                logMessage = Session["userName"].ToString();
            }
            else
            {
                logMessage = "?";
            }
            logMessage += " +";
            Logger.AddMessageToLog(logMessage);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult GetSong(int id = 0)
        {
            using (var context = new DbMifEF())
            {
                //string path = null;
                // MvcHtmlString text = new MvcHtmlString("");
                //List<MvcHtmlString> listStr = new List<MvcHtmlString>();
                //List<string> listStr = new List<string>();
                TextOrAccordLineList model = new TextOrAccordLineList();
                model.List = new List<TextOrAccord>();
                //List<TextOrAccord> model = new List<TextOrAccord>();
                Song song = null;
                
                if (id != 0)
                {
                    song = context.Songs.Where(s => s.SongID == id).First();
                    //str = String.Format("ID песни - {0}", id);    
                    if (song != null)
                    {
                        //var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                        //Path.GetFullPath(song.PathToText);
                        string path = Path.Combine(Server.MapPath(song.PathToText));
                        path = path.Replace("\\Home", "");
                        if (System.IO.File.Exists(path))
                        {
                           // text = System.IO.File.ReadAllText(path, Encoding.UTF8);
                            const int BufferSize = 128;
                            using (var fileStream = System.IO.File.OpenRead(path))
                            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                            {
                                string line;
                                
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    TextOrAccord textOrAcc = new TextOrAccord();

                                    if (line.Length > 0)
                                    { 
                                        if (line[0] == '@')
                                        {
                                            line = line.Remove(0, 1);
                                            textOrAcc.IsText = false;
                                        }
                                        else
                                        {
                                            textOrAcc.IsText = true;
                                        }                                        
                                    }
                                    textOrAcc.Str = line;
                                    model.List.Add(textOrAcc);
                                    //listStr.Add(line);
                                }
                                //text = MvcHtmlString.Create(sb.ToString());
                               //text = sb.ToString();
                            }
                        }
                        // Process line
                    }
                    
                }                
                //ViewBag.SongText = listStr;
                return PartialView(model);
            }
        }
    }
}