using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAuditClasses;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;

namespace Gizmo.HardwareAudit
{
    public class DomainDiscovery
    {
        public DomainDiscovery()
        { }

        public static DomainCollection EnumerateDomains(string forestName, UserProfile options)
        {
            using (Forest forest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, forestName, options.UserName, UserProfile.ToInsecureString(options.UserPassword))))
            {
                return forest.Domains;
            }
        }

        public static GlobalCatalogCollection EnumerateGlobalCatalogs(string name, UserProfile options)
        {
            using (Forest forest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, name, options.UserName, UserProfile.ToInsecureString(options.UserPassword))))
            {
                return forest.GlobalCatalogs;
            }
        }

        public static bool Authenticate(string domain, UserProfile options)
        {
            bool result = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, options.UserName, UserProfile.ToInsecureString(options.UserPassword));
                object nativeObject = entry.NativeObject;
                result = true;
            }
            catch (DirectoryServicesCOMException)
            { }
            return result;
        }

        public static List<DomainInformation> EnumerateComputersFromDomain(string name, UserProfile options)
        {
            var dc = EnumerateDomains(name, options);
            List<DomainInformation> Domains = new List<DomainInformation>();
            foreach (var domain in dc)
            {
                DomainInformation newDomain = new DomainInformation() { Type = DomainInformationTypeEnum.Root, Name = (domain as Domain).Name };
                var gc = DomainDiscovery.EnumerateGlobalCatalogs((domain as Domain).Name, options);
                foreach (var globalCatalog in gc)
                {
                    if ((globalCatalog as GlobalCatalog).SiteName != string.Empty)
                    {
                        DirectoryEntry directoryRoot = new DirectoryEntry("LDAP://" + (globalCatalog as GlobalCatalog).Name, options.UserName, UserProfile.ToInsecureString(options.UserPassword));
                        EnumerateComputers(newDomain, directoryRoot);
                    }
                }
                Domains.Add(newDomain);
            }
            return Domains;
        }

        public static List<DomainInformation> EnumerateUsersFromDomain(string name, UserProfile options)
        {
            var dc = EnumerateDomains(name, options);
            List<DomainInformation> Domains = new List<DomainInformation>();
            foreach (var domain in dc)
            {
                DomainInformation newDomain = new DomainInformation() { Type = DomainInformationTypeEnum.Root, Name = (domain as Domain).Name };
                var gc = DomainDiscovery.EnumerateGlobalCatalogs((domain as Domain).Name, options);
                foreach (var globalCatalog in gc)
                {
                    if ((globalCatalog as GlobalCatalog).SiteName != string.Empty)
                    {
                        DirectoryEntry directoryRoot = new DirectoryEntry("LDAP://" + (globalCatalog as GlobalCatalog).Name, options.UserName, UserProfile.ToInsecureString(options.UserPassword));
                        EnumerateUsers(newDomain, directoryRoot);
                    }
                }
                Domains.Add(newDomain);
            }
            return Domains;
        }

        public static List<DomainInformation> EnumerateGroupsFromDomain(string name, UserProfile options)
        {
            var dc = EnumerateDomains(name, options);
            List<DomainInformation> Domains = new List<DomainInformation>();
            foreach (var domain in dc)
            {
                DomainInformation newDomain = new DomainInformation() { Type = DomainInformationTypeEnum.Root, Name = (domain as Domain).Name };
                var gc = DomainDiscovery.EnumerateGlobalCatalogs((domain as Domain).Name, options);
                foreach (var globalCatalog in gc)
                {
                    if ((globalCatalog as GlobalCatalog).SiteName != string.Empty)
                    {
                        DirectoryEntry directoryRoot = new DirectoryEntry("LDAP://" + (globalCatalog as GlobalCatalog).Name, options.UserName, UserProfile.ToInsecureString(options.UserPassword));
                        EnumerateGroups(newDomain, directoryRoot);
                    }
                }
                Domains.Add(newDomain);
            }
            return Domains;
        }

        public static DomainInformation EnumerateComputersFromDomainController(string name, UserProfile options)
        {
            DomainInformation domainController = new DomainInformation() { Type = DomainInformationTypeEnum.Root, Name = name };
            DirectoryEntry directoryRoot = new DirectoryEntry("LDAP://" + name, options.UserName, UserProfile.ToInsecureString(options.UserPassword));
            EnumerateComputers(domainController, directoryRoot);
            return domainController;
        }

        public static DomainInformation EnumerateUsersFromDomainController(string name, UserProfile options)
        {
            DomainInformation domainController = new DomainInformation() { Type = DomainInformationTypeEnum.Root, Name = name };
            DirectoryEntry directoryRoot = new DirectoryEntry("LDAP://" + name, options.UserName, UserProfile.ToInsecureString(options.UserPassword));
            EnumerateUsers(domainController, directoryRoot);
            return domainController;
        }

        public static DomainInformation EnumerateGroupsFromDomainController(string name, UserProfile options)
        {
            DomainInformation domainController = new DomainInformation() { Type = DomainInformationTypeEnum.Root, Name = name };
            DirectoryEntry directoryRoot = new DirectoryEntry("LDAP://" + name, options.UserName, UserProfile.ToInsecureString(options.UserPassword));
            EnumerateGroups(domainController, directoryRoot);
            return domainController;
        }

        private static void EnumerateComputers(DomainInformation root, DirectoryEntry directory)
        {
            foreach (DirectoryEntry child in directory.Children)
            {
                if (child.SchemaClassName == "organizationalUnit" || child.SchemaClassName == "container" || child.SchemaClassName == "computer")
                {
                    DirectorySearcher mySearcher = new DirectorySearcher(child)
                    {
                        Filter = "(objectCategory=computer)"
                    };
                    if (mySearcher.FindAll().Count != 0 || child.SchemaClassName == "computer")
                    {
                        switch (child.SchemaClassName)
                        {
                            case "organizationalUnit":
                                {

                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.OrganizationUnit, Name = child.Name.Replace("OU=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                        EnumerateComputers(item, child);
                                    }
                                    break;
                                }
                            case "container":
                                {

                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.OrganizationUnit, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                        EnumerateComputers(item, child);
                                    }
                                    break;
                                }
                            case "computer":
                                {
                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.Computer, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty, Info = ParseComputerInfo(child) };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                        continue;
                }
                else
                    continue;
            }
        }

        private static void EnumerateUsers(DomainInformation root, DirectoryEntry directory)
        {
            foreach (DirectoryEntry child in directory.Children)
            {
                if (child.SchemaClassName == "organizationalUnit" || child.SchemaClassName == "container" || child.SchemaClassName == "user")
                {
                    DirectorySearcher mySearcher = new DirectorySearcher(child)
                    {
                        Filter = "((&(objectCategory=Person)(objectClass=User)))"
                    };
                    if (mySearcher.FindAll().Count != 0 || child.SchemaClassName == "user")
                    {
                        switch (child.SchemaClassName)
                        {
                            case "organizationalUnit":
                                {

                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.OrganizationUnit, Name = child.Name.Replace("OU=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                        EnumerateUsers(item, child);
                                    }
                                    break;
                                }
                            case "container":
                                {

                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.OrganizationUnit, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                        EnumerateUsers(item, child);
                                    }
                                    break;
                                }
                            case "user":
                                {
                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.User, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty, Info = ParseUserInfo(child) };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                        continue;
                }
                else
                    continue;
            }
        }

        private static void EnumerateGroups(DomainInformation root, DirectoryEntry directory)
        {
            foreach (DirectoryEntry child in directory.Children)
            {
                if (child.SchemaClassName == "organizationalUnit" || child.SchemaClassName == "container" || child.SchemaClassName == "group")
                {
                    DirectorySearcher mySearcher = new DirectorySearcher(child)
                    {
                        Filter = "(objectClass=group)"
                    };
                    if (mySearcher.FindAll().Count != 0 || child.SchemaClassName == "group")
                    {
                        switch (child.SchemaClassName)
                        {
                            case "organizationalUnit":
                                {

                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.OrganizationUnit, Name = child.Name.Replace("OU=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                        EnumerateGroups(item, child);
                                    }
                                    break;
                                }
                            case "container":
                                {

                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.OrganizationUnit, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        root.Childrens.Add(item);
                                        EnumerateGroups(item, child);
                                    }
                                    break;
                                }
                            case "group":
                                {
                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.Group, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty, Info = ParseGroupInfo(child) };
                                    if (root.Childrens.Where(x => x.Name == item.Name && x.Description == item.Description).Count() == 0)
                                    {
                                        if (child.Properties["member"] != null)
                                        {
                                            foreach (var member in child.Properties["member"])
                                            {
                                                (item.Info as ActiveDirectoryGroupInfo).Members.Add(member.ToString().Split(',')[0].Replace("CN=", ""));
                                            }
                                        }
                                        root.Childrens.Add(item);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                        continue;
                }
                else
                    continue;
            }
        }

        public static List<DomainInformation> EnumerateComputersInformation(string name, UserProfile options, DomainDiscoveryModeEnum mode) => mode switch
        {
            DomainDiscoveryModeEnum.FromDomainName => EnumerateComputersFromDomain(name, options),
            DomainDiscoveryModeEnum.FromDomainController => new List<DomainInformation>() { EnumerateComputersFromDomainController(name, options) },
            _ => new List<DomainInformation>()
        };

        public static List<DomainInformation> EnumerateUsersInformation(string name, UserProfile options, DomainDiscoveryModeEnum mode) => mode switch
        {
            DomainDiscoveryModeEnum.FromDomainName => EnumerateUsersFromDomain(name, options),
            DomainDiscoveryModeEnum.FromDomainController => new List<DomainInformation>() { EnumerateUsersFromDomainController(name, options) },
            _ => new List<DomainInformation>()
        };

        public static List<DomainInformation> EnumerateGroupsInformation(string name, UserProfile options, DomainDiscoveryModeEnum mode) => mode switch
        {
            DomainDiscoveryModeEnum.FromDomainName => EnumerateGroupsFromDomain(name, options),
            DomainDiscoveryModeEnum.FromDomainController => new List<DomainInformation>() { EnumerateGroupsFromDomainController(name, options) },
            _ => new List<DomainInformation>()
        };

        private static ActiveDirectoryComputerInfo ParseComputerInfo(DirectoryEntry directoryEntry)
        {
            return new ActiveDirectoryComputerInfo()
            {
                Id = Guid.NewGuid(),
                CN = string.Empty,
                Description = directoryEntry.Properties["description"].Value != null ? directoryEntry.Properties["description"].Value.ToString() : string.Empty,
                DistinguishedName = directoryEntry.Properties["distinguishedName"].Value != null ? directoryEntry.Properties["distinguishedName"].Value.ToString() : string.Empty,
                DNSHostName = directoryEntry.Properties["dNSHostName"].Value != null ? directoryEntry.Properties["dNSHostName"].Value.ToString().ToLower() : string.Empty,
                WhenCreated = string.Empty,
                WhenChanged = string.Empty,
                Name = directoryEntry.Properties["name"].Value != null ? directoryEntry.Properties["name"].Value.ToString().ToLower() : string.Empty,
                LastLogon = string.Empty,
                OperatingSystem = string.Empty,
                LastLogonTimestamp = string.Empty,

                ExtensionAttribute1 = string.Empty,
                ExtensionAttribute2 = string.Empty,
                ExtensionAttribute4 = string.Empty,
                ExtensionAttribute6 = string.Empty,
                ExtensionAttribute7 = string.Empty,
            };
        }

        private static ActiveDirectoryGroupInfo ParseGroupInfo(DirectoryEntry directoryEntry)
        {
            return new ActiveDirectoryGroupInfo()
            {
                Id = Guid.NewGuid(),
                Name = directoryEntry.Properties["cn"].Value != null ? directoryEntry.Properties["cn"].Value.ToString() : string.Empty,
                Description = directoryEntry.Properties["description"].Value != null ? directoryEntry.Properties["description"].Value.ToString() : string.Empty,
                DistinguishedName = directoryEntry.Properties["distinguishedName"].Value != null ? directoryEntry.Properties["distinguishedName"].Value.ToString() : string.Empty,
                Members = new List<string>()
            };
        }

        private static ActiveDirectoryUserInfo ParseUserInfo(DirectoryEntry directoryEntry)
        {
            var userAccountControl = directoryEntry.Properties["userAccountControl"].Value != null ? directoryEntry.Properties["userAccountControl"].Value.ToString() : string.Empty;

            var result = new ActiveDirectoryUserInfo()
            {
                Id = Guid.NewGuid(),
                FirstName = directoryEntry.Properties["givenName"].Value != null ? directoryEntry.Properties["givenName"].Value.ToString() : string.Empty,
                LastName = directoryEntry.Properties["sn"].Value != null ? directoryEntry.Properties["sn"].Value.ToString() : string.Empty,
                MN = directoryEntry.Properties["initials"].Value != null ? directoryEntry.Properties["initials"].Value.ToString() : string.Empty,
                ScreenName = directoryEntry.Properties["displayName"].Value != null ? directoryEntry.Properties["displayName"].Value.ToString() : string.Empty,
                Description = directoryEntry.Properties["description"].Value != null ? directoryEntry.Properties["description"].Value.ToString() : string.Empty,
                Room = directoryEntry.Properties["physicalDeliveryOfficeName"].Value != null ? directoryEntry.Properties["physicalDeliveryOfficeName"].Value.ToString() : string.Empty,
                Street = directoryEntry.Properties["streetAddress"].Value != null ? directoryEntry.Properties["streetAddress"].Value.ToString() : string.Empty,
                PostCode = directoryEntry.Properties["postalCode"].Value != null ? directoryEntry.Properties["postalCode"].Value.ToString() : string.Empty,
                PostBox = directoryEntry.Properties["postOfficeBox"].Value != null ? directoryEntry.Properties["postOfficeBox"].Value.ToString() : string.Empty,
                Town = directoryEntry.Properties["l"].Value != null ? directoryEntry.Properties["l"].Value.ToString() : string.Empty,

                Area = directoryEntry.Properties["st"].Value != null ? directoryEntry.Properties["st"].Value.ToString() : string.Empty,
                Country = directoryEntry.Properties["co"].Value != null ? directoryEntry.Properties["co"].Value.ToString() : string.Empty,
                PhoneNumber = directoryEntry.Properties["telephoneNumber"].Value != null ? directoryEntry.Properties["telephoneNumber"].Value.ToString() : string.Empty,
                Email = directoryEntry.Properties["mail"].Value != null ? directoryEntry.Properties["mail"].Value.ToString() : string.Empty,
                DistinguishedName = directoryEntry.Properties["distinguishedName"].Value != null ? directoryEntry.Properties["distinguishedName"].Value.ToString() : string.Empty,
                UserName = directoryEntry.Properties["sAMAccountName"].Value != null ? directoryEntry.Properties["sAMAccountName"].Value.ToString() : string.Empty,
            };
            /*
             * This is a hex values for userAccountControl property
            00000001	SCRIPT
            00000002	ACCOUNTDISABLE
            00000008	HOMEDIR_REQUIRED
            00000010	LOCKOUT
            00000020	PASSWD_NOTREQD
            00000040	PASSWD_CANT_CHANGE
            00000080	ENCRYPTED_TEXT_PWD_ALLOWED
            00000100	TEMP_DUPLICATE_ACCOUNT
            00000200	NORMAL_ACCOUNT
            00000800	INTERDOMAIN_TRUST_ACCOUNT
            00001000	WORKSTATION_TRUST_ACCOUNT
            00002000	SERVER_TRUST_ACCOUNT
            00010000	DONT_EXPIRE_PASSWORD
            00020000	MNS_LOGON_ACCOUNT
            00040000	SMARTCARD_REQUIRED
            00080000	TRUSTED_FOR_DELEGATION
            00100000	NOT_DELEGATED
            00200000	USE_DES_KEY_ONLY
            00400000	DONT_REQ_PREAUTH
            00800000	PASSWORD_EXPIRED
            01000000	TRUSTED_TO_AUTH_FOR_DELEGATION
            04000000	PARTIAL_SECRETS_ACCOUNT
             */
            if (userAccountControl != string.Empty)
            {
                var heXvalue = int.Parse(userAccountControl).ToString("X8");
                result.IsActive = heXvalue[7] == '2' ? false : true;
                result.PasswordRequired = heXvalue[6] == '2' ? false : true;
                result.PasswordChangeable = heXvalue[6] == '4' ? false : true;
                result.PasswordExpires = heXvalue[3] == '1' ? false : true;
            }
            else
            {
                result.IsActive = false;
                result.PasswordChangeable = false;
                result.PasswordExpires = false;
                result.PasswordRequired = false;
            }
            return result;
        }

    }
}
