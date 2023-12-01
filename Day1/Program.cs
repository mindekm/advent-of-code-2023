var input = File.ReadAllLines("input1.txt");
Console.WriteLine(PartOne(input));
Console.WriteLine(PartTwo(input));
return;

int PartOne(IEnumerable<string> lines)
{
    var result = 0;
    foreach (var line in lines)
    {
        var digits = line.Where(char.IsDigit).ToList();
        if (digits.Count < 1)
        {
            throw new InvalidOperationException();
        }

        result += int.Parse([digits[0], digits[^1]]);
    }

    return result;
}

int PartTwo(IEnumerable<string> lines)
{
    List<Replacement> replacements =
    [
        new Replacement("one", "one1one"),
        new Replacement("two", "two2two"),
        new Replacement("three", "three3three"),
        new Replacement("four", "four4four"),
        new Replacement("five", "five5five"),
        new Replacement("six", "six6six"),
        new Replacement("seven", "seven7seven"),
        new Replacement("eight", "eight8eight"),
        new Replacement("nine", "nine9nine"),
    ];

    var replaced = lines.Select(l =>
    {
        foreach (var replacement in replacements)
        {
            l = l.Replace(replacement.SpelledOut, replacement.Replace);
        }

        return l;
    });

    return PartOne(replaced);
}

record Replacement(string SpelledOut, string Replace);