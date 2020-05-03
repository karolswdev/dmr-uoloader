using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using UOLoader.Contract;
using UOLoader.Helpers;
using Constants = UOLoader.Contract.Constants;

namespace UOLoader.Entities
{
    /// <summary>
    /// Object representing a cache used for tracking which update files have been downloaded client-side already
    /// </summary>
    public class FileCache
    {
        /// <summary>
        /// All the actual files stored
        /// </summary>
        public IList<FileCacheEntry> SavedFiles { get; set; }
        /// <summary>
        /// Whether UO - the base package has been saved already
        /// </summary>
        public bool UoSaved { get; set; }
        public FileCache() {
            SavedFiles = new List<FileCacheEntry>();
        }

        /// <summary>
        /// Returns an instance of FileCache depending on whether it was loaded from settings or not
        /// </summary>
        /// <param name="cachePath">The optional path to the cache file</param>
        /// <returns>An instance of <see cref="FileCache"/></returns>
        public static FileCache Load(string cachePath = Constants.UoLoaderDefaultCacheName) {
            if (!File.Exists(cachePath)) {
                return GetDefaultFileCache();
            }

            var loadObject = File.ReadAllText(cachePath);

            var settingsObject = JsonConvert.DeserializeObject<FileCache>(loadObject);

            if (settingsObject == null) {
                ConsoleHelper.WriteLine("Corrupt Cache File, recreating!");
                settingsObject = GetDefaultFileCache();
                settingsObject.Save(cachePath);
            }

            return settingsObject;
        }

        /// <summary>
        /// Has this file been downloaded already
        /// </summary>
        /// <param name="file">The file to check against</param>
        /// <returns>true if downloaded already</returns>
        public bool HasFile(UpdateFileInfo file) {
            return SavedFiles.Any(f => f.Name == file.Name && f.Revision == file.Revision);
        }

        /// <summary>
        /// Projects the UpdateFileInfo into a <see cref="FileCacheEntry"/>
        /// </summary>
        /// <param name="file">The <see cref="UpdateFileInfo"/> received from remote</param>
        /// <param name="fileName">Filename stored on local FS</param>
        /// <returns>true when added</returns>
        public bool AddFile(UpdateFileInfo file, string fileName) {

            if (SavedFiles == null) {
                SavedFiles = new List<FileCacheEntry>();
            }


            SavedFiles.Add(new FileCacheEntry() {
                FileName = fileName,
                Name = file.Name,
                Revision = file.Revision
            });

            return true;

        }

        public bool Save(string cachePath = Constants.UoLoaderDefaultCacheName) {

            var saveObject = JsonConvert.SerializeObject(this);
            try {
                File.WriteAllText(cachePath, saveObject);
                return true;
            }
            // Bad suppression, ad-hoc code
            catch (Exception ex) {
                return false;
            }
        }


        public static FileCache GetDefaultFileCache() {
            return new FileCache() {
                UoSaved = false
            };
        }
    }
}
