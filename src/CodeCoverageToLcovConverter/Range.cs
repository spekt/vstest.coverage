namespace Spekt.Vstest.Coverage
{
    public class Range
    {
        public int StartLine;

        public int EndLine;

        public bool Covered;

        /// <summary>
        /// It is the index of the source file in the list
        /// </summary>
        public int SourceFileId;

        public Range()
        {

        }

        public Range(int startLine, int endLine, bool covered, int sourceFileId)
        {
            StartLine = startLine;
            EndLine = endLine;
            Covered = covered;
            SourceFileId = sourceFileId;
        }

        public override string ToString()
        {
            return $"{StartLine} {EndLine} {Covered} {SourceFileId}";
        }
    }
}