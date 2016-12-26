using FtpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleFtp
{
    class Program
    {
        static void Main(string[] args)
        {
            test();
            Console.WriteLine("Press any key to quit...");
            Console.Read();
        }

        static void test()
        {
            using (FtpConnection ftp = new FtpConnection("192.168.225.1", "flairmicro", "flairmicro"))
            {
                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */

                try
                {
                    ftp.SetCurrentDirectory("/images");
                    ftp.PutFile(@"ftplib.dll", "ftplib.dll"); 
                }
                catch (FtpException e)
                {
                    Console.WriteLine(String.Format("FTP Error: {0} {1}", e.ErrorCode, e.Message));
                }

                foreach (var dir in ftp.GetDirectories("/"))
                {
                    Console.WriteLine(dir.Name);
                    Console.WriteLine(dir.CreationTime);
                    foreach (var file in dir.GetFiles())
                    {
                        Console.WriteLine(file.Name);
                        Console.WriteLine(file.LastAccessTime);
                    }
                }
            }
        }

        /// <summary>
        /// This library wraps the wininet.dll functions for FTP to create an effective way to interact with FTP servers 
        /// using the C# language and the .Net framework. 
        /// https://ftplib.codeplex.com/Wikipage?ProjectName=ftplib
        /// </summary>
        void demo()
        {
            using (FtpConnection ftp = new FtpConnection("ftpserver", "username", "password"))
            {

                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */

                if (ftp.DirectoryExists("/incoming")) /* check that a directory exists */
                    ftp.SetCurrentDirectory("/incoming"); /* change current directory */

                if (ftp.FileExists("/incoming/file.txt"))  /* check that a file exists */
                    ftp.GetFile("/incoming/file.txt", false); /* download /incoming/file.txt as file.txt to current executing directory, overwrite if it exists */

                //do some processing

                try
                {
                    ftp.SetCurrentDirectory("/outgoing");
                    ftp.PutFile(@"c:\localfile.txt", "file.txt"); /* upload c:\localfile.txt to the current ftp directory as file.txt */
                }
                catch (FtpException e)
                {
                    Console.WriteLine(String.Format("FTP Error: {0} {1}", e.ErrorCode, e.Message));
                }

                foreach (var dir in ftp.GetDirectories("/incoming/processed"))
                {
                    Console.WriteLine(dir.Name);
                    Console.WriteLine(dir.CreationTime);
                    foreach (var file in dir.GetFiles())
                    {
                        Console.WriteLine(file.Name);
                        Console.WriteLine(file.LastAccessTime);
                    }
                }
            }
        }
    }
}
