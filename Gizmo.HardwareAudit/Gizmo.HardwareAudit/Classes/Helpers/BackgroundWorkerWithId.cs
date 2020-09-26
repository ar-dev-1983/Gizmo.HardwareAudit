using System;
using System.ComponentModel;

namespace Gizmo.HardwareAudit
{
    public class BackgroundWorkerWithId : BackgroundWorker
    {
        public Guid Id { set; get; }
        public bool Done { set; get; }
        public int FailureAttempts { set; get; }
        public int MaxFailureAttempts { set; get; }

        public BackgroundWorkerWithId()
        {
            Id = new Guid();
            Done = true;
            FailureAttempts = 0;
            MaxFailureAttempts = 1;
        }
        public BackgroundWorkerWithId(Guid id)
        {
            Id = id;
            Done = true;
            FailureAttempts = 0;
            MaxFailureAttempts = 1;
        }
        public BackgroundWorkerWithId(Guid id, int maxFailureAttempts)
        {
            Id = id;
            Done = true;
            FailureAttempts = 0;
            MaxFailureAttempts = maxFailureAttempts;
        }

    }

}
