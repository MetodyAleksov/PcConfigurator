using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcConfigurator.Models
{
    public class Configuration
    {
        //returns empty array if there are no errors otherwise all errors are returned as a string array
        public static string[] ValidateConfiguration(CPU cpu, Motherboard motherboard, Memory memory)
        {
            List<string> errors = new List<string>();

            if(cpu.Socket != motherboard.Socket)
            {
                errors.Add($"Motherboard socket is {motherboard.Socket} while cpu is with {cpu.Socket} socket!");
            }

            if(cpu.SupportedMemory != memory.Type)
            {
                errors.Add($"Memory of type {memory.Type} is not supported by this CPU it only support {cpu.SupportedMemory}!");
            }

            return errors.ToArray();
        }

        public static Configuration[] GetAllValidConfigurations(CPU cpu, Data data)
        {
            var compadMotherboards = data.Motherboards.Where(motherboard => motherboard.Socket == cpu.Socket).ToArray();
            var compadMemory = data.Memory.Where(memory => memory.Type == cpu.SupportedMemory);

            var allValidConfigurations = new List<Configuration>();

            foreach(var compadMotherboard in compadMotherboards)
            {
                foreach (var memory in compadMemory)
                {
                    var config = new Configuration(cpu, compadMotherboard, memory);
                    allValidConfigurations.Add(config);
                }
            }

            return allValidConfigurations.ToArray();
        }

        public static Configuration[] GetAllValidConfigurations(CPU cpu, Motherboard motherboard, Data data)
        {
            var compadMemory = data.Memory.Where(m => m.Type == cpu.SupportedMemory);

            var allValidConfigs = new List<Configuration>();

            foreach (var memory in compadMemory)
            {
                allValidConfigs.Add(new Configuration(cpu, motherboard, memory));
            }

            return allValidConfigs.ToArray();
        }

        public static Configuration[] GetAllValidConfigurations(CPU cpu, Memory memory, Data data)
        {
            var compadMotherboards = data.Motherboards.Where(m => m.Socket == cpu.Socket);

            var allValidConfigs = new List<Configuration>();

            foreach (var motherboard in compadMotherboards)
            {
                allValidConfigs.Add(new Configuration(cpu, motherboard, memory));
            }

            return allValidConfigs.ToArray();
        }

        public Configuration(CPU cpu, Motherboard motherboard, Memory memory)
        {
            this.CPU = cpu;
            this.Motherboard = motherboard;
            this.Memory = memory;
        }

        public CPU CPU { get; private set; }

        public Motherboard Motherboard { get; private set; }

        public Memory Memory { get; private set; }

        public decimal TotalPrice => Memory.Price + Motherboard.Price + CPU.Price;

        public override string ToString()
        {
            return $"CPU - {CPU.Name}\n" +
                $"Motherboard - {Motherboard.Name}\n" +
                $"Memory - {Memory.Name}\n" +
                $"Price: {TotalPrice}\n" +
                $"-------------";
        }
    }
}
