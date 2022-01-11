using System;
using System.IO;
using System.Text;

namespace SSDLMaintenanceTool.Helpers
{
    public class CSVHelper
    {
        public CSVHelper()
        {

        }

        public bool IsFileValid(Stream stream, long contentLength, int scanLength = 4096)
        {
            //Attempt 1 and 1 use same approach. Only difference is that Attempt 2 keeps reading larger blocks till the end of file until non-readable text is found.

            var textFileContentValidator = new TextFileContentValidator();

            #region Attempt 1
            //return textFileContentValidator.IsUtf8(stream);
            #endregion

            #region attempt 2
            return textFileContentValidator.CheckStreamFirstBlockWithoutParsing(stream, contentLength, scanLength);
            #endregion
        }

        public Encoding GetEncoding(string encodingString)
        {
            Encoding enc = null;
            try
            {
                enc = Encoding.GetEncoding(encodingString);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Invalid encoding:" + encodingString, ex);
            }
            return enc;
        }
    }
}
