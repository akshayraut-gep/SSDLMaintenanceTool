using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDLMaintenanceTool.Models
{
    public class ServiceBusLog
    {
        public int MyProperty { get; set; }
        public string MessageId { get; internal set; }
        public string CorrelationId { get; internal set; }
        public int DeliveryCount { get; internal set; }
        public string SessionId { get; internal set; }
        public long SequenceNumber { get; internal set; }
        public DateTimeOffset ScheduledEnqueueTime { get; internal set; }
        public DateTimeOffset LockedUntil { get; internal set; }
        public DateTimeOffset ExpiresAt { get; internal set; }
        public DateTimeOffset EnqueuedTime { get; internal set; }
        public long EnqueuedSequenceNumber { get; internal set; }
    }
}
