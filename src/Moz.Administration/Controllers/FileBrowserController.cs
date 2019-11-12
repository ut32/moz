using System;
using System.Collections;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Moz.Administration.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Moz.Administration.Controllers
{
    public class UploadConfig
    {
        public string FILES_ROOT { get; } = "upload";
        public string RETURN_URL_PREFIX { get; } = "";
        public string SESSION_PATH_KEY { get; } = "";
        public string THUMBS_VIEW_WIDTH { get; } = "140";
        public string THUMBS_VIEW_HEIGHT { get; } = "120";
        public string PREVIEW_THUMB_WIDTH { get; } = "300";
        public string PREVIEW_THUMB_HEIGHT { get; } = "200";
        public string MAX_IMAGE_WIDTH { get; } = "10000";
        public string MAX_IMAGE_HEIGHT { get; } = "10000";
        public string INTEGRATION { get; } = "ckeditor";
        public string DIRLIST { get; } = "filebrowser/dirlist";
        public string CREATEDIR { get; } = "filebrowser/createdir";
        public string DELETEDIR { get; } = "filebrowser/deletedir";
        public string MOVEDIR { get; } = "asp_net/main.ashx?a=MOVEDIR";
        public string COPYDIR { get; } = "asp_net/main.ashx?a=COPYDIR";
        public string RENAMEDIR { get; } = "filebrowser/RenameDir";
        public string FILESLIST { get; } = "filebrowser/fileslist";
        public string UPLOAD { get; } = "filebrowser/upload";
        public string DOWNLOAD { get; } = "filebrowser/download";
        public string DOWNLOADDIR { get; } = "asp_net/main.ashx?a=DOWNLOADDIR";
        public string DELETEFILE { get; } = "filebrowser/DeleteFile";
        public string MOVEFILE { get; } = "asp_net/main.ashx?a=MOVEFILE";
        public string COPYFILE { get; } = "asp_net/main.ashx?a=COPYFILE";
        public string RENAMEFILE { get; } = "filebrowser/RenameFile";
        public string GENERATETHUMB { get; } = "filebrowser/GenerateThumb";
        public string DEFAULTVIEW { get; } = "list";

        public string FORBIDDEN_UPLOADS { get; } =
            "zip js jsp jsb mhtml mht xhtml xht php phtml php3 php4 php5 phps shtml jhtml pl sh py cgi exe application gadget hta cpl msc jar vb jse ws wsf wsc wsh ps1 ps2 psc1 psc2 msh msh1 msh2 inf reg scf msp scr dll msi vbs bat com pif cmd vxd cpl htpasswd htaccess";

        public string ALLOWED_UPLOADS { get; } = "";
        public string FILEPERMISSIONS { get; } = "0644";
        public string DIRPERMISSIONS { get; } = "0755";
        public string LANG { get; } = "zh";
        public string DATEFORMAT { get; } = "yyyy-MM-dd HH:mm";
        public string OPEN_LAST_DIR { get; } = "yes";
    }

    public class FileBrowserController : AdminAuthBaseController
    {
        
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UploadConfig _uploadConfig;
        public FileBrowserController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._uploadConfig = new UploadConfig();
        }

        public IActionResult Index()
        {
            return View("~/Administration/Views/FileBrowser/Index.cshtml");
        }

        public IActionResult Config()
        {
            return Json(_uploadConfig);
        }

        #region utils
        private string GetFilesRoot(){
            return Path.Combine(_hostingEnvironment.WebRootPath,"upload");
        }
        
        private void CheckPath(string path)
        {
            var fixedPath = FixPath(path);
            var root = GetFilesRoot();
            if (fixedPath.IndexOf(root, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new Exception("Access to " + path + " is denied");
            }
        }
        private string FixPath(string path)
        {
            path = path.TrimStart('~').TrimStart('/');
            return Path.Combine(_hostingEnvironment.WebRootPath,path);
        }
        private double LinuxTimestamp(DateTime d){
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();
            TimeSpan timeSpan = (d.ToLocalTime() - epoch);
            return timeSpan.TotalSeconds;
        }
        private string GetResultStr(string type, string msg)
        {
            return "{\"res\":\"" + type + "\",\"msg\":\"" + msg.Replace("\"","\\\"") + "\"}";
        }
        private string GetSuccessRes(string msg)
        {
            return GetResultStr("ok", msg);
        }
        private string GetSuccessRes()
        {
            return GetSuccessRes("");
        }
        private string GetErrorRes(string msg)
        {
            return GetResultStr("error", msg);
        }
        
        private bool CanHandleFile(string filename)
        {
            bool ret = false;
            FileInfo file = new FileInfo(filename);
            string ext = file.Extension.Replace(".", "").ToLower();
            string setting = _uploadConfig.FORBIDDEN_UPLOADS.Trim().ToLower();
            if (setting != "")
            {
                ArrayList tmp = new ArrayList();
                tmp.AddRange(Regex.Split(setting, "\\s+"));
                if (!tmp.Contains(ext))
                    ret = true;
            }
            setting = _uploadConfig.ALLOWED_UPLOADS.Trim().ToLower();
            if (setting != "")
            {
                ArrayList tmp = new ArrayList();
                tmp.AddRange(Regex.Split(setting, "\\s+"));
                if (!tmp.Contains(ext))
                    ret = false;
            }
            return ret;
        }
        
        private string MakeUniqueFilename(string dir, string filename){
            string ret = filename;
            int i = 0;
            while (System.IO.File.Exists(Path.Combine(dir, ret)))
            {
                i++;
                ret = Path.GetFileNameWithoutExtension(filename) + " - Copy " + i.ToString() + Path.GetExtension(filename);
            }
            return ret;
        }
        protected string LangRes(string name)
        {
            return "zh";
        }
        protected void ImageResize(string path, string dest, int width, int height)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Image img = Image.FromStream(fs);
            fs.Close();
            fs.Dispose();
            float ratio = (float)img.Width / (float)img.Height;
            if ((img.Width <= width && img.Height <= height) || (width == 0 && height == 0))
                return;

            int newWidth = width;
            int newHeight = Convert.ToInt16(Math.Floor((float)newWidth / ratio));
            if ((height > 0 && newHeight > height) || (width == 0))
            {
                newHeight = height;
                newWidth = Convert.ToInt16(Math.Floor((float)newHeight * ratio));
            }
            Bitmap newImg = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage((Image)newImg);
            g.InterpolationMode = System.DrawingCore.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, 0, 0, newWidth, newHeight);
            img.Dispose();
            g.Dispose();
            if(dest != ""){
                newImg.Save(dest, GetImageFormat(dest));
            }
            newImg.Dispose();
        }
        private ImageFormat GetImageFormat(string filename){
            ImageFormat ret = ImageFormat.Jpeg;
            switch(new FileInfo(filename).Extension.ToLower()){
                case ".png": ret = ImageFormat.Png; break;
                case ".gif": ret = ImageFormat.Gif; break;
            }
            return ret;
        }
        #endregion

        #region 目录

        public IActionResult DirList([FromQuery]string type , string path) 
        {
            var d = new DirectoryInfo(Path.Combine(_hostingEnvironment.WebRootPath,"upload"));
            if(!d.Exists)
                throw new Exception("Invalid files root directory. Check your configuration.");

            ArrayList dirs = new ArrayList(); 
            if (!path.IsNullOrEmpty())
            {
                if (path.StartsWith('/') || path.StartsWith('\\'))
                    path = path.Substring(1);
                path = path.Replace("/","\\");
                dirs = ListDirs(Path.Combine(_hostingEnvironment.WebRootPath, path));
            }
            else
            {
                dirs = ListDirs(d.FullName);
                dirs.Insert(0, d.FullName);
            }

            string localPath = _hostingEnvironment.WebRootPath;
            var sb = new StringBuilder();
            sb.Append("[");
            for(int i = 0; i <dirs.Count; i++){
                string dir = (string) dirs[i];
                sb.Append("{\"p\":\"" + dir.Replace(localPath, "").Replace("\\", "/") + "\",\"f\":\"" + GetFiles(dir, type).Count.ToString() + "\",\"d\":\"" + Directory.GetDirectories(dir).Length.ToString() + "\"}");
                if(i < dirs.Count -1)
                    sb.Append(",");
            }
            sb.Append("]");
            return Content(sb.ToString());
        }
        
        private List<string> GetFiles(string path, string type){
            List<string> ret = new List<string>();
            if(type == "#")
                type = "";
            string[] files = Directory.GetFiles(path);
            foreach(string f in files){
                if ((GetFileType(new FileInfo(f).Extension) == type) || (type == ""))
                    ret.Add(f);
            }
            return ret;
        }
        protected string GetFileType(string ext){
            string ret = "file";
            ext = ext.ToLower();
            if(ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                ret = "image";
            else if(ext == ".swf" || ext == ".flv")
                ret = "flash";
            return ret;
        }
        private ArrayList ListDirs(string path){
            string[] dirs = Directory.GetDirectories(path);
            ArrayList ret = new ArrayList();
            foreach(string dir in dirs){
                ret.Add(dir);
                //ret.AddRange(ListDirs(dir));
            }
            return ret;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public IActionResult CreateDir(string d,string n)
        {
            CheckPath(d);
            d = FixPath(d);
            if(!Directory.Exists(d))
                return Content(GetErrorRes("无效路径下不能创建文件夹"));
            else{
                try
                {
                    d = Path.Combine(d, n);
                    if(Directory.Exists(d))
                        return Content(GetErrorRes("此文件夹已存在，不能重名"));
                    else
                        Directory.CreateDirectory(d);
                    return Content(GetSuccessRes());
                }
                catch (Exception ex)
                {
                    return Content(GetErrorRes("此文件夹已存在，不能重名"));
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public IActionResult RenameDir(string d, string n)
        {
            CheckPath(d);
            DirectoryInfo source = new DirectoryInfo(FixPath(d));
            DirectoryInfo dest = new DirectoryInfo(Path.Combine(source.Parent.FullName, n));
            if(source.FullName.Equals(GetFilesRoot(), StringComparison.OrdinalIgnoreCase))
                return Content(GetErrorRes("不能重命名根目录"));
            else if (!source.Exists)
                return Content(GetErrorRes("文件夹无效"));
            else if (dest.Exists)
                return Content(GetErrorRes("名字已存在"));
            else
            {
                try
                {
                    source.MoveTo(dest.FullName);
                    return Content(GetSuccessRes());
                }
                catch (Exception ex)
                {
                    return Content(GetErrorRes("名字已存在"));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public IActionResult DeleteDir(string d)
        {
            CheckPath(d);
            d = FixPath(d);
            if (!Directory.Exists(d))
                return Content(GetErrorRes(LangRes("路径无效")));
            else if (d.Equals(GetFilesRoot(), StringComparison.OrdinalIgnoreCase))
                return Content(GetErrorRes("不能删除根目录"));
            else if(Directory.GetDirectories(d).Length > 0 || Directory.GetFiles(d).Length > 0)
                return Content(GetErrorRes("该目录下还有文件夹或文件，不能删除"));
            else
            {
                try
                {
                    Directory.Delete(d);
                    return Content(GetSuccessRes());
                }
                catch (Exception ex)
                {
                    return Content(GetErrorRes("不能删除目录"));
                }
            }
        }

        #endregion

        #region 文件
        public IActionResult FilesList([FromQuery] string d, [FromQuery] string type) 
        {
            CheckPath(d);
            string fullPath = FixPath(d);
            List<string> files = GetFiles(fullPath, type);
            var _r = new StringBuilder();
            _r.Append("[");
            for(int i = 0; i < files.Count; i++){
                FileInfo f = new FileInfo(files[i]);
                int w = 0, h = 0;
                if (GetFileType(f.Extension) == "image"){
                    try{
                        FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read);
                        Image img = Image.FromStream(fs);
                        w = img.Width;
                        h = img.Height;
                        fs.Close();
                        fs.Dispose();
                        img.Dispose();
                    }
                    catch(Exception ex){throw ex;}
                }
                _r.Append("{");
                _r.Append("\"p\":\""+d + "/" + f.Name+"\"");
                _r.Append(",\"t\":\"" + Math.Ceiling(LinuxTimestamp(f.LastWriteTime)).ToString() + "\"");
                _r.Append(",\"s\":\""+f.Length.ToString()+"\"");
                _r.Append(",\"w\":\""+w.ToString()+"\"");
                _r.Append(",\"h\":\""+h.ToString()+"\"");
                _r.Append("}");
                if (i < files.Count - 1)
                    _r.Append(",");
            }
            _r.Append("]");
            return Content(_r.ToString());
        }

        public ActionResult GenerateThumb(string f,int width, int height)
        {
            CheckPath(f);
            FileStream fs = new FileStream(FixPath(f), FileMode.Open, FileAccess.Read);
            Bitmap img = new Bitmap(Bitmap.FromStream(fs));
            fs.Close();
            fs.Dispose();
            int cropWidth = img.Width, cropHeight = img.Height;
            int cropX = 0, cropY = 0;

            double imgRatio = (double)img.Width / (double)img.Height;

            if(height == 0)
                height = Convert.ToInt32(Math.Floor((double)width / imgRatio));

            if (width > img.Width)
                width = img.Width;
            if (height > img.Height)
                height = img.Height;

            double cropRatio = (double)width / (double)height;
            cropWidth = Convert.ToInt32(Math.Floor((double)img.Height * cropRatio));
            cropHeight = Convert.ToInt32(Math.Floor((double)cropWidth / cropRatio));
            if (cropWidth > img.Width)
            {
                cropWidth = img.Width;
                cropHeight = Convert.ToInt32(Math.Floor((double)cropWidth / cropRatio));
            }
            if (cropHeight > img.Height)
            {
                cropHeight = img.Height;
                cropWidth = Convert.ToInt32(Math.Floor((double)cropHeight * cropRatio));
            }
            if(cropWidth < img.Width){
                cropX = Convert.ToInt32(Math.Floor((double)(img.Width - cropWidth) / 2));
            }
            if(cropHeight < img.Height){
                cropY = Convert.ToInt32(Math.Floor((double)(img.Height - cropHeight) / 2));
            }

            Rectangle area = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropImg = img.Clone(area, System.DrawingCore.Imaging.PixelFormat.DontCare);
            img.Dispose();
            Image.GetThumbnailImageAbort imgCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            
            var thumbImg = cropImg.GetThumbnailImage(width, height, imgCallback, IntPtr.Zero);
            cropImg.Dispose();
            using (var memStream = new MemoryStream())
            {
                thumbImg.Save(memStream, ImageFormat.Png);
                var data = memStream.ToArray();
                return File(data, @"image/png");
            }
        }
        private bool ThumbnailCallback()
        {
            return false;
        }

        public ActionResult RenameFile(string f, string n)
        {
            CheckPath(f);
            FileInfo source = new FileInfo(FixPath(f));
            FileInfo dest = new FileInfo(Path.Combine(source.Directory.FullName, n));
            if (!source.Exists)
                throw new Exception(LangRes("E_RenameFileInvalidPath"));
            else if (!CanHandleFile(n))
                throw new Exception(LangRes("E_FileExtensionForbidden"));
            else
            {
                try
                {
                    source.MoveTo(dest.FullName);
                    return Content(GetSuccessRes());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "; " + LangRes("E_RenameFile") + " \"" + f + "\"");
                }
            }
        }

        public ActionResult Download(string f)
        {
            CheckPath(f);
            FileInfo file = new FileInfo(FixPath(f));
            return PhysicalFile(file.FullName,@"application/force-download",file.Name);
        }
        public ActionResult DeleteFile(string f)
        {
            CheckPath(f);
            f = FixPath(f);
            if (!System.IO.File.Exists(f))
                throw new Exception(LangRes("E_DeleteFileInvalidPath"));
            else
            {
                try
                {
                    System.IO.File.Delete(f);
                    return Content(GetSuccessRes());
                }
                catch (Exception ex)
                {
                    throw new Exception(LangRes("E_DeletеFile"));
                }
            }
        }
        #endregion

        #region 上传
        public class UploadModel
        {
            //public IFormFile[] Files { get; set; }
            public string d { get; set; }
            public string method { get; set; }
        }

        public ActionResult Upload(UploadModel model)
        { 
            CheckPath(model.d);
            var path = FixPath(model.d);
            string res = GetSuccessRes();
            bool hasErrors = false;
            try{
                for(int i = 0; i < Request.Form.Files.Count; i++){
                    if (CanHandleFile(Request.Form.Files[i].FileName))
                    {
                        FileInfo f = new FileInfo(Request.Form.Files[i].FileName);
                        string filename = MakeUniqueFilename(path, f.Name);
                        string dest = Path.Combine(path, filename);
                        //model.Files[i].SaveAs(dest);
                        using (var stream = new FileStream(dest,FileMode.CreateNew))
                        {
                            Request.Form.Files[i].CopyTo(stream);
                        }
                        if (GetFileType(new FileInfo(filename).Extension) == "image")
                        {
                            int w = 0;
                            int h = 0;
                            int.TryParse(_uploadConfig.MAX_IMAGE_WIDTH, out w);
                            int.TryParse(_uploadConfig.MAX_IMAGE_HEIGHT, out h);
                            ImageResize(dest, dest, w, h);
                        }
                    }
                    else
                    {
                        hasErrors = true;
                        res = GetSuccessRes(LangRes("E_UploadNotAll"));
                    }
                }
            }
            catch(Exception ex){
                res = GetErrorRes(ex.Message);
            }
            var _r = new StringBuilder();
            if (model.method.Equals("ajax", StringComparison.OrdinalIgnoreCase))
            {
                if(hasErrors)
                    res = GetErrorRes(LangRes("E_UploadNotAll"));
                _r.Append(res);
            }
            else
            {
                _r.Append("<script>");
                _r.Append("parent.fileUploaded(" + res + ");");
                _r.Append("</script>");
            }

            return Content(_r.ToString());
        }


        #endregion
    }
}