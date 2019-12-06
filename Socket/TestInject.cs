using SocketApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocketApp
{
    public class TestInject : IService
    {
        public string Name { get; set; } = "TestInject";
    }

    public class MyInject : IService
    {
        public string Name = "MyInject";
    }

    public class PoldoInject : IService
    {
        public string Name = "PoldoInject";
    }


}
