using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LearnNewWord.Managers.Extension
{
    public static class FileExtension
    {
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="pathFile">The path file.</param>
        /// <returns></returns>
        public static bool DeleteFile(string pathFile)
        {
            try
            {
                if (!File.Exists(pathFile)) return false;

                File.Delete(pathFile);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathFile">The path file.</param>
        /// <returns></returns>
        /// <exception cref="FileFormatException"></exception>
        public static List<T> ReadFile<T>(string pathFile)
        {
            try
            {
                var strJson = File.ReadAllText(pathFile);

                if (string.IsNullOrEmpty(strJson)) 
                    throw new FileFormatException();

                return JsonConvert.DeserializeObject<List<T>>(strJson);
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
            
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool WriteFile<T>(IList<T> data, string path)
        {
            try
            {
                var strJson = JsonConvert.SerializeObject(data);
                File.WriteAllText(path, strJson);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Files the dialog.
        /// </summary>
        /// <param name="Filter">The filter.</param>
        /// <returns></returns>
        public static string FileDialog(string Filter)
        {
            string result = null;

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {

                fileDialog.Filter = Filter;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    result = fileDialog.FileName;
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="saveDirectory">The save directory.</param>
        /// <param name="pathFileName">Name of the path file.</param>
        /// <returns></returns>
        public static string SaveFile(string saveDirectory, string pathFileName)
        {
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string fileName = Path.GetFileName(pathFileName);
            string fileSavePath = Path.Combine(saveDirectory, fileName);
            File.Copy(pathFileName, fileSavePath, true);

            return fileName;
        }

        /// <summary>
        /// Sizes the file information.
        /// </summary>
        /// <param name="fileNameDialog">The file name dialog.</param>
        /// <returns></returns>
        public static long SizeFileInfo(string fileNameDialog)
        {
            string sizeFileInfo = new FileInfo(fileNameDialog).Length.ToString();
            MessageBox.Show(sizeFileInfo);
            long size = Convert.ToInt64(sizeFileInfo);
            return size;
        }
    }

    public static class FileUploadFilter
    {
        public static string Image = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
    }
}