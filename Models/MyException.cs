using System;

namespace dotnet_cloud_run_hello_world.Models
{
    public class MyException:Exception
    {
        public MyException(string message):base(message)
        {
                
        }
    }
}