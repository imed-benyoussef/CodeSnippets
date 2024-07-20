using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Model.CodeValidators
{

    public enum VerificationType
    {
        Email,
        Sms
    }

    public enum CodeStatus
    {
        Active,
        Used,
        Expired,
        Cancelled
    }

}
