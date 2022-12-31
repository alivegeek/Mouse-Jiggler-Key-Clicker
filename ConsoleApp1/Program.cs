using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace MouseKeyRandomizer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Set the time in minutes until the application should close
                int timeUntilClose = 60;

                Random random = new Random();
                char[] keys = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

                // Start the console application as a new process
                Process process = Process.Start("cmd.exe", "/c " + System.Reflection.Assembly.GetEntryAssembly().Location);

                // Set the console to treat Ctrl + C as input
                Console.TreatControlCAsInput = true;

                // Create a stopwatch to measure the elapsed time
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Create a Timer object that will close the application after the specified time
                System.Threading.Timer timer = new System.Threading.Timer(state =>
                {
                    Console.WriteLine("Closing application");
                    process.CloseMainWindow();
                }, null, timeUntilClose * 60 * 1000, Timeout.Infinite);

                Console.WriteLine("Timer started");

                // Handle the CancelKeyPress event to cancel the termination of the application
                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true;
                    Console.WriteLine("\nQuit the application using Ctrl + C");
                };

                while (true)
                {
                    // Generate a random interval between 30 seconds and 5 minutes
                    int interval = random.Next(30, 301) * 1000;

                    // Wait for the interval
                    Thread.Sleep(interval);

                    // Generate a random location on the screen
                    int x = random.Next(0, Screen.PrimaryScreen.Bounds.Width);
                    int y = random.Next(0, Screen.PrimaryScreen.Bounds.Height);

                    // Move the mouse to the random location
                    Cursor.Position = new System.Drawing.Point(x, y);
                    Console.WriteLine("Mouse moved to ({0}, {1})", x, y);

                    // Generate a random key to press
                    char key = keys[random.Next(0, keys.Length)];

                    // Send the keypress to the console application process
                    SendKeys.SendWait(key.ToString());
                    Console.WriteLine("Key {0} pressed", key);

                    // Calculate the time remaining and print it to the console output
                    int timeRemaining = (timeUntilClose * 60 * 1000 - (int)stopwatch.ElapsedMilliseconds) / 1000;
                    Console.WriteLine("Time remaining: {0} seconds", timeRemaining);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: {0}", ex.Message);
            }
        }
    }
}
