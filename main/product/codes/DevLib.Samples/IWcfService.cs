﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DevLib.Samples
{
    [ServiceContract()]
    public interface IWcfService
    {
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [FaultContract(typeof(Exception))]
        string MyOperation1(string arg1, int arg2);

        [OperationContract]
        void MyOperation2(string value);
    }
}
