using System;

namespace Gizmo.HardwareAudit
{
    public class CheckTPCPortResult
    {
        public Guid Id { set; get; }
        public bool IsOpened { set; get; }

        public CheckTPCPortResult()
        {
            Id = Guid.NewGuid();
            IsOpened = false;
        }
    }
}
