namespace AspUnitRunner.Core {
    internal interface IResultParser {
        IResults Parse(string htmlResults);
    }
}
