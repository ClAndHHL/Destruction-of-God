using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon
{
    public enum ReturnCode : short
    {
        Success,
        Error,
        Failure,
        Exception,
        HavingTeam,  //成功组队
        WaitingTeam,  //等待组队
    }
}
