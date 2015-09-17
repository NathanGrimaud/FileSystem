using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class File
    {
        public int permission { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public Directory parent { get; set; }

        public List<File> childList = new List<File>();

        public bool isRoot;

        public File(Directory parent,string name)
        {
            this.parent = parent;
            this.name = name;
            this.permission = 4;
            isRoot = false;
            if(this.parent.GetType() != null){
                this.parent.childList.Add(this);
                var fullPath = "";
                fullPath = this.parent.path + "/" + name;
                path = fullPath;
            }
        }
        public File() { }  
        public Directory toDirectory()
        {
            return ((Directory)this);
        }
        public void getName()
        {
            if (this.canRead())
            {
                Console.WriteLine("   " + this.name); 
            }
            else
                Console.WriteLine("Permissions insufisantes :@");
        }
        

        public void chmod(int perm)
        {

            if (0<=perm&&perm<=7)
                this.permission = perm;
            else
                Console.WriteLine("Cette permission n'existe pas !");
        }
        public bool canWrite()
        {
            return (this.permission & 2) > 0;
        }
        public bool canExecute()
        {
            return (this.permission & 1) > 0;
        }
        public bool canRead()
        {
            return (this.permission & 4) > 0;
        }
        public File getRoot()
        {
            File root = new File();            
            root = (File)this;
            while (root.isRoot == false)
            {
                root = root.parent;
            }
            return root;
        }
 
        public File cd(string options)
        {

            File objective = new File();
            if (this.canRead())
            {               
                bool find = false;
                if (childList.Count > 0)
                {
                    childList.ForEach(delegate(File d)
                    {
                        if (d.name == options)
                        {
                            objective = d;
                            find = true;
                        }
                    });
                }
                else
                {
                    Console.WriteLine("Empty directory");
                }
                if (!find)
                {
                    Console.WriteLine("Impossible de se déplacer dans " + options);
                }
            }
            
            return objective;
        }
        public bool getDir()
        {
            bool f;
            if (this.GetType() == typeof(Directory))
            {
                f = true;
            }
            else
            {
                f = false;
            }                
                return f;
        }
        public bool getFile()
        {
            bool f = false;
            if (this.canRead() && this.GetType() == typeof(File))
                f = true;
            return f;
        }
        public Directory getParent()
        {
                if (this.parent != null)
                    return this.parent;
                else
                {
                    Console.WriteLine("You shall not pass !");
                    return this.toDirectory();
                }         
        }
        public List<File> ls()
        {
            if (this.canRead())
            {
                if (childList.Count > 0 && this.canRead())
                {
                    return this.childList;
                }
                else
                {
                    if(!this.getFile())
                        Console.WriteLine("Dossier vide");
                }
            }
            List<File> l = new List<File>();
            return l;
        }
    }
}
