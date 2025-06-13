using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;



namespace ToDo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\danni\documents\MyToDo.txt";
            if (!File.Exists(path))
            {
                //create a file to write to
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Your ToDo list \n Your Current options \n");
                }
            }
            List<string> ToDoItems = new List<string>();

            using (StreamReader sr = File.OpenText(path))
            {
                string addToArray = "";
                while ((addToArray = sr.ReadLine()) != null)
                {
                    ToDoItems.Add(addToArray);
                }
            }

            Console.WriteLine("ReadAll : Read the hole list");
            Console.WriteLine("Add : Add an objective to your list");
            Console.WriteLine("Remove : removes an objectiv from your list");
            Console.WriteLine("ToDay : Reads out the deadlines for the day");
            Console.WriteLine("Timer : Set an 25 timer for focus");
            Console.WriteLine("Exit : This will close the app");

            while (true)
            {
                string option = Console.ReadLine().ToLower();
                if (option == "exit")
                    break;
                switch (option)
                {
                    case "readall":
                        using (StreamReader sr = File.OpenText(path))
                        {
                            string LineToRead = "";
                            while ((LineToRead = sr.ReadLine()) != null)
                            {
                                string[] split = LineToRead.Split(';');
                                string txt = split[0];
                                Console.WriteLine(txt);
                            }
                        }
                        break;

                    case "add":
                        string Opgaven;
                        Console.WriteLine("What do you want to add to the list?");
                        Opgaven = Console.ReadLine();

                        Console.WriteLine("whats the deadline? in format dd/mm/yyyy or ddmmyyyy");
                        string date = Console.ReadLine();
                        date = date.Replace("/", "");
                        DateTime deadline;
                        if (date.Length == 8 && date.All(char.IsDigit))
                        {
                            string formatDate = date.Substring(0, 2) + "/" +
                                                date.Substring(2, 2) + "/" +
                                                date.Substring(4, 4);
                            if (DateTime.TryParse(formatDate, out deadline))
                            {
                                Console.WriteLine($"do you want to add the following?" +
                                    $" \n {Opgaven} \n {deadline} \n YES/NO");
                                if (Console.ReadLine().ToLower() != "no")
                                {
                                    try
                                    {
                                        string[] nyeOpg = { Opgaven + ";" +
                                                deadline.ToString("yyyy-MM-dd") };
                                        File.AppendAllLines(path, nyeOpg);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Exception: " + ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid date");
                            }
                        }
                        break;

                    case "remove":
                        Console.WriteLine("What do you want to remove from your list?");
                        string itemToRemove;
                        itemToRemove = Console.ReadLine();
                        if (ToDoItems.Contains(itemToRemove))
                        {
                            ToDoItems.Remove(itemToRemove);
                            File.WriteAllLines(path, ToDoItems);

                        }
                        break;

                    case "today":
                        Console.WriteLine("Your Objectivs for ToDay");
                        DateTime today = DateTime.Now;
                        using (StreamReader sr = File.OpenText(path))
                        {
                            string LineToRead = "";
                            while ((LineToRead = sr.ReadLine()) != null)
                            {
                                if (LineToRead.Contains(";"))
                                {
                                    string[] split = LineToRead.Split(';');
                                    if (split[1] == today.ToString("yyyy-MM-dd"))
                                    {
                                        string txt = split[0];
                                        Console.WriteLine(txt);
                                    }
                                }

                            }
                        }
                        break;

                    case "timer":
                        Console.WriteLine("Set a 25 minut timer for focus \n It starts now");
                        break;

                    case "edit":
                        Console.WriteLine("What item do you want to edit?");

                        break;

                    default:
                        Console.WriteLine("your command did match any of the options, try again");
                        break;
                }

            }
        }
    }
}
