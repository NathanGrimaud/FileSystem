using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class Directory : File
    {
        public Directory(Directory parent,string name)
            : base( parent,name)
        { }
        public Directory() : base() 
        { }
        public Directory(string name)//pour la racine
        {
            this.isRoot = true;
            this.name = name;
            this.path = "/" + name;
        }        
        public bool mkdir( string name)
        {
            bool nexist = this.notExist(name);
            if (this.canWrite() && nexist)
            {
                if (name != "")
                {
                    File n = new Directory(this, name);
                    return true;
                }
            }
            else if(this.canWrite()==false && nexist == true)
                Console.WriteLine("Permissions insufissantes 0_o");
            return false;
        }
        public bool notExist(string name)
        {
            bool can = true;
            foreach (File file in this.childList)
            {
                if (name == file.name)
                {
                    can = false;
                    Console.WriteLine("Un élément avec le même nom existe déja ");
                }
                    
            }
            return can;
        }

        public string getPath()
        {
            if (this.canRead())
            {
                return this.path;
            }
            else return ("Permissions trop faible B-)");
        }
        public List<File> search(string name, List<File> list)
        {
            if (this.canRead())
            {
                this.childList.ForEach(delegate(File child)
                {
                    if (name == child.name)
                        {
                            list.Add(child);
                        }     
                    Directory enfant;
                    if (child.getDir())
                    {
                        enfant = (Directory)child;
                        list = enfant.search(name, list);
                    }
                });
            }
            return list;
        }
        public bool create(string name)
        {
            bool create = false;
             bool nexist = this.notExist(name);
            if (this.canWrite() &&  nexist)
            {
                if (name != "")
                {
                    File fichier = new File((Directory)this, name);
                    create = true;
                }
            }
            else if (this.canWrite() == false && nexist == true)
                Console.WriteLine("Permissions insufissantes :{");
            return create;
        }
        public bool rename(string name, string futureName)
        {
            bool reussi = false;
            if (this.canWrite())
            {
                List<string> option = name.Split().ToList();
                this.childList.ForEach(delegate(File child)
                {
                    if (child.name == name)
                    {
                        child.path = child.path.Replace(name, futureName);
                        child.name = futureName;
                        reussi = true;
                    }
                });
            }
            else
            {
                Console.WriteLine("Permissions insufisantes 8=(");
                reussi = false;
            }
            return reussi;
        }
        public bool delete(string name)
        {
            bool found = false;
            if (this.canWrite())
            {
                File toRemove = new File();
                this.childList.ForEach(delegate(File child)
                {
                    if (child.name == name)
                    {
                        destroy(child);
                        toRemove = child;
                    }
                });
                toRemove.parent.childList.Remove(toRemove);
                toRemove = null;
                found = true;
            }
            else
                Console.WriteLine("Permissions insufisantes :p");
            return found;
        }
        public void destroy(File f)
        {
            for (int i = 0; i < f.childList.Count; i++)
            {
                var child = f.childList[i];
                if (child.childList.Count > 0)
                {
                    destroy(child);
                }
                else
                {
                    child.parent.childList.Remove(child);
                    child = null;
                }
            }
        }

  

    }
}
