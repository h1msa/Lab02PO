// See https://aka.ms/new-console-template for more information

namespace Lab02;

public class Program
{

    public static void Main()
    {
        Product soap = Product.Of(name: "Fa", price: 1.23m);
        Console.WriteLine(soap);
        Money m1 = Money.Of(value: 12.4m, Currency.EUR);
        Money m2 = Money.Of(value: 3.5m, Currency.EUR);
        Money result = m1 + m2;
        Console.WriteLine(result);
        if (m1 == m2)
        {
            Console.WriteLine("Równe");
        }
        else
        {
            Console.WriteLine("Różne");
        }
        if (m1 > m2)
        {
            Console.WriteLine("Większe");
        }
        else
        {
            Console.WriteLine("Nie większe");
        }

        List<Money> prices = new List<Money>()
        {
            m1, m2, Money.Of(5m, Currency.EUR)
        };
        prices.Sort();
        Console.WriteLine(string.Join(",", prices));
    }
     
}

public enum Currency
{
    USD = 1, EUR = 2, PLN = 3
}

public class Money: IEquatable<Money>, IComparable<Money>
{
    public decimal Value { get; init; }
    public Currency Currency { get; init; }

    private Money()
    {
        
    }
    // a + b => add(a, b) 
    public static Money operator +(Money a, Money b)
    {
        IsSameCurrencies(a, b);
        return Money.Of(value: a.Value + b.Value, a.Currency);
    }

    private static void IsSameCurrencies(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new ArgumentException(message: "Both currencies must be the same");
        }
    }

    public static bool operator ==(Money a, Money b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Money a, Money b)
    {
        return !(a == b);
    }

    public static bool operator <(Money a, Money b)
    {
        IsSameCurrencies(a, b);
        return a.Value < b.Value;
    }

    public static bool operator >(Money a, Money b)
    {
        IsSameCurrencies(a, b);
        return a.Value > b.Value;
    }

    public static Money Of(decimal value, Currency currency)
    {
        if (value < 0)
        {
            throw new ArgumentException(message: "Value must be positive or zero");
        }

        return new Money() { Value = value, Currency = currency };
    }

    public override string ToString()
    {
        return $"{nameof(Value)}: {Value}, {nameof(Currency)}: {Currency}";
    }

    public bool Equals(Money other)
    {
        return Value == other.Value && Currency == other.Currency;
    }
// 1 - other jest mniejsze od this
// 0 - other jest równe z this 
// -1 - other jest większe od this 
    public int CompareTo(Money? other)
    {
        if (other is null)
        {
            return 1;
        } 
        IsSameCurrencies(this, other);
        return decimal.Compare(this.Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Money)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, (int)Currency);
    }
}

public class Product
{
    public int Id { get; set; }
    private string _name = "";
    public string Name
    {
        get => _name;
       // po zdefiniowaniu metody of metoda init jest zbędna
       // bo nigdy nie wykonamy warunku bo już w trakcie tworzenia produktu to sprawdzamy
        init
        {
            if (value.Length > 1)
            {
                _name = value;
            }
            else
            {
                throw new ArgumentException(message:"Invalid name!");
            }
        }
    }
    public decimal Price { get; init; }

    private Product()
    {
        
    }

    public static Product Of(string name, decimal price)
    {
        if (name.Length < 2)
        {
            throw new ArgumentException(message: "Name cant be empty!");
        }

        if (price < 0)
        {
            throw new ArgumentException(message: "Price cant be nonpositive!");
        }

        return new Product() { Name = name, Price = price };
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Price)}: {Price}";
    }
}