using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FSExplorer.Controllers
{
    public class FileSystemController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        [Route("api/FileSystem/GetDrives")]
        public IEnumerable<dynamic> GetDrives()
        {
            string [] driveList = Directory.GetLogicalDrives();
            List<DriveInfo> drives = driveList.Select(a => new DriveInfo(a)).ToList();
            return drives.Select(a => new { a.Name , Label = a.VolumeLabel, FullPath = a.Name, HasChildFolders = HasChildFolders(a.Name), IsDirectory = true});
        }

        [HttpGet]
        [Route("api/FileSystem/GetSubFolders")]
        public IEnumerable<dynamic> GetSubFolders(string path)
        {

            string[] dirList = Directory.GetDirectories(path);
            List<DirectoryInfo> directories = dirList.Select(a => new DirectoryInfo(a)).ToList();
            return directories.Where(a => !a.Attributes.HasFlag(FileAttributes.System) & !a.Attributes.HasFlag(FileAttributes.Hidden)).Select(a => new { a.Name, FullPath = a.FullName, Label = (string)null, HasChildFolders = HasChildFolders(a.FullName), IsDirectory = true, FileSize = (string)null });
        }

        [HttpGet]
        [Route("api/FileSystem/GetFolderContent")]
        public IEnumerable<dynamic> GetFolderContent(string path)
        {

            string[] dirList = Directory.GetDirectories(path);
            string[] fileList = Directory.GetFiles(path);
            List<DirectoryInfo> directories = dirList.Select(a => new DirectoryInfo(a)).ToList();
            List<FileInfo> files = fileList.Select(a => new FileInfo(a)).ToList();
            return directories.Where(a => !a.Attributes.HasFlag(FileAttributes.System) & !a.Attributes.HasFlag(FileAttributes.Hidden)).Select(a => new { a.Name, FullPath = a.FullName, Label = (string)null, HasChildFolders = HasChildFolders(a.FullName), IsDirectory = true, FileSize=(string)null })
                .Union(files.Where(a => !a.Attributes.HasFlag(FileAttributes.System) & !a.Attributes.HasFlag(FileAttributes.Hidden)).Select(a => new { a.Name, FullPath = a.FullName, Label = (string)null, HasChildFolders = HasChildFolders(a.FullName), IsDirectory = false, FileSize = CalulateFileSize(a) }));
        }

        private string CalulateFileSize(FileInfo fi)
        {
            long kbSize = fi.Length / (long)1024;
            return kbSize.ToString("N0") + "KB";
        }
        private static bool HasChildFolders(string path)
        {
            try
            {
                return Directory.GetDirectories(path).Length > 0 ? true : false;
            }
            catch { return false; }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
