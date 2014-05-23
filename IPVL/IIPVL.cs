using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPVL
{
    interface IIPVL
    {
        bool ValidateEmail(string inputEmail);
        bool ValidateDate(string inputDateTime);
        bool ValidateString(string inputString);
        bool ValidateFileUpload(string inputFilePath, int maxAllowedFileSize);        
    }
}
