// See https://aka.ms/new-console-template for more information

for (int i = 1; i < 50; i++)
{
	Console.WriteLine($"Hello, World! {i}");

	await Task.Delay(5000);
}