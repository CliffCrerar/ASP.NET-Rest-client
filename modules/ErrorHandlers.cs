/* 
 * DEMO REST CLIENT 
 * Author: Cliff crerar
 * Date: 22 April 2019
*/

using System;
namespace ErrorHandlers
{

    public class ErrorHandler
    {
        //public Action Operation;
        private readonly string errStartText = "### DEMO RESTClient ERROR: ";
        private readonly string errPrefix = "| -> ERROR Details:";
        private readonly string errHead = "|----------------ERROR DETAILS---------------|";
        private readonly string errEnd = "|--------------------END---------------------|";
        public class Error

        {
            public Exception err;
        }

        private void StartError()
        {
            var startText = errStartText;
            Console.WriteLine(' ');
            Console.WriteLine(startText);
        }

        private void WriteHead()
        {
            Console.WriteLine(errHead);
            Console.WriteLine(errPrefix);
        }

        private void WriteEnd()
        {
            Console.WriteLine(errEnd);
            Console.WriteLine(' ');
        }
        public void Handle(Exception ecxeption)
        {
            {
                StartError();
                WriteHead();
                Console.WriteLine(ecxeption);
                WriteEnd();
            }
        }
    }
}
