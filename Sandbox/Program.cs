using Simulation;
using Simulation.Agents;

var drone = new Drone(1, 5, 5, new List<Rule>()); // create a Drone object with initial x = 5, y = 5

// Show UI to set rules
while (true)
{
    Console.WriteLine("Enter condition template (e.g. 'X > 10'): ");
    var conditionTemplate = Console.ReadLine();

    Console.WriteLine("Enter action template (e.g. 'Y += 1'): ");
    var actionTemplate = Console.ReadLine();
    
    
    Console.WriteLine("Rule added successfully. Add another rule? (Y/N)");
    var addAnotherRule = Console.ReadLine();
    if (addAnotherRule?.ToUpper() != "Y")
    {
        break;
    }
}