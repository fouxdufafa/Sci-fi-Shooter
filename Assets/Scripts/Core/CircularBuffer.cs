using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class CircularBuffer<T> : Queue<T>
{
    int _size;

    public CircularBuffer(int size) : base(size)
    {
        _size = size;
    }

    public void Push(T element)
    {
        this.Enqueue(element);
        this.TrimToCapacity();
    }

    public T Pop()
    {
        return this.Dequeue();
    }

    void TrimToCapacity()
    {
        while (this.Count > _size)
        {
            this.Dequeue();
        }
    }
}
