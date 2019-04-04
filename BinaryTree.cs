using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class BinaryTree<TNode> : IEnumerable<TNode> where TNode : IComparable
    {
        private  BinaryTree<TNode> _head;
                               
        public TNode Value { get; set; }
        public BinaryTree<TNode> Left { get; set; }
        public BinaryTree<TNode> Right { get; set; }
        
        public bool Contains(TNode tNode)
        {
            BinaryTree<TNode> parent;
            return FindWithParent(tNode, out parent) != null;
        }

        public int Count { get; private set; }

        private BinaryTree<TNode> FindWithParent(TNode value, out BinaryTree<TNode> parent)
        {
            var current = _head;
            parent = null;
 
            while (current != null)
            {
                var result = current.CompareTo(value);
                if (result > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else
                    break;
            }
 
            return current;
        }

        private int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }

        public void Add(TNode tNode)
        {
            if (_head != null)
                AddTo(_head, tNode);
            else
                _head = new BinaryTree<TNode> {Value = tNode, Count = 1};
            
            Count++;
        }

        private void AddTo(BinaryTree<TNode> node, TNode value)
        {
            while (true)
            {
                node.Count++;
                if (value.CompareTo(node.Value) < 0)
                {
                    if (node.Left == null)
                    {
                        node.Left = new BinaryTree<TNode> {Value = value, Count = 1};
                    }
                    else
                    {
                        node = node.Left;
                        continue;
                    }
                }
                else
                {
                    if (node.Right == null)
                    {
                        node.Right = new BinaryTree<TNode> {Value = value, Count = 1};
                    }
                    else
                    {
                        node = node.Right;
                        continue;
                    }
                }

                break;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<TNode> GetEnumerator()
        {
            return InOrderTraversal();
        }
         
        private IEnumerator<TNode> InOrderTraversal()
        {
            if (_head == null) yield break;
            var stack = new Stack();
            var current = _head;
            var goLeftNext = true;
                
            stack.Push(current);
 
            while (stack.Count > 0)
            {
                if (goLeftNext)
                {
                    while (current.Left != null)
                    {
                        stack.Push(current);
                        current = current.Left;
                    }
                }
 
                yield return current.Value;
 
                if (current.Right != null)
                {
                    current = current.Right;
                    goLeftNext = true;
                }
                else
                {
                    current = stack.Pop() as BinaryTree<TNode>;
                    goLeftNext = false;
                }
            }
        }

        public TNode this[int i]
        {
            get
            {
                if (i >= Count || i < 0) throw new ArgumentException();
                if (this == null) throw new NullReferenceException();
                var current = _head;
                while (true)
                {
                    var beforeCount = current?.Left?.Count ?? 0;
                    if (beforeCount == i )
                        return current == null ? default(TNode) : current.Value;

                    if (beforeCount < i)
                    {
                        i = i - beforeCount - 1;
                        current = current?.Right;
                    }
                    else
                        current = current?.Left;
                }
            }
        }
    }
}
