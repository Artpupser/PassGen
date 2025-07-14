using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace PassGen;

public sealed class ProjectContext
{
    public static Terminal Terminal { get; private set; }
    public static async Task Main(string[] args) => await new ProjectContext().Start();
    
    private async Task Start()
    {
        Terminal = new Terminal("PassGen", ConsoleColor.White, ConsoleColor.Black);
        IReadOnlyList<(object, PassCoreIdAttribute)> allCores = AttributeUtils.GetInstancesAndAttrs<PassCoreIdAttribute>().ToList();
        var ids = string.Join(',', allCores.Select(x => x.Item2.Id));
        while (true)
        {
            var coreName = Terminal.Input($"Выбери ID: {ids}");
            if (allCores.All(x => x.Item2.Id != coreName))
            {
                Terminal.OutL($"Core with name [{coreName}] not found");
                continue;
            }

            var core = allCores.First(x => x.Item2.Id == coreName).Item1;
            var result = (PassResult)((dynamic)core).Generate();
            Terminal.OutL(result.GetPassCheck(), ConsoleColor.Yellow);
        }
    }
}

[PassCoreId("alpha")]
public sealed class PassCoreAlpha : PassCore<PassResultAlpha>
{
    public override PassResultAlpha Generate()
    {
        var codeKey = ProjectContext.Terminal.Input("Ключ. слово");
        l:
        var length = ProjectContext.Terminal.Input("Длинна");
        if (!int.TryParse(length, out var res))
        {
            ProjectContext.Terminal.OutL($"Content [{length}] not parsed to int32");
            goto l;
        }
        if (res > Math.Pow(2, 13))
        {
            ProjectContext.Terminal.OutL($"Content [{length}] huge {Math.Pow(2, 13)} symbols");
            goto l;
        }
        var hash = new StringBuilder();
        for (var i = 0; i < res + 1 % Hash256('*').Length; i++)
            hash.Append(Hash256(codeKey));
        var ex = hash.ToString()[..res];
        hash.Clear();
        var rnd = new Random(codeKey.Select(x => (byte)x).Sum(x => x % 2 == 0 ? x + res : x  - res));
        while (ex.Length != hash.Length)
            hash.Append(ex[rnd.Next(0, ex.Length)]);
        return new PassResultAlpha(hash.ToString(), $"\n\tСекретный ключ: {codeKey}\n\tДлинна пароля: {length}");
    }
}

public sealed class PassResultAlpha(string pass, string info) : PassResult
{
    private readonly DateTime _timeCreation = DateTime.Now;
    public override string GetPass() => pass;
    public override string GetInfo() => info;
    public override DateTime GetDate() => _timeCreation;
}

public static class AttributeUtils
{
    public static IEnumerable<T> GetAttrs<T>() where T : Attribute
    {
        return Assembly.GetEntryAssembly()!.GetTypes().Where(x => x.GetCustomAttribute<T>() != null)
            .Select(x => x.GetCustomAttribute<T>());
    }
    public static IEnumerable<(object, TAttr)> GetInstancesAndAttrs<TAttr>() 
        where TAttr : Attribute
    {
        return Assembly.GetEntryAssembly()!.GetTypes().Where(x => x.GetCustomAttribute<TAttr>() != null)
            .Select(x => (Activator.CreateInstance(x), x.GetCustomAttribute<TAttr>()));
    }
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class PassCoreIdAttribute(string id) : Attribute
{
    public string Id { get; } = id;
}
public abstract class PassResult
{
    public abstract string GetPass();
    public abstract string GetInfo();
    public abstract DateTime GetDate();
    public string GetPassCheck()
    {
        return $"<< START PASS CHECK >>\nCORE: {GetType().Name[10..]}\nCREATION DATE: {GetDate()}\nPASSWORD: {GetPass()}\nINFORMATION: {GetInfo()}\n<< END PASS CHECK >>";
    }
}
public abstract class PassCore<T> where T : PassResult
{
    public abstract T Generate();

    protected static string Hash256(object content)
    {
        return string.Join(string.Empty, SHA256.HashData(Encoding.UTF8.GetBytes(content.ToString() ?? string.Empty)).Select(b => b.ToString("x2")));
    }
    
    protected static string HashMD5(object content)
    {
        return string.Join(string.Empty, MD5.HashData(Encoding.UTF8.GetBytes(content.ToString() ?? string.Empty)).Select(b => b.ToString("x2")));
    }
}

public sealed class Terminal
{
    private readonly string _title;
    private readonly ConsoleColor _fgColor;
    private readonly ConsoleColor _bgColor;
    
    public Terminal(string title, ConsoleColor foreground, ConsoleColor background)
    {
        _title = title;
        _fgColor = foreground;
        _bgColor = background;
    }
    
    public void Out(object content) => Console.Out.Write(content);
    public void OutL(object content) => Console.Out.WriteLine(content);

    public void OutL(object content, ConsoleColor fg)
    {
        Console.ForegroundColor = fg;
        Console.Out.WriteLine(content);
        Console.ForegroundColor = _fgColor;
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
    
    public void UseDefault()
    {
        Console.Title = _title;
        Console.ForegroundColor = _fgColor;
        Console.BackgroundColor = _bgColor;
    }
}