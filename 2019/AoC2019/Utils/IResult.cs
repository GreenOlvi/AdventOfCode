using System;

namespace AoC2019.Utils
{
    public interface IResult
    {
        public bool IsOk { get; }
    }

    public static class Result
    {
        public static Ok Ok = new Ok();
        public static Error Error(Exception e) => new Error(e);
    }

    public class Ok : IResult
    {
        public bool IsOk => true;
    }

    public class Error : IResult
    {
        public Error(Exception e)
        {
            Exception = e;
        }

        public bool IsOk => false;
        public Exception Exception { get; }
    }
}
