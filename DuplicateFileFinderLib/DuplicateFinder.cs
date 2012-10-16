using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DuplicateFileFinderLib
{
    public class DuplicateFinder
    {
        public static string FindDuplicates(string rootPath)
        {
            DirectoryInfo dir = new DirectoryInfo(rootPath);
            Dictionary<string, List<FileInfo>> collection = getFilesFromDir(dir);
            Dictionary<string, List<FileInfo>> duplicates = new Dictionary<string, List<FileInfo>>();
            foreach (KeyValuePair<String, List<FileInfo>> entry in collection)
            {
                if (entry.Value.Count > 1)
                {
                    duplicates[entry.Key] = entry.Value;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Duplicates:");
            foreach (KeyValuePair<String, List<FileInfo>> entry in duplicates)
            {
                sb.AppendLine(entry.Key);
                foreach (FileInfo file in entry.Value)
                {
                    sb.AppendLine(file.FullName);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        private static Dictionary<string, List<FileInfo>> getFilesFromDir(DirectoryInfo dir)
        {
            Console.WriteLine();
            Console.Write("Processing " + dir.FullName + "");
            Dictionary<string, List<FileInfo>> collection = new Dictionary<string, List<FileInfo>>();
            DirectoryInfo[] dirs = dir.GetDirectories();
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                addFileToCollection(file, collection);
                Console.Write(".");
            }
            foreach (DirectoryInfo subdir in dirs)
            {
                Dictionary<string, List<FileInfo>> subCollection = getFilesFromDir(subdir);
                addCollection(collection, subCollection);
            }
            return collection;
        }
        private static void addCollection(Dictionary<string, List<FileInfo>> collection, Dictionary<string, List<FileInfo>> addedCollection)
        {
            foreach (KeyValuePair<String, List<FileInfo>> entry in addedCollection)
            {
                string md5 = entry.Key;
                List<FileInfo> files = entry.Value;
                if (!collection.ContainsKey(md5))
                {
                    collection[md5] = new List<FileInfo>();
                }
                collection[md5].AddRange(files);
            }
        }
        private static bool addFileToCollection(FileInfo file, Dictionary<string, List<FileInfo>> collection)
        {
            string MD5 = "";
            try
            {
                MD5 = GetMD5(file);
            }
            catch (IOException ioe)
            {
                return false;
            }
            if (collection.ContainsKey(MD5))
            {
                collection[MD5].Add(file);
                return true;
            }
            else
            {
                collection[MD5] = new List<FileInfo>();
                collection[MD5].Add(file);
                return false;
            }
        }
        private static string GetMD5(FileInfo fileinfo)
        {
            FileStream file = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
