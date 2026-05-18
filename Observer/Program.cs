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

    // Java equivalent: public interface ThermometerEventHandler { void handleTemperatureChange(float temperature); }'
    public interface IThermometerEventHandler {
        void HandleTemperatureChange(float temperature);
    }

    public class JavaThermometer{
        List<IThermometerEventHandler> m_ThermometerEventHandlerList;

        public JavaThermometer() {
            
        }

        public void SubScribeHandler(IThermometerEventHandler handler) {
            if (m_ThermometerEventHandlerList == null) {
                m_ThermometerEventHandlerList = new List<IThermometerEventHandler>();
            }
            m_ThermometerEventHandlerList.Add(handler);
        }
        
        // Unsubscribe a handler
        public void UnsubscribeHandler(IThermometerEventHandler handler) {
            if (m_ThermometerEventHandlerList != null && m_ThermometerEventHandlerList.Contains(handler)) {
                m_ThermometerEventHandlerList.Remove(handler);
            }
        }

        public void NotifyTemperatureChange(float temperature) {
            if (m_ThermometerEventHandlerList != null) {
                foreach (var handler in m_ThermometerEventHandlerList) {
                    handler.HandleTemperatureChange(temperature);
                }
            }
        }

        public void MeasureTemperature() {
            // Simulate temperature reading
            Random rand = new Random();
            float currentTemperature = (float)(rand.NextDouble() * 40.0); // Temperature between 0 and 40
            
            NotifyTemperatureChange(currentTemperature);
        }
    }

    // This class would be the observer in Java, implementing the IThermometerEventHandler interface
    public class JavaMachine : IThermometerEventHandler{
        public void HandleTemperatureChange(float temperature) {
            Console.WriteLine($"JavaMachine received temperature update: {temperature}°C");
            // Add logic to respond to temperature change
        }
    }


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
