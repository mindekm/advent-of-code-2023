var input = File.ReadAllLines("input1.txt");

var result1 = input
    .Select(Game.Parse)
    .Where(g => g.Sets.All(s => s is { Red: <= 12, Green: <= 13, Blue: <= 14 }))
    .Sum(g => g.Id);

var result2 = input
    .Select(Game.Parse)
    .Select(g =>
    {
        return new
        {
            MinimumRed = g.Sets.Max(s => s.Red),
            MinimumGreen = g.Sets.Max(s => s.Green),
            MinimumBlue = g.Sets.Max(s => s.Blue),
        };
    })
    .Select(v => v.MinimumRed * v.MinimumGreen * v.MinimumBlue)
    .Sum();

Console.WriteLine(result1);
Console.WriteLine(result2);

record Game(int Id, List<GameSet> Sets)
{
    public static Game Parse(string input)
    {
        var split = input.Split(':');
        if (split.Length != 2)
        {
            throw new InvalidOperationException();
        }

        var id = int.Parse(split[0].Replace("Game ", string.Empty));
        var sets = split[1]
            .Split(";")
            .Select(s => s.Split(",").Select(v => v.Trim()).ToList())
            .Select(game =>
            {
                var red = 0;
                var green = 0;
                var blue = 0;

                foreach (var split in game.Select(cubes => cubes.Split(" ")))
                {
                    if (split[1] == "red")
                    {
                        red += int.Parse(split[0]);
                    }

                    if (split[1] == "green")
                    {
                        green += int.Parse(split[0]);
                    }

                    if (split[1] == "blue")
                    {
                        blue += int.Parse(split[0]);
                    }
                }

                return new GameSet(red, green, blue);
            })
            .ToList();

        return new Game(id, sets);
    }
};

record GameSet(int Red, int Green, int Blue);