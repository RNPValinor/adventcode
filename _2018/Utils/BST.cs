namespace _2018.Utils
{
    public class BST<T>
    {
        public Node<T> Root;
        
        public BST()
        {
            this.Root = new Node<T>();
        }
        
        public class Node<T>
        {
            public T Value { get; set; }
            
            private Node<T> Left { get; set; }
            
            private Node<T> Right { get; set; }

            public Node<T> GetLeft()
            {
                return this.Left ?? (this.Left = new Node<T>());
            }

            public Node<T> GetRight()
            {
                return this.Right ?? (this.Right = new Node<T>());
            }
        }
    }
}