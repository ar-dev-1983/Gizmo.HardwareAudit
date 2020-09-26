using Gizmo.HardwareAudit.Enums;
using System;
using System.Collections.Generic;

namespace Gizmo.HardwareAudit
{

    public class DomainInformation
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public DomainInformationTypeEnum Type { set; get; }
        public List<DomainInformation> Childrens { set; get; }
        public bool HasChildren { get => Childrens != null && Childrens.Count > 0; }
        public object Info { set; get; }

        public DomainInformation()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
            Type = DomainInformationTypeEnum.None;
            Childrens = new List<DomainInformation>();
            Info = null;
        }
    }
}
