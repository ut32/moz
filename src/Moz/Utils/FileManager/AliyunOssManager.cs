using System;
using System.IO;
using System.Net;
using Aliyun.OSS;

namespace Moz.Utils.FileManager
{
    
    internal class AliyunOssManager : IFileManager
    { 
        private readonly string _accessKeyId;
        private readonly string _accessKeySecret;
        private readonly string _bucketName;
        private readonly string _endPoint;

        internal AliyunOssManager() 
        {
            _bucketName = "miya-mp-instagram.oss-cn-hongkong-internal.aliyuncs.com";
            _endPoint = "oss-cn-hongkong-internal.aliyuncs.com";
            _accessKeyId = "KUOieTEDteF4HTZo";
            _accessKeySecret = "Dn8Q3niUOX152xt9RVSDvm9ipcAbfV";
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public UploadResult Upload(UploadFile file)
        {
            var result = new UploadResult();
            var metadata = new ObjectMetadata
            {
                ContentType = file.ContentType
            };
            var client = new OssClient(_endPoint, _accessKeyId, _accessKeySecret);
            try
            {
                var prefix = "files";
                var contentType = file.ContentType.ToLower();
                if (contentType.Contains("image")
                    || contentType.Contains("gif")
                    || contentType.Contains("png")
                    || contentType.Contains("jpg")
                    || contentType.Contains("jpeg"))
                    prefix = "images";
                if (contentType.Contains("video")
                    || contentType.Contains("mp4"))
                    prefix = "videos";
                if (contentType.Contains("audio")
                    || contentType.Contains("mp3"))
                    prefix = "audios";
                var dt = DateTime.Now;
                var newFilename = $"{prefix}/{dt.Year}/{dt.Month}/{Guid.NewGuid():N}{file.FileName}";

                using (var stream = new MemoryStream(file.Data))
                {
                    var putResult = client.PutObject(_bucketName, newFilename, stream, metadata);
                    if (putResult.HttpStatusCode == HttpStatusCode.Continue)
                    //{
                    //}

                    result.RelativePath = newFilename;
                    //result.Server = _server;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Error = e.Message;
            }

            return result;
        }
    }
}