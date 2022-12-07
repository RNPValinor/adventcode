namespace _2022.Utils;

public class Directory : IFile
{
    private readonly string _name;
    private readonly Directory? _parent;
    private readonly List<IFile> _children = new();

    private int _cachedSize;

    public Directory(Directory? parent, string name)
    {
        _parent = parent;
        _name = name;
    }

    public int GetSize()
    {
        if (this._cachedSize is 0)
            this._cachedSize = this._children.Sum(c => c.GetSize());
        
        return this._cachedSize;
    }

    public string GetName()
    {
        return this._name;
    }

    public Directory GetParent()
    {
        if (this._parent is null)
            throw new NullReferenceException($"Attempted to get parent for {this.GetName()}, but parent is null!");
        
        return this._parent;
    }

    public Directory GetChildDirectory(string name)
    {
        var child = this._children.FirstOrDefault(c => c.GetName() == name);

        if (child is Directory childDirectory)
        {
            return childDirectory;
        }

        throw new DirectoryNotFoundException($"Failed to find directory {name} within {this.GetName()}");
    }

    public IEnumerable<Directory> GetDirectoryChildren()
    {
        return this._children.Where(c => c is Directory dir).Cast<Directory>();
    }

    public void AddChild(IFile child)
    {
        this._cachedSize = 0;
        this._children.Add(child);
    }
}