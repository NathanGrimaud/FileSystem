using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class Program
    {

        static Directory C = new Directory("root") { path = "#" ,permission = 7};
        static File location = C;
        static void Main(string[] args)
        {          
            while (true)
            {
                getCommand(location);
            }
            Console.ReadLine();
        }
        public static void getCommand(File location)
        {
            Console.Write(location.path + "/> ");
            string command = Console.ReadLine();
            List<string> instruction = command.Split().ToList();
            if (instruction.Count < 2)
            {
                instruction.Add(""); ;
            }
            getInstruction(instruction[0], location, instruction);
        }
        public static void getInstruction(string instruction, File Current, List<string> options)
        {
            File old = Current;
            var dir = new Directory();
            if(Current.getDir())
                 dir = (Directory)Current;
            switch (instruction)
            {
                case "ls":
                    List<File>ls =new List<File>();
                    ls = Current.ls();
                    if (ls.Count == 0 && Current.getFile())
                        Console.WriteLine("Vous etes dans un fichier");
                    else
                    if (ls.Count > 0)
                    {
                        ls.ForEach(delegate(File f){
                            if (f.canExecute())
                                Console.Write("e");
                            Console.Write('-');
                            if (f.canRead())
                                Console.Write("r");
                            Console.Write("-");
                            if (f.canWrite())
                                Console.Write("w");
                            Console.Write("-");
                            Console.Write("   (" + f.GetType().ToString().Replace("FileSystem.", " ").Substring(0,4) + " )   " + f.path + "\n");
                        });
                    }
                    break;
                case "cd":
                    location = Current.cd(options[1]);
                    if (location.path == null)
                        location = old;
                    break;
                case "type":
                    String type = Current.GetType().ToString();
                    string[] t = type.Split('.');
                    Console.WriteLine(t[1]);
                    break;
                case "mkdir":
                    bool created = false;
                    if (Current.getDir())                                     
                        created = dir.mkdir(options[1]);                                             
                    else
                        Console.WriteLine("Impossible de créer quelque chose dans un fichier");                 
                    break;
                case "parent":
                    location = Current.getParent();
                    break;
                case "root":
                    File root = new File();
                    root =Current.getRoot();                    
                    location = (Directory)root;
                    break;
                case "create":
                    bool c = false;
                    if(Current.getDir())
                        c= dir.create(options[1]);
                    if (!c)
                        Console.WriteLine("    Une erreur c'est produite");
                    break;
                case "rename":
                    bool r = false;
                    if(Current.getDir())
                      r = dir.rename(options[1],options[2]);
                    if (r)
                        Console.WriteLine("    " + options[1] + " a été renommé en : " + options[2]);
                    else
                        Console.WriteLine("    Impossible de renommer ce fichier.");
                    break;
                case "path":
                    string path = dir.getPath();
                    Console.WriteLine("   " + path);
                    break;
                case "name":
                    Current.getName();
                    break;
                case "file":               
                    break;
                case "directory":                   
                    if (Current.getDir())
                        Console.WriteLine("C'est un dossier");
                    else
                        Console.WriteLine("Ce n'est pas un dossier");
                    break;
                case "search":
                    var  list = new List<File>();
                    if (Current.getDir())
                    dir.search(options[1],list);
                    list.ForEach(delegate(File f)
                    {
                        Console.WriteLine("   found : " + f.GetType().ToString().Replace("FileSystem.", " ").Substring(0,4) + "  "+f.path);
                    });
                    break;
                case "delete":       
                    bool deleted = false;
                    if(Current.getDir())
                        deleted = dir.delete(options[1]);
                    if (deleted)
                        Console.WriteLine("Le dossier " + options[1] + " a été érradiqué");
                    else
                        Console.WriteLine("Le dossier n'a pas été trouvé :(");
                    break;
                case "chmod":
                    if(dir.isRoot==true)
                    {
                        Console.WriteLine("Impossible de faire un chmod sur la racine");
                        break;
                    }
                    int number;
                    if (int.TryParse(options[1].ToString(), out number))
                        Current.chmod(int.Parse(options[1]));
                    else
                        Console.WriteLine("    You shall not parse !");
                    break;
                default:
                    Console.WriteLine("Commande inconnue");
                    break;
            }
        }

    }
}
