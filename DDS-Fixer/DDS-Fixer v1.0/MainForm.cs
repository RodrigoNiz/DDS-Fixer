using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DDS_Fixer_v1._0
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Instance of DDS_Fixer object that do all the work.
        /// </summary>
        private DDS_Fixer fixer;

        /// <summary>
        /// Delegate to refresh UI info from another thread.
        /// </summary>
        /// <param name="info">Information to be included in the UI</param>
        public delegate void RefreshInfoDelegate(string info);

        /// <summary>
        /// Delegate to execute funcions from another thread call.
        /// </summary>
        public delegate void DoFunctionDelegate();
        
        public MainForm()
        {
            InitializeComponent();
            fixer = new DDS_Fixer();
            
            fixer.Analyzed += OnFileFixed;
            fixer.Finished += OnFinished;
            fixer.DeniedAccess += OnAccessDenied;
            
        }

        /// <summary>
        /// Starts fixing routine.
        /// </summary>
        private void Start()
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Select the folder containing .dds files to fix. Subfolders will be selected automatically.";
                folderBrowser.ShowNewFolderButton = false;

                if (DialogResult.OK == folderBrowser.ShowDialog() && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                {
                    string message = string.Empty;

                    Application.UseWaitCursor = true;
                    Application.DoEvents();

                    int foundFiles = fixer.CountFiles(folderBrowser.SelectedPath);
                    int foundSubdirectories = fixer.CountSubDirectories(folderBrowser.SelectedPath);

                    Application.UseWaitCursor = false;

                    if (foundSubdirectories == 0)
                    {
                        message = $"Found {foundFiles} files.";
                    }
                    else
                    {
                        message = $"Found {foundFiles} files inside {foundSubdirectories} subdirectories.";
                    }
                    if(foundFiles == 0 )
                    {
                        message += " Proceed just for fun?";
                    }
                    else
                    {
                        if (foundFiles > 10000)
                        {
                            message += " This might take several minutes to complete. Proceed?";
                        }
                        else
                        {
                            message += " This will take a couple of seconds. Proceed?";
                        }
                    }

                    if (DialogResult.Yes == MessageBox.Show(message, "Start", MessageBoxButtons.YesNo))
                    {
                        progressBar.Minimum = 0;
                        progressBar.Maximum = foundFiles;
                        progressBar.Value = 0;
                        
                        buttonCancel.Show();
                        buttonStart.Hide();

                        richTextBoxFiles.Clear();

                        fixer.Start(folderBrowser.SelectedPath);
                    }
                }
            }
        }

        /// <summary>
        /// Aborts fixing progress.
        /// </summary>
        private void Cancel()
        {
            if (DialogResult.Yes == MessageBox.Show("This will abort the fixing progress. Proceed?", "Cancel", MessageBoxButtons.YesNo))
            {
                fixer.Cancel();
            }
        }

        /// <summary>
        /// Do final tasks, after fixing is done.
        /// </summary>
        private void Finish()
        {
            progressBar.Value = 0;
            ShowTaskBarProgress();

            buttonCancel.Hide();
            buttonStart.Show();

            System.Media.SystemSounds.Beep.Play();

            if (DialogResult.Yes == MessageBox.Show($"{fixer.FilesAnalyzed} files analyzed. {fixer.FilesFixed} files fixed. Save fix log?", "Finished", MessageBoxButtons.YesNo))
            {
                SaveLog();
            }
        }
        
        /// <summary>
        /// Saves fixing log into a file.
        /// </summary>
        private void SaveLog()
        {
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                FileName = $"DDS-Fixer_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm")}",
                Filter = "Text|*.txt|All|*.*"
            };

            if (DialogResult.OK == saveFile.ShowDialog())
            {
                fixer.SaveLog(saveFile.FileName);
            }
        }

        /// <summary>
        /// Refreshes progress info. 
        /// </summary>
        /// <param name="newFile">New file name to include in stack.</param>
        private void RefreshInfo(string newFile)
        {
            if (String.IsNullOrEmpty(richTextBoxFiles.Text))
            {
                richTextBoxFiles.AppendText(newFile);
            }
            else
            {
                richTextBoxFiles.AppendText(Environment.NewLine + newFile);
            }

            richTextBoxFiles.ScrollToCaret();

            progressBar.Value = fixer.FilesAnalyzed;

            ShowTaskBarProgress();
            
            labelProgress.Text = $"{fixer.FilesAnalyzed} files analyzed. {fixer.FilesFixed} files fixed.";
            Application.DoEvents();
        }


        /// <summary>
        /// If feature is supported, shows progress in taskbar icon.
        /// </summary>
        private void ShowTaskBarProgress()
        {
            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                var taskbarInstance = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                
                if (progressBar.Value != 0)
                {
                    taskbarInstance.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
                    taskbarInstance.SetProgressValue(progressBar.Value, progressBar.Maximum);
                }
                else
                {
                    taskbarInstance.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.NoProgress);
                }
            }
        }
        
        /// <summary>
        /// Informs about acess denied and asks user to restart as admin.
        /// </summary>
        private void AccessDeniedMessage()
        {
            if (DialogResult.Yes == MessageBox.Show("Denied access to modify files in this folder. Restart DDS-Fixer with elevated privileges?", "Denied Access", MessageBoxButtons.YesNo))
            {
                RestartAsAdmin();
            }
        }

        /// <summary>
        /// Creates a new process with raised privileges and exits the current application.
        /// </summary>
        private void RestartAsAdmin()
        {
            var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(exeName);
            startInfo.Verb = "runas";
            System.Diagnostics.Process.Start(startInfo);
            Close();
        }

        /// <summary>
        /// Event raised whenever the fixing function ins't alowed to write into a file.
        /// </summary>
        /// <param name="sender">DDS_Fixer object</param>
        /// <param name="e"></param>
        private void OnAccessDenied(object sender, EventArgs e)
        {
            Invoke(new DoFunctionDelegate(this.AccessDeniedMessage), null);
        }

        /// <summary>
        /// Event raised whenever a file is analyzed.
        /// </summary>
        /// <param name="sender">DDS_Fixer object.</param>
        /// <param name="filePath">Last analyzed file.</param>
        private void OnFileFixed(object sender, string filePath)
        {
            Invoke(new RefreshInfoDelegate(this.RefreshInfo), new object[] { filePath });
        }


        /// <summary>
        /// Event raised whenever the fixing function is done.
        /// </summary>
        /// <param name="sender">DDS_Fixer object.</param>
        private void OnFinished(object sender, EventArgs e)
        {
            Invoke(new DoFunctionDelegate(this.Finish), null);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            buttonCancel.Hide();
            buttonStart.Show();

            toolTip.SetToolTip(buttonStart, "Start to fix .dss files");
            toolTip.SetToolTip(buttonCancel, "Abort file fixing");
            toolTip.SetToolTip(progressBar, "Shows progress");
            toolTip.SetToolTip(richTextBoxFiles, "Analyzed files appear here");
            toolTip.SetToolTip(linkLabelAbout, "About DSS-Fixer");
            toolTip.SetToolTip(labelProgress, "Progress info");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void linkLabelAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AboutForm about = new AboutForm();
            about.Show();
        }
    }
}
