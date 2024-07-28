namespace Brainf1ck_IDE.Domain
{
    public struct ExecutionStepInfo
    {
        public string Input { get; set; }
        public string Output { get; set; }
        public string CellIndex { get; set; }
        public string CellValue { get; set; }        
    }
}
