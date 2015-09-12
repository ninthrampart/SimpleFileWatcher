using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleFileWatcher
{
    public partial class Form1 : Form
    {

        //List<string> files;
        List<WatchableFile> wfiles;
        List<DirRating> ratings;

        // This delegate enables asynchronous calls for setting 
        // the text property on a TextBox control. 
        delegate void SetTextCallback(string text);

        // This thread is used to demonstrate both thread-safe and 
        // unsafe ways to call a Windows Forms control. 
        private Thread demoThread = null;

        // This BackgroundWorker is used to demonstrate the  
        // preferred way of performing asynchronous operations. 
        private BackgroundWorker backgroundWorker1;

        public Form1()
        {
            InitializeComponent();
            //updateTextBox = new updateTextBoxDelegate(updateTextBox1);
            //files = new List<string>();
            wfiles = new List<WatchableFile>();
            ratings = new List<DirRating>();
        }

        private void ThreadProcSafe(string msg)
        {
            this.SetText(msg);
        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.tbLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tbLog.Text += text;
            }
        }

        private void btnWatch_Click(object sender, EventArgs e)
        {
            tbLog.Text = "Start watching" + Environment.NewLine;

            List<string> directoriesToWatch = new List<string>();
            directoriesToWatch.Add(@"D:\MyApps\W1");
            DirRating r = new DirRating(@"D:\MyApps\W1", 0);
            ratings.Add(r);

            directoriesToWatch.Add(@"D:\MyApps\W2");
            r = new DirRating(@"D:\MyApps\W2", 1);
            ratings.Add(r);

            foreach (string dir in directoriesToWatch)
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                //watcher.Path = @"D:\MyApps\W1";
                watcher.Path = dir;
                watcher.NotifyFilter = NotifyFilters.FileName;

                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {

            string txt = string.Format("File: {0} renamed to {1}", e.OldFullPath, e.FullPath) + Environment.NewLine;

            this.demoThread =
                new Thread(() => this.ThreadProcSafe(txt));

            this.demoThread.Start();

        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {

            string txt = "File: " + e.FullPath + " " + e.ChangeType + Environment.NewLine;

            this.demoThread =
                new Thread(() => this.ThreadProcSafe(txt));

            this.demoThread.Start();

            int lInd = e.FullPath.LastIndexOf("\\");
            string tmp = e.FullPath.Substring(0, lInd);

            var r = ratings.Where(x => x.Dir == tmp);
            int i = r.Count();
            string s = "";

            if(r.Any())
            {
                DirRating current = r.First();

                WatchableFile f = new WatchableFile(e.FullPath, current.Rating);
                wfiles.Add(f);

            }

            //WatchableFile f = new WatchableFile(e.FullPath,
            //    )

            //files.Add(e.FullPath);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //int i = files.Count();
            int i = wfiles.Count();

            wfiles = wfiles.OrderBy(x => x.Priority).ToList();

            int j = ratings.Count();
        }
    }
}
