using HtmlAgilityPack;
using System.Threading;
using System.Collections.Generic;
using System.Net;

namespace OP.GG_Scrap
{
    public static class Scraper
    {
        private static Form1 mainForm = null;
        public static string locale = "euw";
        public static List<string> summonerNames = new List<string>();
        public static List<string> scannedNames = new List<string>();
        public static List<string> queueNames = new List<string>();
        public static List<string> GetSummoners()
        {
            return summonerNames;
        }
        public static void ChangeLocale(string state)
        {
            locale = state.ToLower();
        }
        public static void SetForm(Form1 frm)
        {
            if (mainForm == null)
            {
                mainForm = frm;
            }
        }
        public static void Master()
        {
            while (true)
            {
                if (queueNames.Count != 0)
                {
                    for (int i = 0; i<queueNames.Count;i++)
                    {
                        string tmpName = queueNames[i];
                        while (PipeFull())
                        {

                        }
                        Core.RunThread(LoadProfile, tmpName);
                        Thread.Sleep(100);
                    }
                }

            }
        }
        public static bool PipeFull()
        {
            return mainForm.maxThreads == mainForm.threadCount;
        }
        public static bool LoadProfile(string uname)
        {

            try
            {
                if (scannedNames.Contains(uname))
                {
                    return false;
                }
                scannedNames.Add(uname);
                WebClient client = new WebClient()
                {
                    Proxy = null
                };
                string source = client.DownloadString("http://" + locale + ".op.gg/summoner/userName=" + uname);
                HtmlDocument page = new HtmlDocument();
                page.LoadHtml(source);
                foreach (HtmlNode node in page.DocumentNode.SelectNodes("//div[@class='SummonerName']"))
                {
                    string sumName = node.InnerText.TrimStart().TrimEnd();
                    if (!summonerNames.Contains(sumName))
                    {
                        summonerNames.Add(sumName);
                        queueNames.Add(sumName);
                    }
                }
                Invoker.SetLabelText(mainForm.counter_lbl, summonerNames.Count.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
