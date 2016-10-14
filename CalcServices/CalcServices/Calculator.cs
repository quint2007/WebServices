namespace CalcService
{
    public class Calculator : ICalculator
    {
        public Result Add(Arguments args)
        {
            return new Result {Value = args.Arg1 + args.Arg2};
        }
    }
}