namespace _2022.Utils;

public class PlainFile : IFile
{
    private readonly string _name;
    private readonly int _size;

    public PlainFile(int size, string name)
    {
        _size = size;
        _name = name;
    }

    public string GetName()
    {
        return this._name;
    }

    public int GetSize()
    {
        return this._size;
    }
}