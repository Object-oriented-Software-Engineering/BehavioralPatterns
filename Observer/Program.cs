namespace Observer {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            
            Thermometer thermometer = new Thermometer();
            Machine machine = new Machine();

            // Subscribe the Machine's OnTemperatureChanged method to the Thermometer's event
            thermometer.m_ThermometerEventHandler += machine.OnTemperatureChanged;

            // Simulate temperature measurements
            for (int i = 0; i < 5; i++) {
                thermometer.MeasureTemperature();
                System.Threading.Thread.Sleep(500); // Wait for half a second
            }
        }
    }

    public delegate void ThermometerEventHandler(float temperature);

    public class Thermometer{
        
        public event ThermometerEventHandler m_ThermometerEventHandler;

        public void MeasureTemperature() {
            // Simulate temperature reading
            Random rand = new Random();
            float currentTemperature = (float)(rand.NextDouble() * 40.0); // Temperature between 0 and 40

            // Check if there are any subscribers to the event
            if (m_ThermometerEventHandler != null) {
                // Raise the event with the current temperature
                m_ThermometerEventHandler(currentTemperature);
            }
        }
    }
    
    public class Machine{
        public void OnTemperatureChanged(float temperature) {
            Console.WriteLine($"Machine received temperature update: {temperature}°C");
            // Add logic to respond to temperature change, e.g., adjust cooling system
        }
    }

}
