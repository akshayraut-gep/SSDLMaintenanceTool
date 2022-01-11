using Aspose.Cells;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDLMaintenanceTool.Helpers
{
    public class ExcelHelper
    {
        public ExcelHelper()
        {

        }

        public async Task<bool> IsFileValid(Stream stream, string fileExtension)
        {
            try
            {
                Workbook workbook = null;
                using (var ms = new MemoryStream())
                {
                    if (fileExtension.ToLower() == ".xlsb")
                    {
                        ApplyAsposeLicense();
                        workbook = new Aspose.Cells.Workbook(stream);
                        workbook.Save(ms, SaveFormat.Xlsx);
                        ms.Position = 0;
                    }
                    IExcelDataReader excelReader = null;
                    if (fileExtension.ToLower() == ".xlsx")
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream); //3 min
                    else if (fileExtension.ToLower() == ".xlsb")
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(ms); //3 min
                    else
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream); //3 min

                    do
                    {
                        //Sheet level control
                        var noOfColumns = excelReader.FieldCount;
                        int rowCounter = 0;
                        //var sheets = new List<string>();

                        while (excelReader.Read() && rowCounter < 100) //Row level control
                        {
                            List<string> cellValues = new List<string>();
                            for (int i = 0; i < noOfColumns; i++)  // Cell level control
                            {
                                var sheetName = excelReader.Name;
                                var value = new object();
                                var dataType = excelReader.GetFieldType(i);
                                //if (dataType == null)
                                //break;
                                if (dataType != null && dataType.Name.ToLower().Equals("datetime"))
                                {
                                    var rawValue = excelReader.GetValue(i).ToString();
                                    var dateParts = rawValue.Split(' ');
                                    if (dateParts.Count() <= 1 || (dateParts.Count() > 1 && dateParts.LastOrDefault().Contains("00:00:00")))
                                        value = Convert.ToDateTime(excelReader.GetValue(i)).ToString("yyyy-MM-dd");
                                    else
                                        value = Convert.ToDateTime(excelReader.GetValue(i)).ToString("yyyy-MM-dd hh:mm:ss");
                                }
                                else
                                    value = excelReader.GetValue(i);
                                if (value != null)
                                    cellValues.Add(value.ToString());
                                else
                                    cellValues.Add(string.Empty);
                            }
                            //excelReader.GetString(1);
                            rowCounter++;
                        }
                    } while (excelReader.NextResult());

                    excelReader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("invalid file signature") || ex.Message.ToLower().Contains("end of central directory record could not be found"))
                {
                    return false;
                }
                else
                    throw;
            }
            finally
            {
                await Task.Delay(0);
            }
        }

        private void ApplyAsposeLicense()
        {
            if (!IsLicensApplied())
            {
                var license = "<License>  <Data>    <LicensedTo>GEP</LicensedTo>    <EmailTo>malik.bardai@gep.com</EmailTo>    <LicenseType>Developer OEM</LicenseType>    <LicenseNote>Limited to 1 developer, unlimited physical locations</LicenseNote>    <OrderID>200108004105</OrderID>    <UserID>105940</UserID>    <OEM>This is a redistributable license</OEM>    <Products>      <Product>Aspose.Total for .NET</Product>    </Products>    <EditionType>Enterprise</EditionType>    <SerialNumber>57994ee4-6f36-4a91-bf96-33b01d6f9283</SerialNumber>    <SubscriptionExpiry>20220226</SubscriptionExpiry>    <LicenseVersion>3.0</LicenseVersion>    <LicenseInstructions>https://purchase.aspose.com/policies/use-license</LicenseInstructions>  </Data>  <Signature>mQfXbdxGLxlFTDt+ScZ4TcPhBtZFjnpzgWKxB5Pb6OtZ7o5PGlFLS9ObW6DQJxGSYyNksXoTQgixLj67JAh8JW24ZvuMczpLALEJVee5xQlZvcrPEmDoZG3ku/s7rjjAUO5SlIY8JxJJ/6p8gL0I3X/sx6mah42is3TwzV6Qe1k=</Signature></License>";
                //var licenseStream = System.Text.Encoding.ASCII.GetBytes("<License>  <Data>    <LicensedTo>GEP</LicensedTo>    <EmailTo>malik.bardai@gep.com</EmailTo>    <LicenseType>Developer OEM</LicenseType>    <LicenseNote>Limited to 1 developer, unlimited physical locations</LicenseNote>    <OrderID>200108004105</OrderID>    <UserID>105940</UserID>    <OEM>This is a redistributable license</OEM>    <Products>      <Product>Aspose.Total for .NET</Product>    </Products>    <EditionType>Enterprise</EditionType>    <SerialNumber>57994ee4-6f36-4a91-bf96-33b01d6f9283</SerialNumber>    <SubscriptionExpiry>20220226</SubscriptionExpiry>    <LicenseVersion>3.0</LicenseVersion>    <LicenseInstructions>https://purchase.aspose.com/policies/use-license</LicenseInstructions>  </Data>  <Signature>mQfXbdxGLxlFTDt+ScZ4TcPhBtZFjnpzgWKxB5Pb6OtZ7o5PGlFLS9ObW6DQJxGSYyNksXoTQgixLj67JAh8JW24ZvuMczpLALEJVee5xQlZvcrPEmDoZG3ku/s7rjjAUO5SlIY8JxJJ/6p8gL0I3X/sx6mah42is3TwzV6Qe1k=</Signature></License>");
                RegisterLicense(license);
            }
        }

        public bool IsLicensApplied()
        {
            Workbook workbook = new Workbook();
            return workbook.IsLicensed;
        }

        public void RegisterLicense(string license)
        {
            MemoryStream licenseStream = null;
            try
            {
                //licenseStream = await GetAsposeLicenseStream();
                var bytes = Encoding.UTF8.GetBytes(license);

                licenseStream = new MemoryStream();
                licenseStream.Write(bytes, 0, bytes.Length);
                licenseStream.Position = 0;

                License licenseExcel = new License();
                licenseExcel.SetLicense(licenseStream);
            }
            finally
            {
                if (licenseStream != null)
                    licenseStream.Dispose();
            }
        }
    }
}
