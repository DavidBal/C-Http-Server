using System;
namespace Server
{
    public class ServerException : Exception
    {
        String message;

        public ServerException(String message)
        {
            this.message = message;
        }

        public String GetMessage() => this.message;
    }
}
