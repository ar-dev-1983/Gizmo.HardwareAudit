using Gizmo.HardwareAudit.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>();
            stack.Push(item);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }

    }
}
