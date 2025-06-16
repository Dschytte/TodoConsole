using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Threading;



namespace ToDo
{
    internal class Program
    {
        static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyToDo.txt");
        static List<string> ToDoItems = new List<string>();
        static void Main(string[] args)
        {

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Your ToDo list");
                }
            }

            using (StreamReader sr = File.OpenText(path))
            {
                string addToList = "";
                while ((addToList = sr.ReadLine()) != null)
                {
                    ToDoItems.Add(addToList);
                }
            }

            Console.WriteLine("ReadAll : Read the hole list");
            Console.WriteLine("Add : Add an objective to your list");
            Console.WriteLine("Remove : removes an objectiv from your list");
            Console.WriteLine("ToDay : Reads out the deadlines for the day");
            Console.WriteLine("Edit : Edits an item on your list");
            Console.WriteLine("DL : Edit the deadline for an objectiv on your list");
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
                        ReadAll();
                        break;

                    case "add":
                        AddItem();
                        break;

                    case "remove":
                        RemoveItem();
                        break;

                    case "today":
                        ToDay();
                        break;

                    case "timer":
                        Pomodoro();
                        break;

                    case "edit":
                        EditObj();
                        break;

                    case "dl":
                        EditDeadline();
                        break;

                    default:
                        Console.WriteLine("your command did match any of the options, try again");
                        break;
                }

            }
        }
        static void ReadAll()
        {
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
        }
        static void AddItem()
        {
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
                            Console.WriteLine("A ToDo was addded to the list");
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
        }
        static void RemoveItem()
        {
            ReadAll();
            Console.WriteLine("What do you want to remove from your list?");
            string itemToRemove = Console.ReadLine();
            for (int i = 0; i < ToDoItems.Count; i++)
            {
                if (ToDoItems[i].Contains(";"))
                {
                    string[] split = ToDoItems[i].Split(';');
                    string txt = split[0];
                    if (txt == itemToRemove)
                    {
                        ToDoItems.RemoveAt(i);
                        File.WriteAllLines(path, ToDoItems);
                        Console.WriteLine("Item removed!");
                        break;
                    }
                }
            }
        }
        static void ToDay()
        {
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
        }
        static void EditObj()
        {
            ReadAll();
            Console.WriteLine("What item do you want to edit?");
            string edit;
            edit = Console.ReadLine();
            Console.WriteLine("new to do");
            string newTodo;
            newTodo = Console.ReadLine();
            for (int i = 0; i < ToDoItems.Count; i++)
            {
                if (ToDoItems[i].Contains(";"))
                {
                    string[] split = ToDoItems[i].Split(';');
                    string txt = split[0];
                    string dat = split[1];
                    if (txt == edit)
                    {
                        ToDoItems[i] = newTodo + ";" + dat;
                        File.WriteAllLines(path, ToDoItems);
                        Console.WriteLine("Item updated");
                        break;
                    }
                }

            }
        }
        static void Pomodoro()
        {
            Console.WriteLine("25 minut timer for focus starts now");
            for (int i = 25; i > 0; i--)
            {
                Console.WriteLine($"{i} minuts left... ");
                Thread.Sleep(60000);
            }
            Console.WriteLine("Pomodoro timer ended");

        }
        static void EditDeadline()
        {
            Console.WriteLine("What ToDo do you want to edit the deadline for?");
            ReadAll();
            string opgaven = Console.ReadLine();

            Console.WriteLine("And the new deadline? in format dd/mm/yyyy or ddmmyyyy");
            string input = Console.ReadLine().Replace("/", "");

            if (input.Length == 8 && input.All(char.IsDigit))
            {
                string formatDate = input.Substring(0, 2) + "/" +
                                    input.Substring(2, 2) + "/" +
                                    input.Substring(4, 4);

                if (DateTime.TryParse(formatDate, out DateTime newDeadline))
                {
                    for (int i = 0; i < ToDoItems.Count; i++)
                    {
                        if (ToDoItems[i].Contains(";"))
                        {
                            var split = ToDoItems[i].Split(';');
                            if (split[0] == opgaven)
                            {
                                ToDoItems[i] = opgaven + ";" + newDeadline.ToString("yyyy-MM-dd");
                                File.WriteAllLines(path, ToDoItems);
                                Console.WriteLine("Deadline updated");
                                return;
                            }
                        }
                    }
                    Console.WriteLine("ToDo not found.");
                }
                else
                {
                    Console.WriteLine("Forkert datoformat.");
                }
            }
        }
    }
}

