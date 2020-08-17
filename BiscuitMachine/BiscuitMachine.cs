using System;
using System.Threading;
using BiscuitMachine.Services;

namespace BiscuitMachine
{
    public class BiscuitMachine
    {
        static void Main()
        {
            ConsoleKeyInfo KeyInfo;

            Console.WriteLine("Choose oven degrees (220 - 240 is good for biscuits)!");
            
            // Starting the machine
            int ovenTemperature = int.Parse(Console.ReadLine());

            Console.WriteLine("Wait until the machine is prepared!");
            var machineService = new MachineService();

            // Switching on the machine, heating the oven and starting the motor
            machineService.SwitchOnMachine(ovenTemperature);

            Console.WriteLine("Press 'S' to Start the Machine!");
            Console.WriteLine("Press 'E' to Switch off the Machine!");
            Console.WriteLine("Press 'P' to Pause the Machine!");

            do
            {
                KeyInfo = Console.ReadKey(true);

                if (KeyInfo.Key == ConsoleKey.S)
                {
                    Console.WriteLine("The machine is switched on!");

                    while (!Console.KeyAvailable)
                    {
                        machineService.MachineStartWorking(ovenTemperature);
                        Thread.Sleep(1000);
                        Console.WriteLine();
                    }
                }
                else if (KeyInfo.Key == ConsoleKey.E)
                {
                    Console.WriteLine("The machine is switched off!");

                    machineService.SwitchOffMachine();
                    Thread.Sleep(1000);
                    Console.WriteLine();
                }
                else if (KeyInfo.Key == ConsoleKey.P)
                {
                    Console.WriteLine("The machine is paused!");

                    machineService.PauseMachine();
                    Thread.Sleep(1000);
                    Console.WriteLine();
                }
            } while (KeyInfo.Key != ConsoleKey.E);

            Console.WriteLine("Exit!");
        }
    }
}
