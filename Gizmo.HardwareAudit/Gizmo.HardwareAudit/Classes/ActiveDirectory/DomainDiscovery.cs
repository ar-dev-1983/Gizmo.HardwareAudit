using Gizmo.HardwareAudit.Enums;
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
                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.Computer, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty, Info = new ActiveDirectoryComputerInfo(child) };
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
                                    var item = new DomainInformation() { Type = DomainInformationTypeEnum.Computer, Name = child.Name.Replace("CN=", ""), Description = child.Properties["description"].Value != null ? child.Properties["description"].Value.ToString() : string.Empty, Info = new ActiveDirectoryUserInfo(child) };
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
    }
}
