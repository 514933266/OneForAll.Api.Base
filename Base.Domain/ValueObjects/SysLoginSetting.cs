using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Domain.ValueObjects
{
    public class SysLoginSetting
    {
        public int BanTime { get; set; }

        public int MaxPwdErrCount { get; set; }
    }
}
