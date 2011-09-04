/*	
 * Author: Vincent Tollu 
 * vinzz@altern.org
 * Date: 07/27/2010
 * Time: 01:06
 * 
 * Distributed under the General Public License
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Diagnostics;
using Mp3Sync.Properties;

namespace Mp3Sync
{
    public static class Mp3Synchronizer
    {
        //Add Target2 + mode
        public static void synchronize(DirectoryInfo dSource, Stack<DirectoryInfo> dTargets, DirectoryInfo dBinTarget, string stExtensions)
        {
            try
            {
                long iCountSnip = 0;

                Dictionary<string, FileInfo> srcMap = new Dictionary<string, FileInfo>();
                long srcCapacity = 0;

                Console.WriteLine(Resources.Step1);
                Console.WriteLine();    

                srcCapacity += SearchForInterestFiles(dSource, srcMap, stExtensions);
                Console.WriteLine(String.Format(Resources.srcCount,srcMap.Count));
                Console.WriteLine(); 

                Dictionary<string, FileInfo> dstMap = new Dictionary<string, FileInfo>();

                long destCapacity = 0;
                foreach (DirectoryInfo target in dTargets)
                {
                    DriveInfo dTargetInfo = new DriveInfo(target.FullName.Substring(0, 1));
                    destCapacity += dTargetInfo.AvailableFreeSpace;

                    destCapacity += SearchForInterestFiles(target, dstMap, stExtensions);

                    destCapacity -= 10000000; //10Mo kept free per unit
                }
                Console.WriteLine(String.Format(Resources.dstCount, dstMap.Count));
                Console.WriteLine(); 
                Console.WriteLine(String.Format(Resources.srcWeight,srcCapacity / 1000000));
                Console.WriteLine(String.Format(Resources.dstCapacity,destCapacity / 1000000));
                Console.WriteLine(); 
                long free = destCapacity - srcCapacity;

                Console.WriteLine(String.Format(Resources.freeSpace,((free>0)?"+":""),free / 1000000));

                if (free < 0)
                    Console.WriteLine(Resources.warnLackPlace);

                Console.WriteLine(Resources.Step2);
                if (dBinTarget != null)
                    Console.WriteLine(String.Format(Resources.extraFilesDest,dBinTarget.FullName));
                else
                    Console.WriteLine(Resources.extradel);

                //Snip extra dest files
                foreach (KeyValuePair<string, FileInfo> kvp in dstMap)
                {
                    if (!srcMap.ContainsKey(kvp.Key))
                    {
                        if (dBinTarget != null)
                        {
                            string destName = Path.Combine(dBinTarget.FullName, kvp.Value.Directory.Name);

                            if (!Directory.Exists(destName))
                                Directory.CreateDirectory(destName);

                            destName = Path.Combine(destName, kvp.Value.Name);
                            if (!File.Exists(destName))
                                kvp.Value.MoveTo(destName);
                            else
                                kvp.Value.Delete();
                        }
                        else
                        {
                            try
                            {
                                kvp.Value.Delete();
                            }
                            catch (Exception e)
                            {
                                throw new Exception(String.Format(Resources.errorNoDelete,kvp.Value.FullName,e.ToString()));
                            }
                        }
                        ++iCountSnip;
                    }
                }
                Console.WriteLine(String.Format(Resources.removedFiles,iCountSnip));

                //Snip empty directories
                foreach (DirectoryInfo target in dTargets)
                    SnipVoidDir(target);

                Console.WriteLine(Resources.Step3);

                //Add new files
                long count = 0;
                foreach (DirectoryInfo target in dTargets)
                    count += AddInDest(target, srcMap, dstMap);

                Console.WriteLine(String.Format(Resources.copiedFiles, count));

                if (srcMap.Count > 0)
                {
                    Console.WriteLine(Resources.errorNoPlace);

                    foreach (KeyValuePair<string, FileInfo> kvp in srcMap)
                    {
                        Console.WriteLine(Path.Combine(Path.GetDirectoryName(kvp.Value.FullName),kvp.Value.Name));
                    }
                }
                else
                    Console.WriteLine(Resources.synchroOK);

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void SnipVoidDir(DirectoryInfo dTarget)
        {
            try
            {
                System.Collections.Stack stackDirs = new System.Collections.Stack();

                //Sub-subdirs
                SnipVoidRound(dTarget, stackDirs);

                //SubDirs
                SnipVoidRound(dTarget, stackDirs);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private static void SnipVoidRound(DirectoryInfo dTarget, System.Collections.Stack stackDirs)
        {
            DirectoryInfo currentDir = null;
            try
            {
                foreach (DirectoryInfo diNext in dTarget.GetDirectories())
                    stackDirs.Push(diNext);

                while (stackDirs.Count > 0)
                {
                    currentDir = (DirectoryInfo)stackDirs.Pop();

                    //Process dir
                    if (currentDir.GetFiles().Length == 0)
                    {
                        if (currentDir.GetDirectories().Length == 0)
                            currentDir.Delete();
                        else
                            //Process Subdirectories
                            foreach (DirectoryInfo diNext in currentDir.GetDirectories())
                                stackDirs.Push(diNext);
                    }
                }
            }
            catch (Exception e)
            {
                if (currentDir != null)
                    throw new Exception(String.Format(Resources.errorNoDelete, currentDir.FullName, e.ToString()));
                else
                    throw;
            }
        }

        private static int AddInDest(DirectoryInfo dTarget, Dictionary<string, FileInfo> srcMap, Dictionary<string, FileInfo> dstMap)
        {
            try
            {
                Console.WriteLine('\n');
                DriveInfo dTargetInfo = new DriveInfo(dTarget.FullName.Substring(0, 1));
                Stack<String> stRemove = new Stack<string>();
                int count = 0;
                int schonIndex = 0;
                string currDir;

                List<string> Keys = new List<string>();
                List<FileInfo> Values = new List<FileInfo>();

                foreach (KeyValuePair<string, FileInfo> kvp in srcMap)
                {
                    Keys.Add(kvp.Key);
                    Values.Add(kvp.Value);
                }

                bool Warn = false;

                int currTop = Console.CursorTop;
                int longest = 0;
                for(int i = 0; i < srcMap.Count; ++i)
                {
                    string Key = Keys[i];
                    FileInfo Value = Values[i];

                    if (!dstMap.ContainsKey(Key))
                    {
                        currDir = Value.Directory.FullName;

                        if (Value.Name.Length > longest) longest = Value.Name.Length;
                        string fill = string.Empty;
                        for (int l = 0; l < (longest - Value.Name.Length); ++l)
                            fill += " ";
                        longest = Value.Name.Length;

                        //Keep 10Mo free
                        if (dTargetInfo.AvailableFreeSpace > ((Value.Length + 10000000)))
                        {
                                string destName = Path.Combine(dTarget.FullName, Value.Directory.Name);

                                if (!Directory.Exists(destName))
                                    Directory.CreateDirectory(destName);

                                destName = Path.Combine(destName, Value.Name);

                                ++schonIndex;

                                Console.SetCursorPosition(0, currTop);
                                Console.Write(String.Format(Resources.copyFile,_stSchon[schonIndex % _stSchon.Length].ToString(),Value.Name,fill));  
                                
                                Value.CopyTo(destName);
                                ++count;

                                stRemove.Push(Key);
                        }
                        else
                        {
                            Console.SetCursorPosition(0, currTop);
                            if (!Warn)
                            {
                                Console.Write(String.Format(Resources.destFull, dTarget.FullName, fill));
                                Warn = true;
                            }
                        }
                    }
                    else
                        stRemove.Push(Key); // File already present
                }

                foreach (string st in stRemove)
                    srcMap.Remove(st);

                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static long SearchForInterestFiles(DirectoryInfo dSrcDir, Dictionary<string, FileInfo> src, string stExtensions)
        {
            try
            {
                DateTime cacheDate = DateTime.MinValue;

                Dictionary<string, string> srcCache = new Dictionary<string, string>();

                System.Collections.Stack stackDirs = new System.Collections.Stack();

                string stCachePath = Path.Combine(Path.GetTempPath(),dSrcDir.FullName.Replace('\\', '_').Replace(':','_'));

                int cursorLeft = 0;
                if (File.Exists(stCachePath))
                {
                    Console.Write(String.Format(Resources.readCache, dSrcDir.FullName) + " ");
                    cursorLeft = Console.CursorLeft;

                    FileInfo f = new FileInfo(stCachePath);

                    StreamReader inputlist = File.OpenText(stCachePath);

                    string line = null;

                    while ((line = inputlist.ReadLine()) != null)
                    {
                        if (line.Contains("|"))
                        {
                            string[] split = line.Split('|');
                            srcCache.Add(split[1], split[0]);
                        }
                    }

                    inputlist.Close();

                    if (srcCache.Count > 0)
                        cacheDate = f.LastWriteTimeUtc;
                }
                else
                    Console.WriteLine(String.Format(Resources.warnNocache,dSrcDir.FullName));

                StreamWriter sw = new StreamWriter(stCachePath);

                long TotalInterestSize = 0;

                stackDirs.Push(dSrcDir);
                int schonIndex = 0;

                Stack<FileInfo> staHashToCompute = new Stack<FileInfo>();
                while (stackDirs.Count > 0)
                {
                    DirectoryInfo currentDir = (DirectoryInfo)stackDirs.Pop();

                    //Process .\files
                    foreach (FileInfo fileInfo in currentDir.GetFiles())
                    {
                        if (stExtensions.Contains(fileInfo.Extension))
                        {
                            string hash;
                            if ((fileInfo.LastWriteTimeUtc < cacheDate) && srcCache.ContainsKey(fileInfo.FullName))
                            {
                                hash = srcCache[fileInfo.FullName];
                                AddInSrc(src, fileInfo, hash, sw);
                            }
                            else
                                staHashToCompute.Push(fileInfo);
   
                            
                            TotalInterestSize += fileInfo.Length;

                            {
                                ++schonIndex;
                                Console.CursorLeft = cursorLeft;
                                Console.Write(_stSchon[schonIndex % _stSchon.Length].ToString());
                            }
                        }
                    }

                    //Process Subdirectories
                    foreach (DirectoryInfo diNext in currentDir.GetDirectories())
                        stackDirs.Push(diNext);
                }

                //Process not cached hashs
                Console.WriteLine();
                 if(staHashToCompute.Count > 10)
                    Console.Write(String.Format(Resources.hashToDo,staHashToCompute.Count) + " ");
                cursorLeft = Console.CursorLeft;

                int done = 0;
                foreach (FileInfo fi in staHashToCompute)
                {
                    ++done;
                    Console.CursorLeft = cursorLeft;
                    Console.Write(_stSchon[done % _stSchon.Length].ToString());
                    if(staHashToCompute.Count > 10)
                        if(done % (staHashToCompute.Count / 10) == 0)
                            Console.Write(" " + String.Format(Resources.hashWIP, done, staHashToCompute.Count));
                    string hash = GetMD5HashFromFile(fi.FullName);
                    AddInSrc(src, fi, hash, sw);
                }
                sw.Close();

                //stopw.Stop();
                Console.WriteLine();
                return TotalInterestSize; 
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void AddInSrc(Dictionary<string, FileInfo> src, FileInfo fileInfo, string hash, StreamWriter sw)
        {
            if (!src.ContainsKey(hash))
            {
                src.Add(hash, fileInfo);
                sw.WriteLine(hash + '|' + fileInfo.FullName);
            }
            else
                if (!fileInfo.Extension.Contains("jpg") && (fileInfo.FullName != src[hash].FullName))
                {
                    Console.WriteLine(String.Format(Resources.warnduplicate, fileInfo.FullName, src[hash].FullName));
                }
        }

        private static char[] _stSchon = new char[] { '-', '\\', '|', '/' };


        private static MD5 md5 = new MD5CryptoServiceProvider();

        private static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
