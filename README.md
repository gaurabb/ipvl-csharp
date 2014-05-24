ipvl
====
[Still Work In Progress]
Input Validation Library - C#

How Magic Number Checks done?

1) The code will first check for the extension of the uploaded file.
2) If the extension is in the whitelist then the magic number check will be performed.
3) The code will read the magic number value of the uploaded file's type based on the extension.
4) If the magic number is valid then the file is marked as a valid file.

The magicnumbers.txt file in the root folder is used to obtain the magic numbers for validation.
When the IPVL is invoked, the constructor of the IPVL class will check for he magicnumbers.txt file in the project root. If found, the values obtained in this file will be used. If the file does not exist, then a new file will be created and the default set of magic numbers will be written into this file and used for validation.

If new file types needs to be supported then the magic number values can be added to the magicnumbers.txt file in the following format, one key value per line:
.<filetype>=<magic#Value>
Eg.
.bmp=42-4D


