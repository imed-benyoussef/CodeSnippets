﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain
{
    public interface IAggregateRoot
    {
    }
    public interface IDomainEvent : INotification { }
}
