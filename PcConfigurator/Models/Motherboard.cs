public class Motherboard
{
    public string ComponentType { get; set; }

    public string PartNumber { get; set; }

    public string Name { get; set; }

    public string Socket { get; set; }

    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"Type: {this.ComponentType}\n" +
            $"Part number: {this.PartNumber}\n" +
            $"Name: {this.Name}\n" +
            $"Socket: {this.Socket}," +
            $"Price: {this.Price}\n" +
            $"---------";
    }
}
