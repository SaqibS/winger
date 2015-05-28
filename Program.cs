namespace Winger
{
    using System;
    using System.Configuration;
    using System.DirectoryServices;

    internal static class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Usage: Winger <username>.");
                    return;
                }

                string username = args[0];
                SearchResult result = FindUser(username);
                if (result == null)
                {
                    Console.WriteLine("User {0} not found.", username);
                    return;
                }

                PrintDetails(result);
                    }
        catch (Exception x)
    {
        Console.WriteLine("Error: {0}", x.Message);
        }
        }

        private static SearchResult FindUser(string username)
        {
            var root = new DirectoryEntry(ConfigurationManager.AppSettings["ActiveDirectoryRoot"]);
            var searcher = new DirectorySearcher(root);
            searcher.Filter = "(&(objectClass=user)(samaccountname=" + username + "))";
            SearchResult result = searcher.FindOne();
            return result;
        }

        private static void PrintDetails(SearchResult user)
        {
            WriteLine(user.Properties, "{0}", "displayname");
            WriteLine(user.Properties, "{0}, {1}", "title", "department");
            WriteLine(user.Properties, "Email: {0}", "mail");
            WriteLine(user.Properties, "Phone: {0}", "telephonenumber");
            WriteLine(user.Properties, "Mobile: {0}", "mobile");
            WriteLine(user.Properties, "Office: {0}", "physicaldeliveryofficename");
        }

        private static void WriteLine(ResultPropertyCollection properties, string format, params string[] propertyNames)
        {
            string[] propertyValues = new string[propertyNames.Length];
            for (int i = 0; i < propertyNames.Length; i++)
            {
                if (properties.Contains(propertyNames[i]))
                {
                    propertyValues[i] = properties[propertyNames[i]][0].ToString();
                }
                else
                {
                    return; // Don't print lines with missing values.
                }
            }

            Console.WriteLine(format, propertyValues);
        }
    }
}
