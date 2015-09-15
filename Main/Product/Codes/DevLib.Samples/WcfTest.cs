﻿namespace DevLib.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceModel;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfTest : IWcfTest
    {
        public WcfTest()
        {
            this._echo = "Default Ctor";
        }

        public WcfTest(string echo)
        {
            this._echo = echo;
        }

        List<Animal> animals = new List<Animal>();

        private string _echo = "";

        public string MyOperation1(string arg1, int arg2)
        {
            Console.WriteLine(arg1 + arg2.ToString());
            return arg1 + arg2.ToString();
        }

        public void MyOperation2(string value)
        {
            Console.WriteLine(value);
        }


        public object Foo(string value)
        {
            Console.WriteLine(value);
            return value;
        }


        public void AddAnimal(Animal value)
        {
            Console.WriteLine("call AddAnimal");
            animals.Add(value);
            Console.WriteLine(value.Name);
        }

        public string Echo()
        {
            return _echo;
        }
    }
}
