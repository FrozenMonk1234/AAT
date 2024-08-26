
string sql = $"SELECT TOP 1000000 * FROM received WHERE status = 1 ORDER BY re_ref";
/*list of sql nodes to query
Direct index access can reduce the amount of time needed to retreive the collections by
directly retreiving the data instead of having to filter 
*/
IEnumerable<IConfigurationSection> SqlNodes = Program.Configuration.GetValue("ConnectionStrings:SqlNodes").GetChildren();

//merged results set
/*An Array can be accessed faster.
//Since we know how big our dataSet is,
//we can initialize the object with control on maximum memory allocation.
*/
received[] results = new received[1000000];

Parallel.ForEach(SqlNodes, Node =>
{
    received[] result = DBQuery<received>.Query(Node.Value, sql); //internal function to query db and return results
    results.AddRange(result);
});

//run though results and insert into table
/*
 Moving the foreach into the using statemenent allows one connection to be used
for all sql executions instead of having to reastablish a new connection for each execution.
This will then reduce CPU overhead. 
 */
using (SqlConnection connection = new(ConnectionString))
{
    using SqlCommand command = new();

    StringBuilder sb = new StringBuilder();
    connection.Open();
    foreach (received rec in results)
    {
        /*
         The string builder creates one string that does not need to be recreated when being manipulated.
        This is mutablility is beneficial when you have to manipulate a string multiple times in an iteration 
        without creating excess memory usage.
         */
        sb.Clear();
        sb.AppendLine("INSERT INTO received_total (rt_msisdn, rt_message) VALUES ('");
        sb.AppendLine(rec.re_fromnum);
        sb.AppendLine("', '");
        sb.AppendLine(rec.re_message);
        sb.AppendLine(")");

        command.Connection = connection;
        command.CommandText = sb.ToString();
        command.ExecuteNonQuery();
    }
    /* 
     defining connection.Close(); is not necessary. block using statement
    manage clean up of resources used when exiting execution.
    */
}
