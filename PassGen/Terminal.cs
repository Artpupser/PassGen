namespace PassGen;

public sealed class Terminal(string title, ConsoleColor foreground, ConsoleColor background)
{
    public void Out(object content) => Console.Out.Write(content);
    
    public void OutL(object content) => Console.Out.WriteLine(content);

    public void Out(object content, ConsoleColor fg)
    {
        Console.ForegroundColor = fg;
        Console.Out.Write(content);
        Console.ForegroundColor = foreground;
    }
    public void Out(object content, ConsoleColor fg, ConsoleColor bg)
    {
        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;
        Console.Out.Write(content);
        Console.BackgroundColor = background;
        Console.ForegroundColor = foreground;
    }
    public void OutL(object content, ConsoleColor fg)
    {
        Console.ForegroundColor = fg;
        Console.Out.WriteLine(content);
        Console.ForegroundColor = foreground;
    }
    public void Clear() => Console.Clear();
    public string Input(object prompt)
    {
        repeat:
        Out($"[{prompt}] > ");
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) return input;
        Out("Exception, your input is empty.");
        goto repeat;
    }
    public string Input(object prompt, ConsoleColor promptColor)
    {
        repeat:
        Console.ForegroundColor = promptColor;
        Out($"[{prompt}] > ");
        Console.ForegroundColor = foreground;
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) return input;
        Console.ForegroundColor = ConsoleColor.Red;
        Out("Exception, your input is empty.");
        Console.ForegroundColor = foreground;
        goto repeat;
    }
    
    public void UseDefault()
    {
        Console.Title = title;
        Console.ForegroundColor = foreground;
        Console.BackgroundColor = background;
    }
}