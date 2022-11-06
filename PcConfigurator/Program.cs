using System.Text.Json;

var jsonText = await File.ReadAllTextAsync("json/pc-store-inventory.json");
var json = JsonSerializer.Deserialize<Data>(jsonText);