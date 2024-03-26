namespace WebApplication7.Models
{
    public class DataClientCursor
    {
        public int TotalRecords;
        public int Iteration;
        public bool NextIterationPossible;
        public DataClientCursor() { 
            TotalRecords = 0;
            Iteration = 0;
            NextIterationPossible = true;
        }
    }
}
