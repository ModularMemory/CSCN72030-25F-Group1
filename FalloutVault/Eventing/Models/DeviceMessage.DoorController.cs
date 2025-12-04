using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutVault.Eventing.Models;
public partial class DeviceMessage
{
    public class DoorOpenCloseChanged(bool data) : DeviceOpenCloseChanged(data)
    {
        public override string Message => "Door open/close changed";
    }

    public class DoorLockChanged(bool data) : DeviceMessage(data)
    {
        public bool IsLocked { get; } = data;

        public override string Message => "Door lock state changed";
    }
}
