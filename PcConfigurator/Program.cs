using PcConfigurator.Models;
using System.Text;
using System.Text.Json;

//reads the information in the json file and gets it as text
var jsonText = await File.ReadAllTextAsync("json/pc-store-inventory.json");

//deserialized the json object
Data data = JsonSerializer.Deserialize<Data>(jsonText);

//takes input from user from console
Console.WriteLine("Please enter configuration(s):");
string[] input = Console.ReadLine().Split(", ").ToArray();

while (input[0] != "end")
{
    if (input[0].ToLower() == "list cpus")
    {
        //displays cpus components on the console
        Console.WriteLine("---CPUs---");
        Console.WriteLine(String.Join("\n", data.CPUs));
    }
    else if (input[0].ToLower() == "list motherboards")
    {
        //displays motherboards components on the console
        Console.WriteLine("---Motherboards---");
        Console.WriteLine(String.Join("\n", data.Motherboards));
    }
    else if (input[0].ToLower() == "list memory")
    {
        //displays memory components on the console
        Console.WriteLine("---Memory---");
        Console.WriteLine(String.Join("\n", data.Memory));
    }


    StringBuilder errors = new StringBuilder();

    if (input.Length == 0)
    {
        errors.AppendLine("ERROR: Please input components!");
    }
    else if(input.Length > 3)
    {
        errors.AppendLine("ERROR: Please input 3 or less componenets!");
    }

    //Checks to see how many components are entered
    //If there are 3 components checks if every one is valid
    if(input.Length == 3)
    {
        //Checks if cpu is valid
        if(data.CPUs.Any(c => c.PartNumber == input[0]))
        {
            var cpu = data.CPUs.FirstOrDefault(c => c.PartNumber == input[0]);

            //checks if motherboard is valid
            if(data.Motherboards.Any(m => m.PartNumber == input[1]))
            {
                var motherboard = data.Motherboards.FirstOrDefault(m => m.PartNumber == input[1]);

                //checks if memory is valid
                if(data.Memory.Any(m => m.PartNumber == input[2]))
                {
                    var memory = data.Memory.FirstOrDefault(m => m.PartNumber == input[2]);

                    //Checks if the configuration is valid if not prints the errors
                    string[] configErrors = Configuration.ValidateConfiguration(cpu, motherboard, memory);
                    if(configErrors.Length > 0)
                    {
                        errors.AppendLine("ERROR: Selected configuration is invalid.");
                        for (int i = 0; i < configErrors.Length; i++)
                        {
                            errors.AppendLine($"{i + 1}. {configErrors[i]}");
                        }
                    }
                    //if the configuration is valid it is printed on the console
                    else
                    {
                        var config = new Configuration(cpu, motherboard, memory);
                        Console.WriteLine($"The configuration is valid");
                        Console.WriteLine(config.ToString());
                    }
                }
                else
                {
                    errors.AppendLine("ERROR: Invalid memory part number!");
                }
            }
            else
            {
                errors.AppendLine("ERROR: Invalid motherboard part number!");
            }
        }
        else
        {
            errors.AppendLine("ERROR: Invalid cpu part number!");
        }
    }
    //if there are 2 components entered
    else if (input.Length == 2)
    {
        //checks if the components are valid
        if(data.CPUs.Any(c => c.PartNumber == input[0]))
        {
            var cpu = data.CPUs.First(c => c.PartNumber == input[0]);

            //checks which components is entered
            if (data.Motherboards.Any(c => c.PartNumber == input[1]))
            {
                var motherboard = data.Motherboards.First(c => c.PartNumber == input[1]);

                if(cpu.Socket == motherboard.Socket)
                {
                    var configs = Configuration.GetAllValidConfigurations(cpu, motherboard, data);

                    //prints all valid configurations on the console
                    Console.WriteLine($"There are {configs.Length} possible configurations:");
                    Console.WriteLine($"-------------");
                    Console.WriteLine(string.Join("\n", configs.ToList().OrderBy(c => c.TotalPrice)));
                }
                else
                {
                    errors.AppendLine($"ERROR: Motherboard is with {motherboard.Socket} socket and cpu is with socket {cpu.Socket}");
                }
            }
            else
            {
                if(data.Memory.Any(c => c.PartNumber == input[1]))
                {
                    var memory = data.Memory.First(c => c.PartNumber == input[1]);

                    if(cpu.SupportedMemory == memory.Type)
                    {
                        var configs = Configuration.GetAllValidConfigurations(cpu, memory, data);

                        //prints all valid configurations on the console
                        Console.WriteLine($"There are {configs.Length} possible configurations:");
                        Console.WriteLine($"-------------");
                        Console.WriteLine(string.Join("\n", configs.ToList().OrderBy(c => c.TotalPrice)));
                    }
                    else
                    {
                        errors.AppendLine($"ERROR: Cpu only supports {cpu.SupportedMemory} memory!");
                    }
                }
                else
                {
                    errors.AppendLine("ERROR: Invalid part number!");
                }
            }
        }
        else
        {
            errors.AppendLine("ERROR: Invalid cpu part number!");
        }
    }
    //if only 1 components is entered
    else if(input.Length == 1)
    {
        //validated if the cpu is exists
        if(data.CPUs.Any(c => c.PartNumber == input[0]))
        {
            var cpu = data.CPUs.First(c => c.PartNumber == input[0]);

            //gets all valid configurations for this cpu
            Configuration[] configs = Configuration.GetAllValidConfigurations(cpu, data);

            //prints all valid configurations on the console
            Console.WriteLine($"There are {configs.Length} possible configurations:");
            Console.WriteLine($"-------------");
            Console.WriteLine(string.Join("\n", configs.ToList().OrderBy(c => c.TotalPrice)));
        }
        else
        {
            errors.AppendLine("ERROR: Invalid cpu part number!");
        }
    }
   
    string errorsOutput = errors.ToString();
    
    //checks if there are any errors
    if(errorsOutput != "")
    {
        Console.WriteLine(errorsOutput);
    }

    //input for next rotation of the cycle
    Console.WriteLine("Please enter configuration(s):");
    input = Console.ReadLine().Split(", ").ToArray();
}