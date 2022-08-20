namespace SpanCollections.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

public class SpanStackStruct
{
    [Fact]
    public void MiscTests()
    {
        var bytes = new byte[SpanStack<Guid>.MinNumBytes + Unsafe.SizeOf<Guid>() * 3];
        var one = Guid.NewGuid();
        var two = Guid.NewGuid();
        var three = Guid.NewGuid();
        var stack = new SpanStack<Guid>(bytes);
        Assert.Equal(0, stack.Count);
        Assert.Equal(3, stack.Capacity);
        Assert.False(stack.TryPeek(out _));
        stack.Push(in one);
        Assert.True(stack.TryPeek(out var peekOne));
        Assert.Equal(one, peekOne);
        Assert.Equal(1, stack.Count);
        stack.Push(in two);
        Assert.True(stack.TryPeek(out var peekTwo));
        Assert.Equal(two, peekTwo);
        Assert.Equal(2, stack.Count);
        stack.Push(in three);
        Assert.NotEqual(
            Enumerable.Repeat<byte>(0, bytes.Length),
            bytes
        );
        Assert.True(stack.TryPeek(out var peekThree));
        Assert.Equal(three, peekThree);
        Assert.Equal(3, stack.Count);
        Assert.True(stack.TryPop(out var popThree));
        Assert.Equal(three, popThree);
        Assert.Equal(2, stack.Count);
        Assert.True(stack.TryPop(out var popTwo));
        Assert.Equal(two, popTwo);
        Assert.Equal(1, stack.Count);
        Assert.True(stack.TryPop(out var popOne));
        Assert.Equal(one, popOne);
        Assert.Equal(0, stack.Count);
        Assert.False(stack.TryPeek(out _));
        Assert.False(stack.TryPop(out _));
        Assert.Equal(
            Enumerable.Repeat<byte>(0, bytes.Length),
            bytes
        );
    }

    public class GetEnumeratorMethodShould
    {
        [Fact]
        public void EnumerateInSameOrderAsFrameworkStack()
        {
            var bytes = new byte[6];
            var stack = new SpanStack<byte>(bytes);
            stack.Push(1);
            stack.Push(2);
            var reference = new Stack<byte>();
            reference.Push(1);
            reference.Push(2);
            using var referenceEnumerator = reference.GetEnumerator();
            foreach (ref readonly var actual in stack)
            {
                Assert.True(referenceEnumerator.MoveNext());
                Assert.Equal(referenceEnumerator.Current, actual);
            }
            Assert.False(referenceEnumerator.MoveNext());
        }

        [Fact]
        public void YieldReferences()
        {
            var bytes = new byte[5];
            var stack = new SpanStack<byte>(bytes);
            stack.Push(1);
            var enumerator = stack.GetEnumerator();
            Assert.True(enumerator.MoveNext());
            ref readonly var current = ref enumerator.Current;
            Assert.Equal(1, current);
            bytes[4] = 2;
            Assert.Equal(2, current);
        }
    }

    public class PushMethodShould
    {
        [Fact]
        public void WorkUntilFullAndThenThrowException()
        {
            var bytes = new byte[5];
            var stack = new SpanStack<byte>(bytes);
            stack.Push(1);
            bool threw;
            try
            {
                stack.Push(2);
                threw = false;
            }
            catch
            {
                threw = true;
            }
            Assert.True(threw);
        }
    }

    public class TryPopMethodShould
    {
        [Fact]
        public void Work()
        {
            var stack = new SpanStack<byte>(new byte[6]);
            Assert.False(stack.TryPop(out _));
            stack.Push(1);
            Assert.True(stack.TryPop(out var one));
            Assert.Equal(1, one);
            Assert.False(stack.TryPop(out _));
            stack.Push(2);
            stack.Push(3);
            Assert.True(stack.TryPop(out var three));
            Assert.Equal(3, three);
            Assert.True(stack.TryPop(out var two));
            Assert.Equal(2, two);
            Assert.False(stack.TryPop(out _));
        }
    }
}