﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IMessageBroker
    {
        Task<bool> PublishMessageAsync<T>(string topic, T message,string eventType);
        //Task<T> ConsumeMessageAsync<T>(string topic);
    }
}