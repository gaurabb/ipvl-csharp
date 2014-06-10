using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net.Mime;


namespace IPVL
{
    /// <summary>
    /// IPVL: Input Validation Library    
    /// </summary>
    public class IPVL : IIPVL
    {

        #region Global Variable Declaration/Definitions
        bool GLOBAL_RESULTS = false;
        //Default Log file path. If it does not exist then the constructor will create the file. ToDo: A better path will work!
        string GLOBAL_LOGFILEPATH = @"Errors.txt";
        string[] DEFAULT_MAGIC_NUMBERS = { ".bmp=42-4D", ".jpg=FF-D8", ".doc=D0-CF-11-E0-A1-B1-1A-E1", ".pdf=25-50-44-46", ".docx=50-4B-03-04" };
        string[] INITIALIZE_VALID_FILE_EXTENSIONS = { ".xls", ".pdf", ".gif", ".tif", ".bmp", ".jpg", ".docx" }; //ToDo: Initialize from the configuration file
        int MAX_ALLOWED_FILE_SIZE = 1813345; //Donot have to move it to the configuration file as the user has the option to pass the file size to the method call
        #endregion

        #region Constructor Function
        /// <summary>
        /// Constructor Method
        /// </summary>
        public IPVL()
        {
            /*Creates a log file  to write all the statuses to in Application Startup path*/
            if (!File.Exists("Errors.txt"))
            {
                try
                {
                    File.Create(@"Errors.txt").Close();
                    WriteToErrorLogFile("Log file created on: " + DateTime.Now.ToString() + ".");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /*Create a Configuration file if one does not exist in the root
             * This file will contain the values that will need to be configurable like maximum allowed file size and file extensions
             */
            if (!File.Exists("IPVLConfig.txt"))
            {
                try
                {
                    File.Create(@"IPVLConfig.txt").Close();
                    WriteToErrorLogFile("IPVL's Configuration file created on: " + DateTime.Now.ToString() + ".");
                    //Write the basic configuration settings into the configuration file
                    using (StreamWriter txIPVLConfigWriter = new StreamWriter("IPVLConfig.txt"))
                    {
                        foreach (string EXT in INITIALIZE_VALID_FILE_EXTENSIONS)
                        {
                            txIPVLConfigWriter.Write(EXT+",");
                        }                       
                    }
                }
                catch(Exception ex)
                {
                    WriteToErrorLogFile(ex.ToString());
                }
            }

            
            /*Checks for magicnumbers.txt file in the root; creates one if not found with the default set of extension - magic number mappings */
            if (!File.Exists("magicnumbers.txt"))
            {
                try
                {
                    File.Create(@"magicnumbers.txt").Close();
                    WriteToErrorLogFile("Default Magic Numbers file created on: " + DateTime.Now.ToString() + ".");
                    //Write the default File Type and Magic Numbers into the text file
                    using (StreamWriter txtMagicNumWriter = new StreamWriter("magicnumbers.txt"))
                    {
                        for (int i = 0; i < DEFAULT_MAGIC_NUMBERS.Length; i++)
                        {
                            txtMagicNumWriter.WriteLine(DEFAULT_MAGIC_NUMBERS[i]);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    WriteToErrorLogFile(ex.ToString());
                }
            }
        }

        #endregion

        #region Email Validation Code
        /// <summary>
        ///  Validates email
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public bool ValidateEmail(string inputEmail)
        {
            return GLOBAL_RESULTS;
        }

        #endregion

        #region Date Validation Code
        /// <summary>
        /// Validates Date Time
        /// </summary>
        /// <param name="inputDateTime"></param>
        /// <returns></returns>
        public bool ValidateDate(string inputDateTime)
        {
            return GLOBAL_RESULTS;
        }

        #endregion

        #region Validate Input String
        /// <summary>
        /// Validate Input String
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public bool ValidateString(string inputString)
        {
            return GLOBAL_RESULTS;
        }

        #endregion

        #region Validate Uploaded File
        /// <summary>
        /// Validate the uploaded file
        /// Performs following validations: 
        /// 1) The file exists 
        /// 2) File Size
        /// 3) File Extension
        /// 4) File Content
        /// </summary>
        /// <param name="inputFilePath">Path of the file to validate</param>
        /// <param name="maxAllowedFileSize">Maximum allowed file size</param>
        /// <returns></returns>
        public bool ValidateFileUpload(string inputFilePath, int maxAllowedFileSize)
        {
            #region Local Variables Declarations/Definitions
            /* Local Variable Declarations */
            //int MAX_ALLOWED_FILE_SIZE = 1813345; //ToDo: Initialize from the configuration file
            FileInfo infoFileToValidate;            
            #endregion

            /*Log the input path provided by the end user*/
            WriteToErrorLogFile("************************ " + DateTime.Now.ToString() + " ************************");
            WriteToErrorLogFile("The file path provided to the validator: " + inputFilePath);
            
            #region Validate File Path

            /* Validate the file to valiate filepath */
            if (!File.Exists(inputFilePath))
            {
                WriteToErrorLogFile("The file does not exits!");
                return false;  
            }
            else
            {
                infoFileToValidate = new FileInfo(inputFilePath);
            }

            #endregion

            #region Validate File Size
            /*
             * VALIDATE FILE SIZE IS WITHIN LIMITS
             * Step 1: Check if the caller has supplied the MAX file size; If not use the default value from the configuration file
             * Step 2: Validate the file size [* ToDo: Check for file size on the disk as well]
             */           
            try
            {
                if (maxAllowedFileSize != 0)
                {
                    MAX_ALLOWED_FILE_SIZE = maxAllowedFileSize;
                }

                if (infoFileToValidate.Length <= MAX_ALLOWED_FILE_SIZE)
                {
                    WriteToErrorLogFile("The file size is valid. The size of the file in BYTES is: " + infoFileToValidate.Length.ToString());
                    GLOBAL_RESULTS = true;
                }
                else
                {
                    WriteToErrorLogFile("The file size is invalid/out of bounds for this functionality. The size of the file in BYTES is: " + infoFileToValidate.Length.ToString());
                    return false;
                }
            }
            catch(Exception ex)
            {
                WriteToErrorLogFile(ex.Message.ToString());
            }
            #endregion

            #region Validate File Extension
            /*
             * VALIDATE FILE EXTENSION
             */     

            #region InLine Extension Validation Code
            //Read the allowed extensions from the IPVLConfiguration file
            string Extentions = File.ReadAllText("IPVLConfig.txt");
            string[] VALID_FILE_EXTENSIONS = Extentions.Split(',');
            if (Array.IndexOf(VALID_FILE_EXTENSIONS, infoFileToValidate.Extension.ToString()) != -1)
            {
                WriteToErrorLogFile("The file has a valid extension. The extension of the file is: " + infoFileToValidate.Extension.ToString());
                GLOBAL_RESULTS = true;
            }
            else
            {
                WriteToErrorLogFile("The file has an invalid extension that is not allowed. The extension of the file is: " + infoFileToValidate.Extension.ToString());
                return false;
            }

            #endregion

            #endregion

            #region Validate File Content - Magic Numbers

            WriteToErrorLogFile("------------STARTING THE MAGIC NUMBER CHECK----------------");
            
            var expectedMaginNumber = File
            .ReadAllLines("magicnumbers.txt")
            .Select(x => x.Split('='))
            .Where(x => x.Length > 1)
            .ToDictionary(x => x[0].Trim(), x => x[1]);
           
            try
            {
                WriteToErrorLogFile("Read the first 10 bytes from the file to validate");
                byte[] first10Bytes = new byte[10];
                using (BinaryReader reader = new BinaryReader(new FileStream(inputFilePath, FileMode.Open)))
                {
                    reader.Read(first10Bytes, 0, 10);
                }
                
                WriteToErrorLogFile("Done. First 10 BYTES of upload file look like: ");
                WriteToErrorLogFile(BitConverter.ToString(first10Bytes));
                
                if (BitConverter.ToString(first10Bytes).StartsWith(expectedMaginNumber[infoFileToValidate.Extension.ToString()]))
                {
                    WriteToErrorLogFile("The file content matches the content signature expected. The file is valid.");
                    GLOBAL_RESULTS = true;
                }
                else
                {
                    WriteToErrorLogFile("The file content does not match the content signature expected. The file is possibly invalid.");
                    GLOBAL_RESULTS = false;
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLogFile("There is an error determining the ContentType of the file being validated: " + ex.Message.ToString());
                GLOBAL_RESULTS = false;
            }

            #endregion

            return GLOBAL_RESULTS;
        }

        #endregion

        #region Helper Method to write to log file

        /// <summary>
        /// Method to write messages to the log file
        /// </summary>
        /// <param name="errorMessage"></param>
        public void WriteToErrorLogFile(string logMessage)
        {
            try
            {
                using (StreamWriter txtWriter = File.AppendText(GLOBAL_LOGFILEPATH))
                {
                    txtWriter.WriteLine(@logMessage);
                }                
            }
            catch (Exception ex)
            {
                WriteToErrorLogFile(ex.Message.ToString());
            }
        }
        #endregion

    }
}
