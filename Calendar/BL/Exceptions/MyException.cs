namespace BL.Exceptions
{
    public class MyException : Exception
    {
        public MyException() : base() { }
        public MyException(string message) : base(message) { }
        public MyException(string message, Exception e) : base(message, e) { }
    }

    public class NoRecord : MyException
    {
        public NoRecord() : base("No record in database") { }
    }

    public class UnpredictableException : MyException
    {
        public UnpredictableException() : base("UnpredictableException") { }
        public UnpredictableException(Exception e) : base("UnpredictableException", e) { }
    }

    public class NoDBConnection : MyException
    {
        public NoDBConnection() : base("No database connection") { }
    }

    public class ExistingName : MyException
    {
        public ExistingName() : base("This name in database already exists") { }
    }

    public class BusyDate : MyException
    {
        public BusyDate() : base("This date in database is busy") { }
    }

}
