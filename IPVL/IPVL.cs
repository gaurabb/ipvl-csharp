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

        #endregion

        #region Constructor Function
        /// <summary>
        /// Constructor Method
        /// </summary>
        public IPVL()
        {
            /*Creates a log file  to write all he statuses to in Application Startup path*/
            if (!File.Exists("Errors.txt"))
            {
                try
                {
                    File.Create(@"Errors.txt");
                    WriteToErrorLogFile("Log file created on: " + DateTime.Now.ToString() + ". This file to be used if the invoking app does not provide a different error log");
                }
                catch (Exception ex)
                {
                    throw ex;
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
            int MAX_ALLOWED_FILE_SIZE = 1813345; //ToDo: Initialize from the configuration file
            string[] VALID_FILE_EXTENSIONS = { ".xls", ".pdf", ".txt", ".php", ".gif", ".tif", ".bmp", ".jpg" }; //ToDo: Initialize from the configuration file
            
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
            try
            {
                //1. Read the first 10 bytes from the file
                byte[] testByte = new byte[10];
                using (BinaryReader reader = new BinaryReader(new FileStream(inputFilePath, FileMode.Open)))
                {
                    reader.Read(testByte, 0, 10);
                }
               // File.WriteAllBytes(GLOBAL_LOGFILEPATH, testByte);
                WriteToErrorLogFile(BitConverter.ToString(testByte).ToString());
            }
            catch(Exception ex)
            {
                WriteToErrorLogFile("There is an error determining the ContentType of the file being validated: " + ex.Message.ToString());
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
