using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Permissions;
using System.Security;

namespace DDS_Fixer_v1._0
{
    class DDS_Fixer
    {
        /* If _DDSFileSignature and _DDSFileExtention are changed to appropriated values
         * this class can be useful to fix another type of files.*/

        /// <summary>
        /// DirectDraw Surface file digital signature ("Magic number").
        /// </summary>
        private byte[] _DDSFileSignature = new byte[4] { 0x44, 0x44, 0x53, 0x20, };

        /// <summary>
        /// DirectDraw Surface file extension.
        /// </summary>
        private string _DDSFileExtention = "dds";

        /// <summary>
        /// Background Thread that runs the analyzis
        /// </summary>
        private Thread workerThread;

        /// <summary>
        /// Stores the name of every modified file.
        /// </summary>
        private StringBuilder filesFixedStack;

        /// <summary>
        /// Total number of .dss files to analyze in a directory and its subdirectories.
        /// </summary>
        private int filesToAnalyze;

        /// <summary>
        /// Number of files already analyzed by the fixing function.
        /// </summary>
        private int filesAnalyzed;

        /// <summary>
        /// Number of files modified by the fixing function.
        /// </summary>
        private int filesFixed;

        /// <summary>
        /// Determines if the fixing function should keep running or stop.
        /// </summary>
        private bool keepGoing;

        /// <summary>
        /// Total number of .dss files to analyze in a directory and its subdirectories.
        /// </summary>
        public int FilesToAnalyze
        {
            get { return filesToAnalyze; }
        }

        /// <summary>
        /// Number of files already analyzed by the fixing function.
        /// </summary>
        public int FilesAnalyzed
        {
            get { return filesAnalyzed; }
        }

        /// <summary>
        /// Number of files modified by the fixing function.
        /// </summary>
        public int FilesFixed
        {
            get { return filesFixed; }
        }

        /// <summary>
        /// Determines if the fixing function should keep running or stop.
        /// </summary>
        public bool KeepGoing
        {
            get { return keepGoing; }
            set { keepGoing = value; }
        }

        /// <summary>
        /// Event raised whenever the fixing function has analyzed a file.
        /// </summary>
        /// <param name="filePath">The name of the last file analyzed.</param>
        public event EventHandler<string> Analyzed;

        /// <summary>
        /// Event raised whenever the fixing function has finish its work.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Event raised if the fixing funcion isn't alowed to write into a file.
        /// </summary>
        public event EventHandler DeniedAccess;

        public DDS_Fixer()
        {
            filesFixed = 0;
            filesAnalyzed = 0;
            filesToAnalyze = 0;

            keepGoing = true;

            filesFixedStack = new StringBuilder();
        }

        /// <summary>
        /// Event raised whenever the fixing function has finish its work.
        /// </summary>
        protected virtual void OnFinished()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event raised whenever the fixing function has analyzed a file.
        /// </summary>
        /// <param name="filePath">The name of the last file analyzed.</param>
        protected virtual void OnAnalyzed(string filePath)
        {
            Analyzed?.Invoke(this, filePath);
        }

        /// <summary>
        /// Event raised if the fixing funcion isn't alowed to write into a file.
        /// </summary>
        protected virtual void OnDeniedAccess()
        {
            DeniedAccess?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Returns the number of .dds files in a directory and its subdirectories.
        /// </summary>
        /// <param name="directoryPath">Starter path for the search.</param>
        /// <returns></returns>
        public int CountFiles(string directoryPath)
        {
            int fileCounter = 0;

            try
            {
                foreach (string file in Directory.GetFiles(directoryPath, $"*.{_DDSFileExtention}"))
                {
                    fileCounter++;
                }
                foreach (string directory in Directory.GetDirectories(directoryPath))
                {
                    fileCounter += CountFiles(directory);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return fileCounter;
            }

            catch (Exception e)
            {
                throw new Exception(String.Format("An error ocurred while searching for files", e.Message), e);
            }

            return fileCounter;
        }

        /// <summary>
        /// Returns the number of subdirectories inside selected directory.
        /// </summary>
        /// <param name="directoryPath">Starter path for the search.</param>
        /// <returns></returns>
        public int CountSubDirectories(string directoryPath)
        {
            int directoryCounter = 0;

            try
            {
                foreach (string directory in Directory.GetDirectories(directoryPath))
                {
                    directoryCounter++;
                    directoryCounter += CountSubDirectories(directory);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return directoryCounter;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("An error ocurred while searching for directories", e.Message), e);
            }

            return directoryCounter;
        }


        /// <summary>
        /// For each .dds files in a directory and its subdirectories, analyses the
        /// file signature and fixes it if needed.
        /// </summary>
        /// <param name="directoryPath">Starter directory path</param>
        private void FixSignature(string directoryPath)
        {
            foreach (string filePath in Directory.GetFiles(directoryPath, $"*.{_DDSFileExtention}"))
            {
                if (keepGoing)
                {
                    try
                    {
                        using (Stream file = File.Open(filePath, FileMode.Open))
                        {
                            byte[] buffer = new byte[_DDSFileSignature.Length];

                            file.Position = 0;
                            file.Read(buffer, 0, _DDSFileSignature.Length);

                            if (!buffer.SequenceEqual(_DDSFileSignature))
                            {
                                file.Position = 0;
                                file.Write(_DDSFileSignature, 0, 4);

                                filesFixedStack.AppendLine(filePath);
                                filesFixed++;
                            }

                            OnAnalyzed(filePath);
                            filesAnalyzed++;
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        OnDeniedAccess();
                        keepGoing = false;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("An error ocurred while executing the data import", e.Message), e);
                    }

                }
                else break;
            }

            foreach (string directory in Directory.GetDirectories(directoryPath))
            {
                if (keepGoing)
                {
                    FixSignature(directory);
                }
                else break;
            }
        }

        /// <summary>
        /// Starts the fixing funcion in a background worker thread.
        /// </summary>
        /// <param name="directoryPath">Starter directory path.</param>
        public void Start(string directoryPath)
        {
            keepGoing = true;
            filesToAnalyze = CountFiles(directoryPath);

            workerThread = new Thread(() => { FixSignature(directoryPath); OnFinished(); })
            {
                IsBackground = true
            };
            workerThread.Start();
        }

        /// <summary>
        /// Aborts the fixing function's progress.
        /// </summary>
        public void Cancel()
        {
            keepGoing = false;
        }

        /// <summary>
        /// Saves a fixing log into a file.
        /// </summary>
        /// <param name="fileName">The name of the file to save.</param>
        public void SaveLog(string fileName)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    StringBuilder Header = new StringBuilder();
                    Header.AppendLine("DDS-Fixer v1.0");
                    Header.AppendLine($"Log Date: {DateTime.Now}");
                    Header.AppendLine($"Files Analyzed: {filesAnalyzed}");
                    Header.AppendLine($"Fixed Files: {FilesFixed}");
                    Header.AppendLine("------------------------------------------");

                    file.Write(Header.ToString() + filesFixedStack.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data export: {0}", e.Message), e);
            }
        }
    }

}
