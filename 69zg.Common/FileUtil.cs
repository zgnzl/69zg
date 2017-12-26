using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace _69zg.Common
{
    public class FileUtil
    {
        public string zipExtend = ".zip";
        private object OLOCK = new object();
        public string temppath = "TEMP";
        public List<DictionaryEntry> GetCssStyle(string directorypath)
        {
            List<DictionaryEntry> listCssStyle = new List<DictionaryEntry>();
            DirectoryInfo dirinfo = new DirectoryInfo(directorypath);
            FileInfo[] fileinfos = dirinfo.GetFiles();
            foreach (FileInfo fileinfo in fileinfos)
            {
                DictionaryEntry de = new DictionaryEntry();
                de.Value = fileinfo.Name.Replace(fileinfo.Extension, "");
                de.Key = fileinfo.Name;
                listCssStyle.Add(de);
            }
            return listCssStyle;
        }

        public List<TreeNode> UnZipFile(HttpPostedFile postFile, string path, string filename)
        {
            string zippath = GetDirectory(path);
            string name = postFile.FileName;
            string filepath = Path.Combine(zippath, filename);
            byte[] bytes = new byte[postFile.ContentLength];
            postFile.InputStream.Read(bytes, 0, postFile.ContentLength);
            postFile.SaveAs(filepath + name.Substring(name.LastIndexOf(".")));
            if (Directory.Exists(filepath + temppath))
            {
                DeleteDirectory(filepath + temppath);
            }
            Task.Factory.StartNew(() =>
            {
                UnCompressZipToTemp(filepath);
            });
            List<TreeNode> list = ZipUtils.ZipList(bytes);
            return list;
        }

        public bool UnCompressZipToTemp(string directory)
        {
            if (!Directory.Exists(directory + temppath) && !File.Exists(directory + zipExtend))
            {
                return false;
            }
            else if (!Directory.Exists(directory + temppath))
            {
                ReadOptions readoption = new ReadOptions();
                readoption.Encoding = Encoding.Default;
                using (ZipFile zip = ZipFile.Read(new FileUtil().ReadFileByPath(directory + zipExtend), readoption))
                {
                    zip.ExtractAll(directory, ExtractExistingFileAction.OverwriteSilently);
                    GetChildFolder(directory).MoveTo(directory + temppath);
                    if (Directory.Exists(directory))
                    {
                        DeleteDirectory(directory);
                    }
                }
            }
            return true;
        }

        public DirectoryInfo GetChildFolder(string path)
        {
            DirectoryInfo directoryinfo = new DirectoryInfo(path);
            if (directoryinfo.GetDirectories().Length == 1 && directoryinfo.GetFiles().Length == 0)
            {
                return GetChildFolder(directoryinfo.GetDirectories()[0].FullName);
            }
            else
            {
                return directoryinfo;
            }
        }

        public string GetZipFileContent(string directory, string filepath)
        {
            directory = RelativeToAbsolutePath(directory);
            if (!UnCompressZipToTemp(directory))
            {
                return null;
            }
            string content = string.Empty;
            FileInfo fileinfo = new FileInfo(Path.Combine(directory + temppath, filepath));
            fileinfo.IsReadOnly = false;
            using (FileStream reader = fileinfo.OpenRead())
            {
                Encoding encoding = GetFileEncodeType(Path.Combine(directory + temppath, filepath));
                int nBytes = (int)reader.Length;//计算流的长度
                byte[] byteArray = new byte[nBytes];//初始化用于MemoryStream的Buffer
                int nBytesRead = reader.Read(byteArray, 0, nBytes);//将File里的内容一次性的全部读到byteArray中去
                //去除BOM
                string withoutBomString = "";
                byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };
                if (reader.Length > 3 && byteArray[0] == bomBuffer[0] && byteArray[1] == bomBuffer[1] && byteArray[2] == bomBuffer[2])
                {
                    withoutBomString = encoding.GetString(byteArray, 3, byteArray.Length - 3);
                }
                else
                {
                    withoutBomString = encoding.GetString(byteArray);
                }
                content = filepath.Length - filepath.LastIndexOf("txt") == 3 ? Encoding.Default.GetString(byteArray) : withoutBomString;
                reader.Close();
            }
            return content;
        }

        public bool UpdateZipFileContent(string directory, string filepath, string content)
        {
            string path = Path.Combine(RelativeToAbsolutePath(directory + temppath), filepath);
            FileInfo info = new FileInfo(path);
            info.Attributes = info.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
            using (StreamWriter writer = new StreamWriter(path, false, GetFileEncodeType(path)))
            {
                writer.Write(content);
                writer.Flush();
                writer.Close();
            }
            return true;
        }

        public bool HandleTempFiles(string AC_ID)
        {
            bool result = false;
            string directorypath = Path.Combine(RelativeToAbsolutePath("GlobalConstParam.ActiviteHtmlPath"), AC_ID);
            if (!Directory.Exists(directorypath + temppath) && File.Exists(directorypath + zipExtend))
            {
                return true;
            }
            if (result = ZipUtils.CompressZip(directorypath + temppath, directorypath + zipExtend))
            {
                DeleteDirectory(directorypath + temppath);
            }
            return result;
        }

        public DataTable GetTableByExcel(string filepath)
        {
            DataTable execldt = new DataTable();
            execldt = ExcelHelper.GetExcelData(filepath, "GlobalConstParam.ExcelSheetName");
            if (execldt != null)
            {
                foreach (DataColumn dc in execldt.Columns)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
            }
            return execldt;
        }

        public DataTable GetTableByExcel(HttpPostedFile postFile)
        {
            string filename = postFile.FileName;
            string directorypath = string.Empty;
            string filepath = Path.Combine(GetDirectory("GlobalConstParam.TempFile), UserInfo.GetCurrentUserInfo().accountName" + DateTime.Now.ToString("yyyy-MM-dd") + filename));
            postFile.SaveAs(filepath);
            return GetTableByExcel(filepath);
        }
        public bool DeleteTempFile()
        {
            DirectoryInfo dirinfo = new DirectoryInfo(GetDirectory("GlobalConstParam.TempFile"));
            FileInfo[] files = dirinfo.GetFiles();
            if (files.Length == 0)
            {
                return false;
            }
            foreach (FileInfo fileinfo in files)
            {
                fileinfo.Delete();
            }
            return true;
        }

        public void DeleteActivitesFiles(string route, string id)
        {
            string directorypath = RelativeToAbsolutePath("GlobalConstParam.ActiviteHtmlPath");
            if (Directory.Exists(Path.Combine(directorypath, id, temppath)))
            {
                DeleteDirectory(Path.Combine(directorypath, id, temppath));
            }
            foreach (FileInfo fileinfo in new DirectoryInfo(directorypath).GetFiles())
            {
                if (fileinfo.Name.Equals(route + zipExtend) || fileinfo.Name.StartsWith(id, false, System.Globalization.CultureInfo.CurrentCulture))
                {
                    fileinfo.IsReadOnly = false; fileinfo.Delete();
                }
            }
        }

        public void DeleteDirectory(string directory)
        {
            DirectoryInfo directroyinfo = new DirectoryInfo(directory);
            if (directroyinfo.GetDirectories().Length > 0)
            {
                foreach (DirectoryInfo dire in directroyinfo.GetDirectories())
                    DeleteDirectory(dire.FullName);
                foreach (FileInfo fileinfo in directroyinfo.GetFiles())
                {
                    fileinfo.IsReadOnly = false; fileinfo.Delete();
                }
                directroyinfo.Delete(true);
            }
            else
            {
                foreach (FileInfo fileinfo in directroyinfo.GetFiles())
                {
                    fileinfo.IsReadOnly = false; fileinfo.Delete();
                }
                directroyinfo.Delete(true);
            }
        }

        public string GetToken(Guid BR_ID, string token)
        {
            string filepath = Path.Combine(GetDirectory("tokenfiles"), BR_ID + ".txt");
            if (string.IsNullOrEmpty(token))
            {
                StreamWriter sw = new StreamWriter(filepath, false, Encoding.UTF8);
                token = EncryptUtil.EnDeString(DateTime.Now.ToBinary() + (new Random().Next(1000, 9999)).ToString() + BR_ID, true);
                sw.Write(token);
                sw.Flush();
                sw.Close();
                return token.Trim();
            }
            else
            {
                if (File.Exists(filepath))
                {
                    StreamReader sr = new StreamReader(filepath, Encoding.UTF8);
                    string localtoken = sr.ReadToEnd();
                    sr.Close();
                    if (token.Equals(localtoken.Trim()))
                    {
                        return bool.TrueString;
                    }
                }
                return bool.FalseString;
            }
        }
        public void SyncToken(string BR_ID, string token)
        {
            //AsyncFile(BR_ID, System.IO.Path.Combine(GetDirectory("tokenfiles"), BR_ID, ".txt"), "token,0");
        }

        public string GetToken(string id)
        {
            string filepath = Path.Combine(GetDirectory("tokenfiles"), id + ".txt");
            StreamReader sr = new StreamReader(filepath, Encoding.UTF8);
            string localtoken = sr.ReadToEnd();
            sr.Close();
            return localtoken;
        }

        public string GetTextFromXml(XmlDocument xmldoc, string nodename)
        {
            StringBuilder returnsb = new StringBuilder();
            foreach (XmlNode xmlnode in xmldoc.SelectNodes(xmldoc.DocumentElement.Name + "/" + nodename))
            {
                returnsb.Append(xmlnode.InnerText + ";");
            }
            return returnsb.ToString().Trim(';');
        }

        public string GetDirectory(string relativedirectory)
        {
            string directroyPath = RelativeToAbsolutePath(relativedirectory);
            if (!Directory.Exists(directroyPath))
            {
                Directory.CreateDirectory(directroyPath);
            }
            return directroyPath;
        }
        public string RelativeToAbsolutePath(string relativedirectory)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativedirectory);
        }
      
        /// <summary>
        /// 获取文件夹和文件的路径集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<FileNode> GetDirectoryAndFilePath(string path)
        {

            List<FileNode> list = new List<FileNode>();
            if (Directory.Exists(path))
            {
                list = GetChildDirecotryAndFilePath(path, path, list);
            }
            return list;
        }

        /// <summary>
        /// 获取子文件夹和文件的路径集合
        /// </summary>
        /// <param name="root">根目录</param>
        /// <param name="path">文件路径</param>
        /// <param name="list">文件节点集</param>
        /// <returns></returns>
        private static List<FileNode> GetChildDirecotryAndFilePath(string root, string path, List<FileNode> list)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                FileNode node = new FileNode();
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    node.IsDirectory = true;
                    GetChildDirecotryAndFilePath(root, i.FullName, list);
                }
                else
                {
                    node.IsDirectory = false;
                }
                //node.FileName = i.FullName;
                node.FileName = i.FullName.Replace(root, "").Replace("\\", "/");
                list.Add(node);
            }
            return list;
        }

        /// <summary>
        /// 根据文件路径读取到内存中
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Stream ReadFileByPath(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);// 打开文件   
            byte[] bytes = new byte[fileStream.Length];// 读取文件的 byte[]   
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="from">文件原路径</param>
        /// <param name="to">文件目标路径</param>
        /// <param name="isDel">是否删除源文件</param>
        public void CopyTo(string from, string to, bool isDel = false)
        {
            File.Copy(from, to, true);
            if (isDel)
            {
                File.Delete(from);//删除源文件
                DelFolder(from.Replace(".zip", ""));//删除对应文件夹
            }
        }

        /// <summary>
        /// 删除文件夹和子文件
        /// </summary>
        /// <param name="path"></param>
        public void DelFolder(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        //subdir.Delete(true);          //删除子目录和文件
                        DeleteFileByDirectory(subdir);
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="path"></param>
        public void DelFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 删除目标文件夹下同名的所有文件
        /// </summary>
        /// <param name="path"></param>
        public void DelSameNameFile(string destFolder, string name)
        {
            if (Directory.Exists(destFolder))
            {
                DirectoryInfo dir = new DirectoryInfo(destFolder);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (Path.GetFileNameWithoutExtension(i.Name).ToLower() == name.ToLower())
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            //subdir.Delete(true);          //删除子目录和文件
                            DeleteFileByDirectory(subdir);
                        }
                        else
                        {
                            File.Delete(i.FullName);      //删除指定文件
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  用来遍历删除目录下的文件以及该文件夹
        /// </summary>
        public void DeleteFileByDirectory(DirectoryInfo info)
        {
            foreach (DirectoryInfo newInfo in info.GetDirectories())
            {
                DeleteFileByDirectory(newInfo);
            }
            foreach (FileInfo newInfo in info.GetFiles())
            {
                newInfo.Attributes = newInfo.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                newInfo.Delete();
            }
            info.Attributes = info.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
            info.Delete();

        }

        /// <summary>
        /// 判断文件夹是否存在，不存在的情况下创建
        /// </summary>
        /// <param name="path"></param>
        public static void FolderExistOrNot(string path)
        {
            if (!Directory.Exists(path))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 移动子文件和文件夹到第一级子目录
        /// </summary>
        /// <param name="path"></param>
        public static void MoveFileAndFolderToFirstLayer(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                if (fileinfo.Length == 1)
                {
                    FileSystemInfo i = fileinfo[0];
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        GetTheFirstLayerFile(i.FullName, path, i.FullName);
                    }
                }
            }
        }

        private static void GetTheFirstLayerFile(string path, string destPath, string delPath)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            if (fileinfo.Length == 1)
            {
                FileSystemInfo i = fileinfo[0];
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    GetTheFirstLayerFile(i.FullName, destPath, delPath);
                }
                else
                {
                    MoveFolder(Path.GetDirectoryName(i.FullName), destPath);
                    new DirectoryInfo(delPath).Delete(true);//删除子目录和文件
                }
            }
            else
            {
                MoveFolder(path, destPath);
                new DirectoryInfo(delPath).Delete(true);//删除子目录和文件
            }
        }

        /// <summary>  
        /// 移动文件夹中的所有文件夹与文件到另一个文件夹 
        /// 
        /// </summary>  
        /// <param name="sourcePath">源文件夹</param>  
        /// <param name="destPath">目标文件夹</param>  
        public static void MoveFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建  
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("创建目标目录失败：" + ex.Message);
                    }
                }
                //获得源文件下所有文件  
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //覆盖模式  
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                    File.Move(c, destFile);
                });
                //获得源文件下所有目录文件  
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));

                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //采用递归的方法实现  
                    MoveFolder(c, destDir);
                });
            }
            else
            {
                throw new DirectoryNotFoundException("源目录不存在！");
            }
        }

        #region 判断文件编码类型
        public static System.Text.Encoding GetFileEncodeType(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        public static System.Text.Encoding GetFileEncodeType(FileStream fs)
        {
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        /// <summary> 
        /// 通过给定的文件流，判断文件的编码类型 
        /// </summary> 
        /// <param name=“fs“>文件流</param> 
        /// <returns>文件的编码类型</returns> 
        public static System.Text.Encoding GetType(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss))
            {
                reVal = Encoding.UTF8;
            }
            else if ((ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = new UTF8Encoding(false);
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }

        /// <summary> 
        /// 判断是否是不带 BOM 的 UTF8 格式 
        /// </summary> 
        /// <param name="data"></param> 
        /// <returns></returns> 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
        #endregion

        public Tuple<bool, string> UploadMergeFile(string UploadFilePath)
        {
            var chunk = ResquestUtil.GetValue("chunk");//当前是第多少片 
            var path = Path.Combine(UploadFilePath, "UserInfo.GetCurrentUserInfo().accountName", Path.GetFileNameWithoutExtension(HttpContext.Current.Request.Files[0].FileName));// Server.MapPath("~/App_Data/") + Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
            if (!Directory.Exists(path))//判断是否存在此路径，如果不存在则创建
            {
                Directory.CreateDirectory(path);
            }
            if (HttpContext.Current.Request.Files.Count > 0 && !HttpContext.Current.Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                HttpContext.Current.Request.Files[0].SaveAs(Path
                    .Combine(path, "0"));
            }
            else
            {

                //保存文件到根目录 App_Data + 获取文件名称和格式
                var filePath = path + "/" + chunk;
                //创建一个追加（FileMode.Append）方式的文件流
                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        //读取文件流
                        BinaryReader br = new BinaryReader(HttpContext.Current.Request.Files[0].InputStream);
                        //将文件留转成字节数组
                        byte[] bytes = br.ReadBytes((int)HttpContext.Current.Request.Files[0].InputStream.Length);
                        //将字节数组追加到文件
                        bw.Write(bytes);
                    }
                }
            }
            return Tuple.Create(true, "文件上传成功");
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Tuple<bool, string, string> CombinFileMerge(string ID, string UploadFilePath, bool rename = false)
        {
            var user = SessionInfo.GetSession("GlobalConstParam.UserInfo");
            if (user == null)
            {
                return Tuple.Create(false, new ErrorMsg().GetCodeMsg("-1004"), "");
            }
            var fileName = ResquestUtil.GetValue( "fileName");
            string savefilename = fileName;
            if (rename)
            {
                savefilename = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            }
            var path = Path.Combine(UploadFilePath, "UserInfo.GetCurrentUserInfo().accountName", Path.GetFileNameWithoutExtension(fileName));// Server.MapPath("~/App_Data/") + Path.GetFileNameWithoutExtension(fileName);
            //这里排序一定要正确，转成数字后排序（字符串会按1 10 11排序，默认10比2小）
            if (!Directory.Exists(Path.Combine(UploadFilePath, ID)))//判断是否存在此路径，如果不存在则创建
            {
                Directory.CreateDirectory(Path.Combine(UploadFilePath, ID));
            }
            foreach (var filePath in Directory.GetFiles(path).OrderBy(t => int.Parse(Path.GetFileNameWithoutExtension(t))))
            {
                using (FileStream fs = new FileStream(Path.Combine(UploadFilePath, ID, savefilename), FileMode.Append, FileAccess.Write))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);//读取文件到字节数组
                    fs.Write(bytes, 0, bytes.Length);//写入文件
                }
                System.IO.File.Delete(filePath);
            }
            Directory.Delete(Path.Combine(UploadFilePath, "UserInfo.GetCurrentUserInfo().accountName", Path.GetFileNameWithoutExtension(fileName)), true);
            return Tuple.Create(true, fileName + "_长传成功", savefilename);
        }

    }

    public class UploadFileUtils
    {
        public string FileName { get; set; }
        public string TempFolder { get; set; }
        public int MaxFileSizeMB { get; set; }
        public List<String> FileParts { get; set; }

        public UploadFileUtils()
        {
            FileParts = new List<string>();
        }

        /// <summary>
        /// original name + ".part_N.X" (N = file part number, X = total files)
        /// Objective = enumerate files in folder, look for all matching parts of split file. If found, merge and return true.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool MergeFile(string FileName)
        {
            bool rslt = false;
            // parse out the different tokens from the filename according to the convention
            string partToken = ".part_";
            string baseFileName = FileName.Substring(0, FileName.IndexOf(partToken));
            string trailingTokens = FileName.Substring(FileName.IndexOf(partToken) + partToken.Length);
            int FileIndex = 0;
            int FileCount = 0;
            int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
            int.TryParse(trailingTokens.Substring(trailingTokens.IndexOf(".") + 1), out FileCount);
            // get a list of all file parts in the temp folder
            string Searchpattern = Path.GetFileName(baseFileName) + partToken + "*";
            string[] FilesList = Directory.GetFiles(Path.GetDirectoryName(FileName), Searchpattern);
            //  merge .. improvement would be to confirm individual parts are there / correctly in sequence, a security check would also be important
            // only proceed if we have received all the file chunks
            if (FilesList.Count() == FileCount)
            {
                // use a singleton to stop overlapping processes
                if (!MergeFileManager.Instance.InUse(baseFileName))
                {
                    MergeFileManager.Instance.AddFile(baseFileName);
                    if (File.Exists(baseFileName))
                        File.Delete(baseFileName);
                    // add each file located to a list so we can get them into 
                    // the correct order for rebuilding the file
                    List<SortedFile> MergeList = new List<SortedFile>();
                    foreach (string Files in FilesList)
                    {
                        SortedFile sFile = new SortedFile();
                        sFile.FileName = Files;
                        baseFileName = Files.Substring(0, Files.IndexOf(partToken));
                        trailingTokens = Files.Substring(Files.IndexOf(partToken) + partToken.Length);
                        int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
                        sFile.FileOrder = FileIndex;
                        MergeList.Add(sFile);
                    }
                    // sort by the file-part number to ensure we merge back in the correct order
                    var MergeOrder = MergeList.OrderBy(s => s.FileOrder).ToList();
                    using (FileStream FS = new FileStream(baseFileName, FileMode.Create))
                    {
                        // merge each file chunk back into one contiguous file stream
                        foreach (var chunk in MergeOrder)
                        {
                            try
                            {
                                using (FileStream fileChunk = new FileStream(chunk.FileName, FileMode.Open))
                                {
                                    fileChunk.CopyTo(FS);
                                }
                                File.Delete(chunk.FileName);
                            }
                            catch (IOException ex)
                            {
                                // handle                                
                            }
                        }
                    }
                    rslt = true;
                    // unlock the file from singleton
                    MergeFileManager.Instance.RemoveFile(baseFileName);
                }
            }
            return rslt;
        }
    }

    public struct SortedFile
    {
        public int FileOrder { get; set; }
        public String FileName { get; set; }
    }

    public class MergeFileManager
    {
        private static MergeFileManager instance;
        private List<string> MergeFileList;

        private MergeFileManager()
        {
            try
            {
                MergeFileList = new List<string>();
            }
            catch { }
        }

        public static MergeFileManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MergeFileManager();
                return instance;
            }
        }

        public void AddFile(string BaseFileName)
        {
            MergeFileList.Add(BaseFileName);
        }

        public bool InUse(string BaseFileName)
        {
            return MergeFileList.Contains(BaseFileName);
        }

        public bool RemoveFile(string BaseFileName)
        {
            return MergeFileList.Remove(BaseFileName);
        }
       
    }
    public class FileNode
    {
        public bool IsDirectory { get; set; }
        public string FileName { get; set; }
    }
}
