using Utilities;

var lines = File.ReadAllLines("input1.txt");
var data = new char[lines.Length][];
for (var i = 0; i < lines.Length; i++)
{
    data[i] = lines[i].ToCharArray();
}

var windows = new List<Window>();
var gears = new List<Coordinate>();
var result1 = 0;

for (var i = 0; i < lines.Length; i++)
{
    var array = data[i];
    for (var j = 0; j < array.Length; j++)
    {
        var c = array[j];
        if (c == '*')
        {
            gears.Add(new Coordinate(i, j));
        }
    }
    
    var index = 0;
    while (TryFindNextPosition(array, index, char.IsNumber).TryUnwrap(out var start))
    {
        var end = TryFindNextPosition(array, start, c => !char.IsNumber(c)).UnwrapOr(array.Length);

        var number = int.Parse(array.AsSpan().Slice(start, end - start));

        var upperLeftCorner = new Coordinate(Math.Max(0, i - 1), Math.Max(0, start - 1));
        var lowerRightCorner = new Coordinate(Math.Min(lines.Length - 1, i + 1), Math.Min(array.Length - 1, end));
        var window = new Window(upperLeftCorner, lowerRightCorner, number);
        windows.Add(window);
        var isValid = window.Contents(data).Any(c => !char.IsNumber(c) && c != '.');
        if (isValid)
        {
            result1 += number;
        }

        index = end;
    }
}

Console.WriteLine(result1);

var result2 = 0;
foreach (var gear in gears)
{
    var adjacentWindows = windows.Where(w => w.Contains(gear)).ToList();
    if (adjacentWindows.Count != 2)
    {
        continue;
    }

    result2 += adjacentWindows.Aggregate(1, (initial, window) => initial * window.Number);
}

Console.WriteLine(result2);


Maybe<int> TryFindNextPosition(ReadOnlySpan<char> input, int start, Func<char, bool> predicate)
{
    for (var i = start; i < input.Length; i++)
    {
        if (predicate(input[i]))
        {
            return Maybe.Some(i);
        }
    }

    return Maybe.None;
}

record Coordinate(int X, int Y);

record Window(Coordinate Start, Coordinate End, int Number)
{
    public IEnumerable<char> Contents(char[][] data)
    {
        for (var i = Start.X; i <= End.X; i++)
        {
            for (var j = Start.Y; j <= End.Y; j++)
            {
                yield return data[i][j];
            }
        }
    }
    
    public bool Contains(Coordinate coordinate)
    {
        if (coordinate.X >= Start.X && coordinate.X <= End.X && coordinate.Y >= Start.Y && coordinate.Y <= End.Y)
        {
            return true;
        }

        return false;
    }
}