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
using Mp3Sync;
using System.Xml.Serialization;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    PrintUsage();
                else
                {
                    Mp3SyncSettings settings = new Mp3SyncSettings();
 

                    #region args
                    if (args.Length > 1)
                    {
                        for (int i = 0; i < args.Length; ++i)
                        {
                            switch (args[i])
                            {
                                case "-s":
                                    settings.Source = args[++i];
                                    break;
                                case "-d1":
                                case "-d2":
                                    settings.Dests.Add(args[++i]);
                                    break;
                                case "-dbin":
                                    settings.DestBin = args[++i];
                                    break;
                                case "ext":
                                    settings.StExt = args[++i];
                                    break;
                                case "auto":
                                    settings.AutoSync = args[++i];
                                    break;
                                default:
                                    throw new ArgumentException("Unknown option: " + args[i]);
                            }
                        }
                    }
                    else
                    {
                        if (File.Exists(args[0]) && (Path.GetExtension(args[0]) == ".xml"))
                        {
                            using (StreamReader sr = new StreamReader(args[0], true))
                            {
                                XmlSerializer s = new XmlSerializer(typeof(Mp3SyncSettings));
                                settings = (Mp3SyncSettings)(s.Deserialize(sr));
                            }
                        }
                        else
                        {
                            PersistSettings(new Mp3SyncSettings(true));
                            throw new Exception("Could not process the xml input " + args[0] + " see the generated Mp3SyncInputs.xml input file");
                        }
                    }

                    PersistSettings(settings);

                    foreach(string dir in settings.Dests)
                        if (!Directory.Exists(dir))
                            throw new ArgumentException("can't find dest dir: " + dir);

                    if (!Directory.Exists(settings.Source))
                        throw new ArgumentException("can't find source dir: " + settings.Source);

                    if ((settings.DestBin != null) && (!Directory.Exists(settings.DestBin)))
                        throw new ArgumentException("can't find destination Bin dir: " + settings.DestBin);
                    #endregion

                    Mp3Sync.Mp3Synchronizer.synchronize(settings);

                    Console.WriteLine("Process over - Press any key");
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n-- Error --");
                Console.WriteLine(e.Message);
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                Console.WriteLine("\nProcess over - Press any key");
                Console.ReadKey();
            }
        }

        private static void PersistSettings(Mp3SyncSettings settings)
        {
            if (!File.Exists(@"Mp3SyncInputs.xml"))
            {
                //Persist curr settings
                XmlSerializer s = new XmlSerializer(typeof(Mp3SyncSettings));
                System.IO.TextWriter xw = new System.IO.StreamWriter(@"Mp3SyncInputs.xml", false, System.Text.Encoding.UTF8);
                s.Serialize(xw, settings);
                xw.Close();
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine(@"Mp3Sync. synchronizes a source directory with 1 or 2 destination dirs.

* Any file from the source will be copied to the destination
* Any present in the destination but not in the source will be moved to the destinationBin, or deleted
* Any out of date file in the destination will be updated.

Syntax:
    Mp3Sync.exe -s SourcePath -d1 DestinationPath1 -d2 DestinationPath2 [-dbin DestinationBin] [-ext ExtensionString] [-auto AutoSync]

SourcePath:     Path of the source to synchronize with
DestinationPath1:   Path of the destination 1 (player music dir)
DestinationPath2 (optional):    Path of the destination 2 (player sd card music dir)
DestinationBin (optional):  Were to put files found on the player, but not in the source. 
                            If not provided, extra or out of date files will be deleted.
ExtensionString (optional): Pipe separated extensions of the files to synchronize. if not provided, "".mp3"" will be used.
AutoSync in split folders (optional): name of a file to duplicate if a folder happens to be duplicated. If not provided ""folder.jpg"" will be used.

- or -

Mp3Sync.exe Inputs.xml

Provided you filled the Mp3SyncInputs.xml file just created.

Licensed under the GPL - 2010 Yocto Projects - Vincent Tollu

Press any key.");

            PersistSettings(new Mp3SyncSettings(true));

Console.ReadKey();
        }
    }
}
