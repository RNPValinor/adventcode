namespace _2018.Utils
{
    public class Bst<T>
    {
        public readonly Node Root;
        
        public Bst()
        {
            this.Root = new Node();
        }
        
        public class Node
        {
            public T Value { get; set; }
            
            private Node Left { get; set; }
            
            private Node Right { get; set; }

            public Node GetLeft()
            {
                return this.Left ?? (this.Left = new Node());
            }

            public Node GetRight()
            {
                return this.Right ?? (this.Right = new Node());
            }
        }
    }
}