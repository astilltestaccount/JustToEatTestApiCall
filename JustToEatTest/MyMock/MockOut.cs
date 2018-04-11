using System;
using System.Text;
using JustToEat;

namespace JustToEatTest.MyMock
{
    public class MockOut : Output
    {
        public StringBuilder _stringBuilder = new StringBuilder();

        public override void Write(string output)
        {
            _stringBuilder.Append(output);
        }
    }
}
