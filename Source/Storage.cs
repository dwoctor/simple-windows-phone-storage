using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Windows.Media.Imaging;

namespace SimpleWindowsPhoneStorage
{
    /// <summary>
    /// Interact with the phone's Local Storage
    /// </summary>
    public class Storage
    {
        /// <summary>
        /// Loads binary data from Phone
        /// </summary>
        /// <param name="path">Where the file is loacated</param>
        /// <returns>Binary</returns>
        public static Byte[] LoadBinary(String path)
        {
            if (Storage.SimpleMethods.FileExists(path))
            {
                return Storage.SimpleMethods.LoadBinaryFile(path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Saves binary data to Phone
        /// </summary>
        /// <param name="data">The file's data</param>
        /// <param name="path">Where to save the file</param>
        public static void SaveBinary(Byte[] data, String path)
        {

            if (Storage.SimpleMethods.DirectoryExists(path) == false)
            {
                Storage.SimpleMethods.CreateDirectory(path);
            }
            if (Storage.SimpleMethods.FileExists(path) == false)
            {
                Storage.SimpleMethods.SaveNewBinaryFile(data, path);
            }
            else
            {
                Storage.SimpleMethods.ReplaceExistingBinaryFile(data, path);
            }
        }

        /// <summary>
        /// Loads image data from Phone
        /// </summary>
        /// <param name="path">Where the file is loacated</param>
        /// <returns>Image</returns>
        public static BitmapImage LoadImage(String path)
        {
            if (Storage.SimpleMethods.FileExists(path))
            {
                return Storage.SimpleMethods.LoadImageFile(path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Loads xml data from Phone
        /// </summary>
        /// <param name="path">Where the file is loacated</param>
        /// <returns>XML</returns>
        public static XDocument LoadXML(String path)
        {
            if (Storage.SimpleMethods.FileExists(path))
            {
                return Storage.SimpleMethods.LoadXMLFile(path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Saves Data to Phone
        /// </summary>
        /// <param name="xml">The XML File</param>
        /// <param name="path">Where to save the file</param>
        public static void SaveXML(XDocument xml, String path)
        {
            
            if (Storage.SimpleMethods.DirectoryExists(path) == false)
            {
                Storage.SimpleMethods.CreateDirectory(path);
            }
            if (Storage.SimpleMethods.FileExists(path) == false)
            {
                Storage.SimpleMethods.SaveNewXMLFile(xml, path);
            }
            else
            {
                Storage.SimpleMethods.ReplaceExistingXMLFile(xml, path);
            }
        }

        /// <summary>
        /// Basic core functions of phone storage to perform singular tasks (NO ERROR HANDLING IMPLEMENTED)
        /// </summary>
        public class SimpleMethods
        {
            #region Directories
            /// <summary>
            /// Gets all the names of directories on the phone
            /// </summary>
            public static String[] GetDirectories() { return GetDirectories("*"); }

            /// <summary>
            /// Gets the names of directorieson the phone
            /// </summary>
            /// <param name="searchPattern">A search pattern. Both single-character ("?") and multi-character ("*") wildcards are supported.</param>
            public static String[] GetDirectories(String searchPattern)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return isoStore.GetDirectoryNames(searchPattern);
                }
            }

            /// <summary>
            /// Creates a new directory on the phone
            /// </summary>
            /// <param name="path">Name of the directory to create</param>
            public static void CreateDirectory(String name)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (name.EndsWith("/") == false)
                    {
                        isoStore.CreateDirectory(name.Substring(0, name.LastIndexOf('/')));
                    }
                    else
                    {
                        isoStore.CreateDirectory(name);
                    }
                }
            }

            /// <summary>
            /// Does the directory exist on the phone
            /// </summary>
            /// <param name="path">Name of the directory to find</param>
            public static Boolean DirectoryExists(String name)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (name.EndsWith("/") == false)
                    {
                        return isoStore.DirectoryExists(name.Substring(0, name.LastIndexOf('/')));
                    }
                    else
                    {
                        return isoStore.DirectoryExists(name);
                    }
                }
            }

            /// <summary>
            /// Deletes a directory on the phone
            /// </summary>
            /// <param name="directory">The directory to be deleted</param>
            public static void DeleteDirectory(String directory)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    isoStore.DeleteDirectory(directory);
                }
            }
            #endregion

            #region Files
            /// <summary>
            /// Gets all the names of files on the phone
            /// </summary>
            public static String[] GetFilesNames() { return GetFilesNames("*"); }

            /// <summary>
            /// Gets the names of files on the phone
            /// </summary>
            /// <param name="searchPattern">A search pattern. Both single-character ("?") and multi-character ("*") wildcards are supported.</param>
            public static String[] GetFilesNames(String searchPattern)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return isoStore.GetFileNames(searchPattern);
                }
            }

            /// <summary>
            /// Does the file exist on the phone
            /// </summary>
            /// <param name="path">Name of the file to find</param>
            public static Boolean FileExists(String name)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return isoStore.FileExists(name);
                }
            }

            /// <summary>
            /// Deletes a file on the phone
            /// </summary>
            /// <param name="path">The file to be deleted</param>
            public static void DeleteFile(String path)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    isoStore.DeleteFile(path);
                }
            }
            #endregion

            #region Binary
            /// <summary>
            /// Load binary file from the Phone
            /// </summary>
            /// <param name="fileName">The name of file to load</param>
            /// <returns>Binary</returns>
            public static Byte[] LoadBinaryFile(String fileName)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read, isoStore))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(stream))
                        {
                            Int64 totalBytes = stream.Length;
                            return binaryReader.ReadBytes(Convert.ToInt32(totalBytes));
                        }
                    }
                }
            }

            /// <summary>
            /// Replace existing binary file on the phone
            /// </summary>
            /// <param name="data">The file's data</param>
            /// <param name="path">Where to save the file</param>
            public static void ReplaceExistingBinaryFile(Byte[] data, String path)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.Write, isoStore))
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }

            /// <summary>
            /// Saves new binary file to the phone
            /// </summary>
            /// <param name="data">The file's data</param>
            /// <param name="path">Where to save the file</param>
            public static void SaveNewBinaryFile(Byte[] data, String path)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileMode.CreateNew, FileAccess.Write, isoStore))
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            #endregion

            #region Image
            /// <summary>
            /// Load image file from phone
            /// </summary>
            /// <param name="path">Where the file is loacated</param>
            public static BitmapImage LoadImageFile(String path)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, isoStore))
                    {
                        BitmapImage image = new BitmapImage();
                        image.SetSource(stream);
                        return image;
                    }
                }
            }
            #endregion

            #region XML
            /// <summary>
            /// Load xml file from Phone
            /// </summary>
            /// <param name="path">Where the file is loacated</param>
            public static XDocument LoadXMLFile(String path)
            {
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, isoStore))
                    {
                        return XDocument.Load(stream);
                    }
                }
            }

            /// <summary>
            /// Replace existing xml file on the phone
            /// </summary>
            /// <param name="data">The xml data</param>
            /// <param name="path">Where to save the file</param>
            public static void ReplaceExistingXMLFile(XDocument data, String path)
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.Write, store))
                    {
                        data.Save(stream);
                    }
                }
            }

            /// <summary>
            /// Saves new xml file to the Phone
            /// </summary>
            /// <param name="data">The xml data</param>
            /// <param name="path">Where to save the file</param>
            public static void SaveNewXMLFile(XDocument data, String path)
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileMode.CreateNew, FileAccess.Write, store))
                    {
                        data.Save(stream);
                    }
                }
            }
            #endregion
        }
    }
}