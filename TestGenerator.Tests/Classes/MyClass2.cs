﻿namespace TestGenerator.Tests.Classes
{
    internal class MyClass2
    {
        public void FirstMethod()
        {
            Console.WriteLine("First method");
        }

        public void SecondMethod()
        {
            Console.WriteLine("Second method");
        }

        public void ThirdMethod(int a)
        {
            Console.WriteLine("Third method (int)");
        }

        public void ThirdMethod(double a)
        {
            Console.WriteLine("Third method (double)");
        }

        private void ThirdMethod(string a)
        {
            Console.WriteLine(a);
        }
    }

    internal class AbobaClass
    {
        public void ABOBAMethod()
        {
            Console.WriteLine("ABOBA");
        }
    }
}