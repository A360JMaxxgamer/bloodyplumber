using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace BloodyPlumber
{
    public class IsolatedStorageFile : IDisposable
    {
        private readonly string gamepath;

        private IsolatedStorageFile()
        {
            gamepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BloodyPlumber");
        }

        static public IsolatedStorageFile GetUserStoreForApplication()
        {
            return new IsolatedStorageFile();
        }

        public void Dispose()
        {
        }

        public void CreateDirectory(string dir)
        {
            Directory.CreateDirectory(gamepath);
        }

        public IsolatedStorageFileStream CreateFile(string path)
        {
            return OpenFile(path, FileMode.Create);
        }

        public IsolatedStorageFileStream OpenFile(string path, FileMode mode)
        {
            switch (mode)
            {
                case FileMode.Create:
                    return OpenFile(path, mode, FileAccess.Write);
                case FileMode.Open:
                    return OpenFile(path, mode, FileAccess.Read);
            }
            return null;
        }

        public IsolatedStorageFileStream OpenFile(string path, FileMode mode, FileAccess access)
        {
            Stream stream = null;
            switch (mode)
            {
                case FileMode.Create:
                    stream = Task.Run(
                        () =>
                        {
                            try
                            {
                                return new FileStream(Path.Combine(gamepath, path), FileMode.Create);
                            }
                            catch (IOException e)
                            {
                                throw new IsolatedStorageException(e.Message, e);
                            }
                        }).Result;
                    break;

                case FileMode.Open:
                    stream = Task.Run(
                        () =>
                        {
                            try
                            {
                                return new FileStream(Path.Combine(gamepath, path), FileMode.Open);
                            }
                            catch (IOException e)
                            {
                                throw new IsolatedStorageException(e.Message, e);
                            }
                        }).Result;
                    break;
            }
            return new IsolatedStorageFileStream(stream);
        }

        public bool DirectoryExists(string directoryName)
        {
            return Directory.Exists(Path.Combine(gamepath, directoryName));
        }

        public bool FileExists(string fileName)
        {
            return File.Exists(Path.Combine(gamepath, fileName));
        }

        public string[] GetFileNames()
        {
            return GetFileNames("*");
        }

        public string[] GetFileNames(string path)
        {
            return Directory.GetFiles(Path.Combine(gamepath, path)).ToArray();
        }

        public string[] GetDirectoryNames()
        {
            return GetDirectoryNames("*");
        }

        public string[] GetDirectoryNames(string path)
        {
            return Directory.GetDirectories(Path.Combine(gamepath, path)).ToArray();
        }

        public void DeleteDirectory(string path)
        {
            Directory.Delete(Path.Combine(gamepath, path));
        }

        public void DeleteFile(string path)
        {
            File.Delete(Path.Combine(gamepath, path));
        }
    }
}
