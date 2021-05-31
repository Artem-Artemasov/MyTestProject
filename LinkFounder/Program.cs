using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Services;
using System;
using System.Collections.Generic;

namespace UKAD
{
   
    class Program
    {
        public static void Main(string[] args)
        {
            var RequestService = new RequestService();
            var LinkParser = new LinkParser();
            var LinkConverter = new LinkConverter();
            var LinkValidator = new LinkValidator();

            LinkConverter.RelativeToAbsolute(new List<string> { "/" }, "https://example.com/books");

            Console.WriteLine();
        }
    }
}
