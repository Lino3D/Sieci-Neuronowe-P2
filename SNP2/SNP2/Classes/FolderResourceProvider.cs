using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SNP2.Interfaces
{
    public class FolderResourceProvider
    {
        private string FirstFolderPath = "./Resource /";
        private string SecondFolderPath = String.Empty;

        public List<string> FirstFolderDocuments;
        public List<string> SecondFolderDocuments;

        public FolderResourceProvider(string firstFolderPath, string secondFolderPath)
        {
            FirstFolderPath = firstFolderPath;
            SecondFolderPath = secondFolderPath;
            FirstFolderDocuments = new List<string>();
            SecondFolderDocuments = new List<string>();

            try
            {
                foreach (string fileName in Directory.GetFiles(FirstFolderPath))
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        FirstFolderDocuments.Add(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                
               // throw;
            }
            try
            {
                foreach (string fileName in Directory.GetFiles(SecondFolderPath))
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        SecondFolderDocuments.Add(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception  e)
            {

                //throw new DirectoryNotFoundException(e.Message);
            }
         

        }
    }
}
