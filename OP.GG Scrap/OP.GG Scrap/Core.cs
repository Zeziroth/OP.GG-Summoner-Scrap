using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OP.GG_Scrap
{
    public static class Core
    {
        #region Declaration Stuff
        private static Form1 mainForm = null;
        private static Dictionary<double, Thread> threadPool = new Dictionary<double, Thread>();
        private static readonly string NA_OCE = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly string EUW_EUNE = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyzàâÇçèÉéÊêëîïÔôœùûĄąĘęÓóĆćŁłŃńŚśŹźŻżÄäÉéÖöÜüßÁáÉéÍíÑñÓóÚúÜüΑαΒβΓγΔδΕεΖζΗηΘθΙιΚκΛλΜμΝνΞξΟοΠπΡρΣσςΤτΥυΦφΧχΨψΩωΆΈΉΊΌΎΏάέήόίύώΪΫϊϋΰΐĂăÂâÎîȘșŞşȚțŢţÀàÈèÉéÌìÍíÒòÓóÙùÚúÁáĄąÄäÉéĘęĚěÍíÓóÔôÚúŮůÝýČčďťĹĺŇňŔŕŘřŠšŽž";
        private static readonly string BRAZIL = "0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÀÁÂÃÇÉÊÍÓÔÕÚàáâãçéêíóôõú";
        private static readonly string RUSSIA = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private static readonly string TURKEY = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïð 0123456789ABCDEFGĞHIİJKLMNOPQRSŞTUVWXYZabcçdefgğhıijklmnoöpqrsştuvwxyzªµºÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿıŁłŒœŠšŸŽžƒˆˇˉμﬁﬂĄąĘęÓóĆćŁłŃńŚśŹźŻż";
        private static readonly string LATAM = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚÜ abcdefghijklmnñopqrstuvwxyzáéíóúü";
        private static readonly string[] charsets = new string[] { EUW_EUNE, NA_OCE, BRAZIL, RUSSIA, TURKEY, LATAM };
        private static ManualResetEvent syncEvent = new ManualResetEvent(false);
        #endregion
        public static void DummyMethod()
        {
            int ii = 0;
            for (int i = 0; i < int.MaxValue; i++)
            {
                ii++;
            }
        }
        public static void CloseThreads()
        {
            threadPool.Keys.ToList().ForEach(k => threadPool[k].Abort());
            threadPool.Keys.ToList().ForEach(k => threadPool.Remove(k));
            RefreshThreadLabel();
            //for (int i = 0; i<threadPool.Count; i++)
            //{
            //    double key = threadPool.element
            //    threadPool[key].Abort();
            //    threadPool.Remove(key);
                
            //}
        }
        private static void RefreshThreadLabel()
        {
            mainForm.threadCount = threadPool.Count;
            if (mainForm.threadCount == 1 && mainForm.started == true)
            {
                System.Windows.Forms.MessageBox.Show("DONE!");
            }
            Invoker.SetLabelText(mainForm.threadsLabel, "Threads running: " + threadPool.Count.ToString());
        }
        //public static void RunScrapper()
        //{
        //    Scraper.LoadProfile(mainForm.metroTextBox1.Text);
        //}
        public static bool isValidName(string input, int state)
        {
            string charset = "";
            switch (state)
            {
                case 0:
                case 1:
                case 3:
                    charset = charsets[0];
                    break;

                case 2:
                case 4:
                    charset = charsets[1];
                    break;

                case 5:
                    charset = charsets[2];
                    break;

                case 6:
                    charset = charsets[4];
                    break;

                case 7:
                    charset = charsets[3];
                    break;
                case 8:
                case 9:
                    charset = charsets[5];
                    break;


            }
            Scraper.ChangeLocale(mainForm.metroComboBox1.Text);
            if (input.Length <= 16 && input.Trim() != "")
            {
                foreach (char chr in input.ToCharArray())
                {
                    if (!charsets[mainForm.metroComboBox1.SelectedIndex].Contains(chr.ToString()))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public static void Init(Form1 frm)
        {
            if (mainForm == null)
            {
                Scraper.SetForm(frm);
                mainForm = frm;
            }
        }
        public static Thread RunThread(Action methodName)
        {
            ManualResetEvent syncEvent = new ManualResetEvent(false);
            double unixMilli = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            Thread newThread = new Thread(
        () =>
        {
            syncEvent.Set();
            RefreshThreadLabel();
            methodName();
            syncEvent.WaitOne();
            threadPool.Remove(unixMilli);
            RefreshThreadLabel();
        }

    );
            if (threadPool.ContainsKey(unixMilli))
            {
                Thread.Sleep(5);
                unixMilli = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
            threadPool.Add(unixMilli, newThread);

            newThread.Start();
            return newThread;
        }


        public static Thread RunThread(Func<string,bool> methodName, string arg)
        {
            ManualResetEvent syncEvent = new ManualResetEvent(false);
            double unixMilli = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            Thread newThread = new Thread(
        () =>
        {
            syncEvent.Set();
            RefreshThreadLabel();
            methodName(arg);
            syncEvent.WaitOne();
            threadPool.Remove(unixMilli);
            RefreshThreadLabel();
        }

    );
            if (threadPool.ContainsKey(unixMilli))
            {
                Thread.Sleep(5);
                unixMilli = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
            threadPool.Add(unixMilli, newThread);

            newThread.Start();
            return newThread;
        }
    }
}
