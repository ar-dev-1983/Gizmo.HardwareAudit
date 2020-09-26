using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    public class SoftwareLicensingProduct
    {
        [Category("MicrosoftSoftwareProductItem")]
        [Description("Лицензия Microsoft: имя")]
        public string Name { set; get; }

        [Description("Лицензия Microsoft: описание")]
        public string Description { set; get; }

        [Category("MicrosoftSoftwareProductItem")]
        [Description("Лицензия Microsoft: статус")]
        public string LicenseStatus { set; get; }

        [Category("MicrosoftSoftwareProductItem")]
        [Description("Лицензия Microsoft: семейство")]
        public string LicenseFamily { set; get; }

        [Category("MicrosoftSoftwareProductItem")]
        [Description("Лицензия Microsoft: часть ключа продукта")]
        public string PatrialProductKey { set; get; }

        [Category("MicrosoftSoftwareProductItem")]
        [Description("Лицензия Microsoft: идентификатор продукта")]
        public string ProductKeyID { set; get; }

        public SoftwareLicensingProduct()
        {
            Name = string.Empty;
            Description = string.Empty;
            LicenseStatus = string.Empty;
            LicenseFamily = string.Empty;
            PatrialProductKey = string.Empty;
            ProductKeyID = string.Empty;
        }

        public static List<SoftwareLicensingProduct> Enumerate(ManagementScope Scope)
        {
            var result = new List<SoftwareLicensingProduct>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM SoftwareLicensingProduct");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    if (m["Name"] != null && m["PartialProductKey"] != null)
                    {
                        if (m["Name"].ToString() != string.Empty && m["PartialProductKey"].ToString() != string.Empty)
                        {
                            var LicenseStatus = m["LicenseStatus"] != null ? int.Parse(m["LicenseStatus"].ToString()) : -1;
                            var ProductKeyID = string.Empty;
                            try { ProductKeyID = m["ProductKeyID2"] != null ? StringFormatting.CleanInvalidXmlChars(m["ProductKeyID2"].ToString()).TrimStart().TrimEnd() : string.Empty; } catch (Exception) { }
                            result.Add(new SoftwareLicensingProduct
                            {
                                Name = m["Name"] != null ? StringFormatting.CleanInvalidXmlChars(m["Name"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                Description = m["Description"] != null ? StringFormatting.CleanInvalidXmlChars(m["Description"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                LicenseFamily = m["LicenseFamily"] != null ? StringFormatting.CleanInvalidXmlChars(m["LicenseFamily"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                PatrialProductKey = m["PartialProductKey"] != null ? StringFormatting.CleanInvalidXmlChars(m["PartialProductKey"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                ProductKeyID = ProductKeyID,
                                LicenseStatus = LicenseStatus switch
                                {
                                    -1 => "Unknown",
                                    0 => "Unlicensed",
                                    1 => "Licensed",
                                    2 => "OOBGrace",
                                    3 => "OOTGrace",
                                    4 => "NonGenuineGrace",
                                    5 => "Notification",
                                    _ => ""
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception) { }

            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM OfficeSoftwareProtectionProduct");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    if (m["Name"] != null && m["PartialProductKey"] != null)
                    {
                        if (m["Name"].ToString() != string.Empty && m["PartialProductKey"].ToString() != string.Empty)
                        {
                            var LicenseStatus = m["LicenseStatus"] != null ? int.Parse(m["LicenseStatus"].ToString()) : -1;
                            result.Add(new SoftwareLicensingProduct
                            {
                                Name = m["Name"] != null ? StringFormatting.CleanInvalidXmlChars(m["Name"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                Description = m["Description"] != null ? StringFormatting.CleanInvalidXmlChars(m["Description"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                LicenseFamily = m["LicenseFamily"] != null ? StringFormatting.CleanInvalidXmlChars(m["LicenseFamily"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                PatrialProductKey = m["PartialProductKey"] != null ? StringFormatting.CleanInvalidXmlChars(m["PartialProductKey"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                ProductKeyID = m["ProductKeyID2"] != null ? StringFormatting.CleanInvalidXmlChars(m["ProductKeyID2"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                LicenseStatus = LicenseStatus switch
                                {
                                    -1 => "Unknown",
                                    0 => "Unlicensed",
                                    1 => "Licensed",
                                    2 => "OOBGrace",
                                    3 => "OOTGrace",
                                    4 => "NonGenuineGrace",
                                    5 => "Notification",
                                    _ => ""
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception) { }

            foreach (var node in result)
            {
                if (node.Name.ToLower().Contains("windows"))
                {
                    try
                    {
                        var Name = string.Empty;
                        var SerialNumber = string.Empty;
                        Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                        Scope.Connect();
                        ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                        ManagementObjectCollection queryCollection = searcher.Get();
                        foreach (ManagementObject m in queryCollection)
                        {
                            Name = m["Caption"] != null ? StringFormatting.CleanInvalidXmlChars(m["Caption"].ToString()).TrimStart().TrimEnd() : string.Empty;
                            SerialNumber = m["SerialNumber"] != null ? StringFormatting.CleanInvalidXmlChars(m["SerialNumber"].ToString()).TrimStart().TrimEnd() : string.Empty;
                        }

                        if (Name != string.Empty) { node.Name = Name.Replace(" (Registered Trademark)", ""); }
                        if (SerialNumber != string.Empty) { node.ProductKeyID = SerialNumber; }

                    }
                    catch (Exception) { }
                }
            }
            return result.OrderBy(x => x.Name).ToList();
        }
    }
}
