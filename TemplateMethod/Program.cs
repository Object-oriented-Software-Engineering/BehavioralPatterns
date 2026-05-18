namespace TemplateMethod {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
        }
    }

    public abstract class TemplateMethod{

        public void HighLevelProcess() {
            MethodA(); // hook method, can be overridden by subclasses
            MethodB(); // hook method, can be overridden by subclasses
        }
        
        protected abstract void MethodA();
        protected abstract void MethodB();
        
    }
    
    public class ConcreteMethodA : TemplateMethod{
        protected override void MethodA() {
            Console.WriteLine("Concrete Method A");
        }

        protected override void MethodB() {
            Console.WriteLine("Concrete Method B");
        }
    }
    
    public class ConcreteMethodB : TemplateMethod{
        protected override void MethodA() {
            Console.WriteLine("Concrete Method B (overridden A)");
        }

        protected override void MethodB() {
            Console.WriteLine("Concrete Method B (overridden B)");
        }
    }
    
    
}
