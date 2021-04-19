﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.Utilities.Interceptors;

namespace Core.Aspects.Autofac.Logging
{
   public class LogAspect:MethodInterception
   {
       private LoggerServiceBase _loggerServiceBase;

       public LogAspect(Type loggerService)
       {

           if (loggerService.BaseType!=typeof(LoggerServiceBase))
           {
               throw new Exception("wrong logger type");
           }
           _loggerServiceBase = (LoggerServiceBase) Activator.CreateInstance(loggerService);
       }

       protected override void OnBefore(IInvocation invocation)
       {
           _loggerServiceBase.Info(getLogDetail(invocation));
       }

       private LogDetail getLogDetail(IInvocation invocation)
       {
           var logParameters=new List<LogParameter>();
           for (int i = 0; i < invocation.Arguments.Length; i++)
           {
               logParameters.Add(new LogParameter
               {
                   Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                   Value = invocation.Arguments[i],
                   Type = invocation.Arguments[i].GetType().Name
               });
           }

           var logDetail=new LogDetail
           {
               MethodName = invocation.Method.Name,
               LogParameters = logParameters
           };

           return logDetail;
       }
   }
}
