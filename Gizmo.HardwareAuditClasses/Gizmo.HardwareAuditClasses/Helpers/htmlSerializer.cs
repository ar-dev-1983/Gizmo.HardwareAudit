using Gizmo.HardwareAuditClasses.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Gizmo.HardwareAuditClasses.Helpers
{
    public class fakeColor
    {
        public byte A { set; get; }
        public byte R { set; get; }
        public byte G { set; get; }
        public byte B { set; get; }
        public fakeColor()
        {
            A = 255;
            R = 0;
            G = 0;
            B = 0;
        }
        public fakeColor(byte r, byte g, byte b)
        {
            A = 255;
            R = r;
            G = g;
            B = b;
        }
        public fakeColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static string GetHEXFromBytes(byte r, byte g, byte b) => $"#{r:X2}{g:X2}{b:X2}";
        public static string GetHEXAsARGBFromBytes(byte a, byte r, byte g, byte b) => $"#{a:X2}{r:X2}{g:X2}{b:X2}";
        public static string GetHEXAsRGBAFromBytes(byte a, byte r, byte g, byte b) => $"#{r:X2}{g:X2}{b:X2}{a:X2}";

        public static fakeColor GetFromARGB(fakeColor background, fakeColor foreground)
        {
            if (foreground.A == 255)
                return foreground;

            var alpha = foreground.A / 255.0;
            var diff = 1.0 - alpha;
            return new fakeColor(255,
                (byte)(foreground.R * alpha + background.R * diff),
                (byte)(foreground.G * alpha + background.G * diff),
                (byte)(foreground.B * alpha + background.B * diff));
        }

        public string GetHEXFromBytes() => $"#{R:X2}{G:X2}{B:X2}";
        public string GetHEXAsARGBFromBytes() => $"#{A:X2}{R:X2}{G:X2}{B:X2}";
        public string GetHEXAsRGBAFromBytes() => $"#{R:X2}{G:X2}{B:X2}{A:X2}";

        public fakeColor GetFromARGB(fakeColor background)
        {
            if (A == 255)
                return new fakeColor(R, G, B);

            var alpha = A / 255.0;
            var diff = 1.0 - alpha;
            return new fakeColor(255, (byte)(R * alpha + background.R * diff), (byte)(G * alpha + background.G * diff), (byte)(B * alpha + background.B * diff));
        }
    }

    public static class htmlSerializer
    {

        /*
         Of course, i shouldn't write html serialization this way, but how to make serialization exactly in the same form as the scan is displayed in the program?
         If I find another way, this section will be rewritten.
         */
        public static string Serialize(string name, ComputerHardwareScan sc, Dictionary<string, fakeColor> BrushList, ObservableCollection<object> ViewModes, string description = "", string ipaddres = "", string fqdn = "", bool includeFullInfo = false)
        {
            var fileContentTemplate = new StringBuilder();
            fileContentTemplate.AppendLine("<!DOCTYPE html>");
            fileContentTemplate.AppendLine("<head>");
            fileContentTemplate.AppendLine("    <style>");
            fileContentTemplate.AppendLine("        HTML { margin: 0 !important; border: none !important; }");
            fileContentTemplate.AppendLine("        body { background-color: " + BrushList["Background"].GetHEXFromBytes() + ";");
            fileContentTemplate.AppendLine("               color: " + BrushList["Text"].GetHEXFromBytes() + ";");
            fileContentTemplate.AppendLine("               font-family: Segoe UI;");
            fileContentTemplate.AppendLine("               font-size: 12px;");
            fileContentTemplate.AppendLine("             }");
            fileContentTemplate.AppendLine("        .headerStyle { width: 100%; border-collapse: collapse;}");
            fileContentTemplate.AppendLine("        .headerStyle th { background-color: " + BrushList["Header"].GetFromARGB(BrushList["Background"]).GetHEXFromBytes() + "; width: 135px; text-align: right; height: 22px;}");
            fileContentTemplate.AppendLine("        .headerStyle td { height: 0px;}");
            fileContentTemplate.AppendLine("        .dataStyle { width: 100%; border-collapse: collapse;}");
            fileContentTemplate.AppendLine("        hr.separator { margin-left: 145px; border: 3px solid " + BrushList["Header"].GetFromARGB(BrushList["Background"]).GetHEXFromBytes() + ";}");
            fileContentTemplate.AppendLine("    </style>");
            fileContentTemplate.AppendLine("    <meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\">");
            fileContentTemplate.AppendLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            fileContentTemplate.AppendLine("    <meta name=\"google\" value=\"notranslate\">");
            fileContentTemplate.AppendLine("    <meta name=\"gwt:property\" content=\"locale=ru_RU\">");
            fileContentTemplate.AppendLine("    <title>" + name + "</title>");
            fileContentTemplate.AppendLine("</head>");
            fileContentTemplate.AppendLine("<body>");

            if (includeFullInfo)
            {
                fileContentTemplate.AppendLine(GetHeader("GENERAL"));
                fileContentTemplate.AppendLine(GetOneValue("Name", name));
                fileContentTemplate.AppendLine(GetOneValue("Description", description));
                fileContentTemplate.AppendLine(GetOneValue("IP Address", ipaddres));
                fileContentTemplate.AppendLine(GetOneValue("FQDN", fqdn));
            }

            if (ViewModes != null)
            {
                if (ViewModes.Count != 0)
                {
                    bool showAll = false;

                    foreach (var node in ViewModes)
                    {
                        if (Equals(node, UIViewModeEnum.All))
                        {
                            showAll = true;
                        }
                    }

                    if (showAll)
                    {
                        SystemEnclosure(sc, fileContentTemplate);
                        CPUs(sc, fileContentTemplate);
                        MemoryDevices(sc, fileContentTemplate);
                        VideoControllers(sc, fileContentTemplate);
                        Displays(sc, fileContentTemplate);
                        NetworkAdapters(sc, fileContentTemplate);
                        PhysicalDrives(sc, fileContentTemplate);
                        Partitions(sc, fileContentTemplate);
                        Licenses(sc, fileContentTemplate);
                        Printers(sc, fileContentTemplate);
                        LocalUsers(sc, fileContentTemplate);
                        LocalGroups(sc, fileContentTemplate);
                    }
                    else
                    { 
                        foreach (var node in ViewModes)
                        {
                            if (Equals(node, UIViewModeEnum.SystemEnclosure))
                            {
                                SystemEnclosure(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.CPUs))
                            {
                                CPUs(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.MemoryDevices))
                            {
                                MemoryDevices(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.VideoControllers))
                            {
                                VideoControllers(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.Displays))
                            {
                                Displays(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.NetworkAdapters))
                            {
                                NetworkAdapters(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.PhysicalDrives))
                            {
                                PhysicalDrives(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.Partitions))
                            {
                                Partitions(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.Licenses))
                            {
                                Licenses(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.Printers))
                            {
                                Printers(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.LocalUsers))
                            {
                                LocalUsers(sc, fileContentTemplate);
                            }
                            else if (Equals(node, UIViewModeEnum.LocalGroups))
                            {
                                LocalGroups(sc, fileContentTemplate);
                            }
                        }
                    }
                }
                else
                {
                    SystemEnclosure(sc, fileContentTemplate);
                    CPUs(sc, fileContentTemplate);
                    MemoryDevices(sc, fileContentTemplate);
                    VideoControllers(sc, fileContentTemplate);
                    Displays(sc, fileContentTemplate);
                    NetworkAdapters(sc, fileContentTemplate);
                    PhysicalDrives(sc, fileContentTemplate);
                    Partitions(sc, fileContentTemplate);
                    Licenses(sc, fileContentTemplate);
                    Printers(sc, fileContentTemplate);
                    LocalUsers(sc, fileContentTemplate);
                    LocalGroups(sc, fileContentTemplate);
                }
            }
            else
            {
                SystemEnclosure(sc, fileContentTemplate);
                CPUs(sc, fileContentTemplate);
                MemoryDevices(sc, fileContentTemplate);
                VideoControllers(sc, fileContentTemplate);
                Displays(sc, fileContentTemplate);
                NetworkAdapters(sc, fileContentTemplate);
                PhysicalDrives(sc, fileContentTemplate);
                Partitions(sc, fileContentTemplate);
                Licenses(sc, fileContentTemplate);
                Printers(sc, fileContentTemplate);
                LocalUsers(sc, fileContentTemplate);
                LocalGroups(sc, fileContentTemplate);
            }

            fileContentTemplate.AppendLine("</body>");

            return fileContentTemplate.ToString();
        }

        private static void LocalGroups(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsWindowsLocalGroupsPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("LOCAL GROUPS"));
                foreach (var node in sc.WindowsLocalGroups)
                {
                    fileContentTemplate.AppendLine(GetLocalGroupValue("Name", node.Name, "Caption", node.Caption, "Description", node.Description, "Local", node.IsLocal.ToString(), "Status", node.Status, "Users", string.Join("<br>", node.MembersInOneLine.Split('\n'))));
                    if (sc.WindowsLocalGroups.IndexOf(node) != sc.WindowsLocalGroups.Count - 1)
                        fileContentTemplate.AppendLine("<hr class=\"separator\">");
                }
            }
        }

        private static void LocalUsers(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsWindowsLocalUsersPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("LOCAL USERS"));
                foreach (var node in sc.WindowsLocalUsers)
                {
                    fileContentTemplate.AppendLine(GetLocalUserValue("Name", node.Name, "Caption", node.Caption, "Description", node.Description, "Active", node.IsActive.ToString(), "Status", node.Status, "Changeable", node.PasswordChangeable.ToString(), "Expires", node.PasswordExpires.ToString(), "Required", node.PasswordRequired.ToString()));
                    if (sc.WindowsLocalUsers.IndexOf(node) != sc.WindowsLocalUsers.Count - 1)
                        fileContentTemplate.AppendLine("<hr class=\"separator\">");
                }
            }
        }

        private static void Printers(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsPrintersPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("PRINTERS"));
                foreach (var node in sc.Printers)
                {
                    fileContentTemplate.AppendLine(GetPrinterValue("Name", node.DefaultPrinter, "Port", node.DefaultPrinterPortName, "Local", node.IsLocal.ToString(), "Network", node.IsNetwork.ToString(), "Shared", node.IsShared.ToString()));
                    if (sc.Printers.IndexOf(node) != sc.Printers.Count - 1)
                        fileContentTemplate.AppendLine("<hr class=\"separator\">");
                }
            }
        }

        private static void Licenses(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsSoftwareLicensingProductsPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("MS LICENSES"));
                foreach (var node in sc.SoftwareLicensingProducts)
                {
                    fileContentTemplate.AppendLine(GetSoftwareLicencingProductValue("Name", node.Name, "Channel", node.Description, "Status", node.LicenseStatus, "Partial Product Key", node.PatrialProductKey, "Type", node.LicenseFamily, "Product Key ID", node.ProductKeyID));
                    if (sc.SoftwareLicensingProducts.IndexOf(node) != sc.SoftwareLicensingProducts.Count - 1)
                        fileContentTemplate.AppendLine("<hr class=\"separator\">");
                }
            }
        }

        private static void Partitions(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsLogicalDrivesPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("PARTITIONS"));
                foreach (var node in sc.LogicalDrives)
                {
                    fileContentTemplate.AppendLine(GetLogicalDriveValue(node.Letter, node.TotalSize, "Aviailable Size", node.AviailableSize));
                }
            }
        }

        private static void PhysicalDrives(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsPhysicalDrivesPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("PHYSICAL DRIVES"));
                foreach (var node in sc.PhysicalDrives)
                {
                    fileContentTemplate.AppendLine(GetPhsicalDriveValue("Product", node.Model, "Serial Number", node.SerialNumber, "Size", node.Size));
                }
            }
        }

        private static void NetworkAdapters(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsNetworkAdaptersPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("NETWORK ADAPTERS"));
                foreach (var node in sc.NetworkAdapters)
                {
                    fileContentTemplate.AppendLine(GetNetworkAdapterValue("Product", node.Adapter, "IP", node.IPAddress, "Mask", node.SubnetMasks, "Geteway", node.DefaultGeteway, "MAC", node.MAC, "DHCP", node.DHCP_Enabled, "DHCP Server", node.DHCP_ServerIP));
                    if (sc.NetworkAdapters.IndexOf(node) != sc.NetworkAdapters.Count - 1)
                        fileContentTemplate.AppendLine("<hr class=\"separator\">");
                }
            }
        }

        private static void Displays(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsMonitorsPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("DISPLAYS"));
                foreach (var node in sc.Monitors)
                {
                    fileContentTemplate.AppendLine(GetDisplayValue("Vendor", node.Manufacturer, "Product", node.MonitorModel, "Serial Number", node.MonitorSerialNumber));
                }
            }
        }

        private static void VideoControllers(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsVideoControllersPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("VIDEO ADAPTERS"));
                foreach (var node in sc.VideoControllers)
                {
                    fileContentTemplate.AppendLine(GetVideoControllerValue("Product", node.Name, "CPU", node.VideoProcessor, "Mode", node.VideoModeDescription));
                }
            }
        }

        private static void MemoryDevices(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsMemoryDevicesPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("MEMORY DEVICES"));
                foreach (var node in sc.MemoryDevices)
                {
                    fileContentTemplate.AppendLine(GetMemoryDeviceValue(string.Empty, node.MemoryTypeString, "Vendor", node.ManufacturerName, "Part Number", node.PartNumber, "Size", node.SizeString));
                    if (sc.MemoryDevices.IndexOf(node) != sc.MemoryDevices.Count - 1)
                        fileContentTemplate.AppendLine("<hr class=\"separator\">");
                }
            }
        }

        private static void CPUs(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            if (sc.IsCPUsPresent)
            {
                fileContentTemplate.AppendLine(GetHeader("CPU"));
                foreach (var node in sc.CPUs)
                {
                    fileContentTemplate.AppendLine(GetCPUValue(node.SlotLocator, node.ManufacturerName, "Model", node.Version, "Cores", node.CoreCount.ToString(), "Threads", node.ThreadCount.ToString()));
                }
            }
        }

        private static void SystemEnclosure(ComputerHardwareScan sc, StringBuilder fileContentTemplate)
        {
            fileContentTemplate.AppendLine(GetHeader("SYSTEM"));
            fileContentTemplate.AppendLine(GetFourValues("System Enclosure", sc.SystemInformation.ManufacturerName, "Product", sc.SystemInformation.ProductName, "Serial Number", sc.SystemInformation.SerialNumber, "Version", sc.SystemInformation.Version));
            fileContentTemplate.AppendLine(GetFourValues("Motherboard", sc.MotherBoardInformation.ManufacturerName, "Product", sc.MotherBoardInformation.ProductName, "Serial Number", sc.MotherBoardInformation.SerialNumber, "Version", sc.MotherBoardInformation.Version));
            fileContentTemplate.AppendLine(GetHeader("OS"));
            fileContentTemplate.AppendLine(GetFourValues("Manufacturer", sc.WindowsInformation.Manufacturer, "Product", sc.WindowsInformation.Name, "Version", sc.WindowsInformation.Version, "Architecture", sc.WindowsInformation.OSArchitecture));
            fileContentTemplate.AppendLine(GetFourValues("Install Date", sc.WindowsInformation.InstallDate, "Directory", sc.WindowsInformation.WindowsDirectory, "Total Memory", sc.WindowsInformation.TotalMemory, "Aviailable Memory", sc.WindowsInformation.AviailableMemory));
            fileContentTemplate.AppendLine(GetOneValue("LoggedInUser", sc.LoggedInUser));
        }

        private static string GetHeader(string Header)
        {
            return "<table class=\"headerStyle\"><tr><th>" + Header + "</th><th style=\"width: 700px;\"/><th style=\"width: auto;\"/></tr></table>";
        }

        private static string GetOneValue(string Header1, string Value1)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 295px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetFourValues(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 140px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 60px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 280px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 175px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: 110px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 175px;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetCPUValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 120px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 280px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: 50px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 40px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: 50px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 40x;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetMemoryDeviceValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 120px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 135px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 155px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: 50px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 40x;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetVideoControllerValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 265px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 40px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 265px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: 50px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 295px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetDisplayValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 120px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 120px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: 100px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 150px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetNetworkAdapterValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4, string Header5, string Value5, string Header6, string Value6, string Header7, string Value7)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 680px;\" colspan=\"7\">" + Value1 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 110px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\">" + Header5 + "</td>\n" +
                "        <td style=\"width: 110px;\">" + Value5 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\">" + Header6 + "</td>\n" +
                "        <td style=\"width: 110px;\">" + Value6 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: 110x;\"></td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 110px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: 110px;\"></td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\">" + Header7 + "</td>\n" +
                "        <td style=\"width: 110px;\">" + Value7 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: 110x;\"></td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 110px;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: 110px;\"></td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: 110px;\"></td>\n" +
                "        <td style=\"width: 80px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: 110x;\"></td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetPhsicalDriveValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 295px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 305px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: 50px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 70px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetLogicalDriveValue(string Header1, string Value1, string Header2, string Value2)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 70px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 70px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetSoftwareLicencingProductValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4, string Header5, string Value5, string Header6, string Value6)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 680px;\" colspan=\"4\">" + Value1 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 680px;\" colspan=\"4\">" + Value2 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 135px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: 100px; font-weight: bold; text-align: right;\">" + Header5 + "</td>\n" +
                "        <td style=\"width: 215px;\">" + Value5 + "</td>\n" +
                "        <td style=\"width: 240px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 135px;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: 100px; font-weight: bold; text-align: right;\">" + Header6 + "</td>\n" +
                "        <td style=\"width: 215px;\">" + Value6 + "</td>\n" +
                "        <td style=\"width: 240px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetPrinterValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4, string Header5, string Value5)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 295px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 295px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>" +
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\"></td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 70px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 40px;\">" + Value3 + "</td>\n" +
                "        <td style=\"width: 70px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 40px;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: 70px; font-weight: bold; text-align: right;\">" + Header5 + "</td>\n" +
                "        <td style=\"width: 40px;\">" + Value5 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetLocalUserValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4, string Header5, string Value5, string Header6, string Value6, string Header7, string Value7, string Header8, string Value8)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 295px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 295px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 680px;\" colspan=\"4\">" + Value3 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 295px;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header5 + "</td>\n" +
                "        <td style=\"width: 295px;\">" + Value5 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>" +
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">Password</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 70px; font-weight: bold; text-align: right;\">" + Header6 + "</td>\n" +
                "        <td style=\"width: 40px;\">" + Value6 + "</td>\n" +
                "        <td style=\"width: 70px; font-weight: bold; text-align: right;\">" + Header7 + "</td>\n" +
                "        <td style=\"width: 40px;\">" + Value7 + "</td>\n" +
                "        <td style=\"width: 70px; font-weight: bold; text-align: right;\">" + Header8 + "</td>\n" +
                "        <td style=\"width: 40px;\">" + Value8 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }

        private static string GetLocalGroupValue(string Header1, string Value1, string Header2, string Value2, string Header3, string Value3, string Header4, string Value4, string Header5, string Value5, string Header6, string Value6)
        {
            return
                "<table class=\"dataStyle\">\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header1 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 295px;\">" + Value1 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header2 + "</td>\n" +
                "        <td style=\"width: 295px;\">" + Value2 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header3 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 680px;\" colspan=\"4\">" + Value3 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; text-align: right;\">" + Header4 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 295px;\">" + Value4 + "</td>\n" +
                "        <td style=\"width: 90px; font-weight: bold; text-align: right;\">" + Header5 + "</td>\n" +
                "        <td style=\"width: 295px;\">" + Value5 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "    <tr>\n" +
                "        <td style=\"width: 135px; font-weight: bold; vertical-align:top; text-align: right;\">" + Header6 + "</td>\n" +
                "        <td style=\"width: 20px;\"></td>\n" +
                "        <td style=\"width: 680px; vertical-align:top;\" colspan=\"4\">" + Value6 + "</td>\n" +
                "        <td style=\"width: auto;\"></td>\n" +
                "    </tr>\n" +
                "</table>";
        }
    }

}
