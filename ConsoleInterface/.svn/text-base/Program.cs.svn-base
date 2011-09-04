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
                    String source = String.Empty;
                    String dest1 = String.Empty;
                    String dest2 = String.Empty;
                    String destBin = null;
                    String stExt = ".mp3|.jpg";

                    #region args
                    for (int i = 0; i < args.Length; ++i)
                    {
                            switch (args[i])
                            {
                                case "-s":
                                    source = args[++i];
                                    break;
                                case "-d1":
                                    dest1 = args[++i];
                                    break;
                                case "-d2":
                                    dest2 = args[++i];
                                    break;
                                case "-dbin":
                                    destBin = args[++i];
                                    break;
                                case "ext":
                                    stExt = args[++i];
                                    break;
                                default:
                                    throw new ArgumentException("Unknown option: " + args[i]);
                            }   
                    }
                    

                    Stack<DirectoryInfo> targets = new Stack<DirectoryInfo>();

                    if(Directory.Exists(dest1))
                        targets.Push(new DirectoryInfo(dest1));
                    else 
                        throw new ArgumentException("can't find dest dir: " + dest1);

                     if(Directory.Exists(dest2))
                         targets.Push(new DirectoryInfo(dest2));
                    else
                         throw new ArgumentException("can't find dest dir: " + dest2);

                    if(!Directory.Exists(source))
                        throw new ArgumentException("can't find source dir: " + source);

                    if ( (destBin != null) && (!Directory.Exists(destBin)))
                        throw new ArgumentException("can't find destination Bin1 dir: " + destBin);

                    #endregion

                    Mp3Sync.Mp3Synchronizer.synchronize(new DirectoryInfo(source),
                                                        targets,
                                                        destBin == null ? null : new DirectoryInfo(destBin),
                                                        stExt);
                    Console.WriteLine("Process over - Press any key");
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n-- Error --");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Process over - Press any key");
                Console.ReadKey();
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine(@"Mp3Sync. synchronizes a source directory with 1 or 2 destination dirs.

* Any file from the source will be copied to the destination
* Any present in the destination but not in the source will be moved to the destinationBin, or deleted
* Any out of date file in the destination will be updated.

Syntax:
    Mp3Sync.exe -s SourcePath -d1 DestinationPath1 -d2 DestinationPath2 [-dbin DestinationBin] [-ext ExtensionString]

SourcePath:     Path of the source to synchronize with
DestinationPath1:   Path of the destination 1 (player music dir)
DestinationPath2 (optional):    Path of the destination 2 (player sd card music dir)
DestinationBin (optional):  Were to put files found on the player, but not in the source. 
                            If not provided, extra or out of date files will be deleted.
ExtensionString (optional): Pipe separated extensions of the files to synchronize. if not provided, "".mp3|.jpg"" will be used.

Licensed under the GPL - 2010 Yocto Projects - Vincent Tollu

Press any key.");
Console.ReadKey();
        }
    }
}
