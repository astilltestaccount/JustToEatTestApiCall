using System;
namespace JustToEat
{
    public abstract class Output
    {
        public virtual void Write(string output)
        {
            Console.WriteLine(output);
        }
    }
}
