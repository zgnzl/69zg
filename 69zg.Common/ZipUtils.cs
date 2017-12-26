using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _69zg.Common
{
    /// <summary>
    /// Zip操作基于 DotNetZip 的封装
    /// </summary>
    public static class ZipUtils
    {

        #region 将文件夹重新压缩
        public static bool CompressZip(string path)
        {
            bool result = true;
            string zipName = string.Format(@"{0}.zip", path);
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(path);
                    //zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                    zip.Save(zipName);
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 将文件压缩到不同的路径下
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="sourcePath">原路径</param>
        /// <param name="desitPath">目标路径</param>
        /// <returns></returns>
        public static bool CompressZip(string sourcePath, string destPath, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = UTF8Encoding.UTF8;
            }
            bool result = true;
            if (File.Exists(destPath))
            {
                File.Delete(destPath);
            }
            try
            {
                using (ZipFile zip = new ZipFile(encode))
                {
                    zip.AddDirectory(sourcePath);
                    zip.Save(destPath);
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region 将zip文件解压到指定目录
        public static void ExtractZip(string filefullpath, string destinationPath)
        {
            using (ZipFile zip = ZipFile.Read(new FileUtil().ReadFileByPath(filefullpath)))
            {
                zip.ExtractAll(destinationPath, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        public static void ExtractZip(string filefullpath, string destinationPath, bool formatTopLevel = false)
        {
            ExtractZip(filefullpath, destinationPath);
            FileUtil.MoveFileAndFolderToFirstLayer(destinationPath);
        }
        #endregion

        #region 获取zip树
        /// <summary>
        /// 获取zip文件的目录树
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="isExtract">是否解压zip包，默认不解压</param>
        /// <returns>目录树json</returns>
        public static List<TreeNode> ZipList(string path, bool isExtract = false)
        {
            List<ZipItem> result = new List<ZipItem>();
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);// 打开文件   
            byte[] bytes = new byte[fileStream.Length];// 读取文件的 byte[]   
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            Stream ExistingZipFile = new MemoryStream(bytes);// 把 byte[] 转换成 Stream   
            ReadOptions readoption = new ReadOptions();
            readoption.Encoding = Encoding.GetEncoding("GB2312");
            //readoption.Encoding = Encoding.Default;
            using (ZipFile zip = ZipFile.Read(ExistingZipFile, readoption))
            {
                #region 判断是否解压文件夹
                if (isExtract)
                {
                    string folderpath = path.Replace(".zip", "");
                    new FileUtil().DelFolder(folderpath);
                    zip.ExtractAll(folderpath);
                    FileUtil.MoveFileAndFolderToFirstLayer(folderpath);
                    List<FileNode> list = FileUtil.GetDirectoryAndFilePath(folderpath);
                    foreach (var li in list)
                    {
                        ZipItem zipitem = new ZipItem();
                        string filename = li.FileName;
                        string[] ary = filename.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        int length = ary.Length;
                        zipitem.path = filename;
                        zipitem.rank = length;
                        zipitem.name = ary[length - 1];
                        zipitem.isdirectory = li.IsDirectory;
                        if (zipitem != null && !result.Contains(zipitem))
                        {
                            result.Add(zipitem);
                        }
                    }
                }
                else
                {
                    foreach (ZipEntry e in zip)
                    {
                        ZipItem zipitem = new ZipItem();
                        string filename = e.FileName;
                        string[] ary = filename.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        int length = ary.Length;
                        zipitem.path = filename;
                        zipitem.rank = length;
                        zipitem.name = ary[length - 1];
                        zipitem.isdirectory = e.IsDirectory;
                        if (zipitem != null && !result.Contains(zipitem))
                        {
                            result.Add(zipitem);
                        }
                    }
                }
                #endregion
            }
            List<TreeNode> nodeList = TreeDiTui(result, GetfirstDirectory(result, 1));
            return nodeList;
        }

        private static int GetfirstDirectory(List<ZipItem> result, int firstDirectory)
        {
            if (result.Count > 0 && result.Where(p => p.rank == firstDirectory).Count() == 1 && result.Where(p => p.rank == firstDirectory).ToList()[0].isdirectory)
            {
                return GetfirstDirectory(result, firstDirectory + 1);
            }
            else
            {
                return firstDirectory;
            }
        }

        /// <summary>
        /// 获取zip文件的目录树
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="isExtract">是否解压zip包，默认不解压</param>
        /// <returns>目录树json</returns>
        public static List<TreeNode> ZipList(string path, string destPath, bool isExtract = false)
        {
            List<ZipItem> result = new List<ZipItem>();
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);// 打开文件   
            byte[] bytes = new byte[fileStream.Length];// 读取文件的 byte[]   
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            Stream ExistingZipFile = new MemoryStream(bytes);// 把 byte[] 转换成 Stream   
            ReadOptions readoption = new ReadOptions();
            readoption.Encoding = Encoding.GetEncoding("GB2312");
            using (ZipFile zip = ZipFile.Read(ExistingZipFile, readoption))
            {
                #region 判断是否解压文件夹
                if (isExtract)
                {
                    zip.ExtractAll(destPath, ExtractExistingFileAction.OverwriteSilently);
                }
                #endregion

                foreach (ZipEntry e in zip)
                {
                    ZipItem zipitem = new ZipItem();
                    string filename = e.FileName;
                    string[] ary = filename.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    int length = ary.Length;
                    zipitem.path = filename;
                    zipitem.rank = length;
                    zipitem.name = ary[length - 1];
                    zipitem.isdirectory = e.IsDirectory;
                    if (zipitem != null && !result.Contains(zipitem))
                    {
                        result.Add(zipitem);
                    }
                }
            }

            List<TreeNode> nodeList = TreeDiTui(result, 1);
            return nodeList;
        }

        public static List<TreeNode> ZipList(byte[] bytes)
        {
            List<ZipItem> result = new List<ZipItem>();
            Stream ExistingZipFile = new MemoryStream(bytes);// 把 byte[] 转换成 Stream   
            ReadOptions readoption = new ReadOptions();
            readoption.Encoding = Encoding.GetEncoding("GB2312");
            using (ZipFile zip = ZipFile.Read(ExistingZipFile, readoption))
            {
                foreach (ZipEntry e in zip)
                {
                    ZipItem zipitem = new ZipItem();
                    string filename = e.FileName;
                    string[] ary = filename.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    int length = ary.Length;
                    zipitem.path = filename;
                    zipitem.rank = length;
                    zipitem.name = ary[length - 1];
                    zipitem.isdirectory = e.IsDirectory;
                    if (zipitem != null && !result.Contains(zipitem))
                    {
                        result.Add(zipitem);
                    }
                }
            }
            List<TreeNode> nodeList = TreeDiTui(result, GetfirstDirectory(result, 1));
            return nodeList;
        }
        public static List<TreeNode> TreeDiTui(List<ZipItem> list, int rank)
        {
            List<ZipItem> zips = list.Where(p => p.rank == rank).ToList();
            List<TreeNode> children = new List<TreeNode>();
            foreach (var zip in zips)
            {

                TreeNode child = new TreeNode();
                child.path = zip.path;
                child.text = zip.name;
                child.state = "{'opened': true}";
                child.rank = zip.rank;
                child.children = TreeDiTui(list.Where(p => p.path.StartsWith(zip.path)).ToList(), rank + 1);
                if (child.children == null || child.children.Count == 0)
                {
                    child.icon = "none";
                }
                if (zip.isdirectory)
                {
                    child.icon = null;
                }
                if (!children.Contains(child))
                {
                    children.Add(child);
                }
            }
            return children;
        }
        #endregion

        #region 修改zip中的文件
        /// <summary>
        /// 修改zip文件的文件内容
        /// </summary>
        /// <param name="path">zip文件的绝对路径</param>
        /// <param name="filename">zip文件中的文件名称</param>
        /// <param name="content">待写入zip文件中指定文件的内容</param>
        /// <returns></returns>
        public static bool ZipUpdate(string path, string filename, string content)
        {
            bool result = true;

            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);// 打开文件   
            byte[] bytes = new byte[fileStream.Length];// 读取文件的 byte[]   
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            Stream ExistingZipFile = new MemoryStream(bytes);// 把 byte[] 转换成 Stream   

            using (ZipFile zip = ZipFile.Read(ExistingZipFile))
            {
                try
                {
                    zip.UpdateEntry(filename, content);
                    zip.Save(path);
                }
                catch
                {
                    result = false;
                }

            }
            return result;
        }
        #endregion

        #region 获取zip中的文件
        /// <summary>
        /// 修改zip文件的文件内容
        /// </summary>
        /// <param name="path">zip文件的绝对路径</param>
        /// <param name="filename">zip文件中的文件名称</param>
        /// <param name="content">待写入zip文件中指定文件的内容</param>
        /// <returns></returns>
        public static string ZipFileGet(string path, string filename)
        {

            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);// 打开文件   
            byte[] bytes = new byte[fileStream.Length];// 读取文件的 byte[]   
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            Stream ExistingZipFile = new MemoryStream(bytes);// 把 byte[] 转换成 Stream   

            MemoryStream tempStream = new MemoryStream();
            string text = "";

            using (ZipFile zip = ZipFile.Read(ExistingZipFile))
            {
                foreach (ZipEntry item in zip)
                {
                    if (item.FileName == filename)
                    {
                        item.Extract(tempStream);
                        tempStream.Position = 0;
                        StreamReader reader = new StreamReader(tempStream);
                        text = reader.ReadToEnd();
                    }
                }
            }
            return text;
        }
        #endregion
    }

    public class TreeNode
    {
        public int rank { get; set; }
        public string icon { get; set; }
        public string text { get; set; }
        public string path { get; set; }
        public string state { get; set; }
        public List<TreeNode> children { get; set; }
    }

    public class ZipItem
    {
        public int rank { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public bool isdirectory { get; set; }
    }
}
