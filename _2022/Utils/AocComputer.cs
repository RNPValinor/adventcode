using System.Text.RegularExpressions;

namespace _2022.Utils;

public class AocComputer
{
    private readonly Directory _root;
    private Directory _currentDirectory;
    
    private readonly Regex _commandRegex = new("^\\$ ([a-z]+) ?(.*)?$");
    private readonly Regex _fileRegex = new("^(dir|[0-9]+) (.*)$");

    

    public AocComputer()
    {
        _root = new Directory(null, "/");
        this._currentDirectory = this._root;
    }

    public void ProcessTerminalLine(string line)
    {
        if (line.StartsWith('$'))
        {
            // Command
            var match = this._commandRegex.Match(line);

            if (!match.Success)
            {
                throw new ArgumentException($"Unrecognized console line: {line}");
            }

            var command = match.Groups[1].Value;

            switch (command)
            {
                case "cd":
                    var directory = match.Groups[2].Value;
                    this.ChangeDirectory(directory);
                    break;
                case "ls":
                    break;
                default:
                    throw new ArgumentException($"Unrecognized command: {command}");
            }
        }
        else
        {
            // File or directory
            var match = this._fileRegex.Match(line);

            if (!match.Success)
            {
                throw new ArgumentException($"Unrecognized console line: {line}");
            }

            var dirOrFileSize = match.Groups[1].Value;
            var name = match.Groups[2].Value;

            if (dirOrFileSize is "dir")
            {
                this._currentDirectory.AddChild(new Directory(this._currentDirectory, name));
            }
            else
            {
                if (!int.TryParse(dirOrFileSize, out var size))
                {
                    throw new ArgumentException("Failed to parse file size");
                }
                
                this._currentDirectory.AddChild(new PlainFile(size, name));
            }
        }
    }

    public Directory GetRoot()
    {
        return this._root;
    }

    public void AddFileToCurrentDirectory(IFile file)
    {
        this._currentDirectory.AddChild(file);
    }

    public void ChangeDirectory(string path)
    {
        this._currentDirectory = path switch
        {
            "/" => this._root,
            ".." => this._currentDirectory.GetParent(),
            _ => this._currentDirectory.GetChildDirectory(path)
        };
    }
}