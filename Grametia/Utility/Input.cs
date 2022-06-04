namespace Grametia.Utility;

public static class Input
{
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var result)) return result;
            Console.WriteLine("Please input a number!");
        }
    }

    public static long ReadLong(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (long.TryParse(Console.ReadLine(), out var result)) return result;
            Console.WriteLine("Please input a number!");
        }
    }

    public static void Prompt()
    {
        Console.Write("Press enter to continue...");
        Console.ReadLine();
    }

    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    public static string ReadPassword(string prompt)
    {
        Console.Write(prompt);
        var password = "";
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password.Remove(password.Length - 1);
                continue;
            }

            Console.Write("*");
            password += key.KeyChar;
        }

        WriteLine();
        return password;
    }

    public static bool TryAgain()
    {
        Console.Write("Try again? (y/n) : ");
        return Console.ReadLine() == "y";
    }

    public static void WriteLine(string line)
    {
        Console.WriteLine(line);
    }

    public static void WriteLine()
    {
        Console.WriteLine();
    }

    public static void WriteHeader(string header)
    {
        WriteLine(header);
        WriteLine(new string('=', header.Length));
    }

    public static void WriteSeparator(int length)
    {
        WriteLine(new string('=', length));
    }

    public static void Clear()
    {
        Console.Clear();
    }
}