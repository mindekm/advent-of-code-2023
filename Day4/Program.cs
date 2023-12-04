var lines = File.ReadAllLines("input1.txt");

var result = 0;

foreach (var line in lines)
{
    var card = Card.Parse(line);
    result += card.CalculateScore();
}

Console.WriteLine(result);


var cards = lines.Select(Card.Parse).ToList();
var result2 = new int[lines.Length];
Array.Fill(result2, 1);
for (var i = 0; i < cards.Count; i++)
{
    var card = cards[i];
    for (var j = 0; j < result2[i]; j++)
    {
        foreach (var id in card.NextCards())
        {
            result2[id - 1] += 1;
        }
    }
}

Console.WriteLine(result2.Sum());


class Card
{
    private Card(int id, List<int> winningNumbers)
    {
        Id = id;
        WinningNumbers = winningNumbers;
    }

    public int Id { get; }

    public List<int> WinningNumbers { get; set; }
    
    public IEnumerable<int> NextCards()
    {
        return Enumerable.Range(Id + 1, WinningNumbers.Count);
    }

    public int CalculateScore()
    {
        var result = 0;
        foreach (var _ in WinningNumbers)
        {
            result = result == 0 ? 1 : result * 2;
        }

        return result;
    }
    
    public static Card Parse(string value)
    {
        var split1 = value.Split(": ");
        var cardId = int.Parse(split1[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

        var split2 = split1[1].Split(" | ");
        var winningNumbers = GetNumbers(split2[0]);
        var numbers = GetNumbers(split2[1]);

        return new Card(cardId, winningNumbers.Intersect(numbers).ToList());
    }

    private static HashSet<int> GetNumbers(string value)
    {
        var split = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = new HashSet<int>(split.Length);
        foreach (var number in split)
        {
            result.Add(int.Parse(number));
        }

        return result;
    }
    
    public override string ToString() => $"Id: {Id}";
}